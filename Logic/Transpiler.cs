using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using AST;
using Logic;

namespace Logic
{
    public class GetAST
    {
        public static Node Show(string text)
        {
            var input = new AntlrInputStream(text);
            var lexer = new coolgrammarLexer(input);
            var tokens = new CommonTokenStream(lexer);
            var parser = new coolgrammarParser(tokens);
            var v = new Transpiler();
            return v.Visit(parser.program());
        }
    }

    public class Transpiler : coolgrammarBaseVisitor<Node>
    {
        public override Node VisitAssign([NotNull] coolgrammarParser.AssignContext context)
        {
            string id = context.ID().GetText();
            Expr r = (Expr)Visit(context.expr());
            return new Assign(id, r);
        }
        public override Node VisitSumaresta([NotNull] coolgrammarParser.SumarestaContext context)
        {
            Expr l = (Expr)Visit(context.expr(0));
            Expr r = (Expr)Visit(context.expr(1));
            return new BinaryExpr(l, r, context.op.Text);
        }

        public override Node VisitComp([NotNull] coolgrammarParser.CompContext context)
        {
            Expr l = (Expr)Visit(context.expr(0));
            Expr r = (Expr)Visit(context.expr(1));
            return new BinaryExpr(l, r, context.op.Text);
        }

        public override Node VisitMultdiv([NotNull] coolgrammarParser.MultdivContext context)
        {
            Expr l = (Expr)Visit(context.expr(0));
            Expr r = (Expr)Visit(context.expr(1));
            return new BinaryExpr(l, r, context.op.Text);
        }

        public override Node VisitUnary_exp([NotNull] coolgrammarParser.Unary_expContext context)
        {
            Expr exp = (Expr)Visit(context.expr());
            return new UnaryExpr(exp, context.op.Text);
        }

        public override Node VisitId([NotNull] coolgrammarParser.IdContext context)
        {
            return new Id(context.ID().GetText());
        }

        public override Node VisitBool([NotNull] coolgrammarParser.BoolContext context)
        {
            return new Const(context.cons.Text);
        }

        public override Node VisitInt([NotNull] coolgrammarParser.IntContext context)
        {
            return new Const(context.INTEGER().GetText());
        }

        public override Node VisitParentesis([NotNull] coolgrammarParser.ParentesisContext context)
        {
            return Visit(context.expr());
        }

        public override Node VisitProgram([NotNull] coolgrammarParser.ProgramContext context)
        {
            var clases = new List<Class_Def>();
            foreach (var item in context.@class())
            {
               clases.Add((Class_Def)Visit(item));
            }
            return new Program(clases);
        }
        
    }
}
