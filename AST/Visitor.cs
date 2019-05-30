using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST
{
    public interface IVisitorAST<T>
    {
        T Visit(Node node);

        T Visit(Program node);

        T Visit(Lista<Node> node);

        T Visit(Class_Def node);

        T Visit(Method_Def node);

        T Visit(Attr_Def node);

        T Visit(Formal node);

        T Visit(Type_cool node);

        T Visit(Expr node);

        T Visit(Call_Method node);

        T Visit(Let_In node);

        T Visit(If_Else node);

        T Visit(While_loop node);

        T Visit(Body node);

        T Visit(New_type node);

        T Visit(IsVoid node);

        T Visit(BinaryExpr node);

        T Visit(UnaryExpr node);

        T Visit(Assign node);

        T Visit(Id node);

        T Visit(Const node);
    }

    //public class CheckSemantic : IVisitorAST<bool>
    //{
    //    public bool Visit(Node node)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool Visit(BinaryExpr node)
    //    {
    //        var l = node.left.Visit(this);
    //        //var l = Visit(node.left);
    //        var r = node.right.Visit(this);
    //        return l && r;
    //    }
    //}
}
