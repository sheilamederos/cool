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
            //Console.WriteLine(parser.program().ToString(parser));
            
            var v = new Transpiler();
            return v.Visit(parser.program());
        }
    }

    public class Transpiler : coolgrammarBaseVisitor<Node>
    {
        public override Node VisitAssign([NotNull] coolgrammarParser.AssignContext context)
        {
            Id id = (Id)Visit(context.ID());
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
        public override Node VisitClass([NotNull] coolgrammarParser.ClassContext context)
        {
            List<Method_Def> met = new List<Method_Def>();
            List<Attr_Def> attr = new List<Attr_Def>();
            foreach (var item in context.feature())
            {
                var v = Visit(item);
                if (v is Method_Def) met.Add((Method_Def)v);
                else attr.Add((Attr_Def)v);
            }

            string father;
            if (context.TYPE().Length > 1) father = context.TYPE(1).GetText();
            else father = "Object";

            return new Class_Def(new Type_cool(context.TYPE(0).GetText()), new Type_cool(father), new Lista<Method_Def>(met), new Lista<Attr_Def>(attr));
        }

        public override Node VisitF_method([NotNull] coolgrammarParser.F_methodContext context)
        {
            return Visit(context.method());
        }

        public override Node VisitF_attr([NotNull] coolgrammarParser.F_attrContext context)
        {
            return Visit(context.attr());
        }

        public override Node VisitMethod([NotNull] coolgrammarParser.MethodContext context)
        {
            List<Formal> args = new List<Formal>();
            foreach (var item in context.args_def().formal())
            {
                var v = Visit(item);
                args.Add((Formal)v);
            }
            Expr exp = (Expr)Visit(context.expr());
            return new Method_Def(new Id(context.ID().GetText()), new Type_cool(context.TYPE().GetText()), new Lista<Formal>(args), exp);
        }

        public override Node VisitAttr([NotNull] coolgrammarParser.AttrContext context)
        {
            var a = (Formal)Visit(context.formal());
            var exp = (Expr)Visit(context.expr());
            return new Attr_Def(a.name, a.type, exp);
        }

        public override Node VisitFormal([NotNull] coolgrammarParser.FormalContext context)
        {
            var id = new Id(context.ID().GetText());
            var t = new Type_cool(context.TYPE().GetText());
            return new Formal(id,t);
        }

        public override Node VisitCall_method([NotNull] coolgrammarParser.Call_methodContext context)
        {
            var list = new List<Expr>();
            foreach (var item in context.args_call().expr())
            {
                var v = Visit(item);
                list.Add((Expr)v);
            }
            return new Call_Method(new Id(context.ID().GetText()), new Lista<Expr>(list));
        }

        public override Node VisitDispatch([NotNull] coolgrammarParser.DispatchContext context)
        {
            Expr exp = (Expr)Visit(context.expr());
            Type_cool t = null;
            if (context.TYPE() != null) t = new Type_cool(context.TYPE().GetText());
            Id id = new Id(context.ID().GetText());
            var list = new List<Expr>();
            foreach (var item in context.args_call().expr())
            {
                var v = Visit(item);
                list.Add((Expr)v);
            }

            return new Dispatch(exp, t, new Call_Method(id, new Lista<Expr>(list)));
        }
        public override Node VisitLet([NotNull] coolgrammarParser.LetContext context)
        {
            List<Attr_Def> list = new List<Attr_Def>();
            Expr exp = (Expr)Visit(context.expr());

            foreach (var item in context.attr())
            {
                var v = (Attr_Def)Visit(item);
                list.Add(v);
            }

            return new Let_In(new Lista<Attr_Def>(list), exp);
        }

        public override Node VisitIf([NotNull] coolgrammarParser.IfContext context)
        {
            Expr exp1 = (Expr)Visit(context.expr(0));
            Expr exp2 = (Expr)Visit(context.expr(1));
            Expr exp3 = (Expr)Visit(context.expr(2));

            return new If_Else(exp1, exp2, exp3);
        }

        public override Node VisitWhile([NotNull] coolgrammarParser.WhileContext context)
        {
            Expr exp1 = (Expr)Visit(context.expr(0));
            Expr exp2 = (Expr)Visit(context.expr(1));
            return new While_loop(exp1, exp2);
        }

        public override Node VisitBody([NotNull] coolgrammarParser.BodyContext context)
        {
            List<Expr> list = new List<Expr>();
            foreach (var item in context.expr_list().expr())
            {
                var v = (Expr)Visit(item);
                list.Add(v);
            }
            return new Body(new Lista<Expr>(list));
        }

        public override Node VisitString([NotNull] coolgrammarParser.StringContext context)
        {
            return new Str(context.STR().GetText());
        }

        public override Node VisitNew_type([NotNull] coolgrammarParser.New_typeContext context)
        {
            return new New_type(new Type_cool(context.TYPE().GetText()));
        }

        public override Node VisitIsvoid([NotNull] coolgrammarParser.IsvoidContext context)
        {
            Expr exp = (Expr)Visit(context.expr());
            return new IsVoid(exp);
        }

    }
}
