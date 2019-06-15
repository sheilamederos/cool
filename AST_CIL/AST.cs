using System;
using System.Collections.Generic;

namespace AST_CIL
{
    public abstract class CIL_Node
    {
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
        public List<Tuple<string, int>> _integerVars { get; }

        public CIL_Data()
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
        public Dictionary<string, int> Args;   // estos son los nombres de las variables y el lugar que ocupan en
        public Dictionary<string, int> Locals; // memoria en MIPS
        public List<CIL_Instruction> Instructions;

        public CIL_Function(string name, Dictionary<string, int> inValues)
        {
            _name = name;
            Args = inValues;
            Locals = new Dictionary<string, int>();
            Instructions = new List<CIL_Instruction>();
        }

        public void AddLocal(string name, int id)
        {
            Locals.Add(name, id);
        }

        public void AddInstruction(CIL_Instruction ins)
        {
            Instructions.Add(ins);
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
        public CIL_MyVar Dest;
        public CIL_Atom RigthMem;

        public CIL_Assig(CIL_MyVar dest, CIL_Atom rigthMem)
        {
            Dest = dest;
            RigthMem = rigthMem;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public abstract class CIL_Atom : CIL_Node
    {
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_MyVar : CIL_Atom
    {
        public string Name { get; }
        public int Id { get; }

        public CIL_MyVar(string name, int id=0)
        {
            Name = name;
            Id = id;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_MyCons : CIL_Atom
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

    public class CIL_ArithExpr : CIL_Instruction
    {
        public CIL_MyVar Dest;
        public CIL_Atom RigthOp;
        public CIL_Atom LeftOp;
        public string Op;
        
        public CIL_ArithExpr(CIL_MyVar dest, CIL_Atom rigthOp, CIL_Atom leftOp, string op)
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
        public CIL_Atom Value;

        public CIL_SetAttr(string instance, CIL_OneType myType, CIL_Atom value)
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
        public CIL_OneType MyType;

        public CIL_Allocate(string dest, CIL_OneType myType)
        {
            Dest = dest;
            MyType = myType;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Call : CIL_Instruction
    {
        public CIL_MyVar Dest;
        public CIL_Function MyFunc;

        public CIL_Call(CIL_MyVar dest, CIL_Function myFunc)
        {
            Dest = dest;
            MyFunc = myFunc;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Load : CIL_Instruction
    {
        public CIL_MyVar Dest;
        public CIL_MyVar Msg;

        public CIL_Load(CIL_MyVar dest, CIL_MyVar msg)
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
        public CIL_Atom MyVar;

        public CIL_Str(string dest, CIL_Atom myVar)
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
        public CIL_Atom _atom;

        public CIL_Return(CIL_Atom atom)
        {
            _atom = atom;
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
        public CIL_MyVar ConditionVar;
        public string Label;

        public CIL_ConditionalJump(CIL_MyVar conditionVar, string label)
        {
            ConditionVar = conditionVar;
            Label = label;
        }
        
        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }

    public class CIL_Arg : CIL_Instruction
    {
        /// <summary>
        /// Importante: el constructor de esta clase solo recive variables.
        /// Si se va a pasar una constante a un metodo, primero crear una
        /// nueva variable, luego hacer una asignacion y despues pasar esa
        /// variable al llamado de la funcion
        /// </summary>
        
        public CIL_MyVar Arg;

        public CIL_Arg(CIL_MyVar arg)
        {
            Arg = arg;
        }

        public override void Accept(IVisitor visitor) => visitor.Accept(this);
    }
}