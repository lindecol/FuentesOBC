Imports MovilidadCF.Windows.Forms

Module Datos
    Public Barcode As New BarcodeParser
    Public Productos As New GestorProductos
    Public Pedidos As New GestorPedidos
    Public Venta As New GestorVenta
    Public Pacientes As New GestorPacientes
    Public bEntidadBloqueada As Boolean
    Public bRespetaPrecio As Boolean
    Public bVerificarAutorizacion As Boolean
    Public bRemisionValorizada As Boolean
    Public bPagoAlquiler As Boolean
    Public bPagoDeposito As Boolean

    Public PrimeraVez As Boolean = True

    Public Function cstrDBNULL(ByVal sValor As Object, Optional ByVal sValorDefecto As String = "") As String
        If sValor Is Nothing Then
            Return sValorDefecto
        ElseIf sValor.GetType Is Type.GetType("System.DBNull") Then
            If sValorDefecto <> "" Then
                Return sValorDefecto
            Else
                Return ""
            End If
        Else
            Return CStr(sValor)
        End If
    End Function

    Public Function CIntDBNull(ByVal iValor As Object, Optional ByVal iValorDefecto As Integer = 0) As Integer
        If iValor Is Nothing Then
            Return iValorDefecto
        ElseIf iValor.GetType Is Type.GetType("System.DBNull") Then
            Return iValorDefecto
        Else
            Return CInt(iValor)
        End If
    End Function

    Public Function CDblDBNull(ByVal iValor As Object, Optional ByVal iValorDefecto As Double = 0) As Double
        If iValor Is Nothing Then
            Return iValorDefecto
        ElseIf iValor.GetType Is Type.GetType("System.DBNull") Then
            Return iValorDefecto
        ElseIf iValor.ToString = "" Then
            Return iValorDefecto
        Else
            Return CDbl(iValor)
        End If
    End Function

    Public Function CShortDBNull(ByVal iValor As Object, Optional ByVal iValorDefecto As Short = 0) As Short
        If iValor Is Nothing Then
            Return iValorDefecto
        ElseIf iValor.GetType Is Type.GetType("System.DBNull") Then
            Return iValorDefecto
        Else
            Return CShort(iValor)
        End If
    End Function

    Public Function CDecDBNull(ByVal iValor As Object, Optional ByVal iValorDefecto As Short = 0) As Decimal
        If iValor Is Nothing Then
            Return iValorDefecto
        ElseIf iValor.GetType Is Type.GetType("System.DBNull") Then
            Return iValorDefecto
        Else
            Return CDec(iValor)
        End If
    End Function

    Public Function QuitarFormato(ByVal sValor As String) As String
        Return sValor.Replace(",", "")
    End Function

    Public Sub ActualizarCampoShort(ByVal Campo As Object, ByVal Cantidad As Short)
        If Campo Is DBNull.Value Then
            Campo = Cantidad
        Else
            Campo = CShort(Campo) + Cantidad
        End If
    End Sub

    Public Sub ActualizarCampoDouble(ByVal Campo As Object, ByVal Cantidad As Double)
        If Campo Is DBNull.Value Then
            Campo = Cantidad
        Else
            Campo = CDec(Campo) + Cantidad
        End If
    End Sub

    Public Function CalcularPorcentaje(ByVal Precio As Double, ByVal Porcentaje As Double) As Double
        If Porcentaje > 0 Then
            Return (Precio * Porcentaje) / 100
        Else
            Return 0
        End If
    End Function

    Public Function ObtenerNoFactura(ByRef Row As DataRow, ByVal sTipoDocumento As Short) As Boolean
        Row = Nucleo.GetTalonarioActual(sTipoDocumento)

        If Row Is Nothing Then
            UIHandler.ShowError("No existen documentos disponibles para la impresión!!")
            Return False
        Else
            If PrimeraVez Then
                PrimeraVez = False
            Else
                IncrementarNumeroDocumento(Row)
                Nucleo.UpdateTalonario(Row)
            End If

            If Venta.ObtenerFactura(cstrDBNULL(Row("Actual"))) Then
                IncrementarNumeroDocumento(Row)
                Nucleo.UpdateTalonario(Row)
                Row = Nucleo.GetTalonarioActual(sTipoDocumento)
            End If
        End If

        Return True
    End Function

    Public Sub WriteLog(ByVal ex As Exception)
        ' Se guarda la información del error en el archivo
        Dim stream As New System.IO.StreamWriter("\Log.txt", True)
        Dim bld As New System.Text.StringBuilder()
        Dim inner As Exception = ex.InnerException

        stream.WriteLine("==============================================================================")
        stream.WriteLine(Now.ToString() & "Ha ocurrido una excepción: " & ex.GetType().FullName)
        stream.WriteLine(ex.Message)
        If Not inner Is Nothing Then
            stream.WriteLine("Inner Exception: " & inner.ToString())
        End If
        If ex.GetType() Is GetType(System.Data.SqlServerCe.SqlCeException) Then
            Dim sqlex As System.Data.SqlServerCe.SqlCeException
            Dim err As System.Data.SqlServerCe.SqlCeError
            sqlex = DirectCast(ex, System.Data.SqlServerCe.SqlCeException)

            ' Enumerate each error to a message box.
            For Each err In sqlex.Errors
                stream.WriteLine(" Error Code: " & err.HResult.ToString("X"))
                stream.WriteLine(" Message   : " & err.Message)
                stream.WriteLine(" Minor Err.: " & err.NativeError)
                stream.WriteLine(" Source    : " & err.Source)

                ' Retrieve the error parameter numbers for each error.
                Dim numPar As Integer
                For Each numPar In err.NumericErrorParameters
                    If 0 <> numPar Then
                        stream.WriteLine(" Num. Par. : " & numPar)
                    End If
                Next numPar

                ' Retrieve the error parameters for each error.
                Dim errPar As String
                For Each errPar In err.ErrorParameters
                    If String.Empty <> errPar Then
                        bld.Append((ControlChars.Cr & " Err. Par. : " & errPar))
                    End If
                Next errPar
            Next err
        End If
        stream.WriteLine("Stack Trace: " & ex.StackTrace.ToString())
        stream.Close()
    End Sub

End Module
