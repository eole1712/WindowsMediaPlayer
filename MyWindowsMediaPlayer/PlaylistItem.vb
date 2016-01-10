Public Class PlaylistItem
    Public Enum TypeMedia
        All
        Video
        Audio
    End Enum

    Private Property _path As String
    Private Property _title As String
    Private Property _duration As TimeSpan
    Private Property _prettyDuration As String
    Private Property _type As TypeMedia

    ' ************* BEGIN Getters/Setters *************

    Public Property Path As String
        Get
            Return _path
        End Get
        Set(value As String)
            _path = value
        End Set
    End Property

    Public Property Title As String
        Get
            Return _title
        End Get
        Set(value As String)
            _title = value
        End Set
    End Property

    Public Property Duration As TimeSpan
        Get
            Return _duration
        End Get
        Set(value As TimeSpan)
            _duration = value
        End Set
    End Property

    Public Property PrettyDuration As String
        Get
            Return _prettyDuration
        End Get
        Set(value As String)
            _prettyDuration = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    ' ************* END Getters/Setters *************

    Sub New()

    End Sub

    Sub New(ByVal Path As String, ByVal Duration As TimeSpan, Type As TypeMedia)
        Dim tmpName As String() = Split(Path, "/")
        Dim PrettyName As String = tmpName(tmpName.Length - 1)

        _path = Path
        _title = PrettyName
        _duration = Duration
        _prettyDuration = Duration.ToString("hh\:mm\:ss")
        _type = Type
    End Sub
End Class