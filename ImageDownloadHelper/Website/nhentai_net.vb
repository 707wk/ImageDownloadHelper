Imports System.ComponentModel
Imports System.Deployment.Application
Imports System.Web

Public Class nhentai_net
    Inherits _BaseWebsite

    Public Sub New(url As String)
        MyBase.New(url)
    End Sub

    Public Overrides Sub Download()
        Using downladWebClient As New Net.WebClient

            Dim webClient As HtmlAgilityPack.HtmlWeb = New HtmlAgilityPack.HtmlWeb()
            Dim doc As HtmlAgilityPack.HtmlDocument = webClient.Load(PageUri)
            Debug.WriteLine(doc.DocumentNode.OuterHtml)
            Dim titleNodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//div[@id='info']/h1")

            If titleNodes Is Nothing Then
                Throw New Exception("未找到标题信息")
            End If

            Dim title As String = $"{HttpUtility.HtmlDecode(titleNodes(0).InnerText)}".
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
            Console.WriteLine($"标题 :{title}")

            titleNodes = doc.DocumentNode.SelectNodes("//a[@class='gallerythumb']")
            For Each item In titleNodes

                Console.SetCursorPosition(0, Console.CursorTop)
                Console.Write($"下载进度 :{titleNodes.IndexOf(item) + 1,4}/{titleNodes.Count}")

                doc = webClient.Load($"https://{PageUri.Host}{item.Attributes("href").Value}")

                Dim imageNodes As HtmlAgilityPack.HtmlNodeCollection = doc.DocumentNode.SelectNodes("//section[@id='image-container']//img")
                Dim imageSrc As String = $"{imageNodes(0).Attributes("src").Value}"

                If IO.File.Exists($"{tmpPath}\{IO.Path.GetFileName(imageSrc)}") Then
                    Continue For
                End If

                downladWebClient.DownloadFile(imageSrc, $"{tmpPath}\{IO.Path.GetFileName(imageSrc)}")

            Next

        End Using
    End Sub

    Public Shared Function IsEquals(pageUrl As String) As Boolean
        If pageUrl.ToLower.IndexOf("nhentai.net".ToLower) > -1 Then
            Return True
        End If

        Return False
    End Function

End Class
