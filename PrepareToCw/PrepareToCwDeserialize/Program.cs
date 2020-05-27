using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PrepareToCwLib;

namespace PrepareToCwDeserialize
{
    class Program
    {
        const string deserializefile = "../../../PrepareToCw/bin/Debug/out.ser";

        static void Main(string[] args)
        {
            // Цикл повтора решения.
            do
            {
                Street[] streetArray = null;
                var xmlDeserializer = new XmlSerializer(typeof(Street[]));
                try
                {
                    using (var sr = new StreamReader(deserializefile))
                    {
                        streetArray = (Street[])xmlDeserializer.Deserialize(sr);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception while deserializing file: {e}");
                }

                if (streetArray != null)
                {
                    var magicalStreets = from street in streetArray
                                         where ~street % 2 != 0 && !street
                                         select street;
                    Array.ForEach(magicalStreets.ToArray(), Console.WriteLine);
                }

                Console.WriteLine("Для выхода нажмите ESC...");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
        }
    }
}
