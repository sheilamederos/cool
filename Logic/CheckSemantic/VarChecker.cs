using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    public class VarCheckerVisitor : IVisitorAST<bool>
    {
        ContextType Context; //tipos, metodos, atributos

        public VarCheckerVisitor(ContextType context)
        {
            Context = context;
        }

        public bool Visit(Node node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Program node)
        {
            foreach (Node cld in node.children)
            {
                return true;
            }
            return false;
        }

        public bool Visit(Expr node)
        {
            foreach (Node cld in node.children)
            {
                return true;
            }
            return false;
        }

        public bool Visit(BinaryExpr node)
        {
            return this.Visit(node.left) && this.Visit(node.right);
        }

        public bool Visit(UnaryExpr node)
        {
            return this.Visit(node.exp);
        }

        public bool Visit(Assign node)
        {
            throw new NotImplementedException();
            //Context. node.id
        }

        public bool Visit(Const node)
        {
            return true;
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

        public bool Visit(Call_Method node)
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

        public bool Visit(Id node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Dispatch node)
        {
            throw new NotImplementedException();
        }
    }
}
