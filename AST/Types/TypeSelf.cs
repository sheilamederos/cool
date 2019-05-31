using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST.Types
{
    public class TypeSelf: IType
    {
        IType Self { get; }

        static TypeSelf Instance;

        public TypeSelf(IType self) : base("self_type", self.Father)
        {
            Self = self;
        }

        public static TypeSelf GetInstance(IType self)
        {
            if (Instance == null || Instance.Self.Name != self.Name)
                Instance = new TypeSelf(self);
            return Instance;
        }

        public override TypeValue Default()
        {
            return Self.Default();
        }
    }
}
