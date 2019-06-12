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

        public string Logger;

        public TypeCheckerVisitor(ContextType cxt) { Context = cxt; }

        public IType Visit(Node node)
        {
            throw new NotImplementedException();
        }

        public IType Visit(Program node)
        {
            foreach (Class_Def cldr in node.list)
            {
                Context.ActualType = Context.GetType(cldr.type.s);
                this.Visit(cldr);
            }
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
                    Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos Int \n";
                    return null;
                }
                else return type_rigth;
            }

            if(comp_op.Contains(node.op))
            {
                if (type_left.Name != "Int" || type_rigth.Name != "Int")
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (Int) \n";
                    return null;
                }
                else return Context.GetType("Bool");
            }

            if (!basic_types.Contains(type_left.Name) || !basic_types.Contains(type_rigth.Name))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (NO TIPOS BUILT-IN) \n";
                return null;
            }

            if(type_left.Name != type_rigth.Name)
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (" + type_left.Name + " y " + type_rigth.Name + ")\n";
                return null;
            }

            return type_left;

        }
        
        public IType Visit(UnaryExpr node)
        {
            IType type_exp = this.Visit(node.exp);

            if(node.op == "~" && type_exp.Name != "Int")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (Int)\n";
                return null;
            }

            else if(type_exp.Name != "Bool")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (Bool)\n";
                return null;
            }

            return type_exp;
        }

        public IType Visit(Assign node)
        {
            IType type_exp = this.Visit(node.exp);
            IType type_id = Context.GetTypeFor(node.id.name);

            if (type_exp != null && type_exp != null && !type_exp.Conform(type_id))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de conformidad de tipos\n";
                return null;
            }

            return type_exp;
        }

        public IType Visit(Const node)
        {
            int num;

            if (int.TryParse(node.name, out num))
                return Context.GetType("Int");

            if (node.type.s == "true" || node.type.s == "false")
                return Context.GetType("Bool");

            Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (no Int, no Bool)\n";
            return null; 
        }

        public IType Visit(Lista<Node> node)
        {
            throw new NotImplementedException();
        }

        public IType Visit(Class_Def node)
        {
            foreach (var cldr in node.children)
                this.Visit(cldr);
            return null;
        }

        public IType Visit(Method_Def node)
        {
            IType type_exp = this.Visit(node.exp);
            IType type_return = Context.GetType(node.type.s);

            if (type_exp != null && type_return != null && !type_exp.Conform(type_return))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de conformidad de tipos \n";
                return null;
            }

            return type_return;
        }

        public IType Visit(Attr_Def node)
        {
            IType type_exp = this.Visit(node.exp);
            IType type_formal = Context.GetType(node.type.s);

            if (type_exp != null && type_formal != null && !type_exp.Conform(type_formal))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de conformidad de tipos \n";
                return null;
            }

            return type_exp;
        }

        public IType Visit(Formal node)
        {
            return Context.GetType(node.type.s);
        }

        public IType Visit(Type_cool node)
        {
            return Context.GetType(node.s);
        }
        
        public IType Visit(Call_Method node)
        {
            Method m = Context.ActualType.GetMethod(node.name.name);
            for (int i = 0; i < node.args.list_Node.Count; i++)
            {
                var exp = node.args.list_Node[i];
                IType type = this.Visit(exp);

                if (type != null && m.Arguments[i].Type != null && !type.Conform(m.Arguments[i].Type))
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de conformidad de tipos en el argumento " + (i + 1).ToString() + "\n";
                    return null;
                }
            }
            return Context.GetType(m.ReturnType.Name);
            
        }

        public IType Visit(Let_In node)
        {
            foreach (Node cld in node.attrs.list_Node)
                this.Visit(cld);

            return this.Visit(node.exp);
        }

        public IType Visit(If_Else node)
        {
            IType type_exp1 = this.Visit(node.exp1);
            IType type_exp2 = this.Visit(node.exp2);
            IType type_exp3 = this.Visit(node.exp3);

            if (type_exp1.Name != "Bool")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (Bool) en la condicion \n";
                return null;
            }
            return Context.GetType(type_exp2.LCA(type_exp3).Name);
        }

        public IType Visit(While_loop node)
        {
            IType type_exp1 = this.Visit(node.exp1);
            this.Visit(node.exp2);
            if(type_exp1.Name != "Bool")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de compatibilidad de tipos (Bool) en la condicion \n";
                return null;
            }
            return Context.GetType("Object");
        }

        public IType Visit(Body node)
        {
            IType type = new IType("", null);
            foreach (var exp in node.list.list_Node)
                type = this.Visit(exp);

            return type;
        }

        public IType Visit(New_type node)
        {
            return Context.GetType(node.type.s);
        }

        public IType Visit(IsVoid node)
        {
            return Context.GetType("Bool");
        }

        public IType Visit(Id node)
        {
            return Context.GetTypeFor(node.name);
        }

        public IType Visit(Dispatch node)
        {
            IType type_exp = this.Visit(node.exp);
            if(type_exp != null && Context.GetType(node.s) != null && !type_exp.Conform(Context.GetType(node.s)))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de conformidad de tipos en la expresion \n";
                return null;
            }
            return this.Visit(node.call);
        }

        public IType Visit(Str node)
        {
            return Context.GetType("String");
        }
    }
}
