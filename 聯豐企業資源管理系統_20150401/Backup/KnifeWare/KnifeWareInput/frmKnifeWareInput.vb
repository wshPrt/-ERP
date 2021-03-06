Imports LFERP.Library.WareHouse.WareInput
Imports LFERP.Library
Imports LFERP.Library.Purchase.SharePurchase
Imports LFERP.FileManager
Imports LFERP.SystemManager
Imports LFERP.Library.WareHouse
Imports System.Threading
Imports LFERP.Library.Shared
Imports LFERP.Library.Product
Imports LFERP.DataSetting
Imports LFERP.Library.KnifeWare

Class frmKnifeWareInput
#Region "字段屬性"
    Dim ds As New DataSet
    Dim OldCheck As Boolean
    Dim strWHID As String
    Dim strDPTID As String
    Private _EditItem As String '屬性欄位
    Private _EditID As String
    Private _NodeTag As String
    Private _NodeText As String
    Property EditItem() As String '屬性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
    Property EditID() As String '屬性
        Get
            Return _EditID
        End Get
        Set(ByVal value As String)
            _EditID = value
        End Set
    End Property
    Property NodeTag() As String '屬性
        Get
            Return _NodeTag
        End Get
        Set(ByVal value As String)
            _NodeTag = value
        End Set
    End Property
    Property NodeText() As String '屬性
        Get
            Return _NodeText
        End Get
        Set(ByVal value As String)
            _NodeText = value
        End Set
    End Property
#End Region

    Sub CreateTables()
        ds.Tables.Clear()
        With ds.Tables.Add("KnifeWareInput")
            .Columns.Add("WIP_NUM", GetType(String))
            .Columns.Add("WIP_ID", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("M_Gauge", GetType(String))
            .Columns.Add("M_Unit", GetType(String))
            .Columns.Add("WIP_Qty", GetType(Int32))
            .Columns.Add("OS_BatchID", GetType(String))
            .Columns.Add("WIP_Remark", GetType(String))
            .Columns.Add("KnifeType", GetType(String))
        End With

        With ds.Tables.Add("DelDate")
            .Columns.Add("WIP_NUM", GetType(String))
            .Columns.Add("WIP_ID", GetType(String))
            .Columns.Add("M_Code", GetType(String))
        End With
        Grid.DataSource = ds.Tables("KnifeWareInput")
    End Sub

    Private Sub frmKnifeWareInput_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTables()
        txtWIPID.EditValue = EditID
        tempValue = ""
        CreateTables()
        txtWIPID.Enabled = False
        DateEdit1.Enabled = False

        '重置新刷卡機
        Dim reset As New ResetPassWords.SetPassWords
        reset.SetPassWords()

        Select Case EditItem
            Case "popWareInputAdd"
                If Edit = True Then
                    Me.Text = "刀具入庫--修改"
                    loadedit(txtWIPID.EditValue)
                ElseIf Edit = False Then
                    Me.Text = "刀具入庫--新增"
                    txtWIPID.EditValue = ""
                    DateEdit1.DateTime = Now
                    strWHID = NodeTag
                    txtWH.EditValue = NodeText
                    ' CheckEdit1.Checked = True '默認為已審核狀況(添加時直接保存--審核)
                    cbKnifeType.Enabled = False
                    cbKnifeType.Text = "新刀"
                    cbType.EditValue = "正常"
                End If
                XtraTabControl1.SelectedTabPage = XtraTabPage1
                SetObjectEnable(True, False)
            Case "popWareInputAddKnife"
                If Edit = True Then
                    Me.Text = "刀具入庫--修改"
                    loadedit(txtWIPID.EditValue)
                ElseIf Edit = False Then
                    Me.Text = "刀具入庫--新增"
                    txtWIPID.EditValue = ""
                    DateEdit1.DateTime = Now
                    strWHID = NodeTag
                    txtWH.EditValue = NodeText
                    cbKnifeType.Enabled = False
                    cbKnifeType.Text = "待處理"
                    cbType.EditValue = "正常"
                    ' CheckEdit1.Checked = True  '默認為已審核狀況(添加時直接保存--審核)
                End If
                XtraTabControl1.SelectedTabPage = XtraTabPage1
                SetObjectEnable(True, False)
            Case "popWareInputCheck"
                loadedit(txtWIPID.EditValue)
                XtraTabControl1.SelectedTabPage = XtraTabPage2
                SetObjectEnable(False, True)
                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                popKnifeWareInput.Enabled = False
            Case "popWareInputView" '---------------------------------------查看
                loadedit(txtWIPID.EditValue)
                XtraTabControl1.SelectedTabPage = XtraTabPage1
                SetObjectEnable(False, False)
                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                cmdSave.Enabled = False
                popKnifeWareInput.Enabled = False
            Case "popWareInputReCheck" '-------------------------------------復核
                loadedit(txtWIPID.EditValue)
                XtraTabControl1.SelectedTabPage = XtraTabPage3
                SetObjectEnable(False, True)
                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                popKnifeWareInput.Enabled = False
        End Select
        '加載附件供顯示
        GridFile.AutoGenerateColumns = False
        GridFile.RowHeadersWidth = 15
        'Dim dt As New FileController
        'GridFile.DataSource = dt.FileBond_GetList("5101", txtWIPID.EditValue, Nothing)
        GridFile.Refresh()
        '  XtraTabPage2.PageVisible = False  '不再需要審核記錄信息
    End Sub
    Sub loadedit(ByVal WIP_ID As String)
        ds.Tables("KnifeWareInput").Clear()
        Dim objInfo As List(Of LFERP.Library.KnifeWare.KnifeWareInputInfo)
        Dim pc As New LFERP.Library.KnifeWare.KnifeWareInputContraller
        Dim i As Integer
        Dim row As DataRow
        Try
            objInfo = pc.KnifeWareInput_Getlist(WIP_ID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If objInfo Is Nothing Then
                '沒有數據
                Exit Sub
            End If
            cbType.EditValue = objInfo(0).WIP_Type
            strWHID = objInfo(0).WH_ID
            txtWH.EditValue = objInfo(0).WH_Name
            DateEdit1.EditValue = Format(objInfo(0).WIP_AddDate, "yyyy/MM/dd")
            strDPTID = objInfo(0).DPT_ID
            ButtonEdit2.EditValue = objInfo(0).DPT_Name

            CheckEdit1.Checked = objInfo(0).WIP_Check
            CheckDate.Text = objInfo(0).WIP_CheckDate
            CheckAction.Text = objInfo(0).WIP_CheckActionName
            CheckRemark.Text = objInfo(0).WIP_CheckRemark
            OldCheck = objInfo(0).WIP_Check

            CheckEdit2.Checked = objInfo(0).WIP_ReCheck
            RecheckDate.Text = objInfo(0).WIP_ReCheckDate
            RecheckAction.Text = objInfo(0).WIP_ReCheckAction
            RecheckRemark.Text = objInfo(0).WIP_ReCheckRemark
            txtCardID.Text = objInfo(0).CardID
            cbKnifeType.Text = objInfo(0).KnifeType

            For i = 0 To objInfo.Count - 1
                row = ds.Tables("KnifeWareInput").NewRow
                row("WIP_NUM") = objInfo(i).WIP_NUM
                row("WIP_ID") = objInfo(i).WIP_ID
                row("M_Code") = objInfo(i).M_Code
                row("M_Name") = objInfo(i).M_Name
                row("M_Gauge") = objInfo(i).M_Gauge
                row("M_Unit") = objInfo(i).M_Unit
                row("WIP_Qty") = objInfo(i).WIP_Qty
                row("OS_BatchID") = objInfo(i).OS_BatchID
                row("WIP_Remark") = objInfo(i).WIP_Remark
                row("KnifeType") = objInfo(i).KnifeType
                ds.Tables("KnifeWareInput").Rows.Add(row)
            Next
        Catch ex As Exception
            MsgBox(ex.Message, 64, "提示")
        End Try
    End Sub

    Private Sub popKnifeWareInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popWareInputAdd.Click, popWareInputDel.Click
        Select Case sender.Name
            Case "popWareInputAdd"
                tempCode = ""
                tempValue5 = strWHID
                tempValue6 = "倉庫管理"
                frmKnifeBOMSelect.StartPosition = FormStartPosition.CenterScreen
                frmKnifeBOMSelect.KnifeInPut = Label1.Text
                frmKnifeBOMSelect.ShowDialog()

                If frmKnifeBOMSelect.XtraTabControl1.SelectedTabPageIndex = 0 Then
                    '增加記錄
                    If tempCode = "" Then
                        Exit Sub
                    Else
                        AddRow(tempCode, "")
                    End If
                ElseIf frmKnifeBOMSelect.XtraTabControl1.SelectedTabPageIndex = 1 Then
                    Dim i, n As Integer
                    Dim arr(n) As String
                    arr = Split(tempValue7, ",")
                    n = Len(Replace(tempValue7, ",", "," & "*")) - Len(tempValue7)
                    For i = 0 To n
                        If arr(i) = "" Then
                            Exit Sub
                        End If
                        AddRow(arr(i), EditID)
                    Next
                ElseIf frmKnifeBOMSelect.XtraTabControl1.SelectedTabPageIndex = 2 Then
                    Dim i, n As Integer
                    Dim arr(n) As String
                    arr = Split(tempValue8, ",")
                    n = Len(Replace(tempValue8, ",", "," & "*")) - Len(tempValue8)
                    For i = 0 To n
                        If arr(i) = "" Then
                            Exit Sub
                        End If
                        AddRow(arr(i), "")
                    Next
                End If
                tempValue7 = ""
                tempValue8 = ""
            Case "popWareInputDel"
                If ds.Tables("KnifeWareInput").Rows.Count = 0 Then Exit Sub
                Dim DelTemp As String
                DelTemp = GridView1.GetRowCellDisplayText(ArrayToString(GridView1.GetSelectedRows()), "M_Code")

                If DelTemp = "M_Code" Then
                Else
                    '在刪除表中增加被刪除的記錄
                    Dim row As DataRow = ds.Tables("DelDate").NewRow
                    'row("M_CodeSub") = CodeSubData.Tables("CodeSub").Rows(GridView1.FocusedRowHandle)("M_CodeSub")
                    row("WIP_NUM") = ds.Tables("KnifeWareInput").Rows(GridView1.FocusedRowHandle)("WIP_NUM")
                    row("WIP_ID") = ds.Tables("KnifeWareInput").Rows(GridView1.FocusedRowHandle)("WIP_ID")
                    row("M_Code") = DelTemp
                    ds.Tables("DelDate").Rows.Add(row)
                End If
                ds.Tables("KnifeWareInput").Rows.RemoveAt(CInt(ArrayToString(GridView1.GetSelectedRows())))
        End Select
    End Sub
    ''增加行
    Sub AddRow(ByVal strCode As String, ByVal OS_BatchID As String)
        Dim row As DataRow
        row = ds.Tables("KnifeWareInput").NewRow
        If strCode = "" Then
        Else
            Dim i As Integer
            For i = 0 To ds.Tables("KnifeWareInput").Rows.Count - 1
                If strCode = ds.Tables("KnifeWareInput").Rows(i)("M_Code") Then
                    MsgBox("一張單不允許有重復物料編碼....", 64, "提示")
                    Exit Sub
                End If
            Next
            Dim mc As New LFERP.Library.Material.MaterialController
            Dim objInfo As New LFERP.Library.Material.MaterialInfo
            objInfo = mc.MaterialCode_Get(strCode)

            If objInfo Is Nothing Then Exit Sub

            If objInfo.M_IsEnabled = False Then  '判斷當前物料是否可用 2012-2-20，不可用不允許報價！
                MsgBox("當前物料不可用，不允許入庫！", 64, "提示")
                Exit Sub
            End If
            ' row = ds.Tables("WareInput").NewRow
            'CodeSubData.Tables("CodeSub").NewRow()
            row("WIP_NUM") = Nothing
            row("WIP_ID") = Nothing
            row("M_Code") = objInfo.M_Code
            row("M_Name") = objInfo.M_Name
            Dim unit As New LFERP.DataSetting.UnitController
            Dim unitinfo As List(Of LFERP.DataSetting.UnitInfo)

            unitinfo = unit.GetUnitList(objInfo.M_Unit)
            If unitinfo.Count > 0 Then
                row("M_Unit") = unitinfo(0).U_Name
            End If

            row("M_Gauge") = objInfo.M_Gauge
            row("OS_BatchID") = OS_BatchID
            row("WIP_Qty") = 0

            ds.Tables("KnifeWareInput").Rows.Add(row)
        End If
        GridView1.MoveLast()
    End Sub


    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Select Case EditItem
            Case "popWareInputAdd" ''''''''''''''''''''''''''''''''新刀
                If Edit = False Then
                    If CheckSave() = True Then
                        SaveDataNew()
                        cmdPrint.Visible = True
                        cmdSave.Enabled = False
                        MsgBox("新增成功", 64, "提示")
                    End If
                ElseIf Edit = True Then
                    SaveDataEdit()
                    MsgBox("修改成功", 64, "提示")
                End If

            Case "popWareInputAddKnife" ''''''''''''''''''''''''''''''''待處理
                If Edit = False Then

                    If CheckSave() = True Then
                        SaveDataNew()
                        cmdPrint.Visible = True
                        cmdSave.Enabled = False
                        MsgBox("新增成功", 64, "提示")
                    End If
                ElseIf Edit = True Then
                    SaveDataEdit()
                    MsgBox("修改成功", 64, "提示")
                End If
            Case "popWareInputCheck"
                UpdateCheck()  '取消審核
            Case "popWareInputReCheck" '-------------------------------復核
                UpdateReCheck() '復核
            Case "popWareInputView" '-----------------------------------查看
                loadedit(txtWIPID.EditValue)
                XtraTabControl1.SelectedTabPage = XtraTabPage1
                SetObjectEnable(False, False)
                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                cmdSave.Enabled = False
        End Select
    End Sub
    Sub SetObjectEnable(ByVal a As Boolean, ByVal b As Boolean)
        txtWH.Enabled = a
        ButtonEdit2.Enabled = a
        DateEdit1.Enabled = a
        cbType.Enabled = a
        CheckEdit1.Enabled = b
        CheckRemark.Enabled = b
    End Sub

    Sub SaveDataNew()
        Dim mc As New KnifeWareInputContraller
        Dim mi As New KnifeWareInputInfo
        Dim lockobj As New Object()
        '需要鎖定的代碼塊 
        SyncLock lockobj
            Thread.Sleep(Int(Rnd() * 500) + 100)
            txtWIPID.EditValue = GetWI_ID()
        End SyncLock

        If Len(txtWIPID.EditValue) = 0 Then
            MsgBox("不能生成單號，無法保存！", 64, "提示")
            Exit Sub
        End If

        mi.WIP_ID = txtWIPID.EditValue
        mi.WIP_Type = cbType.EditValue
        mi.WH_ID = strWHID
        mi.WIP_AddDate = Format(DateEdit1.EditValue, "yyyy/MM/dd")
        mi.WIP_Action = InUserID
        'mi.DPT_ID = ButtonEdit2.EditValue
        mi.DPT_ID = strDPTID
        mi.WIP_Check = CheckEdit1.Checked
        mi.WIP_CheckAction = InUserID
        mi.WIP_CheckDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
        mi.WIP_CheckRemark = CheckRemark.Text
        mi.CardID = txtCardID.Text
        Select Case EditItem
            Case "popWareInputAdd" ''''新刀
                mi.KnifeType = "新刀"
            Case "popWareInputAddKnife" '待處理
                mi.KnifeType = "待處理"
        End Select

        Dim m As Integer
        For m = 0 To ds.Tables("knifeWareInput").Rows.Count - 1
            If IsDBNull(ds.Tables("knifeWareInput").Rows(m)("WIP_Qty")) Or ds.Tables("knifeWareInput").Rows(m)("WIP_Qty") = 0 Then
                MsgBox("入庫數量不能為空或0!", 64, "提示")
                Me.Close()
                Exit Sub
            End If
        Next

        Dim i As Integer
        For i = 0 To ds.Tables("knifeWareInput").Rows.Count - 1
            mi.WIP_NUM = GetWI_NUM()
            mi.M_Code = ds.Tables("knifeWareInput").Rows(i)("M_Code")
            mi.OS_BatchID = ds.Tables("knifeWareInput").Rows(i)("OS_BatchID")
            mi.WIP_Qty = CDbl(ds.Tables("knifeWareInput").Rows(i)("WIP_Qty"))

            If IsDBNull(ds.Tables("knifeWareInput").Rows(i)("WIP_Remark")) Then
                mi.WIP_Remark = Nothing
            Else
                mi.WIP_Remark = ds.Tables("knifeWareInput").Rows(i)("WIP_Remark")
            End If
            mc.KnifeWareInput_Add(mi)
        Next

        ' If CheckEdit1.Checked = True Then
        Dim mt As New SharePurchaseController
        Dim mm As New SharePurchaseInfo
        Dim j As Integer

        mm.WH_ID = strWHID
        For j = 0 To ds.Tables("knifeWareInput").Rows.Count - 1
            '-----------------------變更庫存主檔-------------------------------
            Dim wifo As New LFERP.Library.KnifeWare.KnifeWareInventorySubInfo
            mm.M_Code = ds.Tables("knifeWareInput").Rows(j)("M_Code")
            Dim DouQty As Double = CDbl(ds.Tables("knifeWareInput").Rows(j)("WIP_Qty"))
            Dim wi As LFERP.Library.WareHouse.WareInventory.WareInventoryInfo
            Dim wc As New LFERP.Library.WareHouse.WareInventory.WareInventoryMTController
            wi = wc.WareInventory_GetSub(ds.Tables("knifeWareInput").Rows(j)("M_Code"), strWHID)
            If wi Is Nothing Then
                wifo.WI_All = 0 + DouQty
            Else
                wifo.WI_All = wi.WI_Qty + DouQty
            End If
            '-----------------------變更庫存子檔-------------------------------
            Dim wiinfo As New LFERP.Library.KnifeWare.KnifeWareInventorySubInfo
            Dim wcco As New LFERP.Library.KnifeWare.KnifeWareInventorySubControl
            wiinfo = wcco.KnifeWareInventorySub_GetList(ds.Tables("knifeWareInput").Rows(j)("M_Code"), strWHID)
            If wiinfo Is Nothing Then
                Select Case EditItem
                    Case "popWareInputAdd" ''''新刀
                        wifo.WI_SQty = (0 + DouQty)
                        wifo.WI_SReQty = 0
                    Case "popWareInputAddKnife" '待處理
                        wifo.WI_SReQty = (0 + DouQty)
                        wifo.WI_SQty = 0
                End Select
            Else
                Select Case EditItem
                    Case "popWareInputAdd" ''''新刀
                        wifo.WI_SQty = (wiinfo.WI_SQty + DouQty)
                        wifo.WI_SReQty = wiinfo.WI_SReQty
                    Case "popWareInputAddKnife" '待處理
                        wifo.WI_SReQty = (wiinfo.WI_SReQty + DouQty)
                        wifo.WI_SQty = wiinfo.WI_SQty
                End Select
            End If
            wifo.M_Code = ds.Tables("knifeWareInput").Rows(j)("M_Code")
            wifo.WH_ID = strWHID
            wcco.KnifeWareInventorySub_Update(wifo)
            '------------------------變更當前結餘數----------------------------
            Dim info As New KnifeWareInputInfo
            info.WIP_ID = txtWIPID.EditValue
            info.WH_ID = strWHID
            info.M_Code = ds.Tables("knifeWareInput").Rows(j)("M_Code")
            info.WIP_EndQty = mm.WI_Qty  '當前倉庫結餘數
            mc.KnifeWareInput_UpdateEndQty(info)
            '----------------------------------------------------
        Next
        ' End If
        'Me.Close()
    End Sub
#Region "沒有使用"
    Function CheckSave() As Boolean
        CheckSave = True
        Dim isNegative As Boolean
        Dim isPositive As Boolean
        Dim wulc As New WhiteUserListController
        Dim wuliL As New List(Of WhiteuserListInfo)
        Dim i As Integer
        Dim strCardID As String

        isNegative = False
        isPositive = False

        If Len(txtWH.EditValue) = 0 Then
            MsgBox("請選擇倉庫", 64, "提示")
            CheckSave = False
            Exit Function
        End If
        If Len(ButtonEdit2.EditValue) = 0 Then
            MsgBox("請選擇部門", 64, "提示")
            CheckSave = False
            Exit Function
        End If
        If ds.Tables("KnifeWareInput").Rows.Count = 0 Then
            MsgBox("請選擇物料", 64, "提示")
            CheckSave = False
            Exit Function
        End If
        '查詢是否相應的倉庫中夠數
        Dim mw As New WareInventory.WareInventoryMTController
        Dim mwi As List(Of WareInventory.WareInventoryInfo)
        Dim m As Integer
        For m = 0 To ds.Tables("KnifeWareInput").Rows.Count - 1

            If ds.Tables("KnifeWareInput").Rows(m)("M_Code").ToString.Trim = "" Then
                MsgBox("物料編碼不能為空", 64, "提示")
                CheckSave = False
                Exit Function
            End If
            If ds.Tables("KnifeWareInput").Rows(m)("M_Name").ToString.Trim = "" Then
                MsgBox("物料名稱不能為空", 64, "提示")
                CheckSave = False
                Exit Function
            End If
            If ds.Tables("KnifeWareInput").Rows(m)("M_Gauge").ToString.Trim = "" Then
                MsgBox("物料規格不能為空", 64, "提示")
                CheckSave = False
                Exit Function
            End If
            If ds.Tables("KnifeWareInput").Rows(m)("M_Unit").ToString.Trim = "" Then
                MsgBox("物料單位不能為空", 64, "提示")
                CheckSave = False
                Exit Function
            End If

            mwi = mw.WareInventory_GetList3(ds.Tables("KnifeWareInput").Rows(m)("M_Code"), strWHID, "True")

            If mwi.Count = 0 Then
                If CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) < 0 Then
                    MsgBox("當前物料在此倉庫未建立, 此時不允許入庫為負數!", 64, "提示")
                    Grid.Focus()
                    GridView1.FocusedRowHandle = i
                    CheckSave = False
                    Exit Function
                ElseIf CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) = 0 Then
                    MsgBox("入庫數量不能為零!", 64, "提示")
                    Grid.Focus()
                    GridView1.FocusedRowHandle = i
                    CheckSave = False
                    Exit Function
                ElseIf CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) > 0 Then
                    CheckSave = True
                End If
            Else
                If mwi(0).WI_Qty = 0 Then
                    If CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) < 0 Then
                        MsgBox("當前物料庫存為0, 此時不允許入庫為負數!", 64, "提示")
                        Grid.Focus()
                        GridView1.FocusedRowHandle = i
                        CheckSave = False
                        Exit Function
                    Else
                        CheckSave = True
                    End If
                ElseIf CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) = 0 Then
                    MsgBox("入庫數量不能為零!", 64, "提示")
                    Grid.Focus()
                    GridView1.FocusedRowHandle = i
                    CheckSave = False
                    Exit Function
                ElseIf mwi(0).WI_Qty > 0 And mwi(0).WI_Qty + ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty") < 0 Then
                    MsgBox("當期沖帳數量大於當前倉庫庫存數,請核查!", 64, "提示")
                    CheckSave = False
                    Exit Function
                ElseIf mwi(0).WI_Qty < 0 Then

                    If mwi(0).WI_Qty + CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) >= 0 Then
                        CheckSave = True
                    Else
                        MsgBox("當前物料" & ds.Tables("KnifeWareInput").Rows(m)("M_Code") & "庫存變更后為負數,請檢查原因!", 64, "提示")
                        Grid.Focus()
                        GridView1.FocusedRowHandle = i
                        CheckSave = False
                        Exit Function
                    End If

                Else
                    CheckSave = True
                End If
            End If
            If CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) < 0 Then
                isNegative = True
            ElseIf CDbl(ds.Tables("KnifeWareInput").Rows(m)("WIP_Qty")) > 0 Then
                isPositive = True
            End If
        Next

        If isPositive = True And isNegative = True Then
            MsgBox("同一張入庫單中入庫數量不允許同時存在正數和負數!", 64, "提示")
            Grid.Focus()
            GridView1.FocusedRowHandle = i
            CheckSave = False
            Exit Function
        ElseIf isNegative = True Then
            If txtCardID.Text = "" Then
                MsgBox("輸入數量為負數時需要刷卡，請刷卡!", 64, "提示")
                Grid.Focus()
                GridView1.FocusedRowHandle = i
                CheckSave = False
                Exit Function
            End If

            '@2013/1/30 添加 只有指定人員才可以在入庫數量為負數時刷卡-----
            'strCardID = txtCardID.Text
            'wuliL = wulc.WhiteUserListSub_GetList(strCardID, Nothing)

            'If wuliL.Count > 0 Then
            '    For i = 0 To wuliL.Count - 1
            '        If wuliL(i).W_Remark = "沖銷卡" Then
            '            CheckSave = True
            '            Exit Function
            '        End If
            '    Next
            'End If

            'MsgBox("該刷卡人不存在入庫數量為負數時的刷卡權限，不能入負數!", 64, "提示")
            'txtCardID.Text = ""
            'CheckSave = False
            Exit Function
            '--------------------------------------------------------------------------------------------------
        ElseIf isPositive = True Then
            If txtCardID.Text <> "" Then
                If MsgBox("輸入數量為正數時不需要刷卡，是否清空刷卡人信息，并保存數據？", MsgBoxStyle.YesNo + MsgBoxStyle.Information, "提示") = MsgBoxResult.Yes Then
                    txtCardID.Text = ""
                    CheckSave = True
                Else
                    CheckSave = False
                End If
            End If
        End If
    End Function

    Sub SaveDataEdit()
        On Error Resume Next
        If Len(txtWH.EditValue) = 0 Then
            MsgBox("請選擇倉庫", 64, "提示")
            Exit Sub
        End If
        If Len(ButtonEdit2.EditValue) = 0 Then
            MsgBox("請選擇部門", 64, "提示")
            Exit Sub
        End If

        If ds.Tables("knifeWareInput").Rows.Count = 0 Then
            MsgBox("請選擇物料", 64, "提示")
            Exit Sub
        End If

        If Len(txtWIPID.EditValue) = 0 Then
            MsgBox("單號為空，無法保存！", 64, "提示")
            Exit Sub
        End If
        '更新刪除的記錄
        If ds.Tables("DelDate").Rows.Count > 0 Then
            Dim ii As Integer
            For ii = 0 To ds.Tables("DelDate").Rows.Count - 1
                Dim mc2 As New KnifeWareInputContraller
                If Not IsDBNull(ds.Tables("DelDate").Rows(ii)("WIP_NUM")) Then
                    mc2.KnifeWareInput_Delete(ds.Tables("DelDate").Rows(ii)("WIP_NUM"), Nothing)
                End If
            Next ii
        End If

        Dim mc As New KnifeWareInputContraller
        Dim mi As New KnifeWareInputInfo
        mi.WIP_ID = txtWIPID.EditValue
        mi.WIP_Type = cbType.EditValue
        mi.WH_ID = strWHID
        mi.WIP_AddDate = DateEdit1.EditValue
        mi.WIP_Action = InUserID
        'mi.DPT_ID = ButtonEdit2.EditValue
        mi.DPT_ID = strDPTID
        mi.WIP_Check = CheckEdit1.Checked
        mi.WIP_CheckAction = InUserID
        mi.WIP_CheckDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
        mi.WIP_CheckRemark = CheckRemark.Text
        Dim i As Integer
        For i = 0 To ds.Tables("knifeWareInput").Rows.Count - 1
            If IsDBNull(ds.Tables("knifeWareInput").Rows(i)("WIP_NUM")) Then   '新增
                mi.WIP_NUM = GetWI_NUM()
                mi.M_Code = ds.Tables("knifeWareInput").Rows(i)("M_Code")
                'mi.M_Name = ds.Tables("knifeWareInput").Rows(i)("M_Name")
                'mi.M_Gauge = ds.Tables("knifeWareInput").Rows(i)("M_Gauge")
                'mi.M_Unit = ds.Tables("knifeWareInput").Rows(i)("M_Unit")
                mi.OS_BatchID = ds.Tables("knifeWareInput").Rows(i)("OS_BatchID")
                mi.WIP_Qty = CDbl(ds.Tables("knifeWareInput").Rows(i)("WIP_Qty"))
                mi.WIP_Remark = ds.Tables("knifeWareInput").Rows(i)("WIP_Remark")
                mc.KnifeWareInput_Add(mi)

            ElseIf Not IsDBNull(ds.Tables("knifeWareInput").Rows(i)("WIP_NUM")) Then ' 修改
                mi.WIP_NUM = ds.Tables("knifeWareInput").Rows(i)("WIP_NUM")
                mi.M_Code = ds.Tables("knifeWareInput").Rows(i)("M_Code")
                'mi.M_Name = ds.Tables("knifeWareInput").Rows(i)("M_Name")
                'mi.M_Gauge = ds.Tables("knifeWareInput").Rows(i)("M_Gauge")
                'mi.M_Unit = ds.Tables("knifeWareInput").Rows(i)("M_Unit")
                mi.OS_BatchID = ds.Tables("knifeWareInput").Rows(i)("OS_BatchID")
                mi.WIP_Qty = CDbl(ds.Tables("knifeWareInput").Rows(i)("WIP_Qty"))
                mi.WIP_Remark = ds.Tables("knifeWareInput").Rows(i)("WIP_Remark")
                mi.WIP_EditDate = Format(Now, "yyyy/MM/dd")
                mc.KnifeWareInput_Update(mi)
            End If
        Next
        'Me.Close()
    End Sub
#End Region
    Sub UpdateCheck()
        Dim mc As New KnifeWareInputContraller
        Dim mi As New KnifeWareInputInfo
        mi.WIP_ID = txtWIPID.EditValue
        mi.WIP_Check = CheckEdit1.Checked
        mi.WIP_CheckAction = InUserID
        mi.WIP_CheckDate = Format(Now, "yyyy/MM/dd")
        mi.WIP_CheckRemark = CheckRemark.Text
        If OldCheck = CheckEdit1.Checked Then
            MsgBox("取消審核狀態未改變，請更改狀態後再保存……", 64, "提示")
            Exit Sub
        End If
        If mc.KnifeWareInput_UpdateCheck(mi) = False Then
            MsgBox("取消審核失敗", 64, "提示")
            Exit Sub
        End If
        Me.Close()
    End Sub
    Sub UpdateReCheck()
        Dim mc As New KnifeWareInputContraller
        Dim mi As New KnifeWareInputInfo
        mi.WIP_ID = txtWIPID.EditValue
        mi.WIP_ReCheck = CheckEdit2.Checked
        mi.WIP_ReCheckAction = InUserID
        mi.WIP_ReCheckDate = Format(Now, "yyyy/MM/dd")
        mi.WIP_ReCheckRemark = CheckRemark.Text

        If mc.KnifeWareInput_UpdateReCheck(mi) = False Then
            MsgBox("審核失敗", 64, "提示")
            Exit Sub
        End If
        Me.Close()
    End Sub


    Function GetWI_ID() As String
        '生成新pm
        Dim pm As New KnifeWareInputContraller
        Dim pi As New KnifeWareInputInfo
        Dim ndate As String
        ndate = "WI" + Format(Now(), "yyMM")
        pi = pm.KnifeWareInput_GetID(ndate)
        If pi Is Nothing Then
            GetWI_ID = ndate + "00001"
        Else
            GetWI_ID = ndate + Mid((CInt(Mid(pi.WIP_ID, 7)) + 100001), 2)
        End If
    End Function

    Function GetWI_NUM() As String
        '生成新pS
        Dim pm As New KnifeWareInputContraller
        Dim pi As New KnifeWareInputInfo
        pi = pm.KnifeWareInput_GetNUM(Nothing)
        If pi Is Nothing Then
            GetWI_NUM = "I000000001"
        Else
            GetWI_NUM = "I" & Mid((CInt(Mid(pi.WIP_NUM, 2)) + 1000000001), 2)
        End If
    End Function

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Dim ds As New DataSet

        If GridView1.RowCount = 0 Then Exit Sub

        Dim ltc As New CollectionToDataSet
        Dim ltc1 As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet

        Dim pmc As New LFERP.Library.KnifeWare.KnifeWareInputContraller
        Dim suc As New SystemUser.SystemUserController
        Dim wh As New WareHouseController
        Dim uc2 As New DepartmentControler

        ds.Tables.Clear()
        Dim strA As String
        strA = txtWIPID.Text

        ltc.CollToDataSet(ds, "KnifeWareInput", pmc.KnifeWareInput_Getlist(strA, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        ltc1.CollToDataSet(ds, "WareHouse", wh.WareHouse_GetList(Nothing))
        ltc2.CollToDataSet(ds, "Department", uc2.Department_GetList(Nothing, Nothing, Nothing))
        PreviewRPT(ds, "rptKnifeWareInput1", "入庫單", True, True)

        ltc = Nothing
        ltc1 = Nothing
        ltc2 = Nothing
    End Sub

    Private Sub ButtonEdit2_ButtonClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles ButtonEdit2.ButtonClick
        ''frmDepartmentSelect.DptID = ""
        ''frmDepartmentSelect.DptName = ""
        ''frmDepartmentSelect.ShowDialog()
        ''If frmDepartmentSelect.DptID = "" Then
        ''Else

        ''    ButtonEdit2.Text = frmDepartmentSelect.DptName
        ''    strDPTID = frmDepartmentSelect.DptID
        ''    'ButtonEdit2.Tag = frmDepartmentSelect.DptID

        ''End If
        '2013-1-7
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "510107")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value <> "" Then
                frmDepartmentSelect1.DptID = ""
                frmDepartmentSelect1.DptName = ""
                tempValue = pmwiL.Item(0).PMWS_Value
                frmDepartmentSelect1.ShowDialog()

                If frmDepartmentSelect1.DptID = "" Then
                Else
                    ButtonEdit2.Text = frmDepartmentSelect1.DptName
                    strDPTID = frmDepartmentSelect1.DptID
                End If

                Exit Sub
            End If
        End If

        frmDepartmentSelect.DptID = ""
        frmDepartmentSelect.DptName = ""
        frmDepartmentSelect.ShowDialog()

        If frmDepartmentSelect.DptID = "" Then
        Else
            ButtonEdit2.Text = frmDepartmentSelect.DptName
            strDPTID = frmDepartmentSelect.DptID
            'ButtonEdit2.Tag = frmDepartmentSelect.DptID
        End If
    End Sub

    Private Sub btnRefCard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefCard.Click

        txtCardID.Text = ReadCard1()
        ButtonEdit2.Enabled = False

        Dim wulc As New WhiteUserListController
        Dim wuliL As New List(Of WhiteuserListInfo)
        wuliL = wulc.WhiteUserListSub_GetList(txtCardID.Text, strWHID)

        If wuliL.Count <= 0 Or txtCardID.Text = "" Then
            MsgBox("非法卡號", 64, "提示")
            Exit Sub
        End If

        If wuliL.Item(0).WareChange = False Then
            MsgBox("沒有沖銷權限", 64, "提示")
            Exit Sub
        Else
            ButtonEdit2.Text = wuliL.Item(0).DPT_Name
            strDPTID = wuliL.Item(0).DPT_ID
        End If

    End Sub
End Class