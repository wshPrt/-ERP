Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.MrpManager.MrpMaterialCode
Imports LFERP.Library.MrpManager.MrpSelect
Imports LFERP.Library.MrpManager.MrpSetting
Imports LFERP.Library.MrpManager.MrpSupplierQuotation
Public Class frmMrpMaterialCodeMain
#Region "屬性"
    Dim MMCcon As New MrpMaterialCodeController
    Dim mrpSQcon As New MrpSupplierQuotationController
#End Region

#Region "載入窗體"
    ''' <summary>
    ''' 載入
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMrpMaterialCodeMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        Dim msi As New List(Of MrpSettingInfo)
        Dim msc As New MrpSettingController

        Dim StrCheck As String = String.Empty
        Dim StrUser As String = Nothing

        msi = msc.MrpSetting_GetList(InUserID)
        If msi.Count > 0 Then
            Select Case msi(0).materialCheckType
                Case "0,1"
                    StrCheck = String.Empty
                Case "1"
                    StrCheck = "True"
                Case "0"
                    StrCheck = "False"
            End Select

            If msi(0).materialCreateUserID = "All" Then
                StrUser = Nothing
            Else
                StrUser = msi(0).materialCreateUserID
            End If

            Grid.DataSource = MMCcon.MrpMaterialCode_GetList(Nothing, msi(0).materialBeginDate, StrCheck, StrUser, Nothing, Nothing, Nothing, Nothing, Nothing, msi(0).materialDisplayNum)
        Else
            Grid.DataSource = MMCcon.MrpMaterialCode_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        GridView1_FocusedRowChanged(Nothing, Nothing)
    End Sub
#End Region

#Region "新增事件"
    ''' <summary>
    ''' 添加
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        On Error Resume Next
        Dim fr As frmMrpMaterialCode
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpMaterialCode Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpMaterialCode
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.ADD
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "修改事件"
    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        On Error Resume Next
        Dim StrM_Code As String = GridView1.GetFocusedRowCellValue("M_Code").ToString()
        If MMCcon.MrpMaterialCode_GetList(StrM_Code, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim fr As frmMrpMaterialCode
            For Each fr In MDIMain.MdiChildren
                If TypeOf fr Is frmMrpMaterialCode Then
                    fr.Activate()
                    Exit Sub
                End If
            Next
            fr = New frmMrpMaterialCode
            fr.MdiParent = MDIMain
            fr.EditItem = EditEnumType.EDIT
            fr.EditValue = StrM_Code
            fr.WindowState = FormWindowState.Maximized
            fr.Show()
        Else
            MsgBox("已經審核不能修改！", 60, "提示")
        End If
    End Sub
#End Region

#Region "刪除事件"

    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        Dim StrAutoID As String = GridView1.GetFocusedRowCellValue("AutoID").ToString()
        Dim StrM_Code As String = GridView1.GetFocusedRowCellValue("M_Code").ToString()
        If MMCcon.MrpMaterialCode_GetList(StrM_Code, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim result As Windows.Forms.DialogResult = MessageBox.Show("是否確定刪除產品編號：" + GridView1.GetFocusedRowCellValue("M_Code") + "？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If (result = Windows.Forms.DialogResult.Yes) Then
                If MMCcon.MrpMaterialCode_PreDelete(StrM_Code) = False Then
                    Exit Sub
                End If
                If MMCcon.MrpMaterialCode_Delete(StrAutoID) = True And mrpSQcon.MRPSupplierQuotation_Delete(StrM_Code, Nothing) = True Then
                    cmsReflash_Click(Nothing, Nothing)
                Else
                    MsgBox("刪除失敗！", 60, "提示")
                End If
            End If
        Else
            MsgBox("已經審核不能刪除！", 60, "提示")
        End If
    End Sub
#End Region

#Region "刷新事件"
    ''' <summary>
    ''' 刷新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsReflash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdReflash.Click
        frmMrpMaterialCodeMain_Load(Nothing, Nothing)
    End Sub
#End Region

#Region "查看事件"
    ''' <summary>
    ''' 查看
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsLook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLook.Click
        On Error Resume Next
        Dim fr As frmMrpMaterialCode
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpMaterialCode Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpMaterialCode
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.VIEW
        fr.EditValue = GridView1.GetFocusedRowCellValue("M_Code").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "審核事件"
    ''' <summary>
    ''' 審核
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCheck.Click
        On Error Resume Next
        Dim fr As frmMrpMaterialCode
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpMaterialCode Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpMaterialCode
        fr.MdiParent = MDIMain
        fr.EditItem = EditEnumType.CHECK
        fr.EditValue = GridView1.GetFocusedRowCellValue("M_Code").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
#End Region

#Region "列印事件"
    ''' <summary>
    ''' 列印
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        'Try
        '    Dim dss As New DataSet
        '    Dim ltc As New CollectionToDataSet
        '    Dim StrSend As String = String.Empty
        '    StrSend = InUser
        '    'Dim strSO_ID As String = GridView1.GetFocusedRowCellValue("AutoId").ToString
        '    ltc.CollToDataSet(dss, "MrpMaterialCode", MMCcon.MrpMaterialCode_GetList(Nothing, Nothing, Nothing, Nothing, Nothing))
        '    PreviewRPT1(dss, "rptMrpMaterialCodeAll", StrSend, StrSend, "庫存記錄表", True, True)
        '    ltc = Nothing
        '    Me.Close()
        'Catch ex As Exception
        '    MsgBox(ex.Message)
        'End Try
        On Error Resume Next
        Dim fr As MrpReportSelect
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is MrpReportSelect Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New MrpReportSelect
        fr.intShowPage = 3
        'fr.MdiParent = MDIMain
        'fr.WindowState = FormWindowState.Maximized

        fr.ShowDialog()
        fr.Focus()
    End Sub
#End Region

#Region "查詢"
    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFind.Click
        'Dim fr As New frmSelect
        'fr.FormText = "MRP物料編碼"
        'fr.TableName = "MrpMaterialCode"
        'fr.ID = "M_Code"
        'fr.ShowDialog()
        'Dim sc As New Select_Controller
        'If String.IsNullOrEmpty(tempValue) = False Then
        '    Grid.DataSource = sc.MrpMaterialCode_GetList(tempValue)
        'End If
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480101")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmdAdd.Visible = True
                cmdAdd.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480102")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmdEdit.Visible = True
                cmdEdit.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480103") '審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmdDel.Visible = True
                cmdDel.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480104") '確認審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                cmdCheck.Visible = True
                cmdCheck.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480105") '供應商
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsm_Supplier.Visible = True
                tsm_Supplier.Enabled = True
            End If

        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "480106") '倉儲
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then
                tsm_Stock.Visible = True
                tsm_Stock.Enabled = True
            End If
        End If
    End Sub
#End Region

#Region "表格事件"
    Private Sub GridView1_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        SetRightClickMenuEnable()
        If GridView1.FocusedRowHandle < 0 Then
            Grid1.DataSource = Nothing
        Else
            Grid1.DataSource = mrpSQcon.MRPSupplierQuotation_GetList(GridView1.GetFocusedRowCellValue("M_Code"), Nothing, Nothing)
        End If
    End Sub
#End Region

#Region "設置右擊菜單項是否可用"
    Private Sub grid_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Grid.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            SetRightClickMenuEnable()
        End If
    End Sub

    Private Sub SetRightClickMenuEnable()
        Dim mmci As New MrpMaterialCodeInfo
        Dim mmciList As New List(Of MrpMaterialCodeInfo)
        If GridView1.FocusedRowHandle >= 0 Then
            mmciList = MMCcon.MrpMaterialCode_GetList(GridView1.GetFocusedRowCellValue("M_Code"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If mmciList.Count > 0 Then
                mmci = mmciList(0)
            Else
                MsgBox(GridView1.GetFocusedRowCellValue("M_Code") + "的物料編碼已被其他用戶刪除", MsgBoxStyle.Information, "提示")
                frmMrpMaterialCodeMain_Load(Nothing, Nothing)
                Exit Sub
            End If

        End If
        Try
            Dim c As ToolStripItem
            If GridView1.FocusedRowHandle < 0 Then
                For Each c In ContextMenuStrip1.Items
                    If (c.Name = "cmdAdd" Or c.Name = "cmdReflash") Then
                        c.Enabled = True
                    Else
                        c.Enabled = False
                    End If
                Next
            ElseIf mmci.CheckBit.Equals(True) Then
                For Each c In ContextMenuStrip1.Items
                    If (c.Name = "cmdEdit" Or c.Name = "cmdDel") Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                    End If
                Next
            Else
                For Each c In ContextMenuStrip1.Items
                    If (c.Name = "tsm_Supplier" Or c.Name = "tsm_Stock") Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                    End If
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "SetRightClickMenuEnable方法出錯")
        End Try
    End Sub
#End Region

 

#Region "導出Excel"
    Private Sub cmsExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsExcel.Click, cmsSubExcel.Click
        If sender.Owner Is ContextMenuStrip1 Then
            ConrotlExportExcel(Grid)
        Else 
            ConrotlExportExcel(Grid1)
        End If
    End Sub
#End Region

    Private Sub tsm_Supplier_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_Supplier.Click
        On Error Resume Next
        Dim StrM_Code As String = GridView1.GetFocusedRowCellValue("M_Code").ToString()
        Dim StrM_Type As String = GridView1.GetFocusedRowCellValue("MC_Source").ToString()
        If StrM_Type = "原物料" Then
            If MMCcon.MrpMaterialCode_GetList(StrM_Code, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = True Then
                Dim fr As frmMrpMaterialCode
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpMaterialCode Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpMaterialCode
                fr.MdiParent = MDIMain
                fr.EditItem = EditEnumType.ELSEONE
                fr.EditValue = StrM_Code
                fr.WindowState = FormWindowState.Maximized
                fr.Show()
            Else
                MsgBox("必須先審核才能選擇供應商！", 60, "提示")
            End If
        Else
            MsgBox("原物料才能選擇供應商！", 60, "提示")
        End If
    End Sub

    Private Sub tsm_Stock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_Stock.Click
        On Error Resume Next
        Dim StrM_Code As String = GridView1.GetFocusedRowCellValue("M_Code").ToString()
        If MMCcon.MrpMaterialCode_GetList(StrM_Code, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0).CheckBit = True Then
            Dim fr As frmMrpMaterialCode
            For Each fr In MDIMain.MdiChildren
                If TypeOf fr Is frmMrpMaterialCode Then
                    fr.Activate()
                    Exit Sub
                End If
            Next
            fr = New frmMrpMaterialCode
            fr.MdiParent = MDIMain
            fr.EditItem = EditEnumType.ELSETWO
            fr.EditValue = StrM_Code
            fr.WindowState = FormWindowState.Maximized
            fr.Show()
        Else
            MsgBox("必須先審核才能選擇倉儲！", 60, "提示")
        End If
    End Sub
End Class