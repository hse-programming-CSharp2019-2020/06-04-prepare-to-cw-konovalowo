using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PrepareToCwLib;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace PrepareToCw
{
    class Program
    {
        const string streetfile = "data.txt";
        const string serializefile = "out.ser";
        const int maxHouseCount = 10;
        const int minHouseCount = 1;
        const int maxHouseNumber = 100;
        const int minHouseNumber = 1;

        public static Random rnd = new Random();

        public static int Input(string prompt)
        {
            int n;
            do
            {
                Console.Write(prompt);
            } while (!int.TryParse(Console.ReadLine(), out n) || n < 1);
            return n;
        }

        public static Street[] ReedStreetsFromFile(string filepath)
        {
            int lineNum = 0;
            List<Street> streets = new List<Street>();
            try
            {
                string line;
                using (var sr = new StreamReader(filepath))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] splitLine = line.Split();
                        string streetName = splitLine[0];
                        int[] houses = Array.ConvertAll(splitLine.Skip(1).ToArray(), s => int.Parse(s));
                        streets.Add(new Street { Name = streetName, Houses = houses });
                    }
                }
            }
            catch (IOException e)
            {
                Console.WriteLine($"IO exception while reading streets from file (line={lineNum}): {e.Message}");
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception while reading streets from file (line={lineNum}): {e.Message}");
                return null;
            }
            return streets.ToArray();
        }

        public static Street[] GenerateStreets(int n)
        {
            Street[] streetArray = new Street[n];
            return Array.ConvertAll(streetArray, s => new Street
            {
                Name = RandomString(5, 11),
                Houses = Array.ConvertAll(new int[rnd.Next(minHouseCount, maxHouseCount + 1)], c =>
                {
                    return rnd.Next(minHouseNumber, maxHouseNumber + 1);
                })
            });
        }

        public static string RandomString(int min, int max)
        {
            return new string(Array.ConvertAll(new int[rnd.Next(min, max)], c => (char)rnd.Next('a', 'z' + 1)));
        }

        static void Main(string[] args)
        {
            // Цикл повтора решения.
            do
            {
                int N = Input("Введите количетво улиц: ");
                Street[] streetArray = ReedStreetsFromFile(streetfile);
                if (streetArray == null)
                {
                    streetArray = new Street[N];
                    streetArray = GenerateStreets(N);
                }
                Array.ForEach(streetArray, Console.WriteLine);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Street[]));
                try
                {
                    using (var sw = new StreamWriter(serializefile))
                    {
                        xmlSerializer.Serialize(sw, streetArray);
                    }
                    Console.WriteLine($"Serialized array to {serializefile}");

                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception while serializing file: {e}");
                }


                Console.WriteLine("Для выхода нажмите ESC...");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
