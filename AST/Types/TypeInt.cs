using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Types
{
    public class TypeInt : IType
    {
        static TypeInt Instance;

        public TypeInt() : base("int", TypeObject.GetInstance()) { }

        public static TypeInt GetInstance()
        {
            if (Instance == null) Instance = new TypeInt();
            return Instance;
        }

        public override TypeValue Default()
        {
            return new TypeValue(this, 0);
        }
    }
}
