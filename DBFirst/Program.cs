using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testMySQLEF;
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
            List<users> coutry_list = BLL<users>.GetModelList(null);
            Console.WriteLine("Total:" + coutry_list.Count.ToString());
            foreach(users c in coutry_list)
            {
                Console.WriteLine("coutry:"+c.firstname+"; ");
            }
            Console.WriteLine();
        }
    }
}
