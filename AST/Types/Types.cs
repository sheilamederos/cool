using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Types
{
    public class Attribute
    {
        public string Name { get; }
        public IType Type { get; }

        public Attribute(string name, IType type)
        {
            Name = name;
            Type = type;
        }
    }

    public abstract class Method 
{
        public string Name { get; }
        public IType ReturnType { get; }
        public List<Attribute> Arguments { get; }

        public Method(string name, IType retType)
        {
            Name = name;
            ReturnType = retType;
            Arguments = new List<Attribute>();
        }

        public Method(string name, IType retType, List<Attribute> attr)
        {
            Name = name;
            ReturnType = retType;
            Arguments = attr;
        }

        public abstract TypeValue Call(List<TypeValue> args);

    }

    public abstract class IType
    {
        public IType Father { get; }
        public string Name { get; }
        public List<Attribute> Attributes { get; }
        public List<Method> Methods { get; }

        public IType(string name, IType father)
        {
            Name = name;
            Father = father;
            Attributes = new List<Attribute>();
            Methods = new List<Method>();
        }

        public IType(string name, IType father, List<Attribute> attr, List<Method> mtd)
        {
            Name = name;
            Father = father;
            Attributes = attr;
            Methods = mtd;
        }

        public bool DefineAttribute(string name, IType type)
        {
            if (GetAttribute(name) != null) return false;

            Attributes.Add(new Attribute(name, type));

            return true; 
        }

        public bool DefineMethod(Method mtd)
        {
            if (GetMethod(mtd.Name) != null) return false;

            Methods.Add(mtd);

            return true;
        }

        public List<Attribute> AllAttributes()
        {
            List<Attribute> list = new List<Attribute>();
            if (Father != null)
                list = Father.AllAttributes();

            list.Concat<Attribute>(Attributes);
            return list;
        }

        public Attribute GetAttribute(string name)
        {
            foreach (Attribute attr in AllAttributes())
                if (name == attr.Name) return attr;

            return null;
        }

        public List<Method> AllMethods()
        {
            List<Method> list = new List<Method>();
            if (Father != null)
                list = Father.AllMethods();
            
            list.Concat<Method>(Methods);
            return list;
        }

        public Method GetMethod(string name)
        {
            foreach (Method mtd in AllMethods())
                if (name == mtd.Name) return mtd;

            return null;
        }

        public abstract TypeValue Default();
    }

    public class ComposeType : IType
    {
        object DefaultValue;

        public ComposeType(string name, IType father, object def) : base(name, father)
        {
            DefaultValue = def;
        }

        public override TypeValue Default()
        {
            return new TypeValue(this, DefaultValue);
        }
    }

    public class TypeValue
    {
        public IType Type { get; }
        public object Value { get; }

        public TypeValue(IType type, object value)
        {
            Type = type;
            Value = value;
        }
    }

}
