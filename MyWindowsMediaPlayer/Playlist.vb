﻿Imports System.Collections.ObjectModel

Public Class Playlist
    Private Property _playlistAll As ObservableCollection(Of PlaylistItem) = New ObservableCollection(Of PlaylistItem)()
    Private Property _playlistFiltered As ObservableCollection(Of PlaylistItem) = _playlistAll
    Private Property _indexIsPlaying As Integer? = Nothing
    Private Property _filter As PlaylistItem.TypeMedia = PlaylistItem.TypeMedia.All

    ' ************* BEGIN Getters/Setters *************

    Public Property PlaylistAll As ObservableCollection(Of PlaylistItem)
        Get
            Return _playlistAll
        End Get
        Protected Set(value As ObservableCollection(Of PlaylistItem))
            _playlistAll = value
        End Set
    End Property

    Public Property PlaylistFiltered As ObservableCollection(Of PlaylistItem)
        Get
            Return _playlistFiltered
        End Get
        Protected Set(value As ObservableCollection(Of PlaylistItem))
            _playlistFiltered = value
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
            If _indexIsPlaying < 0 OrElse _indexIsPlaying >= PlaylistFiltered.Count Then
                _indexIsPlaying = OldValue
            End If
        End Set
    End Property

    Public ReadOnly Property TitleIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return PlaylistFiltered.Item(_indexIsPlaying).Title
        End Get
    End Property

    Public ReadOnly Property PathIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return PlaylistFiltered.Item(_indexIsPlaying).Path
        End Get
    End Property

    Public ReadOnly Property PrettyDurationIsPlaying As String
        Get
            If _indexIsPlaying Is Nothing Then
                Return ""
            End If
            Return PlaylistFiltered.Item(_indexIsPlaying).PrettyDuration
        End Get
    End Property

    ' ************* END Getters/Setters *************

    ' ************* BEGIN Actions *************

    Public Function IsEmpty() As Boolean
        Return (PlaylistFiltered.Count = 0)
    End Function

    Public Sub Filter(ByVal Filter As String)
        If Filter = "" Then
            If _filter = PlaylistItem.TypeMedia.All Then
                Return
            End If

            _filter = PlaylistItem.TypeMedia.All
            PlaylistFiltered = New ObservableCollection(Of PlaylistItem)()
            For Each Item In PlaylistAll
                Add(Item)
            Next
        ElseIf Filter = "Video" Then
            If _filter = PlaylistItem.TypeMedia.Video Then
                Return
            End If

            _filter = PlaylistItem.TypeMedia.Video
            PlaylistFiltered = New ObservableCollection(Of PlaylistItem)()
            For Each Item In PlaylistAll
                If Item.Type = PlaylistItem.TypeMedia.Video Then
                    Add(Item)
                End If
            Next
        ElseIf Filter = "Audio" Then
            If _filter = PlaylistItem.TypeMedia.Audio Then
                Return
            End If

            _filter = PlaylistItem.TypeMedia.Audio
            PlaylistFiltered = New ObservableCollection(Of PlaylistItem)()
            For Each Item In PlaylistAll
                If Item.Type = PlaylistItem.TypeMedia.Audio Then
                    Add(Item)
                End If
            Next
        End If
    End Sub

    Public Sub Clear()
        While Not IsEmpty()
            Remove(0)
        End While
        IndexIsPlaying = Nothing
    End Sub

    Public Sub Add(ByVal Item As PlaylistItem)
        PlaylistAll.Add(Item)
        If Item.Type = _filter Then
            PlaylistFiltered.Add(Item)
        End If
    End Sub

    Public Function Remove(Index As Integer) As Boolean
        If Index < 0 OrElse Index >= PlaylistFiltered.Count Then
            Return False
        End If
        PlaylistFiltered.Remove(PlaylistFiltered.ElementAt(Index))
        If IndexIsPlaying = Index Then
            If Not (IndexIsPlaying - 1 < 0) Then
                IndexIsPlaying -= 1
            ElseIf Not (IndexIsPlaying + 1 >= PlaylistFiltered.Count) Then
                IndexIsPlaying += 1
            Else
                IndexIsPlaying = Nothing
            End If
            Return True
        End If
        Return False
    End Function

    Public Sub Move(IndexSrc As Integer, IndexDest As Integer)
        If IndexDest >= 0 AndAlso IndexDest < PlaylistFiltered.Count Then
            PlaylistFiltered.Move(IndexSrc, IndexDest)
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
            If IndexIsPlaying + 1 >= PlaylistFiltered.Count Then
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