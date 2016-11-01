using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using System.Threading;

namespace AddressParse
{
    class Program
    {
        static void Main(string[] args)
        {
            AddressOperator.TestRedis();
            AddressOperator.iniRedis();
            object tmp = new object();
            Timer t = new Timer(new TimerCallback(AddressOperator.Parse), tmp, 0, 1000 * 60 * 60 * 24); 
            //AddressOperator.Parse();
            Console.Read();
        }
    }
}
