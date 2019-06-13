using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using Logic.CheckSemantic.Types;

namespace Logic.CheckSemantic
{
    public class SymCheckerVisitor : IVisitorAST<bool>
    {
        ContextType Context; 

        public string Logger;

        public SymCheckerVisitor(ContextType context)
        {
            Context = context;
            Logger = "";
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
            Context.DefineSymbol("self", Context.GetSelf_Type());
            foreach (Class_Def cldr in node.list)
                if (!this.Visit(cldr)) return false;

            return true;
        }

        public bool Visit(Expr node)
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
            bool solve = true;
            if (!Context.IsDefineSymbol(node.id.name))
            {
                solve = false;
                Logger += "En la expresion " + node.ToString() + "-> error de identificador ('" + node.id.name +"' no esta definido) \n";
            }
            return solve && this.Visit(node.exp);
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
            bool solve = true; 
            Context.ActualType = Context.GetType(node.type.s);
            List<string> id_defines = new List<string>();
            foreach (var attr in node.attr.list_Node)
            {
                if (id_defines.Contains(attr.name.name) || Context.ActualType.Father.GetAttribute(attr.name.name) != null)
                {
                    solve = false;
                    Logger += "En la expresion " + node.ToString() + "-> error de identificador ('" + attr.name.name + "' ya esta definido) \n";
                }
                else
                {
                    id_defines.Add(attr.name.name);
                    Context.DefineSymbol(attr.name.name, Context.GetType(attr.type.s));
                }
            }
            foreach (var mtd in node.method.list_Node)
            {
                if (Context.ThereAreMethod(mtd.name.name))
                {
                    solve = false;
                    Logger += "En la expresion " + node.ToString() + "-> error de identificador (metodo '" + mtd.name.name + "' ya esta definido) \n";
                }
                else
                {
                    Context.DefineMethod(mtd.name.name, Context.GetType(node.type.s));
                }
                Method mtd_father = Context.ActualType.Father.GetMethod(mtd.name.name);
                if (mtd_father != null && !Method.Equal_Def(mtd, mtd_father))
                {
                    solve = false;
                    Logger += "En la expresion " + node.ToString() + "-> error de identificador (metodo '" + mtd.name.name + "' esta definido con elementos diferentes en un tipo mayor) \n";
                }
            }
            foreach (var cld in node.attr.list_Node)
                if (!this.Visit(cld)) solve = false;

            foreach (var cld in node.method.list_Node)
                if (!this.Visit(cld)) solve = false;

            Context.UndefineSymbol(id_defines.Count);

            Context.UndefineMethods();

            return solve;
        }

        public bool Visit(Method_Def node)
        {
            bool solve = true;
            List<string> id_defines = new List<string>();
            foreach (var arg in node.args.list_Node)
            {
                if (id_defines.Contains(arg.name.name))
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de identificador ('" + arg.name.name + "' ya esta definido) \n";
                    solve = false;
                }
                else
                {
                    id_defines.Add(arg.name.name);
                    Context.DefineSymbol(arg.name.name, Context.GetType(arg.type.s));
                }
            }

            solve &= this.Visit(node.exp);

            Context.UndefineSymbol(id_defines.Count);

            return solve;
        }

        public bool Visit(Attr_Def node)
        {
            if (node == null) return true;
            return this.Visit(node.exp);
        }

        public bool Visit(Formal node)
        {
            return true;
        }

        public bool Visit(Type_cool node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Call_Method node)
        {
            bool solve = true;
            foreach (var arg in node.args.list_Node)
            {
                solve &= this.Visit(arg);
            }
            if (!Context.IsDefineMethod(node.name.name, Context.ActualType))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de identificador (metodo '" + node.name.name + "' no esta definido) \n";
                solve = false;
            }
            return solve;
        }

        public bool Visit(Let_In node)
        {
            bool solve = true;
            List<string> id_defines = new List<string>();
            foreach (var attr in node.attrs.list_Node)
            {
                if (id_defines.Contains(attr.name.name))
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de identificador ('" + attr.name.name + "' ya esta definido) \n";
                    solve = false;
                }
                else
                {
                    id_defines.Add(attr.name.name);
                    Context.DefineSymbol(attr.name.name, Context.GetType(attr.type.s));
                }
            }

            solve &= this.Visit(node.exp);

            Context.UndefineSymbol(id_defines.Count);

            return solve;
        }

        public bool Visit(If_Else node)
        {
            return this.Visit(node.exp1) && this.Visit(node.exp2) && this.Visit(node.exp3);
        }

        public bool Visit(While_loop node)
        {
            return this.Visit(node.exp1) && this.Visit(node.exp2);
        }

        public bool Visit(Body node)
        {
            foreach (var exp in node.list.list_Node)
                if (!this.Visit(exp)) return false;

            return true;
        }

        public bool Visit(New_type node)
        {
            return true;
        }

        public bool Visit(IsVoid node)
        {
            return this.Visit(node.exp);
        }

        public bool Visit(Id node)
        {
            if (!Context.IsDefineSymbol(node.name))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de identificador ('" + node.name + "' no esta definido) \n";
                return false;
            }
            return true;
        }

        public bool Visit(Dispatch node)
        {
            return this.Visit(node.exp) && this.Visit(node.call);
        }

        public bool Visit(Str node)
        {
            return true;
        }
    }
}
