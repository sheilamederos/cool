using System;
using System.Collections.Generic;

namespace CIL
{
    public abstract class CIL_Node
    {
        public int dir;
        public virtual void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Program : CIL_Node
    {
        public CIL_Code Code;
        public CIL_Data Data;
        public CIL_Types Types;

        public CIL_Program(CIL_Code code, CIL_Data data, CIL_Types types)
        {
            Code = code;
            Data = data;
            Types = types;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_OneType : CIL_Node
    {
        public List<string> Attributes;
        public List<Tuple<string, string>> Methods;

        public CIL_OneType(List<string> attributes, List<Tuple<string, string>> methods)
        {
            Attributes = attributes;
            Methods = methods;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Types : CIL_Node
    {
        public Dictionary<string, CIL_OneType> _types { get; }

        public CIL_Types()
        {
            _types = new Dictionary<string, CIL_OneType>();
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Data : CIL_Node
    {
        public List<Tuple<string, string>> _stringVars { get; }

        public CIL_Data()
        {
            _stringVars = new List<Tuple<string, string>>();
        }

        public void AddStringVar(string name, string value)
        {
            _stringVars.Add(new Tuple<string, string>(name, value));
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public abstract class CIL_Instruction : CIL_Node
    {
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Function : CIL_Node
    {
        private string _name;
        public string Name => _name;
        public List<string> Args;   // estos son los nombres de las variables y el lugar que ocupan en
        public List<string> Locals; // memoria en MIPS
        public List<CIL_Instruction> Instructions;

        public CIL_Function(string name, List<string> args, List<string> locals, List<CIL_Instruction> inst)
        {
            _name = name;
            Args = args;
            Locals = locals;
            Instructions = inst;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Code : CIL_Node
    {
        public List<CIL_Function> Funcs;

        public CIL_Code()
        {
            Funcs = new List<CIL_Function>();
        }

        public void AddFunc(CIL_Function func)
        {
            Funcs.Add(func);
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Assig : CIL_Instruction
    {
        public string Dest;
        public string RigthMem;

        public CIL_Assig(string dest, string rigthMem)
        {
            Dest = dest;
            RigthMem = rigthMem;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_MyVar : CIL_Instruction
    {
        public string Name { get; }
        public int Id { get; }

        public CIL_MyVar(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_MyCons : CIL_Instruction
    {
        public int Value { get; }

        public CIL_MyCons(int value)
        {
            Value = value;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Concat : CIL_Instruction
    {
        public CIL_MyVar Dest;
        public CIL_MyVar Var1;
        public CIL_MyVar Var2;

        public CIL_Concat(CIL_MyVar dest, CIL_MyVar var1, CIL_MyVar var2)
        {
            Dest = dest;
            Var1 = var1;
            Var2 = var2;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Substring : CIL_Instruction
    {
        public string Dest;
        public CIL_MyVar Var1;
        public CIL_MyVar Var2;

        public CIL_Substring(string dest, CIL_MyVar var1, CIL_MyVar var2)
        {
            Dest = dest;
            Var1 = var1;
            Var2 = var2;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_UnaryExpr : CIL_Instruction
    {
        public string dest, op;
        public string expr;
        public CIL_UnaryExpr(string dest, string op, string exp)
        {
            this.dest = dest;
            this.op = op;
            expr = exp;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_ArithExpr : CIL_Instruction
    {
        public string Dest;
        public string RigthOp;
        public string LeftOp;
        public string Op;

        public CIL_ArithExpr(string dest, string rigthOp, string leftOp, string op)
        {
            Dest = dest;
            RigthOp = rigthOp;
            LeftOp = leftOp;
            Op = op;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_GetAttr : CIL_Instruction
    {
        public string Dest;
        public string Instance;
        public CIL_OneType MyType;

        public CIL_GetAttr(string dest, string instance, CIL_OneType myType)
        {
            Dest = dest;
            Instance = instance;
            MyType = myType;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_SetAttr : CIL_Instruction
    {
        public string Instance;
        public CIL_OneType MyType;
        public string Value;

        public CIL_SetAttr(string instance, CIL_OneType myType, string value)
        {
            Instance = instance;
            MyType = myType;
            Value = value;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_VCall : CIL_Instruction
    {
        public string Dest;
        public CIL_OneType MyType;
        public string FuncName;

        public CIL_VCall(string dest, CIL_OneType myType, string funcName)
        {
            Dest = dest;
            MyType = myType;
            FuncName = funcName;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Allocate : CIL_Instruction
    {
        public string Dest;
        public string MyType;

        public CIL_Allocate(string dest, string myType)
        {
            Dest = dest;
            MyType = myType;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Call : CIL_Instruction
    {
        public string Dest;
        public CIL_Function MyFunc;

        public CIL_Call(string dest, CIL_Function myFunc)
        {
            Dest = dest;
            MyFunc = myFunc;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Load : CIL_Instruction
    {
        public string Dest;
        public string Msg;

        public CIL_Load(string dest, string msg)
        {
            Dest = dest;
            Msg = msg;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Length : CIL_Instruction
    {
        public string Dest;
        public CIL_MyVar Msg;

        public CIL_Length(string dest, CIL_MyVar msg)
        {
            Dest = dest;
            Msg = msg;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Str : CIL_Instruction
    {
        public string Dest;
        public string MyVar;

        public CIL_Str(string dest, string myVar)
        {
            Dest = dest;
            MyVar = myVar;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Label : CIL_Instruction
    {
        public string _label;

        public CIL_Label(string label)
        {
            _label = label;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_If : CIL_Instruction
    {
        public string cond;
        public string label;
        public CIL_If(string cond, string label)
        {
            this.cond = cond;
            this.label = label;
        }
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Goto : CIL_Instruction
    {
        public string _label;

        public CIL_Goto(string label)
        {
            _label = label;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Return : CIL_Instruction
    {
        public string id;
        public string value;

        public CIL_Return(string id, string value)
        {
            this.id = id;
            this.value = value;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Read : CIL_Instruction
    {
        public CIL_MyVar _var;

        public CIL_Read(CIL_MyVar myVar)
        {
            _var = myVar;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Print : CIL_Instruction
    {
        public CIL_MyVar _var;

        public CIL_Print(CIL_MyVar myVar)
        {
            _var = myVar;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_ConditionalJump : CIL_Instruction
    {
        public CIL_Instruction ConditionVar;
        public string Label;

        public CIL_ConditionalJump(CIL_Instruction conditionVar, string label)
        {
            ConditionVar = conditionVar;
            Label = label;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Arg : CIL_Instruction
    {
        public string name;

        public CIL_Arg(string n)
        {
            name = n;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_is_void : CIL_Instruction
    {
        public string arg;
        public string ret;
        public CIL_is_void(string ret, string arg)
        {
            this.ret = ret;
            this.arg = arg;
        }
        public override void Accept(IVisitor visitor) => visitor.Accept(this);


    }
}