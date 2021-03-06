Imports LFERP.Library.MrpManager.MrpSupplierQuotation
Imports LFERP.Library.MrpManager.MrpSetting
Imports LFERP.SystemManager
Public Class frmMRPSupplierQuotationMain
    Dim mrpSQcon As New MrpSupplierQuotationController
    Private Sub frmMRPSupplierQuotationMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tsmRefresh_Click(Nothing, Nothing)
    End Sub

    Private Sub tsmNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNew.Click
        On Error Resume Next
        Dim fr As frmMRPSupplierQuotation
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMRPSupplierQuotation Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMRPSupplierQuotation
        fr.EditItem = EditEnumType.ADD
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub tsmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDelete.Click
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        If mrpSQcon.MRPSupplierQuotation_GetIsCheck(GridView.GetFocusedRowCellValue("MrpSQID").ToString()) = True Then
            MsgBox("審核狀態無法刪除！")
            Exit Sub
        End If
        Dim StrMrpSQID As String = GridView.GetFocusedRowCellValue("MrpSQID").ToString()
        If MsgBox("是否確定刪除？", vbOKCancel, "请选择") = vbOK Then
            Dim result As Boolean
            result = mrpSQcon.MRPSupplierQuotation_Delete(StrMrpSQID, Nothing)
            If result = True Then
                MsgBox("刪除成功！")
                tsmRefresh_Click(Nothing, Nothing)
            End If
        End If
    End Sub

    Private Sub tsmEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmEdit.Click
        On Error Resume Next
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        If mrpSQcon.MRPSupplierQuotation_GetIsCheck(GridView.GetFocusedRowCellValue("MrpSQID").ToString()) = True Then
            MsgBox("審核狀態無法修改！")
            Exit Sub
        End If
        Dim fr As frmMRPSupplierQuotation
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMRPSupplierQuotation Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMRPSupplierQuotation
        fr.StrMrpSQID = GridView.GetFocusedRowCellValue("MrpSQID").ToString
        fr.EditItem = EditEnumType.EDIT
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub tsmView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmView.Click
        On Error Resume Next
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        Dim fr As frmMRPSupplierQuotation
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMRPSupplierQuotation Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMRPSupplierQuotation
        fr.StrMrpSQID = GridView.GetFocusedRowCellValue("MrpSQID").ToString
        fr.EditItem = EditEnumType.VIEW
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub tsmFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFind.Click
        'Dim fr As New frmMrpSelect
        'fr = New frmMrpSelect
        'fr.EditItem = "MrpForecastOrder"
        'fr.lblinfo.Text = "預測訂單--查詢"
        'fr.lbltip.Text = "請選擇預測單號:"
        'fr.ShowDialog()
        'Select Case tempValue
        '    Case "固定樣式"
        '        GridControl1.DataSource = mrpcon.MrpForecastOrder_GetList(tempValue2, Nothing, Nothing, Nothing, Nothing, Nothing)
        '    Case "自定義樣式"
        '        Dim MScon As New MrpSelect_Controller
        '        GridControl1.DataSource = MScon.MrpForecastOrder_Select_GetList("MrpForecastOrder", tempValue2)
        'End Select
    End Sub

    Private Sub tsmCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmCheck.Click
        On Error Resume Next
        If GridView.RowCount <= 0 Then
            Exit Sub
        End If
        Dim fr As frmMRPSupplierQuotation
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMRPSupplierQuotation Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMRPSupplierQuotation
        fr.StrMrpSQID = GridView.GetFocusedRowCellValue("MrpSQID").ToString
        fr.EditItem = EditEnumType.CHECK
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    Private Sub tsmRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmRefresh.Click
        Grid1.DataSource = mrpSQcon.MRPSupplierQuotation_GetList(Nothing, Nothing, Nothing)
        If GridView.RowCount <= 0 Then
            tsmDelete.Enabled = False
            tsmEdit.Enabled = False
            tsmView.Enabled = False
            tsmCheck.Enabled = False
            tsmPaint.Enabled = False
            tsmFind.Enabled = False
        Else
            tsmDelete.Enabled = True
            tsmEdit.Enabled = True
            tsmView.Enabled = True
            tsmCheck.Enabled = True
            tsmPaint.Enabled = True
            tsmFind.Enabled = True
        End If
    End Sub
    Private Sub tsm_MRPOrderFous_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_MRPOrderFous.Click

    End Sub

    Private Sub tsm_MRPOrderTotal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_MRPOrderTotal.Click

    End Sub

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010101")
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmNew.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010102")
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmEdit.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010103") '審核
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmDelete.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010104") '確認審核
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmCheck.Enabled = True
        'End If
    End Sub
#End Region


End Class