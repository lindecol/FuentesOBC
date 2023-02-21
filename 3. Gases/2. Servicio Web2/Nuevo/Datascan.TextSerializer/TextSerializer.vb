Imports System.Text
Imports System.Data
Imports System.IO

Public Class TextSerializer

    Public Shared Function Serialize(ByVal ds As DataSet) As String
        '' Creamos una variable para gualdar la cultura actual
        Dim OldCultureInfo As System.Globalization.CultureInfo =
                              System.Threading.Thread.CurrentThread.CurrentCulture

        'Crear una cultura standard (en-US) inglés estados unidos
        System.Threading.Thread.CurrentThread.CurrentCulture =
        New System.Globalization.CultureInfo("en-US")

        Dim sb As New StringBuilder
        Dim dt As DataTable
        Dim I, nTableCount, nTotalRowsCount As Integer

        For I = 0 To ds.Tables.Count - 1
            If ds.Tables(I).Rows.Count > 0 Then
                nTableCount += 1
                nTotalRowsCount += ds.Tables(I).Rows.Count
            End If
        Next

        sb.Append("TABLECOUNT: " & nTableCount.ToString() & vbCrLf)
        sb.Append("TOTALROWCOUNT: " & nTotalRowsCount.ToString() & vbCrLf)

        For I = 0 To ds.Tables.Count - 1
            dt = ds.Tables(I)
            If dt.Rows.Count > 0 Then
                sb.Append("TABLE: " & dt.TableName & vbCrLf)
                sb.Append("ROWCOUNT: " & dt.Rows.Count.ToString() & vbCrLf)

                ' Se agrega una primer linea con los nombres de los campos
                For Each col As DataColumn In dt.Columns
                    sb.Append(col.ColumnName & "|")
                Next
                sb.Append(vbCrLf)

                ' Se agrega una segunda linea con los tipos de datos
                For Each col As DataColumn In dt.Columns
                    sb.Append(col.DataType.ToString() & "|")
                Next
                sb.Append(vbCrLf)

                ' Se agregan lineas con los datos de todos los registros
                For Each row As DataRow In dt.Rows
                    For J As Integer = 0 To dt.Columns.Count - 1
                        If row.IsNull(J) Then
                            sb.Append("(null)|")
                        Else
                            If dt.Columns(J).DataType Is GetType(System.DateTime) Then
                                sb.Append(CDate(row(J)).ToString("yyyy/MM/dd HH:mm:ss") & "|")
                            ElseIf dt.Columns(J).DataType Is GetType(System.String) Then
                                'REEMPLAZAR EL ENTER
                                row(J) = CStr(row(J)).Replace(Chr(13), "")
                                row(J) = CStr(row(J)).Replace(Chr(10), "")
                                sb.Append(CStr(row(J)).Replace("|", "") & "|")
                            Else
                                sb.Append(CStr(row(J)) & "|")
                            End If
                        End If
                    Next
                    sb.Append(vbCrLf)
                Next
                sb.Append(vbCrLf) ' Se agrega una linea adicional para delimitar los datos de la tabla
            End If
        Next

        'Retornamos la cultura anterior
        System.Threading.Thread.CurrentThread.CurrentCulture = OldCultureInfo

        Return sb.ToString()
    End Function

    Public Shared Function GetDataSet(ByVal sSerializedData As String) As DataSet
        Dim sr As New StringReader(sSerializedData)
        Dim sLine As String
        Dim sFields(), sFieldsTypes(), sValues() As String
        Dim sTableName As String
        Dim nTableCount, nTotalRowCount, nRowCount As Integer

        Dim ds As New DataSet
        Dim row As DataRow
        Dim I As Integer
        '' Creamos una variable para gualdar la cultura actual
        Dim OldCultureInfo As System.Globalization.CultureInfo =
                              System.Threading.Thread.CurrentThread.CurrentCulture

        Try

            'Crear una cultura standard (en-US) inglés estados unidos
            System.Threading.Thread.CurrentThread.CurrentCulture =
            New System.Globalization.CultureInfo("en-US")

            ' Se lee la liena con el número de tablas serializadas
            sLine = sr.ReadLine()
            nTableCount = CInt(sLine.Substring(12))
            sLine = sr.ReadLine()
            nTotalRowCount = CInt(sLine.Substring(15))

            Do
                ' Se obtiene el nombre de cada tabla serializada y el número de registros de cada una
                sLine = sr.ReadLine()
                If sLine Is Nothing Then
                    Exit Do
                End If
                sTableName = sLine.Substring(7)
                sLine = sr.ReadLine()
                nRowCount = CInt(sLine.Substring(10))

                Dim dt As New DataTable
                dt.TableName = sTableName

                ' se obtienen los nombres de los campos
                sLine = sr.ReadLine()
                sFields = sLine.Split("|"c)

                ' se obtienen los tipos de datos
                sLine = sr.ReadLine()
                sFieldsTypes = sLine.Split("|"c)

                ' Se configuran los campos del datatable
                For I = 0 To UBound(sFields) - 1
                    dt.Columns.Add(sFields(I), Type.GetType(sFieldsTypes(I)))
                Next

                ' Se obtienen las datos de las filas y se agregan
                sLine = sr.ReadLine()
                While Not sLine Is Nothing
                    If sLine.Trim() = String.Empty Then
                        Exit While
                    End If
                    sValues = sLine.Split("|"c)
                    row = dt.NewRow()
                    For I = 0 To UBound(sFields) - 1
                        If sValues(I) <> "(null)" Then
                            If sFieldsTypes(I) = "System.DateTime" Then
                                If sValues(I).Trim <> "" Then
                                    row(sFields(I)) = DateTime.ParseExact(sValues(I), "yyyy/MM/dd HH:mm:ss", Nothing)
                                Else
                                    row(sFields(I)) = DBNull.Value
                                End If

                            ElseIf sFieldsTypes(I) = "System.String" Then
                                If sValues(I).Length = 0 Then
                                    row(sFields(I)) = " "
                                Else
                                    row(sFields(I)) = sValues(I)
                                End If
                            ElseIf sFieldsTypes(I) = "System.Int16" Then
                                row(sFields(I)) = ObtenerShort(sValues(I))
                            ElseIf sFieldsTypes(I) = "System.Int32" Then
                                row(sFields(I)) = ObtenerInt(sValues(I))
                            ElseIf sFieldsTypes(I) = "System.Decimal" Then
                                row(sFields(I)) = ObtenerDecimal(sValues(I))
                            ElseIf sFieldsTypes(I) = "System.Boolean" Then
                                row(sFields(I)) = ObtenerBoolean(sValues(I))
                            Else
                                row(sFields(I)) = sValues(I)
                            End If
                        End If
                    Next
                    dt.Rows.Add(row)
                    sLine = sr.ReadLine()
                End While
                'dt.AcceptChanges()
                ds.Tables.Add(dt)
            Loop
        Finally
            sr.Close()
            'Retornamos la cultura anterior
            System.Threading.Thread.CurrentThread.CurrentCulture = OldCultureInfo
        End Try
        Return ds
    End Function

    Private Shared Function ObtenerShort(ByVal valor As String) As Object
        Try
            Dim valorInt As Int16 = Convert.ToInt16(valor)
            Return valorInt
        Catch ex As Exception
            Return System.DBNull.Value
        End Try
    End Function

    Private Shared Function ObtenerInt(ByVal valor As String) As Object
        Try
            Dim valorInt As Int32 = Convert.ToInt32(valor)
            Return valorInt
        Catch ex As Exception
            Return System.DBNull.Value
        End Try
    End Function

    Private Shared Function ObtenerDecimal(ByVal valor As String) As Object
        Try
            Dim valorDecimal As Int32 = Convert.ToDecimal(valor)
            Return valorDecimal
        Catch ex As Exception
            Return System.DBNull.Value
        End Try
    End Function

    Private Shared Function ObtenerBoolean(ByVal valor As String) As Object
        Try
            Dim valorBoolen As Boolean = Convert.ToBoolean(valor)
            Return valorBoolen
        Catch ex As Exception
            Return System.DBNull.Value
        End Try
    End Function

End Class
