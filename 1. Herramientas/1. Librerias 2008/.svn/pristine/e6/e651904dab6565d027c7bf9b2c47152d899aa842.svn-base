Imports System.IO

Friend Class EmpresasForm

    Private m_rowEmpresa As DataRow
    Private Datos As New GestorConfiguracion

    Public Shared Sub Run()
        Dim frm As New EmpresasForm
        'CREAR EL GESTOR
        frm.ShowDialog()
        frm.Dispose()
    End Sub


    Private Sub EmpresasForm_DeleteCurrent(ByVal row As System.Data.DataRow) Handles Me.DeleteCurrent
        m_rowEmpresa = CType(row, DataRow)
        Datos.DeleteUsuariosEmpresas(CInt(Me.RowDelete("IdEmpresa")))
        Datos.UpdateEmpresas()
        If Not FrameworkApp.MultiEmpresa Then
            If Me.bsEmpresas.Count = 1 Then
                Me.bsEmpresas.AllowNew = False
                Me.CanInsert = False
            Else
                Me.bsEmpresas.AllowNew = True
                Me.CanInsert = True
            End If
            Me.Refresh()
        End If
    End Sub

    Private Sub EmpresasForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not DBUtils.GetCurrentRow(bsEmpresas) Is Nothing Then
            If DBUtils.GetCurrentRow(bsEmpresas).RowState <> DataRowState.Unchanged Then
                If Not Confirmar("Hay datos modificados, si cierra estos cambios se perderan." & _
                    vbCrLf & vbCrLf & "¿Esta seguro de cerrar?") Then
                    e.Cancel = True
                End If
            End If
        End If
    End Sub

    Private Sub EmpresasForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadData()
        If Not FrameworkApp.MultiEmpresa Then
            If Me.bsEmpresas.Count = 1 Then
                Me.bsEmpresas.AllowNew = False
                Me.CanInsert = False
            End If
        End If
    End Sub

    Private Sub LoadData()
        dsConfiguracion = Datos.dsConfig
        bsEmpresas.DataSource = dsConfiguracion
        Datos.LoadEmpresas()
        Me.bsEmpresas.DataMember = "Empresas"
        Me.NombreCompletoTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "NombreCompleto", True))
        Me.NombreCortoTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "NombreCorto", True))
        Me.DireccionTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "Direccion", True))
        Me.CiudadTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "Ciudad", True))
        Me.PaisTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "Pais", True))
        Me.pcImagen.DataBindings.Add(New System.Windows.Forms.Binding("Image", Me.bsEmpresas, "Logo", True))
        Me.NITTextBox.DataBindings.Add(New System.Windows.Forms.Binding("Text", Me.bsEmpresas, "NIT", True))
        If dsConfiguracion.Tables("Empresas").Rows.Count = 0 Then
            bsEmpresas.AddNew()
        End If
        Me.CargaDatos = True
    End Sub

    Private Sub EmpresasForm_SaveAll(ByVal dtChanges As System.Data.DataTable) Handles MyBase.SaveAll
        Datos.UpdateEmpresas(dtChanges)
        If Not FrameworkApp.MultiEmpresa Then
            If Me.bsEmpresas.Count = 1 Then
                Me.bsEmpresas.AllowNew = False
                Me.CanInsert = False
                Me.Refresh()
            End If
        End If
    End Sub

    Private Sub EmpresasForm_SaveCurrent(ByVal row As System.Data.DataRow) Handles Me.SaveCurrent
        m_rowEmpresa = row

        If txtLogo.Text <> "" Then
            Dim fi As New FileInfo(txtLogo.Text)
            Using fs As FileStream = File.OpenRead(txtLogo.Text)
                Dim br As New BinaryReader(fs)
                m_rowEmpresa("Logo") = br.ReadBytes(CInt(fi.Length))
            End Using
        End If

        Datos.UpdateEmpresas()
        Datos.dsConfig.Empresas.Clear()
        Datos.LoadEmpresas()
        If Not FrameworkApp.MultiEmpresa Then
            If Me.bsEmpresas.Count = 1 Then
                Me.bsEmpresas.AllowNew = False
                Me.CanInsert = False
                Me.Refresh()
            End If
        End If
    End Sub

    Private Sub btnExaminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExaminar.Click
        Dim oFile As New OpenFileDialog

        If oFile.ShowDialog() = DialogResult.OK Then
            txtLogo.Text = oFile.FileName
        End If

        Try
            If Not File.Exists(txtLogo.Text) Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        Dim fi As New FileInfo(txtLogo.Text)
        Using fs As FileStream = File.OpenRead(txtLogo.Text)
            Dim br As New BinaryReader(fs)
            pcImagen.ImageLocation = txtLogo.Text
        End Using
    End Sub
End Class