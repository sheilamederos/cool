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
                    class Sheila {
                                edad : Bool <- true ;
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
