Public MustInherit Class _BaseWebsite
    ''' <summary>
    ''' 下载页面地址
    ''' </summary>
    Public PageUri As Uri

    ''' <summary>
    ''' 存放文件夹地址
    ''' </summary>
    Public DirectoryPath As String

    Public Sub New(url As String)
        PageUri = New Uri(url)
    End Sub

    '''' <summary>
    '''' 下载是否出错
    '''' </summary>
    'Public IsError As Boolean = False

    Public MustOverride Sub Download()

End Class
