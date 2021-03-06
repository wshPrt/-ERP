Imports LFERP.Library.Outward
Imports LFERP.Library.WareHouse
Imports LFERP.Library.WareHouse.WareOut
Imports LFERP.Library.Purchase.SharePurchase
Imports LFERP.Library.Outward.OutwardAcceptance
Imports LFERP.Library.Orders

Public Class frmOutward

    Dim ds As New DataSet
    Dim strWHID As String

    Private Sub frmOutward_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label24.Text = tempValue
        txtOMID.Text = tempValue2
        OwType.EditValue = tempValue3
        Label6.Text = tempValue4

        tempValue = ""
        tempValue2 = ""
        tempValue3 = ""
        tempValue4 = ""
        '-----------------------------------------------------------導入供應商
        Dim mtd As New LFERP.DataSetting.SuppliersControler
        gluSupplier.Properties.DisplayMember = "S_SupplierName"
        gluSupplier.Properties.ValueMember = "S_Supplier"
        gluSupplier.Properties.DataSource = mtd.GetSuppliersList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "True")
        '-----------------------------------------------------------導入加工類型
        Dim oc As New OutwardController
        Me.RepositoryItemLookUpEdit1.DataSource = oc.LookUpEdit_Get("外發加工").Tables(0)
        Me.RepositoryItemLookUpEdit1.ValueMember = "OT_NO"
        Me.RepositoryItemLookUpEdit1.DisplayMember = "OT_Name"
        '------------------------------------------------------------導入外發加工屬性

        CreateTable()

        Select Case Label6.Text

            Case "外發加工作業"

                Select Case Label24.Text

                    Case "外發加工單"
                        If Edit = True Then
                            Me.Text = "外發加工單--修改" & "[" & txtOMID.Text & "]"
                            LoadData(txtOMID.Text)
                            getenable(True, False, False)

                            txtOMID.Enabled = False
                            OwType.Enabled = False
                            OwDate.Enabled = False
                            WhCode.Enabled = False

                        Else
                            getenable(True, False, False)
                            Me.Text = "外發加工單--新增"
                            OwDate.EditValue = Format(Now, "yyyy/MM/dd")
                            txtOMID.Text = ""
                            txtOMID.Enabled = False
                            OwType.Enabled = False
                            strWHID = "W0701"
                            WhCode.EditValue = "A區"
                        End If
                        XtraTabControl1.SelectedTabPage = XtraTabPage1

                    Case "查看"
                        Me.Text = "外發加工單--查看" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        cmdSave.Visible = False
                        getenable(False, False, False)
                        XtraTabControl1.SelectedTabPage = XtraTabPage1
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                    Case "審核"
                        Me.Text = "審核" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        XtraTabControl1.SelectedTabPage = XtraTabPage2
                        getenable(False, True, False)
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                    Case "復核"
                        Me.Text = "復核" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        XtraTabControl1.SelectedTabPage = XtraTabPage3
                        getenable(False, False, True)
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                End Select

            Case "外發返修作業"

                Select Case Label24.Text

                    Case "外發返修"
                        If Edit = True Then
                            Me.Text = "外發返修單--修改" & "[" & txtOMID.Text & "]"
                            LoadData(txtOMID.Text)
                            getenable(True, False, False)

                            txtOMID.Enabled = False
                            OwType.Enabled = False
                            OwDate.Enabled = False
                            WhCode.Enabled = False

                        Else
                            getenable(True, False, False)
                            Me.Text = "外發返修單--新增"
                            OwDate.EditValue = Format(Now, "yyyy/MM/dd")
                            txtOMID.Text = ""
                            txtOMID.Enabled = False
                            OwType.Enabled = False
                            strWHID = "W0701"
                            WhCode.EditValue = "A區"
                        End If
                        XtraTabControl1.SelectedTabPage = XtraTabPage1

                    Case "查看"
                        Me.Text = "外發返修單--查看" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        cmdSave.Visible = False
                        getenable(False, False, False)
                        XtraTabControl1.SelectedTabPage = XtraTabPage1
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                    Case "審核"
                        Me.Text = "審核" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        XtraTabControl1.SelectedTabPage = XtraTabPage2
                        getenable(False, True, False)
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                    Case "復核"

                        Me.Text = "復核" & "[" & txtOMID.Text & "]"
                        LoadData(txtOMID.Text)
                        txtOMID.Enabled = False
                        XtraTabControl1.SelectedTabPage = XtraTabPage3
                        getenable(False, False, True)
                        GridView1.OptionsBehavior.AutoSelectAllInEditor = False
                        GridView1.OptionsBehavior.Editable = False
                        GridView1.OptionsSelection.EnableAppearanceFocusedCell = False
                End Select
                Label1.Text = "返修作業"

        End Select

    End Sub
    Sub CreateTable()
        ds.Tables.Clear()

        With ds.Tables.Add("Outward")

            .Columns.Add("O_NO", GetType(String))
            .Columns.Add("O_Type", GetType(String))
            .Columns.Add("O_Date", GetType(String))
            .Columns.Add("O_Action", GetType(String))
            .Columns.Add("WH_ID", GetType(String))
            .Columns.Add("O_Remark", GetType(String))
            .Columns.Add("O_NOsub", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("M_Gauge", GetType(String))
            .Columns.Add("OS_Qty", GetType(Double))
            .Columns.Add("OS_NoSendQty", GetType(Double))
            .Columns.Add("OS_Price", GetType(Double))
            .Columns.Add("OS_ItemType", GetType(String))
            .Columns.Add("OS_Depict", GetType(String))
            .Columns.Add("OS_BatchID", GetType(String))
            .Columns.Add("OS_Remark", GetType(String))
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("OP_NO", GetType(String))
            .Columns.Add("ExtraName", GetType(String))

        End With
        '創建刪除數據表
        With ds.Tables.Add("DelData")

            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("O_NOsub", GetType(String))

        End With
        '綁定表格
        Grid.DataSource = ds.Tables("Outward")

    End Sub
    Function LoadData(ByVal O_NO As String) As Boolean
        LoadData = True
        Dim oi As New List(Of OutwardInfo)
        Dim oc As New OutwardController

        Try
            oi = oc.OutwardMain_GetList(O_NO, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If oi Is Nothing Then
                '沒有數據
                LoadData = False
                Exit Function
            End If


            OwType.EditValue = oi(0).O_Type
            OwDate.DateTime = oi(0).O_Date
            strWHID = oi(0).WH_ID
            WhCode.EditValue = oi(0).WH_Name
            txtRemark.Text = oi(0).O_Remark
            gluSupplier.Text = oi(0).S_Supplier

            oi(0).O_Action = InUserID
            If oi(0).O_Check = False Then
                CheckEdit1.Checked = False
            Else
                CheckEdit1.Checked = True
            End If

            CheckDate.Text = oi(0).O_CheckDate
            CheckAction.Text = oi(0).CheckActionName
            CheckRemark.Text = oi(0).O_CheckRemark
            If oi(0).O_AccCheck = False Then
                CheckEdit2.Checked = False
            Else
                CheckEdit2.Checked = True
            End If

            ACheckType.EditValue = oi(0).O_AccCheckType
            ACdate.Text = oi(0).O_AccCheckDate
            ACAction.Text = oi(0).AccCheckActionName
            MemoEdit2.Text = oi(0).O_AccCheckRemark

            ds.Tables("Outward").Rows.Clear()
            Loadsub(oc.OutwardSub_GetList(O_NO, Nothing, Nothing, Nothing, Nothing))

        Catch ex As Exception
            LoadData = False
            MsgBox(ex.Message)
        End Try

    End Function
    Sub Loadsub(ByVal dlist As List(Of OutwardInfo))

        On Error Resume Next

        If dlist Is Nothing Then Exit Sub

        Dim i As Integer
        Dim row As DataRow

        For i = 0 To dlist.Count - 1
            row = ds.Tables("Outward").NewRow

            row("O_NO") = dlist(i).O_NO
            row("O_NOsub") = dlist(i).O_NOsub
            row("OS_BatchID") = dlist(i).OS_BatchID

            Dim oc As New OrdersBomController
            Me.RepositoryItemLookUpEdit2.DataSource = oc.OrdersBom_GetList(Nothing, dlist(i).OS_BatchID, Nothing, 0)
            Me.RepositoryItemLookUpEdit2.ValueMember = "M_Code"
            Me.RepositoryItemLookUpEdit2.DisplayMember = "M_Code"

            row("M_Code") = dlist(i).M_Code
            row("M_Name") = dlist(i).M_Name
            row("M_Gauge") = dlist(i).M_Gauge

            row("OS_Qty") = dlist(i).OS_Qty
            row("OS_NoSendQty") = dlist(i).OS_NoSendQty
            row("OS_Price") = dlist(i).OS_Price
            row("OS_ItemType") = dlist(i).OS_ItemType
            row("OS_Depict") = dlist(i).OS_Depict

            row("OS_Remark") = dlist(i).OS_Remark
            row("PM_M_Code") = dlist(i).PM_M_Code
            row("OP_NO") = dlist(i).OP_NO
            row("ExtraName") = dlist(i).ExtraName

            ds.Tables("Outward").Rows.Add(row)
        Next

    End Sub
    Sub SaveNew()
        Dim oi As New OutwardInfo
        Dim oc As New OutwardController
        If CheckData() = True Then
        Else
            Exit Sub
        End If

        txtOMID.Text = GetNum()

        If Len(txtOMID.Text) = False Then
            MsgBox("不能生成單號，無法保存")
            Exit Sub
        End If
        oi.O_NO = txtOMID.Text
        oi.O_Type = OwType.EditValue
        oi.O_Date = OwDate.DateTime
        oi.WH_ID = strWHID
        'oi.WH_ID = WhCode.Text
        oi.O_Remark = txtRemark.Text
        oi.O_Action = InUserID
        oi.S_Supplier = gluSupplier.EditValue
        If oc.OutwardMain_Add(oi) = False Then
            MsgBox("保存失敗!")
            Exit Sub
        End If
        For i As Integer = 0 To ds.Tables("OutWard").Rows.Count - 1

            oi.O_NOsub = GetNo()

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                oi.OS_Qty = Nothing
            Else
                oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
            End If
           
            oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                oi.OS_Price = Nothing
            Else
                oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                oi.OS_BatchID = Nothing
            Else
                oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                oi.M_Code = Nothing
            Else
                oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                oi.OS_ItemType = Nothing
            Else
                oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                oi.OS_Remark = Nothing
            Else
                oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                oi.OS_Depict = Nothing
            Else
                oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                oi.PM_M_Code = Nothing
            Else
                oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                oi.OP_NO = Nothing
            Else
                oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                oi.ExtraName = Nothing
            Else
                oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
            End If
            If oc.OutwardSub_Add(oi) = False Then
                MsgBox("保存失敗!")
                Exit Sub
            End If
        Next
        Me.Close()
    End Sub
    Sub SaveEdit()

        Dim oi As New OutwardInfo
        Dim oc As New OutwardController

        If CheckData() = True Then
        Else
            Exit Sub
        End If
        If ds.Tables("Outward").Rows.Count = 0 Then
            MsgBox("請選擇外發物料！")
            Exit Sub
        End If

        '更新刪除的記錄
        If ds.Tables("DelData").Rows.Count > 0 Then
            For i As Integer = 0 To ds.Tables("DelData").Rows.Count - 1
                If Not IsDBNull(ds.Tables("DelData").Rows(i)("O_NOsub")) Then

                    oc.OutwardSub_Delete(Nothing, ds.Tables("DelData").Rows(i)("O_NOsub"))

                End If
            Next i
        End If

        oi.O_NO = txtOMID.Text
        oi.O_Type = OwType.EditValue
        oi.O_Date = OwDate.EditValue

        oi.WH_ID = strWHID
        'oi.WH_ID = WhCode.Text
        oi.O_Remark = txtRemark.Text
        oi.O_Action = InUserID
        oi.S_Supplier = gluSupplier.EditValue

        If oc.OutwardMain_Update(oi) = False Then
            MsgBox("保存失敗!")
            Exit Sub
        End If
        
        For i As Integer = 0 To ds.Tables("OutWard").Rows.Count - 1
            If IsDBNull(ds.Tables("OutWard").Rows(i)("O_NOsub")) Then '有流水號新增

                oi.O_NOsub = GetNo()

                If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                    oi.M_Code = Nothing
                Else
                    oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                    oi.OS_Qty = 0
                Else
                    oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
                End If
              
                oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                    oi.OS_Price = 0
                Else
                    oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                    oi.OS_ItemType = Nothing
                Else
                    oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                    oi.OS_Depict = Nothing
                Else
                    oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                    oi.OS_BatchID = Nothing
                Else
                    oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                    oi.OS_Remark = Nothing
                Else
                    oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                    oi.PM_M_Code = Nothing
                Else
                    oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                    oi.OP_NO = Nothing
                Else
                    oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                    oi.ExtraName = Nothing
                Else
                    oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
                End If
                If oc.OutwardSub_Add(oi) = False Then
                    MsgBox("保存失敗!")
                    Exit Sub
                End If
            ElseIf Not IsDBNull(ds.Tables("OutWard").Rows(i)("O_NOsub")) Then '修改


                oi.O_NOsub = ds.Tables("OutWard").Rows(i)("O_NOsub")
                oi.O_NO = ds.Tables("OutWard").Rows(i)("O_NO")
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                    oi.OS_Qty = Nothing
                Else
                    oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
                End If
               
                oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_NoSendQty"))

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                    oi.OS_Price = Nothing
                Else
                    oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                    oi.OS_BatchID = Nothing
                Else
                    oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                    oi.M_Code = Nothing
                Else
                    oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                    oi.OS_ItemType = Nothing
                Else
                    oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                    oi.OS_Remark = Nothing
                Else
                    oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                    oi.OS_Depict = Nothing
                Else
                    oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                    oi.PM_M_Code = Nothing
                Else
                    oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                    oi.OP_NO = Nothing
                Else
                    oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                    oi.ExtraName = Nothing
                Else
                    oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
                End If
                If oc.OutwardSub_Update(oi) = False Then
                    MsgBox("保存失敗!")
                    Exit Sub
                End If
            End If
        Next
        Me.Close()
    End Sub

    Sub ReWorkSaveNew()
        Dim oi As New OutwardInfo
        Dim oc As New OutwardController
        If CheckData() = True Then
        Else
            Exit Sub
        End If

        txtOMID.Text = GetNum1()

        If Len(txtOMID.Text) = False Then
            MsgBox("不能生成單號，無法保存")
            Exit Sub
        End If
        oi.O_NO = txtOMID.Text
        oi.O_Type = OwType.EditValue
        oi.O_Date = OwDate.DateTime
        oi.WH_ID = strWHID
        'oi.WH_ID = WhCode.Text
        oi.O_Remark = txtRemark.Text
        oi.O_Action = InUserID
        oi.S_Supplier = gluSupplier.EditValue
        If oc.OutwardMain_Add(oi) = False Then
            MsgBox("保存失敗!")
            Exit Sub
        End If
        For i As Integer = 0 To ds.Tables("OutWard").Rows.Count - 1

            oi.O_NOsub = GetNo()

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                oi.OS_Qty = Nothing
            Else
                oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
            End If

            oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                oi.OS_Price = Nothing
            Else
                oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                oi.OS_BatchID = Nothing
            Else
                oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                oi.M_Code = Nothing
            Else
                oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                oi.OS_ItemType = Nothing
            Else
                oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                oi.OS_Remark = Nothing
            Else
                oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
            End If

            If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                oi.OS_Depict = Nothing
            Else
                oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                oi.PM_M_Code = Nothing
            Else
                oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                oi.OP_NO = Nothing
            Else
                oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
            End If
            If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                oi.ExtraName = Nothing
            Else
                oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
            End If

            If oc.OutwardSub_Add(oi) = False Then
                MsgBox("保存失敗!")
                Exit Sub
            End If
        Next
        Me.Close()
    End Sub
    Sub ReWorkSaveEdit()
        Dim oi As New OutwardInfo
        Dim oc As New OutwardController

        If CheckData() = True Then
        Else
            Exit Sub
        End If
        If ds.Tables("Outward").Rows.Count = 0 Then
            MsgBox("請選擇外發物料！")
            Exit Sub
        End If

        '更新刪除的記錄
        If ds.Tables("DelData").Rows.Count > 0 Then
            For i As Integer = 0 To ds.Tables("DelData").Rows.Count - 1
                If Not IsDBNull(ds.Tables("DelData").Rows(i)("O_NOsub")) Then

                    oc.OutwardSub_Delete(Nothing, ds.Tables("DelData").Rows(i)("O_NOsub"))

                End If
            Next i
        End If

        oi.O_NO = txtOMID.Text
        oi.O_Type = OwType.EditValue
        oi.O_Date = OwDate.EditValue

        oi.WH_ID = strWHID
        'oi.WH_ID = WhCode.Text
        oi.O_Remark = txtRemark.Text
        oi.O_Action = InUserID
        oi.S_Supplier = gluSupplier.EditValue

        If oc.OutwardMain_Update(oi) = False Then
            MsgBox("保存失敗!")
            Exit Sub
        End If

        For i As Integer = 0 To ds.Tables("OutWard").Rows.Count - 1
            If IsDBNull(ds.Tables("OutWard").Rows(i)("O_NOsub")) Then '有流水號新增

                oi.O_NOsub = GetNo()

                If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                    oi.M_Code = Nothing
                Else
                    oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                    oi.OS_Qty = 0
                Else
                    oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
                End If

                oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                    oi.OS_Price = 0
                Else
                    oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                    oi.OS_ItemType = Nothing
                Else
                    oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                    oi.OS_Depict = Nothing
                Else
                    oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                    oi.OS_BatchID = Nothing
                Else
                    oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                    oi.OS_Remark = Nothing
                Else
                    oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                    oi.PM_M_Code = Nothing
                Else
                    oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                    oi.OP_NO = Nothing
                Else
                    oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                    oi.ExtraName = Nothing
                Else
                    oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
                End If

                If oc.OutwardSub_Add(oi) = False Then
                    MsgBox("保存失敗!")
                    Exit Sub
                End If
            ElseIf Not IsDBNull(ds.Tables("OutWard").Rows(i)("O_NOsub")) Then '修改


                oi.O_NOsub = ds.Tables("OutWard").Rows(i)("O_NOsub")
                oi.O_NO = ds.Tables("OutWard").Rows(i)("O_NO")
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Qty")) Then
                    oi.OS_Qty = Nothing
                Else
                    oi.OS_Qty = CDbl(ds.Tables("OutWard").Rows(i)("OS_Qty"))
                End If

                oi.OS_NoSendQty = CDbl(ds.Tables("OutWard").Rows(i)("OS_NoSendQty"))

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Price")) Then
                    oi.OS_Price = Nothing
                Else
                    oi.OS_Price = CDbl(ds.Tables("OutWard").Rows(i)("OS_Price"))
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_BatchID")) Then
                    oi.OS_BatchID = Nothing
                Else
                    oi.OS_BatchID = ds.Tables("OutWard").Rows(i)("OS_BatchID")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("M_Code")) Then
                    oi.M_Code = Nothing
                Else
                    oi.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_ItemType")) Then
                    oi.OS_ItemType = Nothing
                Else
                    oi.OS_ItemType = ds.Tables("OutWard").Rows(i)("OS_ItemType")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Remark")) Then
                    oi.OS_Remark = Nothing
                Else
                    oi.OS_Remark = ds.Tables("OutWard").Rows(i)("OS_Remark")
                End If

                If IsDBNull(ds.Tables("OutWard").Rows(i)("OS_Depict")) Then
                    oi.OS_Depict = Nothing
                Else
                    oi.OS_Depict = ds.Tables("OutWard").Rows(i)("OS_Depict")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("PM_M_Code")) Then
                    oi.PM_M_Code = Nothing
                Else
                    oi.PM_M_Code = ds.Tables("OutWard").Rows(i)("PM_M_Code")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("OP_NO")) Then
                    oi.OP_NO = Nothing
                Else
                    oi.OP_NO = ds.Tables("OutWard").Rows(i)("OP_NO")
                End If
                If IsDBNull(ds.Tables("OutWard").Rows(i)("ExtraName")) Then
                    oi.ExtraName = Nothing
                Else
                    oi.ExtraName = ds.Tables("OutWard").Rows(i)("ExtraName")
                End If

                If oc.OutwardSub_Update(oi) = False Then
                    MsgBox("保存失敗!")
                    Exit Sub
                End If
            End If
        Next
        Me.Close()
    End Sub

    Sub UpdateCheck()
        Dim oi As New OutwardInfo
        Dim oc As New OutwardController

        oi.O_NO = txtOMID.Text
        oi.O_Check = CheckEdit1.Checked
        'CheckAction.Text = InUserID
        oi.O_CheckAction = InUserID
        CheckDate.Text = Format(Now, "yyyy/MM/dd")
        oi.O_CheckDate = CheckDate.Text
        oi.O_CheckRemark = CheckRemark.Text
        If oc.OutwardMain_UpdateCheck(oi) = True Then
            MsgBox("已保持審核信息！")
        Else
            MsgBox("審核失敗，請檢查原因！")
        End If

        '－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－暫停外發倉庫操作(入庫,出庫,扣數等)

        'Dim mt As New SharePurchaseController
        'Dim mm As New SharePurchaseInfo

        'mm.WH_ID = strWHID

        'For i As Integer = 0 To ds.Tables("OutWard").Rows.Count - 1
        '    mm.M_Code = ds.Tables("OutWard").Rows(i)("M_Code")

        '    Dim Qty As Single
        '    Dim wi As LFERP.Library.WareHouse.WareInventory.WareInventoryInfo
        '    Dim wc As New LFERP.Library.WareHouse.WareInventory.WareInventoryMTController
        '    wi = wc.WareInventory_GetSub(ds.Tables("OutWard").Rows(i)("M_Code"), strWHID)

        '    If wi Is Nothing Then
        '        Qty = 0
        '    Else
        '        Qty = wi.WI_Qty
        '    End If

        '    If CheckEdit1.Checked = False Then

        '        mm.WI_Qty = Qty + CSng(ds.Tables("OutWard").Rows(i)("OS_Qty"))

        '    ElseIf CheckEdit1.Checked = True Then

        '        mm.WI_Qty = Qty - CSng(ds.Tables("OutWard").Rows(i)("OS_Qty"))
        '    End If

        '    mt.UpdateWareInventory_WIQty2(mm)

        'Next
        '－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－－
        Me.Close()
    End Sub
    Sub UpdateAccCheck()
        Dim oi As New OutwardInfo
        Dim oc As New OutwardController

        oi.O_NO = txtOMID.Text
        oi.O_AccCheck = CheckEdit2.Checked
        ACdate.Text = Format(Now, "yyyy/MM/dd")
        'ACAction.Text = InUserID
        oi.O_AccCheckAction = InUserID
        oi.O_AccCheckDate = ACdate.Text
        oi.O_AccCheckRemark = MemoEdit2.Text
        oi.O_AccCheckType = ACheckType.EditValue

        If oc.OutwardMain_UpdateAccCheck(oi) = True Then
            MsgBox("已保持復核信息！")
        Else
            MsgBox("復核失敗，請檢查原因！")
        End If
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Select Case Label6.Text

            Case "外發加工作業"
                Select Case Label24.Text

                    Case "外發加工單"
                        If Edit = True Then
                            OwDate.Enabled = False
                            WhCode.Enabled = False
                            SaveEdit()
                        Else
                            SaveNew()
                        End If
                    Case "審核"
                        Dim oac As New OutwardAcceptanceControl
                        Dim oai As New List(Of OutwardAcceptanceInfo)
                        oai = oac.OutwardAcceptance_GetList(Nothing, Nothing, txtOMID.Text, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

                        If oai.Count <> 0 Then
                            MsgBox("此單已有驗收記錄，不能修改審核！")

                        Else
                            UpdateCheck()
                        End If

                    Case "復核"
                        UpdateAccCheck()
                End Select

            Case "外發返修作業"
                Select Case Label24.Text

                    Case "外發返修"
                        If Edit = True Then
                            OwDate.Enabled = False
                            WhCode.Enabled = False
                            ReWorkSaveEdit()
                        Else
                            ReWorkSaveNew()
                        End If
                    Case "審核"
                        Dim oac As New OutwardAcceptanceControl
                        Dim oai As New List(Of OutwardAcceptanceInfo)
                        oai = oac.OutwardAcceptance_GetList(Nothing, Nothing, txtOMID.Text, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

                        If oai.Count <> 0 Then
                            MsgBox("此單已有驗收記錄，不能修改審核！")
                        Else
                            UpdateCheck()
                        End If

                    Case "復核"
                        UpdateAccCheck()
                End Select
        End Select

      
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' 自動獲取外發單號
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNum() As String
        Dim oi1 As New OutwardInfo
        Dim oc1 As New OutwardController
        Dim str As String
        str = CStr(Format(Now, "yyMMdd"))
        oi1 = oc1.OutwardMain_GetNum(str)
        If oi1 Is Nothing Then
            GetNum = "LF" & str & "0001"
        Else
            GetNum = "LF" & str & Mid((CInt(Mid(oi1.O_NO, 9)) + 10001), 2)
        End If

    End Function
    ''' <summary>
    ''' 自動獲取外發返修單號
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNum1() As String
        Dim oi1 As New OutwardInfo
        Dim oc1 As New OutwardController
        Dim str As String
        str = CStr(Format(Now, "yyMMdd"))
        oi1 = oc1.OutwardReWork_GetNum(str)
        If oi1 Is Nothing Then
            GetNum1 = "R" & str & "0001"
        Else
            GetNum1 = "R" & str & Mid((CInt(Mid(oi1.O_NO, 8)) + 10001), 2)
        End If

    End Function

    ''' <summary>
    ''' 自動獲取外發流水號
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNo() As String
        Dim oi1 As New OutwardInfo
        Dim oc1 As New OutwardController
        oi1 = oc1.OutwardSub_GetNo("OWS")
        If oi1 Is Nothing Then
            GetNo = "OWS" & "000000001"
        Else
            GetNo = "OWS" & Mid((CInt(Mid(oi1.O_NOsub, 4)) + 1000000001), 2)
        End If
    End Function

    Private Sub WhCode_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles WhCode.ButtonClick
        frmWareHouseSelect.SelectWareID = ""
        tempValue3 = "700106"
        frmWareHouseSelect.ShowDialog()

        If frmWareHouseSelect.SelectWareID = "" Then

        Else
            WhCode.Text = frmWareHouseSelect.SelectWareName
            strWHID = frmWareHouseSelect.SelectWareID
        End If
    End Sub

    Private Sub cmdSubAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubAdd.Click
        tempCode = ""
        If WhCode.EditValue = "" Then
            MsgBox("請選擇需要外發加工的倉庫編號！")
            Exit Sub
        End If
        tempValue5 = WhCode.EditValue
        tempValue6 = "外發"
        frmBOMSelect.ShowDialog()
        '增加記錄
        If frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 0 Then
            If tempCode = "" Then
                Exit Sub
            Else
                AddRow(tempCode, 0)
            End If
        ElseIf frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 1 Then
            Dim i, n As Integer
            Dim arr(n) As String
            arr = Split(tempValue7, ",")
            n = Len(Replace(tempValue7, ",", "," & "*")) - Len(tempValue7)
            For i = 0 To n
                Dim j As Integer

                For j = 0 To ds.Tables("Outward").Rows.Count - 1
                    If tempValue2 = ds.Tables("Outward").Rows(j)("OS_BatchID") And arr(i) = ds.Tables("Outward").Rows(j)("M_Code") Then
                        MsgBox("一張單不允許存在相同批次重復物料編碼情況....")
                        Exit Sub
                    End If
                Next
                Dim mc As New LFERP.Library.Material.MaterialController
                Dim objInfo As New LFERP.Library.Material.MaterialInfo
                objInfo = mc.MaterialCode_Get(arr(i))
                Dim osc As New Library.Orders.OrdersSubController
                Dim osi As List(Of Library.Orders.OrdersSubInfo)
                If tempValue2 = "" Then Exit Sub
                osi = osc.OrdersSub_GetList(Nothing, tempValue2, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                '---------------------------------------------------------------------------
                'Dim wtc As New WareInventory.WareInventoryMTController
                'Dim wti As List(Of WareInventory.WareInventoryInfo)
                'wti = wtc.WareInventory_GetList(arr(i), strWHID)       '暫停倉庫入庫操作當前倉庫無庫存亦可外發
                'If wti.Count = 0 Then                                  '2010-12-14
                '    MsgBox(WhCode.Text & "倉庫中不存在此物料！")
                '    Exit Sub
                'End If
                '---------------------------------------------------------------------------
                Dim row As DataRow
                row = ds.Tables("Outward").NewRow
                row("O_NO") = Nothing
                row("O_NOsub") = Nothing

                row("M_Code") = objInfo.M_Code
                row("M_Name") = objInfo.M_Name
                row("M_Gauge") = objInfo.M_Gauge

                'row("OS_Qty") = wti(0).WI_Qty     '2010-12-14
                row("OS_Qty") = 0
                row("OS_NoSendQty") = 0
                row("OS_Price") = 0
                row("OS_BatchID") = tempValue2
                row("OS_ItemType") = Nothing
                row("OS_Remark") = Nothing
                row("OS_Depict") = Nothing
                row("PM_M_Code") = osi(0).PM_M_Code

                ds.Tables("Outward").Rows.Add(row)

                GridView1.MoveLast()

            Next

        ElseIf frmBOMSelect.XtraTabControl1.SelectedTabPageIndex = 2 Then
            Dim i, n As Integer
            Dim arr(n) As String
            arr = Split(tempValue8, ",")
            n = Len(Replace(tempValue8, ",", "," & "*")) - Len(tempValue8)
            For i = 0 To n
                Dim j As Integer

                For j = 0 To ds.Tables("Outward").Rows.Count - 1
                    If arr(i) = ds.Tables("Outward").Rows(j)("M_Code") Then
                        MsgBox("一張單不允許有重復物料編碼....")
                        Exit Sub
                    End If
                Next
                Dim mc As New LFERP.Library.Material.MaterialController
                Dim objInfo As New LFERP.Library.Material.MaterialInfo
                objInfo = mc.MaterialCode_Get(arr(i))

                If tempValue3 = "" Then Exit Sub

                'Dim wtc As New WareInventory.WareInventoryMTController
                'Dim wti As List(Of WareInventory.WareInventoryInfo)
                'wti = wtc.WareInventory_GetList(arr(i), strWHID)       '原因同上相同
                'If wti.Count = 0 Then
                '    MsgBox(WhCode.Text & "倉庫中不存在此物料！")
                '    Exit Sub
                'End If

                Dim row As DataRow
                row = ds.Tables("Outward").NewRow
                row("O_NO") = Nothing
                row("O_NOsub") = Nothing

                row("M_Code") = objInfo.M_Code
                row("M_Name") = objInfo.M_Name
                row("M_Gauge") = objInfo.M_Gauge

                'row("OS_Qty") = wti(0).WI_Qty         '2010-12-14  
                row("OS_NoSendQty") = 0
                row("OS_Price") = 0
                row("OS_BatchID") = ""
                row("OS_ItemType") = Nothing
                row("OS_Remark") = Nothing
                row("OS_Depict") = Nothing
                row("PM_M_Code") = tempValue3

                ds.Tables("Outward").Rows.Add(row)

                GridView1.MoveLast()
            Next
        End If

        tempValue4 = ""
        tempValue3 = ""
        tempValue2 = ""
        tempValue7 = ""
        tempValue8 = ""
        '---------------------------------------------------------------------以下2010-11-29
        'Dim row As DataRow = ds.Tables("Outward").NewRow
        'row("OS_Price") = 0
        'ds.Tables("Outward").Rows.Add(row)
    End Sub

    Sub getenable(ByVal a As Boolean, ByVal b As Boolean, ByVal c As Boolean)

        OwType.Enabled = a
        OwDate.Enabled = a
        WhCode.Enabled = a
        txtRemark.Enabled = a
        gluSupplier.Enabled = a

        CheckEdit1.Enabled = b
        CheckDate.Enabled = b
        CheckAction.Enabled = b
        CheckRemark.Enabled = b

        CheckEdit2.Enabled = c
        ACheckType.Enabled = c
        MemoEdit2.Enabled = c
        ACdate.Enabled = c
        ACAction.Enabled = c
    End Sub

    Private Sub cmdSubDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubDel.Click
        If ds.Tables("Outward").Rows.Count = 0 Then Exit Sub


        Dim DelTemp As String
        DelTemp = GridView1.GetRowCellDisplayText(ArrayToString(GridView1.GetSelectedRows()), "M_Code")

        If DelTemp = "M_Code" Then
        Else
            '在刪除表中增加被刪除的記錄
            Dim row As DataRow = ds.Tables("DelData").NewRow
            row("O_NOsub") = ds.Tables("Outward").Rows(GridView1.FocusedRowHandle)("O_NOsub")
            row("M_Code") = DelTemp
            ds.Tables("DelData").Rows.Add(row)
        End If
        ds.Tables("Outward").Rows.RemoveAt(CInt(ArrayToString(GridView1.GetSelectedRows())))
    End Sub

    Sub AddRow(ByVal strCode As String, ByVal Qty As Double)
        If strCode = "" Then
        Else

            Dim i As Integer

            For i = 0 To ds.Tables("Outward").Rows.Count - 1
                If strCode = ds.Tables("Outward").Rows(i)("M_Code") Then
                    MsgBox("一張單不允許有重復物料編碼....")
                    Exit Sub
                End If
            Next
            Dim mc As New LFERP.Library.Material.MaterialController
            Dim objInfo As New LFERP.Library.Material.MaterialInfo
            objInfo = mc.MaterialCode_Get(strCode)
            'Dim wi As List(Of WareInventory.WareInventoryInfo)
            'Dim wic As New WareInventory.WareInventoryMTController
            'wi = wic.WareInventory_GetList(strCode, WH_id)
            Dim row As DataRow
            row = ds.Tables("Outward").NewRow

            row("O_NO") = Nothing
            row("O_NOsub") = Nothing

            row("M_Code") = objInfo.M_Code
            row("M_Name") = objInfo.M_Name
            row("M_Gauge") = objInfo.M_Gauge

            row("OS_Qty") = Qty
            row("OS_NoSendQty") = 0
            row("OS_Price") = 0
            row("OS_BatchID") = Nothing
            row("OS_ItemType") = Nothing
            row("OS_Remark") = Nothing
            row("OS_Depict") = Nothing
            row("PM_M_Code") = Nothing

            ds.Tables("Outward").Rows.Add(row)


            GridView1.MoveLast()
        End If
    End Sub

    Function CheckData() As Boolean
        CheckData = False
        If OwType.EditValue = "" Then
            MsgBox("外發類型不能為空")
            Exit Function
        End If
        If CStr(OwDate.EditValue) = "" Then
            MsgBox("外發日期不能為空")
            Exit Function
        End If
        If WhCode.EditValue = "" Then
            MsgBox("倉庫代號不能為空！")
            Exit Function
        End If
        If gluSupplier.Text = "" Then
            MsgBox("供应商不能為空！")
            Exit Function
        End If

        If ds.Tables("Outward").Rows.Count = 0 Then
            MsgBox("請選擇外發物料！")
            Exit Function
        End If
        For i As Integer = 0 To ds.Tables("Outward").Rows.Count - 1
            If ds.Tables("Outward").Rows(i)("OS_Qty") <= 0 Then
                MsgBox("外發數量不能為空或者小於零！")

                Exit Function
            End If
            If IsDBNull(ds.Tables("Outward").Rows(i)("OS_ItemType")) Then
                MsgBox("项目類型不能為空！")
                Exit Function
            End If

        Next
        'Dim mw As New WareInventory.WareInventoryMTController
        'Dim mwi As New WareInventory.WareInventoryInfo

        'For i As Integer = 0 To ds.Tables("Outward").Rows.Count - 1
        '    '查詢是否相應的倉庫中夠數
        '    mwi = mw.WareInventory_GetSub(ds.Tables("Outward").Rows(i)("M_Code"), strWHID)

        '    If mwi Is Nothing Then
        '        MsgBox("物料" & ds.Tables("Outward").Rows(i)("M_Code") & " 在倉庫" & WhCode.EditValue & "中不存在，不能保存！")
        '        Exit Function
        '    End If

        '    If mwi.WI_Qty < ds.Tables("Outward").Rows(i)("OS_Qty") Then
        '        MsgBox("物料" & ds.Tables("Outward").Rows(i)("M_Code") & " 在倉庫" & WhCode.EditValue & "中不夠數，不能保存！")
        '        Exit Function
        '    End If
        'Next
        CheckData = True
    End Function

    Private Sub WhCode_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WhCode.EditValueChanged
        'If Me.WhCode.EditValue = "" Then
        'Else
        '    Me.WhCode.Enabled = False
        'End If
    End Sub

    Private Sub OwType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OwType.SelectedIndexChanged
        'If Me.OwType.EditValue = "" Then
        'Else
        '    Me.OwType.Enabled = False
        'End If
        'Dim oc As New OutwardController
        'If OwType.EditValue = "" Then Exit Sub
        'Me.RepositoryItemLookUpEdit1.DataSource = oc.LookUpEdit_Get(OwType.EditValue.ToString).Tables(0)
        'Me.RepositoryItemLookUpEdit1.ValueMember = "OT_NO"
        'Me.RepositoryItemLookUpEdit1.DisplayMember = "OT_Name"
    End Sub

    Private Sub RepositoryItemButtonEdit1_ButtonClick(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.ButtonPressedEventArgs) Handles RepositoryItemButtonEdit1.ButtonClick
        tempValue3 = GridView1.GetFocusedRowCellValue("OS_BatchID").ToString
        tempValue4 = GridView1.GetFocusedRowCellValue("OS_ItemType").ToString
        tempValue6 = GridView1.GetFocusedRowCellValue("M_Code").ToString
        If GridView1.GetFocusedRowCellValue("OS_ItemType").ToString = "" Then
            MsgBox("加工項目類型不能為空！")
            Exit Sub
        End If
        Dim oti As List(Of OutwardInfo)
        Dim otc As New OutwardController
        oti = otc.OutwardType_GetList(GridView1.GetFocusedRowCellValue("OS_ItemType").ToString, Nothing)
        tempValue2 = oti(0).OT_Name

        frmOutwardType.ShowDialog()
        Dim i As Integer
        For i = 0 To ds.Tables("Outward").Rows.Count - 1

            If ds.Tables("Outward").Rows(i)("OS_BatchID") = tempValue6 And ds.Tables("Outward").Rows(i)("OS_ItemType") = tempValue7 And ds.Tables("Outward").Rows(i)("M_Code") = tempValue8 Then
                ds.Tables("Outward").Rows(i)("OS_Depict") = tempValue5
            End If
        Next

        tempValue5 = ""
        tempValue6 = ""
        tempValue7 = ""
        tempValue8 = ""

    End Sub

    Private Sub RepositoryItemLookUpEdit2_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemLookUpEdit2.EditValueChanged
        Dim oc As New OrdersBomController

        If GridView1.GetFocusedRowCellValue("OS_BatchID").ToString = "" Then
            MsgBox("批次不能為空!")
            Exit Sub
        End If
        Dim Rlue As DevExpress.XtraEditors.LookUpEdit = CType(sender, DevExpress.XtraEditors.LookUpEdit)

        Dim mc As New LFERP.Library.Material.MaterialController
        ds.Tables("Outward").Rows((GridView1.FocusedRowHandle)).Item("M_Name") = mc.MaterialCode_Get(Rlue.EditValue).M_Name
        ds.Tables("Outward").Rows((GridView1.FocusedRowHandle)).Item("M_Gauge") = mc.MaterialCode_Get(Rlue.EditValue).M_Gauge
    End Sub

    Private Sub RepositoryItemTextEdit1_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemTextEdit1.Leave
      
    End Sub

    Private Sub GridView1_CellValueChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs) Handles GridView1.CellValueChanged
        If e.Column.FieldName = "OS_BatchID" Then
            Dim oc As New OrdersBomController
            ds.Tables("Outward").Rows((GridView1.FocusedRowHandle)).Item("PM_M_Code") = oc.OrdersBom_GetList(Nothing, GridView1.GetFocusedRowCellValue("OS_BatchID").ToString, Nothing, Nothing)(0).PM_M_Code

            Me.RepositoryItemLookUpEdit2.DataSource = oc.OrdersBom_GetList(Nothing, GridView1.GetFocusedRowCellValue("OS_BatchID").ToString, Nothing, 0)
            Me.RepositoryItemLookUpEdit2.ValueMember = "M_Code"
            Me.RepositoryItemLookUpEdit2.DisplayMember = "M_Code"
        End If
    End Sub

    Private Sub cmdBatchAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBatchAdd.Click
        tempValue4 = strWHID
        frmLoadingBatchID.ShowDialog()

        If tempValue = "" Or tempValue2 = "" Then
            Exit Sub
        Else
            AddRow1(tempValue, tempValue2, tempValue3)

            tempValue = ""
            tempValue2 = ""
            tempValue3 = Nothing

        End If
    End Sub

    Sub AddRow1(ByVal BatchID As String, ByVal strCode As String, ByVal Qty As String)
        If strCode = "" Then
        Else

            Dim j As Integer

            For j = 0 To ds.Tables("Outward").Rows.Count - 1
                If BatchID = ds.Tables("Outward").Rows(j)("OS_BatchID") And strCode = ds.Tables("Outward").Rows(j)("M_Code") Then
                    MsgBox("一張單不允許存在相同批次重復物料編碼情況....")
                    Exit Sub
                End If
            Next

            Dim mc As New LFERP.Library.Material.MaterialController
            Dim objInfo As New LFERP.Library.Material.MaterialInfo
            objInfo = mc.MaterialCode_Get(strCode)
       
            Dim row As DataRow
            row = ds.Tables("Outward").NewRow

            row("O_NO") = Nothing
            row("O_NOsub") = Nothing

            row("M_Code") = objInfo.M_Code
            row("M_Name") = objInfo.M_Name
            row("M_Gauge") = objInfo.M_Gauge

            row("OS_Qty") = Qty
            row("OS_Price") = 0
            row("OS_BatchID") = BatchID
            row("OS_ItemType") = Nothing
            row("OS_Remark") = Nothing
            row("OS_Depict") = Nothing
            row("OP_NO") = ""
            Dim osc As New Library.Orders.OrdersSubController
            Dim osi As List(Of Library.Orders.OrdersSubInfo)

            osi = osc.OrdersSub_GetList(Nothing, BatchID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            row("PM_M_Code") = osi(0).PM_M_Code

            ds.Tables("Outward").Rows.Add(row)

            GridView1.MoveLast()
        End If
    End Sub


End Class