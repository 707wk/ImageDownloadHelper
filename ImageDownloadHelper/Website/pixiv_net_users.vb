Public Class pixiv_net_users
    Inherits _BaseWebsite

    Public Sub New(url As String)
        MyBase.New(url)
    End Sub

    Public Overrides Sub Download()
        Throw New NotImplementedException()
    End Sub

    Public Shared Function IsEquals(pageUrl As String) As Boolean
        If pageUrl.ToLower.IndexOf("pixiv.net/users".ToLower) > -1 Then
            Return True
        End If

        Return False
    End Function

End Class
