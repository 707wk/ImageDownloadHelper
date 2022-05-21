Public NotInheritable Class _WebsiteFactory
    Private Sub New()
    End Sub

    Public Shared Function Create(pageUrl As String) As _BaseWebsite

        If SiteNhentai_net.IsEquals(pageUrl) Then
            Return New SiteNhentai_net(pageUrl)

        ElseIf Site177pica_com.IsEquals(pageUrl) Then
            Return New Site177pica_com(pageUrl)

            'ElseIf pixiv_net_users.IsEquals(pageUrl) Then
            '    Return New pixiv_net_users(pageUrl)

        Else
            Throw New Exception($"不支持的网址:{ pageUrl}")
        End If

    End Function

End Class
