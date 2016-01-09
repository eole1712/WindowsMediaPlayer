Namespace MyWindowsMediaPlayer
    Public Class VideoFlux
        Private Property mp As MediaPlayer
        Private Property vd As VideoDrawing
        Private Property db As DrawingBrush

        Sub New(ByVal file As String)
            Dim mp As MediaPlayer = New MediaPlayer()
            mp.Open(New Uri(file))

            vd.Player = mp
            vd.Rect = New Rect(0, 0, 200, 200)

            'Binding videobrush
            'MediaElement. = db

            mp.Play()
        End Sub
    End Class
End Namespace