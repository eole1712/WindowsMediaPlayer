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

Class MainWindow

    Sub New()
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    End Sub

    Private Sub openButton_Click(sender As Object, e As RoutedEventArgs) Handles openButton.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez un fichier..."
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        ' check à faire si fichier de merde !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        If fd.ShowDialog() = Forms.DialogResult.OK Then
            mediaScreen.Source = New Uri(fd.FileName)
        End If
    End Sub

    Private Sub Element_MediaOpened(ByVal sender As Object, ByVal args As RoutedEventArgs)
        timeSlider.Maximum = mediaScreen.NaturalDuration.TimeSpan.TotalMilliseconds
        timeSliderMaxTime.Content = mediaScreen.NaturalDuration.TimeSpan.ToString("hh\:mm\:ss")
    End Sub

    Private Sub Element_MediaEnded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        mediaScreen.Stop()
    End Sub

    Private Sub playButton_Click(sender As Object, e As RoutedEventArgs) Handles playButton.Click
        If mediaScreen.Source <> Nothing Then
            mediaScreen.Play()
        End If
    End Sub

    Private Sub timeSlider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles timeSlider.ValueChanged
        Dim SliderValue As Integer = CType(timeSlider.Value, Integer)

        Dim ts As New TimeSpan(0, 0, 0, 0, SliderValue)
        mediaScreen.Position = ts

        timeSliderCurrentTime.Content = TimeSpan.FromSeconds(timeSlider.Value).ToString("hh\:mm\:ss")
    End Sub
End Class