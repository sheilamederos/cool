using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.CheckSemantic.Types;

namespace Logic.CheckSemantic
{
    public class ContextType
    {
        public IType ActualType { get; set; }

        public Dictionary<string, IType> Types { get; set; }

        public Stack<Tuple<string, IType>> Symbols { get; set; }

        public ContextType(Dictionary<string, IType> types)
        {
            Types = types;
            Symbols = new Stack<Tuple<string, IType>>();
        }

        public bool IsDefineSymbol(string name)
        {
            List<string> list = (List<string>)Symbols.Select<Tuple<string, IType>, string>((t) => t.Item1);
            return list.Contains(name);
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
    }
}
