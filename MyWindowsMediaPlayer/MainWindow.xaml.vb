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
        _timer.Start()

        'timeSliderCurrentTime.Content.SetBinding(TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\: mm\: ss"), "timeSliderCurrentTime")
        'timeSliderCurrentTime.Content.SetBinding(_txt, New Forms.Binding("timeSliderCurrentTime", _txt, timeSliderCurrentTime.Content))

        'Dim myBinding As Forms.Binding = New Forms.Binding("currentTime", _txt, timeSliderCurrentTime.Content)
        'timeSliderCurrentTime.Content.SetBinding(TextBlock.TextProperty, myBinding)
    End Sub

    Private Sub timeSlider_Tick(ByVal sender As Object, e As EventArgs) Handles _timer.Tick
        If mediaScreen.Source <> Nothing AndAlso mediaScreen.NaturalDuration.HasTimeSpan AndAlso Not _isDraggingSlider Then
            timeSlider.Value = mediaScreen.Position.TotalSeconds
        End If
    End Sub

    Private Sub openButton_Click(sender As Object, e As RoutedEventArgs) Handles openButton.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez un fichier à ouvrir..."
        fd.InitialDirectory = "C:/"
        fd.Filter = "Media files (*.mp3;*.mpg;*.mpeg)|*.mp3;*.mpg;*.mpeg|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        If fd.ShowDialog() = Forms.DialogResult.OK Then
            mediaScreen.Source = New Uri(fd.FileName)
            mediaScreen.Play()
        End If
    End Sub

    Private Sub closeButton_Click(sender As Object, e As RoutedEventArgs) Handles closeButton.Click

    End Sub

    Private Sub Element_MediaOpened(ByVal sender As Object, ByVal args As RoutedEventArgs)
        timeSlider.Minimum = 0
        timeSliderCurrentTime.Content = "00:00:00"

        timeSlider.Maximum = mediaScreen.NaturalDuration.TimeSpan.TotalSeconds
        timeSliderMaxTime.Content = mediaScreen.NaturalDuration.TimeSpan.ToString("hh\:mm\:ss")
    End Sub

    Private Sub Element_MediaEnded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        mediaScreen.Stop()
    End Sub

    Private Sub playButton_Click(ByVal sender As Object, e As RoutedEventArgs) Handles playButton.Click
        If mediaScreen.Source <> Nothing Then
            mediaScreen.Play()
        End If
    End Sub

    Private Sub timeSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles timeSlider.ValueChanged
        mediaScreen.Position = TimeSpan.FromSeconds(timeSlider.Value)

        timeSliderCurrentTime.Content = TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\:mm\:ss")
    End Sub

    Private Sub volumeSlider_ValueChanged(ByVal sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles volumeSlider.ValueChanged
        mediaScreen.Volume = volumeSlider.Value / 100
    End Sub
End Class