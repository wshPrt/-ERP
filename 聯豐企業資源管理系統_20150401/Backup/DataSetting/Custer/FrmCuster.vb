Imports LFERP.DataSetting
Imports LFERP.SystemManager
Public Class FrmCuster

    Private Sub FrmCuster_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        cmdRef_Click(Nothing, Nothing)
    End Sub

    Private Sub cmdDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        '-----------------------------------------------
        Dim mtd As New CusterControler

        If MsgBox("確定要刪除客戶代號為： '" & GridView1.GetFocusedRowCellValue("C_CusterID").ToString & "' 的記錄嗎?", MsgBoxStyle.YesNo, "提示") = MsgBoxResult.Yes Then

            If mtd.GetCuster_Delete(GridView1.GetFocusedRowCellValue("C_CusterID").ToString) = True Then

                MsgBox("刪除當前記錄信息成功！", 60, "提示")
                Me.Grid1.DataSource = mtd.GetCusterList(Nothing, Nothing, Nothing)
            Else
                MsgBox("刪除當前選定記錄失敗，請檢查原因！", 60, "提示")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        FrmCusterAdd.Text = "客戶資料"
        FrmCusterAdd.Edititem = "Add"
        FrmCusterAdd.ShowDialog()
        FrmCusterAdd.Dispose()
    End Sub

    Private Sub cmdRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRef.Click
        Dim mtd As New CusterControler
        Me.Grid1.DataSource = mtd.GetCusterList(Nothing, Nothing, Nothing)
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        FrmCusterAdd.Text = "客戶資料"
        FrmCusterAdd.EditItem = "Edit"
        FrmCusterAdd.EditValue = GridView1.GetFocusedRowCellValue("C_CusterID").ToString
        FrmCusterAdd.ShowDialog()
        FrmCusterAdd.Dispose()
    End Sub
    '设置权限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900101")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdAdd.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900102")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdEdit.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900103")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdDel.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900104")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdCopy.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900105")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdCopyAll.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "900106")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdExportl.Enabled = True
        End If


    End Sub

    Private Sub View_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles View.Click
        FrmCusterAdd.Text = "客戶資料"
        FrmCusterAdd.EditItem = "View"
        FrmCusterAdd.EditValue = GridView1.GetFocusedRowCellValue("C_CusterID").ToString
        FrmCusterAdd.ShowDialog()
        FrmCusterAdd.Dispose()
    End Sub

    Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click
        FrmCusterAdd.Text = "客戶資料"
        FrmCusterAdd.EditItem = "Copy"
        FrmCusterAdd.EditValue = GridView1.GetFocusedRowCellValue("C_CusterID").ToString
        FrmCusterAdd.ShowDialog()
        FrmCusterAdd.Dispose()
    End Sub

    Private Sub cmdCopyAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopyAll.Click
        Dim FiledNameStr As String
        FiledNameStr = "C_CusterID,C_EngName,C_ChsName,C_Link,C_LinkTel,C_Fax,C_AddDate,C_Adder1,C_Adder2,C_Adder3,C_Department"
        GridViewCopyMulitRow(GridView1, FiledNameStr, "ALL")
    End Sub

    Private Sub cmdExportl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExportl.Click
        If GridView1.RowCount = 0 Then Exit Sub

        Dim saveFileDialog As New SaveFileDialog()

        saveFileDialog.Title = "導出Excel"

        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"

        Dim dialogResult__1 As DialogResult = saveFileDialog.ShowDialog(Me)

        If dialogResult__1 = Windows.Forms.DialogResult.OK Then

            GridView1.BestFitColumns()

            Grid1.ExportToExcelOld(saveFileDialog.FileName)

            DevExpress.XtraEditors.XtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information)

        End If

    End Sub

    Private Sub cmdcontext_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles cmdcontext.Opening

    End Sub
End Class