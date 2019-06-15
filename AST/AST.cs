using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST
{
    public abstract class Node
    {
        public List<Node> children;

        public Node(IEnumerable<Node> children)
        {
            if (children != null) this.children = new List<Node>(children);
            else this.children = new List<Node>();
        }

        // visitor
        public virtual T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);
    }

    public class Program : Node
    {
        public List<Class_Def> list;
        public Program(IEnumerable<Class_Def> children) : base(children)
        {
            this.list = new List<Class_Def>(children);
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "begin_Program";
        }
    }

    public class Lista<T> : Node where T : Node
    {
        public List<T> list_Node;
        public Lista(List<T> list) : base(list)
        {
            list_Node = list;
        }

        public override Q Visit<Q>(IVisitorAST<Q> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Lista de " + typeof(T).ToString() ; 
        }
    }

    public class Class_Def : Node
    {
        public Type_cool type;
        public Type_cool inherit_type;
        public Lista<Method_Def> method;
        public Lista<Attr_Def> attr;
        public Class_Def(Type_cool t, Type_cool t1, Lista<Method_Def> m, Lista<Attr_Def> a) : base(new Node[] { m, a })
        {
            type = t;
            inherit_type = t1;
            method = m;
            attr = a;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "clase: " + type + " padre: " + inherit_type;
        }
    }

    public class Method_Def : Node
    {
        public Id name;
        public Type_cool type;
        public Lista<Formal> args;
        public Expr exp;
        public Method_Def(Id n, Type_cool t, Lista<Formal> a, Expr e) : base ( new Node[] { n, t, a, e })
        {
            name = n;
            type = t;
            args = a;
            exp = e;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "metodo: " + name;
        }

    }

    public class Attr_Def : Node
    {
        public Id name;
        public Type_cool type;
        public Expr exp;
        public Attr_Def(Id n, Type_cool type, Expr exp) : base (new Node[] { n, type, exp })
        {
            name = n;
            this.type = type;
            this.exp = exp;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "atributo: " + name.ToString() + " tipo: " + type.ToString();
        }
    }    

    public class Formal : Node
    {
        public Id name;
        public Type_cool type;
        public Formal(Id n, Type_cool t) : base (new Node[] { n, t })
        {
            name = n;
            type = t;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "name: " + name + " tipo: " + type;
        }
    }

    public class Type_cool : Node
    {
        public string s;
        public Type_cool(string s) : base (null)
        {
            this.s = s;
        }
        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "tipo: " + s;
        }
    }

    public class Expr : Node
    {
        public Type_cool type { get; set; }

        public Expr(IEnumerable<Node> children) : base (children)
        {
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Expr";
        }
    }

    public class Call_Method : Expr
    {
        public Id name;
        public Lista<Expr> args;
        public Call_Method(Id n, Lista<Expr> args) : base (new Node[] { n,args })
        {
            name = n;
            this.args = args;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Call_Method: " + name;
        }
    }
    public class Dispatch : Expr
    {
        public Expr exp;
        public Call_Method call;
        public string s;
        public Dispatch(Expr exp, Type_cool type, Call_Method call) : base (new Node[] { exp, type, call })
        {
            this.exp = exp;
            this.type = type;
            this.call = call;
            s = (this.type != null) ? this.type.s + ' ' : "sin castear "; 
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Dispatch: " + "Exp: " + exp.ToString() + " " + "tipo: " + this.s + call.ToString() ;
        }
    }

    public class Str : Expr
    {
        public string s;
        public Str(string s) : base (null)
        {
            this.s = s.Substring(1, s.Length - 2);
        }
        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return s;
        }
    }

    public class Let_In : Expr
    {
        public Lista<Attr_Def> attrs;
        public Expr exp;
        public Let_In(Lista<Attr_Def> a, Expr exp) :base (new Node[] { a, exp})
        {
            attrs = a;
            this.exp = exp;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Let_In: " ;
        }

    }

    public class If_Else : Expr
    {
        public Expr cond, then, elsse;
        public If_Else(Expr e1, Expr e2, Expr e3) : base(new Node[] { e1, e2, e3 })
        {
            cond = e1;
            then = e2;
            elsse = e3;
        }
        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "If: " + cond.ToString() + " Then: " + then.ToString() + " Else: " + elsse.ToString();
        }
    }

    public class While_loop : Expr
    {
        public Expr exp1, exp2;
        public While_loop(Expr e1, Expr e2) : base(new Node[] { e1, e2 })
        {
            exp1 = e1;
            exp2 = e2;
        }
        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "While";
        }
    }

    public class Body : Expr
    {
        public Lista<Expr> list;
        public Body(Lista<Expr> l) : base(new Node[] {l})
        {
            list = l;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Body";
        }
    }

    public class New_type : Expr
    {
        public New_type(Type_cool t) : base (new Node[] { t})
        {
            this.type = t;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "New_type";
        }

    }

    public class IsVoid : Expr
    {
        public Expr exp;
        public IsVoid(Expr e) : base (new Node[] { e})
        {
            exp = e;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "IsVoid";
        }
    }

    public class BinaryExpr : Expr
    {
        public Expr left, right;
        public string op;
        public BinaryExpr(Expr left, Expr right, string op) : base(new Node[] { left, right })
        {
            this.left = left; this.right = right; this.op = op;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return left.ToString() + ' ' + op + ' ' + right.ToString();
        }
    }

    public class UnaryExpr: Expr
    {
        public Expr exp;
        public string op;
        public UnaryExpr(Expr exp, string op) : base(new Node[] { exp })
        {
            this.exp = exp;
            this.op = op;
        }
        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return op + ' ' + exp.ToString();
        }
    }

    public class Assign : Expr
    {
        public Id id;
        public Expr exp;
        public Assign(Id id, Expr exp) : base (new Node[] { id, exp})
        {
            this.id = id;
            this.exp = exp;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Assign";
        }
    }

    public class Id : Expr
    {
        //public IToken tok;
        public string name;
        public Id(string name) : base(null)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Const : Expr
    {
        //public IToken tok;
        public string name;
        public Const(string name) : base (null)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }

    

}
