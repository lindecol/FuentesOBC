Imports System.Data
Imports System.Data.Common
Imports Desktop.Data
Imports System.ComponentModel

Public Class ExecuteProcessSQL
    Inherits TaskBase


    Private m_dsConfig As ExecuteProcessDataSet

    Public Overrides ReadOnly Property dsConfig() As System.Data.DataSet
        Get
            If m_dsConfig Is Nothing Then
                m_dsConfig = New ExecuteProcessDataSet
            End If
            Return m_dsConfig
        End Get
    End Property

    Public Overrides Sub Execute(ByVal pc As DataTransformation.IProgressController)
        If m_dsConfig.General.Count > 0 Then
            Dim Destino As GestorDatosBase
            Destino = GestorDatosFactory.CreateInstance( _
                        Parent.ConnectionTypeName, Parent.ConnectionString)
            Dim sentenciasSQL() As String = m_dsConfig.General(0).SentenciaSQL.Split(";"c)
            If (m_dsConfig.General(0).TipoSentencia = "Sentencia SQL Texto") Then
                For Each sentencia As String In sentenciasSQL
                    If Not sentencia.Trim.Equals("") Then
                        Destino.ExecuteNonQuery(CommandType.Text, sentencia)
                    End If
                Next
            Else
                For Each sentencia As String In sentenciasSQL
                    Destino.ExecuteNonQuery(CommandType.StoredProcedure, sentencia)
                Next
            End If
        End If
    End Sub

    Public Overrides Function ShowConfigDialog() As Boolean
        Return ExcuteProcessSQLDialog.Run(CType(Me.dsConfig, ExecuteProcessDataSet))
    End Function
End Class
