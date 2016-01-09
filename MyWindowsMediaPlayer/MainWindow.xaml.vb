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
    Private Property _fileName As String
    Private Property _videoFlux As VideoFlux

    Sub New()
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        _fileName = ""
    End Sub

    Private Sub openButton_Click(sender As Object, e As RoutedEventArgs) Handles openButton.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()

        fd.Title = "Choisissez un fichier..."
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True

        ' check à faire si fichier de merde !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        'If fd.ShowDialog() = DialogResult Then
        fd.ShowDialog()
        _fileName = fd.FileName

        mediaScreen.Source = New Uri(_fileName)
        mediaScreen.LoadedBehavior = MediaState.Manual
        timeSlider.Maximum = mediaScreen.NaturalDuration.TimeSpan.TotalMilliseconds
        'End If
    End Sub

    Private Sub playButton_Click(sender As Object, e As RoutedEventArgs) Handles playButton.Click
        If _fileName <> "" Then
            mediaScreen.Play()
        End If
    End Sub

    Private Sub timeSlider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double)) Handles timeSlider.ValueChanged
        Dim SliderValue As Integer = CType(timeSlider.Value, Integer)

        ' Overloaded constructor takes the arguments days, hours, minutes, seconds, miniseconds.
        ' Create a TimeSpan with miliseconds equal to the slider value.
        Dim ts As New TimeSpan(0, 0, 0, 0, SliderValue)
        mediaScreen.Position = ts
    End Sub
End Class