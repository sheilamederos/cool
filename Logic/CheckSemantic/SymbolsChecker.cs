using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    //ver si estan definidos los tipos de las variables
    public class SymCheckerVisitor : IVisitorAST<bool>
    {
        ContextType Context; //tipos, metodos, atributos

        public SymCheckerVisitor(ContextType context)
        {
            Context = context;
        }

        public bool Visit(Node node)
        {
            if (node is Program) return this.Visit((Program)node);
            if (node is Expr) return this.Visit((Expr)node);
            if (node is Class_Def) return this.Visit((Class_Def)node);
            if (node is Method_Def) return this.Visit((Method_Def)node);
            if (node is Attr_Def) return this.Visit((Attr_Def)node);
            if (node is Formal) return this.Visit((Formal)node);
            if (node is Type_cool) return this.Visit((Type_cool)node);
            return this.Visit((Call_Method)node);
        }

        public bool Visit(Program node)
        {
            foreach (Class_Def cldr in node.list)
                if (!this.Visit(cldr)) return false;

            return true;
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

        public bool Visit(Str node)
        {
            throw new NotImplementedException();
        }
    }
}
