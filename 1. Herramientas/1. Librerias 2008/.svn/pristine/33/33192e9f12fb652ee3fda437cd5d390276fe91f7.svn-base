' Interfaz publica, para utilizar en procesos largos que requieren mostrar un avance\
' al usuario
Public Interface IProgress

    ' Define si el proceso se ha cancelado
    ReadOnly Property IsCancel() As Boolean

    ' Funciona llamada al inicio de proceso, se debe utilizar para inicializar la interfaz
    ' de usuario.
    Sub Init(ByVal nMaxValue As Integer)

    ' Funciona llamada para reportar avance en el proceso, se debe actualizar la interfaz de usuario
    Sub SetProgress(ByVal nValue As Integer)

    ' Funciona llamada al inicio de proceso, se debe utilizar para inicializar la interfaz
    ' de usuario.
    Sub Done()

End Interface
