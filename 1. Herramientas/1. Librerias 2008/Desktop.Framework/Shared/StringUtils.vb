Imports System.Collections.Specialized

Public Module StringUtils

    Function UnescapeString(ByVal s As String) As String
        '\' - Single quote 0x0027
        If s.IndexOf("\'") > 0 Then
            s = s.Replace("\'", "'")
        End If
        '\" - Double quote 0x0022
        If s.IndexOf("\" & Chr(&H22)) > 0 Then
            s = s.Replace("\" & Chr(&H22), Chr(&H22))
        End If
        '\0 - Null 0x0000
        If s.IndexOf("\0") > 0 Then
            s = s.Replace("\0", Chr(&H0))
        End If
        '\a - Alert 0x0007
        If s.IndexOf("\a") > 0 Then
            s = s.Replace("\a", Chr(&H7))
        End If
        '\b - Backspace 0x0008
        If s.IndexOf("\b") > 0 Then
            s = s.Replace("\b", Chr(&H8))
        End If
        '\f - Form feed 0x000C
        If s.IndexOf("\f") > 0 Then
            s = s.Replace("\f", Chr(&HC))
        End If
        '\n - New line 0x000A
        If s.IndexOf("\n") > 0 Then
            s = s.Replace("\n", vbLf)
        End If
        '\r - Carriage return 0x000D
        If s.IndexOf("\r") > 0 Then
            s = s.Replace("\r", Chr(&HD))
        End If
        '\t - Horizontal tab 0x0009
        If s.IndexOf("\v") > 0 Then
            s = s.Replace("\v", Chr(&H9))
        End If
        '\v - Vertical tab 0x000B
        If s.IndexOf("\v") > 0 Then
            s = s.Replace("\v", Chr(&HB))
        End If
        '\\ - Backslash 0x005C
        If s.IndexOf("\\") > 0 Then
            s = s.Replace("\\", Chr(&H5C))
        End If
        Return s
    End Function

    ' Obtiene los argumentos de un comando
    Public Function ParseCommand(ByVal sCommand As String) As StringCollection
        Dim I As Integer
        Dim sArguments As New StringCollection
        Dim sCurrentArg As String = ""
        Dim c As Char
        Dim bTextArgument As Boolean = False
        For I = 0 To sCommand.Length - 1
            c = sCommand.Chars(I)
            If c = """"c Or c = "'"c Then
                bTextArgument = Not bTextArgument
            ElseIf (Not bTextArgument) AndAlso (c = " "c OrElse c = Chr(9)) Then
                If sCurrentArg <> "" Then
                    sArguments.Add(sCurrentArg)
                    sCurrentArg = ""
                End If
            Else
                sCurrentArg &= c
            End If
        Next
        If sCurrentArg <> "" Then
            sArguments.Add(sCurrentArg)
            sCurrentArg = ""
        End If
        Return sArguments
    End Function

    'Public Function MontoEscrito(ByVal value As Double) As String
    '    Select Case value
    '        Case 0 : MontoEscrito = "CERO"
    '        Case 1 : MontoEscrito = "UN"
    '        Case 2 : MontoEscrito = "DOS"
    '        Case 3 : MontoEscrito = "TRES"
    '        Case 4 : MontoEscrito = "CUATRO"
    '        Case 5 : MontoEscrito = "CINCO"
    '        Case 6 : MontoEscrito = "SEIS"
    '        Case 7 : MontoEscrito = "SIETE"
    '        Case 8 : MontoEscrito = "OCHO"
    '        Case 9 : MontoEscrito = "NUEVE"
    '        Case 10 : MontoEscrito = "DIEZ"
    '        Case 11 : MontoEscrito = "ONCE"
    '        Case 12 : MontoEscrito = "DOCE"
    '        Case 13 : MontoEscrito = "TRECE"
    '        Case 14 : MontoEscrito = "CATORCE"
    '        Case 15 : MontoEscrito = "QUINCE"
    '        Case Is < 20 : MontoEscrito = "DIECI" & MontoEscrito(value - 10)
    '        Case 20 : MontoEscrito = "VEINTE"
    '        Case Is < 30 : MontoEscrito = "VEINTI" & MontoEscrito(value - 20)
    '        Case 30 : MontoEscrito = "TREINTA"
    '        Case 40 : MontoEscrito = "CUARENTA"
    '        Case 50 : MontoEscrito = "CINCUENTA"
    '        Case 60 : MontoEscrito = "SESENTA"
    '        Case 70 : MontoEscrito = "SETENTA"
    '        Case 80 : MontoEscrito = "OCHENTA"
    '        Case 90 : MontoEscrito = "NOVENTA"
    '        Case Is < 100 : MontoEscrito = MontoEscrito(Int(value \ 10) * 10) & " Y " & MontoEscrito(value Mod 10)
    '        Case 100 : MontoEscrito = "CIEN"
    '        Case Is < 200 : MontoEscrito = "CIENTO " & MontoEscrito(value - 100)
    '        Case 200, 300, 400, 600, 800 : MontoEscrito = MontoEscrito(Int(value \ 100)) & "CIENTOS"
    '        Case 500 : MontoEscrito = "QUINIENTOS"
    '        Case 700 : MontoEscrito = "SETECIENTOS"
    '        Case 900 : MontoEscrito = "NOVECIENTOS"
    '        Case Is < 1000 : MontoEscrito = MontoEscrito(Int(value \ 100) * 100) & " " & MontoEscrito(value Mod 100)
    '        Case 1000 : MontoEscrito = "MIL"
    '        Case Is < 2000 : MontoEscrito = "MIL " & MontoEscrito(value Mod 1000)
    '        Case Is < 1000000 : MontoEscrito = MontoEscrito(Int(value \ 1000)) & " MIL"
    '            If value Mod 1000 Then MontoEscrito = MontoEscrito & " " & MontoEscrito(value Mod 1000)
    '        Case 1000000 : MontoEscrito = "UN MILLON"
    '        Case Is < 2000000 : MontoEscrito = "UN MILLON " & MontoEscrito(value Mod 1000000)
    '        Case Is < 1000000000000.0# : MontoEscrito = MontoEscrito(Int(value / 1000000)) & " MILLONES "
    '            If (value - Int(value / 1000000) * 1000000) Then MontoEscrito = MontoEscrito & " " & MontoEscrito(value - Int(value / 1000000) * 1000000)
    '        Case 1000000000000.0# : MontoEscrito = "UN BILLON"
    '        Case Is < 2000000000000.0# : MontoEscrito = "UN BILLON " & MontoEscrito(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
    '        Case Else : MontoEscrito = MontoEscrito(Int(value / 1000000000000.0#)) & " BILLONES"
    '            If (value - Int(value / 1000000000000.0#) * 1000000000000.0#) Then MontoEscrito = MontoEscrito & " " & MontoEscrito(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
    '    End Select
    'End Function

    Public Function ValidarNumeros(ByVal sCadena As String) As Boolean
        Dim bExito As Boolean = False

        For i As Integer = 0 To sCadena.Length - 1
            If Char.IsDigit(sCadena.Chars(i)) Then
                bExito = True
            Else
                bExito = False
                Exit For
            End If
        Next
        Return bExito
    End Function

    Public Function ValidarDouble(ByVal sCadena As String) As Boolean
        Dim bExito As Boolean = False

        For i As Integer = 0 To sCadena.Length - 1
            If (Convert.ToUInt32(sCadena.Chars(i)) > 47 And Convert.ToUInt32(sCadena.Chars(i)) < 58) Or _
               (Convert.ToUInt32(sCadena.Chars(i)) = 46) Or (Convert.ToUInt32(sCadena.Chars(i)) = 44) Then
                bExito = True
            Else
                bExito = False
                Exit For
            End If
        Next
        Return bExito
    End Function


End Module
