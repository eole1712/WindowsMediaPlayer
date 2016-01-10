Imports System.Collections.ObjectModel

Public Class Playlist
    Private Property _playlist As ObservableCollection(Of PlaylistItem) = New ObservableCollection(Of PlaylistItem)()
    Private Property _indexIsPlaying As Integer? = Nothing

    ' ************* BEGIN Getters/Setters *************

    Public Property Playlist As ObservableCollection(Of PlaylistItem)
        Get
            Return _playlist
        End Get
        Protected Set(value As ObservableCollection(Of PlaylistItem))
            _playlist = value
            IndexIsPlaying = Nothing
        End Set
    End Property

    Public Property IndexIsPlaying As Integer?
        Get
            Return _indexIsPlaying
        End Get
        Protected Set(value As Integer?)
            Dim OldValue = _indexIsPlaying
            _indexIsPlaying = value
            If _indexIsPlaying < 0 OrElse _indexIsPlaying >= _playlist.Count Then
                _indexIsPlaying = OldValue
            End If
        End Set
    End Property

    Public ReadOnly Property TitleIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return _playlist.Item(_indexIsPlaying).Title
        End Get
    End Property

    Public ReadOnly Property PathIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return _playlist.Item(_indexIsPlaying).Path
        End Get
    End Property

    Public ReadOnly Property PrettyDurationIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return _playlist.Item(_indexIsPlaying).PrettyDuration
        End Get
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
        If IsEmpty() OrElse IndexIsPlaying IsNot Nothing Then
            Return ""
        Else
            IndexIsPlaying = 0
            Return PathIsPlaying
        End If
    End Function

    Public Sub StopIt()
        IndexIsPlaying = Nothing
    End Sub

    Public Function PlayNext() As String
        If IsEmpty() Then
            Return ""
        End If

        If IndexIsPlaying Is Nothing Then
            IndexIsPlaying = 0
        Else
            If IndexIsPlaying + 1 >= _playlist.Count Then
                Return ""
            Else
                IndexIsPlaying += 1
            End If
        End If

        Return PathIsPlaying
    End Function

    Public Function PlayPrev() As String
        If IsEmpty() OrElse IndexIsPlaying Is Nothing OrElse IndexIsPlaying = 0 Then
            Return ""
        Else
            IndexIsPlaying -= 1
        End If

        Return PathIsPlaying
    End Function

    ' ************* END Actions *************
End Class