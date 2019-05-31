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
            int elem = 0;
            if (int.TryParse(node.name, out elem))
                node.type = "int";

            else if (node.name == "true" || node.name == "false")
                node.type = "bool";

            else
                node.type = "string";
            return true;
        }
    }
}
