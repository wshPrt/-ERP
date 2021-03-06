Imports LFERP.Library.SampleManager.SampleOrdersMain
Imports LFERP.Library.SampleManager.SampleWareInventory
Imports LFERP.Library.PieceProcess
Imports LFERP.Library.SampleManager.SampInventoryCheck

Public Class frmSampInventory

#Region "屬性"
    Dim somcon As New SampleOrdersMainControler
    Dim pncon As New PersonnelControl
    Dim sicon As New SampInventoryCheckControl
    Private strTID As String
    Private strD_ID As String
    Dim DT_M As New DataTable
    Dim DT_N As New DataTable
    Dim strPM_M_Code As String = String.Empty
    Dim BoolZeroShow As Boolean = False
#End Region

#Region "窗体载入"
    Private Sub frmSampInventory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '1加载材质
        Dim somlist As New List(Of SampleOrdersMainInfo)
        somlist = somcon.SampleOrdersType_GetList(Nothing, "M", "True", Nothing)
        chkMaterialType.DataSource = somlist
        '2加载部门
        Dim pmlist As New List(Of PersonnelInfo)
        pmlist = pncon.FacBriSearch_GetList("Z", Nothing, Nothing, Nothing)
        chkD_ID.DataSource = pmlist
        '2加载产品
        Dim solist As New List(Of SampleOrdersMainInfo)
        Dim soinfo As New SampleOrdersMainInfo
        soinfo.PM_M_Code = "ALL"
        soinfo.SO_ID = "全部"
        soinfo.SO_SampleID = "全部"
        soinfo.M_Code_Type = "全部"
        solist = somcon.SampleOrdersMain_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False)
        solist.Insert(0, soinfo)
        gluPM_M_Code.Properties.DataSource = solist
        gluPM_M_Code.EditValue = "ALL"
    End Sub
#End Region

#Region "控件全选"
    Private Sub chkMaterialTypeAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMaterialTypeAll.CheckedChanged
        For i As Integer = 0 To chkMaterialType.ItemCount - 1
            If chkMaterialTypeAll.Checked Then
                chkMaterialType.SetItemChecked(i, True)
            Else
                chkMaterialType.SetItemChecked(i, False)
            End If
        Next
    End Sub
    Private Sub chkD_IDAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkD_IDAll.CheckedChanged
        For i As Integer = 0 To Me.chkD_ID.ItemCount - 1
            If chkD_IDAll.Checked Then
                chkD_ID.SetItemChecked(i, True)
            Else
                chkD_ID.SetItemChecked(i, False)
            End If
        Next
    End Sub
#End Region

#Region "控件返回值"
    Private Sub gluMaterialType_QueryResultValue(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.QueryResultValueEventArgs) Handles gluMaterialType.QueryResultValue
        Dim str As String = String.Empty
        strTID = String.Empty
        For i As Integer = 0 To chkMaterialType.ItemCount - 1
            If chkMaterialType.GetItemChecked(i) = True Then
                str += chkMaterialType.GetItemText(i) + ","
                strTID += "'" + chkMaterialType.GetItemValue(i) + "',"

            End If
        Next
        If str.Length > 1 Then
            str = str.Remove(str.LastIndexOf(","), 1)
            strTID = strTID.Remove(strTID.LastIndexOf(","), 1)
        End If

        gluMaterialType.ToolTipTitle = lblMaterialType.Text
        gluMaterialType.ToolTip = str
        e.Value = str
    End Sub

    Private Sub gluD_ID_QueryResultValue(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.QueryResultValueEventArgs) Handles gluD_ID.QueryResultValue
        Dim str As String = String.Empty
        strD_ID = String.Empty
        For i As Integer = 0 To chkD_ID.ItemCount - 1
            If chkD_ID.GetItemChecked(i) = True Then
                str += chkD_ID.GetItemText(i) + ","
                strD_ID += "[" + chkD_ID.GetItemValue(i) + "],"

            End If
        Next
        If str.Length > 1 Then
            str = str.Remove(str.LastIndexOf(","), 1)
            strD_ID = strD_ID.Remove(strD_ID.LastIndexOf(","), 1)
        End If
        gluD_ID.ToolTipTitle = lblD_ID.Text
        gluD_ID.ToolTip = str
        e.Value = str
    End Sub
#End Region

#Region "查询"
    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        '1.表格清空
        Grid2.DataSource = Nothing
        Grid3.DataSource = Nothing
        '2.控件是否为空
        If gluD_ID.EditValue = String.Empty Then
            MsgBox("部门不能为空,请您输入部门", MsgBoxStyle.Information, "提示")
            Exit Sub
        ElseIf (gluPM_M_Code.Text = String.Empty) Then
            MsgBox("产品编号不能为空,请您输入产品编号", MsgBoxStyle.Information, "提示")
            Exit Sub
        ElseIf (gluMaterialType.EditValue = String.Empty) Then
            MsgBox("材质不能为空,请您输入材质", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        '3.产品控件

        If gluPM_M_Code.Text = "ALL" Then
            strPM_M_Code = Nothing
        Else
            strPM_M_Code = gluPM_M_Code.Text
        End If
        '4.是否去0

        If chkZero.Checked Then
            BoolZeroShow = False
        Else
            BoolZeroShow = True
        End If
        '5.产品类别
        Dim strM_Code_Type As String = String.Empty
        If Me.txtM_Code_Type.Text = "ALL" Then
            strM_Code_Type = Nothing
        Else
            strM_Code_Type = txtM_Code_Type.Text
        End If
        '6.产品部门汇总
        Try
            Grid1.DataSource = Nothing
            DT_M = sicon.SampleInventoryTatalA_GetList(strPM_M_Code, strTID, strD_ID)
            If DT_M.Rows.Count > 0 Then
                AdvBandedGridView1.Bands.Clear()
                AdvBandedGridView1.Columns.Clear()
                BGridViewDeal(DT_M, 7, AdvBandedGridView1, Grid1)
            End If
        Catch
            MsgBox("产品部门汇总数据查询错误,请检查原因", MsgBoxStyle.Information, "提示")
            Exit Sub
        End Try
        '7.材质汇总
        Try
            Grid4.DataSource = Nothing
            DT_N = sicon.SampleInventoryTatalD_GetList(strM_Code_Type, strTID, strD_ID)
            If DT_N.Rows.Count > 0 Then
                AdvBandedGridView2.Bands.Clear()
                AdvBandedGridView2.Columns.Clear()
                BGridViewDealA(DT_N, 6, AdvBandedGridView2, Grid4)
            End If
        Catch
            MsgBox("产品类别数据查询错误,请检查原因", MsgBoxStyle.Information, "提示")
            Exit Sub
        End Try


        Grid2.DataSource = sicon.SampleInventoryTatalB_GetList(strPM_M_Code, strTID, BoolZeroShow).Tables(0)
        Grid3.DataSource = sicon.SampleInventoryTatalC_GetList(strPM_M_Code, strTID, BoolZeroShow).Tables(0)
    End Sub
#End Region

#Region "BGridView 賦值 處理 "
    Private Sub BGridViewDeal(ByVal dt As DataTable, ByVal begionNum As Integer, ByVal BG As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView, ByVal GC As DevExpress.XtraGrid.GridControl)
        '1.求和與修改列標題
        SetDataTableColumnName(dt, begionNum, 7)
        For i As Integer = 0 To dt.Columns.Count - 1
            If i >= 0 And i <= begionNum - 3 Then
                If i = 0 Then
                    BG.Bands.AddBand("產品信息")
                    GC.DataSource = dt
                    BG.Bands.AddBand("部门数量")
                    BG.Bands.AddBand("总计")

                    BG.Columns(3).SummaryItem.DisplayFormat = "合计："
                    BG.Columns(3).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom
                End If

                BG.Bands(0).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))
            End If

            If i > begionNum - 3 And i <= dt.Columns.Count - 2 Then
                BG.Columns(i).SummaryItem.DisplayFormat = "{0}"
                BG.Columns(i).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                BG.Bands(1).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))

            End If

            If i = dt.Columns.Count - 1 Then
                BG.Columns(i).SummaryItem.DisplayFormat = "{0}"
                BG.Columns(i).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                BG.Bands(2).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))
            End If
        Next
        BG.Columns(3).Visible = False
        BG.Bands(0).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        BG.Bands(2).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right
    End Sub
    Private Sub BGridViewDealA(ByVal dt As DataTable, ByVal begionNum As Integer, ByVal BG As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView, ByVal GC As DevExpress.XtraGrid.GridControl)
        '1.求和與修改列標題
        SetDataTableColumnName(dt, begionNum, 5)
        For i As Integer = 0 To dt.Columns.Count - 1
            If i >= 0 And i <= begionNum - 3 Then
                If i = 0 Then
                    BG.Bands.AddBand("类别")
                    GC.DataSource = dt
                    BG.Bands.AddBand("部门数量")
                    BG.Bands.AddBand("总计")

                    BG.Columns(2).SummaryItem.DisplayFormat = "合计："
                    BG.Columns(2).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom
                End If

                BG.Bands(0).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))
            End If

            If i > begionNum - 3 And i <= dt.Columns.Count - 2 Then
                BG.Columns(i).SummaryItem.DisplayFormat = "{0}"
                BG.Columns(i).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                BG.Bands(1).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))

            End If

            If i = dt.Columns.Count - 1 Then
                BG.Columns(i).SummaryItem.DisplayFormat = "{0}"
                BG.Columns(i).SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum
                BG.Bands(2).Columns.Add(BG.Columns(dt.Columns(i).ColumnName))
            End If
        Next
        BG.Columns(3).Visible = False
        BG.Bands(0).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        BG.Bands(2).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right

    End Sub
#End Region

#Region "Table 處理"
    Private Sub SetDataTableColumnName(ByVal dt As DataTable, ByVal begionNum As Integer, ByVal IntWin As Integer)
        For i As Integer = 0 To dt.Columns.Count - 1
            If i = 0 Then
                If IntWin = 5 Then
                    dt.Columns(0).ColumnName = "材质"
                    dt.Columns(1).ColumnName = "产品类别"
                    dt.Columns(2).ColumnName = "材质编号"
                    dt.Columns(3).ColumnName = "材料"
                End If
                If IntWin = 7 Then
                    dt.Columns(0).ColumnName = "材料"
                    dt.Columns(1).ColumnName = "材质"
                    dt.Columns(2).ColumnName = "样办单号"
                    dt.Columns(3).ColumnName = "材质编号"
                    dt.Columns(4).ColumnName = "产品编号"
                End If

            End If
            If i > begionNum - 3 And i <= dt.Columns.Count - 2 Then
                dt.Columns(i).ColumnName = pncon.FacBriSearch_GetList("Z", Nothing, dt.Columns(i).ColumnName, Nothing)(0).DepName
            End If
            If i = dt.Columns.Count - 1 Then
                dt.Columns(i).ColumnName = "ToTal"
            End If
        Next
    End Sub
#End Region

#Region "转Excel程序"
    Private Sub cmdExcelSub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelSub.Click, cmdExcel.Click
        GetInExcel()
    End Sub

    Private Sub GetInExcel()
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0

                If AdvBandedGridView2.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid4, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
            Case 1

                If AdvBandedGridView1.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid1, saveFileDialog.FileName) Then
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
                    If ExportToExcelOld(Grid2, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
            Case 3

                If GridView3.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid3, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If

        End Select
    End Sub
#End Region

#Region "控件事件"
    Private Sub gluPM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluPM_M_Code.EditValueChanged
        If gluPM_M_Code.EditValue <> String.Empty Then
            Dim solist As New List(Of SampleOrdersMainInfo)
            solist = somcon.SampleOrdersMain_GetList(gluPM_M_Code.EditValue, Nothing, Nothing, Nothing, Nothing, Nothing, False)
            If solist.Count > 0 Then
                Me.txtM_Code_Type.Text = solist(0).M_Code_Type
            Else
                Me.txtM_Code_Type.Text = "ALL"
            End If
        End If
    End Sub
#End Region

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 2
                Dim dss As New DataSet
                dss = sicon.SampleInventoryTatalB_GetList(strPM_M_Code, strTID, BoolZeroShow)
                PreviewRPT1(dss, "rptSampleInventoryTatalBPrint", "产品资料汇总", InUser, InUser, True, True)
            Case 3
                Dim dss As New DataSet
                dss = sicon.SampleInventoryTatalC_GetList(strPM_M_Code, strTID, BoolZeroShow)
                PreviewRPT1(dss, "rptSampleInventoryTatalCPrint", "材质汇总", InUser, InUser, True, True)
        End Select
    End Sub
End Class