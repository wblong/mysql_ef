using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            GetCountryList();
            Console.ReadKey();
        }
        private static void GetCountryList()
        {
            List<country> coutry_list = BLL<country>.GetModelList(e => e.Population > 10000);
            Console.WriteLine("Total:" + coutry_list.Count.ToString());
            foreach(country c in coutry_list)
            {
                Console.WriteLine("coutry:"+c.Name+"; ");
            }
            Console.WriteLine();
        }
    }
}
