Imports LFERP.Library.ProductionSchedule
Imports LFERP.Library.Product
Imports LFERP.DataSetting
Imports LFERP.Library.ProductProcess


Public Class frmProductionSchedule

    Dim ds As New DataSet
    Dim dc As New DepartmentControler
    Dim upi As List(Of UserPowerInfo)
    Dim upc As New UserPowerControl

    Sub LoadProductNo()
        'Dim mc As New ProductController
        Dim pc As New ProcessMainControl
        PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        PM_M_Code.Properties.ValueMember = "PM_M_Code"
        PM_M_Code.Properties.DataSource = pc.ProcessMain_GetList3(Nothing, Nothing)

    End Sub

    Sub LoadFacName()
        Dim fc As New FacControler
        Dim fi As New FacInfo

        GluDep.Properties.DataSource = fc.GetFacList(Nothing, Nothing)
        GluDep.Properties.DisplayMember = "FacName"
        GluDep.Properties.ValueMember = "FacID"

    End Sub

    Private Sub frmProductionSchedule_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer

        CreateTable()
        Label61.Text = tempValue2
        Label62.Text = tempValue3
        tempValue2 = ""
        tempValue3 = ""
        txtNO.Text = Label62.Text
        LoadProductNo()
        LoadFacName()

        Select Case Label61.Text

            Case "生產計劃"
                If Edit = False Then
                    DateEdit1.EditValue = Format(Now, "yyyy/MM/dd")
                    txtNO.Text = Nothing
                    txtNO.Enabled = False
                    Label8.Text = UserName

                    cbType.EditValue = "生產加工"

                    upi = upc.UserPower_GetList(InUserID, Nothing, Nothing, Nothing)

                    If upi.Count > 0 Then

                        GluDep.EditValue = Mid(upi(0).DepID, 1, 1)
                        GluDep.Enabled = False
                    Else
                        GluDep.Enabled = True

                    End If
                Else
                    LoadData(Label62.Text)
                    GluDep.Enabled = False
                    DateEdit1.Enabled = False
                    'CheckEdit2.Checked = True
                    CheckEdit3.Checked = False
                    CheckEdit3.Visible = False

                    Me.Text = "修改生產計劃" & "--" & Label62.Text

                End If
                XtraTabControl1.SelectedTabPage = XtraTabPage1
            Case "PreView"
                LoadData(Label62.Text)
                cmdSave.Visible = False
                Me.Text = "查看生產計劃" & "--" & Label62.Text
                XtraTabControl1.SelectedTabPage = XtraTabPage1
            Case "Check"
                LoadData(Label62.Text)
                Label5.Text = Format(Now, "yyyy/MM/dd")
                Me.Text = "審核生產計劃" & "--" & Label62.Text
                XtraTabControl1.SelectedTabPage = XtraTabPage2

                '@ 2012/11/21 添加 XtraTabPage1中的所有控件都不可編輯
                For i = 0 To XtraTabPage1.Controls.Count - 1
                    XtraTabPage1.Controls(i).Enabled = False
                Next

            Case "視圖"
                LoadData(Label62.Text)
                cmdSave.Visible = False
                Me.Text = "查看生產計劃" & "--" & Label62.Text
                XtraTabControl1.SelectedTabPage = XtraTabPage1
        End Select

    End Sub
    Sub CreateTable()
        ds.Tables.Clear()

        With ds.Tables.Add("ProductType")
            .Columns.Add("PM_Type", GetType(String))
        End With
        gluType.Properties.ValueMember = "PM_Type"
        gluType.Properties.DisplayMember = "PM_Type"
        gluType.Properties.DataSource = ds.Tables("ProductType")


        With ds.Tables.Add("Schedule")
            .Columns.Add("PS_Num", GetType(String))
            .Columns.Add("PS_Date", GetType(Date))
            .Columns.Add("PS_DayNumber", GetType(Integer))
            .Columns.Add("PS_ActualNumber", GetType(Integer))
        End With


    End Sub


    '載入當前項目號信息
    Function LoadData(ByVal PS_NO As String) As Boolean
        LoadData = True

        Dim psi As List(Of ProductionScheduleInfo)
        Dim psc As New ProductionScheduleControl

        psi = psc.ProductionSchedule_GetList(PS_NO, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        Try
            If psi.Count = 0 Then
                MsgBox("沒有數據")
                LoadData = False
                Exit Function
            Else
                Dim i As Integer
                For i = 0 To psi.Count - 1

                    txtNO.Text = psi(i).PS_NO
                    Label11.Text = psi(i).PS_Num
                    DateEdit1.EditValue = Format(psi(i).PS_Date, "yyyy/MM/dd")
                    Label8.Text = psi(0).ActionName
                    cbType.EditValue = psi(i).Pro_Type
                    GluDep.EditValue = psi(i).PS_Dep
                    PM_M_Code.EditValue = psi(i).PM_M_Code

                    PM_M_Code_EditValueChanged(Nothing, Nothing)

                    gluType.EditValue = psi(i).PM_Type
                    txtQty.Text = psi(i).PS_DayNumber
                    txtRemark.Text = psi(i).PS_Remark


                    If psi(i).PS_Check = True Then
                        CheckEdit1.Checked = True
                    Else
                        CheckEdit1.Checked = False
                    End If
                    Label5.Text = Format(psi(i).ps_checkDate, "yyyy/MM/dd HH:mm:ss")
                    txtCheckAction.Text = psi(i).CheckActionName
                    txtCheckRemark.Text = psi(i).PS_CheckRemark

                Next

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


    End Function

    '獲取最新項目號
    Public Function GetNO() As String
        Dim psi As New ProductionScheduleInfo
        Dim psc As New ProductionScheduleControl
        Dim strName As String
        strName = Format(Now, "yyMM")
        psi = psc.ProductionSchedule_GetNO(strName)
        If psi Is Nothing Then
            GetNO = "PS" + strName + "0001"
        Else
            GetNO = "PS" + strName + Mid((CInt(Mid(psi.PS_NO, 7)) + 10001), 2)
        End If

    End Function

    '獲取最新日期項目流水號
    Public Function GetNum() As String
        Dim psi As New ProductionScheduleInfo
        Dim psc As New ProductionScheduleControl
        Dim strName As String
        strName = "P" + Format(Now, "yyMM")
        psi = psc.ProductionSchedule_GetNum(strName)
        If psi Is Nothing Then
            GetNum = strName + "00001"
        Else
            GetNum = strName + Mid((CInt(Mid(psi.PS_Num, 6)) + 100001), 2)
        End If
    End Function

    '@ 2012/1/6 1.添加判斷為空時，相應控件獲得焦點;
    '           2.改為只用一個復選框進行選擇判斷
    Sub DataNew()

        Dim pi As New ProductionScheduleInfo
        Dim psc As New ProductionScheduleControl

        If GluDep.EditValue = "" Then
            MsgBox("生產部門不能為空!", 64, "提示")
            GluDep.Focus()
            Exit Sub
        End If

        If cbType.EditValue = "" Then
            MsgBox("工藝類型不能為空!", 64, "提示")
            cbType.Focus()
            Exit Sub
        End If

        If PM_M_Code.EditValue = "" Then
            MsgBox("產品編號不能為空!", 64, "提示")
            PM_M_Code.Focus()
            Exit Sub
        End If
    
        If gluType.EditValue = "" Then
            MsgBox("類型不能為空!", 64, "提示")
            gluType.Focus()
            Exit Sub
        End If

        If CheckEdit3.Checked = False Then
            txtNO.Text = GetNO()
            pi.PS_NO = txtNO.Text
            pi.Pro_Type = cbType.EditValue
            pi.PM_M_Code = PM_M_Code.EditValue
            pi.PM_Type = gluType.EditValue
            pi.M_Code = GetCode(cbType.EditValue, PM_M_Code.EditValue, gluType.EditValue)
            pi.PS_KaiLiao = False
            pi.PS_Detail = "備料中"
            pi.PS_Action = InUserID
            pi.PS_Dep = GluDep.EditValue
            pi.PS_Remark = txtRemark.Text
            pi.PS_AddDate = Format(Now, "yyyy/MM/dd")

            pi.PS_Num = GetNum()
            pi.PS_DayNumber = txtQty.Text
            pi.PS_Date = DateEdit1.Text

            'If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, GluDep.EditValue, PM_M_Code.EditValue, gluType.EditValue, DateEdit1.EditValue, DateEdit1.EditValue, Nothing).Count > 0 Then
            If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, Nothing, PM_M_Code.EditValue, gluType.EditValue, DateEdit1.EditValue, DateEdit1.EditValue, Nothing).Count > 0 Then
                ' MsgBox("該生產部在當前日期已存在此工藝類型,產品編碼下類型!")
                MsgBox("該生產部或其它生產部在當前日期已存在此工藝類型,產品編碼下類型!", 64, "提示")
                Exit Sub
            End If
            If psc.ProductionSchedule_Add(pi) = True Then

                MsgBox("添加生產計劃成功!", 64, "提示")

            Else
                MsgBox("添加失敗,請檢查原因!", 64, "提示")
                Exit Sub
            End If

        Else

            If CheckData() = False Then
                Exit Sub
            End If

            Dim strDate1 As DateTime
            Dim strDate2 As DateTime

            strDate1 = DateTime.Parse(DateEdit1.Text)
            strDate2 = DateTime.Parse(DateEdit2.Text)

            Dim ts As New TimeSpan
            ts = strDate2 - strDate1

            'If ts.Days > 7 Then
            '    MsgBox("當前最多只能建立7天的生產計劃！")
            '    Exit Sub
            'End If

            Dim i As Integer

            For i = 0 To ts.Days

                txtNO.Text = GetNO()
                pi.PS_NO = txtNO.Text
                pi.Pro_Type = cbType.EditValue
                pi.PM_M_Code = PM_M_Code.EditValue
                pi.PM_Type = gluType.EditValue
                pi.M_Code = GetCode(cbType.EditValue, PM_M_Code.EditValue, gluType.EditValue)
                pi.PS_KaiLiao = False
                pi.PS_Detail = "備料中"
                pi.PS_Action = InUserID
                pi.PS_Dep = GluDep.EditValue
                pi.PS_Remark = txtRemark.Text
                pi.PS_AddDate = Format(Now, "yyyy/MM/dd")

                pi.PS_Num = GetNum()
                pi.PS_DayNumber = txtQty.Text
                pi.PS_Date = DateAdd(DateInterval.Day, +i, CDate(DateEdit1.Text))

                'If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, GluDep.EditValue, PM_M_Code.EditValue, gluType.EditValue, pi.PS_Date, pi.PS_Date, Nothing).Count > 0 Then
                '    MsgBox("該生產部在當前日期已存在此工藝類型,產品編碼下類型!")
                '    Exit Sub
                'End If
                psc.ProductionSchedule_Add(pi)

            Next

            MsgBox("完成建立當前生產計劃!", 64, "提示")

        End If

        Me.Close()
    End Sub

    '@ 2012/1/6 添加判斷為空時，相應控件獲得焦點
    Sub DataEdit()

        Dim pi As New ProductionScheduleInfo
        Dim psc As New ProductionScheduleControl

        If cbType.EditValue = "" Then
            MsgBox("工藝類型不能為空!", 64, "提示")
            cbType.Focus()
            Exit Sub
        End If
        If GluDep.EditValue = "" Then
            MsgBox("生產部門不能為空!", 64, "提示")
            GluDep.Focus()
            Exit Sub
        End If
        If PM_M_Code.EditValue = "" Then
            MsgBox("產品編號不能為空!", 64, "提示")
            PM_M_Code.Focus()
            Exit Sub
        End If

        If gluType.EditValue = "" Then
            MsgBox("類型不能為空!", 64, "提示")
            gluType.Focus()
            Exit Sub
        End If

        pi.PS_NO = txtNO.Text
        pi.Pro_Type = cbType.EditValue
        pi.PM_M_Code = PM_M_Code.EditValue
        pi.PM_Type = gluType.EditValue
        pi.M_Code = GetCode(cbType.EditValue, PM_M_Code.EditValue, gluType.EditValue)
        pi.PS_KaiLiao = False
        pi.PS_Detail = "備料中"
        pi.PS_Action = InUserID
        pi.PS_Dep = GluDep.EditValue
        pi.PS_Remark = txtRemark.Text
        pi.PS_AddDate = Format(Now, "yyyy/MM/dd")

        pi.PS_Num = Label11.Text
        pi.PS_DayNumber = txtQty.Text
        pi.PS_Date = DateEdit1.EditValue

        'If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, GluDep.EditValue, PM_M_Code.EditValue, gluType.EditValue, DateEdit1.EditValue, DateEdit1.EditValue, Nothing).Count > 0 Then
        '    MsgBox("該生產部在當前日期已存在此工藝類型,產品編碼下類型!")
        '    Exit Sub
        'End If

        If psc.ProductionSchedule_Update(pi) = True Then

            MsgBox("修改生產計劃成功!")

        Else
            MsgBox("修改失敗,請檢查原因!")
            Exit Sub
        End If
        Me.Close()
    End Sub

    Sub UpdateCheck()

        Dim pi As New ProductionScheduleInfo
        Dim pc As New ProductionScheduleControl

        pi.PS_NO = txtNO.Text
        pi.PS_Check = CheckEdit1.Checked
        pi.PS_CheckDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
        pi.PS_CheckAction = InUserID
        pi.PS_CheckRemark = txtCheckRemark.Text
        If pc.ProductionSchedule_UpdateCheck(pi) = True Then
            MsgBox("審核成功!", 64, "提示")
        Else
            MsgBox("審核失敗,請檢查原因!", 64, "提示")
        End If
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click

        Select Case Label61.Text
            Case "生產計劃"
                If Edit = False Then
                    DataNew()
                Else
                    DataEdit()
                End If
            Case "Check"
                UpdateCheck()
        End Select

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    'Private Sub txtQty_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyDown

    'If e.KeyCode = Keys.Enter Then
    '    Dim m As New System.Text.RegularExpressions.Regex("^[0-9]*$")

    '    If m.IsMatch(txtQty.Text) = True Then

    '    Else

    '        txtQty.Text = Nothing
    '        Exit Sub
    '    End If

    'End If


    'End Sub

    Public Function GetCode(ByVal Pro_Type As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As String

        Dim ppc As New ProcessMainControl
        Dim ppi As List(Of ProcessMainInfo)
        ppi = ppc.ProcessMain_GetList(Nothing, PM_M_Code, Pro_Type, PM_Type, Nothing, Nothing)
        If ppi.Count = 0 Then
            GetCode = Nothing
            Exit Function
        Else
            GetCode = ppi(0).M_Code
        End If

    End Function

    Private Sub PM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PM_M_Code.EditValueChanged
        On Error Resume Next

        Dim ppc As New ProcessMainControl
        Dim ppi As List(Of ProcessMainInfo)
        ds.Tables("ProductType").Clear()
        ppi = ppc.ProcessMain_GetList2(cbType.EditValue, PM_M_Code.EditValue)
        If ppi.Count = 0 Then
        Else

            Dim i As Integer
            For i = 0 To ppi.Count - 1
                Dim row As DataRow
                row = ds.Tables("ProductType").NewRow
                row("PM_Type") = ppi(i).Type3ID
                ds.Tables("ProductType").Rows.Add(row)
            Next

        End If

    End Sub

    ''@ 2012/1/6 修改為用正則表達式判斷輸入的是否是數字
    'Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
    '    Dim m As New System.Text.RegularExpressions.Regex("^[1-9]+\d*") '只能輸入正整數，且第一個數字不能為0
    '    If m.IsMatch(txtQty.Text) = False Then
    '        txtQty.Text = Nothing
    '    End If
    'End Sub

    Function CheckData() As Boolean
        CheckData = True

        Dim strDate1 As DateTime
        Dim strDate2 As DateTime
        Dim psc As New ProductionScheduleControl


        strDate1 = DateTime.Parse(DateEdit1.Text)
        strDate2 = DateTime.Parse(DateEdit2.Text)

        Dim ts As New TimeSpan
        ts = strDate2 - strDate1
        If ts.Days <= 0 Then
            MsgBox("輸入日期範圍錯誤，請按照順序添加生產計劃！", 64, "提示")
            CheckData = False
            Exit Function
        End If

        If ts.Days > 7 Then
            MsgBox("當前最多只能建立7天的生產計劃！", 64, "提示")
            CheckData = False
            Exit Function
        End If

        Dim i As Integer
        For i = 0 To ts.Days
            ' If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, GluDep.EditValue, PM_M_Code.EditValue, gluType.EditValue, DateAdd(DateInterval.Day, +i, CDate(DateEdit1.Text)), DateAdd(DateInterval.Day, +i, CDate(DateEdit1.Text)), Nothing).Count > 0 Then
            If psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, Nothing, PM_M_Code.EditValue, gluType.EditValue, DateAdd(DateInterval.Day, +i, CDate(DateEdit1.Text)), DateAdd(DateInterval.Day, +i, CDate(DateEdit1.Text)), Nothing).Count > 0 Then
                MsgBox("該生產部或其它生產部在當前日期已存在此工藝類型,產品編碼下類型!", 64, "提示")
                CheckData = False
                Exit Function
            End If
        Next

    End Function
    '@ 2012/1/6 改為只用一個復選框進行選擇判斷
    'Private Sub CheckEdit2_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
    '    If CheckEdit2.Checked = True Then
    '        CheckEdit3.Checked = False
    '        Label9.Visible = False
    '        DateEdit2.Visible = False

    '        DateEdit2.Text = Nothing
    '    End If
    'End Sub

    '@ 2012/1/6 增加else判斷
    Private Sub CheckEdit3_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit3.CheckedChanged
        If CheckEdit3.Checked = True Then
            'CheckEdit2.Checked = False
            Label9.Visible = True
            DateEdit2.Visible = True

            DateEdit2.Text = Format(Now, "yyyy/MM/dd")
        Else
            Label9.Visible = False
            DateEdit2.Visible = False
        End If
    End Sub

    '@ 2012/1/6 添加，當控件內容發生改變，且PM_M_Code控件內容不為空時，加載相應的內容到gluType控件
    '此過程調用以下過程：
    'PM_M_Code_EditValueChanged()
    Private Sub cbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbType.SelectedIndexChanged
        If PM_M_Code.Text <> "" Then
            PM_M_Code_EditValueChanged(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtQty_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtQty.EditValueChanged

    End Sub
End Class