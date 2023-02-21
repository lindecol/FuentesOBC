Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Data.SqlServerCe
Imports MovilidadCF.Data
Imports MovilidadCF.Data.SqlServerCe

'sqlCeSelectCommand.IndexName = GestorNucleo.GetPrimaryKey(sqlCeSelectCommand.CommandText)

Public Class NucleoDataAccess

    Private m_dtPK As DataTable = Nothing
    Private m_dvPK As DataView = Nothing
    Private m_GuiaDocumentos As DataTable = Nothing
    Private tbAGuiaDocumentos As SqlCeDataAdapter

    Public Sub LoadPrimaryKeysInfo()
        ' Se obtiene la informaci�n de los indices de llave primaria de las tablas en 
        ' la base de datos
        OpenConnection()
        Dim daIndices As New SqlCeDataAdapter
        m_dtPK = New DataTable
        daIndices.SelectCommand = New SqlCeCommand
        daIndices.SelectCommand.Connection = Me.Connection
        daIndices.SelectCommand.CommandType = CommandType.Text
        daIndices.SelectCommand.CommandText = _
            " SELECT TABLE_NAME, CONSTRAINT_NAME, COLUMN_NAME, ORDINAL_POSITION " & _
            " FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE " & _
            " WHERE CONSTRAINT_NAME LIKE 'PK%' ORDER BY TABLE_NAME, ORDINAL_POSITION"
        daIndices.Fill(m_dtPK)
        m_dtPK.TableName = "Indices"
        m_dvPK = New DataView(m_dtPK)
        CloseConnection()
    End Sub
    Public Function CadenaConexion() As String
        Return Me.Connection.ConnectionString.ToString()
    End Function
    Public Function GetPrimaryKey(ByVal sTableName As String) As String
        m_dvPK.RowFilter = "TABLE_NAME = '" & sTableName & "'"
        Return CStr(m_dvPK(0)("CONSTRAINT_NAME"))
    End Function

    Public Sub LoadParametros()
        dtaParametros.Connection = Me.Connection
        dtaParametros.Fill(dsNucleo.Parametros)
    End Sub


    Public Sub LoadTiposCliente()
        dsNucleo.TiposCliente.Rows.Clear()
        dsNucleo.TiposCliente.AddTiposClienteRow(TiposCliente.Industrial, "Industrial")
        dsNucleo.TiposCliente.AddTiposClienteRow(TiposCliente.Paciente, "Paciente")
        dsNucleo.TiposCliente.AddTiposClienteRow(TiposCliente.Entidad, "Entidad")
    End Sub

    Public Sub LoadTiposPago()
        dsNucleo.TiposPago.Rows.Clear()
        dsNucleo.TiposPago.AddTiposPagoRow(TiposPago.Contado, "Contado")
        dsNucleo.TiposPago.AddTiposPagoRow(TiposPago.Credito, "Cr�dito")
    End Sub

    Public Sub UpdateParametros()
        dtaParametros.Connection = Me.Connection
        dtaParametros.Update(dsNucleo.Parametros)
        dsNucleo.Parametros.AcceptChanges()
    End Sub

    Public Function HayDatosCargados() As Boolean
        Return dsNucleo.Parametros.Rows.Count > 0
    End Function

    Public Function HayPedidosProcesados() As Boolean
        dtaPedidos.Connection = Me.Connection
        Return (dtaPedidos.GetCountPedidosAtendidosYAnulados().Value > 0)
    End Function

    Public Sub LoadEntidades()
        dtaEntidades.Connection = Me.Connection
        dtaEntidades.Fill(Me.dsNucleo.Entidades)
    End Sub

    Public Sub LoadClientes()
        If Programa.UseQueryFilterInRutero AndAlso Programa.FilterQueryClientes <> "" Then
            Dim daClientes As New SqlCeDataAdapter
            daClientes.SelectCommand = New SqlCeCommand
            daClientes.SelectCommand.CommandText = "SELECT * FROM CLIENTES " & Programa.FilterQueryClientes
            daClientes.SelectCommand.CommandType = CommandType.Text
            daClientes.SelectCommand.Connection = Me.Connection
            dsNucleo.Clientes.Rows.Clear()
            daClientes.Fill(dsNucleo.Clientes)
            daClientes.Dispose()
        Else
            dtaClientes.Connection = Me.Connection
            dtaClientes.Fill(dsNucleo.Clientes)
        End If
    End Sub

    Public Sub UpdateCliente(ByVal rowCliente As NucleoDataSet.ClientesRow)
        dtaClientes.Connection = Me.Connection
        dtaClientes.Update(rowCliente)
        rowCliente.AcceptChanges()
    End Sub

    Public Sub LoadDireccionesCliente(ByVal rowCliente As NucleoDataSet.ClientesRow)
        dtaDirecciones.Connection = Me.Connection
        dtaDirecciones.FillByCliente(dsNucleo.DireccionesCliente, rowCliente.Codigo)
    End Sub

    Public Sub LoadPedidosCliente(ByVal rowCliente As NucleoDataSet.ClientesRow)
        dtaPedidos.Connection = Me.Connection
        dtaPedidos.FillByCliente(dsNucleo.Pedidos, rowCliente.Codigo)
    End Sub

    Public Sub LoadPedidos()
        If Programa.UseQueryFilterInRutero AndAlso Programa.FilterQueryPedidos <> "" Then
            Dim daPedidos As New SqlCeDataAdapter
            daPedidos.SelectCommand = New SqlCeCommand
            daPedidos.SelectCommand.CommandType = CommandType.Text
            daPedidos.SelectCommand.CommandText = "SELECT * FROM PEDIDOS " & Programa.FilterQueryPedidos
            daPedidos.SelectCommand.Connection = Me.Connection
            dsNucleo.Pedidos.Rows.Clear()
            daPedidos.Fill(dsNucleo.Pedidos)
            daPedidos.Dispose()
        Else
            dtaPedidos.Connection = Me.Connection
            dtaPedidos.Fill(dsNucleo.Pedidos)
        End If
    End Sub

    Public Sub UpdatePedido(ByVal rowPedido As NucleoDataSet.PedidosRow)
        dtaPedidos.Connection = Me.Connection
        dtaPedidos.Update(rowPedido)
        rowPedido.AcceptChanges()
    End Sub

    Public Sub UpdatePedidos(ByVal rowsPedidos() As DataRow)
        dtaPedidos.Connection = Me.Connection
        dtaPedidos.Update(rowsPedidos)
        For Each row As DataRow In rowsPedidos
            row.AcceptChanges()
        Next
    End Sub

    Public Sub LoadDocumentos()
        dtaDocumentos.Connection = Me.Connection
        dtaDocumentos.Fill(dsNucleo.Talonarios)
    End Sub

    Public Sub UpdateDocumentoActual(ByVal row As NucleoDataSet.TalonariosRow)
        dtaDocumentos.Connection = Me.Connection
        dtaDocumentos.Update(row)
        row.AcceptChanges()
    End Sub

    Public Function GetDocumentoActual(ByVal CodTipoDocumento As Integer) As NucleoDataSet.TalonariosRow
        Dim row As NucleoDataSet.TalonariosRow = Nothing
        'KUXAN: MODIFICAR ALGORITMO OBTENER TALONARIO
        Try
            'Dim sFilter As String = String.Format("(CodTipoDocumento = {0}) AND (Actual >= Inicio) AND (Actual <= Fin)", CodTipoDocumento)
            Dim sFilter As String = String.Format("CodTipoDocumento = {0}", CodTipoDocumento)
            'Dim rows() As DataRow = dsNucleo.Talonarios.Select(sFilter, "Consecutivo DESC")
            'If rows.Length > 0 Then
            '    If CodTipoDocumento = TiposDocumento.FacturaAutomatica Then
            '        For I As Integer = 0 To UBound(rows)
            '            row = CType(rows(I), NucleoDataSet.TalonariosRow)

            '            ' Se verifica que la resoluci�n este vigente
            '            If row.FechaInicioResolucion.Date <= Today() And row.FechaFinResolucion.Date >= Today() Then
            '                Exit For
            '            End If
            '            row = Nothing
            '        Next
            '    Else ' Es otro tipo de documento que no requiere resoluci�n
            '        row = CType(rows(0), NucleoDataSet.TalonariosRow)
            '    End If
            'End If

            If (dsNucleo.Talonarios.Count = 0) Then
                MovilidadCF.Logging.Logger.Write("NO existen talonarios en memoria")
                Dim taTalonarios As New NucleoDataSetTableAdapters.TalonariosTableAdapter
                taTalonarios.Connection = Me.Connection
                taTalonarios.Fill(dsNucleo.Talonarios)
            End If

            Dim RowsTipo As DataRow() = dsNucleo.Talonarios.Select(sFilter, "Consecutivo DESC")

            If (RowsTipo.Length = 0) Then
                MovilidadCF.Logging.Logger.Write("NO existen talonarios para el tipo documento: " + CodTipoDocumento.ToString())
            End If

            For Each rw As NucleoDataSet.TalonariosRow In RowsTipo
                Dim iActual As Integer = Convert.ToInt32(rw("Actual"))
                Dim iInicio As Integer = Convert.ToInt32(rw("Inicio"))
                Dim iFin As Integer = Convert.ToInt32(rw("Fin"))
           
                If (iActual >= iInicio And iActual <= iFin) Then
                    row = rw
                    If CodTipoDocumento = TiposDocumento.FacturaAutomatica Then
                        If row.FechaInicioResolucion.Date <= Today() And row.FechaFinResolucion.Date >= Today() Then
                            MovilidadCF.Logging.Logger.Write("Talonario OK")
                            Exit For
                        End If
                        MovilidadCF.Logging.Logger.Write("Error en la fecha de inicio de resoluci�n")
                        row = Nothing
                    End If
                Else
                    MovilidadCF.Logging.Logger.Write("Error en inicio y fin")
                End If
            Next
        Catch ex As Exception
            MovilidadCF.Logging.Logger.Write("Error consultando talonarios")
            MovilidadCF.Logging.Logger.Write(ex)
            row = Nothing
        End Try
        If row IsNot Nothing Then
            'KUXAN REVISAR CONSECUTIVO CORRECTO
            Dim tipo As String
            If CodTipoDocumento = 8 Then
                tipo = "B"
            Else
                tipo = "F"
            End If

            Dim talonarioActual As Integer = ExisteTalonario(tipo, row.Actual, row.Prefijo, CInt(row.Fin))

            If talonarioActual < 0 Then
                row = Nothing
            Else
                row.Actual = talonarioActual.ToString().PadLeft(8, Convert.ToChar("0"))
            End If

        End If

        Return row
    End Function

    Public Function ExisteTalonario(ByVal tipo As String, ByVal actual As String, ByVal prefijo As String, ByVal maximo As Int32) As Integer
        Dim iActual As Int32 = Convert.ToInt32(actual)

        While True
            Dim sqlTest As String = "SELECT TipoFactura, NoFactura, Prefijo FROM MaestroFacturas WHERE TipoFactura = '{0}' AND NoFactura = '{1}' AND Prefijo = '{2}'"
            sqlTest = String.Format(sqlTest, tipo, iActual.ToString().PadLeft(8, Convert.ToChar("0")), prefijo)
            Try
                Me.OpenConnection()
                Dim cmdRevisar As New SqlCeDataAdapter(sqlTest, Me.Connection)
                Dim dtMaestro As New DataTable
                cmdRevisar.Fill(dtMaestro)

                If dtMaestro.Rows.Count = 0 Then
                    Return iActual
                Else
                    If iActual < maximo Then
                        iActual = iActual + 1
                    Else
                        Return -1
                    End If
                End If
            Catch ex As Exception
                MovilidadCF.Logging.Logger.Write(ex)
                Return -1
            Finally
                Me.CloseConnection()
            End Try
        End While
        Return -1
    End Function

    Public Sub actualizarGuiaDocumento()

        Dim cmdActualizar As New SqlCeCommand
        Dim sSql As String = ""
        Me.OpenConnection()

        Dim fila As DataRow
        For Each fila In m_GuiaDocumentos.Rows
            cmdActualizar = New SqlCeCommand
            cmdActualizar.Connection = Me.Connection
            cmdActualizar.CommandType = CommandType.Text
            sSql = "UPDATE GUIA_DOCUMENTO SET TRANSMITIDO_SN = 'S', HORA_TRANSMISION = GETDATE() WHERE "
            sSql = sSql & " NUM_GUIA = " & fila("Num_Guia").ToString
            sSql = sSql & " AND PREFIJO_DOCUMENTO = " & fila("Prefijo_Documento").ToString
            sSql = sSql & " AND NUM_DOCUMENTO = " & fila("Num_Documento").ToString
            cmdActualizar.CommandText = sSql
            cmdActualizar.ExecuteNonQuery()
        Next

        Me.CloseConnection()
    End Sub

    ''' <summary>
    ''' KUXAN 26/12/2019 Funcion para actualizar el path del PDF generado
    ''' </summary>
    ''' <param name="tipoFactura">Tipo Factura</param>
    ''' <param name="numeroFactura">Numero Factura</param>
    ''' <param name="prefijo">Prefijo</param>
    ''' <param name="path">Path PDF generado</param>
    ''' <remarks></remarks>
    Public Sub ActualizarDocumentoPDF(ByVal tipoFactura As String, ByVal numeroFactura As String, ByVal prefijo As String, ByVal path As String)
        'KUXAN 14/01/2020: Actualizar path pdf de las facturas

        Dim nuevoTipo As String
        If tipoFactura = "8" Then
            nuevoTipo = "B"
        Else
            nuevoTipo = "F"
        End If


        Dim cmdActualizar As New SqlCeCommand
        Dim sSql As String = "UPDATE MaestroFacturas SET PathPDF = '{0}' WHERE TipoFactura = '{1}' AND NoFactura = '{2}' AND Prefijo = '{3}'"
        Try
            Me.OpenConnection()
            
            cmdActualizar = New SqlCeCommand
            cmdActualizar.Connection = Me.Connection
            cmdActualizar.CommandType = CommandType.Text
            sSql = String.Format(sSql, path, nuevoTipo, numeroFactura, prefijo)
            cmdActualizar.CommandText = sSql
            cmdActualizar.ExecuteNonQuery()
        Finally
            Me.CloseConnection()
        End Try

        Me.ActualizarVersionParametros()
       
    End Sub

    Public Sub ActualizarVersionParametros()
        Try
            'KUXAN 14/01/2020: Actualizar version del programa y enviar
            Dim version As String = "Ver." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Major _
                            & "." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Minor _
                            & "." & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.Build

            Dim sSql As String = "UPDATE Parametros SET VersionOBC = '" & version & "'"
            Dim cmdActualizarParametro As New SqlCeCommand
            Try
                Me.OpenConnection()

                cmdActualizarParametro = New SqlCeCommand
                cmdActualizarParametro.Connection = Me.Connection
                cmdActualizarParametro.CommandType = CommandType.Text
                cmdActualizarParametro.CommandText = sSql
                cmdActualizarParametro.ExecuteNonQuery()
            Finally
                Me.CloseConnection()
            End Try
        Catch ex As Exception
            MovilidadCF.Logging.Logger.Write(ex)
        End Try
       
    End Sub


#Region " Funciones utilizadas para la integraci�n de datos "
    ' Permite realizar la integraci�n de datos de a partir de un dataset serializado
    ' El dataset debe contener tablas con nombres y campos iguales a los creados en la base de
    ' datos
    Public Function IntegrarDatos(ByVal sSerializedData As String, ByVal bUpdateCurrentRows As Boolean, _
        ByVal Estado As IEstadoCarga) As Boolean
        Dim sr As New StringReader(sSerializedData)
        Dim sLine As String
        Dim sFields(), sFieldsTypes(), sValues() As String
        Dim rs As SqlCeResultSet = Nothing
        Dim record As SqlCeUpdatableRecord
        Dim I, J, nIndex As Integer
        Dim nTableCount, nRowCount, nTotalRowCount, nTables, nRows, nTotalRows As Integer
        Dim nProgresoTabla, nProgresoTotal As Integer
        Dim dtNucleo As DataTable = Nothing
        Dim row As DataRow = Nothing
        Dim FieldValue As Object = Nothing
        Try
            ' Se lee la liena con el n�mero de tablas serializadas y el numero total de filas a procesar
            sLine = sr.ReadLine()
            nTableCount = CInt(sLine.Substring(12))
            sLine = sr.ReadLine()
            nTotalRowCount = CInt(sLine.Substring(15))
            nProgresoTotal = 0
            nTables = 0
            nTotalRows = 0

            Me.OpenConnection()
            Do While Not Estado.Cancelado
                ' Se obtiene el nombre y cantidad de registros de cada tabla serializada
                Dim sTableName As String
                sLine = sr.ReadLine()
                If sLine Is Nothing Then
                    Exit Do
                End If
                sTableName = sLine.Substring(7)
                sLine = sr.ReadLine()
                nRowCount = CInt(sLine.Substring(10))
                If nRowCount > 0 Then

                    nProgresoTabla = 0
                    nRows = 0

                    Estado.IniciarTabla(sTableName)

                    ' Se revisa si es una tabla del nucleo y se actualiza
                    If dsNucleo.Tables.IndexOf(sTableName) >= 0 Then
                        dtNucleo = dsNucleo.Tables(sTableName)
                    Else
                        dtNucleo = Nothing
                    End If


                    If bUpdateCurrentRows Then
                        ' Se filtra la informaci�n del indice de llave primario, para la busqueda de
                        ' de las filas actuales
                        m_dvPK.RowFilter = "TABLE_NAME = '" & sTableName & "'"
                    Else
                        ' Si es una tabla del nucleo si eliminan las filas actuales
                        If dtNucleo IsNot Nothing Then
                            dtNucleo.Rows.Clear()
                        End If
                    End If


                    ' Se obtiene el objeto ResultSet por medio del cual se har� la actualizaci�n
                    ' especificando el indice de llave primaria de la tabla
                    Dim cmd As New SqlCeCommand()
                    cmd.Connection = Me.Connection
                    cmd.CommandType = CommandType.TableDirect
                    cmd.CommandText = sTableName
                    If bUpdateCurrentRows Then
                        cmd.IndexName = CStr(m_dvPK(0)("CONSTRAINT_NAME"))
                        rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable Or ResultSetOptions.Sensitive Or ResultSetOptions.Scrollable)
                    Else
                        rs = cmd.ExecuteResultSet(ResultSetOptions.Updatable)
                    End If

                    ' se obtienen los nombres de los campos
                    sLine = sr.ReadLine()
                    sFields = sLine.Split("|"c)

                    ' se obtienen los tipos de datos de las columnas
                    sLine = sr.ReadLine()
                    sFieldsTypes = sLine.Split("|"c)

                    ' Se procesa cada fila que venga serializada en la cadena
                    sLine = sr.ReadLine()

                    Dim bInsertRecord As Boolean = False
                    While (Not sLine Is Nothing) And (Not Estado.Cancelado)
                        If sLine.Trim() = String.Empty Then
                            Exit While
                        End If

                        ' Se obtienen los valores que vienen en el registro
                        sValues = sLine.Split("|"c)

                        ' Se obtienen los valores de llave primaria del registro
                        ' Se crea la matriz de objetos para guardar los valores de la llave primaria de cada registro
                        bInsertRecord = True
                        If bUpdateCurrentRows Then

                            ' Se obtiene la llave primaria del registro
                            Dim RecordKey(m_dvPK.Count - 1) As Object
                            For I = 0 To m_dvPK.Count - 1
                                For J = 0 To UBound(sFields) - 1
                                    If CStr(m_dvPK(I)("COLUMN_NAME")).ToUpper() = sFields(J) Then
                                        RecordKey(I) = GetColumnValue(sFieldsTypes(J), sValues(J))
                                    End If
                                Next
                            Next

                            ' se busca el registro actual y luego se actualizan los datos
                            ' si no se encuentra se inserta un nuevo registro
                            If rs.Seek(DbSeekOptions.FirstEqual, RecordKey) Then
                                bInsertRecord = False

                                ' Se obtiene la fila a modificar
                                rs.Read()
                                If dtNucleo IsNot Nothing Then
                                    row = dtNucleo.Rows.Find(RecordKey)
                                End If

                                ' Se actualizan los valores de cada columna en el registro en la base de datos y si
                                ' se esta procesando una tabla del nucleo tambien se actualiza en memoria
                                If dtNucleo IsNot Nothing AndAlso row IsNot Nothing Then
                                    For I = 0 To UBound(sFields) - 1
                                        Try
                                            nIndex = rs.GetOrdinal(sFields(I))
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues(I))
                                            rs.SetValue(nIndex, FieldValue)
                                            nIndex = row.Table.Columns.IndexOf(sFields(I))
                                            If nIndex >= 0 Then
                                                row(nIndex) = FieldValue
                                            End If
                                        Catch ex As Exception
                                            Throw New InvalidOperationException("Field: " & sFields(I) & vbCrLf & _
                                                "Type: " & rs.GetFieldType(nIndex).ToString() & vbCrLf & _
                                                "Value: " & sValues(I) & vbCrLf & _
                                                ex.Message)
                                        End Try
                                    Next
                                Else
                                    For I = 0 To UBound(sFields) - 1
                                        Try
                                            nIndex = rs.GetOrdinal(sFields(I))
                                            FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues(I))
                                            rs.SetValue(nIndex, FieldValue)
                                        Catch ex As Exception
                                            Throw New InvalidOperationException("Field: " & sFields(I) & vbCrLf & _
                                                "Type: " & rs.GetFieldType(nIndex).ToString() & vbCrLf & _
                                                "Value: " & sValues(I) & vbCrLf & _
                                                ex.Message)
                                        End Try
                                    Next
                                End If
                                rs.Update()
                            End If
                        End If

                        If bInsertRecord Then
                            ' Se crea el nuevo registro
                            record = rs.CreateRecord()
                            If dtNucleo IsNot Nothing Then
                                row = dtNucleo.NewRow()
                            Else
                                row = Nothing
                            End If

                            ' Se actualizan los valores de cada columna en el registro
                            If dtNucleo IsNot Nothing AndAlso row IsNot Nothing Then
                                For I = 0 To UBound(sFields) - 1
                                    Try
                                        nIndex = rs.GetOrdinal(sFields(I))
                                        FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues(I))
                                        record.SetValue(nIndex, FieldValue)
                                        nIndex = row.Table.Columns.IndexOf(sFields(I))
                                        If nIndex >= 0 Then
                                            row(nIndex) = FieldValue
                                        End If
                                    Catch ex As Exception
                                        Throw New InvalidOperationException("Field: " & sFields(I) & vbCrLf & _
                                            "Type: " & rs.GetFieldType(nIndex).ToString() & vbCrLf & _
                                            "Value: " & sValues(I) & vbCrLf & _
                                            ex.Message)
                                    End Try
                                Next
                            Else
                                For I = 0 To UBound(sFields) - 1
                                    Try
                                        nIndex = rs.GetOrdinal(sFields(I))
                                        FieldValue = GetColumnValue(rs.GetFieldType(nIndex).ToString(), sValues(I))
                                        record.SetValue(nIndex, FieldValue)
                                    Catch ex As Exception
                                        Throw New InvalidOperationException("Field: " & sFields(I) & vbCrLf & _
                                            "Type: " & rs.GetFieldType(nIndex).ToString() & vbCrLf & _
                                            "Value: " & sValues(I) & vbCrLf & _
                                            ex.Message)
                                    End Try
                                Next
                            End If

                            ' Se almacena el nuevo registro
                            Try
                                rs.Insert(record, DbInsertOptions.KeepCurrentPosition)
                                If dtNucleo IsNot Nothing AndAlso row IsNot Nothing Then
                                    dtNucleo.Rows.Add(row)
                                    row.AcceptChanges()
                                End If
                            Catch ex As Exception
                                Dim values(rs.FieldCount) As Object
                                record.GetValues(values)
                                Throw ex
                            End Try

                        End If


                        ' Se registra el avance de la tabla
                        nRows += 1
                        nTotalRows += 1
                        If (nRows Mod 100) = 0 Or nRows = nRowCount Then
                            Estado.ProgresoTabla = CInt((nRows * 100 / nRowCount))
                            Estado.ProgresoTotal = CInt(nTotalRows * 100 / nTotalRowCount)
                        End If

                        ' Se se lee el siguiente registro
                        sLine = sr.ReadLine()
                    End While
                    rs.Close()
                End If
            Loop
        Catch ex1 As Exception
            Throw New Exception(ex1.ToString)
        Finally
            If rs IsNot Nothing Then
                If Not rs.IsClosed Then
                    rs.Close()
                    rs = Nothing
                End If
            End If
            Me.CloseConnection()
            sr.Close()
        End Try
    End Function


    Public Function GetSerializedData(ByVal QueryList As ListQuerysDescarga, ByVal Estado As IEstadoCarga, Optional ByVal idProceso As Integer = 0) As String
        Dim sb As New StringBuilder
        Dim dr As SqlCeDataReader
        Dim cmd As New SqlCeCommand
        Dim I, J, nIndexRowCountReplace, nTableCount, nRowCount, nTotalRowsCount As Integer

        ' Se adicionan las 2 primeras lineas con tags para poder remplazarlos al final
        sb.Append("TABLECOUNT: <<TABLECOUNT>>" & vbCrLf)
        sb.Append("TOTALROWCOUNT: <<TOTALROWCOUNT>>" & vbCrLf)
        nTableCount = 0
        nTotalRowsCount = 0
        Try
            Me.OpenConnection()
            For I = 0 To QueryList.Count - 1
                If Estado.Cancelado Then
                    Exit For
                End If
                If Not idProceso = 6 Then Estado.IniciarTabla(QueryList(I).TableName.ToUpper)
                cmd.Connection = Me.Connection
                cmd.CommandText = QueryList(I).Query
                If cmd.CommandText.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase) Then
                    cmd.CommandType = CommandType.Text
                Else
                    cmd.CommandType = CommandType.TableDirect
                End If
                dr = cmd.ExecuteReader()

                'cargo tabla con los documentos que se transmitieron
                If "Guia_Documento" = QueryList(I).TableName Then
                    m_GuiaDocumentos = New DataTable
                    m_GuiaDocumentos.TableName = "GuiaDocumentos"
                    tbAGuiaDocumentos = New SqlCeDataAdapter
                    tbAGuiaDocumentos.SelectCommand = cmd
                    tbAGuiaDocumentos.Fill(m_GuiaDocumentos)
                End If

                If dr.Read() Then
                    nTableCount += 1
                    sb.Append("TABLE: " & QueryList(I).TableName & vbCrLf)
                    nIndexRowCountReplace = sb.Length
                    sb.Append("ROWCOUNT: <<ROWCOUNT>> " & vbCrLf)

                    ' Se agrega una primer linea con los nombres de los campos
                    For J = 0 To dr.FieldCount - 1
                        sb.Append(dr.GetName(J) & "|")
                    Next
                    sb.Append(vbCrLf)

                    ' Se agrega una segunda linea con los tipos de datos
                    For J = 0 To dr.FieldCount - 1
                        sb.Append(dr.GetFieldType(J).ToString() & "|")
                    Next
                    sb.Append(vbCrLf)

                    ' Se agregan lineas con los datos de todos los registros
                    nRowCount = 0
                    Do
                        If Estado.Cancelado Then
                            Exit Do
                        End If
                        For J = 0 To dr.FieldCount - 1
                            If dr.IsDBNull(J) Then
                                sb.Append("(null)|")
                            Else
                                If dr.GetFieldType(J) Is GetType(System.DateTime) Then
                                    sb.Append(dr.GetDateTime(J).ToString("yyyy/MM/dd HH:mm:ss") & "|")
                                Else
                                    sb.Append(CStr(dr.GetValue(J)) & "|")
                                End If
                            End If
                        Next
                        sb.Append(vbCrLf)
                        nRowCount += 1
                        nTotalRowsCount += 1
                    Loop While dr.Read()
                    dr.Close()
                    sb.Replace("<<ROWCOUNT>>", nRowCount.ToString(), nIndexRowCountReplace, 40)
                    sb.Append(vbCrLf) ' Se agrega una linea adicional para delimitar los datos de la tabla
                End If
            Next
            sb.Replace("<<TABLECOUNT>>", nTableCount.ToString(), 0, 60)
            sb.Replace("<<TOTALROWCOUNT>>", nTotalRowsCount.ToString(), 0, 60)
        Finally
            Me.CloseConnection()
        End Try
        Return sb.ToString()
    End Function

    Private Function GetColumnValue(ByVal sFieldType As String, ByVal sValue As String) As Object
        If sValue = "(null)" Then
            Return DBNull.Value
        Else
            Select Case sFieldType
                Case "System.DateTime"
                    Return DateTime.ParseExact(sValue, "yyyy/MM/dd HH:mm:ss", Nothing)
                Case "System.Int32"
                    Return Int32.Parse(sValue)
                Case "System.Int16"
                    Return Int16.Parse(sValue)
                Case Else
                    Return sValue
            End Select
        End If
    End Function


#End Region


End Class
