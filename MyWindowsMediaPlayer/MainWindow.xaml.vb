Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Navigation
Imports System.Windows.Shapes
Imports System.Windows.Data
Imports System.Windows.Media
Imports System.Windows.Input

Imports System.Windows.Forms
Imports System.Windows.Threading

Class MainWindow

    Private Property _txt As String = ""
    Private Property _isDraggingSlider As Boolean = False
    Private WithEvents _timer As DispatcherTimer

    Sub New()
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _timer = New DispatcherTimer()
        _timer.Interval = TimeSpan.FromSeconds(1)

        'timeSliderCurrentTime.Content.SetBinding(TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\: mm\: ss"), "timeSliderCurrentTime")
        'timeSliderCurrentTime.Content.SetBinding(_txt, New Forms.Binding("timeSliderCurrentTime", _txt, timeSliderCurrentTime.Content))

        'Dim myBinding As Forms.Binding = New Forms.Binding("currentTime", _txt, timeSliderCurrentTime.Content)
        'timeSliderCurrentTime.Content.SetBinding(TextBlock.TextProperty, myBinding)
    End Sub

    Private Sub mediaScreen_Load(ByVal sender As Object, e As EventArgs) Handles mediaScreen.Loaded
        volumeSlider.Value = 0.5

        speedSlider.Value = 1
    End Sub

    Sub OpenFile(ByVal PlayNow As Boolean)
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez un fichier à ouvrir..."
        fd.InitialDirectory = "C:/"
        fd.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = Forms.DialogResult.OK Then
            mediaScreen.Source = New Uri(fd.FileName)
            If PlayNow Then
                Play()
            End If
        End If
    End Sub

    Private Sub timeSlider_Tick(ByVal sender As Object, e As EventArgs) Handles _timer.Tick
        If mediaScreen.Source <> Nothing AndAlso mediaScreen.NaturalDuration.HasTimeSpan AndAlso Not _isDraggingSlider Then
            timeSlider.Value = mediaScreen.Position.TotalSeconds
        End If
    End Sub

    Sub Play()
        _timer.Start()
        Try
            If mediaScreen.Source <> Nothing Then
                mediaScreen.Play()
            End If
        Catch ex As Exception
            mediaScreen.Source = Nothing
            MsgBox("Le fichier sélectionné n'est pas lisible par le lecteur.")
        End Try
    End Sub

    Sub Pause()
        If mediaScreen.CanPause Then
            _timer.Stop()
            mediaScreen.Pause()
        End If
    End Sub

    Sub StopIt()
        _timer.Stop()
        timeSlider.Value = 0
        timeSliderCurrentTime.Content = "00:00:00"
        mediaScreen.Stop()
    End Sub

    Private Sub openButton_Click(sender As Object, e As RoutedEventArgs) Handles openButton.Click
        OpenFile(True)
    End Sub

    Private Sub closeButton_Click(sender As Object, e As RoutedEventArgs) Handles closeButton.Click
        StopIt()
        mediaScreen.Source = Nothing
    End Sub

    Private Sub mediaOpened(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If mediaScreen.NaturalDuration.HasTimeSpan Then
            timeSlider.Minimum = 0
            timeSliderCurrentTime.Content = "00:00:00"

            timeSlider.Maximum = mediaScreen.NaturalDuration.TimeSpan.TotalSeconds
            timeSliderMaxTime.Content = mediaScreen.NaturalDuration.TimeSpan.ToString("hh\:mm\:ss")
        End If
    End Sub

    Private Sub mediaEnded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        StopIt()
    End Sub

    Private Sub playButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles playButton.Click
        Play()
    End Sub

    Private Sub stopButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles stopButton.Click
        StopIt()
    End Sub

    Private Sub timeSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles timeSlider.ValueChanged
        mediaScreen.Position = TimeSpan.FromSeconds(timeSlider.Value)

        timeSliderCurrentTime.Content = TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\:mm\:ss")
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
End Class