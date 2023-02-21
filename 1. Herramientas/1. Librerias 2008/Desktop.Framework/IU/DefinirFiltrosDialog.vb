Imports System.Windows.Forms

Public Class DefinirFiltrosDialog

    Private m_bsOrigen As BindingSource
    Private m_dvDatos As DataView
    Private m_dtFiltros As DataTable

    Public Shared Sub Run(ByVal bs As BindingSource)
        Dim dvDatos As DataView
        Try
            dvDatos = CType(bs.List, DataView)
        Catch ex As Exception
            ShowError("Definir Filtro: BindingSource debe estar conectado a una DataTable")
        End Try
        Dim frm As New DefinirFiltrosDialog
        frm.m_bsOrigen = bs
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub DefinirFiltrosDialog_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: esta línea de código carga datos en la tabla 'ProductosDataSet.Inv_Productos' Puede moverla o quitarla según sea necesario.
        CrearTablaFiltros()
        LlenarCamposFiltro()
        dgvFiltros.DataSource = m_dtFiltros
    End Sub

    Private Sub CrearTablaFiltros()
        m_dtFiltros = New DataTable
        m_dtFiltros.Columns.Add("Campo")
        m_dtFiltros.Columns.Add("Comparacion")
        m_dtFiltros.Columns.Add("Valor")
    End Sub

    Private Sub LlenarCamposFiltro()
        m_dvDatos = CType(m_bsOrigen.List, DataView)
        Dim dt As DataTable = m_dvDatos.Table
        CampoColumn.Items.Clear()
        For I As Integer = 0 To dt.Columns.Count - 1
            CampoColumn.Items.Add(dt.Columns(I).ColumnName)
        Next
    End Sub

    Private Function AplicarFiltros() As Boolean
        Dim row As DataRow
        Dim sbFiltro As New System.Text.StringBuilder
        Dim col As DataColumn
        Try
            dgvFiltros.EndEdit()
            bsFiltros.EndEdit()
            SendKeys.Send(vbTab)
            Application.DoEvents()
            dgvFiltros.CancelEdit()
            bsFiltros.CancelEdit()
            m_dtFiltros.AcceptChanges()
            For Each row In m_dtFiltros.Rows
                If Not DBUtils.IsNullOrEmpty(row("Campo")) _
                    And Not DBUtils.IsNullOrEmpty(row("Comparacion")) _
                    And Not DBUtils.IsNullOrEmpty(row("Valor")) Then
                    If sbFiltro.Length <> 0 Then
                        sbFiltro.Append(" AND ")
                    End If
                    col = m_dvDatos.Table.Columns(CStr(row("Campo")))
                    If col.DataType Is GetType(System.String) Then

                        If row("Comparacion").ToString() = "Like" Then
                            sbFiltro.AppendFormat(" {0} {1} '%{2}%' ", _
                            row("Campo"), row("Comparacion"), row("Valor"))
                        Else
                            sbFiltro.AppendFormat(" {0} {1} '{2}' ", _
                            row("Campo"), row("Comparacion"), row("Valor"))
                        End If

                    ElseIf col.DataType Is GetType(System.Boolean) Then


                        If row("Comparacion").ToString() = ">" Or _
                        row("Comparacion").ToString() = ">=" Or _
                        row("Comparacion").ToString() = "<" Or _
                        row("Comparacion").ToString() = "<=" Or _
                        row("Comparacion").ToString() = "Like" Then

                            ShowError("Para la Columna " & row("Campo").ToString() & " solo se puede realizar el operador =")
                            Return False
                        Else

                            'Excepxion de tipo Boolean
                            Try
                                sbFiltro.AppendFormat("( {0} {1} {2} )", _
                                row("Campo"), row("Comparacion"), Convert.ToBoolean(row("Valor")))
                            Catch ex As Exception
                                ShowError("Para la Columna " & row("Campo").ToString() & " solo aplican valores True o False")
                                Return False
                            End Try


                        End If

                    ElseIf col.DataType Is GetType(System.Decimal) Or col.DataType Is GetType(System.Double) Or col.DataType Is GetType(System.SByte) Or col.DataType Is GetType(System.Byte) Or col.DataType Is GetType(System.Single) Or col.DataType Is GetType(System.Int32) Or col.DataType Is GetType(System.Int16) Or col.DataType Is GetType(System.Int64) Then
                        If row("Comparacion").ToString() = "Like" Then

                            ShowError("Para la Columna " & row("Campo").ToString() & " no se permite operador Like")
                            Return False
                        Else

                            If IsNumeric(row("Valor")) Then
                                sbFiltro.AppendFormat("( {0} {1} {2} )", _
                                row("Campo"), row("Comparacion"), row("Valor"))
                            Else

                                ShowError("Para la Columna " & row("Campo").ToString() & " solo se permiten datos numericos")
                                Return False
                            End If

                        End If
                    Else
                    End If
                End If
            Next
            m_bsOrigen.Filter = sbFiltro.ToString()
            Return True
        Catch ex As Exception
            m_bsOrigen.Filter = ""
            ShowError(ex.Message)
        End Try
    End Function

    Private Sub tbtnBorrarFiltros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnBorrarFiltros.Click
        m_dtFiltros.Rows.Clear()
        m_bsOrigen.Filter = ""
    End Sub

    Private Sub tbtnCancelar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnCancelar.Click
        DialogResult = System.Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub tbtnAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbtnAceptar.Click
        If AplicarFiltros() Then
            DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub
End Class