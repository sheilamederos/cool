# <p align="center">Especificaciones de MIPS

Las propiedades `Args` y `Locals` de la clase `CIL_Function` son dos diccionarios de `string` contra `int`. En las llaves van los nombres de las variables y en los valores van identificadores que comienzan en 0 e incrimentan de 1 en 1.
Los valores de ambas propiedades estan unos a continuacion de otros

La clase `CIL_OneType` representa los tipos de cool. Tiene una propiedad `Attributes` donde van las propiedas de todos sus padres y las suyas propias en orden, empezando por las de sus ancestros mas lejanos y terminando en las de ellos. Lo mismo para la propiedad `Methods` lo que en esta se guardan los metodos del tipo y de todos sus ancestros en el mismo orden.

Mes estoy planteando si en realidad es necesario un tipo Atom del cual derivan Variable y Constante. Si son necesarios para que las expresiones aritmeticas recivan dos operandos que sean del tipo Atom

Las variables las voy a colocar en la pila en la posicion `4 * (var.ID + 1)` de el registro `sp`

## TODO: como toletes concateno 2 strings, el substring y el str

Bueno para concatenar y para los substring hacerlo con cilos recorriendo las cadenas

Es necesario usar operaciones unsigned en MIPS para el compilador de cool????