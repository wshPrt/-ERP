
Imports LFERP.SystemManager
Imports LFERPDB
Imports System.Data.SqlClient
'Imports System.data

Module ShareData
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Int32, ByVal lpFileName As String) As Int32
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Int32


    Function WareSelect(ByVal UserName As String, ByVal ModuleNo As String) As String
        '查找用戶權限表, 找出此用戶擁有哪些倉庫的權限,並傳遞給函數
        Dim a As New PermissionModuleWarrantSubController
        Dim b As New List(Of PermissionModuleWarrantSubInfo)
        b = a.PermissionModuleWarrantSub_GetList(UserName, ModuleNo)

        Dim i, n As Integer
        Dim arr(n) As String

        WareSelect = ""
        arr = Split(b.Item(0).PMWS_Value, ",")
        n = Len(Replace(b.Item(0).PMWS_Value, ",", "," & "*")) - Len(b.Item(0).PMWS_Value)
        For i = 0 To n

            If WareSelect <> "" Then
                WareSelect = WareSelect & ",'" & arr(i) & "'"
            End If

            If WareSelect = "" Then
                WareSelect = "'" & arr(i) & "'"
            End If
        Next
    End Function

    Function WareInSelect(ByVal UserName As String, ByVal ModuleNo As String) As String
        '查找用戶權限表, 找出此用戶擁有哪些倉庫的權限,並傳遞給函數
        Dim a As New PermissionModuleWarrantSubController
        Dim b As New List(Of PermissionModuleWarrantSubInfo)
        b = a.PermissionModuleWarrantSub_GetList(UserName, ModuleNo)

        Dim i, n As Integer
        Dim arr(n) As String

        WareInSelect = ""
        arr = Split(b.Item(0).PMWS_Value, ",")
        n = Len(Replace(b.Item(0).PMWS_Value, ",", "," & "*")) - Len(b.Item(0).PMWS_Value)
        For i = 0 To n

            If i = 0 Then
                WareInSelect = "'" & Mid(arr(i), 1, 3) & "'"
            Else
                If InStr(WareInSelect, Mid(arr(i), 1, 3)) = False Then
                    WareInSelect = WareInSelect & ",'" & Mid(arr(i), 1, 3) & "'"
                End If
            End If
        Next
    End Function

    Public Sub CheckForm(ByVal MDIChildForm As Form, ByVal MDIChildFormName As String)
        If MDIMain.MdiChildren.Length < 1 Then
            '如果没有任何一个MDI子窗体，则创该MDI子窗体的窗体实例
            ShowForm(MDIChildForm)
            Exit Sub
        Else
            Dim x As Integer
            Dim frmyn As Boolean
            For x = 0 To (MDIMain.MdiChildren.Length) - 1

                Dim tempChild As Form = CType(MDIMain.MdiChildren(x), Form)
                If tempChild.Name = MDIChildFormName Then
                    frmyn = True
                    '检测到有该MDI子窗体，设为TRUE 并退出循环
                    Exit For
                Else
                    frmyn = False
                End If
            Next
            If frmyn = False Then
                '在打开的窗体中没检测到则新建
                ShowForm(MDIChildForm)
            Else
                '在打开的窗体中检测到则激活
                Dim MDIChildFrm As Form = CType(MDIMain.MdiChildren(x), Form)
                MDIChildFrm.Activate()
            End If
        End If
    End Sub

    Public Sub ShowForm(ByVal MDIChildForm As Form)
        Dim MDIChildFrm As Form = MDIChildForm
        MDIChildFrm.MdiParent = MDIMain ' 定义MDI子窗体
        MDIChildFrm.Show() '打开窗体
    End Sub

    Public Sub SetData(ByVal lbl As Label, ByVal str As String)
        lbl.Text = str
    End Sub

    '@ 2012/1/12 判斷輸入的是否是純數字
    '此函數被以下過程調用：
    'frmWareOutRptSelect.cmdSave_Click()
    'frmWareInventorySeek.cmdSave_Click()
    Function CheckIsNum(ByVal str As String, ByVal blnFirstIsZero As Boolean) As Boolean 'blnFirstIsZero決定第一個字符能否是0
        Dim i%
        If blnFirstIsZero = False Then
            If InStr(1, "123456789", Mid(str, 1, 1)) = 0 Then
                CheckIsNum = False
                Exit Function
            End If
        End If
        For i = 1 To Len(Trim(str))
            If InStr(1, "0123456789", Mid(str, i, 1)) = 0 Then
                CheckIsNum = False
                Exit Function
            Else
                CheckIsNum = True
            End If
        Next
    End Function

    '求兩個時間的數值﹐轉換為60分鐘的倍數(）
    Function CheckDateValue(ByVal Date1 As Date, ByVal Date2 As Date) As Single
        Dim i, X, Y, z As Long
        Dim l As Single

        i = Math.Abs(DateDiff("n", Date1, Date2))
        l = Math.Round(i / 60, 1)
        Y = 0
        For X = 1 To Len(l)
            If Mid(l, X, 1) = "." Then
                Y = X
                Exit For
            End If
        Next

        If Y = 0 Then
            CheckDateValue = l
        Else
            z = i - (CLng(Mid(l, 1, Y - 1) * 60))
            Select Case z
                Case 1 To 14
                    CheckDateValue = CLng(Mid(l, 1, Y - 1))
                Case 15 To 44
                    CheckDateValue = CLng(Mid(l, 1, Y - 1)) + 0.5
                Case 45 To 59
                    CheckDateValue = CLng(Mid(l, 1, Y - 1)) + 1
                Case Else
                    CheckDateValue = Y
            End Select
        End If

    End Function

    '檢查系統是否有更新
    Function CheckUpdate() As Boolean

        'Dim conn As New LFERPDataBase
        'Dim myConn As New SqlConnection(conn.AutoUpdateConnection)
        'Dim strSql As String = ""
        'Dim myCmd As SqlCommand

        'myConn.Open()

        'strSql = "select * from AutoUpdate where Pro_Name='LuenFungERP'"

        'myCmd = New SqlCommand(strSql, myConn)

        'Dim rd As SqlDataReader = myCmd.ExecuteReader

        ''strVer = "1.9.2.72"

        'Do While rd.Read()
        '    If Trim(rd("Pro_ID").ToString) <> strVer Then   '判斷系統版本號與數據庫版本號是否一致，不一致則調用更新程序
        '        'Shell(Application.StartupPath & "\AutoUpdate.exe", 1)
        '        System.Diagnostics.Process.Start(Application.StartupPath & "\AutoUpdate.exe")  '運行更新程序
        '    ElseIf Trim(rd("AutoUpdateVer")) <> GetSetting("AutoUpdateVer", "Version", "LuenFungERPVersionID", "") Then      '判斷自動更新程序在數據庫中的版本號與註冊表中的版本號是否一致,不一致則更新
        '        Shell("xcopy " & Trim(rd("ServerAddress")) & "\AutoUpdate.exe " & Application.StartupPath & "\ /e /c /y", 0)
        '        SaveSetting("AutoUpdateVer", "Version", "LuenFungERPVersionID", Trim(rd("AutoUpdateVer")))      '把更新後的版本號寫進註冊表
        '    End If
        'Loop

        'rd.Close()
        'myConn.Close()
    End Function



    Public Function DtToXlsOK(ByVal GridView As DevExpress.XtraGrid.Views.Grid.GridView, ByVal DefFileName As String, ByVal OfficeType As String, ByVal FiledName As String, ByVal CaptionName As String) As Boolean
        Dim MyOleDbCn As New System.Data.OleDb.OleDbConnection
        Dim MyOleDbCmd As New System.Data.OleDb.OleDbCommand
        'Dim MyTable As New DataTable
        Dim intRowsCnt, intColsCnt As Integer
        intColsCnt = 0
        Dim strSql As String, strFlName As String
        Dim Fso As New System.Object
        If GridView.RowCount <= 0 Then
            MessageBox.Show("未取得數據，無法導出 ", "導出錯誤 ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Function
        End If

        Dim FileName As String
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Title = "保存為 "
        ' SaveFileDialog.Filter = "txt files (*.xls))|*.xls"

        If OfficeType = "2007" Then
            ' SaveFileDialog.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx"
            SaveFileDialog.Filter = "(*.xls)|*.xls"

        Else
            SaveFileDialog.Filter = "(*.xlsx)|*.xlsx"


        End If

        SaveFileDialog.FileName = DefFileName
        If (SaveFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            FileName = SaveFileDialog.FileName
        Else
            Exit Function
            '   TODO:   在此加入開啟檔案的程式碼。 
        End If
        If FileName = " " Then Exit Function
        strFlName = FileName
        If Dir(FileName, FileAttribute.Directory) <> "" Then
            Kill(FileName)
        End If
        Try
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If OfficeType = "2007" Then
                MyOleDbCn.ConnectionString = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + strFlName + ";Extended Properties='Excel 8.0; HDR=Yes; '"    ''''; //此连接只能操作Excel2007之前(.xls)文件
            ElseIf OfficeType = "2010" Then
                MyOleDbCn.ConnectionString = "Provider=Microsoft.Ace.OleDb.12.0;" & "data source=" + strFlName & ";Extended Properties='Excel 12.0; HDR=Yes;'"
            End If

            MyOleDbCn.Open()
            MyOleDbCmd.Connection = MyOleDbCn
            MyOleDbCmd.CommandType = CommandType.Text

            Dim i, n As Integer
            Dim arr(n) As String
            arr = Split(CaptionName, ",")
            n = Len(Replace(CaptionName, ",", "," & "*")) - Len(CaptionName)

            '第一行插入列标题 
            strSql = "CREATE   TABLE   " & DefFileName & "( "
            For i = 0 To n - 1
                If i <> n - 1 Then
                    strSql = strSql & arr(i) & "   text, "
                Else
                    strSql = strSql & arr(i) & "   text) "
                End If
            Next

            MyOleDbCmd.CommandText = strSql
            MyOleDbCmd.ExecuteNonQuery()


            Dim j, m As Integer
            Dim arr1(m) As String
            arr1 = Split(FiledName, ",")
            m = Len(Replace(FiledName, ",", "," & "*")) - Len(FiledName)


            '插入各行 
            For intRowsCnt = 0 To GridView.RowCount - 1
                strSql = "INSERT   INTO   " & DefFileName & "   VALUES( ' "
                For j = 0 To m - 1
                    If j <> m - 1 Then
                        strSql = strSql & ChangeCharOK(GridView.GetRowCellValue(intRowsCnt, Trim(arr1(j)))) & " ', ' "
                    Else
                        strSql = strSql & ChangeCharOK(GridView.GetRowCellValue(intRowsCnt, Trim(arr(j)))) & " ') "
                    End If
                Next
                MyOleDbCmd.CommandText = strSql
                MyOleDbCmd.ExecuteNonQuery()
            Next
            MessageBox.Show("数据已经成功导入EXCEL文件 " & strFlName, "数据导出 ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ErrCode As Exception
            MsgBox("错误信息： " & ErrCode.Message & vbCrLf & vbCrLf & _
            "引发事件： " & ErrCode.TargetSite.ToString, MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "错误来源： " & ErrCode.Source)
            Exit Function
        Finally
            MyOleDbCmd.Dispose()
            MyOleDbCn.Close()
            MyOleDbCn.Dispose()
            Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try

    End Function

    Public Function ChangeCharOK(ByVal Sqlchar) As String

        If Convert.IsDBNull(Sqlchar) Then
            ChangeCharOK = "   "
            Exit Function
        End If
        Dim tStr As String
        tStr = Replace(Sqlchar, " ' ", Chr(39) + Chr(39))
        tStr = Replace(tStr, "| ", "_ ")
        ChangeCharOK = tStr
    End Function
    ''' <summary>
    ''' 在LCD顯示屏顯示內容
    ''' </summary>
    ''' <param name="str1">第一行字符串</param>
    ''' <param name="str2">第二行字符串</param>
    ''' <param name="str3">第三行字符串</param>
    ''' <param name="str4">第四行字符串</param>
    ''' <returns></returns>
    ''' <remarks>@ 2013/3/14 添加</remarks>
    Public Function LoadPingMU(ByVal str1 As String, ByVal str2 As String, ByVal str3 As String, ByVal str4 As String) As Boolean

        Dim intPort As Integer
        intPort = Val(GetIni("CommSet", "LCD"))

        If intPort = 0 Then
            Exit Function
        End If

        Dim sOut As String

        Dim DT As New DisplayText.class1
        Try
            If isOpenCOM = False Then
                'intPort = Mid(My.Settings.strCOM, 4)
                If ApiDisplay.com_init(intPort, 9600) = False Then
                    MsgBox("COM口打開失敗!", 64, "提示")
                    Exit Function
                End If
                isOpenCOM = True
            End If

            sOut = StrConv(str1, VbStrConv.SimplifiedChinese, 2052)
            DT.cmdSendnum1(sOut)
            sOut = StrConv(str2, VbStrConv.SimplifiedChinese, 2052)
            DT.cmdSendnum2(sOut)
            sOut = StrConv(str3, VbStrConv.SimplifiedChinese, 2052)
            DT.cmdSendnum3(sOut)
            sOut = StrConv(str4, VbStrConv.SimplifiedChinese, 2052)
            DT.cmdSendnum4(sOut)
        Catch ex As Exception
            MsgBox(ex.Message, 64, "提示")
        End Try
    End Function

    Public Function GetIni(ByVal _cStr As String, ByVal _iStr As String) As String

        Dim strIni As String
        strIni = New String("", 101)
        Dim oStr As String = ""

        If Dir(Application.StartupPath & "\CommSetting.ini", FileAttribute.Directory) = "" Then
            GetIni = ""
            Exit Function
        End If

        GetPrivateProfileString(_cStr, _iStr, "", strIni, 100, Application.StartupPath & "\CommSetting.ini")
        oStr = Microsoft.VisualBasic.Left(strIni, InStr(strIni, Chr(0)) - 1)

        Return oStr

    End Function
End Module
