using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;
using AST.Types;

namespace Logic.CheckSemantic
{
    public interface IContext
    {
        IType GetType(string type);
        IType GetTypeFor(string symbol);
        IContext CreateChildContext();
        bool DefineSymbol(string symbol, IType type);
        IType CreateType(string name, IType father);
    }

    public class ContextType : IContext
    {
        ContextType Father { get; set; }

        List<IType> Types { get; set; }

        Dictionary<string, IType> Symbols { get; set; }

        public ContextType(ContextType father)
        {
            Father = father;
            Types = new List<IType>();
            Symbols = new Dictionary<string, IType>();
        }

        public IContext CreateChildContext()
        {
            return new ContextType(this);
        }

        public IType CreateType(string name, IType father)
        {
            IType type = new ComposeType(name, father);

            Types.Add(type);

            return type;
        }

        public bool DefineSymbol(string symbol, IType type)
        {
            if (!Symbols.ContainsKey(symbol)) return false;

            if (!Types.Contains(type)) return false;

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
            if (!Symbols.ContainsKey(symbol)) return null;

            return Symbols[symbol];
        }
    }
}
