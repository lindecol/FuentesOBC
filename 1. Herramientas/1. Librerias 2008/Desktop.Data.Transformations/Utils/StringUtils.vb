Module StringUtils

    Function UnescapeString(ByVal s As String) As String
        '\' - Single quote 0x0027
        If s.IndexOf("\'") >= 0 Then
            s = s.Replace("\'", "'")
        End If
        '\" - Double quote 0x0022
        If s.IndexOf("\" & Chr(&H22)) >= 0 Then
            s = s.Replace("\" & Chr(&H22), Chr(&H22))
        End If
        '\0 - Null 0x0000
        If s.IndexOf("\0") >= 0 Then
            s = s.Replace("\0", Chr(&H0))
        End If
        '\a - Alert 0x0007
        If s.IndexOf("\a") >= 0 Then
            s = s.Replace("\a", Chr(&H7))
        End If
        '\b - Backspace 0x0008
        If s.IndexOf("\b") >= 0 Then
            s = s.Replace("\b", Chr(&H8))
        End If
        '\f - Form feed 0x000C
        If s.IndexOf("\f") >= 0 Then
            s = s.Replace("\f", Chr(&HC))
        End If
        '\n - New line 0x000A
        If s.IndexOf("\n") >= 0 Then
            s = s.Replace("\n", vbLf)
        End If
        '\r - Carriage return 0x000D
        If s.IndexOf("\r") >= 0 Then
            s = s.Replace("\r", Chr(&HD))
        End If
        '\t - Horizontal tab 0x0009
        If s.IndexOf("\t") >= 0 Then
            s = s.Replace("\t", Chr(&H9))
        End If
        '\v - Vertical tab 0x000B
        If s.IndexOf("\v") >= 0 Then
            s = s.Replace("\v", Chr(&HB))
        End If
        '\\ - Backslash 0x005C
        If s.IndexOf("\\") >= 0 Then
            s = s.Replace("\\", Chr(&H5C))
        End If
        Return s
    End Function

End Module
