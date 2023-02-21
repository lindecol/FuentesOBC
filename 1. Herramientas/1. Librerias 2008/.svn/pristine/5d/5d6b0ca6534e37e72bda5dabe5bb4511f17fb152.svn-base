Imports System.Windows.Forms

Public Class DataPreviewDialog

    Private m_dtData As DataTable
    Private m_dtEsquema As DataTable

    Public Shared Sub Run(ByVal dtData As DataTable)
        Dim frm As New DataPreviewDialog
        frm.m_dtData = dtData
        frm.ShowDialog()
        frm.m_dtData = dtData
        frm.Dispose()
    End Sub

    Private Sub DataPreviewDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        dgvData.DataSource = m_dtData
        LoadSchemaData()
    End Sub

    Private Sub LoadSchemaData()
        m_dtEsquema = New DataTable
        m_dtEsquema.Columns.Add("Nombre Columna")
        m_dtEsquema.Columns.Add("Tipo de dato")
        m_dtEsquema.Columns.Add("Long. máxima")
        Dim col As DataColumn
        For I As Integer = 0 To m_dtData.Columns.Count - 1
            col = m_dtData.Columns(I)
            m_dtEsquema.Rows.Add(col.ColumnName, col.DataType.ToString(), col.MaxLength)
        Next
    End Sub

    Private Sub btnCerrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCerrar.Click
        DialogResult = System.Windows.Forms.DialogResult.OK
    End Sub

    Private Sub rbtnVerDatos_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnVerDatos.CheckedChanged
        If rbtnVerDatos.Checked Then
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            dgvData.DataSource = m_dtData
        End If
    End Sub

    Private Sub rbtnVerEsquema_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtnVerEsquema.CheckedChanged
        If rbtnVerEsquema.Checked Then
            dgvData.DataSource = m_dtEsquema
            dgvData.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader
        End If
    End Sub
End Class
