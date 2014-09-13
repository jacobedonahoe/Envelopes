Imports System.IO

Public Class Form1
    Dim textToWrite As String = ""
    Dim fileName As String = ""
    Dim frmLog As New System.Windows.Forms.Form
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim myReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("Envelopes.txt")

        myReader.TextFieldType = FileIO.FieldType.Delimited
        myReader.SetDelimiters(",")

        Dim currentRow As String()
        Dim balance As String
        Dim name As String
        Dim envelopesArray As New ArrayList

        While Not myReader.EndOfData
            Try
                currentRow = myReader.ReadFields()
                name = currentRow(0)
                balance = currentRow(1)

                envelopesArray.Add(generate(name, balance))

            Catch ex As Microsoft.VisualBasic.FileIO.MalformedLineException
                MsgBox("Envelope" & ex.Message & "is not valid and will be skipped.")
            End Try
        End While

        Dim envelopeCount As Integer = 0

        For Each Envelope As Envelope In envelopesArray

            Dim envelopePanel As New System.Windows.Forms.Panel
            envelopePanel.Name = "pnl" & Envelope.name
            If envelopeCount < 5 Then
                envelopePanel.Location = New Point(0, envelopeCount * 70)
            ElseIf envelopeCount < 10 Then
                envelopePanel.Location = New Point(230, envelopeCount * 70)
            Else
                envelopePanel.Location = New Point(460, envelopeCount * 70)
            End If
            envelopePanel.Height = 70
            envelopePanel.Width = 230

            Dim envelopeNameButton As New System.Windows.Forms.Button
            AddHandler envelopeNameButton.Click, AddressOf envelopeNameButton_Click
            envelopeNameButton.Name = "btn" & Envelope.name
            envelopeNameButton.Text = Envelope.name
            envelopeNameButton.Location = New Point(10, 10)
            envelopeNameButton.Height = 50
            envelopeNameButton.Width = 100

            Dim envelopeBalanceButton As New System.Windows.Forms.Button
            envelopeBalanceButton.Name = "btn" & Envelope.name & "Balance"
            envelopeBalanceButton.Text = Envelope.Balance
            envelopeBalanceButton.Location = New Point(115, 10)
            envelopeBalanceButton.Height = 25
            envelopeBalanceButton.Width = 100

            Dim depositButton As New System.Windows.Forms.Button
            depositButton.Text = "$+"
            depositButton.ForeColor = Color.ForestGreen
            depositButton.Location = New Point(115, 35)
            depositButton.Height = 25
            depositButton.Width = 50

            Dim withdrawlButton As New System.Windows.Forms.Button
            withdrawlButton.Text = "$-"
            withdrawlButton.ForeColor = Color.DarkRed
            withdrawlButton.Location = New Point(165, 35)
            withdrawlButton.Height = 25
            withdrawlButton.Width = 50

            envelopePanel.Controls.Add(envelopeNameButton)
            envelopePanel.Controls.Add(envelopeBalanceButton)
            envelopePanel.Controls.Add(depositButton)
            envelopePanel.Controls.Add(withdrawlButton)

            Me.Controls.Add(envelopePanel)
            envelopeCount += 1
        Next

    End Sub
    Protected Sub envelopeNameButton_Click(sender As Object, e As EventArgs)
        fileName = sender.text & ".txt"
        frmLog.Height = 600
        frmLog.Width = 600

        Dim txtLog As New System.Windows.Forms.TextBox
        txtLog.Text = File.ReadAllText(fileName)
        txtLog.Refresh()
        AddHandler txtLog.TextChanged, AddressOf textChangedEventHandler
        txtLog.Height = 400
        txtLog.Width = 500
        txtLog.ScrollBars = ScrollBars.Both
        txtLog.Multiline = True
        textToWrite = txtLog.Text

        Dim btnSave As New System.Windows.Forms.Button
        AddHandler btnSave.Click, AddressOf btnSave_Click
        btnSave.Location = New Point(250, 500)
        btnSave.Height = 50
        btnSave.Text = "Save"
        btnSave.Width = 100



        Dim btnExit As New System.Windows.Forms.Button
        frmLog.Controls.Add(txtLog)
        frmLog.Controls.Add(btnSave)
        frmLog.Show()
    End Sub
    Private Sub textChangedEventHandler(sender As Object, e As System.EventArgs)
        textToWrite = sender.text
    End Sub
    Private Sub btnSave_Click()
        File.WriteAllText(fileName, textToWrite)
        frmLog.Hide()
    End Sub

    Public Function generate(ByRef name, ByRef balance) As Envelope
        Dim newEnvelope As New Envelope(name, balance)
        Return newEnvelope
    End Function
End Class
