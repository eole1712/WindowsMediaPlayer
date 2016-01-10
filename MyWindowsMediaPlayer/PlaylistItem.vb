Public Class PlaylistItem
    Private Property _path As String
    Private Property _title As String
    Private Property _duration As TimeSpan
    Private Property _prettyDuration As String

    ' ************* BEGIN Getters/Setters *************

    Public Property Path As String
        Get
            Return _path
        End Get
        Protected Set(value As String)
            _path = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _title
        End Get
        Protected Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Duration As TimeSpan
        Get
            Return _duration
        End Get
        Protected Set(value As TimeSpan)
            _duration = value
        End Set
    End Property

    Public Property PrettyDuration As String
        Get
            Return _prettyDuration
        End Get
        Protected Set(value As String)
            _prettyDuration = value
        End Set
    End Property

    ' ************* END Getters/Setters *************

    Public Sub New(ByVal Path As String, ByVal Duration As TimeSpan)
        Dim tmpName As String() = Split(Path, "/")
        Dim PrettyName As String = tmpName(tmpName.Length - 1)

        _path = Path
        _title = PrettyName
        _duration = Duration
        _prettyDuration = Duration.ToString("hh\:mm\:ss")
    End Sub
End Class