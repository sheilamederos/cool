namespace AST_CIL
{
    public class CilToMips : IVisitor
    {
        public string Data;
        public string Text;
        
        public void Accept(CIL_Program prog)
        {            
            prog.Types.Accept(this);
            prog.Data.Accept(this);
            prog.Code.Accept(this);
        }

        public void Accept(CIL_Node node)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_OneType ot)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Types myType)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Data data)
        {
            foreach (var strVar in data._stringVars)
                this.Data += strVar.Item1 + ":\t .asciiz \t" + "\"" + strVar.Item2 + "\"\n";

            foreach (var intVar in data._integerVars)
                this.Data += intVar.Item1 + ":\t .word \t" + intVar.Item2 + "\n";
        }

        public void Accept(CIL_Instruction ins)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Function function)
        {
            // andamos por aqui
            foreach (var arg in function.Args)
            {
                
            }
        }

        public void Accept(CIL_Code code)
        {
            foreach (var cilFunction in code.Funcs)
                cilFunction.Accept(this);
        }

        public void Accept(CIL_Assig assig)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Atom atom)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_MyCons myCons)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(MyVar myVar)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Concat concat)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Substring substring)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_ArithExpr arithExpr)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_GetAttr getAttr)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_SetAttr setAttr)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_VCall vCall)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Allocate allocate)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Call call)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Load load)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Length length)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Str str)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Label label)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Goto _goto)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Return _return)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Read read)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Print print)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_ConditionalJump cj)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Arg arg)
        {
            throw new System.NotImplementedException();
        }
    }
}