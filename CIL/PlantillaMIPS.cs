namespace CIL
{
    public class PlantillaMIPS
    {
        public static string StringLength = @"_stringlength:
                                                \t lw $a0, 0($sp)
                                              _stringlength.loop:
                                                \t lb $a1, 0($a0)
                                                \t beqz $a1, _stringlength.end
                                                \t addiu $a0, $a0, 1
                                                \t j _stringlength.loop
                                              _stringlength.end:
                                                \t lw $a1, 0($sp)
                                                \t subu $v0, $a0, $a1
                                                \t jr $ra";
    }
}