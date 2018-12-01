using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTableTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Delegate> FTable = new Dictionary<string,Delegate>();
            //有return value用 FUNC
            //沒return value用 ACTION
            //相當於VB裡面 Function與Sub的差別

            //指定的function好像必須是 static?
            FTable.Add("a", new Action<string>(FunctionA));
            FTable.Add("b", new Action<int>(FunctionB));
            
            FTable["b"].DynamicInvoke(123456);
            FTable["a"].DynamicInvoke("回櫻花國");

            Console.ReadLine();
        }


        public static void FunctionA(string data)
        {
            Console.WriteLine("FunctionA output : " + data);
        }
        public static void FunctionB(int data)
        {
            Console.WriteLine("[FunctionB] : " + data.ToString());
        }
    }
}
