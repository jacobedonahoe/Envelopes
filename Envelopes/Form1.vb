Imports System.IO

Public Class Form1

    Dim envelopesArray As New ArrayList
    Dim currentEnvelope As New Object
    Dim amount As Decimal
    Dim balanceOfNewEnvelope
    Dim message As String
    Dim nameOfNewEnvelope As String
    Dim blankException As New Exception
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim myReader As New Microsoft.VisualBasic.FileIO.TextFieldParser("Envelopes.txt")

        myReader.TextFieldType = FileIO.FieldType.Delimited
        myReader.SetDelimiters(",")

        Dim currentRow As String()
        Dim balance As String
        Dim name As String

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

        generatePanels()
    End Sub
    Public Sub updateEnvelopes()
        Dim myWriter As New System.IO.StreamWriter("Envelopes.txt", False)
        For Each Envelope In envelopesArray
            myWriter.WriteLine(Envelope.name & "," & Envelope.balance & vbCrLf)
        Next
        myWriter.Close()
    End Sub
    Public Sub refreshForm()
        updateEnvelopes()
        Me.Controls.Clear()
        generatePanels()
    End Sub
    Public Sub generatePanels()
        Dim envelopeCount As Integer = 0

        For Each Envelope As Envelope In envelopesArray

            Dim envelopePanel As New System.Windows.Forms.Panel
            envelopePanel.Name = "pnl" & Envelope.name
            If envelopeCount < 7 Then
                envelopePanel.Location = New Point(0, envelopeCount * 70)
            ElseIf envelopeCount < 14 Then
                envelopePanel.Location = New Point(230, (envelopeCount - 7) * 70)
            Else
                envelopePanel.Location = New Point(460, (envelopeCount - 14) * 70)
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
            AddHandler depositButton.Click, AddressOf depositButton_Click
            depositButton.Name = "btnDeposit" & Envelope.name
            depositButton.Text = "$+"
            depositButton.ForeColor = Color.ForestGreen
            depositButton.Location = New Point(115, 35)
            depositButton.Height = 25
            depositButton.Width = 50

            Dim withdrawlButton As New System.Windows.Forms.Button
            AddHandler withdrawlButton.Click, AddressOf withdrawlButton_Click
            withdrawlButton.Name = "btnWithdrawl" & Envelope.name
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
        Dim btnAddEnvelope As New System.Windows.Forms.Button
        AddHandler btnAddEnvelope.Click, AddressOf btnAddEnvelope_Click
        btnAddEnvelope.Location = New Point(200, 500)
        btnAddEnvelope.Width = 200
        btnAddEnvelope.Height = 50
        btnAddEnvelope.Text = "Add new envelope"

        Me.Controls.Add(btnAddEnvelope)
    End Sub
    Private Sub textChangedEventHandler(ByVal sender As Object, ByVal args As EventArgs)
        If sender.name = "txtAmount" Then
            Try
                amount = Convert.ToDecimal(sender.text)
            Catch ex As Exception
                MessageBox.Show("please enter a number")
            End Try
        End If
        If sender.name = "txtMessage" Then
            message = sender.text
        End If
        If sender.name = "txtNameOfEnvelope" Then
            Try
                If sender.text = "" Then
                    Throw blankException
                End If
                balanceOfNewEnvelope = (sender.text)
            Catch ex As Exception
                MessageBox.Show("cannot be left blank")
            End Try
            nameOfNewEnvelope = sender.text
        End If
        If sender.name = "txtBalanceOfEnvelope" Then
            Try
                If sender.text = "" Then
                    Throw blankException
                End If
                balanceOfNewEnvelope = (sender.text)
            Catch ex As Exception
                MessageBox.Show("cannot be left blank")
            End Try
        End If
    End Sub
    Private Sub btnDepositSubmit_Click(sender As Object, e As EventArgs)
        currentEnvelope.deposit(amount, message)
        refreshForm()
        sender.parent.close()
    End Sub
    Private Sub btnWithdrawlSubmit_Click(sender As Object, e As EventArgs)
        currentEnvelope.withdraw(amount, message)
        refreshForm()
        sender.parent.close()
    End Sub
    Private Sub envelopeNameButton_Click(sender As Object, e As EventArgs)
        Dim frmLog As New System.Windows.Forms.Form
        frmLog.Height = 600
        frmLog.Width = 600

        Dim txtLog As New System.Windows.Forms.TextBox
        txtLog.Text = File.ReadAllText(sender.text & ".txt")
        txtLog.Refresh()
        txtLog.Height = 400
        txtLog.Width = 500
        txtLog.ScrollBars = ScrollBars.Both
        txtLog.Multiline = True
        txtLog.ReadOnly = True


        Dim btnExit As New System.Windows.Forms.Button
        AddHandler btnExit.Click, AddressOf btnExit_Click
        btnExit.Location = New Point(250, 500)
        btnExit.Height = 50
        btnExit.Text = "Return to System"
        btnExit.Width = 100

        frmLog.Controls.Add(txtLog)
        frmLog.Controls.Add(btnExit)
        frmLog.Show()
    End Sub
    Private Sub btnExit_Click(sender As Object, e As EventArgs)
        sender.parent.Close()
    End Sub
    Private Sub depositButton_Click(sender As Object, e As EventArgs)
        currentEnvelope = findEnvelopeByName(sender.name.remove(0, 10))
        Dim frmDeposit As New System.Windows.Forms.Form
        frmDeposit.Height = 600
        frmDeposit.Width = 600

        Dim lblAmount As New System.Windows.Forms.Label
        lblAmount.Location = New Point(10, 10)
        lblAmount.Text = "Amount"
        lblAmount.Height = 15
        lblAmount.Width = 100

        Dim txtAmount As New System.Windows.Forms.TextBox
        AddHandler txtAmount.TextChanged, AddressOf textChangedEventHandler
        txtAmount.Name = "txtAmount"
        txtAmount.Location = New Point(10, 30)
        txtAmount.Width = 100

        Dim lblMessage As New System.Windows.Forms.Label
        lblMessage.Location = New Point(120, 10)
        lblMessage.Text = "Message"
        lblMessage.Height = 15
        lblMessage.Width = 100

        Dim txtMessage As New System.Windows.Forms.TextBox
        AddHandler txtMessage.TextChanged, AddressOf textChangedEventHandler
        txtMessage.Name = "txtMessage"
        txtMessage.Height = 50
        txtMessage.Width = 100
        txtMessage.Location = New Point(120, 30)
        txtMessage.ScrollBars = ScrollBars.Both
        txtMessage.MaxLength = 100
        txtMessage.Multiline = True

        Dim btnSubmit As New System.Windows.Forms.Button
        AddHandler btnSubmit.Click, AddressOf btnDepositSubmit_Click
        btnSubmit.Height = 50
        btnSubmit.Width = 100
        btnSubmit.Location = New Point(10, 100)
        btnSubmit.Text = "Deposit"

        Dim btnReturn As New System.Windows.Forms.Button
        AddHandler btnReturn.Click, AddressOf btnExit_Click
        btnReturn.Height = 50
        btnReturn.Width = 100
        btnReturn.Text = "Return"
        btnReturn.Location = New Point(120, 100)


        frmDeposit.Controls.Add(lblAmount)
        frmDeposit.Controls.Add(lblMessage)
        frmDeposit.Controls.Add(txtAmount)
        frmDeposit.Controls.Add(txtMessage)
        frmDeposit.Controls.Add(btnSubmit)
        frmDeposit.Controls.Add(btnReturn)

        frmDeposit.Show()
    End Sub
    Private Sub withdrawlButton_Click(sender As Object, e As EventArgs)
        currentEnvelope = findEnvelopeByName(sender.name.remove(0, 12))
        Dim frmWithdrawl As New System.Windows.Forms.Form
        frmWithdrawl.Height = 600
        frmWithdrawl.Width = 600

        Dim lblAmount As New System.Windows.Forms.Label
        lblAmount.Location = New Point(10, 10)
        lblAmount.Text = "Amount"
        lblAmount.Height = 15
        lblAmount.Width = 100

        Dim txtAmount As New System.Windows.Forms.TextBox
        AddHandler txtAmount.TextChanged, AddressOf textChangedEventHandler
        txtAmount.Name = "txtAmount"
        txtAmount.Location = New Point(10, 30)
        txtAmount.Width = 100

        Dim lblMessage As New System.Windows.Forms.Label
        lblMessage.Location = New Point(120, 10)
        lblMessage.Text = "Message"
        lblMessage.Height = 15
        lblMessage.Width = 100

        Dim txtMessage As New System.Windows.Forms.TextBox
        AddHandler txtMessage.TextChanged, AddressOf textChangedEventHandler
        txtMessage.Name = "txtMessage"
        txtMessage.Height = 50
        txtMessage.Width = 100
        txtMessage.Location = New Point(120, 30)
        txtMessage.ScrollBars = ScrollBars.Both
        txtMessage.MaxLength = 100
        txtMessage.Multiline = True

        Dim btnSubmit As New System.Windows.Forms.Button
        AddHandler btnSubmit.Click, AddressOf btnWithdrawlSubmit_Click
        btnSubmit.Height = 50
        btnSubmit.Width = 100
        btnSubmit.Location = New Point(10, 100)
        btnSubmit.Text = "Withdraw"

        Dim btnReturn As New System.Windows.Forms.Button
        AddHandler btnReturn.Click, AddressOf btnExit_Click
        btnReturn.Height = 50
        btnReturn.Width = 100
        btnReturn.Text = "Return"
        btnReturn.Location = New Point(120, 100)


        frmWithdrawl.Controls.Add(lblAmount)
        frmWithdrawl.Controls.Add(lblMessage)
        frmWithdrawl.Controls.Add(txtAmount)
        frmWithdrawl.Controls.Add(txtMessage)
        frmWithdrawl.Controls.Add(btnSubmit)
        frmWithdrawl.Controls.Add(btnReturn)

        frmWithdrawl.Show()
    End Sub
    Private Sub btnAddEnvelope_Click()
        Dim frmAddEnvelope As New System.Windows.Forms.Form

        Dim lblName As New System.Windows.Forms.Label
        lblName.Height = 20
        lblName.Location = New Point(10, 10)
        lblName.Text = "Name of Envelope"
        lblName.Width = 100

        Dim txtNameOfEnvelope As New System.Windows.Forms.TextBox
        txtNameOfEnvelope.Text = "name"
        AddHandler txtNameOfEnvelope.TextChanged, AddressOf textChangedEventHandler
        txtNameOfEnvelope.Height = 20
        txtNameOfEnvelope.Location = New Point(10, 40)
        txtNameOfEnvelope.Name = "txtNameOfEnvelope"
        txtNameOfEnvelope.Width = 100

        Dim lblBalance As New System.Windows.Forms.Label
        lblBalance.Height = 20
        lblBalance.Location = New Point(120, 10)
        lblBalance.Text = "Starting Balance of Envelope"
        lblBalance.Width = 100

        Dim txtBalanceOfEnvelope As New System.Windows.Forms.TextBox
        txtBalanceOfEnvelope.Text = "0"
        AddHandler txtBalanceOfEnvelope.TextChanged, AddressOf textChangedEventHandler
        txtBalanceOfEnvelope.Height = 20
        txtBalanceOfEnvelope.Location = New Point(120, 40)
        txtBalanceOfEnvelope.Name = "txtBalanceOfEnvelope"
        txtBalanceOfEnvelope.Width = 100

        Dim btnAdd As New System.Windows.Forms.Button
        AddHandler btnAdd.Click, AddressOf btnAdd_Click
        btnAdd.Height = 50
        btnAdd.Location = New Point(10, 70)
        btnAdd.Name = "btnAdd"
        btnAdd.Text = "Add Envelope"
        btnAdd.Width = 100

        Dim btnExit As New System.Windows.Forms.Button
        AddHandler btnExit.Click, AddressOf btnExit_Click
        btnExit.Height = 50
        btnExit.Location = New Point(120, 70)
        btnExit.Name = "btnExit"
        btnExit.Text = "Return"
        btnExit.Width = 100

        frmAddEnvelope.Controls.Add(lblBalance)
        frmAddEnvelope.Controls.Add(txtNameOfEnvelope)
        frmAddEnvelope.Controls.Add(txtBalanceOfEnvelope)
        frmAddEnvelope.Controls.Add(lblName)
        frmAddEnvelope.Controls.Add(btnAdd)
        frmAddEnvelope.Controls.Add(btnExit)
        frmAddEnvelope.Show()
    End Sub
    Private Sub btnAdd_Click(sender As Object, e As EventArgs)
        Dim newEnvelope As Envelope
        If Not (nameOfNewEnvelope) = ("") Or Not (balanceOfNewEnvelope) = ("") Then
            newEnvelope = generate(nameOfNewEnvelope, balanceOfNewEnvelope)
            envelopesArray.Add(newEnvelope)
            newEnvelope.create()
            updateEnvelopes()
            refreshForm()
            sender.parent.close()
        Else
            MessageBox.Show("Both inputs must be filled!")
        End If
    End Sub
    Public Function findEnvelopeByName(name)
        Dim envelopeWeWant As New Object
        For Each Envelope In envelopesArray
            If Envelope.name = name Then
                envelopeWeWant = Envelope
            End If
        Next
        Return envelopeWeWant
    End Function
    Public Function generate(ByRef name, ByRef balance) As Envelope
        Dim newEnvelope As New Envelope(name, balance)
        Return newEnvelope
    End Function
End Class
