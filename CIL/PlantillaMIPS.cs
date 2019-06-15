namespace CIL
{
    public class PlantillaMIPS
    {
        // -12($sp) = string a
        public const string StringLength = 
@"_stringlength:

    sw $ra, -4($sp)
    sw $fp, -8($sp)
    subu $sp, $sp, 12
    addu $fp, $sp, 12

    lw $a0, -12($fp)
    li $v0, 0
_stringlength.loop:
    lb $a1, 0($a0)
    beqz $a1, _stringlength.end
    addiu $a0, $a0, 1
    addiu $v0, $v0, 1
    j _stringlength.loop
_stringlength.end:

    lw $ra, -4($fp)
    lw $fp, -8($fp)
    addu $sp, $sp, 12

    jr $ra";

        // -12($sp) = string a, -16($sp) = string b
        public const string StringConcat =
@"_stringconcat:

    sw $ra, -4($sp)
    sw $fp, -8($sp)
    subu $sp, $sp, 16
    addu $fp, $sp, 16

    lw $a0, -12($fp)
    sw $a0, -12($sp)
    jal _stringlength
    move $v1, $v0

    lw $a0, -16($fp)
    sw $a0, -16($sp)
    jal _stringlength
    add $v1, $v1, $v0
    addi $v1, $v1, 1
    li $v0, 9
    move $a0, $v1
    syscall
    move $v1, $v0
    lw $a0, -12($fp)
_stringconcat.loop1:
    lb $a1, 0($a0)
    beqz $a1, _stringconcat.end1
    sb $a1, 0($v1)
    addiu $a0, $a0, 1
    addiu $v1, $v1, 1
    j _stringconcat.loop1
_stringconcat.end1:
    lw $a0, -16($fp)
_stringconcat.loop2:
    lb $a1, 0($a0)
    beqz $a1, _stringconcat.end2
    sb $a1, 0($v1)
    addiu $a0, $a0, 1
    addiu $v1, $v1, 1
    j _stringconcat.loop2
_stringconcat.end2:
    sb $zero, 0($v1)

    lw $ra, -4($fp)
    lw $fp, -8($fp)
    addu $sp, $sp, 16

    jr $ra";

        // -12($sp) = msg, -16($sp) = int a, -20($sp) = int b
        public const string StringSubstring =
@"_stringsubstr:

    sw $ra, -4($sp)
    sw $fp, -8($sp)
    subu $sp, $sp, 20
    addu $fp, $sp, 20

    lw $a0, -20($sp)
    addiu $a0, $a0, 1
    li $v0, 9
    syscall
    move $v1, $v0
    lw $a0, -12($sp)
    lw $a1, -16($sp)
    add $a0, $a0, $a1
    lw $a2, -20($sp)
_stringsubstr.loop:
    beqz $a2, _stringsubstr.end
    lb $a1, 0($a0)
    beqz $a1, _substrexception
    sb $a1, 0($v1)
    addiu $a0, $a0, 1
    addiu $v1, $v1, 1
    addiu $a2, $a2, -1
    j _stringsubstr.loop
_stringsubstr.end:
    sb $zero, 0($v1)

    lw $ra, -4($fp)
    lw $fp, -8($fp)
    addu $sp, $sp, 20

    jr $ra
            
_substrexception:
    la $a0, strsubstrexception
    li $v0, 4
    syscall
    li $v0, 10
    syscall";

        public const string InputString = 
@"_in_string:
    move $a3, $ra
    la $a0, buffer
    li $a1, 65536
    li $v0, 8
    syscall
    addiu $sp, $sp, -4
    sw $a0, 0($sp)
    jal String.length
    addiu $sp, $sp, 4
    move $a2, $v0
    addiu $a2, $a2, -1
    move $a0, $v0
    li $v0, 9
    syscall
    move $v1, $v0
    la $a0, buffer
_in_string.loop:
    beqz $a2, _in_string.end
    lb $a1, 0($a0)
    sb $a1, 0($v1)
    addiu $a0, $a0, 1
    addiu $v1, $v1, 1
    addiu $a2, $a2, -1
    j _in_string.loop
_in_string.end:
    sb $zero, 0($v1)
    move $ra, $a3
    jr $ra";
    }
}