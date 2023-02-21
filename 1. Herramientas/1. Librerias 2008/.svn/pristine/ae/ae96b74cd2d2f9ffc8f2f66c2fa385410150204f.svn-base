Module GUIUtils
    Public Function Confirmar(ByVal sMsg As String) As Boolean
        Return MsgBox(sMsg, MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Confirmar!") = MsgBoxResult.Yes
    End Function

    Public Function ShowError(ByVal sMsg As String) As Boolean
        MsgBox(sMsg, MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "Error!")
    End Function

    Public Function ShowAlert(ByVal sMsg As String) As Boolean
        MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Advertencia!")
    End Function

    Public Function ShowInfo(ByVal sMsg As String) As Boolean
        MsgBox(sMsg, MsgBoxStyle.Exclamation Or MsgBoxStyle.OkOnly, "Información!")
    End Function
End Module
