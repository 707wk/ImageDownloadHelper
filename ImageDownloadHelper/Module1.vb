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

        Console.WriteLine($"等待中 :{Now}")
        Console.Title = "等待中"
        Do
            If GetDownloadingProgrameCount() < 5 Then
                Exit Do
            End If

            Threading.Thread.Sleep(3000)
        Loop

        Console.WriteLine($"开始下载 :{Now}")
        Console.Title = "开始下载"
        Try
            Dim tmp = _WebsiteFactory.Create(pageUrl)
            tmp.DirectoryPath = "D:\Downloads"
            tmp.Download()

            Exit Sub

        Catch ex As Exception
            tmpTaskbarManager.SetProgressState(TaskbarProgressBarState.Error)

            Console.WriteLine()
            Console.WriteLine($"下载异常 :{ex.Message}")

            For i001 = 5 To 1 Step -1
                Console.SetCursorPosition(0, Console.CursorTop)
                Console.Write($"{i001} 秒后尝试重新下载")
                Console.Title = "下载异常"

                Threading.Thread.Sleep(1000)
            Next

            Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location, args(0))

            Exit Sub
        End Try

    End Sub

    Private Function GetDownloadingProgrameCount() As Integer

        Dim currentProcess = Process.GetCurrentProcess

        Dim ProcessItems = Process.GetProcessesByName(currentProcess.ProcessName)

        Dim DownloadingProgrameCount = 0

        For Each item In ProcessItems

            If item.MainWindowTitle.Contains("下载") Then
                DownloadingProgrameCount += 1
            End If

        Next

        Return DownloadingProgrameCount

    End Function

End Module
