Public Class ArchivoError
    Inherits MovilidadCF.Data.FixlenFile.FixlenFile

    Public Sub New()
        MyBase.New("\Archivo.prn")
        Me.AddField("Codigo", 10, 1)
        Me.AddField("Descripcion", 20)
    End Sub

    Public Sub DeleteReg(ByVal Codigo As String)
        Dim ls As New List(Of Integer)
        Me.First()
        While (Not Me.Eof)
            If Me("Codigo") = Codigo Then
                ls.Add(Me.CurrentRow)
            End If
            Me.Next()
        End While

        For i As Integer = ls.Count - 1 To 0 Step i - 1
            Me.Delete(ls(i))
            Me.Post()
        Next
    End Sub

    Public Function Buscar(ByVal Codigo As String) As Integer
        Return Search("Codigo", Codigo)
    End Function

    Public Sub Insertar(ByVal codigo As String, ByVal descripcion As String)
        
        Me.Insert()
        Me("Codigo") = codigo
        Me("Descripcion") = descripcion
        Me.Post()

    End Sub


End Class
