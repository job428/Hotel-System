Imports System.Data.SqlClient
Imports MySql.Data.MySqlClient

Public Class Login
    ' เชื่อมต่อฐานข้อมูล MySQL
    Private connectionString As String = "Server=localhost;Database=pms_database;User ID=root;Password=@168Moontech;"

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text

        ' เรียกใช้ฟังก์ชันเพื่อเชื่อมต่อฐานข้อมูลและตรวจสอบข้อมูลผู้ใช้
        If AuthenticateUser(username, password) Then
            ' เปิดหน้า Dashboard
            Dim dashboard As New Dashboard()
            dashboard.Show()

            ' ซ่อนฟอร์มล็อกอิน
            Me.Hide()
            ' เขียนโค้ดเพื่อเข้าสู่ระบบ หรือเปิดฟอร์มหลัก
        Else
            MessageBox.Show("Invalid username or password.")
        End If
    End Sub

    Private Function AuthenticateUser(username As String, password As String) As Boolean
        Using connection As New MySqlConnection(connectionString)
            Try
                connection.Open()
                Dim query As String = "SELECT password FROM users WHERE username = @username"
                Using cmd As New MySqlCommand(query, connection)
                    cmd.Parameters.AddWithValue("@username", username)
                    Dim dbPassword As String = Convert.ToString(cmd.ExecuteScalar())

                    ' เช็คว่ารหัสผ่านตรงกับในฐานข้อมูลหรือไม่
                    If dbPassword <> "" AndAlso BCrypt.Net.BCrypt.Verify(password, dbPassword) Then
                        Return True
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return False
    End Function

    ' โค้ดสำหรับปุ่มปิด
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' โค้ดสำหรับปุ่มซ่อน
    Private Sub btnMinimize_Click(sender As Object, e As EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

    End Sub
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ตั้งค่าคำอธิบายเริ่มต้น
        SetPlaceholder(txtUsername, "Username")
        SetPlaceholder(txtPassword, "Password")


    End Sub

    ' ฟังก์ชันสำหรับตั้งคำอธิบายใน TextBox
    Private Sub SetPlaceholder(txt As TextBox, placeholder As String)
        txt.Text = placeholder
        txt.ForeColor = Color.Gray ' สีข้อความอธิบาย
    End Sub

    ' ลบคำอธิบายเมื่อผู้ใช้คลิกที่ TextBox
    Private Sub txtUsername_Enter(sender As Object, e As EventArgs) Handles txtUsername.Enter
        RemovePlaceholder(txtUsername, "Username")
    End Sub

    Private Sub txtPAssword_Enter(sender As Object, e As EventArgs) Handles txtPassword.Enter
        RemovePlaceholder(txtPassword, "Password")
    End Sub

    ' คืนคำอธิบายถ้าผู้ใช้ไม่ได้กรอกข้อมูลและออกจาก TextBox
    Private Sub txtUsername_Leave(sender As Object, e As EventArgs) Handles txtUsername.Leave
        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            SetPlaceholder(txtUsername, "Username")
        End If
    End Sub

    Private Sub txtPAssword_Leave(sender As Object, e As EventArgs) Handles txtPassword.Leave
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            SetPlaceholder(txtPassword, "Password")
        End If
    End Sub

    ' ฟังก์ชันสำหรับลบคำอธิบายเมื่อผู้ใช้คลิก
    Private Sub RemovePlaceholder(txt As TextBox, placeholder As String)
        If txt.Text = placeholder Then
            txt.Text = ""
            txt.ForeColor = Color.Black ' เปลี่ยนสีข้อความเป็นสีปกติ
        End If
    End Sub


End Class