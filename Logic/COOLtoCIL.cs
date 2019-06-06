using System;
using System.Collections.Generic;
using System.Linq;
using CIL;
using AST;

namespace Logic
{
    public class Take_str
    {
        int id;
        string str;
        public Take_str(string str, int id = 0)
        {
            this.id = id;
            this.str = str;
        }
        public string take(string name)
        {
            return str + '/' + name + id++.ToString();
        }
    }

    public class Scope
    {
        public Scope father;
        public string id;
        Take_str take;
        Dictionary<string, string> dic;

        public Scope(string id, Scope father = null)
        {
            this.id = id;
            this.father = father;
            take = new Take_str(id);
            dic = new Dictionary<string, string>();
        }

        public string Var(string id = "")
        {
            var x = take.take(id);
            return x;
        }

        public Scope New_scope(string id)
        {
            return new Scope(id, this);
        }

        public string Get_var(string key)
        {
            if (dic.ContainsKey(key)) return dic[key];
            if (father == null) return null;
            return father.Get_var(key);
        }

        public void Add_var(string key, string value)
        {
            dic.Add(key, value);
        }
    }

    public class Current_Method
    {
        public Scope current_scope;
        public List<string> args;
        public Dictionary<string, string> locals;
        public List<CIL_Instruction> body;


        public Current_Method()
        {
            args = new List<string>();
            locals = new Dictionary<string, string>();
            body = new List<CIL_Instruction>();
            current_scope = new Scope("class");
        }

        public string Add_local(string v, bool is_expr = false)
        {
            var local = current_scope.Var(v);
            if (!is_expr)
            {
                locals.Add(v, local);
                current_scope.Add_var(v, local);
            }
            else locals.Add(local, local);
            return local;
        }

        public void Add_Instruction(CIL_Instruction inst)
        {
            body.Add(inst);
        }


        public void Add_scope(string id)
        {
            current_scope = current_scope.New_scope(id);
        }

        public string Take_var(string name)
        {
            return current_scope.Var(name);
        }

        public void End_scope()
        {
            current_scope = current_scope.father;
        }
    }

    public class GET_CIL_AST
    {
        //public static CIL_Program(Program node)
        //{
        //    var comp = new CoolToCil();
        //    comp.Visit(node);
        //    return new CIL_Program()
        //}
    }

    public class CoolToCil : IVisitorAST<string>
    {
        Take_str take_data;
        public Dictionary<string, string> Data;
        List<CIL_Function> Code;
        List<CIL_OneType> Types;
        Current_Method method;

        public CoolToCil()
        {
            method = new Current_Method();
            Data = new Dictionary<string, string>();
            Code = new List<CIL_Function>();
            Types = new List<CIL_OneType>();

        }

        public string Visit(Node node)
        {
            throw new NotImplementedException();
        }


        public string Visit(Program node)
        {
            Dictionary<string, IType> Types_Cool = IType.GetAllTypes(node);

            foreach (var key in Types_Cool.Keys)
            {
                IType type = Types_Cool[key];
                List<string> attrs = new List<string>();
                List<Tuple<string, string>> mtds = new List<Tuple<string, string>>();

                foreach (var attr in type.AllAttributes())      
                {
                    attrs.Add(attr.Name);
                }

                foreach (var mtd in type.AllMethods()) 
                {
                    mtds.Add(new Tuple<string, string>(mtd.Name, mtd.Name + "_" + key));
                }

                Types.Add(new CIL_OneType(attrs, mtds));
            }

            foreach (var item in node.list)
            {
                item.Visit(this);
            }
            return "";
        }

        public string Visit(Class_Def node)
        {
            foreach (Method_Def item in node.method.list_Node)
            {
                Visit(item);
            }
            return "";
        }

        public string Visit(Method_Def node)
        {
            string solution = Visit(node.exp);
            method.Add_Instruction(new CIL_Return("ret", solution));
            Code.Add(new CIL_Function(node.name.name, new List<string>(node.args.list_Node.Select(x => x.name.name)), new List<string>(method.locals.Values), method.body));
            method = new Current_Method();
            return "";
        }

        public string Visit(Attr_Def node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Formal node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Type_cool node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Expr node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Str node)
        {
            if (!Data.ContainsKey(node.s))
            {
                string v = take_data.take(method.current_scope.id);
                Data.Add(node.s, v);
            }
            var exp = method.Add_local("exp", true);
            method.Add_Instruction(new CIL_Load(exp, Data[node.s]));
            return exp;
        }

        public string Visit(Call_Method node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Dispatch node)
        {
            throw new NotImplementedException();
        }

        public string Visit(Let_In node)
        {
            method.Add_scope("let");
            Visit(node.attrs);
            var exp = Visit(node.exp);
            var ret = method.Add_local("expr", true);
            method.Add_Instruction(new CIL_Assig(ret, exp));
            method.End_scope();
            return ret;
        }

        public string Visit(If_Else node)
        {
            var cond = Visit(node.cond);
            var then = Visit(node.then);

            var ret = method.Add_local("ret_if", true);
            var begin_if = method.Take_var("begin_if");
            var end_if = method.Take_var("end_if");
            method.Add_Instruction(new CIL_If(cond, begin_if));

            if (node.elsse != null)
            {
                var elsse = Visit(node.elsse);
                method.Add_Instruction(new CIL_Assig(ret, elsse));
                method.Add_Instruction(new CIL_Goto(end_if));
            }

            method.Add_Instruction(new CIL_Assig(ret, then));
            method.Add_Instruction(new CIL_Label(end_if));
            return ret;
        }

        public string Visit(While_loop node)
        {
            var begin_while = method.current_scope.Get_var("begin_while");
            var body_while = method.current_scope.Get_var("body_while");
            var end_while = method.current_scope.Get_var("end_while");

            var ret = method.Add_local("ret_while", true);
            var cond = Visit(node.exp1);
            var body = Visit(node.exp2);
            method.Add_Instruction(new CIL_Label(begin_while));
            method.Add_Instruction(new CIL_If(cond, body_while));
            method.Add_Instruction(new CIL_Goto(end_while));
            method.Add_Instruction(new CIL_Label(body_while));
            method.Add_Instruction(new CIL_Assig(ret, body));
            method.Add_Instruction(new CIL_Goto(begin_while));
            method.Add_Instruction(new CIL_Label(end_while));
            return ret;

        }

        public string Visit(Body node)
        {
            foreach (var item in node.list.list_Node)
            {
                 item?.Visit(this);
            }
            return "";
        }

        public string Visit(New_type node)
        {
            var ret = method.Add_local("expr", true);
            method.Add_Instruction(new CIL_Allocate(ret, node.type.s));
            return ret;
        }

        public string Visit(IsVoid node)
        {
            var v = method.Add_local("expr", true);
            string exp = Visit(node.exp);
            method.Add_Instruction(new CIL_is_void(v, exp));
            return v;
        }

        public string Visit(BinaryExpr node)
        {
            var left = Visit(node.left);
            var right = Visit(node.right);
            var ret = method.Add_local("expr", true);
            method.Add_Instruction(new CIL_ArithExpr(ret, left, right, node.op));
            return ret;
        }

        public string Visit(UnaryExpr node)
        {
            var exp = Visit(node.exp);
            var ret = method.Add_local("expr", true);
            method.Add_Instruction(new CIL_UnaryExpr(ret, node.op, exp));
            return ret;
        }

        public string Visit(Assign node)
        {
            throw new NotImplementedException();
            var exp = Visit(node.exp);
            //NECESITAMOS QUE SHEILA DEJE DE COMER PINGA
        }

        public string Visit(Id node)
        {
            throw new NotImplementedException();
            //if (method.current_scope.Get_var(node.name) == null && method.args.Contains(node.name))
            //{
            //    return node.name;
            //}
            //else if ()
        }

        public string Visit(Const node)
        {
            int x;
            if (int.TryParse(node.name, out x))
            {
                return node.name;
            }
            else if (node.name.ToUpper() == "TRUE")
            {
                return "1";
            }
            else return "0";
        }

        public string Visit(Lista<Node> node)
        {
            foreach (var item in node.list_Node)
            {
                item.Visit(this);
            }
            return "";
        }
    }
}