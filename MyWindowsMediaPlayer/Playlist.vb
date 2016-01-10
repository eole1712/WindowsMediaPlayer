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

    Public Sub Clear()
        While Not IsEmpty()
            Remove(0)
        End While
        IndexIsPlaying = Nothing
    End Sub

    Public Sub Add(ByVal Item As PlaylistItem)
        _playlist.Add(Item)
    End Sub

    Public Function Remove(Index As Integer) As Boolean
        If Index < 0 OrElse Index >= Playlist.Count Then
            Return False
        End If
        _playlist.Remove(_playlist.ElementAt(Index))
        If IndexIsPlaying = Index Then
            If Not (IndexIsPlaying - 1 < 0) Then
                IndexIsPlaying -= 1
            ElseIf Not (IndexIsPlaying + 1 >= _playlist.Count) Then
                IndexIsPlaying += 1
            Else
                IndexIsPlaying = Nothing
            End If
            Return True
        End If
        Return False
    End Function

    Public Sub Move(IndexSrc As Integer, IndexDest As Integer)
        If IndexDest >= 0 AndAlso IndexDest < Playlist.Count Then
            _playlist.Move(IndexSrc, IndexDest)
            If IndexIsPlaying = IndexSrc Then
                IndexIsPlaying = IndexDest
            End If
        End If
    End Sub

    Public Function Play() As String
        If IsEmpty() Then
            Return ""
        Else
            If IndexIsPlaying Is Nothing Then
                IndexIsPlaying = 0
            End If
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