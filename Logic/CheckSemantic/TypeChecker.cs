using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic.CheckSemantic.Types;

namespace Logic.CheckSemantic
{
    public class TypeCheckerVisitor : IVisitorAST<IType>
    {
        ContextType Context;

        string Logger;

        public TypeCheckerVisitor(ContextType cxt) { Context = cxt; }

        public IType Visit(Node node)
        {
            throw new NotImplementedException();
        }

        public IType Visit(Program node)
        {
            foreach (Node cldr in node.children)
                this.Visit(cldr);
            return null;
        }

        public IType Visit(Expr node)
        {
            throw new NotImplementedException();
        }

        public IType Visit(BinaryExpr node)
        {
            IType type_left = this.Visit(node.left);
            IType type_rigth = this.Visit(node.right);

            List<string> arith_op = new List<string> { "+", "-", "*", "/" };
            List<string> comp_op = new List<string> { "<", "<=" };
            List<string> basic_types = new List<string> { "Int", "Bool", "String" };

            if (arith_op.Contains(node.op))
            {
                if (type_left.Name != "Int" || type_rigth.Name != "Int")
                {
                    Logger += "\n Error ";
                    return null;
                }
                else return Context.GetType("Int");
            }

            if(comp_op.Contains(node.op))
            {
                if (type_left.Name != "Int" || type_rigth.Name != "Int")
                    return null;
                else return Context.GetType("Bool");
            }

            if(!basic_types.Contains(type_left.Name) || !basic_types.Contains(type_rigth.Name))
                return null;

            return type_left;

        }
        
        public IType Visit(UnaryExpr node)
        {
            return this.Visit(node.exp);
            
        }

        public IType Visit(Assign node)
        {
            bool v_exp = this.Visit(node.exp);
            IType type_exp = Context.GetType(node.exp.type.s);
            IType type_id = Context.GetTypeFor(node.id.name);

            if (!v_exp || !type_exp.Conform(type_id)) return false;

            return true;
        }

        public IType Visit(Const node)
        {
            int num;
            if ((node.type.s == "Int" && !int.TryParse(node.type.s, out num)) ||
                (node.type.s == "Bool" && node.type.s != "true" && node.type.s != "false"))
                return false;
           
            return true;
        }

        public IType Visit(Lista<Node> node)
        {
            foreach (Node cldr in node.children)
            {
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public IType Visit(Class_Def node)
        {
            foreach (Node cldr in node.children)
            {
                Context.ActualType = Context.GetType(node.type.s);
                if (!this.Visit(cldr)) return false;
            }
            return true;
        }

        public IType Visit(Method_Def node)
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

        public IType Visit(Attr_Def node)
        {
            if (!this.Visit(node.exp)) return false;

            IType type_formal = Context.GetType(node.type.s);
            IType type_exp = Context.GetType(node.exp.type.s);
            if (!type_exp.Conform(type_formal)) return false;

            return true;
        }

        public IType Visit(Formal node)
        {
            return true;
        }

        public IType Visit(Type_cool node)
        {
            return true;
        }
        
        public IType Visit(Call_Method node)
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

        public IType Visit(Let_In node)
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

        public IType Visit(If_Else node)
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

        public IType Visit(While_loop node)
        {
            if (!this.Visit(node.exp1) || !this.Visit(node.exp2))
            {
                node.type = null;
                return false;
            }
            node.type = new Type_cool("Object");
            return true;
        }

        public IType Visit(Body node)
        {
            if (!this.Visit(node.list))
            {
                node.type = null;
                return true;
            }

            node.type = node.list.list_Node[node.list.list_Node.Count() - 1].type;
            return true;
        }

        public IType Visit(New_type node)
        {
            return true;
        }

        public IType Visit(IsVoid node)
        {
            return true;
        }

        public IType Visit(Id node)
        { 
            return true;
        }

        public IType Visit(Dispatch node)
        {
            throw new NotImplementedException();
            //bool v_call_method = this.Visit(node.call);
            //bool v_exp = this.Visit(node.exp);

//            bool conf = Context.GetType(node.exp.type.s).Conform(Context.GetType(node.caster.s))
            
        }

        public IType Visit(Str node)
        {
            throw new NotImplementedException();
        }
    }
}
