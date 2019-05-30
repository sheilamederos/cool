using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);


    }

    public class Class_Def : Node
    {
        public Id name;
        public Lista<Method_Def> method;
        public Lista<Attr_Def> attr;
        public Class_Def(Id n, Lista<Method_Def> m, Lista<Attr_Def> a) : base(new Node[] { n, m, a })
        {
            name = n;
            method = m;
            attr = a;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "clase: " + name;
        }
    }

    public class Method_Def : Node
    {
        public Id name;
        public Type_cool type;
        public Lista<Formal> args;
        public Method_Def(Id n, Type_cool t, Lista<Formal> a) : base ( new Node[] { n, t, a })
        {
            name = n;
            type = t;
            args = a;
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
            return "atributo: " + name + " tipo: " + type;
        }
    }    

    public class Formal : Node
    {
        public Id name;
        public Type_cool type;
        public Formal(Id n, Type_cool t) : base (null)
        {
            name = n;
            t = type;
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
        public string type { get; set; }
        public Expr(IEnumerable<Node> children) : base (children)
        {
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return "Expr";
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
        public string id;
        public Expr exp;
        public Assign(string id, Expr exp) : base (new Node[] { exp})
        {
            this.id = id;
            this.exp = exp;
        }

        public override T Visit<T>(IVisitorAST<T> visitor) => visitor.Visit(this);

        public override string ToString()
        {
            return id + ' ' + "<-"+ ' ' + exp.ToString();
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
