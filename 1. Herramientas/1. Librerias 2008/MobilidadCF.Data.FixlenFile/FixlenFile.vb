Imports System.Text
Imports System.IO

#If NETCF20 Then
Imports MovilidadCF.Data
#Else
Imports Desktop.Data
#End If

' Posibles estados del registro actual
Friend Enum FileState
    fsNone = 0
    fsEdit = 1
    fsInsert = 2
    fsAppend = 3
End Enum

' Maneja archivos ascii con registros y campos de longitud fija
' Para la clase todos los campos son de tipo string
Public Class FixlenFile

#Region " Campos y Propiedadesde la clase "
    Private Shared mPath As String      'Ruta del directorio de trabajo
    Private mFileName As String         'Nombre del archivo fisico, incluye extensión
    Private mFields As New ArrayList    'Guarda información de campos del archivo
    Private mFieldNames As New ArrayList 'Guarda los nombres de los campos
    Private mKeyFieldNames() As String  'Guarda los nombre de los campos en el orden
    Private mRowNumber As Integer       'Numero de registro
    Private mEof As Boolean             'Indica si se llego al final de archivo
    Private mBof As Boolean             'indica si se llego al comienzo de archivo
    Private mState As FileState         'Estado del registro actual
    Protected mFieldSeparator As String
    Protected mRowSeparator As String = ControlChars.CrLf
    Protected mFile As FileStream       ' Manejador interno del archivo
    Protected mRowLen As Integer        ' Longitud del registro
    Protected mOpenCount As Integer = 0 ' Si es cero el archivo se cierra
    Friend RowData As String            ' ReDim mRowData
    Protected mEncrypted As Boolean = False



    ' Nombre simple del archivo: sin ruta de acceso(path)
    Public ReadOnly Property Name() As String
        Get
            Return mFileName
        End Get
    End Property

    ' Obtiene el nombre completo del archivo, incluyendo el path
#If NETCF20 Then
    Protected ReadOnly Property FullFileName() As String
        Get
            If mPath IsNot Nothing Then
                Return mPath + "\" + mFileName
            Else
                Return mFileName
            End If

        End Get
    End Property
#Else
    Protected ReadOnly Property FullFileName() As String
        Get
            If Not String.IsNullOrEmpty(mPath) Then
                Return Path.Combine(mPath, mFileName)
            Else
                Return mFileName
            End If
        End Get
    End Property
#End If



    Public ReadOnly Property Encrypted() As Boolean
        Get
            Return mEncrypted
        End Get
    End Property

    Public ReadOnly Property RowNumber() As Integer
        Get
            Return mRowNumber
        End Get
    End Property

    ' Determina si el esta ubicado al final del archivo
    Public ReadOnly Property Eof() As Boolean
        Get
            Return mEof
        End Get
    End Property

    ' Indica si esta ubicado al inicio del archivo
    Public ReadOnly Property Bof() As Boolean
        Get
            Return mBof
        End Get
    End Property

    Public ReadOnly Property Fecha() As DateTime
        Get
            Dim info As IO.FileInfo = New IO.FileInfo(FullFileName)
            Return info.LastWriteTime
        End Get
    End Property

    ' Permite obtener y modificar el valor de un campo del registro actual
    Default Public Property FieldValues(ByVal FieldName As String) As String
        Get
            If mState <> FileState.fsInsert And mRowNumber = 0 Then
                Throw New NoRowException(mFileName)
            End If
            Dim nIndex As Integer
            Dim myField As FixlenFileField
            nIndex = mFieldNames.IndexOf(FieldName.Trim().ToUpper())
            If nIndex >= 0 Then
                myField = DirectCast(mFields(nIndex), FixlenFileField)
                Return myField.Value
            Else
                Throw New FieldException(FieldName, mFileName)
            End If
        End Get
        Set(ByVal FieldValue As String)
            If mRowNumber = 0 Then
                Throw New NoRowException(mFileName)
            End If
            Dim nIndex As Integer
            Dim myField As FixlenFileField
            nIndex = mFieldNames.IndexOf(FieldName.Trim().ToUpper())
            If nIndex >= 0 Then
                myField = DirectCast(mFields(nIndex), FixlenFileField)
                myField.Value = FieldValue
                If mState = FileState.fsNone Then
                    mState = FileState.fsEdit
                End If
            Else
                Throw New FieldException(FieldName, mFileName)
            End If
        End Set
    End Property

    ' Permite obtener la información de cada campo que conforma el archivo
    Friend ReadOnly Property Fields(ByVal FieldName As String) As FixlenFileField
        Get
            Dim nIndex As Integer
            Dim myField As FixlenFileField
            nIndex = mFieldNames.IndexOf(FieldName.Trim().ToUpper())
            If nIndex >= 0 Then
                myField = DirectCast(mFields(nIndex), FixlenFileField)
                Return myField
            Else
                Throw New FieldException(FieldName, mFileName)
            End If
        End Get
    End Property


    ' Devuelve la longitud real del registro
    Public ReadOnly Property RowLen() As Integer
        Get
            Return mRowLen + mRowSeparator.Length
        End Get
    End Property

    Public ReadOnly Property FieldLength(ByVal nIndex As Integer) As Integer
        Get
            Return DirectCast(mFields(nIndex), FixlenFileField).Len
        End Get
    End Property

    Public ReadOnly Property FieldLength(ByVal nName As String) As Integer
        Get
            Return DirectCast(Fields(nName), FixlenFileField).Len
        End Get
    End Property

    ' Devuelve el número de registros del archivo
    Public ReadOnly Property RowCount() As Integer
        Get
            Open()
            Dim FileLen As Long = mFile.Length
            Close()
            Return CInt(FileLen / RowLen)
        End Get
    End Property

    Public ReadOnly Property FilterCount() As Integer
        Get
            Return mFilters.Count
        End Get
    End Property

    ' Devuelve el número de registro actual
    Public ReadOnly Property CurrentRow() As Integer
        Get
            Return mRowNumber
        End Get
    End Property



#End Region

#Region " Operaciones básicas de la clase "

    ' Constructor, se debe pasar el nombre del archivo
    Public Sub New(ByVal FileName As String, Optional ByVal bEncrypted As Boolean = False, Optional ByVal bFinLinea As Boolean = False)
        mFileName = FileName
        mRowNumber = 0
        'mFieldsLen = 0
        mFieldSeparator = ""
        If bFinLinea Then
            mRowSeparator = ControlChars.Lf.ToString()
        Else
            mRowSeparator = ControlChars.CrLf.ToString()
        End If
        mRowLen = 0
        mState = FileState.fsNone
        mEof = True
        mBof = True
        mFile = Nothing
        mOpenCount = 0
        mEncrypted = bEncrypted
    End Sub

    ' Abre el archivo y aumenta la cuenta de aperturas del archivo
    Public Sub Open()
        mOpenCount += 1
        If mFile Is Nothing Then
            mFile = New FileStream(FullFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        End If
    End Sub

    ' Cierra el archivoc cuando la cuenta de aperturas llega a cero
    Public Sub Close()
        If mOpenCount > 0 Then
            mOpenCount -= 1
        End If
        If mOpenCount = 0 Then
            If Not mFile Is Nothing Then
                mFile.Close()
                mFile = Nothing
            End If
        End If
    End Sub

    ' Escribe el valor de los campos en el registro fisico
    Private Sub Write(ByVal RowNumber As Integer)
        Dim Data(RowLen) As Byte

        Open()

        ' Si no se logra hacer la lectura del registro se marca 
        ' el registro actual como indefinido
        mRowNumber = 0

        ' Se lee el registro y asigna el valor a los campos
        Data = Encoding.Default.GetBytes(RowData)

        ' Si el archivo esta encriptado se niegan todos los bytes
        If mEncrypted Then
            For I As Integer = 0 To Data.Length - 1
                Data(I) = Not Data(I)
            Next
        End If

        mFile.Seek((RowNumber - 1) * RowLen, SeekOrigin.Begin)
        mFile.Write(Data, 0, RowLen)
        mRowNumber = RowNumber
        mState = FileState.fsNone
        mEof = False
        mBof = False
        Close()
    End Sub



    ' Lee el registro físico y pasa los valores de los campos
    Private Sub Read(ByVal RowNumber As Integer)
        Dim Data(RowLen) As Byte
        Dim Pos As Long


        ' Si no se logra hacer la lectura del registro se marca 
        ' el registro actual como indefinido
        mRowNumber = 0

        ' Se lee el registro 
        Open()
        Pos = (RowNumber - 1) * RowLen
        If mFile.Seek(Pos, SeekOrigin.Begin) <> Pos Then
            Throw New FileReadException(mFileName)
        End If
        If mFile.Read(Data, 0, RowLen) = RowLen Then
            ' Si el archivo esta encriptado se niegan todos los bytes
            If mEncrypted Then
                For I As Integer = 0 To Data.Length - 1
                    Data(I) = Not Data(I)
                Next
            End If

            RowData = Encoding.Default.GetString(Data, 0, RowLen)
            'GetEncoding(1252).GetString(Data, 0, RowLen)
            mRowNumber = RowNumber
            mState = FileState.fsNone
            mEof = False
            mBof = False
        Else
            Throw New FileReadException(mFileName)
        End If
        Close()
    End Sub

    ' Permite agregar campos a la definición de la estructura del archivo
    Protected Sub AddField(ByVal FieldName As String, ByVal Len As Integer, _
        Optional ByVal aSortOrderIndex As Integer = 0)
        Dim myField As FixlenFileField = New FixlenFileField(Me, FieldName, Len, RowLen - mRowSeparator.Length, aSortOrderIndex)
        If mFields.Count > 0 Then
            mRowLen += mFieldSeparator.Length + Len
        Else
            mRowLen = Len
        End If
        mFields.Add(myField)
        mFieldNames.Add(FieldName.Trim().ToUpper())
        If aSortOrderIndex > 0 Then
            If mKeyFieldNames Is Nothing OrElse _
                UBound(mKeyFieldNames) < aSortOrderIndex Then
                ReDim Preserve mKeyFieldNames(aSortOrderIndex)
            End If
            mKeyFieldNames(aSortOrderIndex - 1) = FieldName.Trim().ToUpper()
        End If
    End Sub

    Protected Sub InitRowData()
        ' Se forma la cadena que se guardara en el archivo
        Dim I As Integer
        Dim Pos As Integer = 0
        Dim myField As FixlenFileField
        RowData = ""
        For I = 0 To mFields.Count - 1
            myField = DirectCast(mFields(I), FixlenFileField)
            RowData &= Space(myField.Len)
            If I <> mFields.Count - 1 Then
                RowData &= mFieldSeparator
            End If
        Next
        RowData += mRowSeparator
    End Sub

    ' Permite agregar un registro en la posición especificada
    Public Sub Insert(Optional ByVal RowNumber As Integer = 0)
        ' Se limpia el valor de todos los campos y 
        ' se cambia el estado del registro
        mState = FileState.fsInsert
        If RowNumber <= 0 Then
            RowNumber = RowCount + 1
        End If
        mRowNumber = RowNumber
        InitRowData()
    End Sub

    ' Permite agregar un registro al final del archivo
    Public Sub Append()
        ' Se limpia el valor de todos los campos y 
        ' se cambia el estado del registro
        mState = FileState.fsAppend
        mRowNumber = RowCount + 1
        InitRowData()
    End Sub

    Private Function GetKeyFieldsValues(ByRef Values() As String) As Boolean
        Dim I As Integer
        If Not IsArray(mKeyFieldNames) Then
            Return False
        End If
        If UBound(mKeyFieldNames) > 0 Then
            ReDim Values(UBound(mKeyFieldNames))
            For I = 0 To UBound(mKeyFieldNames) - 1
                Values(I) = Me(mKeyFieldNames(I))
            Next
            Return True
        Else
            Return False
        End If
    End Function

    ' Borra el registro especificado
    Public Overridable Sub Delete(ByVal nReg As Integer)
        Dim Data(RowLen) As Byte
        Dim nRowLen As Integer = RowLen
        Dim I As Integer
        Dim nPos As Long
        Dim nRegs As Integer = Me.RowCount

        If nReg <= 0 Or nReg > RowCount Then
            Throw New ArgumentOutOfRangeException
        End If

        Open()
        ' Se mueven los registros para ocupar el espacio borrado
        For I = nReg + 1 To nRegs
            nPos = (I - 1) * nRowLen
            mFile.Seek(nPos, SeekOrigin.Begin)
            mFile.Read(Data, 0, nRowLen)
            mFile.Seek(nPos - RowLen, SeekOrigin.Begin)
            mFile.Write(Data, 0, nRowLen)
        Next
        mFile.SetLength(nRowLen * (nRegs - 1))
        Close()
    End Sub

    Const MAX_DATALEN As Integer = 65536 ' Se leen bloques de 64K

    Public Sub Post()
        Dim nReg As Integer
        Dim sData As String
        Static Buffer(MAX_DATALEN) As Byte

        If (mState = FileState.fsEdit Or mState = FileState.fsAppend) Or _
            (mState = FileState.fsInsert And Me.mKeyFieldNames Is Nothing) Then
            Write(mRowNumber)
        ElseIf mState = FileState.fsInsert Then
            Me.Open()

            ' Se determina la fila en la cual debe estar el registro
            Dim KeyValues() As String = Nothing
            If GetKeyFieldsValues(KeyValues) Then
                sData = RowData
                nReg = Me.BSearch(mKeyFieldNames, KeyValues)
                If nReg > 0 Then
                    mRowNumber = nReg
                ElseIf nReg = 0 Then
                    mRowNumber = 1
                Else
                    mRowNumber = -nReg
                End If
                RowData = sData
            Else
                mRowNumber = RowCount + 1
            End If

            ' Si el registro no va de ultimo se crea el espacio para guardarlo
            If mRowNumber <= RowCount Then

                Dim nDataLen As Integer
                Dim nInsertPos As Long = ((mRowNumber - 1) * RowLen)
                Dim nPosFinBlock As Long = mFile.Seek(0, SeekOrigin.End)
                Dim nPosIniBlock As Long = nInsertPos
                Do
                    ' Se valida que la nueva posición nunca 
                    ' mayor a la posición minima
                    If (nPosFinBlock - nInsertPos) > MAX_DATALEN Then
                        nPosIniBlock = nPosFinBlock - MAX_DATALEN
                        nDataLen = MAX_DATALEN
                    Else
                        nPosIniBlock = nInsertPos
                        nDataLen = CInt(nPosFinBlock - nInsertPos)
                    End If
                    mFile.Seek(nPosIniBlock, SeekOrigin.Begin)
                    mFile.Read(Buffer, 0, nDataLen)
                    mFile.Seek(nPosIniBlock + RowLen, SeekOrigin.Begin)
                    mFile.Write(Buffer, 0, nDataLen)
                    nPosFinBlock -= nDataLen
                Loop While nPosIniBlock > nInsertPos
            End If

            ' se guarda el registro
            Write(mRowNumber)
            Me.Close()
        End If
    End Sub

    Public Sub DeleteAll()
        Open()
        mFile.SetLength(0)
        Close()
    End Sub

    'Public Sub UpdateData(ByRef sRowsData As String, ByVal pb As ProgressBar)
    '    Dim I As Integer
    '    Dim sRows() As String
    '    Dim nRowLen As Integer = RowLen
    '    Dim Data(nRowLen) As Byte

    '    sRows = Split(sRowsData, vbCrLf)
    '    Me.Open()
    '    Me.DeleteAll()
    '    pb.Visible = True
    '    pb.Minimum = 0
    '    pb.Maximum = UBound(sRows)
    '    pb.Value = 0

    '    ' Se lee el registro y asigna el valor a los campos
    '    For I = 0 To UBound(sRows)
    '        If (Not sRows(I) Is Nothing) _
    '            And sRows(I).Trim() <> "" Then
    '            RowData = sRows(I) + vbCrLf
    '            Data = Encoding.Default.GetBytes(RowData)
    '            mFile.Seek(I * nRowLen, SeekOrigin.Begin)
    '            mFile.Write(Data, 0, nRowLen)
    '            pb.Value = I
    '        End If
    '    Next
    '    Me.Close()
    '    pb.Value = 0
    '    pb.Visible = False
    'End Sub

    Public Function GetData() As DataSet
        Dim ds As New DataSet
        Dim row As DataRow
        Dim I, J As Integer
        Dim myField As FixlenFileField
        Dim sAux As String = ""
        ds.Tables.Add(Me.Name)
        For I = 0 To mFields.Count - 1
            myField = DirectCast(mFields(I), FixlenFileField)
            ds.Tables(0).Columns.Add(myField.Name, sAux.GetType())
        Next
        Open()
        First()
        Do While Not Me.Eof
            row = ds.Tables(0).NewRow()
            For J = 0 To mFields.Count - 1
                myField = DirectCast(mFields(J), FixlenFileField)
                row(J) = myField.Value
            Next
            ds.Tables(0).Rows.Add(row)
            Me.Next()
        Loop
        Close()
        Return ds
    End Function

    'Public Function GetData(ByVal pb As ProgressBar) As DataSet
    '    Dim ds As New DataSet
    '    Dim row As DataRow
    '    Dim I, J As Integer
    '    Dim myField As FixlenFileField
    '    Dim sAux As String = ""
    '    ds.Tables.Add(Me.Name)
    '    For I = 0 To mFields.Count - 1
    '        myField = DirectCast(mFields(I), FixlenFileField)
    '        ds.Tables(0).Columns.Add(myField.Name, sAux.GetType())
    '    Next

    '    pb.Minimum = 0
    '    pb.Maximum = RowCount
    '    pb.Value = 0
    '    pb.Visible = True
    '    Open()
    '    First()
    '    Do While Not Me.Eof
    '        row = ds.Tables(0).NewRow()
    '        For J = 0 To mFields.Count - 1
    '            myField = DirectCast(mFields(J), FixlenFileField)
    '            row(J) = myField.Value
    '        Next
    '        ds.Tables(0).Rows.Add(row)
    '        pb.Value += 1
    '        Me.Next()
    '    Loop
    '    Close()
    '    pb.Value = 0
    '    pb.Visible = False
    '    Return ds
    'End Function

    Public Function GetFieldLengths() As Integer()
        Dim I As Integer
        Dim myField As FixlenFileField
        Dim FieldLengths(Me.mFields.Count - 1) As Integer
        For I = 0 To Me.mFields.Count - 1
            myField = DirectCast(Me.mFields(I), FixlenFileField)
            FieldLengths(I) = myField.Len
        Next
        Return FieldLengths
    End Function

    Public Function GetFieldNames() As String()
        Dim I As Integer
        Dim myField As FixlenFileField
        Dim FieldNames(Me.mFields.Count - 1) As String
        For I = 0 To Me.mFields.Count - 1
            myField = DirectCast(Me.mFields(I), FixlenFileField)
            FieldNames(I) = myField.Name
        Next
        Return FieldNames
    End Function
#End Region

#Region " Operaciones Navegación  "
    Public Sub [Next]()
        Dim nRow As Integer
        Dim nCurrentRow As Integer = mRowNumber
        Dim bFound As Boolean = False
        Dim nEndRow As Integer = RowCount
        Dim KeyFields() As String = Nothing
        Dim KeyValues() As String = Nothing
        If mFilters.GetKeyFields(KeyFields, KeyValues) Then
            nEndRow = BSearchLast(KeyFields, KeyValues)
        End If

        If (nCurrentRow < nEndRow) Then
            For nRow = nCurrentRow + 1 To nEndRow
                Read(nRow)
                If mFilters.Evaluate() Then
                    bFound = True
                    Exit For
                End If
            Next
            If Not bFound Then
                If nCurrentRow > 0 Then
                    Read(nCurrentRow)
                End If
                mEof = True
            End If
        Else
            mEof = True
        End If
    End Sub

    Public Sub Prior()
        Dim nRows As Integer = RowCount
        Dim nRow As Integer
        Dim nCurrentRow As Integer = mRowNumber
        Dim bFound As Boolean = False
        Dim nIniRow As Integer = 1
        Dim KeyFields() As String = Nothing
        Dim KeyValues() As String = Nothing
        If mFilters.GetKeyFields(KeyFields, KeyValues) Then
            nIniRow = BSearchFirst(KeyFields, KeyValues)
        End If

        If (nCurrentRow > nIniRow) Then
            For nRow = nCurrentRow - 1 To nIniRow Step -1
                Read(nRow)
                If mFilters.Evaluate() Then
                    bFound = True
                    Exit For
                End If
            Next
            If Not bFound Then
                If nCurrentRow > 0 Then
                    Read(nCurrentRow)
                End If
                mBof = True
            End If
        Else
            mBof = True
        End If
    End Sub

    Public Sub First()
        Dim nRows As Integer = RowCount
        Dim nRow, nIniRow, nEndRow As Integer
        Dim nCurrentRow As Integer = mRowNumber
        Dim KeyFields() As String = Nothing
        Dim KeyValues() As String = Nothing
        Dim bFound As Boolean = False

        If nRows > 0 Then
            nIniRow = 1
            nEndRow = nRows
            If mFilters.GetKeyFields(KeyFields, KeyValues) Then
                nIniRow = BSearchFirst(KeyFields, KeyValues)
                nEndRow = BSearchLast(KeyFields, KeyValues)
            End If
            If nIniRow > 0 Then
                For nRow = nIniRow To nEndRow
                    Read(nRow)
                    If mFilters.Evaluate() Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If
            If Not bFound Then
                mRowNumber = 0
                mEof = True
                mBof = True
            End If
        Else
            mEof = True
            mBof = True
            mRowNumber = 0
        End If
    End Sub

    Public Sub Last()
        Dim nRows As Integer = RowCount
        Dim nRow, nIniRow, nEndRow As Integer
        Dim KeyFields() As String = Nothing
        Dim KeyValues() As String = Nothing
        Dim nCurrentRow As Integer = mRowNumber
        Dim bFound As Boolean = False

        If nRows > 0 Then
            nIniRow = 1
            nEndRow = nRows
            If mFilters.GetKeyFields(KeyFields, KeyValues) Then
                nIniRow = BSearchFirst(KeyFields, KeyValues)
                nEndRow = BSearchLast(KeyFields, KeyValues)
            End If

            If nIniRow > 0 Then
                For nRow = nEndRow To nIniRow Step -1
                    Read(nRow)
                    If mFilters.Evaluate() Then
                        bFound = True
                        Exit For
                    End If
                Next
            End If
            If Not bFound Then
                mRowNumber = 0
                mEof = True
                mBof = True
            End If
            Read(RowCount)
        Else
            mEof = True
            mBof = True
            mRowNumber = 0
        End If
    End Sub

    Public Sub MoveTo(ByVal RowNumber As Integer)
        Dim Rows As Integer = RowCount
        If Rows > 0 And RowNumber <= Rows Then
            Read(RowNumber)
        Else
            Throw New RowOutRangeException(Me.mFileName)
        End If
    End Sub

#End Region

#Region " Funciones de Busqueda y Filtros "
    ' Los filtros actuan sobre las funciones First, Last, Next, Prior
    Private mFilters As New FieldsCriteria(Me)   ' Filtros actuales del archivo
    Private mIniRow As Integer = -1
    Private mEndRow As Integer = -1

    ' El valor del campo que hace parte del filtro puede usar el comodin * 
    ' para hacer busquedas parciales
    Public Sub AddFilter(ByVal sFieldName As String, ByVal sValue As String)
        mFilters.Add(sFieldName, sValue)
        mBSearchFirstReg = -1
        mBSearchLastReg = -1
        Me.mBof = True
        Me.mEof = True
        Me.mRowNumber = 0
    End Sub

    Public Sub ClearFilters()
        mFilters.Clear()
        mBSearchFirstReg = -1
        mBSearchLastReg = -1
    End Sub

    ' Busqueda secuencial por un campo
    Public Function Search(ByVal FieldName As String, _
        ByVal FieldValue As String) As Integer
        Dim FieldNames(1) As String
        Dim FieldValues(1) As String
        FieldNames(0) = FieldName
        FieldValues(0) = FieldValue
        Return Search(FieldNames, FieldValues)
    End Function

    ' Busqueda secuencial por dos campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String) As Integer

        Dim Values(2) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"
        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 1 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(2)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Busqueda secuencial por tres campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String, ByVal Field3Value As String) As Integer
        Dim Values(3) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"

        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 2 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(3)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Fields(2) = Fields(2).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Values(2) = Field3Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Busqueda secuencial por cuatro campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String, ByVal Field3Value As String, _
    ByVal Field4Value As String) As Integer
        Dim Values(4) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"

        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 3 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(4)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Fields(2) = Fields(2).Trim()
        Fields(3) = Fields(3).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Values(2) = Field3Value.Trim()
        Values(3) = Field4Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Busqueda secuencial por cinco campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String, ByVal Field3Value As String, _
        ByVal Field4Value As String, ByVal Field5Value As String) As Integer
        Dim Values(5) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"

        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 4 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(5)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Fields(2) = Fields(2).Trim()
        Fields(3) = Fields(3).Trim()
        Fields(4) = Fields(4).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Values(2) = Field3Value.Trim()
        Values(3) = Field4Value.Trim()
        Values(4) = Field5Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Busqueda secuencial por seis campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String, ByVal Field3Value As String, _
        ByVal Field4Value As String, ByVal Field5Value As String, _
        ByVal Field6Value As String) As Integer
        Dim Values(6) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"

        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 5 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(6)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Fields(2) = Fields(2).Trim()
        Fields(3) = Fields(3).Trim()
        Fields(4) = Fields(4).Trim()
        Fields(5) = Fields(5).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Values(2) = Field3Value.Trim()
        Values(3) = Field4Value.Trim()
        Values(4) = Field5Value.Trim()
        Values(5) = Field6Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Busqueda secuencial por seis campos
    Public Function Search(ByVal FieldNames As String, ByVal Field1Value As String, _
        ByVal Field2Value As String, ByVal Field3Value As String, _
        ByVal Field4Value As String, ByVal Field5Value As String, _
        ByVal Field6Value As String, ByVal Field7Value As String) As Integer
        Dim Values(7) As String
        Dim Fields() As String
        Dim sDelimiters As String = ",;"

        Fields = FieldNames.Split(sDelimiters.ToCharArray())
        If UBound(Fields) <> 6 Then
            Throw New SearchParametersException(Me.mFileName)
        End If
        ReDim Preserve Fields(7)

        Fields(0) = Fields(0).Trim()
        Fields(1) = Fields(1).Trim()
        Fields(2) = Fields(2).Trim()
        Fields(3) = Fields(3).Trim()
        Fields(4) = Fields(4).Trim()
        Fields(5) = Fields(5).Trim()
        Fields(6) = Fields(6).Trim()
        Values(0) = Field1Value.Trim()
        Values(1) = Field2Value.Trim()
        Values(2) = Field3Value.Trim()
        Values(3) = Field4Value.Trim()
        Values(4) = Field5Value.Trim()
        Values(5) = Field6Value.Trim()
        Values(6) = Field7Value.Trim()
        Return Search(Fields, Values)
    End Function

    ' Realiza la busqueda de la forma más optima posible
    Protected Function Search(ByVal FieldNames() As String, ByVal FieldValues() As String) As Integer
        Try
            ' Se crean filtros temporales de acuerdo a los criterios de busqueda
            Dim I, nReg As Integer
            If Me.RowCount <= 0 Then
                nReg = -1
            Else
                ClearFilters()
                For I = 0 To UBound(FieldNames) - 1
                    AddFilter(FieldNames(I), FieldValues(I))
                Next
                Me.First()
                nReg = Me.CurrentRow
                If nReg <= 0 Then
                    nReg = -1
                End If
                ClearFilters()
            End If
            Return nReg
        Catch ex As Exception
            Throw ex
            '#If NETCF20 Then
            '   MovilidadCF.Logging.Logger.Write(ex)
            '#Else
            '   Desktop.Logging.Logger.Write(ex)
            '#End If
        End Try
    End Function

    ' Realiza la busqueda secuencial
    Private Function SSearch(ByVal FieldNames() As String, ByVal FieldValues() As String) As Integer
        Dim I As Integer
        Dim bFound As Boolean
        Dim nRow As Integer
        If Me.RowCount <= 0 Then
            Return 0
        End If

        nRow = mRowNumber


        ' Se realiza la busqueda
        Me.Open()
        Me.First()
        Do While Not Eof
            bFound = True
            For I = 0 To UBound(FieldNames) - 1
                If (Me(FieldNames(I)) <> FieldValues(I).Trim()) Then
                    bFound = False
                    Exit For
                End If
            Next
            If bFound Then
                Exit Do
            End If
            Me.Next()
        Loop
        ' Se cierra el archivo
        Me.Close()

        If bFound Then
            Return mRowNumber
        Else
            If (mState = FileState.fsNone Or mState = FileState.fsEdit) And nRow > 0 Then
                Read(nRow)
            Else
                Read(1)
            End If
            Return 0
        End If
    End Function

    ' Busqueda binaria Por Multiples campos. 
    Private mBSearchRegIni As Integer
    Private mBSearchRegFin As Integer

    Private Function BSearch(ByVal FieldNames() As String, _
        ByVal FieldValues() As String) As Integer

        Dim nLastReg As Integer
        Dim nRows As Integer
        Dim nReg As Integer = 0
        Dim nFirstReg As Integer
        Dim bFound As Boolean = False
        Dim nRes As Integer
        Dim nInsertPos As Integer

        Me.Open()
        nRows = RowCount
        nLastReg = nRows
        nFirstReg = 1
        If nRows > 0 Then
            Do
                ' Se lee el primer registro
                nReg = nFirstReg
                Me.MoveTo(nReg)
                nRes = CompareRow(FieldNames, FieldValues)
                If nRes = 0 Then
                    bFound = True
                    Exit Do
                End If
                If nRes < 0 Then
                    bFound = False
                    nInsertPos = 1
                    Exit Do
                End If
                If nRes > 0 Then
                    ' Se lee el último registro
                    nReg = nLastReg
                    Me.MoveTo(nReg)
                    nRes = CompareRow(FieldNames, FieldValues)
                    If nRes = 0 Then
                        bFound = True
                        Exit Do
                    ElseIf nRes > 0 Then
                        bFound = False
                        nInsertPos = nLastReg + 1
                        Exit Do
                    ElseIf nRes < 0 Then
                        If (nLastReg - nFirstReg) > 1 Then
                            nReg = nFirstReg + CInt((nLastReg - nFirstReg) / 2)
                            Me.MoveTo(nReg)
                            nRes = Me.CompareRow(FieldNames, FieldValues)
                            If nRes = 0 Then
                                bFound = True
                                Exit Do
                            ElseIf (nRes > 0) Then
                                nFirstReg = nReg
                            ElseIf (nRes < 0) Then
                                nLastReg = nReg
                            End If
                        Else
                            bFound = False
                            nInsertPos = nLastReg
                            Exit Do
                        End If
                    End If
                End If
            Loop
        Else
            bFound = False
            nInsertPos = nLastReg
        End If
        Me.Close()
        If bFound Then
            mBSearchRegIni = nFirstReg
            mBSearchRegFin = nLastReg
            Return nReg
        Else
            Return -nInsertPos
        End If
    End Function

    ' Busca el primer registro
    Private mBSearchFirstReg As Integer = -1
    Private Function BSearchFirst(ByVal FieldNames() As String, _
        ByVal FieldValues() As String) As Integer
        Dim nReg As Integer
        If mBSearchFirstReg > 0 Then
            Return mBSearchFirstReg
        End If

        Me.Open()
        nReg = BSearch(FieldNames, FieldValues)
        If mBSearchRegIni <> nReg Then
            Do While (mBSearchRegFin - mBSearchRegIni) > 1
                nReg = mBSearchRegIni + CInt((mBSearchRegFin - mBSearchRegIni) / 2)
                Me.MoveTo(nReg)
                If Me.CompareRow(FieldNames, FieldValues) = 0 Then
                    mBSearchRegFin = nReg
                Else
                    mBSearchRegIni = nReg
                End If
            Loop
        End If
        Me.Close()
        mBSearchFirstReg = nReg
        Return nReg
    End Function

    ' Busca el último registro
    Private mBSearchLastReg As Integer = -1
    Private Function BSearchLast(ByVal FieldNames() As String, _
        ByVal FieldValues() As String) As Integer
        Dim nReg, nRegs As Integer
        If mBSearchLastReg > 0 Then
            Return mBSearchLastReg
        End If

        Me.Open()
        nRegs = Me.RowCount
        nReg = BSearch(FieldNames, FieldValues)
        nReg = BSearch(FieldNames, FieldValues)
        If mBSearchRegFin <> nReg Then
            Do While (mBSearchRegFin - mBSearchRegIni) > 1
                nReg = mBSearchRegIni + CInt((mBSearchRegFin - mBSearchRegIni) / 2)
                Me.MoveTo(nReg)
                If Me.CompareRow(FieldNames, FieldValues) = 0 Then
                    mBSearchRegIni = nReg
                Else
                    mBSearchRegFin = nReg
                End If
            Loop
        End If
        Me.Close()
        mBSearchLastReg = nReg
        Return nReg
    End Function

    ' Compara el valor actual de los campos especificados con los valores pasados
    Private Function CompareRow(ByVal FieldNames() As String, ByVal FieldValues() As String) As Integer
        Dim nRes As Integer = 0
        Dim I As Integer
        Dim s1, s2 As String
        For I = 0 To UBound(FieldNames) - 1
            s1 = FieldValues(I).Trim()
            s2 = Me(FieldNames(I))
            If s1.Length >= 1 AndAlso s1.Chars(s1.Length - 1) = FieldsCriteria.COMODIN Then
                s2 = Left(s2, s1.Length - 1).Trim()
                s1 = s1.Remove(s1.Length - 1, 1)
            End If
            nRes = s1.CompareTo(s2)
            If nRes <> 0 Then
                Exit For
            End If
        Next
        Return nRes
    End Function

#End Region

#Region "Funciones de manipulación de controles adicionales"
    'Public Sub FillLookupControl(ByVal Control As DSControls.ILookupControl, _
    '    ByVal KeyField As String, ByVal ListField As String)
    '    Me.Open()
    '    Me.First()
    '    Control.ClearValues()
    '    Do While Not Me.Eof
    '        Control.AddValues(Me(KeyField), Me(ListField))
    '        Me.Next()
    '    Loop
    '    Me.Close()
    'End Sub


    'Public Sub Browse(ByVal sFields As String, ByVal ParamArray nColumWidths() As Integer)
    '    Dim form As New frmBrowse
    '    form.Browse(Nothing, Me, sFields, nColumWidths)
    'End Sub

    'Public Sub Browse()
    '    Dim form As New frmBrowse
    '    form.Browse(Nothing, Me, "")
    'End Sub

    'Public Sub Browse(ByVal EventHandler As BrowseEvent)
    '    Dim form As New frmBrowse
    '    form.Browse(EventHandler, Me, "")
    'End Sub

    'Public Sub Browse(ByVal EventHandler As BrowseEvent, ByVal sFields As String, ByVal ParamArray nColumWidths() As Integer)
    '    Dim form As New frmBrowse
    '    form.Browse(EventHandler, Me, sFields, nColumWidths)
    'End Sub

#End Region

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


