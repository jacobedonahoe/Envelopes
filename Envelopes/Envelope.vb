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
    Property Balance() As String
        Get
            Return mBalance
        End Get
        Set(value As String)
            mBalance = value
        End Set
    End Property
    Public Function deposit(amount As Integer)
        Return Me.mBalance = Me.mBalance - amount
    End Function
    Public Function withdraw(amount As Integer)
        Return Me.mBalance = Me.mBalance - amount
    End Function
End Class
