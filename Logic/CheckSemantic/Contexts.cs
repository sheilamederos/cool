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
        public ContextType Father { get; set; }

        public IType ActualType { get; set; }

        public List<IType> Types { get; set; }

       public  Dictionary<string, IType> Symbols { get; set; }

        public ContextType(ContextType father)
        {
            Father = father;
            Types = new List<IType>();
            Symbols = new Dictionary<string, IType>();
        }

        public ContextType CreateChildContext()
        {
            return new ContextType(this);
        }

        public bool IsDefineSymbol(string name)
        {
            return Symbols.ContainsKey(name);
        }

        public bool IsDefineType(string name)
        {
            return Types.Select<IType, string>((t) => t.Name).Contains(name);
        }

        public IType CreateType(string name, IType father)
        {
            IType type = new IType(name, father);

            Types.Add(type);

            return type;
        }

        public bool DefineSymbol(string symbol, IType type)
        {
            Symbols[symbol] = type;

            return true;
        }

        public IType GetType(string type)
        {
            foreach (IType t in Types)
                if (t.Name == type) return t;

            return null;
        }

        public IType GetTypeFor(string symbol)
        {
            return Symbols[symbol];
        }
    }
}
