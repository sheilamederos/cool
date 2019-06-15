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
    

    public class CIL_Concat : CIL_Instruction
    {
        public string Dest;
        public string Var1;
        public string Var2;

        public CIL_Concat(string dest, string var1, string var2)
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
        public string Var1;
        public string Var2;

        public CIL_Substring(string dest, string var1, string var2)
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
        public string Attr;

        public CIL_GetAttr(string dest, string instance, string attr)
        {
            Dest = dest;
            Instance = instance;
            Attr = attr;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_SetAttr : CIL_Instruction
    {
        public string Instance;
        public string Attr;
        public string Value;

        public CIL_SetAttr(string instance, string attr, string value)
        {
            Instance = instance;
            Attr = attr;
            Value = value;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_VCall : CIL_Instruction
    {
        public string Dest;
        public string MyType;
        public string Name;
        public List<string> Args;

        public CIL_VCall(string dest, string myType, string name, List<string> args)
        {
            Dest = dest;
            MyType = myType;
            Name = name;
            Args = args;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Typeof : CIL_Instruction
    {
        public string dest;
        public string expr;
        public CIL_Typeof(string dest, string expr)
        {
            this.dest = dest;
            this.expr = expr;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);

    }

    public class CIL_Call : CIL_Instruction
    {
        public string Dest;
        public string Name;
        public List<string> Args;

        public CIL_Call(string dest, string name, List<string> args)
        {
            Dest = dest;
            Name = name;
            Args = args;
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
        public string Msg;

        public CIL_Length(string dest, string msg)
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
        public string _var;

        public CIL_Read(string myVar)
        {
            _var = myVar;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Print : CIL_Instruction
    {
        public string _var;

        public CIL_Print(string myVar)
        {
            _var = myVar;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_ConditionalJump : CIL_Instruction
    {
        public string ConditionVar;
        public string Label;

        public CIL_ConditionalJump(string conditionVar, string label)
        {
            ConditionVar = conditionVar;
            Label = label;
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