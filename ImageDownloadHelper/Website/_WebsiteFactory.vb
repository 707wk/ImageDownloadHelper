Public NotInheritable Class _WebsiteFactory
    Private Sub New()
    End Sub

    Public Shared Function Create(pageUrl As String) As _BaseWebsite

        If nhentai_net.IsEquals(pageUrl) Then
            Return New nhentai_net(pageUrl)

            'ElseIf pixiv_net_artworks.IsEquals(pageUrl) Then
            '    Return New pixiv_net_artworks(pageUrl)

            'ElseIf pixiv_net_users.IsEquals(pageUrl) Then
            '    Return New pixiv_net_users(pageUrl)

        Else
            Throw New Exception($"不支持的网址:{ pageUrl}")
        End If

    End Function

End Class
