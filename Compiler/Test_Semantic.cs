using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.CheckSemantic.Types;
using AST;
using Logic;
using Logic.CheckSemantic;

namespace Compiler
{
    class Test_Semantic
    {
        static void Main(string[] args)
        {
            string text = @" 
                    class Sheila inherits Persona {
                                edad : Int <- 34 ;
                                mujer : Bool <- true;
                                novio: Persona <- new Persona;
                                metodo1() : Int { {
                                        while i < 0 loop i <- i - 1 pool;
                                        i ;
                                } };

                                        };";


            AST.Program ast = (AST.Program) GetAST.Show(text);
            Program.DFS(ast);

            Dictionary<string, IType> types = IType.GetAllTypes(ast);
            ContextType context = new ContextType(types);
            var TypeCheck = new TypeCheckerVisitor(context);
            TypeCheck.Visit(ast);
            Console.WriteLine(TypeCheck.Logger);
        }
    }
}
