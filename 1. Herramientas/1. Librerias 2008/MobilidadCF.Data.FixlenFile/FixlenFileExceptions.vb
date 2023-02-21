' Excepción lanzada cuando se hace referencia a un campo 
' que no ha sido agregado a la estructura del archivo
Public Class FieldException
    Inherits ApplicationException
    Sub New(ByVal FieldName As String, ByVal FileName As String)
        MyBase.New("El campo '" & FieldName & "' No existe en el archivo '" & FileName & "'")
    End Sub
End Class

' Excepción lanzada cuando se intenta acceder al valor de algún
' campo sin haber leido el archivo
Public Class NoRowException
    Inherits ApplicationException
    Sub New(ByVal FileName As String)
        MyBase.New("Aún no se ha leido ningún registro del archivo '" & FileName & "'")
    End Sub
End Class

' Indica que los registros del archivo no tiene la estructura esperada
Public Class FileStructureException
    Inherits ApplicationException
    Sub New(ByVal FileName As String)
        MyBase.New("El archivo '" & FileName & "' no tiene la estructura esperada!")
    End Sub
End Class

' Indica que hubo un error al leer el archivo
Public Class FileReadException
    Inherits ApplicationException
    Sub New(ByVal FileName As String)
        MyBase.New("Error leyendo registro del archivo '" & FileName & "'")
    End Sub
End Class

' Parametros de busqueda no validos
Public Class SearchParametersException
    Inherits ApplicationException
    Sub New(ByVal FileName As String)
        MyBase.New("Busqueda con parametros erroneos en archivo '" & FileName & "'")
    End Sub
End Class

Public Class RowOutRangeException
    Inherits ApplicationException
    Sub New(ByVal FileName As String)
        MyBase.New("Registro fuera de rango en archivo '" & FileName & "'")
    End Sub
End Class
