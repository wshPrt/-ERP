Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.Product
Imports LFERP.Library.Production.ProuctionWareOutB
Imports LFERP.Library.MrpManager.MrpForecastOrder
Imports LFERP.Library.MrpManager.MrpSelect
Imports LFERP.Library.MrpManager.Bom_M
Imports LFERP.Library.MrpManager.MrpSetting
Public Class frmMrpForecastOrderMain

#Region "實例與字段"
    '實例與字段
    Dim mrpcon As New MrpForecastOrderController
    Dim mrpecom As New MrpForecastOrderEntryController
    Dim ds As New DataSet
    Dim mrporderList As New List(Of MrpForecastOrderInfo)
#End Region

#Region "頁面的加載"
    '加載主表
    Private Sub frmMrpForcastOrderMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()

        Dim MrpSetCon As New MrpSettingController
        Dim MrpSet As New MrpSettingInfo
        If MrpSetCon.MrpSetting_GetList(InUserID).Count > 0 Then
            MrpSet = MrpSetCon.MrpSetting_GetList(InUserID)(0)
        Else
            MrpSet.forecastBeginDate = Year(Now) & "/01/01"
        End If

        GridControl1.DataSource = mrpcon.MrpForecastOrder_GetList(Nothing, MrpSet.forecastBeginDate, Nothing, Nothing, Nothing, Nothing)
    End Sub
    '動態加載子表
    Private Sub GridView1_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Grid1.DataSource = mrpecom.MrpForecastOrderEntry_GetList(GridView1.GetFocusedRowCellValue("ForecastID"))
    End Sub
#End Region

#Region "菜單功能"
    ''' <summary>
    ''' 添加
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmNew.Click
        On Error Resume Next
        Dim fr As frmMrpForecastOrder
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpForecastOrder Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpForecastOrder
        fr.Type = "Add"
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 修改
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmEdit.Click
        On Error Resume Next
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        If mrpcon.MrpForecastOrder_GetIScheck(GridView1.GetFocusedRowCellValue("ForecastID").ToString()) = True Then
            MsgBox("審核狀態無法修改！")
            Exit Sub
        End If
        Dim fr As frmMrpForecastOrder
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpForecastOrder Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpForecastOrder
        fr.StrForecastID = GridView1.GetFocusedRowCellValue("ForecastID").ToString
        fr.Type = "Edit"
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 審核
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmCheck.Click
        On Error Resume Next
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        Dim fr As frmMrpForecastOrder
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpForecastOrder Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpForecastOrder
        fr.StrForecastID = GridView1.GetFocusedRowCellValue("ForecastID").ToString
        fr.Type = "Check"
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 查看
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmView.Click
        On Error Resume Next
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        Dim fr As frmMrpForecastOrder
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmMrpForecastOrder Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmMrpForecastOrder
        fr.StrForecastID = GridView1.GetFocusedRowCellValue("ForecastID").ToString
        fr.Type = "View"
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
    ''' <summary>
    ''' 刪除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmDelete.Click
        If GridView1.RowCount <= 0 Then
            Exit Sub
        End If
        If mrpcon.MrpForecastOrder_GetIScheck(GridView1.GetFocusedRowCellValue("ForecastID").ToString()) = True Then
            MsgBox("審核狀態無法刪除！")
            Exit Sub
        End If
        Dim StrForecastID As String = GridView1.GetFocusedRowCellValue("ForecastID").ToString()
        Dim StrAutoID As String = GridView1.GetFocusedRowCellValue("AutoID").ToString()


        If MsgBox("是否確定刪除？", vbOKCancel, "请选择") = vbOK Then
            If mrpcon.MrpForecastOrder_Delete(StrAutoID, Nothing) = True Then
                Dim result As Boolean
                result = mrpecom.MrpForecastOrderEntry_Delete(Nothing, StrForecastID)
                MsgBox("刪除成功！")
                GridControl1.DataSource = mrpcon.MrpForecastOrder_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 查詢
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmFind.Click
        Dim fr As New frmMrpSelect
        fr = New frmMrpSelect
        fr.EditItem = "MrpForecastOrder"
        fr.lblinfo.Text = "預測訂單--查詢"
        fr.lbltip.Text = "請選擇預測單號:"
        fr.ShowDialog()
        Select Case tempValue
            Case "固定樣式"
                GridControl1.DataSource = mrpcon.MrpForecastOrder_GetList(tempValue2, Nothing, Nothing, Nothing, Nothing, Nothing)
            Case "自定義樣式"
                Dim MScon As New MrpSelect_Controller
                GridControl1.DataSource = MScon.MrpForecastOrder_Select_GetList("MrpForecastOrder", tempValue2)
        End Select
    End Sub
    ''' <summary>
    ''' 刷新
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmRefresh.Click
        Dim MrpSetCon As New MrpSettingController
        Dim MrpSet As New MrpSettingInfo
        If MrpSetCon.MrpSetting_GetList(InUserID).Count > 0 Then
            MrpSet = MrpSetCon.MrpSetting_GetList(InUserID)(0)
        Else
            MrpSet.forecastBeginDate = Year(Now) & "/01/01"
        End If
        GridControl1.DataSource = mrpcon.MrpForecastOrder_GetList(Nothing, MrpSet.forecastBeginDate, Nothing, Nothing, Nothing, Nothing)
        Grid1.DataSource = mrpecom.MrpForecastOrderEntry_GetList(GridView1.GetFocusedRowCellValue("ForecastID"))
        If GridView1.RowCount <= 0 Then
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
    ''' <summary>
    ''' 打印
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsmPaint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsmPaint.Click
        Dim dss As New DataSet
        Dim ltc1 As New CollectionToDataSet
        Dim date1 As Date = "2012/10/06"
        Dim date2 As Date = "2014/01/09"
        Dim strSend As String = String.Empty
        strSend = "預測訂單資料表" + Format(date1, "yyyy/MM/dd") + Format(date2, "yyyy/MM/dd")
        '這裡添加 date1 和date2 為條件的查詢函數
        ltc1.CollToDataSet(dss, "MrpForecastOrder", mrpcon.MrpForecastOrder_GetList(Nothing, date1, date2, Nothing, Nothing, Nothing))
        PreviewRPT1(dss, "rptMrpForecastOrder", "表", strSend, strSend, True, True)
        ltc1 = Nothing
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010101")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then tsmNew.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010102")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then tsmEdit.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010103") '審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then tsmDelete.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010104") '確認審核
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then tsmCheck.Enabled = True
        End If
    End Sub
#End Region

    Private Sub frmMrpForecastOrderMain_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        tsmRefresh_Click(Nothing, Nothing)
    End Sub
End Class