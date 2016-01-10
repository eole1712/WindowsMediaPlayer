Imports System.Collections.ObjectModel

Public Class Playlist
    Private Property _playlist As ObservableCollection(Of PlaylistItem) = New ObservableCollection(Of PlaylistItem)()
    Private Property _indexIsPlaying As Integer? = Nothing
    Private Property _titleIsPlaying As String = ""
    Private Property _prettyDurationIsPlaying As String = ""

    ' ************* BEGIN Getters/Setters *************

    Public Property Playlist As ObservableCollection(Of PlaylistItem)
        Get
            Return _playlist
        End Get
        Protected Set(value As ObservableCollection(Of PlaylistItem))
            _playlist = value
            _indexIsPlaying = Nothing
            _titleIsPlaying = ""
            _prettyDurationIsPlaying = ""
        End Set
    End Property

    Public Property IndexIsPlaying As Integer
        Get
            Return _indexIsPlaying
        End Get
        Protected Set(value As Integer)
            Dim OldValue = _indexIsPlaying
            _indexIsPlaying = value
            If _indexIsPlaying IsNot Nothing AndAlso _indexIsPlaying >= 0 AndAlso _indexIsPlaying < _playlist.Count Then
                _titleIsPlaying = _playlist.Item(_indexIsPlaying).Title
                _prettyDurationIsPlaying = _playlist.Item(_indexIsPlaying).PrettyDuration
            ElseIf _indexIsPlaying Is Nothing Then
                _titleIsPlaying = ""
                _prettyDurationIsPlaying = ""
            Else
                _indexIsPlaying = OldValue
            End If
        End Set
    End Property

    Public Property TitleIsPlaying As String
        Get
            Return _titleIsPlaying
        End Get
        Protected Set(value As String)
            _titleIsPlaying = value
        End Set
    End Property

    Public Property PrettyDurationIsPlaying As String
        Get
            Return _prettyDurationIsPlaying
        End Get
        Protected Set(value As String)
            _prettyDurationIsPlaying = value
        End Set
    End Property

    ' ************* END Getters/Setters *************

    ' ************* BEGIN Actions *************

    Public Function IsEmpty() As Boolean
        Return (_playlist.Count = 0)
    End Function

    Public Sub Add(ByVal Item As PlaylistItem)
        _playlist.Add(Item)
    End Sub

    Public Function Play() As String
        If _indexIsPlaying Is Nothing AndAlso Not IsEmpty() Then
            _indexIsPlaying = 0
            Return _titleIsPlaying
        Else
            Return ""
        End If
    End Function

    Public Sub StopIt()
        _indexIsPlaying = Nothing
    End Sub

    Public Function PlayNext() As String
        If _indexIsPlaying Is Nothing Then
            _indexIsPlaying = 0
        Else
            _indexIsPlaying += 1
        End If

        If IsEmpty() OrElse _indexIsPlaying > _playlist.Count Then
            _indexIsPlaying -= 1
            Return ""
        Else
            Return _titleIsPlaying
        End If
    End Function

    Public Function PlayPrev() As String
        If IsEmpty() OrElse _indexIsPlaying Is Nothing OrElse _indexIsPlaying = 0 Then
            Return ""
        Else
            _indexIsPlaying -= 1
        End If

        Return _titleIsPlaying
    End Function

    ' ************* END Actions *************
End Class