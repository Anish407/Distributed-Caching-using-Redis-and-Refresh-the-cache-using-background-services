using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            dynamic stuff = JsonConvert.DeserializeObject<dynamic>("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");

            string name = stuff.Name;
            string address = stuff.Address.City;

            foreach (JProperty jproperty in stuff)
            {
                Console.WriteLine("jproperty.Name = {0}", jproperty.Name);
            }

            dynamic d = stuff.Address;
            foreach (JProperty jproperty in d)
            {
                Console.WriteLine("d.Name = {0}", jproperty.Name);
            }

            Console.WriteLine("Hello World!");
        }
    }
}
