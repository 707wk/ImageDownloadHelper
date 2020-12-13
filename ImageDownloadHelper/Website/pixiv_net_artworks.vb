Imports System.IO
Imports System.Net
Imports System.Text

Public Class pixiv_net_artworks
    Inherits _BaseWebsite

    Public Sub New(url As String)
        MyBase.New(url)
    End Sub

    Public Overrides Sub Download()
        Using downladWebClient As New Net.WebClient
            Dim webClient As HtmlAgilityPack.HtmlWeb = New HtmlAgilityPack.HtmlWeb()
            Dim doc As HtmlAgilityPack.HtmlDocument = webClient.Load(PageUri)
            Dim titleNodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//title")

            If titleNodes Is Nothing Then
                Throw New Exception("未找到标题信息")
            End If

            Dim tmpstrArray = titleNodes(0).InnerText.Split(" ")

            Dim title As String = $"[{tmpstrArray(3).Remove(tmpstrArray(3).Count - 4)}] {tmpstrArray(1)}".
                    Replace("\", "_").
                    Replace("/", "_").
                    Replace(":", "_").
                    Replace("*", "_").
                    Replace("?", "_").
                    Replace("""", "_").
                    Replace("<", "_").
                    Replace(">", "_").
                    Replace("|", "_")
            Dim tmpPath As String = $"{DirectoryPath}\{title}"

            System.IO.Directory.CreateDirectory(tmpPath)
            Console.WriteLine($"标题: {title}")

            Dim artworksID = PageUri.AbsoluteUri.Substring(PageUri.AbsoluteUri.LastIndexOf("/") + 1)
            '需要登陆才能下载
            Console.WriteLine(HttpGetString($"https://www.pixiv.net/ajax/illust/{artworksID}/pages"))
            Console.ReadLine()
            'titleNodes = doc.DocumentNode.SelectNodes("//a[@class='gallerythumb']")
            'For Each item In titleNodes

            '    Console.SetCursorPosition(0, Console.CursorTop)
            '    Console.Write($"{titleNodes.IndexOf(item) + 1} /{titleNodes.Count}")

            '    doc = webClient.Load($"https://{PageUri.Host}{item.Attributes("href").Value}")

            '    Dim imageNodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//section[@id='image-container']//img")
            '    Dim imageSrc As String = $"{imageNodes(0).Attributes("src").Value}"

            '    If IO.File.Exists($"{tmpPath}\{IO.Path.GetFileName(imageSrc)}") Then
            '        Continue For
            '    End If

            '    downladWebClient.DownloadFile(imageSrc, $"{tmpPath}\{IO.Path.GetFileName(imageSrc)}")

            'Next
        End Using
    End Sub

    Public Function HttpGetString(url As String) As String
        '请求头
        Dim request As System.Net.HttpWebRequest = WebRequest.Create(url)
        request.Method = "GET"
        request.ContentType = "application/json"
        request.Referer = PageUri.AbsoluteUri

        Dim resp As HttpWebResponse = request.GetResponse()
        Dim Stream As Stream = resp.GetResponseStream()
        Using reader As StreamReader = New StreamReader(Stream, Encoding.UTF8)
            Return reader.ReadToEnd()
        End Using

    End Function

    Public Shared Function IsEquals(pageUrl As String) As Boolean
        If pageUrl.ToLower.IndexOf("pixiv.net/artworks".ToLower) > -1 Then
            Return True
        End If

        Return False
    End Function

End Class
