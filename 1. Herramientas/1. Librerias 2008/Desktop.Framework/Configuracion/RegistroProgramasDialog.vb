Imports System.Reflection
Imports System.Transactions

Friend Class RegistroProgramasDialog

    Private Datos As New GestorConfiguracion

    Public Shared Sub Run()
        Dim frm As New RegistroProgramasDialog
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub RegistroProgramasForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dsConfig = Datos.dsConfig
        Datos.LoadProgramas()
        bsProgramas.DataSource = dsConfig
        Me.bsProgramas.DataMember = "Programas"
        Me.ProgramasDataGridView.DataSource = dsConfig
        Me.ProgramasDataGridView.DataMember = "Programas"
        Me.Refresh()
    End Sub

    Private Sub tbtnNuevoPrograma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnNuevoPrograma.Click
        If dlgOpenFile.ShowDialog() = DialogResult.OK Then
            LoadInfoAssembly(dlgOpenFile.FileName)
        End If
    End Sub

    Private Sub LoadInfoAssembly(ByVal sFileName As String)
        Try
            Dim objAssembly As Assembly = Assembly.LoadFile(sFileName)
            Dim rw As DataRow = dsConfig.Tables("Programas").NewRow
            rw("Nombre") = GetTitleAttribute(objAssembly)
            rw("Ensamblado") = System.IO.Path.GetFileName(sFileName)
            dsConfig.Tables("Programas").Rows.Add(rw)
        Catch ex As Exception
            ShowError(ex.Message)
        End Try
    End Sub

    Private Function GetTitleAttribute(ByVal objAssembly As Assembly) As String
        Dim atributes() As Object
        atributes = objAssembly.GetCustomAttributes( _
            GetType(AssemblyTitleAttribute), False)
        If atributes.Length > 0 Then
            Return CType(atributes(0), AssemblyTitleAttribute).Title
        Else
            Throw New ArgumentException("Titulo del assembly no definido")
        End If
    End Function

    Private Function GetVersionAttribute(ByVal objAssembly As Assembly) As String
        Return objAssembly.GetName().Version.ToString()
    End Function

    Private Sub tbtnEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnEliminar.Click
        If bsProgramas.Count > 0 Then
            Dim row As DataRow
            row = CType(DBUtils.GetCurrentRow(bsProgramas), DataRow)
            row.Delete()
        End If
    End Sub

    Private Sub btnGuardarCambios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardarCambios.Click
        Using t As New TransactionScope()
            Datos.UpdateProgramas()
            t.Complete()
        End Using
    End Sub
End Class
