Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.Product
Imports LFERP.Library.MrpManager.MrpPurchaseRecord
Imports LFERP.Library.MrpManager.MrpSetting
Imports LFERP.Library.MrpManager.MrpSupplierQuotation
Imports LFERP.Library.MrpManager.MrpPurchaseOrder
Imports LFERP.Library.MrpManager.MrpMaterialCode
Imports LFERP.Library.MrpManager.MrpSelect

Public Class frmMrpPurchaseRecordMain
#Region "屬性"
    Dim mpreCon As New MrpPurchaseRecordEntryController
    Dim mprCon As New MrpPurchaseRecordController
    'Dim msqCon As New MrpSupplierQuotationController 
    Dim mpoc As New MrpPurchaseOrderController
    Dim mpoec As New MrpPurchaseOrderEntryController
    Dim mmcc As New MrpMaterialCodeController
#End Region

#Region "窗體載入事件"
    Private Sub frmMrpPurchaseRecordMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        tsmRefresh_Click(Nothing, Nothing)
    End Sub
#End Region

#Region "新增事件"
    Private Sub tsmNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNew.Click
        On Error Resume Next
        Dim fr As frmMrpPurchaseRecord
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpPurchaseRecord Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpPurchaseRecord
        fr.EditItem = EditEnumType.ADD
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

#End Region

#Region "修改事件"
    Private Sub tsmEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmEdit.Click
        On Error Resume Next
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If

        Dim StrMrpPurchaseID As String = GridView1.GetFocusedRowCellValue("MrpPurchaseID").ToString()
        If mprCon.MrpPurchaseRecord_GetList(StrMrpPurchaseID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = True Then
            MsgBox("審核狀態無法修改！")
            Exit Sub
        End If

        Dim fr As frmMrpPurchaseRecord
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpPurchaseRecord Then
                fr.Activate()
                Exit Sub
            End If
        Next

        fr = New frmMrpPurchaseRecord
        fr.EditItem = EditEnumType.EDIT
        fr.MrpPurchaseID = StrMrpPurchaseID
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "刪除事件"
    Private Sub tsmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDelete.Click
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        Dim strMrpPurchaseID As String = GridView1.GetFocusedRowCellValue("MrpPurchaseID").ToString()
        If mprCon.MrpPurchaseRecord_PreDelete(strMrpPurchaseID) = False Then
            Exit Sub
        End If

        Dim strDelect As String = String.Empty
        Try
            If MsgBox("是否確定刪除請購單號：" + strMrpPurchaseID + "？", vbOKCancel, "提示") = vbOK Then
                '1.主表刪除
                If mprCon.MrpPurchaseRecord_Delete(Nothing, GridView1.GetFocusedRowCellValue("AutoID").ToString()) = False Then
                    MsgBox("刪除錯誤！", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If
                '2.子表刪除
                If mpreCon.MrpPurchaseRecordEntry_Delete(strMrpPurchaseID, Nothing) = False Then
                    MsgBox("子表明細刪除錯誤！", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If

                tsmRefresh_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            MsgBox(ex)
        End Try
    End Sub
#End Region

#Region "查看事件"
    Private Sub tsmView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmView.Click
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If

        On Error Resume Next
        Dim fr As frmMrpPurchaseRecord
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpPurchaseRecord Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpPurchaseRecord
        fr.EditItem = EditEnumType.VIEW
        fr.MrpPurchaseID = GridView1.GetFocusedRowCellValue("MrpPurchaseID")
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "審核事件"
    Private Sub tsmCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmCheck.Click
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        Dim strMrpPurchaseID As String = GridView1.GetFocusedRowCellValue("MrpPurchaseID")
        On Error Resume Next
        Dim fr As frmMrpPurchaseRecord
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpPurchaseRecord Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpPurchaseRecord
        fr.EditItem = EditEnumType.CHECK
        fr.MrpPurchaseID = strMrpPurchaseID
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "刷新事件"
    Private Sub tsmRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmRefresh.Click
        Dim boolCheckType As String = String.Empty
        Dim MrpSetCon As New MrpSettingController
        Dim MrpSet As New MrpSettingInfo
        If MrpSetCon.MrpSetting_GetList(InUserID).Count > 0 Then
            MrpSet = MrpSetCon.MrpSetting_GetList(InUserID)(0)
            Select Case MrpSet.purchaseCheckType
                Case "0"
                    boolCheckType = "False"
                Case "1"
                    boolCheckType = "True"
                Case "0,1"
                    boolCheckType = Nothing
            End Select
          
            If MrpSet.purchaseCreateUserID = "All" Then
                MrpSet.purchaseCreateUserID = Nothing
            End If

        Else
            MrpSet.purchaseBeginDate = Year(Now) & "/01/01"
            boolCheckType = Nothing
            MrpSet.purchaseCreateUserID = Nothing
        End If
        GridControl1.DataSource = mprCon.MrpPurchaseRecord_GetList(Nothing, Nothing, MrpSet.purchaseCreateUserID, MrpSet.purchaseBeginDate, Nothing, Nothing, Nothing, boolCheckType, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, MrpSet.purchaseDisplayNum)
        GridView_FocusedRowChanged(Nothing, Nothing)
    End Sub
#End Region

#Region "列印"
    Private Sub tsmPaint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPaint.Click

    End Sub
#End Region

#Region "查詢"
    Private Sub tsmFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFind.Click
        'Dim fr As New frmSelect
        ''fr.FormText = "MRP請購申請"
        ''fr.TableName = "MrpPurchaseRecord"
        ''fr.ID = "MrpPurchaseID"
        'fr.ShowDialog()
        'Dim sc As New Select_Controller
        'If String.IsNullOrEmpty(tempValue) = False Then
        '    GridControl1.DataSource = sc.MrpPurchaseRecord_GetList(tempValue)
        'End If
    End Sub

#End Region

#Region "設置右擊菜單項是否可用"
    Private Sub GridControl1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl1.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            SetRightClickMenuEnable()
        End If
    End Sub

    Private Sub SetRightClickMenuEnable()
        Dim mpri As New MrpPurchaseRecordInfo
        Dim mpriList As New List(Of MrpPurchaseRecordInfo)
        If GridView1.FocusedRowHandle >= 0 Then
            mpriList = mprCon.MrpPurchaseRecord_GetList(GridView1.GetFocusedRowCellValue("MrpPurchaseID"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If mpriList.Count > 0 Then
                mpri = mpriList(0)
            Else
                MsgBox(GridView1.GetFocusedRowCellValue("MrpPurchaseID") + "的請購單號已被其他用戶刪除", MsgBoxStyle.Information, "提示")
                tsmRefresh_Click(Nothing, Nothing)
                Exit Sub
            End If
        End If
        Try
            Dim c As ToolStripItem
            If GridView1.FocusedRowHandle < 0 Then
                For Each c In cmsMenuStrip.Items
                    If (c.Name = "tsmNew" Or c.Name = "tsmRefresh") Then
                        c.Enabled = True
                    Else
                        c.Enabled = False
                    End If
                Next
            ElseIf mpri.CheckBit.Equals(True) Then
                For Each c In cmsMenuStrip.Items
                    If (c.Name = "tsmEdit" Or c.Name = "tsmDelete") Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                    End If
                Next
            Else
                For Each c In cmsMenuStrip.Items
                    c.Enabled = True
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SetRightClickMenuEnable方法出錯")
        End Try
    End Sub
#End Region

#Region "表格事件"
    Private Sub GridView_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SetRightClickMenuEnable()
        If GridView1.FocusedRowHandle < 0 Then
            GridControl2.DataSource = Nothing
        Else
            GridControl2.DataSource = mpreCon.MrpPurchaseRecordEntry_GetList(GridView1.GetFocusedRowCellValue("MrpPurchaseID"))
        End If
    End Sub
#End Region

#Region "時間列為空時的顯示"
    Private Sub GridView_CustomColumnDisplayText(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView.CustomColumnDisplayText
        Try
            '----------------當日期為空時，則不顯示----------------
            If e.Column.FieldName = "MPI_NeedDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            If e.Column.FieldName = "MPI_CreateDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            If e.Column.FieldName = "MPI_ModifyDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            If e.Column.FieldName = "ForecastDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
    Private Sub GridView1_CustomColumnDisplayText(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView1.CustomColumnDisplayText
        Try
            '----------------當日期為空時，則不顯示----------------
            If e.Column.FieldName = "MPP_CreateDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            If e.Column.FieldName = "MPP_ModifyDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            If e.Column.FieldName = "MPP_CheckDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
            '----------------當請購日期為空時，則不顯示----------------
            If e.Column.FieldName = "PurchaseDate" Then
                If e.Value = Nothing Then e.DisplayText = ""
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        On Error Resume Next
        Dim fr As frmMrpPurchase
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpPurchase Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpPurchase
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub tsm_SelectSQ_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_SelectSQ.Click
        If mprCon.MrpPurchaseRecord_GetList(GridView1.GetFocusedRowCellValue("MrpPurchaseID").ToString(), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            MsgBox("必須先審核才能選擇供應商！")
            Exit Sub
        End If

        Dim mmcilist As New List(Of MrpMaterialCodeInfo)
        mmcilist = mmcc.MrpMaterialCode_GetList(GridView.GetFocusedRowCellValue("M_Code").ToString, Nothing, "True", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If mmcilist.Count <= 0 Then
            MsgBox("沒有對應的報價信息！")
            Exit Sub
        End If
        Dim fr As New frmMrpPurchaseRecord
        fr.EditItem = EditEnumType.ELSEONE
        fr.MrpPurchaseID = GridView1.GetFocusedRowCellValue("MrpPurchaseID")
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

#Region "轉采購單"
    Private Sub tsm_ToPurchaseOrder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_ToPurchaseOrder.Click
        If GridView.RowCount < 0 Then
            MsgBox("物料明細中無數據，無法轉采購")
            Exit Sub
        End If
        

        Dim unitPrice As Decimal  
        Dim PO As String = String.Empty
        Dim supplierID As String = String.Empty
        Dim bo As Boolean = True
        Dim PR As String = GridView1.GetFocusedRowCellValue("MrpPurchaseID").ToString
        Dim dt As DataTable = mprCon.MrpPurchaseRecord_GetAllTable(PR)
        Dim mpri As New MrpPurchaseRecordInfo
        mpri = mprCon.MrpPurchaseRecord_GetList(PR, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0)
        If mpri.IsPurchase = True Then
            MsgBox("該請購單已轉過采購單，無法再次轉采購單")
            Exit Sub
        End If
        For i As Integer = 0 To dt.Rows.Count - 1
            unitPrice = IIf(IsDBNull(dt.Rows(i)("UnitPrice")), 0, dt.Rows(i)("UnitPrice"))
            If IsDBNull(dt.Rows(i)("SupplierID")) Or unitPrice.Equals(0) Then
                MsgBox("物料明細中存在無供應商或單價為0的資料行，無法轉采購")
                Exit Sub
            End If
        Next
        Try
            For i As Integer = 0 To dt.Rows.Count - 1
                If supplierID.Equals(dt.Rows(i)("SupplierID")) = False Then
                    supplierID = dt.Rows(i)("SupplierID").ToString
                    Dim mpoi As New MrpPurchaseOrderInfo
                    PO = mpoc.MrpPurchaseOrder_GetID()
                    mpoi.PO = PO
                    mpoi.PR = dt.Rows(i)("PR")
                    mpoi.RequestUserID = dt.Rows(i)("RequestUserID")
                    mpoi.RequestDate = dt.Rows(i)("RequestDate")
                    mpoi.DeptID = dt.Rows(i)("DeptID")
                    mpoi.SupplierID = dt.Rows(i)("SupplierID")
                    mpoi.WareHouseID = dt.Rows(i)("WareHouseID")
                    mpoi.IsUrgency = IIf(IsDBNull(dt.Rows(i)("IsUrgency")), False, dt.Rows(i)("IsUrgency"))
                    mpoi.Remarks = dt.Rows(i)("M_Remarks")
                    mpoi.CreateUserID = InUserID
                    bo = mpoc.MrpPurchaseOrder_Add(mpoi) And bo
                End If
                Dim mpoei As New MrpPurchaseOrderEntryInfo
                mpoei.PO = PO
                mpoei.M_Code = dt.Rows(i)("M_Code")
                mpoei.PurchaseQty = dt.Rows(i)("PurchaseQty")
                mpoei.NeedDate = dt.Rows(i)("NeedDate")
                mpoei.UnitPrice = dt.Rows(i)("UnitPrice")
                mpoei.DeliveryDate = dt.Rows(i)("DeliveryDate")
                mpoei.Remarks = dt.Rows(i)("Remarks")
                mpoei.CreateUserID = InUserID
                bo = mpoec.MrpPurchaseOrderEntry_Add(mpoei) And bo
            Next

            mprCon.MrpPurchaseRecord_UpdateUrgent(GridView1.GetFocusedRowCellValue("MrpPurchaseID").ToString)
            If bo = True Then
                MsgBox("轉單成功", MsgBoxStyle.Information, "提示")
            Else
                MsgBox("轉單出錯", MsgBoxStyle.Information, "提示")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "轉采購單方法出錯")
        End Try
        

    End Sub
#End Region

    Private Sub tsm_PrintMrpchaseRecordAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_PrintMrpchaseRecordAll.Click
        On Error Resume Next
        Dim fr As MrpReportSelect
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is MrpReportSelect Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New MrpReportSelect
        fr.intShowPage = 5
        fr.ShowDialog()
        fr.Focus()
    End Sub

#Region "導出Excel"
    Private Sub cmsExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsExcel.Click, tsm_Excel.Click
        Try
            If sender.Owner Is cmsMenuStrip Then
                ConrotlExportExcel(GridControl1)
            Else
                ConrotlExportExcel(GridControl2)
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "提示")
        End Try
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480701")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmNew.Visible = True
                tsmNew.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480703")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmDelete.Visible = True
                tsmDelete.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480702")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmEdit.Visible = True
                tsmEdit.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480704")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsmCheck.Visible = True
                tsmCheck.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480705")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsm_SelectSQ.Visible = True
                tsm_SelectSQ.Enabled = True
            End If
        End If
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsm_ToPurchaseOrder.Visible = True
                tsm_ToPurchaseOrder.Enabled = True
            End If

        End If
    End Sub
#End Region
End Class

