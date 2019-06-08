using System;
using System.Collections.Generic;

namespace AST_CIL
{
    public abstract class Node
    {
        public virtual void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
    public class OneType : Node
    {
        public List<string> Attributes;
        public List<Tuple<string, string>> Methods;

        public OneType(List<string> attributes, List<Tuple<string, string>> methods)
        {
            Attributes = attributes;
            Methods = methods;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
    public class Types : Node
    {
        public Dictionary<string, OneType> _types { get; }

        public Types()
        {
            _types = new Dictionary<string, OneType>();
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Data : Node
    {
        public List<Tuple<string, string>> _stringVars { get; }
        public List<Tuple<string, int>> _integerVars { get; }

        public Data()
        {
            _stringVars = new List<Tuple<string, string>>();
            _integerVars = new List<Tuple<string, int>>();
        }

        public void AddIntegerVar(string name, int value)
        {
            _integerVars.Add(new Tuple<string, int>(name, value));
        }

        public void AddStringVar(string name, string value)
        {
            _stringVars.Add(new Tuple<string, string>(name, value));
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public abstract class Instruction : Node
    {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
    public class Function : Node
    {
        public List<int> Args; // asumo que solo son enteros
        public List<string> Locals;
        public List<Instruction> Instructions;

        public Function(List<int> inValues)
        {
            Args = inValues;
            Locals = new List<string>();
            Instructions = new List<Instruction>();
        }

        public void AddLocal(string name)
        {
            Locals.Add(name);
        }

        public void AddInstruction(Instruction ins)
        {
            Instructions.Add(ins);
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Code : Node
    {
        public List<Function> Funcs;

        public Code()
        {
            Funcs = new List<Function>();
        }

        public void AddFunc(Function func)
        {
            Funcs.Add(func);
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Assig : Instruction
    {
        public string Dest;
        public Atom RigthMem;

        public Assig(string dest, Atom rigthMem)
        {
            Dest = dest;
            RigthMem = RigthMem;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public abstract class Atom : Node
    {
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class MyVar : Atom
    {
        public string Name { get; }

        public MyVar(string name)
        {
            Name = name;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class MyCons : Atom
    {
        public int Value { get; }

        public MyCons(int value)
        {
            Value = value;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Concat : Instruction
    {
        public string Dest;
        public MyVar Var1;
        public MyVar Var2;

        public Concat(string dest, MyVar var1, MyVar var2)
        {
            Dest = dest;
            Var1 = var1;
            Var2 = var2;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Substring : Instruction
    {
        public string Dest;
        public MyVar Var1;
        public MyVar Var2;

        public Substring(string dest, MyVar var1, MyVar var2)
        {
            Dest = dest;
            Var1 = var1;
            Var2 = var2;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class ArithExpr : Instruction
    {
        public string Dest;
        public Atom RigthOp;
        public Atom LeftOp;
        public string Op;
        
        public ArithExpr(string dest, Atom rigthOp, Atom leftOp, string op)
        {
            Dest = dest;
            RigthOp = rigthOp;
            LeftOp = leftOp;
            Op = op;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class GetAttr : Instruction
    {
        public string Dest;
        public string Instance;
        public OneType MyType;

        public GetAttr(string dest, string instance, OneType myType)
        {
            Dest = dest;
            Instance = instance;
            MyType = myType;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class SetAttr : Instruction
    {
        public string Instance;
        public OneType MyType;
        public Atom Value;

        public SetAttr(string instance, OneType myType, Atom value)
        {
            Instance = instance;
            MyType = myType;
            Value = value;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class VCall : Instruction
    {
        public string Dest;
        public OneType MyType;
        public string FuncName;

        public VCall(string dest, OneType myType, string funcName)
        {
            Dest = dest;
            MyType = myType;
            FuncName = funcName;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Allocate : Instruction
    {
        public string Dest;
        public OneType MyType;

        public Allocate(string dest, OneType myType)
        {
            Dest = dest;
            MyType = myType;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Call : Instruction
    {
        public string Dest;
        public Function MyFunc;

        public Call(string dest, Function myFunc)
        {
            Dest = dest;
            MyFunc = myFunc;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Load : Instruction
    {
        public string Dest;
        public MyVar Msg;

        public Load(string dest, MyVar msg)
        {
            Dest = dest;
            Msg = msg;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
    public class Length : Instruction
    {
        public string Dest;
        public MyVar Msg;

        public Length(string dest, MyVar msg)
        {
            Dest = dest;
            Msg = msg;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Str : Instruction
    {
        public string Dest;
        public Atom MyVar;

        public Str(string dest, Atom myVar)
        {
            Dest = dest;
            MyVar = myVar;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Label : Instruction
    {
        public string _label;

        public Label(string label)
        {
            _label = label;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Goto : Instruction
    {
        public string _label;

        public Goto(string label)
        {
            _label = label;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Return : Instruction
    {
        public Atom _atom;

        public Return(Atom atom)
        {
            _atom = atom;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class Read : Instruction
    {
        public MyVar _var;

        public Read(MyVar myVar)
        {
            _var = myVar;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
    
    public class Print : Instruction
    {
        public MyVar _var;

        public Print(MyVar myVar)
        {
            _var = myVar;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    public class ConditionalJump : Node
    {
        public Atom ConditionVar;
        public string Label;

        public ConditionalJump(Atom conditionVar, string label)
        {
            ConditionVar = conditionVar;
            Label = label;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}