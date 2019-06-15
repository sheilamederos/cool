using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    public class ContextType
    {
        public IType ActualType { get; set; }

        public Dictionary<string, IType> Types { get; set; }

        public List<Method> Methods { get; set; }

        public Stack<Tuple<string, IType>> Symbols { get; set; }

        public ContextType(Dictionary<string, IType> types)
        {
            Types = types;
            Symbols = new Stack<Tuple<string, IType>>();
            Methods = new List<Method>();
        }

        public bool IsDefineSymbol(string name)
        {

            foreach (var t in Symbols)
                if (t.Item1 == name) return true;
            return false;
        }

        public bool IsDefineType(string name)
        {
            return Types.ContainsKey(name);
        }

        public void AddType(IType type)
        {
            Types[type.Name] = type;
        }

        public void DefineSymbol(string symbol, IType type)
        {
            Symbols.Push(new Tuple<string, IType>(symbol, type));
        }

        public IType GetType(string type)
        {
            if (type == "SELF_TYPE") type = ActualType.Name;
            if (IsDefineType(type))
                return Types[type];

            return null;
        }

        public IType GetTypeFor(string symbol)
        {
            foreach (var tuple in Symbols)
                if (tuple.Item1 == symbol) return tuple.Item2;

            return null; 
        }

        public void UndefineSymbol()
        {
            Symbols.Pop();
        }

        public void UndefineSymbol(int count)
        {
            if (count > 0)
            {
                UndefineSymbol();
                UndefineSymbol(--count);
            }
        }

        public bool IsDefineMethod(string name, IType type)
        {
            foreach (var item in type.AllMethods())
            {
                if (item.Name == name) return true;
            }
            return false;
        }

        public bool ThereAreMethod(string name)
        {
            foreach (var item in Methods)
            {
                if (item.Name == name) return true;
            }
            return false;
        }

        public void DefineMethod(string m, IType type)
        {
            foreach (var item in type.Methods)
            {
                if(item.Name == m)
                {
                    Methods.Add(item);
                    break;
                }
            }
        }

        public void UndefineMethods()
        {
            Methods = new List<Method>();
        }
    }
}
