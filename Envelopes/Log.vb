Public Class Log
    Private mFileName
    Private mTextToWrite

    Property fileName() As String
        Get
            Return mFileName
        End Get
        Set(ByVal value As String)
            mFileName = value
        End Set
    End Property

    Property TexttoWrite() As String
        Get
            Return mTextToWrite
        End Get
        Set(value As String)
            mTextToWrite = value
        End Set
    End Property
    Public Sub editLog(TextForLog As String)
        Me.TexttoWrite = TextForLog
    End Sub
    Public Sub editFilePath(filepath As String)
        Me.fileName = filepath
    End Sub
End Class
