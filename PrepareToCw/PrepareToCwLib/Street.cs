using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrepareToCwLib
{
    [Serializable]
    public class Street
    {
        public string Name { get; set; }

        public int[] Houses { get; set; }

        public static int operator ~(Street street)
        {
            return street.Houses.Count();
        }

        public static bool operator !(Street street)
        {
            return street.Houses.Contains(7);
        }

        public override string ToString()
        {
            return $"{Name}, Houses: {string.Join(" ", Houses)}";
        }
    }
}
