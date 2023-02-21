Imports System.Windows.Forms

Public Class BaseViewControl

    Protected m_PermisosEspeciales As List(Of PermisoEspecial)

    Protected ReadOnly Property PermiteInsertar() As Boolean
        Get
            Return FrameworkApp.PermiteInsertar(IdModulo)
        End Get
    End Property


    Protected ReadOnly Property PermiteEditar() As Boolean
        Get
            Return FrameworkApp.PermiteEditar(IdModulo)
        End Get
    End Property

    Protected ReadOnly Property PermiteBorrar() As Boolean
        Get
            Return FrameworkApp.PermiteBorrar(IdModulo)
        End Get
    End Property

    Protected ReadOnly Property PermiteImprimir() As Boolean
        Get
            Return FrameworkApp.PermiteImprimir(IdModulo)
        End Get
    End Property

    Protected Function PermitePermisoEspecial(ByVal IdPermiso As Integer) As Boolean
        Return FrameworkApp.PermitePermisoEspecial(IdModulo, IdPermiso)
    End Function

    Private m_IdModulo As Integer = -1

    Private ReadOnly Property IdModulo() As Integer
        Get
            If m_IdModulo = -1 Then
                m_IdModulo = FrameworkApp.GetIdModulo(Me)
            End If
            Return m_IdModulo
        End Get
    End Property

    Public ReadOnly Property PermisosEspeciales() As List(Of PermisoEspecial)
        Get
            If m_PermisosEspeciales Is Nothing Then
                m_PermisosEspeciales = New List(Of PermisoEspecial)
                LoadPermisosEspeciales()
            End If
            Return m_PermisosEspeciales
        End Get
    End Property

    Protected Overridable Sub LoadPermisosEspeciales()
        ' Por defecto Se deja la lista de permisos especiales vacia
    End Sub

    Public Overridable ReadOnly Property MergeMenuStrip() As ContextMenuStrip
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable ReadOnly Property MergeToolStrip() As ToolStrip
        Get
            Return Nothing
        End Get
    End Property

    Public Overridable Sub ActualizarDatos()

    End Sub

    Public Overridable Sub Close()

    End Sub
End Class
