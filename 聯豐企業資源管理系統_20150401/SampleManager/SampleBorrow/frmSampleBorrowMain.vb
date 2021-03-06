Imports LFERP.Library.SampleManager.SampleBorrow
Imports LFERP.Library.SampleManager.SampleWareInventory
Imports LFERP.SystemManager
Public Class frmSampleBorrowMain
#Region "属性"
    Dim frmB As frmSampleBorrow
    Dim frmR As frmSampleRepay
    Dim sbc As New SampleBorrowController
    Dim src As New SampleRepayController
    Dim swcon As New SampleWareInventoryControler

    Dim strDpt As String
#End Region

#Region "窗体载入"
    Private Sub frmSampleBorrowMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        Dim pclist As New List(Of LFERP.Library.ProductionController.ProductionFieldControlInfo)
        Dim pminfo As New LFERP.Library.ProductionController.ProductionFieldControlInfo
        pminfo.DepName = "全部"
        pminfo.ControlDep = "All"

        Dim fc As New LFERP.Library.ProductionController.ProductionFieldControl
        pclist = fc.ProductionFieldControl_GetList(InUserID, Nothing)
        pclist.Insert(0, pminfo)
        GridControl2.DataSource = pclist
        cmsBorrowRefresh_Click(Nothing, Nothing)
    End Sub
#End Region

#Region "借出新增"
    Private Sub cmsBorrowAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowAdd.Click

        If strDpt = Nothing Or strDpt = String.Empty Then
            MsgBox("请您选择借出部门", MsgBoxStyle.Information, "提示")
            Me.GridView2.Focus()
            Exit Sub
        End If

        frmB = New frmSampleBorrow
        frmB.EditItem = EditEnumType.ADD
        frmB.D_ID = strDpt
        frmB.Show()
    End Sub

#End Region

#Region "借出修改"
    Private Sub cmsBorrowEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowEdit.Click
        '1.是否存在借还记录
        Dim strBorrowID As String = viewBorrow.GetFocusedRowCellValue("BorrowID").ToString
        If strBorrowID = String.Empty Or strBorrowID = Nothing Then
            Exit Sub
        End If

        Dim sblist As New List(Of SampleBorrowInfo)
        sblist = sbc.SampleBorrow_GetList(Nothing, strBorrowID, Nothing, Nothing, Nothing, Nothing, False)
        If sblist.Count > 0 Then
            'If sblist(0).Borrow_Qty <> sblist(0).NoBorrow_Qty Then
            '    MsgBox("存在借还记录不能修改！", MsgBoxStyle.Information, "提示")
            '    Exit Sub
            'End If
        End If

        frmB = New frmSampleBorrow
        frmB.EditItem = EditEnumType.EDIT
        frmB.BorrowID = strBorrowID
        frmB.Show()
    End Sub
#End Region

#Region "借出删除"
    Private Sub cmsBorrowDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowDel.Click
        '1.是否存在借还记录
        Dim strBorrowID As String = viewWareInventory.GetFocusedRowCellValue("BorrowID").ToString
        If strBorrowID = String.Empty Or strBorrowID = Nothing Then
            Exit Sub
        End If
        Dim sblist As New List(Of SampleBorrowInfo)
        sblist = sbc.SampleBorrow_GetList(Nothing, strBorrowID, Nothing, Nothing, Nothing, Nothing, False)
        If sblist.Count > 0 Then
            If sblist(0).Borrow_Qty <> sblist(0).NoBorrow_Qty Then
                MsgBox("存在借还记录不能删除！", MsgBoxStyle.Information, "提示")
                Exit Sub
            End If
        End If

        '2.是否正式删除

        If MsgBox("确认删除此借出记录", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.Yes Then
            sbc.SampleBorrow_Delete(Nothing, strBorrowID)
            viewWareInventory.DeleteRow(viewWareInventory.FocusedRowHandle)
            MsgBox("刪除成功", MsgBoxStyle.Information, "提示")
        Else
            MsgBox("刪除失敗", MsgBoxStyle.Information, "提示")
        End If

    End Sub

#End Region

#Region "查看"
    Private Sub cmsBorrowView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowView.Click
        frmB = New frmSampleBorrow
        frmB.EditItem = EditEnumType.VIEW
        frmB.BorrowID = viewBorrow.GetFocusedRowCellValue("BorrowID").ToString
        frmB.Show()
    End Sub
#End Region

#Region "刷新"
    Private Sub cmsBorrowRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowRefresh.Click
        Try
            grid1.DataSource = swcon.SampleWareInventoryA_Getlist(strDpt, Nothing, Nothing, Nothing)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

#Region "审核"
    Private Sub cmsBorrowCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowCheck.Click
        frmB = New frmSampleBorrow
        frmB.EditItem = EditEnumType.CHECK
        frmB.BorrowID = viewWareInventory.GetFocusedRowCellValue("BorrowID").ToString
        frmB.Show()
    End Sub
#End Region

#Region "列印事件"
    Private Sub cmsBorrowPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowPrint.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim StrSend1 = " InUser"
        Dim StrSend2 As String = String.Empty
        Dim sbc As New SampleBorrowController
        ltc.CollToDataSet(dss, "SampleWareInventoryA", swcon.SampleWareInventoryA_Getlist(strDpt, Nothing, Nothing, Nothing))
        PreviewRPT1(dss, "rptSampleWareInventory", "部门库存领还明细", InUser, InUser, True, True)
        ltc = Nothing

    End Sub
#End Region

#Region "还入新增"
    Private Sub cmdRepay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRepay.Click
        If strDpt = Nothing Or strDpt = String.Empty Then
            MsgBox("请您选择借出部门", MsgBoxStyle.Information, "提示")
            Me.GridView2.Focus()
            Exit Sub
        End If

        frmR = New frmSampleRepay
        frmR.EditItem = EditEnumType.ADD
        frmR.D_ID = strDpt
        frmR.Show()
    End Sub
#End Region

#Region "还入查看"
    Private Sub cmdViewSub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdViewSub.Click
        'If viewRepay.RowCount = 0 Then Exit Sub
        'frmR = New frmSampleRepay
        'frmR.EditItem = EditEnumType.VIEW
        'frmR.RepayID = viewRepay.GetFocusedRowCellValue("RepayID").ToString
        'frmR.Show()

        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                If viewBorrow.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid2, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
            Case 1
                If viewRepay.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(grid3, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
            Case 2
                If GridView1.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid4, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
        End Select

    End Sub
#End Region

#Region "設置右擊菜單項是否可用"
    Private Sub grid1_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Right Then
            SetRightClickMenuEnable()
        End If
    End Sub

    Private Sub SetRightClickMenuEnable()
        Dim sbi As New SampleBorrowInfo
        If viewWareInventory.FocusedRowHandle >= 0 Then
            sbi = sbc.SampleBorrow_GetList(Nothing, viewWareInventory.GetFocusedRowCellValue("BorrowID"), Nothing, Nothing, Nothing, Nothing, False)(0)
        End If
        Try
            Dim c As ToolStripItem
            If viewWareInventory.FocusedRowHandle < 0 Then
                For Each c In cmsBorrow.Items
                    If (c.Name = "cmsBorrowAdd" Or c.Name = "cmsBorrowRefresh") Then
                        c.Enabled = True
                    Else
                        c.Enabled = False
                    End If
                Next
            ElseIf sbi.CheckBit.Equals(True) Then
                For Each c In cmsBorrow.Items
                    If (c.Name = "cmsBorrowEdit" Or c.Name = "cmsBorrowDel") Then
                        c.Enabled = False
                    Else
                        c.Enabled = True
                    End If
                Next
            Else
                For Each c In cmsBorrow.Items
                    c.Enabled = True
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub
#End Region

#Region "子表刷新"
    Private Sub viewBorrow_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles viewWareInventory.FocusedRowChanged
        Dim strD_ID As String = String.Empty
        Dim strPS_NO As String = String.Empty
        If viewWareInventory.FocusedRowHandle >= 0 Then
            strD_ID = viewWareInventory.GetFocusedRowCellValue("D_ID").ToString
            strPS_NO = viewWareInventory.GetFocusedRowCellValue("PS_NO").ToString

            Grid2.DataSource = sbc.SampleBorrow_GetList(Nothing, strD_ID, strPS_NO, Nothing, Nothing, Nothing, False)
            grid3.DataSource = src.SampleRepay_GetList(Nothing, strD_ID, strPS_NO, Nothing, Nothing, Nothing, False)
            Grid4.DataSource = sbc.SampleBorrowA_GetList(strD_ID, strPS_NO, Nothing, Nothing, Nothing, False)
        Else
            Grid2.DataSource = Nothing
            grid3.DataSource = Nothing
            Grid4.DataSource = Nothing
        End If
    End Sub
#End Region

#Region "查询"
    Private Sub cmsBorrowSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowSelect.Click
        Dim fr As New frmSelect
        fr.EditItem = "Borrow"
        fr.ShowDialog()
        Select Case tempValue
            Case "FixAtion"
                grid1.DataSource = sbc.SampleBorrow_GetList(Nothing, tempValue2, Nothing, Nothing, Nothing, Nothing, False)
            Case "Dynamic"
                grid1.DataSource = sbc.SampleBorrow_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False)
        End Select
    End Sub
#End Region

#Region "转Excel"
    Private Sub cmsBorrowExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsBorrowExcel.Click
        If viewWareInventory.RowCount = 0 Then Exit Sub
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Title = "導出Excel"
        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
        Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
        If FiledialogResult = Windows.Forms.DialogResult.OK Then
            If ExportToExcelOld(grid1, saveFileDialog.FileName) Then
                MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
            End If
        End If
    End Sub
#End Region

#Region "列印事件"
    Private Sub cmsPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmsPrint.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim StrSend1 = InUser
        Dim StrSend2 As String = String.Empty
        Dim sbc As New SampleBorrowController
        ltc.CollToDataSet(dss, "SampleBorrow", sbc.SampleThrough_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        PreviewRPT1(dss, "rptSampleThough", "汇总信息表", StrSend1, StrSend2, True, True)
        ltc = Nothing
    End Sub
#End Region

#Region "选择部门"
    Private Sub GridControl2_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles GridControl2.MouseUp
        If GridView2.RowCount = 0 Then Exit Sub
        strDpt = GridView2.GetFocusedRowCellValue("ControlDep").ToString
        If strDpt = "All" Then
            strDpt = Nothing
        End If
        Try
            grid1.DataSource = swcon.SampleWareInventoryA_Getlist(strDpt, Nothing, Nothing, Nothing)
        Catch ex As Exception
        End Try
    End Sub
    Private Sub GridView2_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView2.FocusedRowChanged
        If GridView2.RowCount = 0 Then Exit Sub
        strDpt = GridView2.GetFocusedRowCellValue("ControlDep").ToString
        If strDpt = "All" Then
            strDpt = Nothing
        End If
        Try
            grid1.DataSource = swcon.SampleWareInventoryA_Getlist(strDpt, Nothing, Nothing, Nothing)
        Catch ex As Exception
        End Try
    End Sub
#End Region

#Region "设置权限"
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "891301")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsBorrowAdd.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "891302")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdRepay.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "891303")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsBorrowEdit.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "891304")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmsBorrowDel.Enabled = True
        End If
    End Sub
#End Region

#Region "各人领料明细"
    Private Sub cmdPrintBorrow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrintBorrow.Click
        If viewBorrow.RowCount <= 0 Then
            MessageBox.Show("请您选定借出人员", "提示")
            Exit Sub
        End If
        Dim strOutCardID As String = viewBorrow.GetFocusedRowCellValue("OutCardID").ToString
        If strOutCardID = String.Empty Then
            MessageBox.Show("此单还没有借出", "提示")
            Exit Sub
        End If
        Dim dss As New DataSet
        Dim ltc0 As New CollectionToDataSet
        Dim ltc1 As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet
        Dim ltc3 As New CollectionToDataSet
        Dim StrSend1 = " InUser"
        Dim StrSend2 As String = String.Empty
        Dim StrD_ID As String = viewWareInventory.GetFocusedRowCellValue("D_ID").ToString
        Dim StrPS_NO As String = viewWareInventory.GetFocusedRowCellValue("PS_NO").ToString
        Dim StrPM_M_Code As String = viewWareInventory.GetFocusedRowCellValue("PM_M_Code").ToString

        ltc0.CollToDataSet(dss, "SampleWareInventoryA", swcon.SampleWareInventoryA_Getlist(StrD_ID, StrPS_NO, StrPM_M_Code, Nothing))
        ltc1.CollToDataSet(dss, "SampleBorrow", sbc.SampleBorrow_GetList(Nothing, StrD_ID, StrPS_NO, StrPM_M_Code, strOutCardID, Nothing, True))
        ltc2.CollToDataSet(dss, "SampleRepay", src.SampleRepay_GetList(Nothing, StrD_ID, StrPS_NO, strOutCardID, StrPM_M_Code, Nothing, True))
        ltc3.CollToDataSet(dss, "SampleBorrowA", sbc.SampleBorrowA_GetList(StrD_ID, StrPS_NO, StrPM_M_Code, strOutCardID, Nothing, True))
        PreviewRPT1(dss, "rptSampleWareInventoryA", "生产领退料汇总", strOutCardID, strOutCardID, True, True)

        ltc0 = Nothing
        ltc1 = Nothing
        ltc2 = Nothing
        ltc3 = Nothing
    End Sub
#End Region

End Class