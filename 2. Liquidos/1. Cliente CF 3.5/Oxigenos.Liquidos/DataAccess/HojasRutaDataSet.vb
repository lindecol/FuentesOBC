Partial Class HojasRutaDataSet
    Partial Class ReporteDetalleHojaRutaDataTable

        Private Sub ReporteDetalleHojaRutaDataTable_ColumnChanging(ByVal sender As System.Object, ByVal e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.PresionFinalColumn.ColumnName) Then
                'Agregar código de usuario aquí
            End If

        End Sub

    End Class

    Partial Class HojasRutaDataTable

        Private Sub HojasRutaDataTable_HojasRutaRowChanging(ByVal sender As System.Object, ByVal e As HojasRutaRowChangeEvent) Handles Me.HojasRutaRowChanging

        End Sub
    End Class

End Class
