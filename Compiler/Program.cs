using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic;

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
                DFS(item, level + 1);
            }

            for (int i = 0; i < level; i++) Console.Write("-");
            Console.Write(":");

            Console.WriteLine("> {0} ", a.ToString());

        }
        static void Main(string[] args)
        {
            string text = @" 
                    class Sheila inherits Persona
                    {
                        Edad : int <- 22;
                        Mujer : bool <- true;
                        Novio : Persona <- new Persona;
                        i : INT <- 10;

                        metodo1() : int {{while i > 0 loop i <- i - 1 pool; i; }};

                        metodo2() : int { let a : int <- 3, b : int <- 5 in a * b; };

                        metodo4() : bool { not true; };
                    }; ";
            DFS(GetAST.Show(text));
            /* Edad <- 22;
                        Mujer <- true;
                        Novio: Persona <- new Persona;

                        i <- 10;
                        metodo1() : int {{while i > 0 loop i <- i - 1 pool; i; }};

                        metodo2() : int{ let a : int <- 3, b : int <- 5 in a * b; };


                        metodo4() : bool { not true; };*/
        }
    }
}
