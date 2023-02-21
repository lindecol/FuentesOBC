Public Class Style
    Public Shared Sub ApplyStyle(ByVal ctlGridView As System.Windows.Forms.DataGridView)
        ' Se aplica estilo del grid
        ctlGridView.BorderStyle = BorderStyle.None
        ctlGridView.EnableHeadersVisualStyles = False
        ctlGridView.GridColor = Color.SteelBlue

        ' Propiedades headers columnas y filas
        ctlGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        ctlGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.GradientInactiveCaption)
        ctlGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
        ctlGridView.RowHeadersDefaultCellStyle.BackColor = Color.FromKnownColor(KnownColor.ActiveCaption)
        ctlGridView.AllowUserToResizeRows = False

        ' Propiedades especificas para algunos tipos de columna
        Dim I As Integer
        Dim cbCell As DataGridViewComboBoxCell
        For I = 0 To ctlGridView.Columns.Count - 1
            If ctlGridView.Columns(I).CellType Is GetType(DataGridViewComboBoxCell) Then
                cbCell = DirectCast(ctlGridView.Columns(I).CellTemplate, DataGridViewComboBoxCell)
                cbCell.FlatStyle = FlatStyle.Standard
                cbCell.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                cbCell.DisplayStyleForCurrentCellOnly = True
            End If
        Next
    End Sub

End Class
