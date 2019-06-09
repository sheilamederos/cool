using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic.CheckSemantic.Types;

namespace Logic.CheckSemantic
{
    public class TypeCheckerVisitor : IVisitorAST<bool>
    {
        ContextType Context;

        public TypeCheckerVisitor(ContextType cxt) { Context = cxt; }

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
                if (!this.Visit(cldr))
                {
                    node.type = null;
                    return false;
                }
            }
            node.type = ((Expr)node.children[node.children.Count() - 1]).type;;
            return true;
        }

        public bool Visit(BinaryExpr node)
        {
            if(!this.Visit(node.left) || !this.Visit(node.right) || node.left.type.s != node.right.type.s)
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
            bool v_exp = this.Visit(node.exp);
            IType type_exp = Context.GetType(node.exp.type.s);
            IType type_id = Context.GetTypeFor(node.id.name);

            if (!v_exp || !type_exp.Conform(type_id)) return false;

            return true;
        }

        public bool Visit(Const node)
        {
            int num;
            if ((node.type.s == "Int" && !int.TryParse(node.type.s, out num)) ||
                (node.type.s == "Bool" && node.type.s != "true" && node.type.s != "false"))
                return false;
           
            return true;
        }

        public bool Visit(Lista<Node> node)
        {
            foreach (Node cldr in node.children)
            {
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public bool Visit(Class_Def node)
        {
            foreach (Node cldr in node.children)
            {
                Context.ActualType = Context.GetType(node.type.s);
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public bool Visit(Method_Def node)
        {
            foreach (Node cldr in node.children)
            {
                if (!this.Visit(cldr)) return false;
            }
            IType type_exp = Context.GetType(node.exp.type.s);
            IType type_return = Context.GetType(node.type.s);
            if (!type_exp.Conform(type_return)) return false;
            return true;
        }

        public bool Visit(Attr_Def node)
        {
            if (!this.Visit(node.exp)) return false;

            IType type_formal = Context.GetType(node.type.s);
            IType type_exp = Context.GetType(node.exp.type.s);
            if (!type_exp.Conform(type_formal)) return false;

            return true;
        }

        public bool Visit(Formal node)
        {
            return true;
        }

        public bool Visit(Type_cool node)
        {
            return true;
        }
        
        public bool Visit(Call_Method node)
        {
            foreach (Expr exp in node.args.list_Node)
                if (!this.Visit(exp))
                {
                    node.type = null;
                    return false;
                }
            node.type = new Type_cool(Context.ActualType.GetMethod(node.name.name).ReturnType.Name);
            return true;
        }

        public bool Visit(Let_In node)
        {
            foreach (Node cld in node.attrs.list_Node)
                if (!this.Visit(cld))
                {
                    node.type = null;
                    return false;
                }
            if (!this.Visit(node.exp))
            {
                node.type = null;
                return false;
            }
            node.type = node.exp.type;
            return true;
        }

        public bool Visit(If_Else node)
        {
            if(!this.Visit(node.exp1) || !this.Visit(node.exp2) || !this.Visit(node.exp3))
            {
                node.type = null;
                return false;
            }
            IType type_exp1 = Context.GetType(node.exp1.type.s);
            IType type_exp2 = Context.GetType(node.exp2.type.s);
            IType type_exp3 = Context.GetType(node.exp3.type.s);

            if(type_exp1.Name != "Bool")
            {
                node.type = null;
                return false;
            }
            node.type = new Type_cool(type_exp2.LCA(type_exp3).Name);
            return true;
        }

        public bool Visit(While_loop node)
        {
            if (!this.Visit(node.exp1) || !this.Visit(node.exp2))
            {
                node.type = null;
                return false;
            }
            node.type = new Type_cool("Object");
            return true;
        }

        public bool Visit(Body node)
        {
            if (!this.Visit(node.list))
            {
                node.type = null;
                return true;
            }

            node.type = node.list.list_Node[node.list.list_Node.Count() - 1].type;
            return true;
        }

        public bool Visit(New_type node)
        {
            return true;
        }

        public bool Visit(IsVoid node)
        {
            return true;
        }

        public bool Visit(Id node)
        { 
            return true;
        }
    }
}
