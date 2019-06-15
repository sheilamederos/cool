using System;
using System.Web;

namespace AST_CIL
{
    public class CilToMips : IVisitor
    {
        public string Data;
        public string Text;

        private static int _buffer = 0;

//        private string GetBuffer()
//        {
//            var result = "buffer" + _buffer;
//            _buffer++;
//            return result;
//        }
        
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
            Data += "\t buffer: .space 65536 \n";
            
            foreach (var strVar in data._stringVars)
                Data += strVar.Item1 + ":\t .asciiz \t" + "\"" + strVar.Item2 + "\"\n";
        }

        public void Accept(CIL_Instruction ins)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Function function)
        {
            // Las funciones van a recivir los parametros a partir de la
            // tercera posicion del stack (-12($sp)), en las dos primeras
            // van ra (-4($sp)) y fp (-8($sp))
            // Despues de los Args van los Locals
            int n = 4 * (function.Args.Count + function.Locals.Count + 2); 

            Text += function.Name + ":\n";

            Text += "\t sw $ra, -4($sp) \n" +
                    "\t sw $fp, -8($sp) \n" +
                    "\t subu $sp, $sp, " + n + "\n" +
                    "\t addu $fp, $sp, " + n + "\n";

            foreach (var inst in function.Instructions)
                inst.Accept(this);
            
            Text += "\t lw $ra, -4($fp) \n" +
                    "\t lw $fp, -8($fp) \n" +
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
            // TODO: Revisar esto con Ale
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
            // TODO: La misma
            int desPos = 4 * (arithExpr.Dest.Id + 1);

            // Calculando la direccion de memoria o el entero
            // que representa al miembro izquierdo de la operacion
            string leftMem = "";
            
            if (arithExpr.LeftOp is CIL_MyVar myVar1)
            {
                leftMem = 4 * (myVar1.Id + 1) + "($sp)";
                leftMem = "\t lw $t1, " + leftMem + "\n";
            }
            else
                leftMem = "\t li $t1, " + ((CIL_MyCons) arithExpr.LeftOp).Value + "\n";

            // Calculando la direccion de memoria o el entero
            // que representa al miembro derecho de la operacion
            string rigthMem = "";

            if (arithExpr.RigthOp is CIL_MyVar myVar2)
            {
                rigthMem = 4 * (myVar2.Id + 1) + "($sp)";
                rigthMem = "\t lw $t2, " + rigthMem + "\n";
            }
            else
                rigthMem = "\t li $t2, " + ((CIL_MyCons) arithExpr.RigthOp).Value + "\n";

            string operation = "";
            switch (arithExpr.Op)
            {
                    case "+":
                        operation = "\t add $t0, $t1, $t2 \n";
                        break;
                    case "-":
                        operation = "\t sub $t0, $t1, $t2 \n";
                        break;
                    case "*":
                        operation = "\t mulo $t0, $t1, $t2 \n";
                        break;
                    case "/":
                        operation = "\t div $t0, $t1, $t2 \n";
                        break;
                    default:
                        throw new InvalidOperationException("Operacion " + arithExpr.Op + " no definida");
            }

            Text += leftMem +
                    rigthMem +
                    operation +
                    "\t sw $t0, " + desPos + "($sp) \n";
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
            // Ponerle al call la lista de argumentos
            Text += "\t jal " + call.MyFunc.Name + "\n" +
                    "\t sw $v0, " + 4 * (call.Dest.Id + 1) + "($sp) \n";
        }

        public void Accept(CIL_Load load)
        {
            int posDes = 4 * (load.Dest.Id + 1);
            int posMsg = 4 * (load.Msg.Id + 1);

            Text += "\t lw $t0, " + posMsg + "($sp) \n" +
                    "\t sw $t0, " + posDes + "($sp) \n";
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
            Text += "$" + label._label + "\n";
        }

        public void Accept(CIL_Goto _goto)
        {
            Text += "\t j " + _goto._label + "\n";
        }

        public void Accept(CIL_Return _return)
        {
            if (_return._atom == null) return;
            
            if (_return._atom is CIL_MyVar myVar)
            {
                Text += "\t lw $v0, " + 4 * (myVar.Id + 1) + "($sp) \n";
            }
            else
            {
                Text += "\t li $v0, " + ((CIL_MyCons) _return._atom).Value + "\n";
            }
        }

        public void Accept(CIL_Read read)
        {
            // modificar este buffer
            string buffer = "asdasdasd";
            Data += "\t " + buffer + ": .space 65536 \n";
            
            // Creo q va a ser un problema leer siempre con el mismo buffer
            Text += "\t la $a0, " + buffer + " \n" +
                    "\t li $a1, 65536 \n" +
                    "\t li $v0, 8 \n" +
                    "\t syscall \n" +
                    "\t sw $a0, " + 4 * (read._var.Id + 1) + "($sp)";
        }

        public void Accept(CIL_Print print)
        {
            Text += "\t lw $a0, " + 4 * (print._var.Id + 1) + "($sp) \n" +
                    "\t li $v0, 4 \n" +
                    "\t syscall \n";
        }

        public void Accept(CIL_ConditionalJump cj)
        {
            Text += "\t lw $t0, " + 4 * (cj.ConditionVar.Id + 1) + "($sp) \n" +
                    "\t bne $t0, $r0, $" + cj.Label + "\n";
        }

        public void Accept(CIL_Arg arg)
        {
            throw new System.NotImplementedException();
        }
    }
}