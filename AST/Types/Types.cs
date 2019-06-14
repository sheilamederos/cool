using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AST
{
    public class Attribute
    {
        public string Name { get; }
        public IType Type { get; }
        public string TypeSelf { get; set; }

        public Attribute(string name, IType type, string typeSelf = null)
        {
            Name = name;
            Type = type;
            TypeSelf = typeSelf;
        }
    }

    public class Method 
{
        public string Name { get; }
        public IType ReturnType { get; }
        public List<Attribute> Arguments { get; set; }
        public string TypeSelf { get; }

        public Method(string name, IType retType, string typeSelf)
        {
            Name = name;
            ReturnType = retType;
            Arguments = new List<Attribute>();
            TypeSelf = typeSelf;
        }

        public Method(string name, IType retType, List<Attribute> attr, string typeSelf)
        {
            Name = name;
            ReturnType = retType;
            Arguments = attr;
            TypeSelf = typeSelf;
        }

        public static bool Equal_Def(Method_Def mtd, Method mtd_father)
        {
            if (mtd.type.s != mtd_father.ReturnType.Name) return false;
            if (mtd.args.list_Node.Count != mtd_father.Arguments.Count) return false;
            for (int i = 0; i < mtd.args.list_Node.Count; i++)
            {
                if (mtd.args.list_Node[i].type.s != mtd_father.Arguments[i].Type.Name)
                    return false;
            }
            return true;
        }
    }

    public class IType
    {
        public IType Father { get; set; }
        public string Name { get; }
        public List<Attribute> Attributes { get; set; }
        public List<Method> Methods { get; set; }

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

        public bool Conform(IType type)
        {
            IType iter = this;
            if (type.Name == "Object") return true;
            while(iter.Name != "Object")
            {
                if (iter.Name == type.Name) return true;
                iter = iter.Father;
            }
            return false;
        }

        public List<Attribute> AllAttributes()
        {
            List<Attribute> list = new List<Attribute>();
            if (Father != null)
                list = Father.AllAttributes();

            foreach (var item in Attributes)
                list.Add(item);

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

            foreach (var item in Methods)
                list.Add(item);

            return list;
        }

        public Method GetMethod(string name)
        {
            foreach (Method mtd in AllMethods())
                if (name == mtd.Name) return mtd;

            if (Father == null) return null;

            return Father.GetMethod(name);
        }

        public IType LCA(IType other)
        {
            if (this.Conform(other)) return other;
            if (other.Conform(this)) return this;
            return this.LCA(other.Father);
        }

        public static List<IType> GetTypesBuilt_in()
        {
            List<IType> types = new List<IType>();

            IType TObject = new IType("Object", null);
            IType TSelfType = new IType("SELF_TYPE", TObject);
            IType TInt = new IType("Int", TObject);
            IType TString = new IType("String", TObject);
            IType TBool = new IType("Bool", TObject);
            IType TIO = new IType("IO", TObject);

            Method m1 = new Method("abort", TObject, "Object");
            Method m2 = new Method("type_name", TString, "Object");
            Method m3 = new Method("copy", TSelfType, "Object");

            Method m4 = new Method("length", TInt, "String");
            Method m5 = new Method("concat", TString, new List<Attribute> { new Attribute("x", TString) }, "String");
            Method m6 = new Method("substr", TString, new List<Attribute> { new Attribute("i", TInt), new Attribute("l", TInt) }, "String");

            Method m7 = new Method("out_string", TSelfType, new List<Attribute> { new Attribute("x", TString) }, "IO");
            Method m8 = new Method("out_int", TSelfType, new List<Attribute> { new Attribute("x", TInt) }, "IO");
            Method m9 = new Method("in_string", TString, "IO");
            Method m10 = new Method("in_int", TInt, "IO");

            TObject.Methods = new List<Method> { m1, m2, m3 };
            TString.Methods = new List<Method> { m4, m5, m6 };
            TIO.Methods = new List<Method> { m7, m8, m9, m10 };

            types.Add(TObject);
            types.Add(TSelfType);
            types.Add(TInt);
            types.Add(TString);
            types.Add(TBool);
            types.Add(TIO);

            return types;
        }

        public static Dictionary<string, IType> GetAllTypes(Program Ast)
        {
            List<IType> types_built_in = GetTypesBuilt_in();

            Dictionary<string, IType> types = new Dictionary<string, IType>();
            foreach (var item in types_built_in)
                types[item.Name] = item;

            foreach (Class_Def Class in Ast.list)
                types[Class.type.s] = new IType(Class.type.s, null);

            foreach (Class_Def Class in Ast.list)
            {
                if (Class.inherit_type.s == "") types[Class.type.s].Father = types["Object"];
                else types[Class.type.s].Father = types[Class.inherit_type.s];
            }

            foreach (Class_Def Class in Ast.list)
            {
                List<Attribute> attrs = new List<Attribute>();
                List<Method> mtds = new List<Method>();

                foreach (Attr_Def attr in Class.attr.list_Node)
                    attrs.Add(new Attribute(attr.name.name, types[attr.type.s], Class.type.s));

                foreach (Method_Def mtd in Class.method.list_Node)
                {
                    List<Attribute> args = new List<Attribute>();
                    foreach (Formal arg in mtd.args.list_Node)
                        args.Add(new Attribute(arg.name.name, types[arg.type.s]));

                    mtds.Add(new Method(mtd.name.name, types[mtd.type.s], args, Class.type.s));
                }

                types[Class.type.s].Attributes = attrs;
                types[Class.type.s].Methods = mtds;
            }

            return types;
        }
    }
}
