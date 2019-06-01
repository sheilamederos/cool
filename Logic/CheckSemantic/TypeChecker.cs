using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using AST.Types;

namespace Logic.CheckSemantic
{
    public class TypeCheckerVisitor : IVisitorAST<bool>
    {
        public TypeCheckerVisitor() { }

        public bool Visit(Node node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Program node)
        {
            foreach (Node cldr in node.children)
            {
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public bool Visit(Expr node)
        {
            foreach (Node cldr in node.children)
            {
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public bool Visit(BinaryExpr node)
        {
            if (!this.Visit(node.left) || !this.Visit(node.right) || node.left.type != node.right.type)
            {
                node.type = null;
                return false;
            }

            node.type = node.left.type;
            return true;

        }

        public bool Visit(UnaryExpr node)
        {
            if (!this.Visit(node.exp))
            {
                node.type = null;
                return false;
            }

            node.type = node.exp.type;
            return true;
        }

        public bool Visit(Assign node)
        {
            if (!this.Visit(node.exp))
            {
                node.type = null;
                return false;
            }

            node.type = node.exp.type;
            return true;
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
