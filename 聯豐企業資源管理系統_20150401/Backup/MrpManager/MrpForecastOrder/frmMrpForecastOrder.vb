Imports LFERP.Library.MrpManager.MrpForecastOrder
Imports LFERP.Library.MrpManager.Bom_M
Imports LFERP.SystemManager

Public Class frmMrpForecastOrder

#Region "實例與字段"
    Dim MrpOrderInfo As New MrpForecastOrderInfo
    Dim MrpCon As New MrpForecastOrderController
    Dim mrpecom As New MrpForecastOrderEntryController
    Dim mrpeInfo As New MrpForecastOrderEntryInfo
    Dim boCheck As Boolean = False                  '用來判斷審核狀態是否有改變
    Dim ds As New DataSet
    Dim time As Integer = 1
    Dim lock As Integer
    Dim MrpEntryList As New List(Of MrpForecastOrderEntryInfo)
    Dim StartDate As Date
    Dim Days As Integer
    Private _Type As String
    Public Property Type() As String
        Get
            Return _Type
        End Get
        Set(ByVal value As String)
            _Type = value
        End Set
    End Property
    Private _StrForecastID As String = Nothing
    Public Property StrForecastID() As String
        Get
            Return _StrForecastID
        End Get
        Set(ByVal value As String)
            _StrForecastID = value
        End Set
    End Property
    Private _StrM_Code As String
    Public Property StrM_Code()
        Get
            Return _StrM_Code
        End Get
        Set(ByVal value)
            _StrM_Code = value
        End Set
    End Property
#End Region

#Region "頁面加載"

    Private Sub frmMrpForecastOrderAdd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        '創建子表
        CreateTable()
        Dim BOCon As New Bom_MController
        txtMO_CusterID.Properties.DisplayMember = "MO_CusterName"    'txt
        txtMO_CusterID.Properties.ValueMember = "MO_CusterID"   'EditValue
        txtMO_CusterID.Properties.DataSource = MrpCon.CusterGetName(Nothing, Nothing)
        GLU_MCode.Properties.DisplayMember = "ParentGroup"    'txt
        GLU_MCode.Properties.ValueMember = "ParentGroup"   'EditValue
        If _Type = EditEnumType.ADD Then
            ' GLU_MCode.Properties.DataSource = BOCon.Bom_M_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, True, Nothing)
        Else
            ' GLU_MCode.Properties.DataSource = BOCon.Bom_M_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If

        Select Case _Type
            Case EditEnumType.ADD
                Lbl_Title.Text = Lbl_Title.Text + EditEnumValue(EditEnumType.ADD)
                Me.Text = Lbl_Title.Text
                txtForecastID.Text = MrpCon.MrpForecastOrder_GetMONum().ToString
                txtUserID.Text = InUserID
                txtMO_CusterID.Enabled = True
                det_ForecastDate.Enabled = True
                txtMO_CusterID.Enabled = True
                ChkCancellation.Enabled = True
                GLU_MCode.Enabled = True
            Case EditEnumType.EDIT
                Lbl_Title.Text = Lbl_Title.Text + EditEnumValue(EditEnumType.EDIT)
                Me.Text = Lbl_Title.Text

                txtMO_CusterID.Enabled = True
                det_ForecastDate.Enabled = True
                txtMO_CusterID.Enabled = True
                ChkCancellation.Enabled = True

                GLU_MCode.Enabled = True
                getMrpForecastOrder()
            Case EditEnumType.CHECK
                Lbl_Title.Text = Lbl_Title.Text + EditEnumValue(EditEnumType.CHECK)
                Me.Text = Lbl_Title.Text
                getMrpForecastOrder()
                txtCheckRemark.Text = MrpOrderInfo.CheckRemark
                txtUserID.Text = MrpOrderInfo.CreateUserName
                txtPlanType.Enabled = False
                GridView.OptionsBehavior.Editable = False
                xtpCheck.PageVisible = True
                xtcTable.SelectedTabPage = xtpCheck

                cmsMenuStrip.Visible = False
                boCheck = MrpOrderInfo.CheckBit

                lblCheckDate.Text = Format(Now, "yyyy/MM/dd")
                lblCheckUserID.Text = InUser
                If MrpOrderInfo.CheckBit = True Then
                    chkCheckBit.Checked = True
                    lblCheckDate.Text = Format(MrpOrderInfo.CheckDate, "yyyy/MM/dd")
                    lblCheckUserID.Text = MrpOrderInfo.CheckUserName
                    txtCheckRemark.EditValue = MrpOrderInfo.CheckRemark
                Else
                    lblCheckDate.Text = Format(Now, "yyyy/MM/dd")
                    lblCheckUserID.Text = InUser
                End If

                txt_CusterPO.Enabled = False

                Grid1.ContextMenuStrip = Nothing
            Case EditEnumType.VIEW
                txtPlanType.Enabled = False
                Lbl_Title.Text = Lbl_Title.Text + EditEnumValue(EditEnumType.VIEW)
                Me.Text = Lbl_Title.Text
                getMrpForecastOrder()
                txtUserID.Text = MrpOrderInfo.CreateUserName
                GridView.OptionsBehavior.Editable = False
                xtpCheck.PageVisible = True

                txt_CusterPO.Enabled = False
                cmdSave.Visible = False
                chkCheckBit.Enabled = False
                lblCheckUserID.Enabled = False
                txtCheckRemark.Enabled = False

                Grid1.ContextMenuStrip = Nothing
                If MrpOrderInfo.CheckBit = True Then
                    chkCheckBit.Checked = True
                    lblCheckDate.Text = Format(MrpOrderInfo.CheckDate, "yyyy/MM/dd")
                    lblCheckUserID.Text = MrpOrderInfo.CheckUserName
                    txtCheckRemark.EditValue = MrpOrderInfo.CheckRemark
                End If
            Case EditEnumType.ELSEONE
                txtPlanType.Enabled = False
                Lbl_Title.Text = Lbl_Title.Text + "明細審核"
                Me.Text = Lbl_Title.Text
                getMrpForecastOrder()
                txtUserID.Text = MrpOrderInfo.CreateUserName
                GridView.OptionsBehavior.Editable = False

                txt_CusterPO.Enabled = False
                chkCheckBit.Enabled = False
                lblCheckUserID.Enabled = False
                txtCheckRemark.Enabled = False

                If MrpOrderInfo.CheckBit = True Then
                    chkCheckBit.Checked = True
                    lblCheckDate.Text = Format(MrpOrderInfo.CheckDate, "yyyy/MM/dd")
                    lblCheckUserID.Text = MrpOrderInfo.CheckUserName
                    txtCheckRemark.EditValue = MrpOrderInfo.CheckRemark
                End If

                tsmNew.Visible = False
                tsmDelete.Visible = False
                tsm_MAdd.Visible = False


        End Select
        '子表顯示
        If _StrForecastID <> String.Empty Then
            LoadTable()

        End If
    End Sub
#End Region


#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030201")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmNew.Visible = True
                tsmNew.Enabled = True
                tsm_MAdd.Visible = True
                tsm_MAdd.Enabled = True
            End If
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030204")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsm_Check.Visible = True
                tsm_Check.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030203")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmDelete.Visible = True
                tsmDelete.Enabled = True
            End If
        End If
    End Sub
#End Region
#Region "子表的臨時表"
    '創建表
    Private Sub CreateTable()
        ds.Tables.Clear()
        '創建臨時子表
        With ds.Tables.Add("MrpForecastOrderEntry")
            .Columns.Add("ForecastID", GetType(String))
            .Columns.Add("MI_Qty", GetType(Decimal))
            .Columns.Add("MI_NeedDate", GetType(Date))
            .Columns.Add("UnitID", GetType(String))
            .Columns.Add("MI_Note", GetType(String))
            .Columns.Add("MI_WeekNumber", GetType(Integer))
            .Columns.Add("MI_ISpurchase", GetType(Boolean))
            .Columns.Add("MI_ISPpurchase", GetType(Boolean))
            .Columns.Add("MI_ISWorkOrder", GetType(Boolean))
            .Columns.Add("CreateUserID", GetType(String))
            .Columns.Add("CreateDate", GetType(String))
            .Columns.Add("ModifyUserID", GetType(String))
            .Columns.Add("ModifyDate", GetType(String))
            .Columns.Add("AutoID", GetType(Integer))
            .Columns.Add("CreateUserName", GetType(String))
            .Columns.Add("ModifyUserName", GetType(String))
            .Columns.Add("DateCross", GetType(String))
            .Columns.Add("IsMrpOperation", GetType(Boolean))
            .Columns.Add("DeliveryQty", GetType(Decimal))
            .Columns.Add("PlanQty", GetType(Decimal))
            .Columns.Add("PurchasedQty", GetType(Decimal))
            .Columns.Add("IsCheck", GetType(Boolean))
            .Columns.Add("CheckUserID", GetType(String))
        End With
        Grid1.DataSource = ds.Tables("MrpForecastOrderEntry")
        '創建臨時刪除表
        With ds.Tables.Add("DelTable")
            .Columns.Add("AutoID", GetType(Integer))
        End With
    End Sub

    '加載數據
    Private Sub LoadTable()
        ds.Tables("MrpForecastOrderEntry").Clear()
        MrpEntryList = mrpecom.MrpForecastOrderEntry_GetList(StrForecastID)
        If MrpEntryList.Count = 0 Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To MrpEntryList.Count - 1
            Dim row As DataRow
            row = ds.Tables("MrpForecastOrderEntry").NewRow
            row("AutoID") = MrpEntryList(i).AutoID
            row("CreateDate") = MrpEntryList(i).CreateDate
            row("CreateUserID") = MrpEntryList(i).CreateUserID
            row("CreateUserName") = MrpEntryList(i).CreateUserName
            row("ForecastID") = MrpEntryList(i).ForecastID
            row("MI_ISPpurchase") = MrpEntryList(i).MI_ISPpurchase
            row("MI_ISpurchase") = MrpEntryList(i).MI_ISpurchase
            row("MI_ISWorkOrder") = MrpEntryList(i).MI_ISWorkOrder
            row("MI_NeedDate") = MrpEntryList(i).MI_NeedDate
            row("MI_Note") = MrpEntryList(i).MI_Note
            row("MI_Qty") = MrpEntryList(i).MI_Qty
            row("MI_WeekNumber") = MrpEntryList(i).MI_WeekNumber
            row("ModifyDate") = MrpEntryList(i).ModifyDate
            row("ModifyUserID") = MrpEntryList(i).ModifyUserID
            row("ModifyUserName") = MrpEntryList(i).ModifyUserName
            row("DateCross") = MrpEntryList(i).DateCross
            row("IsMrpOperation") = MrpEntryList(i).IsMrpOperation
            row("DeliveryQty") = MrpEntryList(i).DeliveryQty
            row("PlanQty") = MrpEntryList(i).PlanQty
            row("PurchasedQty") = MrpEntryList(i).PurchasedQty
            row("IsCheck") = MrpEntryList(i).IsCheck
            row("CheckUserID") = MrpEntryList(i).CheckUserID
            ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
        Next
    End Sub
    '將子表中的數據載入實例
    Sub mrpeInfoFill(ByVal i As Integer)
        With ds.Tables("MrpForecastOrderEntry")
            mrpeInfo.AutoID = .Rows(i)("AutoID").ToString
            mrpeInfo.CreateDate = .Rows(i)("CreateDate").ToString
            mrpeInfo.CreateUserID = .Rows(i)("CreateUserID").ToString
            mrpeInfo.ForecastID = .Rows(i)("ForecastID").ToString
            mrpeInfo.MI_ISPpurchase = .Rows(i)("MI_ISPpurchase").ToString
            mrpeInfo.MI_ISpurchase = .Rows(i)("MI_ISpurchase").ToString
            mrpeInfo.MI_ISWorkOrder = .Rows(i)("MI_ISWorkOrder").ToString
            mrpeInfo.MI_NeedDate = .Rows(i)("MI_NeedDate")
            mrpeInfo.MI_Note = .Rows(i)("MI_Note").ToString
            mrpeInfo.MI_Qty = .Rows(i)("MI_Qty").ToString
            mrpeInfo.MI_WeekNumber = .Rows(i)("MI_WeekNumber").ToString
            mrpeInfo.ModifyDate = .Rows(i)("ModifyDate").ToString
            mrpeInfo.ModifyUserID = .Rows(i)("ModifyUserID").ToString
            mrpeInfo.DateCross = .Rows(i)("DateCross").ToString
            mrpeInfo.IsMrpOperation = .Rows(i)("IsMrpOperation").ToString
            mrpeInfo.DeliveryQty = .Rows(i)("DeliveryQty").ToString
            mrpeInfo.PlanQty = .Rows(i)("PlanQty").ToString
            mrpeInfo.PurchasedQty = .Rows(i)("PurchasedQty").ToString
            mrpeInfo.IsCheck = .Rows(i)("IsCheck")
            mrpeInfo.CheckUserID = .Rows(i)("CheckUserID").ToString

        End With
    End Sub
#End Region

#Region "按鈕功能"

    '保存按鈕
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Select Case _Type
            Case EditEnumType.ADD
                SaveAdd()
            Case EditEnumType.EDIT
                SaveEdit()
            Case EditEnumType.CHECK
                Check()
            Case EditEnumType.ELSEONE
                SaveChildCheck()
        End Select
    End Sub
    Private Sub SaveAdd()
        '主表數據
        Dim MrpInfo As New MrpForecastOrderInfo
        Dim Str As String
        Str = MrpCon.MrpForecastOrder_GetMONum()
        If Str <> txtForecastID.Text Then
            MsgBox(txtForecastID.Text & " 订单编号已经被占用，订单编号將变更為" + Str, MsgBoxStyle.OkOnly, "提示")
            MrpInfo.ForecastID = Str
        End If
        If CheckDateEmpty() = False Then
            Exit Sub
        End If

        MrpInfo.ForecastID = txtForecastID.Text
        MrpInfo.MO_CusterID = txtMO_CusterID.EditValue
        MrpInfo.MO_Cancellation = ChkCancellation.Checked
        MrpInfo.OrderInterID = txt_CusterPO.Text
        MrpInfo.CreateDate = Now
        MrpInfo.M_Code = GLU_MCode.EditValue
        MrpInfo.MO_PlanType = txtPlanType.Text
        MrpInfo.CheckBit = False
        MrpInfo.MO_ForecastDate = det_ForecastDate.DateTime
        MrpInfo.CreateUserID = InUserID
        Dim SumQty As Decimal = 0
        
        For i As Integer = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1
            If IsDBNull(ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_Qty")) = False Then
                SumQty = SumQty + ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_Qty")
            End If
        Next
        MrpInfo.NeedQty = SumQty
        If MrpCon.MrpForecastOrder_Add(MrpInfo) = False Then
            Exit Sub
        End If

        For i As Integer = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1
            With ds.Tables("MrpForecastOrderEntry")
                Dim mrpeInfo As New MrpForecastOrderEntryInfo
                mrpeInfo.CreateDate = .Rows(i)("CreateDate").ToString
                mrpeInfo.CreateUserID = .Rows(i)("CreateUserID").ToString
                mrpeInfo.ForecastID = .Rows(i)("ForecastID").ToString
                mrpeInfo.MI_NeedDate = .Rows(i)("MI_NeedDate").ToString
                mrpeInfo.MI_Note = .Rows(i)("MI_Note").ToString
                mrpeInfo.MI_Qty = .Rows(i)("MI_Qty")
                mrpeInfo.MI_WeekNumber = .Rows(i)("MI_WeekNumber").ToString
                mrpeInfo.DateCross = .Rows(i)("DateCross").ToString
                mrpecom.MrpForecastOrderEntry_Add(mrpeInfo)
            End With
        Next
        MsgBox("保存成功！")
        Me.Close()
    End Sub

    '保存按鈕功能—添加與修改
    Private Sub SaveEdit()
        '主表數據
        If CheckDateEmpty() = False Then
            Exit Sub
        End If
        Dim MrpInfo As New MrpForecastOrderInfo
        MrpInfo.ForecastID = txtForecastID.Text
        MrpInfo.MO_CusterID = txtMO_CusterID.EditValue
        MrpInfo.MO_Cancellation = ChkCancellation.Checked
        MrpInfo.OrderInterID = txt_CusterPO.Text
        MrpInfo.CreateDate = Now
        MrpInfo.M_Code = GLU_MCode.EditValue
        MrpInfo.MO_PlanType = txtPlanType.Text
        MrpInfo.MO_ForecastDate = det_ForecastDate.DateTime
        MrpInfo.CreateUserID = txtUserID.Text
        MrpInfo.ModifyDate = Format(Now, "yyyy/MM/dd")
        MrpInfo.ModifyUserID = InUserID
        MrpInfo.MO_CusterID = txtMO_CusterID.EditValue.ToString
        MrpInfo.NeedQty = caeNeedQty.Text
        If MrpCon.MrpForecastOrder_Update(MrpInfo) = False Then
            Exit Sub
        End If
        For i As Integer = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 2
            For j As Integer = i + 1 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1

                Dim strnum1 As String = Year(CDate(ds.Tables("MrpForecastOrderEntry").Rows(j)("MI_NeedDate"))).ToString + ds.Tables("MrpForecastOrderEntry").Rows(j)("MI_WeekNumber").ToString
                Dim strnum2 As String = Year(CDate(ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_NeedDate"))).ToString + ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_WeekNumber").ToString
                If strnum1 = strnum2 Then
                    MsgBox(Mid(strnum1, 1, 4) + "年—第" + Mid(strnum1, 5, 2) + "周已經存在！請刪除之後再保存。")
                    GridView.FocusedRowHandle = j
                    Exit Sub
                End If
            Next
        Next
        For i As Integer = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1
            With ds.Tables("MrpForecastOrderEntry")
                Dim mrpeInfo As New MrpForecastOrderEntryInfo
                mrpeInfo.CreateDate = .Rows(i)("CreateDate").ToString
                mrpeInfo.CreateUserID = .Rows(i)("CreateUserID").ToString
                mrpeInfo.ForecastID = .Rows(i)("ForecastID").ToString
                mrpeInfo.DateCross = .Rows(i)("DateCross").ToString
                mrpeInfo.MI_NeedDate = .Rows(i)("MI_NeedDate")
                mrpeInfo.MI_Note = .Rows(i)("MI_Note").ToString
                mrpeInfo.MI_Qty = .Rows(i)("MI_Qty").ToString
                mrpeInfo.MI_WeekNumber = .Rows(i)("MI_WeekNumber").ToString
                mrpeInfo.DateCross = .Rows(i)("DateCross").ToString
                mrpeInfo.ModifyUserID = InUserID
                If IsDBNull(.Rows(i)("PlanQty")) Then
                    mrpeInfo.PlanQty = Nothing
                Else
                    mrpeInfo.PlanQty = .Rows(i)("PlanQty")
                End If
                If ds.Tables("MrpForecastOrderEntry").Rows(i)("AutoID").ToString = String.Empty Then
                    mrpecom.MrpForecastOrderEntry_Add(mrpeInfo)
                Else
                    If ds.Tables("MrpForecastOrderEntry").Rows(i)("AutoID") = 0 Then
                        mrpecom.MrpForecastOrderEntry_Add(mrpeInfo)
                    End If
                    mrpeInfo.AutoID = .Rows(i)("AutoID").ToString
                    mrpecom.MrpForecastOrderEntry_Update(mrpeInfo)
                End If
            End With
        Next
        For i As Integer = 0 To ds.Tables("DelTable").Rows.Count - 1
            If ds.Tables("DelTable").Rows.Count = 0 Then
                Exit For
            End If
            If IsDBNull(ds.Tables("DelTable").Rows(i)("AutoID")) = False Then
                mrpecom.MrpForecastOrderEntry_Delete(ds.Tables("DelTable").Rows(i)("AutoID"), Nothing)
            End If
        Next
        MsgBox("保存成功！")
        Me.Close()
    End Sub


    '保存按鈕功能—添加與修改
    Private Sub SaveChildCheck()
      
        For i As Integer = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1
            With ds.Tables("MrpForecastOrderEntry")
                Dim mrpeInfo As New MrpForecastOrderEntryInfo
                mrpeInfo.CheckUserID = InUserID
                mrpeInfo.AutoID = .Rows(i)("AutoID").ToString
                mrpeInfo.IsCheck = .Rows(i)("IsCheck").ToString
                mrpecom.MrpForecastOrderEntry_Check(mrpeInfo)
            End With
        Next
        MsgBox("保存成功！")
        Me.Close()
    End Sub

    '保存按鈕功能—審核

    Private Sub Check()
        If chkCheckBit.Checked = boCheck Then
            MsgBox("審核狀態沒有改變，無須保存", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        Dim result As Boolean = MrpCon.MrpForecastOrder_Check(txtForecastID.Text, chkCheckBit.Checked, txtCheckRemark.Text, InUserID)
        If result = True Then
            MsgBox("保存成功！")
        Else
            MsgBox("保存失敗！")
        End If
        Me.Close()
    End Sub

    '取消按鈕
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
#End Region

#Region "菜單欄功能"
    '菜單——新增
    Private Sub tsmNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNew.Click
        Dim row As DataRow = ds.Tables("MrpForecastOrderEntry").NewRow
        row("CreateDate") = Format(Now, "yyyy/MM/dd")
        row("CreateUserID") = InUserID
        row("ForecastID") = txtForecastID.Text
        row("MI_ISPpurchase") = False
        row("MI_ISpurchase") = False
        row("MI_ISWorkOrder") = False
        row("IsMrpOperation") = False
        row("ModifyUserID") = Nothing
        row("UnitID") = Nothing
        ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
        GridView.FocusedRowHandle = GridView.RowCount - 1
    End Sub
    '菜單——刪除
    Private Sub tsmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDelete.Click
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        Dim row As DataRow
        row = ds.Tables("DelTable").NewRow
        row("AutoID") = GridView.GetFocusedRowCellValue("AutoID")
        ds.Tables("DelTable").Rows.Add(row)
        GridView.DeleteRow(GridView.FocusedRowHandle())
        GridView.SelectRow(GridView.RowCount - 1)
    End Sub
    '菜單——打印
    Private Sub tsmPaint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        Dim dss As New DataSet
        Dim ltc1 As New CollectionToDataSet
        Dim strSend As String = String.Empty
        strSend = "預測訂單信息表"
        mrpecom.MrpForecastOrderEntry_GetList(StrForecastID)
        ltc1.CollToDataSet(dss, "MrpForecastOrderEntry", mrpecom.MrpForecastOrderEntry_GetList(StrForecastID))
        PreviewRPT1(dss, "rtpMrpForecastOrderEntry", "表", strSend, strSend, True, True)
        ltc1 = Nothing
    End Sub

#End Region

#Region "公用方法、函數、事件"
    '頁面的控件的公用賦值方法
    Private Sub getMrpForecastOrder()
        If MrpCon.MrpForecastOrder_GetList(StrForecastID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).Count <= 0 Then
            Exit Sub
        End If
        MrpOrderInfo = MrpCon.MrpForecastOrder_GetList(StrForecastID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0)
        det_ForecastDate.Text = MrpOrderInfo.MO_ForecastDate.ToString
        txtUserID.Text = MrpOrderInfo.CreateUserName
        ChkCancellation.Checked = MrpOrderInfo.MO_Cancellation
        chkCheckBit.Checked = MrpOrderInfo.CheckBit
        txtMO_CusterID.EditValue = MrpOrderInfo.MO_CusterID.ToString
        txtMO_CusterID.Text = MrpOrderInfo.MO_CusterID.ToString
        txt_CusterPO.Text = MrpOrderInfo.OrderInterID.ToString
        txtForecastID.Text = StrForecastID
        GLU_MCode.EditValue = MrpOrderInfo.M_Code
        txtPlanType.Text = MrpOrderInfo.MO_PlanType
        caeNeedQty.Text = MrpOrderInfo.NeedQty
    End Sub


    '檢查控制值是否為空的方法
    Function CheckDateEmpty() As Boolean
        CheckDateEmpty = True
        If det_ForecastDate.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入預測日期！", MsgBoxStyle.OkOnly, "提示")
            det_ForecastDate.Focus()
            Exit Function
        End If
        If txtMO_CusterID.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入客戶編號！", MsgBoxStyle.OkOnly, "提示")
            txtMO_CusterID.Focus()
            Exit Function
        End If
        If txt_CusterPO.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入客戶訂單！", MsgBoxStyle.OkOnly, "提示")
            txt_CusterPO.Focus()
            Exit Function
        End If
        If GLU_MCode.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入產品信息！", MsgBoxStyle.OkOnly, "提示")
            GLU_MCode.Focus()
            Exit Function
        End If
        '--------訂單子檔
        If ds.Tables("MrpForecastOrderEntry").Rows.Count = 0 Then
            CheckDateEmpty = False
            MsgBox("訂單子檔不能為空！", MsgBoxStyle.OkOnly, "提示")
            GridView.Focus()
            Exit Function
        End If
        Dim i As Integer
        For i = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1
            If ds.Tables("MrpForecastOrderEntry").Rows(i)("ForecastID").ToString() = String.Empty Then
                CheckDateEmpty = False
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                MsgBox("預測訂單编号不能为空！", MsgBoxStyle.OkOnly, "提示")
                Exit Function
            End If
            If ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_Qty").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("訂單數量不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If
            If ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_NeedDate").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("需求日期不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If
        Next

        For i = 0 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 2
            For j As Integer = i + 1 To ds.Tables("MrpForecastOrderEntry").Rows.Count - 1

                Dim strnum1 As String = Year(CDate(ds.Tables("MrpForecastOrderEntry").Rows(j)("MI_NeedDate"))).ToString + ds.Tables("MrpForecastOrderEntry").Rows(j)("MI_WeekNumber").ToString
                Dim strnum2 As String = Year(CDate(ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_NeedDate"))).ToString + ds.Tables("MrpForecastOrderEntry").Rows(i)("MI_WeekNumber").ToString
                If strnum1 = strnum2 Then
                    MsgBox(Mid(strnum1, 1, 4) + "年—第" + Mid(strnum1, 5, 2) + "周已經存在！請刪除之後再保存。")
                    GridView.FocusedRowHandle = j
                    CheckDateEmpty = False
                    Exit Function
                End If
            Next
        Next
    End Function
    '判斷日期是本年的第幾周
    Private Function WeekOfYear(ByVal MI_NeedDate As Date) As Integer
        Return DatePart(DateInterval.WeekOfYear, MI_NeedDate)
    End Function

    '得到時間周數的起止日期
    Public Function GetDateCrossStr(ByVal dateSend As Date) As String
        Return mdlSetting.GetTimeSpan(dateSend)
    End Function

    ''' 客戶ID改變事件
    Private Sub txtMO_CusterID_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMO_CusterID.EditValueChanged
        Try
            Dim Mrplist As New List(Of MrpForecastOrderInfo)
            Mrplist = MrpCon.CusterGetName(txtMO_CusterID.Text, Nothing)
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "物料信息賦值"
    Private Sub GLU_MCode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GLU_MCode.EditValueChanged
        txt_M_Name.Text = GridView4.GetFocusedRowCellValue("M_Name")
        txt_M_Gauge.Text = GridView4.GetFocusedRowCellValue("M_Gauge")
        txt_M_Unit.Text = GridView4.GetFocusedRowCellValue("M_Unit")
        txt_M_Source.Text = GridView4.GetFocusedRowCellValue("M_Source")
    End Sub
#End Region

#Region "批量添加'"
    Private Sub tsm_MAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_MAdd.Click
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        If GridView.GetFocusedRowCellValue("MI_NeedDate").ToString = String.Empty Or IsDBNull(GridView.GetFocusedRowCellValue("MI_NeedDate").ToString) Then
            MsgBox("請先輸入需求日期！")
            Exit Sub
        End If
        If GridView.GetFocusedRowCellValue("MI_Qty").ToString = String.Empty Or IsDBNull(GridView.GetFocusedRowCellValue("MI_Qty").ToString) Then
            MsgBox("請先輸入訂單數量！")
            Exit Sub
        End If
        Dim AddTime As Integer
        Dim NeedStr As String
        Dim WeekNumber As Integer = 200
        NeedStr = InputBox("請輸入重複的次數:( 1 到 " + WeekNumber.ToString + " )", "批量添加", "")
        If NeedStr = String.Empty Then
            Exit Sub
        Else
            If Integer.TryParse(NeedStr, AddTime) = True Then
                If AddTime > WeekNumber Or AddTime < 1 Then
                    MsgBox("輸入的數字應在 1 到 " + WeekNumber.ToString + " 之間！")
                    tsm_MAdd_Click(Nothing, Nothing)
                End If

                Dim NeedDate As Date
                If IsDBNull(GridView.GetRow(0)("MI_NeedDate")) Then
                    NeedDate = Date.MinValue
                End If
                NeedDate = GridView.GetRow(0)("MI_NeedDate")
                For i As Integer = 1 To GridView.RowCount - 1
                    If IsDBNull(GridView.GetRow(i)("MI_NeedDate")) = False Then
                        If GridView.GetRow(i)("MI_NeedDate") > NeedDate Then
                            NeedDate = GridView.GetRow(i)("MI_NeedDate")
                        End If
                    End If
                Next
                Dim YearWeek As String
                Dim num As Int16
                For i As Integer = 0 To AddTime - 1
                    num = Weekday(NeedDate)
                    NeedDate = DateAdd(DateInterval.DayOfYear, 7, NeedDate)
                    'LastFirstDate = ( CDate(Year(NeedDate).ToString + "/1/1"),)
                    Dim row As DataRow = ds.Tables("MrpForecastOrderEntry").NewRow
                    row.ItemArray = GridView.GetDataRow(GridView.FocusedRowHandle).ItemArray()
                    Dim row1 As DataRow = ds.Tables("MrpForecastOrderEntry").NewRow
                    row1.ItemArray = GridView.GetDataRow(GridView.FocusedRowHandle).ItemArray()

                    If WeekOfYear(NeedDate) = 1 Then
                        StartDate = CDate(Year(NeedDate).ToString + "/1/1")
                        Dim EndDate As Date = CDate((Year(NeedDate) - 1).ToString + "/12/31")
                        row("MI_NeedDate") = NeedDate
                        row("MI_WeekNumber") = 1
                        YearWeek = Year(NeedDate).ToString + WeekOfYear(NeedDate).ToString
                        row("DateCross") = GetDateCrossStr(NeedDate)
                        row("AutoID") = 0
                        'If NeedDate = StartDate Then
                        If num <> 7 Then
                            row1("MI_WeekNumber") = 53
                            YearWeek = (Year(NeedDate) - 1).ToString + "53"
                            row1("DateCross") = GetDateCrossStr(EndDate)
                            row1("MI_NeedDate") = EndDate
                            row1("MI_Qty") = CInt(row1("MI_Qty")) * DatePart(DateInterval.Weekday, EndDate) / 7
                            row1("MI_Note") = "跨年周-前半周！！！"
                            row1("AutoID") = 0
                            ds.Tables("MrpForecastOrderEntry").Rows.Add(row1)
                            row("MI_Note") = "跨年周-后半周！！！"
                            row("MI_Qty") = row("MI_Qty") - row1("MI_Qty")
                        End If
                      
                        ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
                    Else
                        If WeekOfYear(NeedDate) = 53 Then
                            If num <> 7 Then
                                row("MI_NeedDate") = NeedDate
                                row("MI_WeekNumber") = WeekOfYear(NeedDate)
                                YearWeek = Year(NeedDate).ToString + WeekOfYear(NeedDate).ToString

                                row("DateCross") = GetDateCrossStr(NeedDate)
                                row("MI_Qty") = CInt(row("MI_Qty")) * DatePart(DateInterval.Weekday, NeedDate) / 7
                                row("MI_Note") = "跨年周-前半周！！！"
                                row("AutoID") = 0
                                ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
                                row1("MI_WeekNumber") = 1
                                YearWeek = (Year(NeedDate) + 1).ToString + "1"
                                row1("MI_NeedDate") = (Year(NeedDate) + 1).ToString + "/01/01"
                                row1("DateCross") = GetDateCrossStr(CDate(row1("MI_NeedDate")))
                                row1("MI_Qty") = row1("MI_Qty") - row("MI_Qty")
                                row1("MI_Note") = "跨年周-后半周！！！"
                                row1("AutoID") = 0
                                ds.Tables("MrpForecastOrderEntry").Rows.Add(row1)
                            Else
                                row("MI_NeedDate") = NeedDate
                                row("MI_WeekNumber") = WeekOfYear(NeedDate)
                                YearWeek = Year(NeedDate).ToString + WeekOfYear(NeedDate).ToString
                                row("DateCross") = GetDateCrossStr(NeedDate)
                                row("MI_Note") = String.Empty
                                row("AutoID") = 0
                                ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
                            End If
                        Else
                            row("MI_NeedDate") = NeedDate
                            row("MI_WeekNumber") = WeekOfYear(NeedDate)
                            YearWeek = Year(NeedDate).ToString + WeekOfYear(NeedDate).ToString
                            row("DateCross") = GetDateCrossStr(NeedDate)
                            row("MI_Note") = String.Empty
                            row("AutoID") = 0
                            ds.Tables("MrpForecastOrderEntry").Rows.Add(row)
                        End If
                    End If

                Next
                GridView.FocusedRowHandle = GridView.RowCount - 1
            Else
                MsgBox("請輸入正確的次數或者重複的周數！")
            End If
        End If

    End Sub
#End Region
   
    Private Sub RepositoryItemCalcEdit1_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles RepositoryItemCalcEdit1.EditValueChanging
        If (CDec(e.NewValue) > 999999999 Or CDec(e.NewValue) < 0) Then
            e.Cancel = True
        End If
        If GridView.FocusedColumn.FieldName = "MI_Qty" Then
            GridView.SetFocusedRowCellValue(MI_Qty, e.NewValue)
        Else
            Exit Sub
        End If
        Dim qty As Decimal = 0
        Try
            For i As Int16 = 0 To GridView.DataRowCount - 1
                If IsDBNull(GridView.GetDataRow(i)("MI_Qty")) = False Then
                    qty += Convert.ToDecimal(GridView.GetDataRow(i)("MI_Qty"))
                End If
            Next
            caeNeedQty.EditValue = qty
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub MI_NeedDateRIDE_EditValueChanging(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ChangingEventArgs) Handles MI_NeedDateRIDE.EditValueChanging
        Try
            Dim needDate As Date = CDate(e.NewValue)
            Dim YearWeek As String
            YearWeek = Year(e.NewValue).ToString + WeekOfYear(e.NewValue).ToString
            GridView.SetFocusedRowCellValue(GridView.Columns("MI_WeekNumber"), WeekOfYear(e.NewValue))
            GridView.SetFocusedRowCellValue(GridView.Columns("DateCross"), GetDateCrossStr(e.NewValue))
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tsm_Check_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_Check.Click
        Try
            If GridView.GetFocusedRowCellValue(GridView.Columns("IsCheck")) = True Then
                GridView.SetFocusedRowCellValue(GridView.Columns("IsCheck"), False)
            Else
                GridView.SetFocusedRowCellValue(GridView.Columns("IsCheck"), True)
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub
End Class
