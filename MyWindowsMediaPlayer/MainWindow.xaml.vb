Imports System.Windows.Forms
Imports System.Windows.Threading
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO

Class MainWindow

    Private Property _isDraggingSlider As Boolean = False
    Private Property _playlistWidth As GridLength = New GridLength(330)
    Private Property _playlist As Playlist = New Playlist()
    Private WithEvents _tmpMedia As MediaPlayer = New MediaPlayer()
    Private WithEvents _timer As DispatcherTimer = New DispatcherTimer()

    ' ************* BEGIN Getters/Setters *************

    Public Property Playlist As Playlist
        Get
            Return _playlist
        End Get
        Protected Set(value As Playlist)
            _playlist = value
        End Set
    End Property

    ' ************* END Getters/Setters *************

    Sub New()
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _timer.Interval = TimeSpan.FromSeconds(1)

        DataContext = Me

        'timeSliderCurrentTime.Content.SetBinding(TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\: mm\: ss"), "timeSliderCurrentTime")
        'timeSliderCurrentTime.Content.SetBinding(_txt, New Forms.Binding("timeSliderCurrentTime", _txt, timeSliderCurrentTime.Content))

        'Dim myBinding As Forms.Binding = New Forms.Binding("currentTime", _txt, timeSliderCurrentTime.Content)
        'timeSliderCurrentTime.Content.SetBinding(TextBlock.TextProperty, myBinding)
    End Sub

    ' ************* BEGIN Actions *************

    Private Sub OpenFile(ByVal PlayNow As Boolean)
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez un fichier à ouvrir..."
        fd.InitialDirectory = ""
        fd.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = Forms.DialogResult.OK Then
            If PlayNow Then
                mediaScreen.Source = New Uri(fd.FileName)
                Play()
            Else
                _tmpMedia.Open(New Uri(fd.FileName))
            End If
        End If
    End Sub

    Private Sub Play()
        If mediaScreen.Source <> Nothing Then
            HidePlayButton()
            _timer.Start()
            Try
                mediaScreen.Play()
            Catch ex As Exception
                _timer.Stop()
                mediaScreen.Source = Nothing
                MsgBox("Le fichier sélectionné n'est pas lisible par le lecteur.")
            End Try
        Else
            Dim toPlay As String = _playlist.Play()
            If toPlay <> "" Then
                mediaScreen.Source = New Uri(toPlay)
                Play()
            End If
        End If
    End Sub

    Private Sub Pause()
        If mediaScreen.CanPause Then
            ShowPlayButton()
            _timer.Stop()
            mediaScreen.Pause()
        End If
    End Sub

    Private Sub StopIt()
        If mediaScreen.Source <> Nothing Then
            ShowPlayButton()
            _timer.Stop()
            timeSlider.Value = 0
            mediaScreen.Stop()
        End If
    End Sub

    Private Sub StopClose()
        StopIt()
        _playlist.StopIt()
        mediaScreen.Source = Nothing
        timeSlider.Maximum = 1
        timeSliderMaxTime.Content = "00:00:00"
    End Sub

    Private Sub Serialize(ByVal Path As String)
        Dim xmlPlayList(_playlist.PlaylistAll.Count()) As PlaylistItem
        Dim serializer As New XmlSerializer(GetType(PlaylistItem()))
        Dim writer As New StreamWriter(Path)

        _playlist.PlaylistAll.CopyTo(xmlPlayList, 0)
        serializer.Serialize(writer, xmlPlayList)
        writer.Flush()
        writer.Close()
        MsgBox("La liste de lecture a bien été enregistrée.")
    End Sub

    Public Sub Unserialize(ByVal Path As String)
        Dim serializer As New XmlSerializer(GetType(PlaylistItem()))
        Dim reader As New FileStream(Path, FileMode.Open)
        Dim xmlPlayList() As PlaylistItem = CType(serializer.Deserialize(reader), PlaylistItem())

        reader.Close()
        _playlist.PlaylistAll.Clear()
        For i = 0 To xmlPlayList.Count - 2
            _playlist.Add(xmlPlayList(i))
        Next
    End Sub

    ' ************* END Actions *************

    ' ************* BEGIN Helpers *************

    ' *** BEGIN Panel ***
    Private Sub ShowPanel()
        panel.Opacity = 0.7
    End Sub

    Private Sub HidePanel()
        panel.Opacity = 0
    End Sub
    ' *** END Panel ***

    ' *** BEGIN Buttons ***
    Private Sub ShowPlayButton()
        pauseButton.Visibility = Visibility.Collapsed
        playButton.Visibility = Visibility.Visible
    End Sub

    Private Sub HidePlayButton()
        playButton.Visibility = Visibility.Collapsed
        pauseButton.Visibility = Visibility.Visible
    End Sub
    ' *** END Buttons ***

    ' *** BEGIN Sliders ***
    Private Sub UpdateCurrentTime()
        mediaScreen.Position = TimeSpan.FromSeconds(timeSlider.Value)
        timeSliderCurrentTime.Content = TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\:mm\:ss")
    End Sub
    ' *** END Sliders ***

    ' ************* END Helpers *************

    ' ************* BEGIN Events Handling *************

    ' *** BEGIN PlayerCanvas ***
    Private Sub PlayerCanvas_MouseWheel(ByVal sender As Object, e As MouseWheelEventArgs) Handles PlayerCanvas.MouseWheel
        Dim Diff As Double = IIf((e.Delta > 0), 0.1, -0.1)
        If Not (volumeSlider.Value + Diff > volumeSlider.Maximum OrElse volumeSlider.Value + Diff < volumeSlider.Minimum) Then
            volumeSlider.Value = Math.Round(volumeSlider.Value + Diff, 1)
        End If
    End Sub

    Private Sub PlayerCanvas_MouseEnter(ByVal sender As Object, e As EventArgs) Handles PlayerCanvas.MouseEnter
        ShowPanel()
    End Sub

    Private Sub PlayerCanvas_MouseLeave(ByVal sender As Object, e As EventArgs) Handles PlayerCanvas.MouseLeave
        HidePanel()
    End Sub

    Private Sub PlayerCanvas_LeftClick(ByVal sender As Object, e As EventArgs) Handles PlayerCanvas.MouseLeftButtonDown
        If mediaScreen.Source = Nothing Then
            OpenFile(True)
        End If
    End Sub
    ' *** END PlayerCanvas ***

    ' *** BEGIN mediaScreen ***
    Private Sub mediaScreen_Load(ByVal sender As Object, e As EventArgs) Handles mediaScreen.Loaded
        timeSlider.Value = 0
        timeSlider.Minimum = 0
        volumeSlider.Value = 0.5
        speedSlider.Value = 1
    End Sub

    Private Sub mediaScreen_MediaOpened(ByVal sender As Object, ByVal args As RoutedEventArgs) Handles mediaScreen.MediaOpened
        If mediaScreen.NaturalDuration.HasTimeSpan Then
            ShowPanel()
            timeSliderCurrentTime.Content = "00:00:00"

            timeSlider.Maximum = mediaScreen.NaturalDuration.TimeSpan.TotalSeconds
            timeSliderMaxTime.Content = mediaScreen.NaturalDuration.TimeSpan.ToString("hh\:mm\:ss")
        Else
            HidePanel()
        End If
    End Sub

    Private Sub mediaScreen_MediaFailed(ByVal sender As Object, ByVal args As RoutedEventArgs) Handles mediaScreen.MediaFailed
        StopClose()
        MsgBox("Le fichier sélectionné n'est pas lisible par le lecteur.")
    End Sub

    Private Sub mediaScreen_MediaEnded(ByVal sender As Object, ByVal args As RoutedEventArgs) Handles mediaScreen.MediaEnded
        StopIt()
        Dim toPlay As String = _playlist.PlayNext()
        If toPlay <> "" Then
            mediaScreen.Source = New Uri(toPlay)
            Play()
        Else
            _playlist.StopIt()
        End If
    End Sub

    ' *** END mediaScreen ***

    ' *** BEGIN File Buttons ***
    Private Sub openButton_Click(sender As Object, e As RoutedEventArgs) Handles openButton.Click
        OpenFile(True)
    End Sub

    Private Sub closeButton_Click(sender As Object, e As RoutedEventArgs) Handles closeButton.Click
        StopClose()
    End Sub
    ' *** END File Buttons ***

    ' *** BEGIN Playlist Buttons ***

    Private Sub openPlaylistButton_Click(sender As Object, e As RoutedEventArgs) Handles openPlaylistButton.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez une playlist à ouvrir..."
        fd.InitialDirectory = ""
        fd.Filter = "XML files (*.xml)|*.xml"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = Forms.DialogResult.OK Then
            Try
                Unserialize(fd.FileName)
            Catch
                Playlist.Clear()
                MsgBox("Le fichier XML sélectionné est invalide.")
            End Try
        End If
    End Sub

    Private Sub addToPlaylistButton_Click(sender As Object, e As RoutedEventArgs) Handles addToPlaylistButton.Click
        OpenFile(False)
    End Sub

    Private Sub playPlaylistButton_Click(sender As Object, e As RoutedEventArgs) Handles playPlaylistButton.Click
        Dim toPlay As String = _playlist.Play()
        If toPlay <> "" Then
            mediaScreen.Source = New Uri(toPlay)
            Play()
        End If
    End Sub

    Private Sub savePlaylistButton_Click(sender As Object, e As RoutedEventArgs) Handles savePlaylistButton.Click
        Dim fd As SaveFileDialog = New SaveFileDialog()

        fd.Title = "Choisissez où enregistrer votre liste de lecture..."
        fd.InitialDirectory = ""
        fd.Filter = "XML files (*.xml)|*.xml"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = Forms.DialogResult.OK Then
            Serialize(fd.FileName)
        End If
    End Sub

    Private Sub closePlaylistButton_Click(sender As Object, e As RoutedEventArgs) Handles closePlayListButton.Click
        StopClose()
        _playlist.Clear()
    End Sub

    ' *** END Playlist Buttons ***

    ' *** BEGIN AboutUs Buttons ***

    Private Sub aboutUsButton_Click(sender As Object, e As RoutedEventArgs) Handles aboutUsButton.Click
        MsgBox("MyWindowsMediaPlayer" & Environment.NewLine _
            & "Un logiciel de lecture de média vidéos, audios et images" & Environment.NewLine _
            & "Développé par Jean GAMAIN, Grisha GHUKASYAN et Adrien HARNAY" & Environment.NewLine _
            & "Développeurs étudiants à Epitech Paris")
    End Sub

    ' *** END AboutUs Buttons ***

    ' *** BEGIN Media Buttons ***
    Private Sub playButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles playButton.Click
        Play()
    End Sub

    Private Sub pauseButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles pauseButton.Click
        Pause()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles stopButton.Click
        StopIt()
        _playlist.StopIt()
    End Sub

    Private Sub nextButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles nextButton.Click
        Dim toPlay As String = _playlist.PlayNext()
        If toPlay <> "" Then
            mediaScreen.Source = New Uri(toPlay)
            Play()
        End If
    End Sub

    Private Sub prevButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles prevButton.Click
        Dim toPlay As String = _playlist.PlayPrev()
        If toPlay <> "" Then
            mediaScreen.Source = New Uri(toPlay)
            Play()
        End If
    End Sub

    Private Sub fullScreenButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles fullScreenButton.Click
        If (WindowStyle = System.Windows.WindowStyle.None) Then
            WindowState = WindowState.Normal
            WindowStyle = System.Windows.WindowStyle.SingleBorderWindow
            playListPanel.Width = _playlistWidth
            menuBar.Height = New GridLength(24)
        Else
            WindowState = WindowState.Maximized
            WindowStyle = System.Windows.WindowStyle.None
            playListPanel.Width = New GridLength(0)
            menuBar.Height = New GridLength(0)
        End If
    End Sub

    Private Sub playListHideButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles playListHideButton.Click
        If playListPanel.Width = New GridLength(0) Then
            playListPanel.Width = _playlistWidth
        Else
            _playlistWidth = playListPanel.Width
            playListPanel.Width = New GridLength(0)
        End If
    End Sub

    Private Sub selectionMoveDown_Click(sender As Object, e As RoutedEventArgs) Handles selectionMoveDown.Click
        _playlist.Move(list.SelectedIndex, list.SelectedIndex + 1)
    End Sub

    Private Sub selectionMoveUp_Click(sender As Object, e As RoutedEventArgs) Handles selectionMoveUp.Click
        _playlist.Move(list.SelectedIndex, list.SelectedIndex - 1)
    End Sub

    ' *** END Media Buttons ***

    ' *** BEGIN Sliders ***
    Private Sub timeSlider_DragStarted(ByVal sender As Object, e As RoutedEventArgs)
        If mediaScreen.Source <> Nothing Then
            _isDraggingSlider = True
            UpdateCurrentTime()
        End If
    End Sub

    Private Sub timeSlider_DragCompleted(ByVal sender As Object, e As RoutedEventArgs)
        If mediaScreen.Source <> Nothing Then
            _isDraggingSlider = False
            UpdateCurrentTime()
        End If
    End Sub

    Private Sub timeSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles timeSlider.ValueChanged
        If mediaScreen.Source <> Nothing AndAlso Not _isDraggingSlider Then
            UpdateCurrentTime()
        End If
    End Sub

    Private Sub volumeSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles volumeSlider.ValueChanged
        If mediaScreen.IsLoaded AndAlso volume.IsLoaded Then
            mediaScreen.Volume = volumeSlider.Value
            volume.Content = (volumeSlider.Value * 100).ToString & "%"
        End If
    End Sub

    Private Sub speedSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles speedSlider.ValueChanged
        If mediaScreen.IsLoaded AndAlso speed.IsLoaded Then
            mediaScreen.SpeedRatio = speedSlider.Value
            speed.Content = speedSlider.Value.ToString & "x"
        End If
    End Sub
    ' *** END Sliders ***

    ' *** BEGIN list (Playlist) ***
    Private Sub list_KeyUp(sender As Object, e As Input.KeyEventArgs) Handles list.KeyUp
        If e.Key = Key.Delete Then
            If _playlist.Remove(list.SelectedIndex) Then
                Dim toPlay As String = _playlist.PlayNext()
                If toPlay <> "" Then
                    mediaScreen.Source = New Uri(toPlay)
                    Play()
                Else
                    toPlay = _playlist.Play()
                    If toPlay <> "" Then
                        mediaScreen.Source = New Uri(toPlay)
                        Play()
                    Else
                        StopClose()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub list_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs) Handles list.MouseDoubleClick
        If list.SelectedIndex <> -1 And mediaScreen.Source <> New Uri(list.SelectedItem.Path) Then
            mediaScreen.Source = New Uri(list.SelectedItem.Path)
            Play()
        End If
    End Sub
    ' *** END list (Playlist) ***

    ' *** BEGIN _tmpMedia ***
    Private Sub _tmpMedia_MediaOpened(ByVal sender As Object, e As EventArgs) Handles _tmpMedia.MediaOpened
        If _tmpMedia.NaturalDuration.HasTimeSpan Then
            Dim Type As PlaylistItem.TypeMedia

            If _tmpMedia.HasVideo Then
                Type = PlaylistItem.TypeMedia.Video
            Else
                Type = PlaylistItem.TypeMedia.Audio
            End If
            _playlist.Add(New PlaylistItem(_tmpMedia.Source.ToString, _tmpMedia.NaturalDuration.TimeSpan, Type))
        Else
            MsgBox("Cet élément ne peut pas être ajouté à la liste de lecture.")
        End If
        _tmpMedia.Close()
    End Sub

    Private Sub _tmpMedia_MediaFailed(ByVal sender As Object, e As EventArgs) Handles _tmpMedia.MediaFailed
        MsgBox("Cet élément ne peut pas être ajouté à la liste de lecture.")
    End Sub
    ' *** END _tmpMedia ***

    ' *** BEGIN _timer ***
    Private Sub _timer_Tick(ByVal sender As Object, e As EventArgs) Handles _timer.Tick
        If mediaScreen.Source <> Nothing AndAlso mediaScreen.NaturalDuration.HasTimeSpan AndAlso Not _isDraggingSlider Then
            timeSlider.Value = mediaScreen.Position.TotalSeconds
        End If
    End Sub
    ' *** END _timer ***

    ' ************* END Events Handling *************
End Class