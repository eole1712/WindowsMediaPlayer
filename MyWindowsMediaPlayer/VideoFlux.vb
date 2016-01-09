Public Class VideoFlux
    Private Property _mp As MediaPlayer
    Private Property _vd As VideoDrawing
    Private Property _db As DrawingBrush

    Sub New(ByVal file As String)
        _mp = New MediaPlayer()
        _mp.Open(New Uri(file))

        _vd = New VideoDrawing
        _vd.Player = _mp
        _vd.Rect = New Rect(0, 0, 200, 200)

        _db = New DrawingBrush(_vd)
        'Binding videobrush
        'MediaElement. = db
    End Sub

    Sub Play()
        _mp.Play()
    End Sub

    Sub Pause()
        If _mp.CanPause Then
            _mp.Pause()
        End If
    End Sub

    Sub StopIt()
        _mp.Stop()
    End Sub

End Class