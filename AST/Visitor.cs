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

        T Visit(Expr node);

        T Visit(BinaryExpr node);

        T Visit(UnaryExpr node);

        T Visit(Assign node);

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
