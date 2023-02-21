Imports Desktop.Data.Transformations
Imports Desktop.Data
Imports System.IO
Imports System.Data.Common

Public Class Form1
    Implements DataTransformation.IProgressController

    Private Transformation As DataTransformation

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Transformation = New DataTransformation(ConnectionTypes.SqlClient, _
            My.Settings.ConnectionString)
        If File.Exists("C:\Prueba.XML") Then

        End If
        TransformationEditControl1.Transformation = Transformation
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Transformation.Execute(Me)
        Catch ex As Exception
            lstLog.Items.Add("Error: " & ex.Message)
        End Try
    End Sub

    Public Sub TaskError(ByVal ErrorMessage As String) Implements Desktop.Data.Transformations.DataTransformation.IProgressController.TaskError
        lstLog.Items.Add(ErrorMessage)
    End Sub

    Public Sub TaskStarted(ByVal TaskName As String, ByVal TaskIndex As Integer) Implements Desktop.Data.Transformations.DataTransformation.IProgressController.TaskStarted
        lstLog.Items.Add("Tarea iniciada: " & TaskName)
    End Sub

    Public Sub UpdateCurrentTaskProgress(ByVal nPercentageProgress As Integer) Implements Desktop.Data.Transformations.DataTransformation.IProgressController.UpdateCurrentTaskProgress

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim gestor As GestorDatosBase = GestorDatosFactory.CreateInstance( _
            ConnectionTypes.SqlServerCe, _
            My.Settings.ConnectionString)
        Dim da As DbDataAdapter = gestor.CreateDataAdapter("SELECT Codigo, Activo from PruebaImportar")

    End Sub

    Private Sub TransformationEditControl1_ConfigurationChanged(ByVal t As Desktop.Data.Transformations.DataTransformation) Handles TransformationEditControl1.ConfigurationChanged
        lstLog.Items.Add("Configuracion cambiada")
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

    End Sub

    Public Sub TaskEnded(ByVal RowsProcessed As Integer, ByVal nRowsWithErrors As Integer, ByVal MensajeError As String) Implements Desktop.Data.Transformations.DataTransformation.IProgressController.TaskEnded
        lstLog.Items.Add("Tarea finalizada. Filas procesadas: " & RowsProcessed.ToString & " Filas con errores: " & nRowsWithErrors.ToString())
    End Sub
End Class
