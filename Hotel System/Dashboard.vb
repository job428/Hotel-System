Imports MySql.Data.MySqlClient

Public Class Dashboard
    Private connectionString As String = "Server=localhost;Database=pms_database;User ID=root;Password=@168Moontech;"
    ' โค้ดสำหรับปุ่มปิด
    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    ' โค้ดสำหรับปุ่มซ่อน
    Private Sub btnMinimize_Click(sender As Object, e As EventArgs) Handles btnMinimize.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub


    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadRoomData()

        ' ปรับให้มุมโค้ง
        CustomizeButton(Button1, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button2, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button3, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button4, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button5, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button6, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button7, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button8, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง
        CustomizeButton(Button9, 50) ' เพิ่มตัวเลขตามต้องการเพื่อเพิ่มความโค้ง

    End Sub

    Private Sub LoadRoomData()
        Using connection As New MySqlConnection(connectionString)
            connection.Open()

            ' ดึงข้อมูลห้องพักทั้งหมดจากตาราง room
            Dim query As String = "
            SELECT room.room_ID, room.room_name, room_detail, room_keylock, room.room_price,
                   room.building_ID, room.floor_ID, room.roomtype_ID,
                   roomtype.roomType_name
            FROM room
            JOIN roomtype ON room.roomtype_ID = roomtype.roomtype_ID"

            Dim command As New MySqlCommand(query, connection)

            Using reader As MySqlDataReader = command.ExecuteReader()
                ' สร้าง Dictionary สำหรับเก็บข้อมูลห้องพักแยกตาม floor_ID
                Dim floors As New Dictionary(Of Integer, List(Of (roomName As String, roomID As Integer, roomTypeName As String)))

                ' อ่านข้อมูลห้องพักและจัดกลุ่มตาม floor_ID
                While reader.Read()
                    Dim floorID As Integer = reader.GetInt32("floor_ID")
                    Dim roomName As String = reader.GetString("room_name")
                    Dim roomID As Integer = reader.GetInt32("room_ID")
                    Dim roomTypeName As String = reader.GetString("roomType_name") ' ดึง roomType_name มาใช้
                    Dim roomStatus As String = "ว่าง"
                    ' จัดกลุ่มข้อมูลตาม floor_ID และเก็บข้อมูล roomType_name
                    If Not floors.ContainsKey(floorID) Then
                        floors(floorID) = New List(Of (String, Integer, String))()
                    End If
                    floors(floorID).Add((roomName, roomID, roomTypeName))
                End While

                ' ตำแหน่ง Y สำหรับ GroupBox แต่ละชั้น
                Dim groupBoxTop As Integer = 10

                ' สร้าง GroupBox สำหรับแต่ละ floor_ID และ Panel สำหรับแต่ละห้อง
                For Each floor In floors
                    Dim floorID As Integer = floor.Key
                    Dim groupBox As New GroupBox()
                    groupBox.Text = "Floor " & floorID
                    groupBox.Width = 600
                    groupBox.AutoSize = True
                    groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink
                    groupBox.Padding = New Padding(10)
                    groupBox.Name = "GroupBoxFloor" & floorID

                    ' จัดตำแหน่งของ GroupBox ภายใน Panel
                    groupBox.Top = groupBoxTop
                    groupBox.Left = 10

                    ' เพิ่ม GroupBox ลงใน Panel ที่ชื่อว่า panelContainer
                    panelContainer.Controls.Add(groupBox)

                    ' เรียงลำดับห้องตาม roomName ก่อนที่จะสร้าง UI
                    Dim sortedRooms = floor.Value.OrderBy(Function(r) CInt(r.roomName)).ToList()

                    ' สร้าง Panel และ Labels สำหรับแต่ละห้องใน floor_ID นั้นๆ
                    Dim xPosition As Integer = 10 ' ตำแหน่ง X สำหรับวาง Panel แต่ละอันภายใน GroupBox
                    Dim yPosition As Integer = 20 ' ตำแหน่ง Y เริ่มต้นของแถวแรก
                    Dim panelCount As Integer = 0 ' ตัวนับจำนวน Panel ในแถว

                    For Each room In sortedRooms
                        ' สร้าง Panel สำหรับห้อง
                        Dim roomPanel As New Panel()
                        roomPanel.Width = 140
                        roomPanel.Height = 80
                        roomPanel.BackColor = Color.LightGray
                        roomPanel.BorderStyle = BorderStyle.FixedSingle
                        roomPanel.Top = yPosition
                        roomPanel.Left = xPosition




                        ' Label สำหรับ roomName
                        Dim roomLabelName As New Label()
                        roomLabelName.Text = room.roomName
                        roomLabelName.Font = New Font(roomLabelName.Font.FontFamily, 12, FontStyle.Bold)
                        roomLabelName.AutoSize = True
                        roomLabelName.Top = 5
                        roomLabelName.Left = 5

                        ' สร้างแถบสีแบบไฟนีออน (neonPanel)
                        Dim neonPanel As New Panel()
                        neonPanel.Width = 80 ' กำหนดความกว้างของแถบไฟนีออน
                        neonPanel.Height = 5 ' ความสูงของแถบ
                        neonPanel.Top = 12 ' จัดตำแหน่งด้านบนของแถบใน roomPanel
                        neonPanel.Left = 50 ' ตำแหน่งทางขวาของหมายเลขห้อง
                        neonPanel.BackColor = Color.Lime ' สีไฟนีออน (สามารถเปลี่ยนสีได้)
                        neonPanel.Parent = roomPanel

                        ' เพิ่มแถบไฟนีออนลงใน Panel ของห้อง
                        roomPanel.Controls.Add(neonPanel)

                        ' Label สำหรับ roomTypeName
                        Dim roomLabelType As New Label()
                        roomLabelType.Text = "(" & room.roomTypeName & ")"
                        roomLabelType.Font = New Font(roomLabelType.Font.FontFamily, 8, FontStyle.Regular)
                        roomLabelType.AutoSize = True
                        roomLabelType.Top = roomLabelName.Top + roomLabelName.Height + 2
                        roomLabelType.Left = 5
                        ' Label สำหรับ roomStatus
                        Dim LabelroomStatus As New Label()
                        LabelroomStatus.Text = "ว่าง"
                        LabelroomStatus.Font = New Font(LabelroomStatus.Font.FontFamily, 8, FontStyle.Regular)
                        LabelroomStatus.AutoSize = True
                        LabelroomStatus.Top = roomLabelType.Top + roomLabelType.Height + 2
                        LabelroomStatus.Left = 5
                        ' เพิ่ม Labels ลงใน Panel
                        roomPanel.Controls.Add(roomLabelName)


                        roomPanel.Controls.Add(roomLabelType)
                        roomPanel.Controls.Add(LabelroomStatus)

                        ' เพิ่ม Event Handler ให้กับ Panel
                        AddHandler roomPanel.Click, AddressOf RoomPanel_Click
                        roomPanel.Tag = room.roomID

                        ' เพิ่ม Panel ลงใน GroupBox ของ floor_ID นั้นๆ
                        groupBox.Controls.Add(roomPanel)

                        ' ปรับตำแหน่ง X สำหรับ Panel ถัดไป
                        xPosition += roomPanel.Width + 10 ' เพิ่มระยะห่างระหว่าง Panel
                        panelCount += 1

                        ' ตรวจสอบว่าจำนวน Panel เกิน 5 หรือไม่ ถ้าเกินให้ขึ้นแถวใหม่
                        If panelCount >= 10 Then
                            xPosition = 10 ' รีเซ็ตตำแหน่ง X กลับไปที่จุดเริ่มต้น
                            yPosition += roomPanel.Height + 10 ' เพิ่มตำแหน่ง Y สำหรับแถวใหม่
                            panelCount = 0 ' รีเซ็ตตัวนับ Panel สำหรับแถวใหม่
                        End If
                    Next

                    ' ปรับตำแหน่ง Top ของ GroupBox ถัดไป โดยเพิ่มระยะห่าง
                    groupBoxTop += groupBox.Height + 20 ' เว้นระยะห่าง 20 พิกเซลระหว่าง GroupBox แต่ละอัน
                Next
            End Using
        End Using
    End Sub


    Private Sub RoomPanel_Click(sender As Object, e As EventArgs)
        Dim panel As Panel = CType(sender, Panel)

        ' ใช้ข้อมูลจาก panel.Tag ซึ่งเก็บ roomID หรือข้อมูลอื่นๆ ไว้
        Dim roomID As Integer = CInt(panel.Tag)

        ' MessageBox.Show("คุณคลิกที่ห้อง ID: " & roomID)
        clickroom.ShowDialog()
    End Sub
    ' Event Handler สำหรับ Button ของแต่ละห้อง

    Private Sub CustomizeButton(btn As Button, cornerRadius As Integer)
        Dim path As New Drawing2D.GraphicsPath()

        ' สร้างเส้นทางสำหรับมุมโค้งของปุ่ม
        path.AddArc(New Rectangle(0, 0, cornerRadius, cornerRadius), 180, 90) ' มุมซ้ายบน
        path.AddArc(New Rectangle(btn.Width - cornerRadius, 0, cornerRadius, cornerRadius), 270, 90) ' มุมขวาบน
        path.AddArc(New Rectangle(btn.Width - cornerRadius, btn.Height - cornerRadius, cornerRadius, cornerRadius), 0, 90) ' มุมขวาล่าง
        path.AddArc(New Rectangle(0, btn.Height - cornerRadius, cornerRadius, cornerRadius), 90, 90) ' มุมซ้ายล่าง

        path.CloseFigure()

        ' กำหนดให้ปุ่มใช้ GraphicsPath ที่สร้างขึ้น
        btn.Region = New Region(path)
    End Sub


End Class