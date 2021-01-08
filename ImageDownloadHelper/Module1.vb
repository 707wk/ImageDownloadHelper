Imports System.Net
Imports Microsoft.WindowsAPICodePack.Taskbar

Module Module1

    Public tmpTaskbarManager As TaskbarManager

    Sub Main(args As String())
        tmpTaskbarManager = TaskbarManager.Instance

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or
            SecurityProtocolType.Tls Or
            SecurityProtocolType.Tls11 Or
            SecurityProtocolType.Tls12

        Dim assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location
        Console.WriteLine($"程序版本 :{System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion}")
        Dim pageUrl As String
        If args.Count = 1 Then
            pageUrl = args(0).Substring(args(0).IndexOf(":") + 1)
            Console.WriteLine($"下载页面地址 :{pageUrl}")
        Else
            Console.Write($"输入下载页面地址 :")
            pageUrl = Console.ReadLine
        End If

        Console.WriteLine($"开始下载 :{Now}")
        Try
            Dim tmp = _WebsiteFactory.Create(pageUrl)
            tmp.DirectoryPath = "D:\Downloads"
            tmp.Download()

            Exit Sub

        Catch ex As Exception
            tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.Error)

            Console.WriteLine($"下载异常 :{ex.Message}")
            If MsgBox($"下载异常 {ex.Message},是否尝试继续下载?",
                      MsgBoxStyle.YesNo,
                      pageUrl) <> MsgBoxResult.Yes Then
            Else
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, args(0))
            End If
            Exit Sub
        End Try

    End Sub

End Module
