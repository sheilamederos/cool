using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    //clases definidas solo una vez (OK)
    //todos los tipos estan definidos (OK)
    //palabras reservadas no son usadas como identificadores (OK)
    //no se puede heredar de built-in (OK)
    public class DefinitionsChecker:IVisitorAST<bool>
    {
        public List<string> Types;

        public string Logger;

        public List<string> ReservesWords;

        public List<string> Types_built_in;

        public DefinitionsChecker()
        {
            Types = new List<string> { "String", "SELF_TYPE", "Bool", "Int", "IO", "Object" };
            Logger = "";
            ReservesWords = new List<string> { "class", "while", "loop", "pool", "if", "then", "else", "fi", "case", "of", "let", "in", "true", "false" };
            Types_built_in = new List<string> { "String", "SELF_TYPE", "Bool", "Int" };
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
            bool solve = true;
            foreach (Class_Def cldr in node.list)
            {
                if (Types.Contains(cldr.type.s))
                {
                    Logger += "En la expresion " + node.ToString() + "-> error de definicion (clase '" + cldr.type.s + "' esta definida mas de una vez) \n";
                    solve = false;
                }
                else Types.Add(cldr.type.s);
            }
            foreach (var cldr in node.list)
                solve &= this.Visit(cldr);

            return solve;
        }

        public bool Visit(Lista<Node> node)
        {
            throw new NotImplementedException();
        }

        public bool Visit(Class_Def node)
        {
            bool solve = true;
            if (Types_built_in.Contains(node.inherit_type.s))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de definicion (clase '" + node.type.s + "' no puede heredar de tipos built-in) \n";
                solve = false;
            }
            if (!Types.Contains(node.inherit_type.s))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de definicion (el tipo padre de la clase '" + node.type.s + "' no esta definido ) \n";
                solve = false;
            }
            foreach (var item in node.attr.list_Node)
                solve &= this.Visit(item);

            foreach (var item in node.method.list_Node)
                solve &= this.Visit(item);

            return solve;
        }

        public bool Visit(Method_Def node)
        {
            bool solve = true;
            solve &= Visit(node.type);

            foreach (var arg in node.args.list_Node)
                solve &= this.Visit(arg);


            solve &= this.Visit(node.exp);
            return solve;
        }

        public bool Visit(Attr_Def node)
        {
            return Visit(node.name) && Visit(node.exp) && Visit(node.type);
        }

        public bool Visit(Formal node)
        {
            return Visit(node.name) && Visit(node.type);
        }

        public bool Visit(Type_cool node)
        {
            if (!Types.Contains(node.s))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de definicion (clase '" + node.s + "' no esta definida) \n";
                return false;
            }
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

        public bool Visit(Str node)
        {
            return true;
        }

        public bool Visit(Call_Method node)
        {
            return true;
        }

        public bool Visit(Dispatch node)
        {
            bool solve = true;
            if(node.s != "sin castear ")
            {
                solve &= Visit(new Type_cool(node.s));
            }
            solve &= this.Visit(node.exp);
            return solve;
        }

        public bool Visit(Let_In node)
        {
            bool solve = true;
            foreach (var attr in node.attrs.list_Node)
            {
                solve &= this.Visit(attr);
            }
            solve &= this.Visit(node.exp);
            return solve;
        }

        public bool Visit(If_Else node)
        {
            return Visit(node.cond) && Visit(node.then) && Visit(node.elsse);
        }

        public bool Visit(While_loop node)
        {
            return Visit(node.exp1) && Visit(node.exp2);
        }

        public bool Visit(Body node)
        {
            bool solve = true;
            foreach (var exp in node.list.list_Node)
            {
                solve &= Visit(exp);
            }
            return solve;
        }

        public bool Visit(New_type node)
        {
            return Visit(node.type);
        }

        public bool Visit(IsVoid node)
        {
            return Visit(node.exp);
        }

        public bool Visit(BinaryExpr node)
        {
            return Visit(node.left) && Visit(node.right);
        }

        public bool Visit(UnaryExpr node)
        {
            return Visit(node.exp);
        }

        public bool Visit(Assign node)
        {
            return true;
        }

        public bool Visit(Id node)
        {
            if (ReservesWords.Contains(node.name))
            {
                Logger += "En la expresion " + node.ToString() + "-> error de definicion (identificador '" + node.name + "' no puede tener nombre de palabras reservadas) \n";
                return false;
            }
            return true;
        }

        public bool Visit(Const node)
        {
            return true;
        }
    }
}
