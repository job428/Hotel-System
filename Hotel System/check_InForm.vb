Imports ThaiNationalIDCard
Imports System.IO
Imports System.Drawing
Imports ThaiNationalIDCard.NET.Models
Imports ThaiNationalIDCard.NET
Imports System.Text.RegularExpressions


Public Class check_InForm

    ' โค้ดสำหรับปุ่มปิด
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' โค้ดสำหรับปุ่มซ่อน
    Private Sub btnMinimize_Click(sender As Object, e As EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs) Handles Label6.Click

    End Sub

    Private Sub ButtonReadCard_Click(sender As Object, e As EventArgs) Handles ButtonReadCard.Click
        Try
            ' สร้าง instance ของ ThaiNationalIDCardReader
            Dim cardReader As New ThaiNationalIDCardReader()

            ' อ่านข้อมูลทั้งหมดจากบัตร รวมถึงรูปภาพ
            Dim personalPhoto As PersonalPhoto = cardReader.GetPersonalPhoto()

            ' ตรวจสอบว่าข้อมูลบัตรมีอยู่หรือไม่
            If personalPhoto IsNot Nothing Then
                ' แสดงข้อมูลลงใน TextBox
                txtCitizenID.Text = personalPhoto.CitizenID
                ' อ่านข้อมูล Prefix จากบัตรประชาชนและตั้งค่า ComboBoxPrefix
                Dim prefix As String = personalPhoto.ThaiPersonalInfo.Prefix

                If (prefix = "น.ส.") Then
                    prefix = "นางสาว"
                End If
                ' ตรวจสอบและเลือก Prefix ที่ตรงกันใน ComboBoxPrefix
                For Each item As String In ComboBoxPrefix.Items
                        If item.Equals(prefix, StringComparison.OrdinalIgnoreCase) Then
                            ComboBoxPrefix.SelectedItem = item
                            Exit For
                        End If
                    Next

                    ' อ่านข้อมูลเพศจากบัตรประชาชน
                    Dim sex As String = personalPhoto.Sex

                    ' แปลงค่าเพศจากบัตรเป็นข้อความที่ตรงกับ ComboBox
                    Dim sexText As String = ""
                    If sex = "1" Then
                        sexText = "ชาย"
                    ElseIf sex = "2" Then
                        sexText = "หญิง"
                    End If

                    ' ตรวจสอบและเลือกค่าที่ตรงกันใน ComboBoxSex
                    For Each item As String In ComboBoxSex.Items
                        If item.Equals(sexText, StringComparison.OrdinalIgnoreCase) Then
                            ComboBoxSex.SelectedItem = item
                            Exit For
                        End If
                    Next

                    ' ดึงข้อมูลชื่อ (ภาษาไทย)
                    txtName.Text = personalPhoto.ThaiPersonalInfo.Prefix & " " & personalPhoto.ThaiPersonalInfo.FirstName
                    txtSurname.Text = personalPhoto.ThaiPersonalInfo.LastName
                ' ดึงข้อมูลที่อยู่
                txtAddress.Text = personalPhoto.AddressInfo.HouseNo & " " & personalPhoto.AddressInfo.VillageNo

                txtCardSubdistrict.Text = RemovePrefixesUsingRegex(personalPhoto.AddressInfo.SubDistrict)
                txtCarddistrict.Text = RemovePrefixesUsingRegex(personalPhoto.AddressInfo.District)
                txtCardProvince.Text = RemovePrefixesUsingRegex(personalPhoto.AddressInfo.Province)

                ' ดึงข้อมูล Base64 String ของรูปภาพ
                Dim base64Photo As String = personalPhoto.Photo

                    ' ลบ prefix 'data:image/jpeg;base64,' ออก (ถ้ามี)
                    If base64Photo.StartsWith("data:image/jpeg;base64,") Then
                        base64Photo = base64Photo.Replace("data:image/jpeg;base64,", "")
                    End If

                    ' แปลง Base64 String เป็น Byte Array
                    Dim photoBytes As Byte() = Convert.FromBase64String(base64Photo)

                    ' แปลง Byte Array เป็น Image และแสดงใน PictureBox
                    Using ms As New MemoryStream(photoBytes)
                        'pictureBoxPhoto.Image = Image.FromStream(ms)
                    End Using
                Else
                    MessageBox.Show("ไม่สามารถอ่านข้อมูลจากบัตรได้")
            End If

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        End Try
    End Sub
    Function RemovePrefixesUsingRegex(address As String) As String
        ' ใช้ Regular Expression เพื่อลบคำ "ตำบล", "อำเภอ", และ "จังหวัด"
        Dim pattern As String = "(ตำบล|อำเภอ|จังหวัด)"
        address = Regex.Replace(address, pattern, "")
        address = address.Trim() ' ตัดช่องว่างที่อาจเหลืออยู่
        Return address
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
        Dashboard.Show()

    End Sub
End Class