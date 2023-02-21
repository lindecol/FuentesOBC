Public Enum TiposModulo
    ModuloNucleo = 0
    ModuloPrograma = 1
    ModuloWeb = 2
    ModuloMovil = 3
End Enum

Public Class PermisoEspecial
    Public IdPermiso As Integer
    Public Descripcion As String

    Public Sub New()
        IdPermiso = -1
        Descripcion = ""
    End Sub

    Public Sub New(ByVal IdPermiso As Integer, ByVal Descripcion As String)
        Me.IdPermiso = IdPermiso
        Me.Descripcion = Descripcion
    End Sub

    Public Overrides Function ToString() As String
        Return Descripcion
    End Function
End Class

Public Enum TiposBases As Integer
    SQLServer = 0
    SQLMobile = 1
End Enum