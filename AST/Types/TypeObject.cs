using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Types
{
    public class TypeObject : IType
    {
        static TypeObject Instance;

        public TypeObject() : base("object", null, new List<Attribute>(), 
            new List<Method>(new Method[3] { new MethodAbort(),
                new MethodTypeName(), new MethodCopy() })) { }

        public static TypeObject GetInstance()
        {
            if (Instance == null)
                Instance = new TypeObject();
            return Instance;
        }
        
        public override TypeValue Default()
        {
            return null;
        }
    }

    public class MethodAbort : Method
    {
        public MethodAbort() : base("abort", TypeObject.GetInstance()) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            throw new NotImplementedException();
        }
    }

    public class MethodTypeName : Method
    {
        public MethodTypeName() : base("type_name", TypeString.GetInstance()) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            return new TypeValue(TypeString.GetInstance(), args[0].Type.Name);
        }
    }

    public class MethodCopy : Method
    {
        public MethodCopy() : base("copy", TypeSelf.GetInstance(TypeObject.GetInstance())) { }

        public override TypeValue Call(List<TypeValue> args)
        {
            return new TypeValue(TypeSelf.GetInstance(args[0].Type), args[0].Value);
        }
    }
}
