Public Module GUIUtils
    Public Function Confirmar(ByVal sMsg As String, Optional ByVal bDefualtYes As Boolean = True) As Boolean
        If bDefualtYes Then
            Return MsgBox(sMsg, MsgBoxStyle.Question Or MsgBoxStyle.YesNo, "Confirmar!") = MsgBoxResult.Yes
        Else
            Return MsgBox(sMsg, MsgBoxStyle.Question Or MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2, "Confirmar!") = MsgBoxResult.Yes
        End If
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
