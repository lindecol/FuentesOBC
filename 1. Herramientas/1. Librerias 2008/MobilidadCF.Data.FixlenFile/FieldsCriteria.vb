Friend Class FieldsCriteria

    Private Enum FieldCriteriaTypes
        EqualTo = 1
        BeginWith = 2
        Contain = 3
        EndWith = 4
    End Enum

    Private Class FieldCriteria
        Implements IComparable
        Public FieldName As String
        Public Type As FieldCriteriaTypes
        Public Value As String
        Public SortOrderIndex As Integer
        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            Return SortOrderIndex.CompareTo(CType(obj, FieldCriteria).SortOrderIndex)
        End Function
    End Class

    Private mFile As FixlenFile
    Private mCriterios() As FieldCriteria
    Private mCount As Integer
    Friend Shared COMODIN As Char = "*"c

    Public Sub New(ByVal File As FixlenFile)
        mFile = File
        mCount = 0
    End Sub

    ReadOnly Property Count() As Integer
        Get
            Return mCount
        End Get
    End Property

    Public Sub Add(ByVal FieldName As String, ByVal Value As String)
        Dim Criteria As New FieldCriteria
        Dim Type As FieldCriteriaTypes
        Value = Value.Trim()
        If Value = COMODIN Then
            ' No es realmente una expresión de filtro
            Exit Sub
        End If

        If Value = "" Then
            Type = FieldCriteriaTypes.EqualTo
        ElseIf Value.Chars(0) = COMODIN Then
            If Value.Chars(Value.Length - 1) = COMODIN Then
                Type = FieldCriteriaTypes.Contain
                Value = Value.Remove(0, 1)
                Value = Value.Remove(Value.Length - 1, 1)
            Else
                Type = FieldCriteriaTypes.EndWith
                Value = Value.Remove(0, 1)
            End If
        Else
            If Value.Chars(Value.Length - 1) = COMODIN Then
                Type = FieldCriteriaTypes.BeginWith
                Value = Value.Remove(Value.Length - 1, 1)
            Else
                Type = FieldCriteriaTypes.EqualTo
            End If
        End If
        If mCount < 10 Then
            ReDim Preserve mCriterios(mCount)
            Criteria.FieldName = FieldName
            Criteria.Type = Type
            Criteria.Value = Value
            Criteria.SortOrderIndex = mFile.Fields(FieldName).SortOrderIndex
            mCriterios(mCount) = Criteria
            Array.Sort(mCriterios)
            mCount += 1
        End If
    End Sub

    Public Sub Clear()
        Erase mCriterios
        mCount = 0
    End Sub

    Public Function Evaluate() As Boolean
        Dim I As Integer
        Dim bPassFilter As Boolean = True
        For I = 0 To mCount - 1
            Select Case mCriterios(I).Type
                Case FieldCriteriaTypes.EqualTo
                    If Not mFile(mCriterios(I).FieldName) = mCriterios(I).Value Then
                        bPassFilter = False
                    End If
                Case FieldCriteriaTypes.Contain
                    If mFile(mCriterios(I).FieldName).IndexOf(mCriterios(I).Value) < 0 Then
                        bPassFilter = False
                    End If
                Case FieldCriteriaTypes.BeginWith
                    If Not mFile(mCriterios(I).FieldName).StartsWith(mCriterios(I).Value) Then
                        bPassFilter = False
                    End If
                Case FieldCriteriaTypes.EndWith
                    If Not mFile(mCriterios(I).FieldName).EndsWith(mCriterios(I).Value) Then
                        bPassFilter = False
                    End If
            End Select
            If Not bPassFilter Then
                Exit For
            End If
        Next
        Return bPassFilter
    End Function

    Public Function GetKeyFields(ByRef KeyFields() As String, ByRef KeyValues() As String) As Boolean
        ' Se determina cuantos campos tiene la llave de busqueda
        Dim I As Integer
        Dim nKeyFields As Integer = 0
        Dim nLastIndex As Integer = 0
        For I = 0 To mCount - 1
            If (mCriterios(I).Type = FieldCriteriaTypes.BeginWith OrElse _
                mCriterios(I).Type = FieldCriteriaTypes.EqualTo) AndAlso _
                mCriterios(I).SortOrderIndex - nLastIndex = 1 Then
                nKeyFields += 1
                nLastIndex = mCriterios(I).SortOrderIndex
            Else
                Exit For
            End If
        Next

        If nKeyFields > 0 Then
            ReDim KeyFields(nKeyFields)
            ReDim KeyValues(nKeyFields)
            For I = 0 To nKeyFields - 1
                KeyFields(I) = mCriterios(I).FieldName
                If mCriterios(I).Type = FieldCriteriaTypes.BeginWith Then
                    KeyValues(I) = mCriterios(I).Value & "*"
                Else
                    KeyValues(I) = mCriterios(I).Value
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

End Class
