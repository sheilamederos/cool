using System;
using System.Collections.Generic;

namespace AST_CIL
{
        public class OneType
    {
        public List<string> Attributes;
        public List<Tuple<string, string>> Methods;

        public OneType(List<string> attributes, List<Tuple<string, string>> methods)
        {
            Attributes = attributes;
            Methods = methods;
        }
    }
    
    public class Types
    {
        public Dictionary<string, OneType> _types { get; }

        public Types()
        {
            _types = new Dictionary<string, OneType>();
        }
    }

    public class Data
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
    }

    public class Instruction
    {}
    
    public class Function
    {
        public List<int> Args; // preguntar sobre el tipo de los argumentos,aqui asumo que solo son enteros
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
    }

    public class Code
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
    }

    public class Assig : Instruction // OJO: las asignaciones solo van a ser con constantes
    {
        private bool _constant = false;
        public bool Constant => _constant;

        public readonly string IzqMem;
        public readonly int DerMemCons;
        public string DerMemVar;

        public Assig(string izqMem, int derMem)
        {
            _constant = true;
            IzqMem = izqMem;
            DerMemCons = derMem;
        }

        public Assig(string izqMem, string derMem)
        {
            IzqMem = izqMem;
            DerMemVar = derMem;
        }
    }

    public class OperationBin : Instruction // OJO: las operaciones solo van a ser con variables
    {
        // aqui entran todas las expresiones aritmeticas, GETATTR, SETATTR y VCALL
        public string Dest;
        public string RigthOp;
        public string LeftOp;
        public readonly string Op;

        public OperationBin(string dest, string rigthOp, string leftOp, string op)
        {
            Dest = dest;
            RigthOp = rigthOp;
            LeftOp = leftOp;
            Op = op;
        }
    }

    public class OperationUni : Instruction
    {
        // aqui entran ALLOCATE, CALL, LOAD, LENGTH, CONCAT, SUBSTRING, STR y ConditionalJump(IF x GOTO l)
        public string RigthMem;
        public string LeftMem;
        public readonly string Op;

        public OperationUni(string rigthMem, string leftMem, string op)
        {
            RigthMem = rigthMem;
            LeftMem = leftMem;
            Op = op;
        }
    }

    public class SimpleOperation : Instruction
    {
        // aqui entran LABEL, GOTO, RETURN, READ y PRINT
        public string Obj;
        public string Op;

        public SimpleOperation(string obj, string op)
        {
            Obj = obj;
            Op = op;
        }
    }
}