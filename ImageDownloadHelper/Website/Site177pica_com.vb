Imports System.Net.Http
Imports System.Web

Public Class Site177pica_com
    Inherits _BaseWebsite

    Public Sub New(url As String)
        MyBase.New(url)
    End Sub

    Private ImageDownloadWebClient As New Net.WebClient
    Private AnalysisHtmlWeb As HtmlAgilityPack.HtmlWeb = New HtmlAgilityPack.HtmlWeb()

    Public Overrides Sub Download()

        Dim doc As HtmlAgilityPack.HtmlDocument = AnalysisHtmlWeb.Load(PageUri)
        'Debug.WriteLine(doc.DocumentNode.OuterHtml)
        Dim titleNodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//h1[@class='entry-title']")

        If titleNodes Is Nothing Then
            Throw New Exception("未找到标题信息")
        End If

        Dim title As String = HttpUtility.HtmlDecode(titleNodes(0).InnerText)

        For Each invalidPathChar In System.IO.Path.GetInvalidFileNameChars
            title = title.Replace(invalidPathChar, "_")
        Next

        For Each invalidPathChar In System.IO.Path.GetInvalidPathChars
            title = title.Replace(invalidPathChar, "_")
        Next

        Console.WriteLine($"标题 :{title}")

        DirectoryPath = $"{DirectoryPath}\{title}"

        System.IO.Directory.CreateDirectory(DirectoryPath)

        Dim pageLinks As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//div[@class='page-links']")

        If pageLinks Is Nothing Then
            Throw New Exception("未找到分页信息")
        End If

        Dim pageIndexs As HtmlAgilityPack.HtmlNodeCollection = pageLinks(0).SelectNodes("//div[@class='page-links']//span")

        Dim PageCount = pageIndexs.Max(Function(html)
                                           Return Val(html.InnerText)
                                       End Function)

        For index = 1 To PageCount
            DownloadPageImage(index, PageCount)
        Next

    End Sub

    Private Sub DownloadPageImage(index As Integer, count As Integer)

        Dim doc As HtmlAgilityPack.HtmlDocument = AnalysisHtmlWeb.Load($"{PageUri}/{index}/")

        Dim titleNodes = doc.DocumentNode.SelectNodes("//div[@class='single-content']/p/img")
        For Each item In titleNodes

            tmpTaskbarManager.SetProgressValue(index, count)
            Console.Title = $"下载: 第 {index}/{count} 页 {titleNodes.IndexOf(item) + 1}/{titleNodes.Count} 个"

            'If titleNodes.IndexOf(item) > 10 Then
            '    Throw New Exception("测试异常")
            'End If

            Console.SetCursorPosition(0, Console.CursorTop)
            Console.Write(Console.Title)

            Dim imageSrc As String = $"{item.Attributes("data-lazy-src").Value}"

            If IO.File.Exists($"{DirectoryPath}\{IO.Path.GetFileName(imageSrc)}") Then
                Continue For
            End If

            ImageDownloadWebClient.DownloadFile(imageSrc, $"{DirectoryPath}\{IO.Path.GetFileName(imageSrc)}")

        Next

    End Sub

    Public Shared Function IsEquals(pageUrl As String) As Boolean
        If pageUrl.ToLower.IndexOf("177pica.com".ToLower) > -1 Then
            Return True
        End If

        Return False
    End Function

End Class
