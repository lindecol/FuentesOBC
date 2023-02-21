Public Interface INucleo

    ReadOnly Property UsuarioActual() As DataRow

    ReadOnly Property EmpresaActual() As DataRow

    ReadOnly Property InicioSesion() As DateTime

    Sub RegisterModalForm(ByVal frm As System.Windows.Forms.Form)

    Function GetIdModulo(ByVal ctlView As BaseViewControl) As Integer

    Function PermiteInsertar(ByVal IdModulo As Integer) As Boolean

    Function PermiteEditar(ByVal IdModulo As Integer) As Boolean

    Function PermiteBorrar(ByVal IdModulo As Integer) As Boolean

    Function PermiteImprimir(ByVal IdModulo As Integer) As Boolean

    Function PermitePermisoEspecial(ByVal IdModulo As Integer, ByVal IdPermiso As Integer) As Boolean

End Interface