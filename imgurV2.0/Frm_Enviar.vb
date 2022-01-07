Imports System.Text
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Security.Principal

Public Class Frm_Enviar

    Dim ClientId As String = "66665db7b4b0608"

    Private Sub txtFilePath_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtFilePath.DragDrop
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim MyFiles() As String
            MyFiles = e.Data.GetData(DataFormats.FileDrop)
            txtFilePath.Text = MyFiles(0)
        End If
    End Sub

    Private Sub txtFilePath_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles txtFilePath.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.All
        End If
    End Sub

    Public Function UploadImage(ByVal image As String)
        Dim w As New WebClient()
        w.Headers.Add("Authorization", "Client-ID " & ClientId)
        Dim Keys As New System.Collections.Specialized.NameValueCollection
        Try
            Keys.Add("image", Convert.ToBase64String(File.ReadAllBytes(image)))
            Dim responseArray As Byte() = w.UploadValues("https://api.imgur.com/3/image", Keys)
            Dim result = Encoding.ASCII.GetString(responseArray)
            Dim reg As New System.Text.RegularExpressions.Regex("link"":""(.*?)""")
            Dim match As Match = reg.Match(result)
            Dim url As String = match.ToString.Replace("link"":""", "").Replace("""", "").Replace("\/", "/")
            Return url
        Catch s As Exception
            MessageBox.Show("Error!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return "Failed!"
        End Try
    End Function

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        Try
            If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                txtFilePath.Text = OpenFileDialog1.FileName
                PictureBox1.Image = Image.FromFile(OpenFileDialog1.FileName)
            Else
                txtFilePath.Text = Nothing
                PictureBox1.Image = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub

    Private Sub btnUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        trueup = False
        Timer1.Start()
    End Sub

    Dim trueup As Boolean = False
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If ProgressBar1.Value >= 100 Then
            Timer1.Stop()
            ProgressBar1.SendToBack()
            btnBrowse.Enabled = True
            btnUpload.Enabled = True
            btnCopy.Enabled = True
            btnCopy2.Enabled = True
            btnCopy3.Enabled = True
            ProgressBar1.Value = 0
        ElseIf ProgressBar1.Value = 50 Then
            If trueup = False Then
                trueup = True
                Dim url As String = UploadImage(txtFilePath.Text)
                If url.Contains("imgur") Then
                    txtHtmlCode.Text = "<a href=""https://imgur.com/""><img src""" & url & """></img></a>"
                    txtBBCode.Text = "[url=https://imgur.com/][img]" & url & "[/img][/url]"
                    txtDirectUrl.Text = url
                    MessageBox.Show("Imagem Enviada !", "Informações", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            ProgressBar1.Value += ++1
        Else
            btnBrowse.Enabled = False
            btnUpload.Enabled = False
            btnCopy.Enabled = False
            btnCopy2.Enabled = False
            btnCopy3.Enabled = False
            ProgressBar1.BringToFront()
            ProgressBar1.Value += ++1
        End If
    End Sub

    Private Sub btnCopy_Click(sender As Object, e As EventArgs) Handles btnCopy.Click
        Clipboard.SetText(txtHtmlCode.Text)
        MessageBox.Show("URL Copiada", "Informações", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCopy2_Click(sender As Object, e As EventArgs) Handles btnCopy2.Click
        Clipboard.SetText(txtBBCode.Text)
        MessageBox.Show("URL Copiada", "Informações", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCopy3_Click(sender As Object, e As EventArgs) Handles btnCopy3.Click
        Clipboard.SetText(txtDirectUrl.Text)
        MessageBox.Show("URL Copiada", "Informações", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub txtHtmlCode_Click(sender As Object, e As EventArgs) Handles txtHtmlCode.Click
        txtHtmlCode.SelectAll()
    End Sub

    Private Sub txtBBCode_Click(sender As Object, e As EventArgs) Handles txtBBCode.Click
        txtBBCode.SelectAll()
    End Sub

    Private Sub txtDirectUrl_Click(sender As Object, e As EventArgs) Handles txtDirectUrl.Click
        txtDirectUrl.SelectAll()
    End Sub

    Dim adel2, rehan As Integer
    Dim newp As System.Drawing.Point

    Private Sub PictureBox1_mousedown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        adel2 = MousePosition.X - Me.Location.X
        rehan = MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub PictureBox1_mousemove(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newp = MousePosition
            newp.X = newp.X - adel2
            newp.Y = newp.Y - rehan
            Me.Location = newp
        End If
    End Sub

    Private Sub Frm_Enviar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnCopy.Enabled = False
        btnCopy2.Enabled = False
        btnCopy3.Enabled = False
        btnBrowse.PerformClick()
        If OpenFileDialog1.FileName = Nothing Then
        Else
            btnUpload.PerformClick()
        End If
    End Sub
End Class
