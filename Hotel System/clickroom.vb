Public Class clickroom

    ' โค้ดสำหรับปุ่มปิด
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub





    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
        check_InForm.Show()
        Dashboard.Hide()
    End Sub
End Class