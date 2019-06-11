using System.Web;

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
        }

        public void Accept(CIL_Instruction ins)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Function function)
        {
            // TODO:calcualar este n, es la cantidad de espacios que voy a reservar para esta funcion en el stack
            int n = 0; 

            Text += function.Name + ":\n";

            Text += "\t subu $sp, $sp, " + n + "\n" +
                    "\t sw $ra, 20($sp) \n" +
                    "\t sw $fp, 16($sp) \n" +
                    "\t addu $fp, $sp, " + n + "\n";

            foreach (var inst in function.Instructions)
                inst.Accept(this);
            
            Text += "\t lw $ra, 20($sp) \n" +
                    "\t lw $fp, 16($sp) \n" +
                    "\t addu $sp, $sp, " + n + "\n" +
                    "\t j $ra \n"; // aqui por el momento voy a poner j pero puede ser jr
        }

        public void Accept(CIL_Code code)
        {
            foreach (var cilFunction in code.Funcs)
                cilFunction.Accept(this);
        }

        public void Accept(CIL_Assig assig)
        {
            int destPos = 4 * (assig.Dest.Id + 1);

            if (assig.RigthMem is CIL_MyVar rigthMem)
            {
                int rigthPos = 4 * (rigthMem.Id + 1);
                
                Text += "\t sw " + rigthPos + "($sp), " + destPos + "($sp) \n";
            }
            else
            {
                Text += "\t sw " + ((CIL_MyCons) assig.RigthMem).Value + ", " + destPos + "($sp) \n";
            }
        }

        public void Accept(CIL_Atom atom)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_MyCons myCons)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_MyVar myVar)
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
            int desPos = 4 * (arithExpr.Dest.Id + 1);

            // 
            string leftMem = "";
            
            if (arithExpr.LeftOp is CIL_MyVar myVar1)
            {
                leftMem = 4 * (myVar1.Id + 1) + "($sp)";
            }
            else
            {
                leftMem = ((CIL_MyCons) arithExpr.LeftOp).Value.ToString();
            }

            string rigthMem = "";

            if (arithExpr.RigthOp is CIL_MyVar myVar2)
            {
                rigthMem = 4 * (myVar2.Id + 1) + "($sp)";
            }
            else
            {
                rigthMem = ((CIL_MyCons) arithExpr.RigthOp).Value.ToString();
            }
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