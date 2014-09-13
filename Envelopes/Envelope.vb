Public Class Envelope
    Private mName
    Private mBalance

    Public Sub New(name As String, balance As String)
        Me.mName = name
        Me.mBalance = System.Convert.ToDecimal(balance)
    End Sub

    Property name() As String
        Get
            Return mName
        End Get
        Set(value As String)
            mName = value
        End Set
    End Property
    Property Balance() As Integer
        Get
            Return mBalance
        End Get
        Set(value As Integer)
            mBalance = value
        End Set
    End Property
    Public Sub deposit(amount As Integer)
        Me.mBalance = Me.mBalance - amount
    End Sub
    Public Sub withdraw(amount As Integer)
        Me.mBalance = Me.mBalance - amount
    End Sub
End Class
