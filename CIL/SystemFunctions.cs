using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIL
{
    public static class SystemFunctions
    {
        public static CIL_Function Enter_main()
        {
            List<string> args = new List<string>();
            List<string> locals = new List<string>();
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Allocate("this", "Main"));
            inst.Add(new CIL_Call("this", "main", new List<string> { "this" }));
            inst.Add(new CIL_Return("this"));

            return new CIL_Function("enter_main", args, locals, inst);
        }

        #region IO
        public static CIL_Function Fun_out_string()
        {
            List<string> args = new List<string> { "this", "x" };
            List<string> locals = new List<string>();
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Print_Str("x"));
            inst.Add(new CIL_Return("this"));

            return new CIL_Function("IO_out_string", args, locals, inst);
        }

        public static CIL_Function Fun_out_int()
        {
            List<string> args = new List<string> { "this", "x" };
            List<string> locals = new List<string>();
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Print_Int("x"));
            inst.Add(new CIL_Return("this"));

            return new CIL_Function("IO_out_int", args, locals, inst);
        }

        public static CIL_Function Fun_in_string()
        {
            List<string> args = new List<string> { "this" };
            List<string> locals = new List<string>();
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Read("this"));
            inst.Add(new CIL_Return("this"));

            return new CIL_Function("IO_in_string", args, locals, inst);
        }

        public static CIL_Function Fun_in_int()
        {
            List<string> args = new List<string>();
            List<string> locals = new List<string>();
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Read("this"));   //Ver si poner 2 nodos del AST de read(str,int)
            inst.Add(new CIL_Return("this"));

            return new CIL_Function("IO_in_int", args, locals, inst);
        }
        #endregion

        #region String
        public static CIL_Function Fun_concat()
        {
            List<string> args = new List<string> { "this", "s" };
            List<string> locals = new List<string> { "local_string_concat_ret" };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Concat("local_string_concat_ret", "this", "s"));
            inst.Add(new CIL_Return("local_string_concat_ret"));

            return new CIL_Function("String_concat", args, locals, inst);
        }

        public static CIL_Function Fun_length()
        {
            List<string> args = new List<string> { "this" };
            List<string> locals = new List<string> { "local_string_length_ret" };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Length("local_string_length_ret", "this"));
            inst.Add(new CIL_Return("local_string_length_ret"));

            return new CIL_Function("String_length", args, locals, inst);
        }

        public static CIL_Function Fun_substr()
        {
            List<string> args = new List<string> { "this", "i", "l" };
            List<string> locals = new List<string> { "local_string_substr_ret" };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Substring("local_string_substr_ret", "this", "i", "l"));

            return new CIL_Function("String_substr", args, locals, inst);
        }

        #endregion

        #region Object

        public static CIL_Function Fun_type_name()
        {
            List<string> args = new List<string> { "this" };
            List<string> locals = new List<string> { "local_object_type_name_ret" };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            inst.Add(new CIL_Typeof("local_object_type_name_ret", "this"));
            inst.Add(new CIL_Return("local_object_type_name_ret"));

            return new CIL_Function("Object_type_name", args, locals, inst);
        }

        public static CIL_Function Fun_abort()
        {
            List<string> args = new List<string> { "this" };
            List<string> locals = new List<string> { };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            //inst.Add(new CIL_Halt());
            inst.Add(new CIL_Return(""));

            return new CIL_Function("Object_abort", args, locals, inst);
        }

        public static CIL_Function Fun_copy()
        {
            List<string> args = new List<string> { "this" };
            List<string> locals = new List<string> { "local_object_copy_ret" };
            List<CIL_Instruction> inst = new List<CIL_Instruction>();

            //inst.Add(new CIL_Copy("local_object_copy_ret", "this"));
            inst.Add(new CIL_Return("local_object_copy_ret"));

            return new CIL_Function("Object_copy", args, locals, inst);
        }

        #endregion

    }
}
