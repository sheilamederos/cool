using System;

namespace AST_CIL
{
    public interface IVisitor
    {
        void Accept(CIL_Program prog);
        
        void Accept(CIL_Node node);

        void Accept(CIL_OneType ot);
        
        void Accept(CIL_Types myType);

        void Accept(CIL_Data data);

        void Accept(CIL_Instruction ins);

        void Accept(CIL_Function function);

        void Accept(CIL_Code code);

        void Accept(CIL_Assig assig);

        void Accept(CIL_Atom atom);

        void Accept(CIL_MyCons myCons);

        void Accept(CIL_MyVar myVar);

        void Accept(CIL_Concat concat);

        void Accept(CIL_Substring substring);

        void Accept(CIL_ArithExpr arithExpr);

        void Accept(CIL_GetAttr getAttr);

        void Accept(CIL_SetAttr setAttr);

        void Accept(CIL_VCall vCall);

        void Accept(CIL_Allocate allocate);

        void Accept(CIL_Call call);

        void Accept(CIL_Load load);

        void Accept(CIL_Length length);

        void Accept(CIL_Str str);

        void Accept(CIL_Label label);

        void Accept(CIL_Goto _goto);

        void Accept(CIL_Return _return);

        void Accept(CIL_Read read);

        void Accept(CIL_Print print);

        void Accept(CIL_ConditionalJump cj);

        void Accept(CIL_Arg arg);
    }
}