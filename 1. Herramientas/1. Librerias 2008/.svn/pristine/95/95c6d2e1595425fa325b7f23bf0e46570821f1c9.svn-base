Imports Desktop.Framework
Imports Desktop.Data

Public Class FrameworkTest

    Public Shared Sub Main()

        FrameworkApp.MultiEmpresa = False
        FrameworkApp.ConnectionType = ConnectionTypes.SqlClient
        FrameworkApp.ConnectionString = My.Settings.FrameworkConnectionString
        'FrameworkApp.ConnectionString = "Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=QuickWMS;Integrated Security=True;User ID=sa;Password=Desktop"
        FrameworkApp.ImportarConfiguracion = False
        FrameworkApp.ExportarConfiguracion = False
        'FrameworkApp.CambiarClaveUsuario = True
        'FrameworkApp.Configure()
        'FrameworkApp.Start()
        FrameworkApp.Configure()
    End Sub

End Class
