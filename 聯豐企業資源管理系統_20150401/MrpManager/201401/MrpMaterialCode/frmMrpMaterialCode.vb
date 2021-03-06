Imports LFERP.SystemManager.SystemUser
Imports LFERP.Library.MrpManager.MrpMaterialCode
Imports LFERP.Library.MrpManager.MrpWareHouseInfo
Public Class frmMrpMaterialCode

#Region "屬性"
    Dim MWHIEcon As New MrpWareHouseInfoEntryController
    Dim MMICcon As New MrpMaterialCodeController
    Dim sms As New SystemUserController
    Dim MMCI As New MrpMaterialCodeInfo
    Private _EditItem As String '屬性欄位
    Private _EditValue As String
    Private _M_Code_List As List(Of String)
    Property EditItem() As String '屬性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
    Property EditValue() As String '屬性
        Get
            Return _EditValue
        End Get
        Set(ByVal value As String)
            _EditValue = value
        End Set
    End Property
    Property M_Code_List() As List(Of String) '屬性
        Get
            Return _M_Code_List
        End Get
        Set(ByVal value As List(Of String))
            _M_Code_List = value
        End Set
    End Property
#End Region

#Region "載入事件"
    ''' <summary>
    ''' 載入事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMrpMaterialCode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        MrpMaterialCodeInfoBindingSource.DataSource = MMCI '控件enable屬性綁定MrpMaterialCodeInfo的IsEnable屬性
        '產品信息
        gueM_Code.Properties.DisplayMember = "M_Code"    'txt
        gueM_Code.Properties.ValueMember = "M_Code"   'EditValue
        gueM_Code.Properties.DataSource = MMICcon.MaterialCode_GetList(Nothing)
        '倉庫信息
        gueMC_WH_ID.Properties.DisplayMember = "MC_WH_ID"    'txt
        gueMC_WH_ID.Properties.ValueMember = "MC_WH_ID"   'EditValue
        gueMC_WH_ID.Properties.DataSource = MMICcon.MrpMaterialCode_GetWareHouseInfo(Nothing)
        '來源別
        gueSource.Properties.DisplayMember = "MC_Source"    'txt
        gueSource.Properties.ValueMember = "MC_SourceID"   'EditValue
        gueSource.Properties.DataSource = MMICcon.MrpSource_GetList(Nothing, Nothing)
        Select Case EditItem
            Case "Add"
                gueM_Code.Enabled = True
                EnableOrDisable(True)
                'gueSource.EditValue = "P"
            Case "Edit"
                EnableOrDisable(True)
                LoadData(EditValue) '載入數據
                txtModifyUserID.Text = sms.SystemUser_Get(InUserID).U_Name
                txtModifyDate.Text = Format(System.DateTime.Now, "yyyy/MM/dd")
            Case "Look"
                'Panel1.Visible = True
                EnableOrDisable(False)
                LoadData(EditValue)
                cmdSave.Visible = False
            Case "Check"
                'Panel1.Visible = True
                EnableOrDisable(False)
                LoadData(EditValue)
                lblCheckUserID.Text = sms.SystemUser_Get(InUserID).U_Name
                lblCheckDate.Text = Format(System.DateTime.Now, "yyyy/MM/dd")
                chkCheckBit.Enabled = True
                cmdSave.Enabled = False
        End Select
    End Sub
#End Region

#Region "添加數據"
    ''' <summary>
    ''' 添加數據
    ''' </summary>
    ''' <remarks></remarks>
    Sub DataAdd()
        Dim MMCI As New MrpMaterialCodeInfo
        MMCI.M_Code = gueM_Code.EditValue
        MMCI.MC_BatchQty = caeMC_BatchQty.EditValue
        MMCI.MC_QtyMax = caeMC_QtyMax.EditValue
        MMCI.MC_QtyMin = caeMC_QtyMin.EditValue
        MMCI.MC_BatFixEconomy = caeMC_BatFixEconomy.EditValue
        MMCI.MC_WH_ID = gueMC_WH_ID.EditValue
        MMCI.MC_SecInv = caeMC_SecInv.EditValue
        MMCI.MC_OrderInterVal = caeMC_OrderInterVal.EditValue
        MMCI.MC_LowLimit = caeMC_LowLimit.EditValue
        MMCI.MC_Source = gueSource.EditValue
        MMCI.MC_OrderMan = txtMC_OrderMan.EditValue
        MMCI.MC_MRPCon = chkMC_MRPCon.Checked
        MMCI.CheckBit = chkCheckBit.Checked
        MMCI.CheckUserID = InUserID
        MMCI.CheckRemark = meeMC_Remark.EditValue
        MMCI.CreateUserID = InUserID
        MMCI.CreateDate = Format(System.DateTime.Now, "yyyy/MM/dd")
        MMCI.MC_OrderRemark = meeOrderRemark.EditValue
        MMCI.MC_WareHouseRemark = meeWareHouseRemark.EditValue
        MMCI.MC_Remark = meeMC_Remark.EditValue
        If MMICcon.MrpMaterialCode_Insert(MMCI) = True Then
            MsgBox("保存成功！", 60, "提示")
            Me.Close()
            Exit Sub
        Else
            MsgBox("保存失敗，請檢查原因！", 60, "提示")
            Me.Close()
            Exit Sub
        End If
    End Sub
#End Region

#Region "編輯數據"
    ''' <summary>
    ''' 編輯數據
    ''' </summary>
    ''' <remarks></remarks>
    Sub DataEdit()
        Dim MMCI As New MrpMaterialCodeInfo
        MMCI.M_Code = gueM_Code.Text
        MMCI.MC_BatchQty = caeMC_BatchQty.Text
        MMCI.MC_QtyMax = caeMC_QtyMax.Text
        MMCI.MC_QtyMin = caeMC_QtyMin.Text
        MMCI.MC_BatFixEconomy = caeMC_BatFixEconomy.Text
        MMCI.MC_WH_ID = gueMC_WH_ID.Text
        MMCI.MC_SecInv = caeMC_SecInv.Text
        MMCI.MC_OrderInterVal = caeMC_OrderInterVal.Text
        MMCI.MC_LowLimit = caeMC_LowLimit.Text
        MMCI.MC_Source = gueSource.EditValue
        MMCI.MC_OrderMan = txtMC_OrderMan.Text
        MMCI.MC_MRPCon = chkMC_MRPCon.Checked
        MMCI.ModifyUserID = InUserID
        MMCI.ModifyDate = Format(System.DateTime.Now, "yyyy/MM/dd")
        MMCI.MC_OrderRemark = meeOrderRemark.Text
        MMCI.MC_WareHouseRemark = meeWareHouseRemark.Text
        MMCI.MC_Remark = meeMC_Remark.Text
        If MMICcon.MrpMaterialCode_Update(MMCI) = True Then
            MsgBox("保存成功！", 60, "提示")
            Me.Close()
            Exit Sub
        Else
            MsgBox("保存失敗，請檢查原因！", 60, "提示")
            Me.Close()
            Exit Sub
        End If
    End Sub
#End Region

#Region "審核數據"
    Sub DataCheck()
        Dim MMCI As New MrpMaterialCodeInfo
        MMCI.M_Code = gueM_Code.Text
        MMCI.CheckUserID = InUserID
        MMCI.CheckBit = chkCheckBit.Checked
        MMCI.CheckRemark = meeMC_Remark.Text
        If MMICcon.MrpMaterialCode_UpdateCheck(MMCI) = True Then
            MsgBox("保存成功!", 60, "提示")
            Me.Close()
            Exit Sub
        Else
            MsgBox("審核失敗，請檢查原因！", 60, "提示")
            Me.Close()
            Exit Sub
        End If
    End Sub
#End Region

#Region "判空"
    Function DataCheckEmpty() As Integer
        If gueM_Code.Text = String.Empty Then
            MsgBox("產品編號不能為空,請輸入！", MsgBoxStyle.Information, "提示")
            gueM_Code.Focus()
            DataCheckEmpty = 0
            Exit Function
        End If
        If Me.gueSource.Text = String.Empty Then
            MsgBox("來源別不能為空,請輸入！", MsgBoxStyle.Information, "提示")
            gueSource.Focus()
            DataCheckEmpty = 0
            Exit Function
        End If
        DataCheckEmpty = 1
    End Function
#End Region

#Region "保存/取消事件"
    ''' <summary>
    ''' 保存
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If DataCheckEmpty() = 0 Then
            Exit Sub
        End If
        Select Case EditItem
            Case "Add"
                DataAdd()
            Case "Edit"
                DataEdit()
            Case "Check"
                DataCheck()
        End Select
    End Sub
    ''' <summary>
    ''' 取消
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub
#End Region

#Region "載入數據"
    Sub LoadData(ByVal StrM_Code As String)
        '-----------------------------------------------
        Dim MWHI_list As New List(Of MrpMaterialCodeInfo)
        MWHI_list = MMICcon.MrpMaterialCode_GetList(StrM_Code, Nothing, Nothing, Nothing, Nothing)
        If MWHI_list.Count = 0 Then
            Exit Sub
        Else
            gueM_Code.Text = MWHI_list(0).M_Code
            caeMC_BatchQty.Text = MWHI_list(0).MC_BatchQty
            caeMC_QtyMax.Text = MWHI_list(0).MC_QtyMax
            caeMC_QtyMin.Text = MWHI_list(0).MC_QtyMin
            caeMC_BatFixEconomy.Text = MWHI_list(0).MC_BatFixEconomy
            gueMC_WH_ID.Text = MWHI_list(0).MC_WH_ID
            caeMC_SecInv.Text = MWHI_list(0).MC_SecInv
            caeMC_OrderInterVal.Text = MWHI_list(0).MC_OrderInterVal
            caeMC_LowLimit.Text = MWHI_list(0).MC_LowLimit
            gueSource.EditValue = MWHI_list(0).MC_SourceID
            txtMC_OrderMan.Text = MWHI_list(0).MC_OrderMan
            chkMC_MRPCon.Checked = MWHI_list(0).MC_MRPCon
            chkCheckBit.Checked = MWHI_list(0).CheckBit
            lblCheckUserID.Text = MWHI_list(0).CheckUserID
            txtM_Gauge.Text = MWHI_list(0).M_Gauge
            txtM_Name.Text = MWHI_list(0).M_Name
            txtM_Unit.Text = MWHI_list(0).M_Unit
            If MWHI_list(0).ModifyUserID.ToString = String.Empty Then
                txtModifyUserID.Text = MWHI_list(0).ModifyUserID.ToString
                txtModifyDate.Text = String.Empty
            Else
                txtModifyUserID.Text = sms.SystemUser_Get(InUserID).U_Name
                txtModifyDate.Text = MWHI_list(0).ModifyDate
            End If
            meeMC_Remark.Text = MWHI_list(0).MC_Remark
            meeOrderRemark.Text = MWHI_list(0).MC_OrderRemark
            meeWareHouseRemark.Text = MWHI_list(0).MC_WareHouseRemark
            meeCheckRemak.Text = MWHI_list(0).CheckRemark
        End If
    End Sub
#End Region

#Region "checkbox改變時才能保存審核信息"

    ''' <summary>
    ''' checkbox改變時才能保存審核信息
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkCheckBit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCheckBit.CheckedChanged
        cmdSave.Enabled = True
    End Sub

#End Region

#Region "控制控件enable屬性--ture/false"
    ''' <summary>
    ''' 控制控件enable屬性--ture/false
    ''' </summary>
    ''' <param name="state"></param>
    ''' <remarks></remarks>
    Sub EnableOrDisable(ByVal state As Boolean)
        MMCI.IsEnabled = state
        MrpMaterialCodeInfoBindingSource.ResetCurrentItem()
    End Sub
#End Region

#Region "通過M_Code獲得其他信息"

    Private Sub GridView3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView3.Click
        txtM_Name.Text = GridView3.GetFocusedRowCellValue("M_Name")
        txtM_Gauge.Text = GridView3.GetFocusedRowCellValue("M_Gauge")
        txtM_Unit.Text = GridView3.GetFocusedRowCellValue("M_Unit")
    End Sub

#End Region

    Private Sub gueM_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gueM_Code.EditValueChanged
        'MsgBox("該產品編碼已添加，請重新選擇！")
        If EditItem = "Add" Then
            If gueM_Code.EditValue = String.Empty Then
            Else
                Select Case Mid(gueM_Code.EditValue, 1, 2)
                    Case "MG"
                        gueSource.EditValue = "C"
                    Case "30"
                        gueSource.EditValue = "T"
                    Case Else
                        gueSource.EditValue = "P"
                End Select

                For i As Integer = 0 To M_Code_List.Count - 1
                    If (gueM_Code.Text = M_Code_List(i)) Then
                        MsgBox("該產品編碼已添加，請重新選擇！", MsgBoxStyle.Information, "提示")
                        gueM_Code.EditValue = String.Empty
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub
End Class