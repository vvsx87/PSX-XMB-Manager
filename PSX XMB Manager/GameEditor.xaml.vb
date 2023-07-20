﻿Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports nQuant

Public Class GameEditor

    Public ProjectDirectory As String
    Public WithEvents PSXDatacenterBrowser As New WebBrowser()
    Public AutoSave As Boolean = False

    Private Sub LoadFromPSXButton_Click(sender As Object, e As RoutedEventArgs) Handles LoadFromPSXButton.Click
        Try
            If Not String.IsNullOrWhiteSpace(GameIDTextBox.Text) Then
                PSXDatacenterBrowser.Navigate("https://psxdatacenter.com/psx2/games2/" + GameIDTextBox.Text + ".html")
            Else
                MsgBox("Please enter a valid game ID (SLUS-12345) to perform a search.", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            MsgBox("Could not load game images and information, please check your Game ID.", MsgBoxStyle.Exclamation, "No information found for this game ID")
        End Try
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As RoutedEventArgs) Handles SaveButton.Click
        If Not Directory.Exists(ProjectDirectory + "\res\image") Then
            Directory.CreateDirectory(ProjectDirectory + "\res\image")
        End If

        Dim Quantizer As New WuQuantizer()

        'Save selected XMB cover as compressed PNG
        'Skips now already saved art
        If CoverPictureBox.Tag IsNot Nothing Then
            Dim Cover1Bitmap As Bitmap = Utils.GetResizedBitmap(CoverPictureBox.Tag.ToString, 140, 200)
            Dim Cover2Bitmap As Bitmap = Utils.GetResizedBitmap(CoverPictureBox.Tag.ToString, 74, 108)

                If Cover1Bitmap.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
                    Utils.ConvertTo32bppAndDisposeOriginal(Cover1Bitmap)
                End If
                If Cover2Bitmap.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
                    Utils.ConvertTo32bppAndDisposeOriginal(Cover2Bitmap)
                End If

            Try
                Using CompressedImage = Quantizer.QuantizeImage(Cover1Bitmap)
                    CompressedImage.Save(ProjectDirectory + "\res\jkt_001.png", Imaging.ImageFormat.Png)
                End Using
                Using CompressedImage = Quantizer.QuantizeImage(Cover2Bitmap)
                    CompressedImage.Save(ProjectDirectory + "\res\jkt_002.png", Imaging.ImageFormat.Png)
                End Using
            Catch ex As Exception
                MsgBox("Could not compress PNG." + vbCrLf + ex.Message, MsgBoxStyle.Exclamation)
            Finally
                Cover1Bitmap.Dispose()
            End Try
        End If

        If BackgroundImagePictureBox.Tag IsNot Nothing Then
            Dim BackgroundImageBitmap As Bitmap = Utils.GetResizedBitmap(BackgroundImagePictureBox.Tag.ToString, 640, 350)
            If BackgroundImageBitmap.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
                Utils.ConvertTo32bppAndDisposeOriginal(BackgroundImageBitmap)
            End If

            Try
                Using CompressedImage = Quantizer.QuantizeImage(BackgroundImageBitmap)
                    CompressedImage.Save(ProjectDirectory + "\res\image\0.png", Imaging.ImageFormat.Png)
                End Using
            Catch ex As Exception
                MsgBox("Could not compress PNG." + vbCrLf + ex.Message, MsgBoxStyle.Exclamation)
            Finally
                BackgroundImageBitmap.Dispose()
            End Try
        End If

        If ScreenshotImage1PictureBox.Tag IsNot Nothing Then
            Dim ScreenshotImageBitmap As Bitmap = Utils.GetResizedBitmap(ScreenshotImage1PictureBox.Tag.ToString, 640, 350)
            If ScreenshotImageBitmap.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
                Utils.ConvertTo32bppAndDisposeOriginal(ScreenshotImageBitmap)
            End If

            Try
                Using CompressedImage = Quantizer.QuantizeImage(ScreenshotImageBitmap)
                    CompressedImage.Save(ProjectDirectory + "\res\image\1.png", Imaging.ImageFormat.Png)
                End Using
            Catch ex As Exception
                MsgBox("Could not compress PNG." + vbCrLf + ex.Message, MsgBoxStyle.Exclamation)
            Finally
                ScreenshotImageBitmap.Dispose()
            End Try
        End If
        If ScreenshotImage2PictureBox.Tag IsNot Nothing Then
            Dim ScreenshotImageBitmap As Bitmap = Utils.GetResizedBitmap(ScreenshotImage2PictureBox.Tag.ToString, 640, 350)

            If ScreenshotImageBitmap.PixelFormat <> Imaging.PixelFormat.Format32bppArgb Then
                Utils.ConvertTo32bppAndDisposeOriginal(ScreenshotImageBitmap)
            End If

            Try
                Using CompressedImage = Quantizer.QuantizeImage(ScreenshotImageBitmap)
                    CompressedImage.Save(ProjectDirectory + "\res\image\2.png", Imaging.ImageFormat.Png)
                End Using
            Catch ex As Exception
                MsgBox("Could not compress PNG." + vbCrLf + ex.Message, MsgBoxStyle.Exclamation)
            Finally
                ScreenshotImageBitmap.Dispose()
            End Try
        End If

        'Write info.sys to res directory
        Using SYSWriter As New StreamWriter(ProjectDirectory + "\res\info.sys", False)
            SYSWriter.WriteLine("title = " + GameTitleTextBox.Text)
            SYSWriter.WriteLine("title_id = " + GameIDTextBox.Text)

            If ShowGameIDCheckBox.IsChecked Then
                SYSWriter.WriteLine("title_sub_id = 1")
            Else
                SYSWriter.WriteLine("title_sub_id = 0")
            End If

            SYSWriter.WriteLine("release_date = " + GameReleaseDateTextBox.Text)
            SYSWriter.WriteLine("developer_id = " + GameDeveloperTextBox.Text)
            SYSWriter.WriteLine("publisher_id = " + PublisherTextBox.Text)
            SYSWriter.WriteLine("note = " + GameNoteTextBox.Text)
            SYSWriter.WriteLine("content_web = " + GameWebsiteTextBox.Text)
            SYSWriter.WriteLine("image_topviewflag = 0")
            SYSWriter.WriteLine("image_type = 0")
            SYSWriter.WriteLine("image_count = 1")
            SYSWriter.WriteLine("image_viewsec = 600")

            If ShowCopyrightCheckBox.IsChecked Then
                SYSWriter.WriteLine("copyright_viewflag = 1")
            Else
                SYSWriter.WriteLine("copyright_viewflag = 0")
            End If

            SYSWriter.WriteLine("copyright_imgcount = 1")
            SYSWriter.WriteLine("genre = " + GameGenreTextBox.Text)
            SYSWriter.WriteLine("parental_lock = 1")
            SYSWriter.WriteLine("effective_date = 0")
            SYSWriter.WriteLine("expire_date = 0")
            SYSWriter.WriteLine("area = " + RegionTextBox.Text)
            SYSWriter.WriteLine("violence_flag = 0")
            SYSWriter.WriteLine("content_type = 255")
            SYSWriter.WriteLine("content_subtype = 0")
        End Using

        'Create man.xml
        Using MANWriter As New StreamWriter(ProjectDirectory + "\res\man.xml", True)
            MANWriter.WriteLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
            MANWriter.WriteLine("")
            MANWriter.WriteLine("<MANUAL version=""1.0"">")
            MANWriter.WriteLine("")
            MANWriter.WriteLine("<IMG id=""bg"" src=""./image/0.png"" />") 'This is the background image
            MANWriter.WriteLine("")
            MANWriter.WriteLine("<MENUGROUP id=""TOP"">")
            MANWriter.WriteLine("<TITLE id=""TOP-TITLE"" label=""" + GameTitleTextBox.Text + """ />")
            MANWriter.WriteLine("<ITEM id=""M00"" label=""Screenshots""	page=""PIC0000"" />")
            MANWriter.WriteLine("</MENUGROUP>")
            MANWriter.WriteLine("")
            MANWriter.WriteLine("<PAGEGROUP>")
            MANWriter.WriteLine("<PAGE id=""PIC0000"" src=""./image/1.png"" retitem=""M00"" retgroup=""TOP"" />")
            MANWriter.WriteLine("<PAGE id=""PIC0000"" src=""./image/2.png"" retitem=""M00"" retgroup=""TOP"" />")
            MANWriter.WriteLine("</PAGEGROUP>")
            MANWriter.WriteLine("</MANUAL>")
            MANWriter.WriteLine("")
        End Using

        If MsgBox("Game ressources saved! Close this window ?", MsgBoxStyle.YesNo, "Saved") = MsgBoxResult.Yes Then
            Close()
        End If
    End Sub

    Private Sub PSXDatacenterBrowser_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles PSXDatacenterBrowser.DocumentCompleted
        Try
            'Get the game infos
            Dim infoTable As HtmlElement = PSXDatacenterBrowser.Document.GetElementById("table4")
            Dim infoRows As HtmlElementCollection = PSXDatacenterBrowser.Document.GetElementsByTagName("tr")

            'Game Title
            If infoRows.Item(4).InnerText IsNot Nothing Then
                GameTitleTextBox.Text = infoRows.Item(4).InnerText.Split(New String() {"OFFICIAL TITLE "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Game ID
            If infoRows.Item(6).InnerText IsNot Nothing Then
                GameIDTextBox.Text = infoRows.Item(6).InnerText.Split(New String() {"SERIAL NUMBER(S) "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Region
            If infoRows.Item(7).InnerText IsNot Nothing Then
                Dim Region As String = infoRows.Item(7).InnerText.Split(New String() {"REGION "}, StringSplitOptions.RemoveEmptyEntries)(0)
                Select Case Region
                    Case "PAL"
                        RegionTextBox.Text = "E"
                    Case "NTSC-U"
                        RegionTextBox.Text = "U"
                    Case "NTSC-J"
                        RegionTextBox.Text = "J"
                End Select
            End If

            'Genre
            If infoRows.Item(8).InnerText IsNot Nothing Then
                GameGenreTextBox.Text = infoRows.Item(8).InnerText.Split(New String() {"GENRE / STYLE "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Developer
            If infoRows.Item(9).InnerText IsNot Nothing Then
                GameDeveloperTextBox.Text = infoRows.Item(9).InnerText.Split(New String() {"DEVELOPER "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Publisher
            If infoRows.Item(10).InnerText IsNot Nothing Then
                PublisherTextBox.Text = infoRows.Item(10).InnerText.Split(New String() {"PUBLISHER "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Release Date
            If infoRows.Item(11).InnerText IsNot Nothing Then
                GameReleaseDateTextBox.Text = infoRows.Item(11).InnerText.Split(New String() {"DATE RELEASED "}, StringSplitOptions.RemoveEmptyEntries)(0)
            End If

            'Publisher
            If infoRows.Item(10).InnerText IsNot Nothing Then
                Dim ReturnPublisherWebsite As String = infoRows.Item(10).InnerText.Split(New String() {"PUBLISHER "}, StringSplitOptions.RemoveEmptyEntries)(0)

                If ReturnPublisherWebsite.Contains("2K") Then
                    GameWebsiteTextBox.Text = "https://2k.com/"
                ElseIf ReturnPublisherWebsite.Contains("Activision") Then
                    GameWebsiteTextBox.Text = "https://www.activision.com/"
                ElseIf ReturnPublisherWebsite.Contains("Bandai") Then
                    GameWebsiteTextBox.Text = "http://www.bandai.com/"
                ElseIf ReturnPublisherWebsite.Contains("Capcom") Then
                    GameWebsiteTextBox.Text = "http://www.capcom.com/"
                ElseIf ReturnPublisherWebsite.Contains("Electronic Arts") Then
                    GameWebsiteTextBox.Text = "http://ea.com/"
                ElseIf ReturnPublisherWebsite.Contains("EA Sports") Then
                    GameWebsiteTextBox.Text = "https://www.easports.com/"
                ElseIf ReturnPublisherWebsite.Contains("Konami") Then
                    GameWebsiteTextBox.Text = "https://www.konami.com/"
                ElseIf ReturnPublisherWebsite.Contains("Rockstar Games") Then
                    GameWebsiteTextBox.Text = "https://www.rockstargames.com/"
                ElseIf ReturnPublisherWebsite.Contains("Sega") Then
                    GameWebsiteTextBox.Text = "http://sega.com/"
                ElseIf ReturnPublisherWebsite.Contains("Sony Computer Entertainment") Then
                    GameWebsiteTextBox.Text = "https://www.sie.com/en/index.html"
                ElseIf ReturnPublisherWebsite.Contains("THQ") Then
                    GameWebsiteTextBox.Text = "https://www.thqnordic.com/"
                ElseIf ReturnPublisherWebsite.Contains("Ubisoft") Then
                    GameWebsiteTextBox.Text = "https://www.ubisoft.com/"
                End If

            End If

            'Get the game cover
            If PSXDatacenterBrowser.Document.GetElementById("table2") IsNot Nothing Then
                CoverPictureBox.Source = New BitmapImage(New Uri(PSXDatacenterBrowser.Document.GetElementById("table2").GetElementsByTagName("img")(1).GetAttribute("src")))
                CoverPictureBox.Tag = PSXDatacenterBrowser.Document.GetElementById("table2").GetElementsByTagName("img")(1).GetAttribute("src")
            End If

            'Get some images
            If PSXDatacenterBrowser.Document.GetElementById("table22") IsNot Nothing Then
                BackgroundImagePictureBox.Source = New BitmapImage(New Uri(PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(0).GetAttribute("src")))
                BackgroundImagePictureBox.Tag = PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(0).GetAttribute("src")

                ScreenshotImage1PictureBox.Source = New BitmapImage(New Uri(PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(1).GetAttribute("src")))
                ScreenshotImage1PictureBox.Tag = PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(1).GetAttribute("src")

                ScreenshotImage2PictureBox.Source = New BitmapImage(New Uri(PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(2).GetAttribute("src")))
                ScreenshotImage2PictureBox.Tag = PSXDatacenterBrowser.Document.GetElementById("table22").GetElementsByTagName("img")(2).GetAttribute("src")
            End If

            If AutoSave = True Then
                SaveButton_Click(SaveButton, New RoutedEventArgs())
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub CoverPictureBox_MouseLeftButtonDown(sender As Object, e As MouseButtonEventArgs) Handles CoverPictureBox.MouseLeftButtonDown
        Dim OFD As New Forms.OpenFileDialog() With {.Title = "Choose your .png file.", .Filter = "png files (*.png)|*.png"}

        If OFD.ShowDialog() = Forms.DialogResult.OK Then
            CoverPictureBox.Source = New BitmapImage(New Uri(OFD.FileName))
            CoverPictureBox.Tag = OFD.FileName
        End If
    End Sub

End Class
