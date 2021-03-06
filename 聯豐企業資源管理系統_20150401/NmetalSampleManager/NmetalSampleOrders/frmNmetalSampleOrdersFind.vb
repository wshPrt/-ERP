Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Public Class frmNmetalSampleOrdersFind

#Region "属性"
    Dim ds As New DataSet
    Dim somcon As New NmetalSampleOrdersMainControler
    Private _EditItem As String
    Private EditType As String
    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
#End Region

    Private Sub frmSampleOrdersFind_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTeble()
        Select Case EditItem
            Case "cmdSampType"
                Me.Text = "样办阶段設置"
                Me.Label1.Text = Me.Text
                GridColumn4.Visible = False

                FillDataSourec()
            Case "cmdMaterialType"
                Me.Text = "样办材料設置"
                Me.Label1.Text = Me.Text
                FillDataSourec()
        End Select
    End Sub


    Sub FillDataSourec()
        ds.Tables("SampTypeTable").Clear()
        Select Case EditItem
            Case "cmdSampType"
                Dim somlist As New List(Of NmetalSampleOrdersMainInfo)
                somlist = somcon.NmetalSampleOrdersType_GetList(Nothing, "T", "True", Nothing)
                For i As Integer = 0 To somlist.Count - 1
                    Dim row As DataRow
                    row = ds.Tables("SampTypeTable").NewRow
                    row("SampNo") = somlist(i).TID
                    row("SampType") = somlist(i).TName
                    row("AutoID") = somlist(i).AutoID
                    row("TMaterialType") = somlist(i).TMaterialType
                    ds.Tables("SampTypeTable").Rows.Add(row)
                Next
                Me.Grid1.DataSource = ds.Tables("SampTypeTable")
            Case "cmdMaterialType"
                Dim somlist As New List(Of NmetalSampleOrdersMainInfo)
                somlist = somcon.NmetalSampleOrdersType_GetList(Nothing, "M", "True", Nothing)
                For i As Integer = 0 To somlist.Count - 1
                    Dim row As DataRow
                    row = ds.Tables("SampTypeTable").NewRow
                    row("SampNo") = somlist(i).TID
                    row("SampType") = somlist(i).TName
                    row("AutoID") = somlist(i).AutoID
                    row("TMaterialType") = somlist(i).TMaterialType
                    ds.Tables("SampTypeTable").Rows.Add(row)
                Next
                Me.Grid1.DataSource = ds.Tables("SampTypeTable")
        End Select
    End Sub

    Sub CreateTeble()
        With ds.Tables.Add("SampTypeTable") '子配件表
            .Columns.Add("SampNo", GetType(String))
            .Columns.Add("SampType", GetType(String))
            .Columns.Add("TMaterialType", GetType(String))
            .Columns.Add("AutoID", GetType(String))
        End With
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Me.txt_No.Enabled = False
        Me.txt_No.Text = "自动编号"
        Me.txtType.Enabled = True
        Me.txtType.Text = String.Empty

        Select Case EditItem
            Case "cmdSampType"
                Label3.Enabled = False
                cobTMaterialType.Enabled = False
            Case "cmdMaterialType"
                Label3.Enabled = True
                cobTMaterialType.Enabled = True
        End Select

        EditType = "Add"
    End Sub

    Private Sub cmdRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRef.Click
        Me.txt_No.Enabled = False
        Me.txtType.Enabled = False
        Me.cobTMaterialType.Enabled = False
        FillDataSourec()
    End Sub

    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        If Me.txt_No.Text = String.Empty Then
            MessageBox.Show("修改時編號不能為空", "提示")
            Me.txt_No.Focus()
            Exit Sub
        End If

        Me.txt_No.Enabled = False
        Me.txtType.Enabled = True
        Me.cobTMaterialType.Enabled = True

        Select Case EditItem
            Case "cmdSampType"
                Label3.Enabled = False
                cobTMaterialType.Enabled = False
            Case "cmdMaterialType"
                Label3.Enabled = True
                cobTMaterialType.Enabled = True
        End Select

        EditType = "Edit"
    End Sub

    Private Sub cmdSava_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSava.Click
        If EditType = "Add" Or EditType = "Edit" Then
            If txtType.Text <> String.Empty Then
                Dim strNO As String = txtType.Text
                Dim somlistA As New List(Of NmetalSampleOrdersMainInfo)

                Select Case EditItem
                    Case "cmdSampType"
                        somlistA = somcon.NmetalSampleOrdersType_GetList(Nothing, "T", "True", strNO)
                    Case "cmdMaterialType"
                        somlistA = somcon.NmetalSampleOrdersType_GetList(Nothing, "M", "True", strNO)
                End Select

                If somlistA.Count > 0 Then
                    If EditType = "Add" Then
                        MessageBox.Show("类型相同,請重新輸入！", "提示")
                        Me.txtType.Text = String.Empty
                        Me.txtType.Focus()
                        Exit Sub
                    Else
                        If txt_No.Text <> somlistA(0).TID Then
                            MessageBox.Show("类型相同,請重新輸入！", "提示")
                            Me.txtType.Text = String.Empty
                            Me.txtType.Focus()
                            Exit Sub
                        End If
                    End If
                End If
            End If
        End If

        If EditType = String.Empty Then
            MessageBox.Show("不是新增修改狀態", "提示")
            Exit Sub
        End If
        If Me.txt_No.Text = String.Empty Then
            MessageBox.Show("編號不能為空", "提示")
            Me.txt_No.Focus()
            Exit Sub
        End If
        If Me.txtType.Text = String.Empty Then
            MessageBox.Show("類型不能為空", "提示")
            Me.txtType.Focus()
            Exit Sub
        End If
        If txtType.Text.Length > 30 Then
            MessageBox.Show("类型长度输入太长", "提示")
            Me.txtType.Focus()
            Exit Sub
        End If
        '---------------------------------------------------
        Select Case EditItem
            Case "cmdSampType"
                Select Case EditType
                    Case "Add"

                        Dim somlist As New NmetalSampleOrdersMainInfo
                        somlist.TEnable = True
                        somlist.TID = GetTypeID("T")
                        somlist.TName = Me.txtType.Text
                        somlist.TType = "T"
                        If somcon.NmetalSampleOrdersType_Add(somlist) = False Then
                            MessageBox.Show("样办阶段新增錯誤", "提示")
                            Exit Sub
                        Else
                            MessageBox.Show("样办阶段新增成功", "提示")
                        End If

                    Case "Edit"

                        Dim somlist As New NmetalSampleOrdersMainInfo
                        somlist.TEnable = True
                        somlist.TID = Me.txt_No.Text
                        somlist.TName = Me.txtType.Text
                        somlist.TType = "T"
                        somlist.AutoID = GridView1.GetFocusedRowCellValue("AutoID").ToString()
                        If somcon.NmetalSampleOrdersType_Update(somlist) = False Then
                            MessageBox.Show("样办阶段修改錯誤", "提示")
                            Exit Sub
                        Else
                            MessageBox.Show("样办阶段修改成功", "提示")
                        End If

                End Select
            Case "cmdMaterialType"
                'If Me.cobTMaterialType.Text = String.Empty Then
                '    MessageBox.Show("产品類型不能為空", "提示")
                '    Me.cobTMaterialType.Focus()
                '    Exit Sub
                'End If

                Select Case EditType
                    Case "Add"

                        Dim somlist As New NmetalSampleOrdersMainInfo
                        somlist.TEnable = True
                        somlist.TID = GetTypeID("M")
                        somlist.TName = Me.txtType.Text
                        somlist.TType = "M"
                        somlist.TMaterialType = cobTMaterialType.EditValue
                        If somcon.NmetalSampleOrdersType_Add(somlist) = False Then
                            MessageBox.Show("样办材料新增錯誤", "提示")
                            Exit Sub
                        Else
                            MessageBox.Show("样办材料新增成功", "提示")
                        End If

                    Case "Edit"

                        Dim somlist As New NmetalSampleOrdersMainInfo
                        somlist.TEnable = True
                        somlist.TID = Me.txt_No.Text
                        somlist.TName = Me.txtType.Text
                        somlist.TType = "M"
                        somlist.TMaterialType = cobTMaterialType.EditValue
                        somlist.AutoID = GridView1.GetFocusedRowCellValue("AutoID").ToString()
                        If somcon.NmetalSampleOrdersType_Update(somlist) = False Then
                            MessageBox.Show("样办材料修改錯誤", "提示")
                            Exit Sub
                        Else
                            MessageBox.Show("样办材料修改成功", "提示")
                        End If
                End Select
        End Select
        Me.txt_No.Enabled = False
        Me.txtType.Enabled = False
        Me.cobTMaterialType.Enabled = False
        EditType = String.Empty
    End Sub


    Private Sub Grid1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Grid1.Click
        Me.txt_No.Text = GridView1.GetFocusedRowCellValue("SampNo").ToString()
        Me.txtType.Text = GridView1.GetFocusedRowCellValue("SampType").ToString()
        Me.cobTMaterialType.Text = GridView1.GetFocusedRowCellValue("TMaterialType").ToString()
    End Sub


    ''' <summary>
    ''' 自動新增流水号排期单号
    ''' </summary>
    Function GetTypeID(ByVal strType As String) As String
        Dim oc As New NmetalSampleOrdersMainControler
        Dim oi As New NmetalSampleOrdersMainInfo
        oi = oc.NmetalSampleOrdersType_GetID(strType)
        If oi Is Nothing Then
            GetTypeID = strType + "01"
        Else
            GetTypeID = strType + Mid(CStr(Mid(oi.TID, 2) + 1001), 3)
        End If
    End Function

    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        Me.txt_No.Text = GridView1.GetFocusedRowCellValue("SampNo").ToString()
        Me.txtType.Text = GridView1.GetFocusedRowCellValue("SampType").ToString()
        Me.cobTMaterialType.Text = GridView1.GetFocusedRowCellValue("TMaterialType").ToString()
    End Sub
End Class