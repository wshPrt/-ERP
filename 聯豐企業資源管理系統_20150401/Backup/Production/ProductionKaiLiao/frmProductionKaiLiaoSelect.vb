Imports LFERP.Library.ProductionKaiLiao
Imports LFERP.DataSetting
Imports LFERP.Library.Product
Imports LFERP.Library.ProductProcess


Public Class frmProductionKaiLiaoSelect

    Dim ds As New DataSet

    Sub LoadProductNo()  '產品編號

        Dim mc As New ProcessMainControl
        GluPM_M_Code.Properties.DisplayMember = "PM_M_Code"
        GluPM_M_Code.Properties.ValueMember = "PM_M_Code"
        GluPM_M_Code.Properties.DataSource = mc.ProcessMain_GetList3(Nothing, Nothing)

    End Sub

    Private Sub frmProductionKaiLiaoSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable()

        LoadProductNo()
        DateEdit1.Text = Format(Now, "yyyy/MM/dd")
        DateEdit2.Text = Format(Now, "yyyy/MM/dd")

        tempValue = Nothing
    End Sub

    Sub CreateTable()
        ds.Tables.Clear()

        With ds.Tables.Add("ProductType")
            .Columns.Add("PM_Type", GetType(String))
        End With
        GluType.Properties.ValueMember = "PM_Type"
        GluType.Properties.DisplayMember = "PM_Type"
        GluType.Properties.DataSource = ds.Tables("ProductType")

    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        tempValue = "單擊查詢按鈕"

        If txtNO.Text = "" Then
            tempValue2 = Nothing
        Else
            tempValue2 = txtNO.Text
        End If
        If ComboBoxEdit1.EditValue = "" Then
            tempValue3 = Nothing
        Else
            tempValue3 = ComboBoxEdit1.EditValue
        End If
        If GluPM_M_Code.EditValue = "" Then
            tempValue4 = Nothing
        Else
            tempValue4 = GluPM_M_Code.EditValue
        End If
        If GluType.EditValue = "" Then
            tempValue5 = Nothing
        Else
            tempValue5 = GluType.EditValue
        End If
        If txtAction.Text = "" Then
            tempValue6 = Nothing
        Else
            tempValue6 = txtAction.Text
        End If
        If txtName.Text = "" Then
            tempValue7 = Nothing
        Else
            tempValue7 = txtName.Text
        End If
        If DateEdit1.Text = "" Then
            tempValue8 = Nothing
        Else
            tempValue8 = DateEdit1.Text
        End If
        If DateEdit2.Text = "" Then
            tempValue9 = Nothing
        Else
            tempValue9 = DateEdit2.Text
        End If
        Me.Close()
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub GluPM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GluPM_M_Code.EditValueChanged

        On Error Resume Next

        Dim ppc As New ProcessMainControl
        Dim ppi As List(Of ProcessMainInfo)
        ds.Tables("ProductType").Clear()

        If ComboBoxEdit1.EditValue = "" Then
            'MsgBox("工藝類型不能為空!")
            Exit Sub
        End If

        If GluPM_M_Code.Text.Trim = "" Then   '@ 2012/8/10 添加
            ds.Tables("ProductType").Clear()
            Exit Sub
        End If

        ppi = ppc.ProcessMain_GetList2(ComboBoxEdit1.EditValue, GluPM_M_Code.EditValue)
        If ppi.Count = 0 Then
        Else

            Dim i As Integer
            For i = 0 To ppi.Count - 1
                Dim row As DataRow
                row = ds.Tables("ProductType").NewRow
                row("PM_Type") = ppi(i).Type3ID
                ds.Tables("ProductType").Rows.Add(row)

            Next
        End If
    End Sub

    '@ 2012/1/5 添加當控件內容發生改變，加載相應的內容到GluType控件
    '些過程調用以下過程：
    'GluPM_M_Code_EditValueChanged()
    Private Sub ComboBoxEdit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxEdit1.SelectedIndexChanged

        GluPM_M_Code_EditValueChanged(Nothing, Nothing)

    End Sub

    '@ 2012/8/10 添加 按空格鍵彈出下拉菜單
    Private Sub GluType_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles GluType.KeyDown, ComboBoxEdit1.KeyDown, GluPM_M_Code.KeyDown
        If e.KeyCode = Keys.Space Then
            sender.ShowPopup()
        End If
    End Sub
End Class