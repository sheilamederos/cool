using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    public class TypeCheckerVisitor : IVisitorAST<IType>
    {
        ContextType Context;

        public string Logger;

        public TypeCheckerVisitor(ContextType cxt) { Context = cxt; Logger = ""; }

        public IType Visit(Node node)
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

        public IType Visit(Program node)
        {
            foreach (Class_Def cldr in node.list)
                this.Visit(cldr);

            return null;
        }

        public IType Visit(Expr node)
        {
            if (node is Call_Method) return this.Visit((Call_Method)node);
            if (node is Dispatch) return this.Visit((Dispatch)node);
            if (node is Str) return this.Visit((Str)node);
            if (node is Let_In) return this.Visit((Let_In)node);
            if (node is If_Else) return this.Visit((If_Else)node);
            if (node is While_loop) return this.Visit((While_loop)node);
            if (node is Body) return this.Visit((Body)node);
            if (node is New_type) return this.Visit((New_type)node);
            if (node is IsVoid) return this.Visit((IsVoid)node);
            if (node is BinaryExpr) return this.Visit((BinaryExpr)node);
            if (node is UnaryExpr) return this.Visit((UnaryExpr)node);
            if (node is Assign) return this.Visit((Assign)node);
            if (node is Id) return this.Visit((Id)node);
            else return this.Visit((Const)node);
        }

        public IType Visit(BinaryExpr node)
        {
            IType type_left = this.Visit(node.left);
            IType type_rigth = this.Visit(node.right);

            List<string> arith_op = new List<string> { "+", "-", "*", "/" };
            List<string> comp_op = new List<string> { "<", "<=" };
            List<string> basic_types = new List<string> { "Int", "Bool", "String", "Object" };

            // Es una operacion aritmetica o de comparacion
            if (arith_op.Contains(node.op) || comp_op.Contains(node.op))
            {
                if (type_left != null && type_left.Name != "Int")
                    Logger += "En la expresion " + node.ToString() + "-> error de tipos (La expresion izquierda no es Int) \n";
                if (type_rigth != null && type_rigth.Name != "Int")
                    Logger += "En la expresion " + node.ToString() + "-> error de tipos (La expresion derecha no es Int) \n";
                if (type_left == null || type_rigth == null) return null;

                if (type_left.Name != "Int" && type_rigth.Name != "Int")
                    Logger += "En la expresion " + node.ToString() + "-> error de tipos (Las expresiones no son Int) \n";

                if (arith_op.Contains(node.op)) return Context.GetType("Int");
                return Context.GetType("Bool");
            }

            // Es una operacion (=)

            if (type_left == null || type_rigth == null) return Context.GetType("Bool");

            bool is_built_in = basic_types.Contains(type_left.Name) || basic_types.Contains(type_rigth.Name);

            if (is_built_in && type_left.Name != type_rigth.Name)
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (Las expresiones no tienen el mismo tipo built-in) \n";
            }

            bool conform1 = type_left.Conform(type_rigth);
            bool conform2 = type_rigth.Conform(type_left);

            if (!is_built_in && !conform1 && !conform2)
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (Las expresiones no tienen tipos conformables)\n";
            }

            return Context.GetType("Bool");
        }
        
        public IType Visit(UnaryExpr node)
        {
            IType type_exp = this.Visit(node.exp);
            if (type_exp == null) return null;

            if(node.op == "~" && type_exp.Name != "Int")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (La expresion no es Int)\n";
                return null;
            }
            else if(node.op == "not" && type_exp.Name != "Bool")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (La expresion no es Bool)\n";
                return null;
            }

            return type_exp;
        }

        public IType Visit(Assign node)
        {
            IType type_exp = this.Visit(node.exp);
            IType type_id = Context.GetTypeFor(node.id.name);

            if (type_exp != null && type_id != null && !type_exp.Conform(type_id))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion no se conforma al del id)\n";
                return null;
            }

            return type_exp;
        }

        public IType Visit(Const node)
        {
            int num;

            if (int.TryParse(node.name, out num))
                return Context.GetType("Int");

            if (node.name == "true" || node.name == "false")
                return Context.GetType("Bool");

            Logger += "En la expresion " + node.ToString() + "-> error de tipos (La constante no es Int ni Bool)\n";
            return null; 
        }

        public IType Visit(Lista<Node> node)
        {
            throw new NotImplementedException();
        }

        public IType Visit(Class_Def node)
        {
            Context.ActualType = Context.GetType(node.type.s);
            Context.DefineSymbol("self", Context.ActualType);

            foreach (var attr in Context.GetType(node.inherit_type.s).AllAttributes())
                Context.DefineSymbol(attr.Name, attr.Type);

            foreach (var cldr in node.attr.list_Node)
            {
                IType t = this.Visit(cldr);
                Context.DefineSymbol(cldr.name.name, t);
            }
            foreach (var cldr in node.method.list_Node)
            {
                this.Visit(cldr);
            }

            foreach (var cldr in node.attr.list_Node)
            {
                Context.UndefineSymbol();
            }

            Context.UndefineSymbol();
            return null;
        }

        public IType Visit(Method_Def node)
        {
            foreach (var arg in node.args.list_Node)
                Context.DefineSymbol(arg.name.name, Context.GetType(arg.type.s));

            IType type_exp = this.Visit(node.exp);
            IType type_return = Context.GetType(node.type.s);

            if (type_exp != null && type_return != null && !type_exp.Conform(type_return))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion no se conforma al tipo de retorno) \n";
                return null;
            }

            foreach (var arg in node.args.list_Node)
                Context.UndefineSymbol();

            return type_return;
        }

        public IType Visit(Attr_Def node)
        {
            IType type_formal = Context.GetType(node.type.s);
            if (node.exp == null) return type_formal;
            IType type_exp = this.Visit(node.exp);

            if (type_exp != null && type_formal != null && !type_exp.Conform(type_formal))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion no se conforma al del atributo)\n";
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
                    Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion " + (i + 1).ToString() + " no se conforma al del argumento)\n";
                    return null;
                }
            }
            return Context.GetType(m.ReturnType.Name);
            
        }

        public IType Visit(Let_In node)
        {
            foreach (Attr_Def cld in node.attrs.list_Node)
            {
                this.Visit(cld);
                Context.DefineSymbol(cld.name.name, Context.GetType(cld.type.s));
            }

            IType type = this.Visit(node.exp);

            foreach (Attr_Def cld in node.attrs.list_Node)
                Context.UndefineSymbol();

            return type;
        }

        public IType Visit(If_Else node)
        {
            IType type_exp1 = this.Visit(node.cond);
            IType type_exp2 = this.Visit(node.then);
            IType type_exp3 = this.Visit(node.elsse);

            if (type_exp1 == null || type_exp2 == null || type_exp3 == null) return null;

            if (type_exp1.Name != "Bool")
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (La condicion no tiene tipo bool) \n";
                return null;
            }
            return Context.GetType(type_exp2.LCA(type_exp3).Name);
        }

        public IType Visit(While_loop node)
        {
            IType type_exp1 = this.Visit(node.exp1);
            
            this.Visit(node.exp2);

            if(type_exp1 == null || type_exp1.Name != "Bool")
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (La condicion no tiene tipo bool) \n";
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
            this.Visit(node.exp);
            return Context.GetType("Bool");
        }

        public IType Visit(Id node)
        {
            return Context.GetTypeFor(node.name);
        }

        public IType Visit(Dispatch node)
        {
            IType type_exp = this.Visit(node.exp);

            IType type = new IType("", null);
            if (node.s != "sin castear ")
                type = Context.GetType(node.s);
            else if (type_exp != null && Context.GetType(node.s) != null && !type_exp.Conform(Context.GetType(node.s)))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion no se conforma al del casteo) \n";
                return null;
            }
            else
            {
                type = type_exp;
            }

            Method m = type.GetMethod(node.call.name.name);
            for (int i = 0; i < node.call.args.list_Node.Count; i++)
            {
                var exp = node.call.args.list_Node[i];
                IType type_exp1 = this.Visit(exp);

                if (type_exp1 != null && m.Arguments[i].Type != null && !type_exp1.Conform(m.Arguments[i].Type))
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de tipos (El tipo de la expresion " + (i + 1).ToString() + " no se conforma al del argumento)\n";
                    return null;
                }
            }
            return Context.GetType(m.ReturnType.Name);
        }

        public IType Visit(Str node)
        {
            return Context.GetType("String");
        }
    }
}
