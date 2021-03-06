Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.MrpManager.MrpWareHouseInfo
Imports LFERP.Library.MrpManager.MrpSelect
Imports LFERP.Library.MrpManager.MrpSetting
Public Class frmMrpWareHouseInfoMain
    Dim MWHIcon As New MrpWareHouseInfoController
    Dim MWHIEcon As New MrpWareHouseInfoEntryController
    Dim ds As New DataSet

#Region "載入事件"
    ''' <summary>
    ''' 加載
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub frmMrpWareHouseInfoMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        cmsReflash_Click(Nothing, Nothing)
    End Sub
#End Region

#Region "增加/刪除/修改/查看/審核/列印/刷新"
    ''' <summary>
    ''' 新增
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsAdd.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.lblinfo.Text = "庫存記錄表——新增"
        fr.EditItem = "Add"
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsEdit.Click
        On Error Resume Next
        Dim StrWare_ID As String = GridView1.GetFocusedRowCellValue("Ware_ID").ToString()
        If MWHIcon.MrpWareHouseInfo_GetList(StrWare_ID, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim fr As frmMrpWareHouseInfoAdd
            For Each fr In MDIMain.MdiChildren
                If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                    fr.Activate()
                    Exit Sub
                End If
            Next
            fr = New frmMrpWareHouseInfoAdd
            fr.MdiParent = MDIMain
            fr.lblinfo.Text = "庫存記錄表——修改"
            fr.EditItem = "Edit"
            fr.EditValue = StrWare_ID
            fr.WindowState = FormWindowState.Maximized
            fr.Show()
        Else
            MsgBox("已經審核不能！", 60, "提示")
        End If
    End Sub
    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsDel.Click
        Dim StrAutoID As String = GridView1.GetFocusedRowCellValue("AutoID").ToString()
        Dim StrWare_ID As String = GridView1.GetFocusedRowCellValue("Ware_ID").ToString()
        If MWHIcon.MrpWareHouseInfo_GetList(StrWare_ID, Nothing, Nothing, Nothing)(0).CheckBit = False Then
            Dim result As Windows.Forms.DialogResult = MessageBox.Show("確認刪除該信息么？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If (result = Windows.Forms.DialogResult.Yes) Then
                If MWHIcon.MrpWareHouseInfo_Delete(StrAutoID) = True Then
                    If MWHIEcon.MrpWareHouseInfoEntry_DeleteAll(StrWare_ID) = False Then
                        MsgBox("刪除失敗！", 60, "提示")
                        Exit Sub
                    End If
                End If
            End If
            cmsReflash_Click(Nothing, Nothing)
        Else
            MsgBox("已經審核不能刪除！", 60, "提示")
        End If
    End Sub
    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsFind.Click
        Dim fr As New frmMrpSelect
        fr = New frmMrpSelect
        fr.EditItem = "MrpWareHouseInfo"
        fr.lblinfo.Text = "庫存記錄表——查詢"
        'fr.EditValue = "MrpWareHouseInfo"
        fr.lbltip.Text = "請選擇庫存單號:"
        fr.ShowDialog()
        Select Case tempValue
            Case "固定樣式"
                Grid.DataSource = MWHIcon.MrpWareHouseInfo_GetList(tempValue2, Nothing, Nothing, Nothing)
                'tempValue2 = String.Empty
            Case "自定義樣式"
                Dim MScon As New MrpSelect_Controller
                Grid.DataSource = MScon.MrpWareHouseInfo_Select_GetList("MrpWareHouseInfo", tempValue2)
                'tempValue2 = String.Empty
        End Select
    End Sub
    ''' <summary>
    ''' 查看
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsLook_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsLook.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.lblinfo.Text = "庫存記錄表——查看"
        fr.EditItem = "Look"
        fr.EditValue = GridView1.GetFocusedRowCellValue("Ware_ID").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 審核
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsCheck.Click
        On Error Resume Next
        Dim fr As frmMrpWareHouseInfoAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpWareHouseInfoAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpWareHouseInfoAdd
        fr.MdiParent = MDIMain
        fr.lblinfo.Text = "庫存記錄表——審核"
        fr.EditItem = "Check"
        fr.EditValue = GridView1.GetFocusedRowCellValue("Ware_ID").ToString
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 刷新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsReflash_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsReflash.Click
        Dim msi As New List(Of MrpSettingInfo)
        Dim msc As New MrpSettingController

        Dim StrCheck As String = Nothing
        Dim StrUser As String = Nothing

        msi = msc.MrpSetting_GetList(InUserID)
        If msi.Count > 0 Then
            Select Case msi(0).warehouseCheckType
                Case "0,1"
                    StrCheck = Nothing
                Case "1"
                    StrCheck = "1"
                Case "0"
                    StrCheck = "0"
            End Select

            If msi(0).warehouseCreateUserID = "All" Then
                StrUser = Nothing
            Else
                StrUser = msi(0).warehouseCreateUserID
            End If

            Grid.DataSource = MWHIcon.MrpWareHouseInfo_GetList(Nothing, StrCheck, StrUser, msi(0).warehouseBeginDate)
        Else
            Grid.DataSource = MWHIcon.MrpWareHouseInfo_GetList(Nothing, Nothing, Nothing, Nothing)
        End If
        GridView1.ActiveFilterString = "MRP_ID like 'MI%'"

    End Sub
    ''' <summary>
    ''' 打印
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsPrint.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        'Dim strSO_ID As String = GridView1.GetFocusedRowCellValue("AutoId").ToString
        ltc.CollToDataSet(dss, "MrpWareHouseInfo", MWHIcon.MrpWareHouseInfo_GetList(Nothing, Nothing, Nothing, Nothing))
        PreviewRPT(dss, "rptMrpWareHouseInfo", "庫存記錄表", True, True)
        ltc = Nothing
        Me.Close()
    End Sub
#End Region

#Region "聚焦改變獲得子表信息"
    ''' <summary>
    ''' 聚焦改變事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridView1_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        GetSubTable()
    End Sub
    ''' <summary>
    ''' 獲得子表信息
    ''' </summary>
    ''' <remarks></remarks>
    Sub GetSubTable()
        If GridView1.RowCount = 0 Then Exit Sub
        If GridView1.GetFocusedRowCellValue("Ware_ID").ToString IsNot Nothing Then
            GridControl1.DataSource = MWHIEcon.MrpWareHouseInfoEntry_GetList(GridView1.GetFocusedRowCellValue("Ware_ID").ToString)
        End If
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030101")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsAdd.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030102")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsEdit.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030103") '審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsDel.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48030104") '確認審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsCheck.Enabled = True
        End If
    End Sub
#End Region
End Class