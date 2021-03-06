Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Imports LFERP.Library.NmetalSampleManager.NmetalSampleWareInventory
Imports LFERP.Library.PieceProcess
Imports LFERP.Library.NmetalSampleManager.NmetalSampInventoryCheck

Public Class frmNmetalSampleInventoryAnnal

#Region "屬性"
    Dim somcon As New NmetalSampleOrdersMainControler
    Dim pncon As New PersonnelControl
    Dim sicon As New NmetalSampInventoryCheckControl
    Private strTID As String
    Private strD_ID As String
    Dim DT_M As New DataTable
    Dim DT_N As New DataTable
    Dim strPM_M_Code As String = String.Empty
    Dim BoolZeroShow As Boolean = False
#End Region

#Region "窗体载入"
    Private Sub frmSampleInventoryAnnal_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '2加载部门
        Dim pmlist As New List(Of PersonnelInfo)
        pmlist = pncon.FacBriSearch_GetList("Z", Nothing, Nothing, Nothing)
        chkD_ID.DataSource = pmlist
        '2加载产品
        Dim solist As New List(Of NmetalSampleOrdersMainInfo)
        Dim soinfo As New NmetalSampleOrdersMainInfo
        soinfo.PM_M_Code = "ALL"
        soinfo.SO_ID = "全部"
        soinfo.SO_SampleID = "全部"
        soinfo.M_Code_Type = "全部"
        solist = somcon.NmetalSampleOrdersMain_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False)
        solist.Insert(0, soinfo)
        gluPM_M_Code.Properties.DataSource = solist
        gluPM_M_Code.EditValue = "ALL"
    End Sub
#End Region

#Region "控件全选"
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
        Grid1.DataSource = Nothing
        Grid2.DataSource = Nothing
        '2.控件是否为空
        If gluD_ID.EditValue = String.Empty Then
            MsgBox("部门不能为空,请您输入部门", MsgBoxStyle.Information, "提示")
            gluD_ID.Focus()
            Exit Sub
        ElseIf (gluPM_M_Code.Text = String.Empty) Then
            MsgBox("产品编号不能为空,请您输入产品编号", MsgBoxStyle.Information, "提示")
            gluPM_M_Code.Focus()
            Exit Sub
        ElseIf (dateStratDate.Text = String.Empty) Then
            MsgBox("起始日期不能为空,请您输入起始日期", MsgBoxStyle.Information, "提示")
            dateStratDate.Focus()
            Exit Sub
        ElseIf (dateEndDate.Text = String.Empty) Then
            MsgBox("截止日期不能为空,请您输入截止日期", MsgBoxStyle.Information, "提示")
            dateEndDate.Focus()
            Exit Sub
        ElseIf CDate(dateEndDate.Text) < CDate(dateStratDate.Text) Then
            MsgBox("截止日期不能小于起始日期,请您输入日期", MsgBoxStyle.Information, "提示")
            dateEndDate.Focus()
            Exit Sub
        End If

        ''3.产品控件
        If gluPM_M_Code.Text = "ALL" Then
            strPM_M_Code = Nothing
        Else
            strPM_M_Code = gluPM_M_Code.Text
        End If

        Grid1.DataSource = sicon.NmetalSampleWareInventoryAnnal3_GetList(strPM_M_Code, Nothing, strD_ID, dateStratDate.Text, dateEndDate.Text).Tables(0)
        Grid2.DataSource = sicon.NmetalSampleWareInventoryAnnal2_GetList(strPM_M_Code, Nothing, strD_ID).Tables(0)

    End Sub
#End Region


#Region "转Excel程序"
    Private Sub cmdExcelSub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelSub.Click, cmdExcel.Click
        GetInExcel()
    End Sub

    Private Sub GetInExcel()
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 0
                If GridView1.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid1, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
            Case 1
                If GridView3.RowCount = 0 Then Exit Sub
                Dim saveFileDialog As New SaveFileDialog()
                saveFileDialog.Title = "導出Excel"
                saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
                Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
                If FiledialogResult = Windows.Forms.DialogResult.OK Then
                    If ExportToExcelOld(Grid2, saveFileDialog.FileName) Then
                        MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
                    End If
                End If
        End Select
    End Sub
#End Region

    Private Sub cmdPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrint.Click
        Select Case XtraTabControl1.SelectedTabPageIndex
            Case 2
                Dim dss As New DataSet
                dss = sicon.NmetalSampleInventoryTatalB_GetList(strPM_M_Code, strTID, BoolZeroShow)
                PreviewRPT1(dss, "rptSampleInventoryTatalBPrint", "产品资料汇总", InUser, InUser, True, True)
            Case 3
                Dim dss As New DataSet
                dss = sicon.NmetalSampleInventoryTatalC_GetList(strPM_M_Code, strTID, BoolZeroShow)
                PreviewRPT1(dss, "rptSampleInventoryTatalCPrint", "材质汇总", InUser, InUser, True, True)
        End Select
    End Sub

End Class