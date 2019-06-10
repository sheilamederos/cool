using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic;
using System.IO;

namespace Compiler
{
    class Program
    {
        public static void DFS(Node a, int level = 0)
        {
            if (a == null) return;
            for (int i = 0; i < level; i++) Console.Write("-");
            Console.Write(":");

            Console.WriteLine("< {0} {1}", a.GetType().ToString(), a.ToString());

            foreach (var item in a.children)
            {
                DFS(item, level + 3);
            }

            for (int i = 0; i < level; i++) Console.Write("-");
            Console.Write(":");

            Console.WriteLine("> {0} ", a.ToString());

        }
        static void Main1(string[] args)
        {
            StreamReader x = new StreamReader("a.txt");
            string adsa = x.ReadToEnd();
            Console.WriteLine(adsa);
            DFS(GetAST.Show(adsa));
           
        }
    }
}
