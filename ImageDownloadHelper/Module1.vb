Imports System.Net
Imports System.Web

Module Module1

    Sub Main(args As String())

        Console.WriteLine(String.Join("#", args))

        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 Or
            SecurityProtocolType.Tls Or
            SecurityProtocolType.Tls11 Or
            SecurityProtocolType.Tls12

        Dim assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location
        Console.WriteLine($"程序版本 :{System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion}")

        Console.Write($"输入下载页面地址 :")
        Dim pageUrl As String = Console.ReadLine

        Do
            Console.WriteLine($"开始下载 :{Now}")
            Try
                Dim tmp = _WebsiteFactory.Create(pageUrl)
                tmp.DirectoryPath = "D:\Downloads"
                tmp.Download()

                Exit Sub

            Catch ex As Exception
                Console.WriteLine($"下载异常 :{ex.Message}")
                If MsgBox($"下载异常 {ex.Message},是否尝试继续下载?",
                          MsgBoxStyle.YesNo,
                          pageUrl) <> MsgBoxResult.Yes Then
                    Exit Sub
                End If

            End Try
        Loop


    End Sub

End Module
