﻿Imports System.IO

Public Class NewPartition

    Private Sub CreatePartitionButton_Click(sender As Object, e As RoutedEventArgs) Handles CreatePartitionButton.Click

        If MsgBox("Dou you really want to create the partition " + NewPartitionNameTextBox.Text + " with the size of " + NewPartitionSizeTextBox.Text + "MB ?", MsgBoxStyle.YesNo, "Please confirm") = MsgBoxResult.Yes Then

            'Set mkpart command
            Using CommandFileWriter As New StreamWriter(My.Computer.FileSystem.CurrentDirectory + "\tools\cmdlist\mkpart.txt", False)
                CommandFileWriter.WriteLine("device " + MainWindow.MountedDrive.DriveID)
                CommandFileWriter.WriteLine("mkpart " + NewPartitionNameTextBox.Text + " " + NewPartitionSizeTextBox.Text + "M PFS")
                CommandFileWriter.WriteLine("exit")
            End Using

            'Proceed to partition creation
            Dim PFSShellOutput As String
            Using PFSShellProcess As New Process()
                PFSShellProcess.StartInfo.FileName = "cmd"
                PFSShellProcess.StartInfo.Arguments = """/c type """ + My.Computer.FileSystem.CurrentDirectory + "\tools\cmdlist\mkpart.txt"" | ""C:\Users\SvenGDK\source\repos\DESRApps\bin\Release\tools\pfsshell.exe"" 2>&1"

                PFSShellProcess.StartInfo.RedirectStandardOutput = True
                PFSShellProcess.StartInfo.UseShellExecute = False

                PFSShellProcess.Start()

                Dim ShellReader As StreamReader = PFSShellProcess.StandardOutput
                Dim ProcessOutput As String = ShellReader.ReadToEnd()

                ShellReader.Close()
                PFSShellOutput = ProcessOutput
            End Using

            If PFSShellOutput.Contains("Main partition of " + NewPartitionSizeTextBox.Text + "M created.") Then
                MsgBox("Partition " + NewPartitionNameTextBox.Text + " created with success!", MsgBoxStyle.Information)
                Utils.ReloadPartitions()
                Close()
            Else
                MsgBox("There was an error in creating the partition, please check if the name doesn't already exists of if you have enough space." + vbCrLf + PFSShellOutput, MsgBoxStyle.Exclamation)
            End If
        End If

    End Sub

End Class
