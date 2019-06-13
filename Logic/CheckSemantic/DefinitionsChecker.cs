using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic.CheckSemantic.Types;

namespace Logic.CheckSemantic
{
    //clases definidas solo una vez 
    //todas los tipos estan definidos
    //palabras reservadas no son usadas como identificadores
    public class DefinitionsChecker:IVisitorAST<bool>
    {
        public List<string> Types;

        public string Logger;

        public DefinitionsChecker()
        {
            Types = new List<string>();
            Logger = "";
        }

        public bool Visit(Node node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Program node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Lista<Node> node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Class_Def node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Method_Def node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Attr_Def node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Formal node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Type_cool node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Expr node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Str node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Call_Method node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Dispatch node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Let_In node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(If_Else node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(While_loop node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Body node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(New_type node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(IsVoid node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(BinaryExpr node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(UnaryExpr node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Assign node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Id node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Const node)
        {
            throw new NotImplementedException();
        }
    }
}
