'//////////////////////////////////////////////////////////////////
' Interfaces utilizadas para pasar datos adicionales a los m�dulos
' invocados por el n�cleo

Public Interface IModuloPrograma
    Sub Run()
End Interface

Public Interface IModuloRutero
    Sub Run()
End Interface

Public Interface IModuloNucleo
    Sub Run()
End Interface

Public Interface IModuloPedido
    Sub Run(ByVal rowCliente As DataRow, ByVal rowPedido As DataRow)
End Interface

Public Interface IModuloCliente
    Sub Run(ByVal rowCliente As DataRow)
End Interface


