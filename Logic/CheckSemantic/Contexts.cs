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

        public List<IType> Types { get; set; }

        public Stack<Tuple<string, IType>> Symbols { get; set; }

        public ContextType(List<IType> list)
        {
            Types = list;
            Symbols = new Stack<Tuple<string, IType>>();
        }

        public bool IsDefineSymbol(string name)
        {
            List<string> list = (List<string>)Symbols.Select<Tuple<string, IType>, string>((t) => t.Item1);
            return list.Contains(name);
        }

        public bool IsDefineType(string name)
        {
            return Types.Select<IType, string>((t) => t.Name).Contains(name);
        }

        public void AddType(IType type)
        {
            Types.Add(type);
        }

        public void DefineSymbol(string symbol, IType type)
        {
            Symbols.Push(new Tuple<string, IType>(symbol, type));
        }

        public IType GetType(string type)
        {
            foreach (IType t in Types)
                if (t.Name == type) return t;

            return null;
        }

        public IType GetTypeFor(string symbol)
        {
            foreach (var tuple in Symbols)
                if (tuple.Item1 == symbol) return tuple.Item2;

            return null; 
        }
    }
}
