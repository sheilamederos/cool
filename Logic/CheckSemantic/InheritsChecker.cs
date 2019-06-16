using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AST;

namespace Logic.CheckSemantic
{
    public static class InheritsChecker
    {
        public static bool Check(Program ast, Dictionary<string, IType> Types)
        {

            foreach (var type in Types)
            {
                List<string> ancesters = new List<string>();

                IType curret_type = type.Value;

                while (true)
                {
                    if (curret_type.Name == "Object") break;

                    if (ancesters.Contains(curret_type.Name)) return false;
                    ancesters.Add(curret_type.Name);

                    curret_type = curret_type.Father;
                }
            }
            return true;
        }
    }
}
