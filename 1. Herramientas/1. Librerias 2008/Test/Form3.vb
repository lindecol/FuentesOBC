Imports Desktop.Windows.Forms

Public Class Form3
    Private Sub ConfigureListBar()
        'ListBar1.BackColor = Color.FromArgb(185, 216, 255)
        ListBar1.Left = 0
        ListBar1.Width = Me.Width + 1
        ListBar1.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Top
        ListBar1.DrawStyle = ListBarDrawStyle.ListBarDrawStyleOfficeXP
    End Sub

    Public Sub AddApplicationGroups(ByVal modulosAdicionar As List(Of Modulo))
        ' Se agregan los grupos y aplicaciones de cada grupo
        Dim group As ListBarGroup
        Dim groupItems(modulosAdicionar.Count - 1) As ListBarItem

        For i As Integer = 0 To modulosAdicionar.Count - 1
            groupItems(i) = New ListBarItem(modulosAdicionar(i).Descripcion, modulosAdicionar(i).Icono)
            groupItems(i).Tag = modulosAdicionar(i).Clase
        Next

        Me.ListBar1.Groups.Clear()
        group = New ListBarGroup("Administración del Sistema", groupItems)
        group.BackColor = Color.DarkRed
        group.View = ListBarGroupView.SmallIcons
        group.ForeColor = Color.White
        group.Font = New System.Drawing.Font("Tahoma", 12, FontStyle.Regular)
        ListBar1.Groups.Add(group)
    End Sub

    Private Sub Form3_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.ConfigureListBar()

        Dim list As New List(Of Modulo)

        For i As Int16 = 0 To 5
            list.Add(New Modulo())
        Next

        AddApplicationGroups(list)

    End Sub

    Private Sub ListBar1_ItemClicked(ByVal sender As System.Object, ByVal e As Desktop.Windows.Forms.ItemClickedEventArgs) Handles ListBar1.ItemClicked
        'e.Item.Enabled = False
        'e.Item.BackColor = Color.Black
        'e.Item.ForeColor = Color.SkyBlue
    End Sub
End Class

Public Class Modulo
    Public Sub New()
        codigo_val = 1
        descripcion_val = "test"
        icono_val = 1
        clase_val = "clase"
    End Sub

    Private codigo_val As Integer
    Private descripcion_val As String
    Private icono_val As Integer
    Private clase_val As String

    Public Property Codigo() As Integer
        Get
            Return codigo_val
        End Get
        Set(ByVal value As Int32)
            codigo_val = value
        End Set
    End Property
    Public Property Descripcion() As String
        Get
            Return descripcion_val
        End Get
        Set(ByVal value As String)
            descripcion_val = value
        End Set
    End Property
    Public Property Icono() As Integer
        Get
            Return icono_val
        End Get
        Set(ByVal value As Integer)
            icono_val = value
        End Set
    End Property
    Public Property Clase() As String
        Get
            Return clase_val
        End Get
        Set(ByVal value As String)
            clase_val = value
        End Set
    End Property
End Class


