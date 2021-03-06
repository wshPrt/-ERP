Imports LFERP.DataSetting
Imports LFERP.Library.Product
Imports LFERP.SystemManager.SystemUser
Imports LFERP.Library.SampleManager.SampleOrdersMain

Public Class frmSampleOrdersAdd
#Region "属性"
    Dim cc As New SampleOrdersMainControler
    Dim cco As New CusterControler
    Dim mc As New ProductController
    Dim sms As New SystemUserController
    Dim ds As New DataSet
    Dim strSO_ID As String

    Private _EditItem As String '属性栏位
    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
#End Region
    ''' <summary>
    ''' 创建表
    ''' </summary>
    ''' <remarks></remarks>
    Sub CreateTables()
        ds.Tables.Clear()
        With ds.Tables.Add("SampleOrders")
            .Columns.Add("AutoID", GetType(String))
            .Columns.Add("SO_ID", GetType(String))
            .Columns.Add("SO_CusterID", GetType(String))
            .Columns.Add("SO_CusterPO", GetType(String))
            .Columns.Add("SO_CusterNo", GetType(String))

            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("SO_SendDate", GetType(Date))
            .Columns.Add("SO_PoDate", GetType(String))
            .Columns.Add("SO_OrderQty", GetType(Integer))
            .Columns.Add("SO_NoSendQty", GetType(Integer))
            .Columns.Add("SO_Completion", GetType(String))
            .Columns.Add("SO_Remark", GetType(String))
            .Columns.Add("SO_Gauge", GetType(String))
            .Columns.Add("CO_ID", GetType(String))
            .Columns.Add("SO_State", GetType(String))
            .Columns.Add("SO_CheckDate", GetType(Date))
            .Columns.Add("SO_CheckUserID", GetType(String))
            .Columns.Add("SO_Check", GetType(Boolean))
            .Columns.Add("SO_AddUserID", GetType(String))
            .Columns.Add("SO_AddDate", GetType(Date))
            .Columns.Add("SO_ModifyUserID", GetType(String))
            .Columns.Add("SO_ModifyDate", GetType(Date))
            .Columns.Add("SO_Price", GetType(Double))
            .Columns.Add("SO_PriceUine", GetType(String))
            .Columns.Add("SO_No", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("SO_Rank", GetType(String))
            .Columns.Add("SO_CreateDate", GetType(String))
            .Columns.Add("M_Code_Type", GetType(String))
            .Columns.Add("TMaterialType", GetType(String))
            .Columns.Add("SO_OrdersType", GetType(String))
        End With
        Grid1.DataSource = ds.Tables("SampleOrders")

        With ds.Tables.Add("SampleOrdersDel")
            .Columns.Add("SO_No", GetType(String))
        End With

        With ds.Tables.Add("ProductSub") '子配件表
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_PID", GetType(String))
            .Columns.Add("M_KEY", GetType(String))
        End With
        Me.TreeList1.DataSource = ds.Tables("ProductSub")
    End Sub
    ''' <summary>
    ''' 傳回值
    ''' </summary>
    ''' <remarks></remarks>
    Sub Loadsampleorders()
        Dim cl As New List(Of SampleOrdersMainInfo)
        cl = cc.SampleOrdersMain_GetList(strSO_ID, Nothing, Nothing, Nothing, Nothing, Nothing, False)
        If cl.Count <= 0 Then
            Exit Sub
        End If
        '2013-11-05
        GridLookUpMaterialType.EditValue = cl(0).MaterialTypeID
        GridLookUpSampType.EditValue = cl(0).SampTypeID
        txt_OrdersType.EditValue = cl(0).SO_OrdersType
        Dim i As Integer
        For i = 0 To cl.Count - 1
            Dim row As DataRow
            row = ds.Tables("SampleOrders").NewRow

            row("AutoID") = cl(i).AutoID
            txtordersid.Text = cl(i).SO_ID
            txt_customerid.EditValue = cl(i).SO_CusterID
            txtSO_SampleID.Text = cl(i).SO_SampleID '李超新增
            txt_customerpo.Text = cl(i).SO_CusterPO
            dtp_podate.EditValue = cl(i).SO_PoDate
            txt_company.EditValue = cl(i).CO_ID


            row("SO_CusterNo") = cl(i).SO_CusterNo
            row("PM_M_Code") = cl(i).PM_M_Code
            row("M_Code") = cl(i).M_Code
            row("M_Name") = cl(i).M_Name
            row("SO_SendDate") = cl(i).SO_SendDate
            row("SO_PoDate") = cl(i).SO_PoDate
            row("SO_OrderQty") = cl(i).SO_OrderQty
            row("SO_NoSendQty") = cl(i).SO_NoSendQty
            row("SO_Completion") = cl(i).SO_Completion
            row("M_Code_Type") = cl(i).M_Code_Type
            row("TMaterialType") = cl(i).TMaterialType

            row("SO_Gauge") = cl(i).SO_Gauge
            txt_company.Text = cl(i).CO_ID
            row("SO_State") = cl(i).SO_State
            row("SO_CheckDate") = cl(i).SO_CheckDate
            row("SO_CheckUserID") = cl(i).SO_CheckUserID
            row("SO_Check") = cl(i).SO_Check
            CheckEdit1.Checked = IIf(cl(i).SO_Check, True, False)
            row("SO_AddUserID") = cl(i).SO_AddUserID
            row("SO_AddDate") = cl(i).SO_AddDate
            row("SO_ModifyUserID") = cl(i).SO_ModifyUserID
            row("SO_ModifyDate") = cl(i).SO_ModifyDate
            row("SO_Price") = cl(i).SO_Price
            row("SO_PriceUine") = cl(i).SO_PriceUine
            row("SO_No") = cl(i).SO_No
            row("SO_Remark") = cl(i).SO_Remark
            row("SO_Rank") = cl(i).SO_Rank
            row("SO_CreateDate") = cl(i).SO_CreateDate
            row("SO_OrdersType") = cl(i).SO_OrdersType
            ds.Tables("SampleOrders").Rows.Add(row)
        Next
    End Sub

    Function CheckDateEmpty() As Boolean
        '--------訂單主檔
        CheckDateEmpty = True
        If txtordersid.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入订单编号！", MsgBoxStyle.OkOnly, "提示")
            txtordersid.Focus()
            Exit Function
        End If
        If txtSO_SampleID.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入样办单号！", MsgBoxStyle.OkOnly, "提示")
            txtSO_SampleID.Focus()
            Exit Function
        End If
        If txt_customerid.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有选择客戶编号！", MsgBoxStyle.OkOnly, "提示")
            txt_customerid.Focus()
            Exit Function
        End If
        If txt_customerpo.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入客戶PO！", MsgBoxStyle.OkOnly, "提示")
            txt_customerpo.Focus()
            Exit Function
        End If
        If txt_company.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您沒有输入公司！", MsgBoxStyle.OkOnly, "提示")
            txt_company.Focus()
            Exit Function
        End If
        If ds.Tables("SampleOrders").Rows.Count <= 0 Then
            CheckDateEmpty = False
            MsgBox("您沒有输入产品信息！", MsgBoxStyle.OkOnly, "提示")
            Exit Function
        End If

        If GridLookUpMaterialType.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您没有选择材料类型！", MsgBoxStyle.OkOnly, "提示")
            GridLookUpMaterialType.Focus()
            Exit Function
        End If

        If GridLookUpSampType.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您没有选择样办階段！", MsgBoxStyle.OkOnly, "提示")
            GridLookUpSampType.Focus()
            Exit Function
        End If
        If txt_OrdersType.Text = String.Empty Then
            CheckDateEmpty = False
            MsgBox("您没有选择訂單类型！", MsgBoxStyle.OkOnly, "提示")
            txt_OrdersType.Focus()
            Exit Function
        End If
        If ds.Tables("SampleOrders").Rows.Count > 1 Then
            CheckDateEmpty = False
            MsgBox("订单明细子表只能新增一笔！", MsgBoxStyle.OkOnly, "提示")
            Exit Function
        End If
        '--------訂單子檔
        Dim i As Integer
        For i = 0 To ds.Tables("SampleOrders").Rows.Count - 1

            If ds.Tables("SampleOrders").Rows(i)("PM_M_Code").ToString() = String.Empty Then
                CheckDateEmpty = False
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                MsgBox("产品编号不能为空！", MsgBoxStyle.OkOnly, "提示")
                Exit Function
            End If

            If EditItem = EditEnumType.ADD Then
                Dim cl As New List(Of SampleOrdersMainInfo)
                cl = cc.SampleOrdersMain_GetList(Nothing, Nothing, ds.Tables("SampleOrders").Rows(i)("PM_M_Code").ToString(), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                If cl.Count <= 0 Then
                Else
                    CheckDateEmpty = False
                    Grid1.Focus()
                    GridView1.FocusedRowHandle = i
                    MsgBox("产品编号," & ds.Tables("SampleOrders").Rows(i)("PM_M_Code").ToString() & "已在其它订单中录入！", MsgBoxStyle.OkOnly, "提示")
                    Exit Function
                End If
            End If

            If ds.Tables("SampleOrders").Rows(i)("M_Name").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("配件名称不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If

            If ds.Tables("SampleOrders").Rows(i)("SO_SendDate").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("交货日期不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If

            If ds.Tables("SampleOrders").Rows(i)("SO_OrderQty").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("订单数量不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If

            If ds.Tables("SampleOrders").Rows(i)("M_Code_Type").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("产品类别不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If
            If ds.Tables("SampleOrders").Rows(i)("TMaterialType").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("材料不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If
            If ds.Tables("SampleOrders").Rows(i)("SO_Rank").ToString() = String.Empty Then
                CheckDateEmpty = False
                MsgBox("订单等级不能为空！", MsgBoxStyle.OkOnly, "提示")
                Grid1.Focus()
                GridView1.FocusedRowHandle = i
                Exit Function
            End If

        Next
    End Function

    Private Sub Savedate()

        Dim objinfo As New SampleOrdersMainInfo()
        Dim str As String
        objinfo.CO_ID = txt_company.EditValue
        objinfo.PM_M_Code = txtordersid.Text
        objinfo.SO_CusterID = txt_customerid.EditValue
        objinfo.SO_SampleID = txtSO_SampleID.Text
        objinfo.SO_CusterPO = txt_customerpo.Text
        objinfo.SO_ID = txtordersid.Text
        objinfo.SO_PoDate = dtp_podate.EditValue
        objinfo.SO_AddUserID = InUserID
        objinfo.SO_AddDate = Format(Now, "yyyy/MM/dd")
        objinfo.SO_OrdersType = txt_OrdersType.EditValue
        '2013-11-05
        objinfo.MaterialTypeID = GridLookUpMaterialType.EditValue
        objinfo.SampTypeID = GridLookUpSampType.EditValue


        If EditItem = EditEnumType.ADD Then
            str = GetOrdersID()
            If str <> txtordersid.EditValue Then
                MsgBox(txtordersid.EditValue & " 订单编号已经被占用，订单编号將变更為" + str, MsgBoxStyle.OkOnly, "提示")
                objinfo.SO_ID = str
            End If
        End If

        Dim j As Integer
        For j = 0 To ds.Tables("SampleOrders").Rows.Count - 1
            objinfo.SO_No = ds.Tables("SampleOrders").Rows(j)("SO_No").ToString
            objinfo.PM_M_Code = ds.Tables("SampleOrders").Rows(j)("PM_M_Code").ToString
            objinfo.M_Code = ds.Tables("SampleOrders").Rows(j)("M_Code").ToString
            objinfo.SO_SendDate = Format(CDate(ds.Tables("SampleOrders").Rows(j)("SO_SendDate")), "yyyy/MM/dd")
            objinfo.SO_OrderQty = ds.Tables("SampleOrders").Rows(j)("SO_OrderQty").ToString

            objinfo.SO_No = ds.Tables("SampleOrders").Rows(j)("SO_No").ToString
            objinfo.SO_Remark = ds.Tables("SampleOrders").Rows(j)("SO_Remark").ToString
            objinfo.SO_Rank = ds.Tables("SampleOrders").Rows(j)("SO_Rank").ToString
            objinfo.SO_Gauge = ds.Tables("SampleOrders").Rows(j)("SO_Gauge").ToString

            objinfo.SO_CusterNo = ds.Tables("SampleOrders").Rows(j)("SO_CusterNo").ToString
            objinfo.SO_NoSendQty = ds.Tables("SampleOrders").Rows(j)("SO_OrderQty").ToString

            objinfo.M_Code_Type = ds.Tables("SampleOrders").Rows(j)("M_Code_Type").ToString
            objinfo.TMaterialType = ds.Tables("SampleOrders").Rows(j)("TMaterialType").ToString


            objinfo.SO_AddUserID = InUserID

            Select Case EditItem
                Case EditEnumType.ADD
                    objinfo.SO_No = GetOrdersNo()
                    If cc.SampleOrdersMain_Add(objinfo) = False Then
                        MsgBox(ds.Tables("SampleOrders").Rows(j)("PM_M_Code") & "，请检查原因！", 60, "提示")
                        Exit Sub
                    End If
                Case EditEnumType.EDIT
                    objinfo.SO_ModifyDate = Format(Now, "yyyy/MM/dd")
                    objinfo.SO_ModifyUserID = InUserID
                    If ds.Tables("SampleOrders").Rows(j)("AutoID").ToString <> "" Then
                        If cc.SampleOrdersMain_Update(objinfo) = False Then
                            MsgBox(ds.Tables("SampleOrders").Rows(j)("PM_M_Code") & "，请检查原因！", 60, "提示")
                            Exit Sub
                        End If
                    Else
                        objinfo.SO_No = GetOrdersNo()
                        If cc.SampleOrdersMain_Add(objinfo) = False Then
                            MsgBox(ds.Tables("SampleOrders").Rows(j)("PM_M_Code") & "，请检查原因！", 60, "提示")
                            Exit Sub
                        End If
                    End If
            End Select
        Next

        '刪除記錄
        For j = 0 To ds.Tables("SampleOrdersDel").Rows.Count - 1
            If cc.SampleOrdersMain_Delete(ds.Tables("SampleOrdersDel").Rows(j)("SO_No")) = False Then
                MsgBox("刪除当前选定记录失敗，请检查原因！", 60, "提示")
                Exit Sub
            End If
        Next
        MsgBox("保存成功!")
        Me.Close()

    End Sub

    Private Sub UpdateCheck()
        Dim objinfo As New SampleOrdersMainInfo()
        objinfo.SO_CheckRemark = txt_checkremark.Text
        objinfo.SO_CheckUserID = InUserID
        objinfo.SO_ID = txtordersid.Text
        objinfo.SO_Check = CheckEdit1.Checked
        objinfo.SO_CheckDate = Date.Today

        If cc.SampleOrdersMain_UpdateCheck(objinfo) = False Then
            MsgBox(txtordersid.Text & "，请检查原因！", 60, "提示")
        End If
        MsgBox("保存成功!")
        Me.Close()
    End Sub

    Private Sub frmSampleOrdersAdd_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTables()
        RepositoryItemGridLookUpEdit1.DisplayMember = "PM_M_Code"
        RepositoryItemGridLookUpEdit1.ValueMember = "PM_M_Code"
        RepositoryItemGridLookUpEdit1.DataSource = mc.Product_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)


        txt_customerid.Properties.DisplayMember = "C_ChsName"    'txt
        txt_customerid.Properties.ValueMember = "C_CusterID"   'EditValue
        txt_customerid.Properties.DataSource = cco.GetCusterList(Nothing, Nothing, Nothing)

        txt_company.Properties.DisplayMember = "CO_No"
        txt_company.Properties.ValueMember = "CO_ID"
        txt_company.Properties.DataSource = cc.CompanyUnion_GetList(Nothing)

        Grid2.DataSource = mc.Product_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        NotsIN() '控件载入事件

        strSO_ID = tempValue2
        tempValue2 = Nothing

        'EditItem = tempValue
        'tempValue = Nothing


        '載入材料類型/樣辦類型
        GridLookUpMaterialType.Properties.DisplayMember = "TName"
        GridLookUpMaterialType.Properties.ValueMember = "TID"

        GridLookUpSampType.Properties.DisplayMember = "TName"
        GridLookUpSampType.Properties.ValueMember = "TID"



        ride_senddate.MaxValue = Date.Today.AddDays(356)
        ride_senddate.MinValue = Date.Today

        txt_company.EditValue = "DGMG"

        Select Case EditItem
            Case EditEnumType.ADD
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.ADD)
                XtraTabPage2.PageVisible = False
                txtordersid.Text = GetOrdersID()
                dtp_podate.EditValue = Format(Now, "yyyy/MM/dd")
                txtSO_SampleID.Enabled = True
                cmdSampType.Enabled = True
                cmdMaterialType.Enabled = True
                txt_OrdersType.SelectedIndex = 0
            Case EditEnumType.EDIT
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.EDIT)
                Loadsampleorders()
                Me.Text = "样版订单修改"
                txtordersid.Enabled = False
                XtraTabPage2.PageVisible = False
                ToolStripMenuItem1.Enabled = False
                ToolStripMenuItem2.Enabled = False
                PM_M_Code.OptionsColumn.AllowEdit = False
                M_Code.OptionsColumn.AllowEdit = False
                cmdSampType.Enabled = True
                cmdMaterialType.Enabled = True
                txtSO_SampleID.Enabled = True
                SetCustomeDate(txt_customerid.EditValue)
            Case EditEnumType.VIEW
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.VIEW)
                Me.Grid1.ContextMenuStrip = Nothing
                Loadsampleorders()
                txt_OrdersType.Enabled = False
                Savebutton.Visible = False
                txtordersid.Enabled = False
                txt_customerid.Enabled = False
                txt_customerpo.Enabled = False
                txt_company.Enabled = False
                dtp_podate.Enabled = False
                GridLookUpSampType.Enabled = False
                GridLookUpMaterialType.Enabled = False
                Me.Text = "样版订单查看"
                txtordersid.Enabled = False
                XtraTabPage2.PageVisible = False
                txtSO_SampleID.Enabled = False
                cmdSampType.Enabled = False
                cmdMaterialType.Enabled = False
                SetCustomeDate(txt_customerid.EditValue)

                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False

            Case EditEnumType.CHECK
                Me.lblTitle.Text = Me.Text + EditEnumValue(EditEnumType.CHECK)
                Me.Grid1.ContextMenuStrip = Nothing
                lbl_checkdate.Text = Format(Now, "yyyy/MM/dd")
                lbl_checkuser.Text = sms.SystemUser_Get(InUserID).U_Name
                dtp_podate.Enabled = False
                Loadsampleorders()
                Me.Text = "样版订单审核"
                XtraTabControl1.SelectedTabPage = XtraTabPage2
                txt_OrdersType.Enabled = False
                Savebutton.Enabled = False
                txtordersid.Enabled = False
                txt_customerid.Enabled = False
                txt_customerpo.Enabled = False
                txt_company.Enabled = False
                GridLookUpSampType.Enabled = False
                GridLookUpMaterialType.Enabled = False
                txtSO_SampleID.Enabled = False
                cmdSampType.Enabled = False
                cmdMaterialType.Enabled = False
                SetCustomeDate(txt_customerid.EditValue)

                GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                GridView1.OptionsBehavior.Editable = False
                GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
        End Select
        Me.Text = lblTitle.Text
    End Sub

    Public Sub NotsIN()
        If EditItem = EditEnumType.VIEW Then
            GridLookUpMaterialType.Properties.DataSource = cc.SampleOrdersType_GetList(Nothing, "M", Nothing, Nothing)
            GridLookUpSampType.Properties.DataSource = cc.SampleOrdersType_GetList(Nothing, "T", Nothing, Nothing)
        Else
            GridLookUpMaterialType.Properties.DataSource = cc.SampleOrdersType_GetList(Nothing, "M", "True", Nothing)
            GridLookUpSampType.Properties.DataSource = cc.SampleOrdersType_GetList(Nothing, "T", "True", Nothing)
        End If
    End Sub


    Private Sub CheckEdit1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If CheckEdit1.Checked = True Then
            Savebutton.Enabled = True
        Else
            Savebutton.Enabled = False
        End If
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        If ds.Tables("SampleOrders").Rows.Count < 1 Then
            Dim row As DataRow
            row = ds.Tables("SampleOrders").NewRow
            ds.Tables("SampleOrders").Rows.Add(row)
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        If GridView1.RowCount > 0 Then

            Dim deltemp As String = ""
            deltemp = ds.Tables("SampleOrders").Rows(GridView1.FocusedRowHandle)("SO_No").ToString
            ds.Tables("SampleOrders").Rows.RemoveAt(GridView1.FocusedRowHandle)

            Dim row As DataRow
            If deltemp <> "" Then
                row = ds.Tables("SampleOrdersDel").NewRow
                row("SO_No") = deltemp
                ds.Tables("SampleOrdersDel").Rows.Add(row)
            End If
        End If
    End Sub

    Private Sub CheckEdit1_CheckedChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit1.CheckedChanged
        Savebutton.Enabled = True
    End Sub

    Private Sub Savebutton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Savebutton.Click
        Select Case EditItem
            Case EditEnumType.CHECK
                UpdateCheck()
            Case EditEnumType.EDIT
                If CheckDateEmpty() = False Then
                    Exit Sub
                End If
                Savedate()
            Case EditEnumType.ADD
                If CheckDateEmpty() = False Then
                    Exit Sub
                End If
                Savedate()
        End Select
    End Sub

    Private Sub CloseButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseButton.Click
        Me.Close()
    End Sub

    Private Sub dtp_podate_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtp_podate.EditValueChanged
        If tempValue = "Add" Then
            If dtp_podate.EditValue < Date.Today And dtp_podate.EditValue <> Nothing Then
                MsgBox("日期不能小于当前日期！！！")
                dtp_podate.EditValue = Date.Today
            End If
            If dtp_podate.EditValue > Date.Today.AddDays(365) Then
                MsgBox("日期不能大于当前一年！！！")
                dtp_podate.EditValue = Date.Today.AddDays(365)
            End If
        End If
    End Sub


    Private Sub Grid2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Grid2.Click
        If GridView3.RowCount > 0 Then
            GridView1.SetFocusedRowCellValue(Me.PM_M_Code, GridView3.GetFocusedRowCellValue("PM_M_Code"))
            GridView1.SetFocusedRowCellValue(Me.AutoID, GridView3.GetFocusedRowCellValue("AutoID"))
            GridView1.SetFocusedRowCellValue(Me.SO_CusterNo, GridView3.GetFocusedRowCellValue("PM_CusterNO"))
            Dim pbiL As List(Of ProductBomInfo)
            Dim pbc As New ProductBomController
            pbiL = pbc.ProductBom_GetList(GridView3.GetFocusedRowCellValue("PM_M_Code"), Nothing, Nothing, Nothing, Nothing, Nothing)

            SubRowAdd(pbiL)
        End If
        GridView1.Focus()
    End Sub

    Private Sub SubRowAdd(ByVal pbil As List(Of ProductBomInfo))
        Dim pc As New ProductController
        Dim piL As List(Of ProductInfo)
        Dim pmmcode As String
        pmmcode = ds.Tables("SampleOrders").Rows(GridView1.FocusedRowHandle)("PM_M_Code").ToString
        piL = pc.Product_GetList(pmmcode, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        ds.Tables("ProductSub").Clear()
        Dim row As DataRow
        row = ds.Tables("ProductSub").NewRow

        row("M_Name") = pmmcode
        row("M_PID") = "0~"
        If pbil.Count > 0 Then
            row("M_Code") = piL(0).PM_M_Code
            row("M_KEY") = pbil(0).PM_PID
        Else
            row("M_Code") = pmmcode
            row("M_KEY") = ""
        End If

        ds.Tables("ProductSub").Rows.Add(row)
        For i As Integer = 0 To pbil.Count - 1
            row = ds.Tables("ProductSub").NewRow
            row("M_Name") = pbil(i).M_Name
            row("M_Code") = pbil(i).M_Code
            row("M_PID") = pbil(i).PM_PID
            row("M_KEY") = pbil(i).PM_Key
            ds.Tables("ProductSub").Rows.Add(row)
        Next
        Me.TreeList1.ExpandAll()
    End Sub

    Private Sub TreeList1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles TreeList1.MouseDoubleClick
        Try
            Dim strName, StrM_Code As String
            StrM_Code = TreeList1.FocusedNode.GetValue("M_Code")
            strName = TreeList1.FocusedNode.GetValue("M_Name")

            GridView1.SetFocusedRowCellValue(M_Code, StrM_Code)
            GridView1.SetFocusedRowCellValue(M_Name, strName)
        Catch
        End Try
    End Sub

    Public Function GetOrdersID() As String
        Dim ordersNo As String
        Dim SoyyMM As String
        SoyyMM = "SAM" & Format(Now, "yyMM")

        ordersNo = cc.SampleOrdersMain_GetOrdersID(SoyyMM)
        If ordersNo = "" Then
            ordersNo = Trim(SoyyMM & "001")
        Else
            ordersNo = SoyyMM & Format(Val(Microsoft.VisualBasic.Right(ordersNo, 3)) + 1, "000")
        End If
        Return ordersNo
    End Function

    Public Function GetOrdersNo() As String
        Dim ordersNo As String
        Dim SoyyMM As String
        SoyyMM = "SO" & Format(Now, "yyyy")

        ordersNo = cc.SampleOrdersMain_GetSoNo()
        If ordersNo = "" Then
            ordersNo = Trim(SoyyMM & "00001")
        Else
            ordersNo = SoyyMM & Format(Val(Microsoft.VisualBasic.Right(ordersNo, 5)) + 1, "00000")
        End If
        Return ordersNo
    End Function

    Private Sub txt_customerid_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_customerid.EditValueChanged
        Dim culist As New List(Of CusterInfo)
        If sender.EditValue = String.Empty Then
            Exit Sub
        End If
        SetCustomeDate(sender.EditValue)
    End Sub

    Sub SetCustomeDate(ByVal strCusterID As String)
        Dim culist As New List(Of CusterInfo)
        If strCusterID = String.Empty Then
            Exit Sub
        End If
        Me.txtDepartment.Text = String.Empty
        Me.txtAddress1.Text = String.Empty
        Me.txtContacts.Text = String.Empty
        Me.txtDepartment.Text = String.Empty
        Me.txtEmail.Text = String.Empty

        culist = cco.GetCusterList(strCusterID, Nothing, Nothing)
        If culist.Count = 1 Then
            Me.txtDepartment.Text = culist(0).C_LinkTel
            Me.txtAddress1.Text = culist(0).C_Adder1
            Me.txtContacts.Text = culist(0).C_Link
            Me.txtDepartment.Text = culist(0).C_Department
            Me.txtEmail.Text = culist(0).C_Email
        End If
    End Sub

    Private Sub cmdSampType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSampType.Click, cmdMaterialType.Click
        Dim fr As New frmSampleOrdersFind
        fr = New frmSampleOrdersFind
        fr.StartPosition = FormStartPosition.Manual
        fr.Left = MDIMain.Left + MDIMain.tvModule.Width + Me.Left + sender.Left
        fr.Top = MDIMain.Top + Me.Top + sender.Top + sender.Height + 120
        fr.EditItem = sender.Name
        fr.ShowDialog()
        NotsIN()
    End Sub

    'Private Sub txtSO_SampleID_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSO_SampleID.EditValueChanged
    '    If EditItem = "Add" Then
    '        If txtSO_SampleID.EditValue <> String.Empty Then
    '            Dim solist As New List(Of SampleOrdersMainInfo)
    '            solist = cc.SampleOrdersMain_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, txtSO_SampleID.EditValue)
    '            If solist.Count > 0 Then
    '                txtSO_SampleID.Text = String.Empty
    '                txtSO_SampleID.Focus()
    '                MsgBox("樣辦單號只能唯一,請重新輸入！")
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Sub txtEmail_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmail.EditValueChanged

    'End Sub
    'Private Sub lblDepartment_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDepartment.Click

    'End Sub
    'Private Sub txtDepartment_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDepartment.EditValueChanged

    'End Sub

    Private Sub lblContacts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblContacts.Click

    End Sub

    Private Sub txtContacts_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtContacts.EditValueChanged

    End Sub
End Class