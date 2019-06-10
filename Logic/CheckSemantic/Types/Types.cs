﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.CheckSemantic.Types
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
        public IType TypeSelf { get; }

        public Method(string name, IType retType, IType typeSelf)
        {
            Name = name;
            ReturnType = retType;
            Arguments = new List<Attribute>();
            TypeSelf = typeSelf;
        }

        public Method(string name, IType retType, List<Attribute> attr, IType typeSelf)
        {
            Name = name;
            ReturnType = retType;
            Arguments = attr;
            TypeSelf = typeSelf;
        }
    }

    public class IType
    {
        public IType Father { get; }
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
            IType TSelfType = new IType("SelfType", TObject);
            IType TInt = new IType("Int", TObject);
            IType TString = new IType("String", TObject);
            IType TBool = new IType("Bool", TObject);
            IType TIO = new IType("IO", TObject);

            Method m1 = new Method("abort", TObject, TObject);
            Method m2 = new Method("type_name", TString, TObject);
            Method m3 = new Method("copy", TSelfType, TObject);

            Method m4 = new Method("length", TInt, TString);
            Method m5 = new Method("concat", TString, new List<Attribute> { new Attribute("x", TString) }, TString);
            Method m6 = new Method("substr", TString, new List<Attribute> { new Attribute("i", TInt), new Attribute("l", TInt) }, TString);

            Method m7 = new Method("out_string", TSelfType, new List<Attribute> { new Attribute("x", TString) }, TIO);
            Method m8 = new Method("out_int", TSelfType, new List<Attribute> { new Attribute("x", TInt) }, TIO);
            Method m9 = new Method("in_string", TString, TIO);
            Method m10 = new Method("in_int", TInt, TIO);

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


    }
}
