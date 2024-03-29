﻿using System;
using System.Collections.Generic;
using System.Web;

namespace CIL
{
    public class Scope
    {
        public Scope father;
        public Dictionary<string, int> VarInStack;

        public Scope()
        {
            VarInStack = new Dictionary<string, int>();
        }
    }
    
    public class CilToMips : IVisitor
    {
        public string Data;
        public string Text;
        public Scope CurrentScope;
        
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
            Data += "\t buffer:             .space    65536 \n" +
                    "\t zero:               .byte     0 \n" +
                    "\t strsubstrexception: .asciiz \"Substring index exception\" \n";
            
            foreach (var strVar in data._stringVars.Values)
                Data += strVar + ":\t .asciiz \t" + "\"" + data._stringVars[strVar] + "\"\n";
        }

        public void Accept(CIL_Instruction ins)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Function function)
        {
            var scope = new Scope {father = CurrentScope};
            CurrentScope = scope;
            
            // Las funciones van a recivir los parametros a partir de la
            // tercera posicion del stack (-12($sp)), en las dos primeras
            // van ra (-4($sp)) y fp (-8($sp))
            // Despues de los Args van los Locals

            int i = -12;
            foreach (var arg in function.Args)
            {
                scope.VarInStack[arg] = i;
                i -= 4;
            }

            foreach (var local in function.Locals)
            {
                scope.VarInStack[local] = i;
                i -= 4;
            }
            
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

            CurrentScope = CurrentScope.father;
        }

        public void Accept(CIL_Code code)
        {
            foreach (var cilFunction in code.Funcs)
                cilFunction.Accept(this);
        }

        public void Accept(CIL_Assig assig)
        {
            if (int.TryParse(assig.RigthMem, out int n))
            {
                Text += "\t li $a0, " + n + "\n" +
                        "\t sw $a0, " + CurrentScope.VarInStack[assig.Dest] + "($fp) \n";
            }
            else
            {
                Text += "\t lw $a0, " + CurrentScope.VarInStack[assig.RigthMem] + "($fp) \n" +
                        "\t sw $a0, " + CurrentScope.VarInStack[assig.Dest] + "($fp) \n";
            }
        }

        public void Accept(CIL_Concat concat)
        {
            // -12($sp) = string a, -16($sp) = string b
            Text += "\t la $a0, " + concat.Msg1 + "\n" +
                    "\t sw $a0, -12($sp) \n" +
                    "\t la $a0, " + concat.Msg2 + "\n" +
                    "\t sw $a0, -16($sp) \n" +
                    "\t jal _stringconcat \n" +
                    "\t sw $v0, " + CurrentScope.VarInStack[concat.Dest] + "($sp) \n";
        }

        public void Accept(CIL_Substring substring)
        {
            // -12($sp) = msg, -16($sp) = int index, -20($sp) = int length
            Text += "\t la $a0, " + substring.Msg + "\n" +
                    "\t sw $a0, -12($sp) \n";

            if (int.TryParse(substring.Index, out int index))
                Text += "\t li $a0, " + index + "\n";
            else
                Text += "\t lw $a0, " + CurrentScope.VarInStack[substring.Index] + "($fp) \n";
            
            Text += "\t sw $a0, -16($sp) \n";

            if (int.TryParse(substring.Length, out int length))
                Text += "\t li $a0, " + length + "\n";
            else
                Text += "\t lw $a0, " + CurrentScope.VarInStack[substring.Length] + "($fp) \n";

            Text += "\t sw $a0, -20($sp) \n";

            Text += "\t jal _stringsubstr \n" +
                    "\t sw $v0, " + CurrentScope.VarInStack[substring.Dest] + "($sp) \n";
        }

        public void Accept(CIL_ArithExpr arithExpr)
        {
            // Calculando la direccion de memoria o el entero
            // que representa al miembro izquierdo de la operacion
            string leftMem = "";
            
            if (int.TryParse(arithExpr.LeftOp, out int n1))
                leftMem = "\t li $t1, " + n1 + "\n";
            else
                leftMem = "\t lw $t1, " + CurrentScope.VarInStack[arithExpr.LeftOp] + "($fp) \n";

            // Calculando la direccion de memoria o el entero
            // que representa al miembro derecho de la operacion
            string rigthMem = "";

            if (int.TryParse(arithExpr.RigthOp, out int n2))
                rigthMem = "\t li $t2, " + n2 + "\n";
            else
                rigthMem = "\t lw $t2, " + CurrentScope.VarInStack[arithExpr.RigthOp] + "($fp) \n";

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
                    "\t sw $t0, " + CurrentScope.VarInStack[arithExpr.Dest] + "($fp) \n";
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
            int i = -12;
            
            foreach (var arg in call.Args)
            {
                if (int.TryParse(arg, out int n))
                {
                    Text += "\t li $a0, " + n + "\n" +
                            "\t sw $a0, " + i + "($sp) \n";
                }
                else
                {
                    Text += "\t lw $a0, " + CurrentScope.VarInStack[arg] + "($fp) \n" +
                            "\t sw $a0, " + i + "($sp) \n";
                }

                i -= 4;
            }

            Text += "\t jal " + call.Name + "\n" +
                    "\t sw $v0, " + CurrentScope.VarInStack[call.Dest] + "($fp) \n";
        }

        public void Accept(CIL_Load load)
        {
            Text += "\t la $t0, " + load.Msg + "\n" +
                    "\t sw $t0, " + CurrentScope.VarInStack[load.Dest] + "($fp) \n";
        }

        public void Accept(CIL_Length length)
        {
            // -12($sp) = string a
            Text += "\t la $a0, " + length.Msg + "\n" +
                    "\t sw $a0, -12($sp) \n" +
                    "\t jal _stringlength \n" +
                    "\t sw $v0, " + CurrentScope.VarInStack[length.Dest] + "($sp) \n";
        }

        public void Accept(CIL_Str str)
        {
            throw new System.NotImplementedException();
        }

        public void Accept(CIL_Label label)
        {
            Text += "$" + label._label + ": \n";
        }

        public void Accept(CIL_Goto _goto)
        {
            Text += "\t j " + _goto._label + "\n";
        }

        public void Accept(CIL_Return _return)
        {
            if (int.TryParse(_return.value, out int n))
            {
                Text += "\t li $v0, " + n + "\n";
            }
            else
            {
                Text += "\t lw $v0, " + CurrentScope.VarInStack[_return.value] + "($fp) \n";
            }
        }

        // TODO: me quede por aqui, hacer los diferentes reads y prints
        public void Accept(CIL_Read read)
        {
        }

        public void Accept(CIL_Print_Int print)
        {
            throw new NotImplementedException();
        }

        public void Accept(CIL_Print_Str print)
        {
            throw new NotImplementedException();
        }

        public void Accept(CIL_ConditionalJump cj)
        {
            Text += "\t lw $t0, " + CurrentScope.VarInStack[cj.cond] + "($fp) \n" +
                    "\t bne $t0, $r0, $" + cj.label + "\n";
        }
    }
}