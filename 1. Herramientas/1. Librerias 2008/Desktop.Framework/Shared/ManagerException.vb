Imports System.Data.SqlClient

Public Class ManagerException

    Public Shared Function Exception(ByVal ex As Exception) As String
        Try

            Dim sMensaje As String = ex.Message

            Select Case ex.Source
                Case ".Net SqlClient Data Provider"
                    Dim objSqlException As SqlException
                    objSqlException = CType(ex, System.Data.SqlClient.SqlException)

                    If objSqlException IsNot Nothing Then
                        Select Case objSqlException.Number
                            Case 547
                                sMensaje = "No es posible eliminar el registro seleccionado porque tiene datos relacionados"
                        End Select
                    End If
                Case "System.Data"
                    Dim objConstrainException As ConstraintException
                    objConstrainException = CType(ex, ConstraintException)

                    If objConstrainException IsNot Nothing Then
                        Select Case objConstrainException.TargetSite.Name
                            Case "CheckConstraint"
                                sMensaje = "No es posible agregar el resgistro porque ya hay un registro con los mismos datos"
                        End Select
                    End If

                Case "mscorlib"

                    Dim objArgumentOutOfRangeException As ArgumentOutOfRangeException
                    objArgumentOutOfRangeException = CType(ex, ArgumentOutOfRangeException)

                    If objArgumentOutOfRangeException IsNot Nothing Then
                        Select Case objArgumentOutOfRangeException.TargetSite.Name
                            Case "get_Item"
                                sMensaje = "Es necesario llenar las filas que se agregaron"
                        End Select
                    End If

                Case "Microsoft.VisualBasic"

                    Select Case ex.TargetSite.Name
                        Case "ToDouble"
                            sMensaje = "No es posible convertir una cadena de tipo texto a numerico"
                    End Select



            End Select

            Return sMensaje

        Catch ez As Exception
            Return ex.Message
        End Try

    End Function

End Class
