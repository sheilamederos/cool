using System;
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

            //Debug_Semantic_Files_Success();

            AST.Program ast = (AST.Program)GetAST.Show(text);
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

        public static void Debug_Semantic_Files_Success()
        {
            string success_path = "../../../../cooltestcases/Semantics/success";
            var dir = new DirectoryInfo(success_path);
            int count = 0;
            int total = 0;
            foreach (var item in dir.GetFiles())
            {
                if (item.Extension == ".cl")
                {
                    var f = new StreamReader(item.FullName);

                    string text = f.ReadToEnd();

                    try
                    {
                        var solve = Debug_Semantic(text);
                        if (solve.Item1 && solve.Item2 && solve.Item3)
                        {

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("OK: ");
                            count++;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write("Wrong: {0} {1} {2}  ", solve.Item1, solve.Item2, solve.Item3);
                        }
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Broken: ");
                    }

                    Console.WriteLine(item.Name);
                    Console.ResetColor();
                    total++;
                }
            }
            Console.WriteLine($"A: {count}, T: {total}, Acc: {(double)(1.0 * count / total) * 100.0}");
        }

        private static Tuple<bool, bool, bool> Debug_Semantic(string text)
        {
            AST.Program ast = (AST.Program)GetAST.Show(text);
            //Program.DFS(ast);

            bool check_def = false;
            bool check_types = false;
            bool check_sym = false;

            var DefChecker = new DefinitionsChecker();
            check_def = DefChecker.Visit(ast);
            if (check_def)
            {
                Dictionary<string, IType> types = IType.GetAllTypes(ast);
                ContextType context = new ContextType(types);
                var SymChecker = new SymCheckerVisitor(context);
                check_sym = SymChecker.Visit(ast);

                if (check_sym)
                {
                    context = new ContextType(types);
                    var TypeCheck = new TypeCheckerVisitor(context);
                    TypeCheck.Visit(ast);
                    check_types = TypeCheck.Logger == "";
                }               
            }

            return new Tuple<bool, bool, bool>(check_def, check_sym, check_types);
        }
    }
}
