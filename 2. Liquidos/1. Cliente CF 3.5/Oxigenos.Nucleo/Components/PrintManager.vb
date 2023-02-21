Imports System.Data
Imports System.Text
Imports OpenNETCF.IO.Serial
Imports OpenNETCF.VisualBasic
Imports MovilidadCF.Windows.Forms
Imports System.IO

Public Class PrintManager
    Private portSettings As New HandshakeNone()
    Private Port As Port
    Private sbDoc As StringBuilder
    Public m_iTipoImpresora As PrinterModels
    Public m_iNumeroReglones As Integer = 0

    ' TIPO FUNCION : METODO DE LA CLASE
    ' DESCRIPCION: FUNCIONA PARA REEMPLAZAR TEXTOS EN EL DOCUMENTO A IMPRIMIR
    Public Sub RemplazarTexto(ByVal sTextoViejo As String, ByVal sNuevoTexto As String)
        sbDoc.Replace(sTextoViejo, sNuevoTexto)
    End Sub

    Public Sub OpenPort()
        ' Se crea y se abre el puerto de la impresora
        If m_iTipoImpresora = PrinterModels.ZebraPortatil Then
            portSettings.BasicSettings.BaudRate = BaudRates.CBR_19200
        Else
            portSettings.BasicSettings.BaudRate = BaudRates.CBR_9600
        End If
        portSettings.BasicSettings.ByteSize = 8
        portSettings.BasicSettings.Parity = Parity.none
        portSettings.BasicSettings.StopBits = StopBits.one
        Port = New Port(Settings.PrinterPort, portSettings)
        Port.Open()
    End Sub

    Public Sub ClosePort()
        Port.Close()
        Port = Nothing
    End Sub

    Public Sub AppendLine()
        sbDoc.Append(vbCrLf)
        m_iNumeroReglones += 1
    End Sub

    Public Sub AppendLines(ByVal nCount As Integer)
        ' Se construye el encabezado
        For I As Integer = 1 To nCount
            AppendLine()
        Next
        m_iNumeroReglones += nCount
    End Sub

    Public Sub AppendLine(ByVal sLine As String)
        If Settings.PrinterModel = PrinterModels.SyscanZFP3Friction Then
            sbDoc.Append(Space(8) & sLine & vbCrLf)
        Else
            sbDoc.Append(sLine & vbCrLf)
        End If
        m_iNumeroReglones += 1
    End Sub

    Public Sub InitPage()
        sbDoc = New StringBuilder
    End Sub

    Public Sub EndPage()
        sbDoc.Append(vbFormFeed)
    End Sub

    Public Sub AppendLine(ByVal sLine As String, ByVal ParamArray args() As Object)
        If Settings.PrinterModel = PrinterModels.SyscanZFP3Friction Then
            sbDoc.AppendFormat(Nothing, Space(8) & sLine & vbCrLf, args)
        Else
            sbDoc.AppendFormat(Nothing, sLine & vbCrLf, args)
        End If
        m_iNumeroReglones += 1
    End Sub

    Public Sub InitPrinter()
        ' Se envia cadena de inicialización
        'Dim sInitData As Byte() = {27, 91, 75, 4, 0, 0, 3, 0, 80}
        ' port.Output = sInitData
        If m_iTipoImpresora <> PrinterModels.ZebraPortatil Then
            Dim sInitPagina() As Byte = {27, 91, 75, 4, 0, 0, 3, 0, 80, 18, 27, 48, 15, 27, 50, 27, 65, 2}
            Port.Output = sInitPagina
        End If
    End Sub

    Private Sub PrintData(ByVal sData As String)
        Port.Output = Encoding.Default.GetBytes(sData)
    End Sub

    Public Function PrintSyscan(ByVal doc As PrinterDocument) As Boolean
        Dim I As Integer

        ' Se arma y el documento envia el encabezado
        InitPage()
        AppendLines(7)
        AppendLine(Space(70) & doc.Nombre)
        AppendLine(Space(8) & doc.Cliente.Nombre)
        AppendLine(Space(18) & doc.Cliente.Direccion & "- " & doc.Cliente.Telefono)
        AppendLine(Space(8) & "{0,-60}{1,-19}{2,-6}{3,-6}{4,-7}{5,-6}{6,-6}{7,-6}", _
            doc.Cliente.Barrio, doc.Consecutivo, doc.FechaElaboracion.Day, _
            doc.FechaElaboracion.Month, doc.FechaElaboracion.Year, doc.FechaVencimiento.Day, _
            doc.FechaVencimiento.Month, doc.FechaVencimiento.Year)
        AppendLine(Space(8) & doc.Cliente.NIT)
        AppendLine(Space(8) & "{0,-58}{1,-13}{2,-33}{3}", _
            doc.Cliente.Entidad, doc.Cliente.Codigo, doc.TipoPago, doc.NumPedido)
        AppendLines(2)

        ' Se agregan los items
        Dim nItemLines As Integer = 0
        For I = 0 To doc.Items.Count - 1
            If doc.Items(I).UsaPrecios Then
                AppendLine("{0,-8} {1,-54}{2,12:#,##0}  {3,-4}{4,17:#,##0.00}{5,23:#,##0.00}", _
                    doc.Items(I).CodProducto, doc.Items(I).Descripcion, doc.Items(I).Cantidad, _
                    doc.Items(I).UnidadMedida, doc.Items(I).PrecioUnitario, doc.Items(I).PrecioTotal)
            Else
                AppendLine("{0,-8} {1,-54}{2,12:#,##0}  {3,-4}", _
                    doc.Items(I).CodProducto, doc.Items(I).Descripcion, _
                    doc.Items(I).Cantidad, doc.Items(I).UnidadMedida)
            End If
            If IsEmptyOrNull(doc.Items(I).InfoAdicional) Then
                nItemLines += 1
            Else
                AppendLine(Space(9) & doc.Items(I).InfoAdicional)
                nItemLines += 2
            End If
        Next
        AppendLines(26 - nItemLines)

        ' Se agregan los totales
        If doc.MostarTotales Then
            AppendLine(Space(8) & "{0,30:#,##0.00}{1,45:#,##0.00}{2,38:#,##0.00}", _
                doc.Subtotal, doc.TotalIVA, doc.Total)
            AppendLines(3)
        Else
            AppendLines(4)
        End If

        ' Se agrega información resolución factura
        If doc.Resolucion IsNot Nothing Then
            AppendLine("       *** SOMOS RETENEDORES DE IVA *** ")
            AppendLine("    *** RESOLUCION DE FACTURACION DIAN ***")
            AppendLine("    RESOL. {0} DEL {1:dd/MM/yyyy}", doc.Resolucion.Numero, doc.Resolucion.Fecha)
            AppendLine("    FACTURA DESDE {0} HASTA {1}", doc.Resolucion.RangoIni, doc.Resolucion.RangoFin)
            AppendLines(3)
            AppendLine("La presente factura cambiaria de compraventa se asimila en todos sus efectos legales a una letra de cambio Art. 774 C.Cio")
        Else
            AppendLines(8)
        End If
        AppendLines(5)

        ' se agrega el final del documento
        AppendLine(Space(12) & GestorNucleo.dsNucleo.Parametros(0).NombreChofer)
        AppendLine(Space(12) & GestorNucleo.dsNucleo.Parametros(0).RutaPrincipal)
        AppendLine(Space(12) & Now.ToString("HH:mm"))
        EndPage()
        Return SendDataToPrinter()
    End Function

    Public Function SendDataToFile() As Boolean

        If System.IO.File.Exists("\testZPL.zpl") Then
            System.IO.File.Delete("\testZPL.zpl")
        End If

        Dim fileTest As New StreamWriter("\testZPL.zpl")
        Try
            fileTest.Write(sbDoc.ToString())
        Catch ex As Exception
            fileTest.Close()
        End Try
    End Function

    Public Function SendDataToPrinter() As Boolean
        ' Se abre el puerto y se envia el documento
        Do
            Try
                OpenPort()
                Exit Do
            Catch
                If Settings.PrinterPortType = PrinterPortTypes.Bluetooth Then
                    If Not UIHandler.Confirm("No se pudo establecer comunicación con la impresora, por favor asegurese de que este encendida", "Volver a intentar?") Then
                        Return False
                    End If
                Else
                    UIHandler.ShowError("No se pudo abrir el puerto de comunicaciones especificado, revise la configuración y vuelva a intentar")
                    Return False
                End If
            End Try
        Loop

        InitPrinter()
        PrintData(sbDoc.ToString())
        ClosePort()
        Return True
    End Function

    Public Function PrintSyscan(ByVal doc As PrinterReport) As Boolean
        InitPage()

        ' Se agrega el encabezado del documento
        AppendLines(2)
        AppendLine(Space(2) & "OXIGENOS DE COLOMBIA" & Space(30) & Now.ToString("yyyy/MM/dd HH:mm"))
        AppendLine()
        AppendLine(Space(2) & "Sucursal:  {0,-60} Placa: {1}", _
            GestorNucleo.dsNucleo.Parametros(0).NombreSucursal, _
            GestorNucleo.dsNucleo.Parametros(0).CodigoVehiculo)
        AppendLine(Space(2) & "Conductor: {0,-60} Ruta:  {1} ", _
            GestorNucleo.dsNucleo.Parametros(0).NombreChofer, _
            GestorNucleo.dsNucleo.Parametros(0).RutaPrincipal)

        ' Se agrega la linea de titulos
        AppendLines(2)
        AppendLine(Space(2) & doc.Titulo)
        AppendLine()
        Dim Item As KeyValuePair(Of String, PrinterReport.ColumnInfo)
        Dim col As PrinterReport.ColumnInfo
        Dim sTitulos As String = Space(2)
        Dim sSepTitulo As String = Space(2)
        Dim sSepTotales As String = Space(2)
        Dim sTotales As String = Space(2)
        For Each Item In doc.Columnas
            col = Item.Value
            If col.Alineacion = PrinterReport.TipoAlineacion.Izquierda Then
                sTitulos &= AlignLeft(col.Caption, col.MaxLength + 1)
            Else
                sTitulos &= AlignRight(col.Caption, col.MaxLength) & " "
            End If
            sSepTitulo &= StrDup(col.MaxLength, "-"c) & " "

            If col.Total IsNot Nothing AndAlso col.Total <> "" Then
                sSepTotales &= StrDup(col.MaxLength, "-"c) & " "
                sTotales &= AlignRight(col.Total, col.MaxLength) & " "
            Else
                sTotales &= Space(col.MaxLength + 1)
                sSepTotales &= Space(col.MaxLength + 1)
            End If
        Next
        AppendLine(sTitulos)
        AppendLine(sSepTitulo)

        ' Se agregan los registros
        Dim I As Integer
        Dim sRegistro As String = Nothing
        Dim sValor As String
        For I = 0 To doc.dtReporte.Rows.Count - 1
            sRegistro = Space(2)
            For Each Item In doc.Columnas
                col = Item.Value
                If col.Formato <> "" Then
                    sValor = String.Format("{0:" & col.Formato & "}", doc.dtReporte.Rows(I)(col.FieldName))
                Else
                    sValor = CStr(doc.dtReporte.Rows(I)(col.FieldName))
                End If
                If col.Alineacion = PrinterReport.TipoAlineacion.Izquierda Then
                    sRegistro &= AlignLeft(sValor, col.MaxLength + 1)
                Else
                    sRegistro &= AlignRight(sValor, col.MaxLength) & " "
                End If
            Next
            AppendLine(sRegistro)
        Next

        ' Se agregan los totales
        AppendLine(sSepTotales)
        AppendLine(sTotales)
        EndPage()

        Return SendDataToPrinter()


    End Function

    Public Function IsEmptyOrNull(ByVal s As String) As Boolean
        Return ((s Is Nothing) OrElse (s.Trim() = ""))
    End Function

    Public Function AlignLeft(ByVal s As String, ByVal Len As Integer) As String
        s = s.Trim()
        If s.Length > Len Then
            s = s.Substring(0, Len - 1)
        End If
        Return s.PadRight(Len)
    End Function

    Public Function AlignRight(ByVal s As String, ByVal Len As Integer) As String
        s = s.Trim()
        If s.Length > Len Then
            s = s.Substring(0, Len - 1)
        End If
        Return s.PadLeft(Len)
    End Function

    Public Function CrearFirma(ByVal path As String, ByVal xPos As Integer, ByVal yPos As Integer) As String
        Try
            Dim cpclData As String

            If Not File.Exists(path) Then
                Return ""
            End If
            Dim bmp As New Bitmap(path)

            If bmp Is Nothing Then
                Return ""
            End If

            'Make sure the width is divisible by 8
            Dim loopWidth As Integer = 8 - (bmp.Width Mod 8)
            If loopWidth = 8 Then
                loopWidth = bmp.Width
            Else
                loopWidth += bmp.Width
            End If

            cpclData = ""
            cpclData = cpclData & (String.Format("EG {0} {1} {2} {3} ", loopWidth \ 8, bmp.Height, xPos, yPos))

            For y As Integer = 0 To bmp.Height - 1
                Dim bit As Integer = 128
                Dim currentValue As Integer = 0
                For x As Integer = 0 To loopWidth - 1
                    Dim intensity As Integer

                    If x < bmp.Width Then
                        Dim color As Color = bmp.GetPixel(x, y)

                        Dim MyR As Integer = color.R
                        Dim MyG As Integer = color.G
                        Dim MyB As Integer = color.B

                        intensity = CInt(255 - ((MyR + MyG + MyB) / 3))
                    Else
                        intensity = 0
                    End If

                    If intensity >= 128 Then
                        currentValue = currentValue Or bit
                    End If
                    bit = bit >> 1
                    If bit = 0 Then
                        cpclData = cpclData & (currentValue.ToString("X2"))
                        bit = 128
                        currentValue = 0
                    End If
                    'x
                Next
            Next
            'y            
            Return cpclData
        Catch ex As Exception
            Return ""
        End Try
    End Function

End Class
