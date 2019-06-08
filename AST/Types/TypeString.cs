using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Types
{
    public class TypeString : IType
    {
        static TypeString Instance;
        public TypeString() : base("string", TypeObject.GetInstance(), new List<Attribute>(), 
            new List<Method>(new Method[3] { new MethodLength(), new MethodConcat(),
                new MethodSubstr() })) { }

        public override TypeValue Default()
        {
            return new TypeValue(this, "");
        }

        public static TypeString GetInstance()
        {
            if (Instance == null)
                Instance = new TypeString();
            return Instance;
        }
    }

    public class MethodLength : Method
    {
        public MethodLength() : base("length", TypeInt.GetInstance()) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            return new TypeValue(TypeInt.GetInstance(), ((string)args[0].Value).Length);
        }
    }

    public class MethodConcat : Method
    {
        public MethodConcat() : base("concat", TypeString.GetInstance()) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            string value = (string)args[0].Value + (string)args[1].Value;
            return new TypeValue(TypeString.GetInstance(), value);
        }
    }

    public class MethodSubstr : Method
    {
        public MethodSubstr() : base("substr", TypeString.GetInstance()) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            string s = (string)args[0].Value;
            int i = (int)args[1].Value;
            int l = (int)args[2].Value;;
            return new TypeValue(TypeString.GetInstance(), s.Substring(i, l));
        }
    }
}
