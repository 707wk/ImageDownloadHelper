Module Module1

    Sub Main()

        Dim assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location
        Console.WriteLine($"程序版本 :{System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion}")

        Console.WriteLine($"工作目录 :{System.IO.Directory.GetCurrentDirectory}")

        Console.Write($"输入检索文件夹路径(输入为空则使用工作目录) :")
        Dim DirectoryPath = Console.ReadLine

        If String.IsNullOrWhiteSpace(DirectoryPath) Then
            Console.WriteLine($"使用工作目录 :{System.IO.Directory.GetCurrentDirectory}")
            DirectoryPath = System.IO.Directory.GetCurrentDirectory
        End If

        Dim fileList = IO.Directory.GetFiles(DirectoryPath)
        Dim count = fileList.Count

        Console.WriteLine($"文件数 : {count}")

        Dim groupList As New Dictionary(Of Char, Integer)

        Dim processedCount = 0
        For i001 = 0 To count - 1

            Console.SetCursorPosition(0, Console.CursorTop)
            Console.Write($"处理第 {i001 + 1}/{count} 个文件")

            Dim fileName = IO.Path.GetFileName(fileList(i001))

            If fileName.Chars(0) <> "[" Then
                Continue For
            End If
            processedCount += 1

            Dim authorName = fileName.Substring(1, fileName.IndexOf("]") - 1)

            Dim groupName = "…"c

            If (authorName.Chars(0) >= "a" AndAlso authorName.Chars(0) <= "z") OrElse
                (authorName.Chars(0) >= "A" AndAlso authorName.Chars(0) <= "Z") Then
                '字母

                groupName = Char.ToUpper(authorName.Chars(0))

            ElseIf authorName.Chars(0) >= "0" AndAlso authorName.Chars(0) <= "9" Then
                '数字
                groupName = "#"

            ElseIf Char.IsPunctuation(authorName.Chars(0)) Then
                '标点符号
                groupName = "&"

            End If

            Try

                IO.Directory.CreateDirectory($"{DirectoryPath}\{groupName}\{authorName}")

                Dim newFileName = $"{DirectoryPath}\{groupName}\{authorName}\{fileName}"
                If IO.File.Exists(newFileName) Then

                    Dim fileCount = 1
                    Do
                        newFileName = $"{DirectoryPath}\{groupName}\{authorName}\{IO.Path.GetFileNameWithoutExtension(fileName)} ({fileCount}){IO.Path.GetExtension(fileName)}"

                        If Not IO.File.Exists(newFileName) Then
                            Exit Do
                        End If

                        fileCount += 1
                    Loop

                End If

                IO.File.Move(fileList(i001), newFileName)

            Catch ex As Exception
                Continue For
            End Try

            If groupList.ContainsKey(groupName) Then
                Dim tmpCount = groupList(groupName)
                groupList(groupName) = tmpCount + 1
            Else
                groupList.Add(groupName, 1)
            End If

        Next
        Console.WriteLine()

        Console.WriteLine($"已处理 : {processedCount}/{count} 个文件")

        For Each item In groupList
            Console.WriteLine($"{item.Key} : {item.Value}")
        Next

        Console.ReadLine()

    End Sub

End Module
