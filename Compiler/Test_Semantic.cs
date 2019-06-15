﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic;
using Logic.CheckSemantic;
using System.IO;

namespace Compiler
{
    class Test_Semantic
    {
        static void Main(string[] args)
        {
            StreamReader x = new StreamReader("a.txt");
            string text = x.ReadToEnd();
            


            AST.Program ast = (AST.Program) GetAST.Show(text);
            Program.DFS(ast);

            var DefChecker = new DefinitionsChecker();
            bool check = DefChecker.Visit(ast);
            Console.WriteLine(DefChecker.Logger);
            if (check)
            {
                Console.WriteLine("Definiciones OK");
                Dictionary<string, IType> types = IType.GetAllTypes(ast);
                ContextType context = new ContextType(types);
                var SymChecker = new SymCheckerVisitor(context);
                bool check_sym = SymChecker.Visit(ast);
                Console.WriteLine(SymChecker.Logger);

                if (check_sym)
                {
                    Console.WriteLine("Simbolos OK");
                    context = new ContextType(types);
                    var TypeCheck = new TypeCheckerVisitor(context);
                    TypeCheck.Visit(ast);
                    Console.WriteLine(TypeCheck.Logger);
                }
                else Console.WriteLine("Simbolos al berro");
            }
            else Console.WriteLine("Definiciones al berro");
        }
    }
}
