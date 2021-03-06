Imports LFERP.Library.SampleManager.SampleOrdersMain
Imports LFERP.Library.SampleManager.SampleSend
Public Class frmSampleOrdersBack
    Dim ssbcon As New SampleSendBackControler
    Dim somcon As New SampleOrdersMainControler
    Private Sub frmSampleOrdersBack_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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

    Private Sub gluPM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluPM_M_Code.EditValueChanged
        If gluPM_M_Code.EditValue <> String.Empty Then
            Dim solist As New List(Of SampleOrdersMainInfo)
            solist = somcon.SampleOrdersMain_GetList(gluPM_M_Code.EditValue, Nothing, Nothing, Nothing, Nothing, Nothing, False)
            If solist.Count > 0 Then
                Me.txtSO_SampleID.Text = solist(0).SO_SampleID
                Me.txtSO_ID.Text = solist(0).SO_ID
                Me.txtM_Code_Type.Text = solist(0).M_Code_Type
            Else
                Me.txtSO_SampleID.Text = "ALL"
                Me.txtSO_ID.Text = "ALL"
                Me.txtM_Code_Type.Text = "ALL"
            End If
        End If
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        If Me.gluPM_M_Code.EditValue = String.Empty Then
            MsgBox("产品编号不能为代,请您输入产品编号", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If

        If txtSO_ID.EditValue = String.Empty Then
            MsgBox("订单编号不能为代,请您输入订单编号", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If

        Dim strSO_ID As String = String.Empty
        If txtSO_ID.Text = "ALL" Then
            strSO_ID = Nothing
        Else
            strSO_ID = txtSO_ID.Text
        End If

        Dim ssblist As New List(Of SampleSendBackInfo)
        ssblist = ssbcon.SampleSendBack_GetList(strSO_ID, Nothing)
        If ssblist.Count > 0 Then
            Grid1.DataSource = ssblist
        Else
            Grid1.DataSource = Nothing
        End If

    End Sub

    Private Sub cmdExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcel.Click
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
    End Sub
End Class