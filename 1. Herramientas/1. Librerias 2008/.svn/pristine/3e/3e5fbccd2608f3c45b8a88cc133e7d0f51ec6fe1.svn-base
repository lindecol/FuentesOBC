' Clase que se encarga de almacenar los datos 
' relativos a un campo del archivo
Friend Class FixlenFileField
    ' Campos y Propiedades de la clase
    Protected mName As String   'Nombre del campo
    Protected mLen As Integer   'Longitud del campo en el archivo
    Protected mPos As Integer   'Posicion a partir de la cual se debe obtener el dato en el buffer
    Protected mSortOrderIndex As Integer 'Define el nivel
    Protected mOwner As FixlenFile

    Public ReadOnly Property Name() As String
        Get
            Return mName
        End Get
    End Property

    Public ReadOnly Property Len() As Integer
        Get
            Return mLen
        End Get
    End Property

    Public Property Value() As String
        Get
            Return mOwner.RowData.Substring(mPos, mLen).Trim()
        End Get
        Set(ByVal aValue As String)
            aValue = aValue.PadRight(mLen).Substring(0, Len)
            mOwner.RowData = mOwner.RowData.Remove(mPos, mLen)
            mOwner.RowData = mOwner.RowData.Insert(mPos, aValue)
        End Set
    End Property

    Public ReadOnly Property SortOrderIndex() As Integer
        Get
            Return mSortOrderIndex
        End Get
    End Property

    ' Funciones de la clase
    Sub New(ByRef aOwner As FixlenFile, ByVal aName As String, _
        ByVal aLen As Integer, ByVal aPos As Integer, _
        ByVal aSortOrderIndex As Integer)
        mOwner = aOwner
        mName = aName.Trim()
        mLen = aLen
        mPos = aPos
        mSortOrderIndex = aSortOrderIndex
    End Sub
End Class