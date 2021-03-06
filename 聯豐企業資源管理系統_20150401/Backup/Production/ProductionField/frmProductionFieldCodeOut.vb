Imports LFERP.Library.ProductionField
Imports LFERP.DataSetting
Imports LFERP.Library.Product
Imports LFERP.Library.ProductionWareHouse
Imports LFERP.Library.ProductProcess
Imports LFERP.Library.ProductionFieldType
Imports LFERP.Library.PieceProcess
Imports LFERP.Library.ProductionKaiLiao
Imports LFERP.Library.ProductionDPTWareInventory
Imports LFERP.Library.WareHouse
Imports LFERP.Library.Purchase.SharePurchase
Imports LFERP.Library.Production.ProductionFieldDaySummary
Imports System.Threading
Imports LFERP.Library.Production.ProductionAffair
Imports LFERP.Library.Production.Datasetting
Imports LFERP.SystemManager


Public Class frmProductionFieldCodeOut

    Dim pc As New LFERP.Library.PieceProcess.PersonnelControl
    Dim OldCheck As Boolean
    Dim ds As New DataSet
    Dim upi As List(Of UserPowerInfo)
    Dim upc As New UserPowerControl
    Dim mc As New ProductionDataSettingControl

    Dim AutoSchedule As Boolean = False


    Sub LoadPower()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "880413")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then AutoSchedule = True

        End If
    End Sub


    Sub LoadProductNo()
        Dim mc As New ProductController
        PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        PM_M_Code.Properties.ValueMember = "PM_M_Code"
        PM_M_Code.Properties.DataSource = mc.Product_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

    End Sub

    '@ 2012/2/22 添加
    '加載成品編號數據
    '此過程被以下過程調用：
    'frmProductionFieldCodeIn_Load()
    'LoadData()
    Sub LoadPM_M_Code()
        Dim mi As List(Of ProductionDataSettingInfo)
        mi = mc.ProductionUser_GetList(gluDep.EditValue, Nothing)

        ds.Tables("PM_M_Code").Clear()

        If mi.Count > 0 Then    '判斷是否有權限限制
            Dim row As DataRow
            Dim j As Integer
            For j = 0 To mi.Count - 1
                row = ds.Tables("PM_M_Code").NewRow
                row("PM_M_Code") = mi(j).PM_M_Code
                row("PM_JiYu") = mi(j).PM_JiYu
                
                ds.Tables("PM_M_Code").Rows.Add(row)
            Next
        Else
            Dim row As DataRow
            Dim j As Integer
            'Dim mpi As List(Of ProductInfo)
            'Dim mpc As New ProductController

            'mpi = mpc.Product_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

            Dim mpi As List(Of ProcessMainInfo)
            Dim mpc As New ProcessMainControl

            mpi = mpc.ProcessMain_GetList3(Nothing, Nothing)

            If mpi.Count > 0 Then
                For j = 0 To mpi.Count - 1
                    row = ds.Tables("PM_M_Code").NewRow
                    row("PM_M_Code") = mpi(j).PM_M_Code
                    row("PM_JiYu") = mpi(j).PM_JiYu
                    ds.Tables("PM_M_Code").Rows.Add(row)
                Next
            End If
        End If

    End Sub

    Sub LoadProductionDetail()
        Dim mc As New ProductionFieldTypeControl
        gluDetail.Properties.DisplayMember = "PT_Type"
        gluDetail.Properties.ValueMember = "PT_NO"
        gluDetail.Properties.DataSource = mc.ProductionFieldType_GetList(Nothing, Nothing, "發料")
    End Sub

    Sub LoadDepartment()

        gluDep.Properties.DataSource = pc.FacBriSearch_GetList(Nothing, Nothing, Nothing, Nothing)  '變更部門
        gluDep.Properties.DisplayMember = "DepName"
        gluDep.Properties.ValueMember = "DepID"

        gluchangedep.Properties.DataSource = pc.FacBriSearch_GetList(Nothing, Nothing, Nothing, Nothing)  '變更部門
        gluchangedep.Properties.DisplayMember = "DepName"
        gluchangedep.Properties.ValueMember = "DepID"

    End Sub

    '@ 2012/2/22 添加
    '加載工序編號數據
    '此過程被以下過程調用：
    'frmProductionFieldCodeIn_Load()
    'gluType_EditValueChanged()
    'LoadData()
    Sub LoadOutPS_Name()
        If gluDep.EditValue = "" Then Exit Sub

        Dim mi As List(Of ProductionDataSettingInfo)
        Dim row As DataRow
        Dim pc As New ProcessMainControl
        Dim pci As List(Of ProcessMainInfo)

        mi = mc.ProductionIssue_GetList(gluDep.EditValue, gluchangedep.EditValue, cbType.Text, PM_M_Code.Text, gluType.Text, Nothing)
        ds.Tables("Process").Clear()

        If mi.Count > 0 Then        '判斷是否有權限限制
            Dim i%
            For i = 0 To mi.Count - 1
                pci = pc.ProcessSub_GetList(Nothing, mi(i).Pro_NO, Nothing, Nothing, Nothing, True)
                If pci.Count > 0 Then
                    row = ds.Tables("Process").NewRow
                    row("PS_NO") = mi(i).Pro_NO
                    row("PS_Name") = pci(0).PS_Name

                    ds.Tables("Process").Rows.Add(row)
                End If
            Next
        Else
            pci = pc.ProcessMain_GetList(Nothing, PM_M_Code.EditValue, cbType.EditValue, gluType.EditValue, Nothing, True)
            If pci.Count = 0 Then Exit Sub
            Dim i As Integer
            For i = 0 To pci.Count - 1
                row = ds.Tables("Process").NewRow
                row("PS_NO") = pci(i).PS_NO
                row("PS_Name") = pci(i).PS_Name

                ds.Tables("Process").Rows.Add(row)
            Next
        End If
    End Sub


    Private Sub frmProductionFieldCodeOut_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i%

        '@ 2012/5/14 添加 判斷是否需要刷卡
        If strRefCard = "是" Then
        Else
            SimpleButton1.Visible = False
            TextEdit1.Visible = False
            SimpleButton2.Visible = False
        End If

        CreateTable()
        LoadPower()

        'LoadProductNo()
        LoadPM_M_Code()     '@2012/2/22 修改 產品編號調用
        LoadProductionDetail()
        LoadDepartment()

        Label22.Text = tempValue4
        Label23.Text = tempValue


        tempValue = ""
        tempValue4 = ""

        Label30.Text = tempValue5
        tempValue5 = ""

        txtNO.Enabled = False

        '重置新刷卡機
        Dim reset As New ResetPassWords.SetPassWords
        reset.SetPassWords()

        Select Case Label23.Text
            Case "CodeHouse"
                If Edit = False Then


                    Dim lockobj As New Object()

                    '需要鎖定的代碼塊 
                    SyncLock lockobj
                        Thread.Sleep(Int(Rnd() * 400) + 100)

                        txtNO.Text = GetNO()
                    End SyncLock

                    upi = upc.UserPower_GetList(InUserID, Nothing, Nothing, Nothing)

                    gluDetail.EditValue = tempValue2
                    tempValue2 = ""
                    If gluDetail.EditValue = "PT14" Then

                        gluchangedep.EditValue = "G101" '聯豐
                        gluDep.EditValue = "F101" '米亞外發部
                        cbType.EditValue = upi(0).UserType
                        gluDep.Enabled = False
                        gluchangedep.Enabled = False
                    Else

                        gluDep.EditValue = Label30.Text
                        gluchangedep.EditValue = Label30.Text
                        gluDep.Enabled = False
                        gluchangedep.Enabled = False
                        cbType.EditValue = upi(0).UserType
                        cbType.Enabled = False
                    End If
                    DateEdit1.EditValue = Format(Now, "yyyy/MM/dd")
                    Label8.Text = Format(Now, "HH:mm:ss")
                Else
                    LoadData(Label22.Text)
                    M_Code.Enabled = True
                    Me.Text = "修改--" & Label22.Text
                End If

                LoadPM_M_Code()

            Case "PreView"
                LoadData(Label22.Text)
                M_Code.Enabled = False
                cmdSave.Visible = False
                Me.Text = "查看--" & Label22.Text
                M_Code.Select()
            Case "InCheck"
                LoadData(Label22.Text)
                'gluchangedep.Enabled = False
                'gluDep.Enabled = False
                'DateEdit1.Enabled = False

                'M_Code.Enabled = False
                'CheckEdit2.Enabled = False

                For i = 0 To XtraTabPage1.Controls.Count - 1
                    XtraTabPage1.Controls(i).Enabled = False
                Next
                CheckEdit1.Enabled = True

                'XtraTabPage1.PageEnabled = False
                Me.Text = "確認--" & Label22.Text
        End Select
        gluDetail.Enabled = False
        'LoadOutPS_Name()

    End Sub

    Function LoadData(ByVal FP_NO As String) As Boolean
        LoadData = True
        LoadPM_M_Code()     '@ 2012/2/22 添加 成品編號調用

        Dim pi As List(Of ProductionFieldInfo)
        Dim pc As New ProductionFieldControl

        pi = pc.ProductionField_GetList(FP_NO, Nothing, "發料", Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "2", Nothing, Nothing)

        If pi.Count = 0 Then
            MsgBox("沒有數據")
            LoadData = False
            Exit Function
        Else

            txtNO.Text = pi(0).FP_NO
            Label6.Text = pi(0).FP_Num

            cbType.EditValue = pi(0).Pro_Type
            PM_M_Code.EditValue = pi(0).PM_M_Code
            gluType.EditValue = pi(0).PM_Type
            DateEdit1.EditValue = Format(pi(0).FP_Date, "yyyy/MM/dd")
            Label8.Text = Format(pi(0).FP_Date, "HH:mm:ss")

            Label5.Text = pi(0).FP_Type  '收發類型

            gluDep.EditValue = pi(0).FP_OutDep

            gluchangedep.EditValue = pi(0).FP_InDep

            gluDetail.EditValue = pi(0).FP_Detail

            txtQty.Text = pi(0).FP_Qty
            txtWeight.Text = pi(0).FP_Weight
            txtRemark.Text = pi(0).FP_Remark

            CheckEdit2.Checked = pi(0).FP_SubtractReQty
            TextEdit1.Text = pi(0).CardID

            If pi(0).FP_InCheck = True Then
                CheckEdit1.Checked = True
                OldCheck = True
            Else
                CheckEdit1.Checked = False
                OldCheck = False
            End If
            LoadOutPS_Name()    '@ 2012/2/22 添加 工序編號調用
            M_Code.EditValue = pi(0).Pro_NO
        End If
    End Function


    Sub CreateTable()

        With ds.Tables.Add("ProductType")
            .Columns.Add("PM_Type", GetType(String))
        End With
        gluType.Properties.ValueMember = "PM_Type"
        gluType.Properties.DisplayMember = "PM_Type"
        gluType.Properties.DataSource = ds.Tables("ProductType")

        With ds.Tables.Add("Process")
            .Columns.Add("PS_NO", GetType(String))
            .Columns.Add("PS_Name", GetType(String))
        End With
        M_Code.Properties.ValueMember = "PS_NO"
        M_Code.Properties.DisplayMember = "PS_Name"
        M_Code.Properties.DataSource = ds.Tables("Process")

        '@ 2012/2/22 添加
        '創建成品編號表
        With ds.Tables.Add("PM_M_Code")
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("PM_JiYu", GetType(String))
        End With
        PM_M_Code.Properties.ValueMember = "PM_M_Code"
        PM_M_Code.Properties.DisplayMember = "PM_M_Code"
        PM_M_Code.Properties.DataSource = ds.Tables("PM_M_Code")

    End Sub

    Function GetNO() As String

        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl
        Dim strA As String
        strA = Format(Now, "yyMM")
        pi = pc.ProductionField_GetNO(strA)

        If pi Is Nothing Then
            GetNO = "PF" & strA & "000001"
        Else
            GetNO = "PF" + strA + Mid((CInt(Mid(pi.FP_NO, 7)) + 1000001), 2)
        End If

    End Function

    Function GetNum() As String

        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl
        Dim strA As String
        strA = Format(Now, "yyMM")
        pi = pc.ProductionField_GetNO(strA)

        If pi Is Nothing Then
            GetNum = strA & "000001"
        Else
            GetNum = strA + Mid((CInt(Mid(pi.FP_Num, 5)) + 1000001), 2)
        End If

    End Function

    Function GetOutNO() As String

        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl
        Dim strA As String
        strA = Format(Now, "yyMM")
        pi = pc.ProductionField_GetOutNO(strA)

        If pi Is Nothing Then
            GetOutNO = "OW" & strA & "0001"
        Else
            GetOutNO = "OW" + strA + Mid((CInt(Mid(pi.OW_NO, 7)) + 10001), 2)
        End If
    End Function

    Sub DataNew()

        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl

        'txtNO.Text = GetNO()
        pi.FP_NO = txtNO.Text
        pi.FP_Num = GetNum()

        pi.Pro_Type = cbType.EditValue
        pi.PM_M_Code = PM_M_Code.EditValue
        pi.PM_Type = gluType.EditValue

        pi.Pro_NO = M_Code.EditValue
        'pi.Pro_NO1 = GluEdit2.EditValue
        If txtQty.Text.Trim = "" Then
            MsgBox("數量不能為空!")
            Exit Sub
        End If
        If txtWeight.Text.Trim = "" Then
            MsgBox("重量不能為空!")
            Exit Sub
        End If

        If CInt(txtQty.Text) < 0 Then
            MsgBox("數量必須為大於0的整數")
        End If
        If CInt(txtWeight.Text) < 0 Then
            MsgBox("數量必須為大於0的整數")
        End If
        If CInt(txtQty.Text) > CInt(Label10.Text) Then
            MsgBox("當前發出數量不能大於當前結餘數!")
            Exit Sub
        End If
        pi.FP_Qty = txtQty.Text
        pi.FP_Weight = txtWeight.Text

        pi.FP_Date = CDate(DateEdit1.EditValue & " " & Label8.Text)
        pi.FP_Detail = gluDetail.EditValue
        pi.FP_OutDep = gluDep.EditValue
        pi.FP_InDep = gluchangedep.EditValue
        pi.FP_Remark = txtRemark.Text
        pi.FP_OutAction = InUserID
        pi.FP_SubtractReQty = CheckEdit2.Checked
        pi.CardID = TextEdit1.Text

        If gluDep.EditValue = "F101" And gluchangedep.EditValue = "G101" Then   '外發部" Then
            pi.OW_NO = GetOutNO()
        Else
            pi.OW_NO = ""
        End If

        If pc.ProductionField_OutAdd(pi) = True Then
            MsgBox("保存成功")
        Else
            MsgBox("保存失敗,請檢查原因!")
        End If
        Me.Close()
    End Sub

    Sub DataEdit()
        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl

        pi.FP_Num = Label6.Text
        pi.PM_M_Code = PM_M_Code.EditValue
        pi.PM_Type = gluType.EditValue
        pi.Pro_NO = M_Code.EditValue


        If txtQty.Text.Trim = "" Then
            MsgBox("數量不能為空!")
            Exit Sub
        End If
        If txtWeight.Text.Trim = "" Then
            MsgBox("重量不能為空!")
            Exit Sub
        End If
        If CInt(txtQty.Text) < 0 Then
            MsgBox("數量必須為大於0的整數")
        End If
        If CInt(txtWeight.Text) < 0 Then
            MsgBox("數量必須為大於0的整數")
        End If
        If CInt(txtQty.Text) > CInt(Label10.Text) Then
            MsgBox("當前發出數量不能大於當前結餘數!")
            Exit Sub
        End If
        pi.FP_Qty = txtQty.Text
        pi.FP_Weight = txtWeight.Text

        pi.FP_Date = CDate(DateEdit1.EditValue & " " & Label8.Text)
        pi.FP_Detail = gluDetail.EditValue
        pi.FP_OutDep = gluDep.EditValue
        pi.FP_InDep = gluchangedep.EditValue
        pi.FP_OutAction = InUserID
        pi.FP_Remark = txtRemark.Text
        pi.FP_SubtractReQty = CheckEdit2.Checked
        pi.CardID = TextEdit1.Text

        If pc.ProductionField_OutUpdate(pi) = True Then
            MsgBox("保存成功")
        Else
            MsgBox("保存失敗,請檢查原因!")
        End If
        Me.Close()
    End Sub
    Function GetTypeData() As Boolean
        GetTypeData = True
        '1<<<<<<<<<<<<-----------查出ProductionFieldType表的許可行數
        Dim SetDataVal, SetDataPTF As Integer
        Dim pfcon As New ProductionFieldTypeControl
        Dim pflist As New List(Of ProductionFieldTypeInfo)
        pflist = pfcon.ProductionFieldType_GetList(GluDetail.EditValue, Nothing, Nothing)
        If pflist.Count > 0 Then
            SetDataVal = pflist(0).PT_DataValue
        Else
            SetDataVal = 0
        End If
        '2-----------------------查出ProductionField表的單筆實際行數
        Dim piL As New List(Of ProductionFieldInfo)
        Dim pc As New ProductionFieldControl
        piL = pc.ProductionField_GetList(txtNo.Text, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "2", Nothing, Nothing)
        If piL.Count > 0 Then
            SetDataPTF = piL.Count
        Else
            SetDataPTF = 0
        End If
        '3-------------正常比對單筆行數不能大于實際行數-------------
        If SetDataPTF = SetDataVal Or SetDataVal = 0 Then

        Else
            GetTypeData = False
        End If
        '---------------------------------------------->>>>>>>>>>>>>
    End Function
    Sub UpdateInCheck()
        If GetTypeData() = False Then
            MsgBox("此單已存在重單,請及時與電腦部聯系!")
            Exit Sub
        End If

        'Dim pi As New ProductionFieldInfo
        'Dim pc As New ProductionFieldControl

        'pi.FP_NO = txtNO.Text
        'pi.FP_Type = "發料"
        'If CheckEdit1.Checked = True Then
        '    pi.FP_InCheck = True
        'Else
        '    pi.FP_InCheck = False
        'End If
        'pi.FP_InAction = InUserID
        'pi.FP_InCheckDate = Format(Now, "yyyy/MM/dd HH:mm:ss")

        'If CheckEdit1.Checked = OldCheck Then
        '    MsgBox("未改變確認狀態,不允許保存!")
        '    Exit Sub
        'End If
        'If pc.ProductionField_UpdateInCheck(pi) = True Then
        '    MsgBox("確認信息發生改變!")
        'Else
        '    MsgBox("保存失敗,請檢查原因!")
        '    Exit Sub
        'End If

        ''------------------------------------------------------------------  收發物料扣數部份

        'Dim mt As New ProductionDPTWareInventoryControl
        'Dim mm As New ProductionDPTWareInventoryInfo

        'mm.DPT_ID = gluDep.EditValue
        'mm.M_Code = M_Code.EditValue

        'Dim Qty, ReQty As Single
        'Dim wi As List(Of ProductionDPTWareInventoryInfo)
        'Dim wc As New ProductionDPTWareInventoryControl
        'wi = wc.ProductionDPTWareInventory_GetList(gluDep.EditValue, M_Code.EditValue, Nothing)

        'If wi.Count = 0 Then
        '    Qty = 0
        '    ReQty = 0
        'Else
        '    Qty = wi(0).WI_Qty
        '    ReQty = wi(0).WI_ReQty
        'End If

        'If CheckEdit1.Checked = True Then
        '    mm.WI_Qty = Qty - CSng(txtQty.Text)   '確認發料
        'ElseIf CheckEdit1.Checked = False Then
        '    mm.WI_Qty = Qty + CSng(txtQty.Text)  '取消--物料加入
        'End If
        'mm.WI_ReQty = ReQty

        'mt.UpdateProductionField_Qty(mm)  '對應當前部門數量

        ''-----------------
        ''變更當前部門當前工序確認審核后--此收發單號的結餘數量(保存此時此部門庫存數)

        'Dim pi1 As New ProductionFieldInfo

        'pi1.FP_NO = txtNO.Text
        'pi1.FP_OutDep = gluDep.EditValue
        'pi1.Pro_NO = M_Code.EditValue
        'pi1.FP_Type = "發料"
        'pi1.FP_EndQty = mm.WI_Qty
        'pi1.FP_EndReQty = mm.WI_ReQty

        'pc.ProductionField_UpdateEndQty(pi1)
        ''-----------------

        ''---------------------------------------------------------------------------對應當前倉庫(交接倉或是物料倉庫)
        'If Mid(gluchangedep.EditValue, 1, 1) = "P" Or Mid(gluchangedep.EditValue, 1, 1) = "W" Then  '對應倉庫
        '    Dim wii As List(Of WareInventory.WareInventoryInfo)
        '    Dim wic As New WareInventory.WareInventoryMTController

        '    Dim Qty1 As Single

        '    wii = wic.WareInventory_GetList3(M_Code.EditValue, gluchangedep.EditValue)
        '    If wii.Count = 0 Then
        '        Qty1 = 0
        '    Else
        '        Qty1 = wii(0).WI_Qty
        '    End If

        '    Dim spc As New SharePurchaseController
        '    Dim spi As New SharePurchaseInfo

        '    spi.M_Code = M_Code.EditValue
        '    spi.WH_ID = gluchangedep.EditValue

        '    If CheckEdit1.Checked = True Then
        '        spi.WI_Qty = Qty1 + CSng(txtQty.Text) '確認發料
        '    ElseIf CheckEdit1.Checked = False Then
        '        spi.WI_Qty = Qty1 - CSng(txtQty.Text) '取消--物料加入
        '    End If
        '    spc.UpdateWareInventory_WIQty2(spi)  '物料在當前倉庫的變更

        'End If

        ''---------------------------------------------------------------------------對應當天該工序編碼該部門數量匯總信息
        'Dim pdi As List(Of ProductionFieldDaySummaryInfo)
        'Dim pdc As New ProductionFieldDaySummaryControl

        'Dim udi As New ProductionFieldDaySummaryInfo


        'Dim StrType As String  '類型
        'Dim IntQty As Integer  '數量

        'pdi = pdc.ProductionFieldDaySummary_GetList(Nothing, Nothing, Nothing, M_Code.EditValue, gluDep.EditValue, Nothing, DateEdit1.Text, DateEdit1.Text)

        'If gluDetail.EditValue = "PT06" Then
        '    StrType = "留辦"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).LiuBan
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.BuNiang = 0
        '    udi.CunCang = 0
        '    udi.CunHuo = 0
        '    udi.DiuShi = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.LiuBan = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.LiuBan = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.SunHuai = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.WaiFaOut = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi)   '變更留辦數量

        'ElseIf gluDetail.EditValue = "PT07" Then
        '    StrType = "損壞"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).SunHuai
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.BuNiang = 0
        '    udi.CunCang = 0
        '    udi.CunHuo = 0
        '    udi.DiuShi = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.SunHuai = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.SunHuai = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.LiuBan = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.WaiFaOut = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi)  '變更損壞數量

        'ElseIf gluDetail.EditValue = "PT08" Then
        '    StrType = "丟失"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).DiuShi
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.BuNiang = 0
        '    udi.CunCang = 0
        '    udi.CunHuo = 0
        '    udi.SunHuai = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.DiuShi = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.DiuShi = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.LiuBan = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.WaiFaOut = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi) '變更丟失數量

        'ElseIf gluDetail.EditValue = "PT09" Then
        '    StrType = "存貨"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).CunHuo
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.BuNiang = 0
        '    udi.CunCang = 0
        '    udi.DiuShi = 0
        '    udi.SunHuai = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.CunHuo = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.CunHuo = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.LiuBan = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.WaiFaOut = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi) '變更存貨數量

        'ElseIf gluDetail.EditValue = "PT10" Then
        '    StrType = "不良"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).BuNiang
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.CunHuo = 0
        '    udi.CunCang = 0
        '    udi.DiuShi = 0
        '    udi.SunHuai = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.BuNiang = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.BuNiang = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.LiuBan = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.WaiFaOut = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi) '變更不良數量

        'ElseIf gluDetail.EditValue = "PT14" Then
        '    StrType = "外發發出"
        '    If pdi.Count = 0 Then
        '        IntQty = 0
        '    Else
        '        IntQty = pdi(0).WaiFaOut
        '    End If
        '    udi.Pro_Type = cbType.EditValue
        '    udi.PM_M_Code = PM_M_Code.EditValue
        '    udi.PM_Type = gluType.EditValue
        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.EditValue
        '    udi.CunHuo = 0
        '    udi.CunCang = 0
        '    udi.DiuShi = 0
        '    udi.SunHuai = 0
        '    udi.FaLiao = 0
        '    udi.FanXiuIn = 0
        '    udi.FanXiuOut = 0
        '    udi.JiaCun = 0
        '    If CheckEdit1.Checked = True Then
        '        udi.WaiFaOut = IntQty + CInt(txtQty.Text)
        '    ElseIf CheckEdit1.Checked = False Then
        '        udi.WaiFaOut = IntQty - CInt(txtQty.Text)
        '    End If
        '    udi.QuCang = 0
        '    udi.QuCun = 0
        '    udi.ShouLiao = 0
        '    udi.LiuBan = 0
        '    udi.ChuHuo = 0
        '    udi.WaiFaIn = 0
        '    udi.BuNiang = 0
        '    udi.AccIn = 0
        '    udi.AccOut = 0
        '    udi.RePairOut = 0
        '    udi.Type = StrType

        '    pdc.UpdateProductionDaySummary_Qty(udi) '變更不良數量

        'End If

        'If pdi.Count > 0 Then

        '    udi.Pro_NO = M_Code.EditValue
        '    udi.FP_OutDep = gluDep.EditValue
        '    udi.PM_Date = DateEdit1.Text

        '    pdc.ProductionFieldDaySummary_Delete(udi) '判斷當前工序是否所有數量都為0 Yes刪除此條記錄,NO繼續保留!

        'End If

        ''---------------------------------------------------------------------------

        Dim pai As New ProductionAffairInfo
        Dim pac As New ProductionAffairControl

        Dim pdi As List(Of ProductionDPTWareInventoryInfo)
        Dim pdc As New ProductionDPTWareInventoryControl

        Dim pdsi As List(Of ProductionFieldDaySummaryInfo)
        Dim pdsc As New ProductionFieldDaySummaryControl

        Dim strQty, strReQty As Integer
        Dim strShouLiao, strJiaCun, strQuCun, strFaLiao, strCunHuo, strFanXiuIn, strFanXiuOut, strLiuBan, strSunHuai, strDiuShi, strBuNiang, strCunCang, strQuCang, strChuHuo, strWaiFaIn, strWaiFaOut, strAccIn, strAccOut, strRePairOut, strZuheOut As Integer

        pdi = pdc.ProductionDPTWareInventory_GetList(gluDep.EditValue, M_Code.EditValue, Nothing)
        If pdi.Count = 0 Then
            strQty = 0
            strReQty = 0
        Else
            strQty = pdi(0).WI_Qty
            strReQty = pdi(0).WI_ReQty
        End If
        pdsi = pdsc.ProductionFieldDaySummary_GetList(Nothing, Nothing, Nothing, M_Code.EditValue, gluDep.EditValue, Nothing, DateEdit1.Text, DateEdit1.Text)
        If pdsi.Count = 0 Then
            strShouLiao = 0
            strJiaCun = 0
            strQuCun = 0
            strFaLiao = 0
            strCunHuo = 0
            strFanXiuIn = 0
            strFanXiuOut = 0
            strLiuBan = 0
            strSunHuai = 0
            strDiuShi = 0
            strBuNiang = 0
            strCunCang = 0
            strQuCang = 0
            strChuHuo = 0
            strWaiFaIn = 0
            strWaiFaOut = 0
            strAccIn = 0
            strAccOut = 0
            strRePairOut = 0
            strZuheOut = 0
        Else
            strShouLiao = pdsi(0).ShouLiao
            strJiaCun = pdsi(0).JiaCun
            strQuCun = pdsi(0).QuCun
            strFaLiao = pdsi(0).FaLiao
            strCunHuo = pdsi(0).CunHuo
            strFanXiuIn = pdsi(0).FanXiuIn
            strFanXiuOut = pdsi(0).FanXiuOut
            strLiuBan = pdsi(0).LiuBan
            strSunHuai = pdsi(0).SunHuai
            strDiuShi = pdsi(0).DiuShi
            strBuNiang = pdsi(0).BuNiang
            strCunCang = pdsi(0).CunCang
            strQuCang = pdsi(0).QuCang
            strChuHuo = pdsi(0).ChuHuo
            strWaiFaIn = pdsi(0).WaiFaIn
            strWaiFaOut = pdsi(0).WaiFaOut
            strAccIn = pdsi(0).AccIn
            strAccOut = pdsi(0).AccOut
            strRePairOut = pdsi(0).RePairOut
            strZuheOut = pdsi(0).ZuheOut

        End If

        If CheckEdit1.Checked = OldCheck Then
            MsgBox("未改變確認狀態,不允許保存!")
            Exit Sub
        End If

        pai.FP_NO = txtNO.Text
        pai.FP_Type = "發料"
        pai.FP_InAction = InUserID
        pai.CardID = TextEdit1.Text

        If CheckEdit1.Checked = True Then
            pai.FP_InCheck = True
        Else
            pai.FP_InCheck = False
        End If

        pai.FP_InCheckDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
        '-------------------------------------------------------------------
        pai.Pro_Type = cbType.EditValue
        pai.PM_M_Code = PM_M_Code.EditValue
        pai.PM_Type = gluType.EditValue
        pai.Pro_Type1 = Nothing
        pai.PM_M_Code1 = Nothing
        pai.PM_Type1 = Nothing

        pai.Pro_NO = M_Code.EditValue
        pai.Pro_NO1 = Nothing
        pai.FP_OutDep = gluDep.EditValue
        pai.FP_InDep = Nothing

        pai.FP_Detail = gluDetail.EditValue
        pai.Type = Nothing

        '------------------------------------------------------變更部門結餘數信息
        If CheckEdit2.Checked = False Then      '@ 2012/6/1 添加 允許扣減返修數
            pai.WI_Qty = strQty - CInt(txtQty.Text)
            pai.WI_ReQty = strReQty
        Else
            pai.WI_Qty = strQty
            pai.WI_ReQty = strReQty - CInt(txtQty.Text)
        End If
        pai.WI_Qty1 = 0
        pai.WI_ReQty1 = 0

        '--------------------------------------------------------------------
        pai.ShouLiao = strShouLiao
        pai.JiaCun = strJiaCun

        If gluDetail.EditValue = "PT06" Then     '留辦
            pai.LiuBan = strLiuBan + CInt(txtQty.Text)
            pai.SunHuai = strSunHuai
            pai.DiuShi = strDiuShi
            pai.CunHuo = strCunHuo
            pai.BuNiang = strBuNiang
            pai.QuCun = strQuCun
        ElseIf gluDetail.EditValue = "PT07" Then  '損壞
            pai.LiuBan = strLiuBan
            pai.SunHuai = strSunHuai + CInt(txtQty.Text)
            pai.DiuShi = strDiuShi
            pai.CunHuo = strCunHuo
            pai.BuNiang = strBuNiang
            pai.QuCun = strQuCun
        ElseIf gluDetail.EditValue = "PT08" Then '丟失
            pai.LiuBan = strLiuBan
            pai.SunHuai = strSunHuai
            pai.DiuShi = strDiuShi + CInt(txtQty.Text)
            pai.CunHuo = strCunHuo
            pai.BuNiang = strBuNiang
            pai.QuCun = strQuCun
        ElseIf gluDetail.EditValue = "PT09" Then '存貨
            pai.LiuBan = strLiuBan
            pai.SunHuai = strSunHuai
            pai.DiuShi = strDiuShi
            pai.CunHuo = strCunHuo + CInt(txtQty.Text)
            pai.BuNiang = strBuNiang
            pai.QuCun = strQuCun
        ElseIf gluDetail.EditValue = "PT10" Then '不良品
            pai.LiuBan = strLiuBan
            pai.SunHuai = strSunHuai
            pai.DiuShi = strDiuShi
            pai.CunHuo = strCunHuo
            pai.BuNiang = strBuNiang + CInt(txtQty.Text)
            pai.QuCun = strQuCun
        ElseIf gluDetail.EditValue = "PT11" Then '取存
            pai.LiuBan = strLiuBan
            pai.SunHuai = strSunHuai
            pai.DiuShi = strDiuShi
            pai.CunHuo = strCunHuo
            pai.BuNiang = strBuNiang
            pai.QuCun = strQuCun + CInt(txtQty.Text)
        End If

        pai.FaLiao = strFaLiao
        pai.FanXiuIn = strFanXiuIn
        pai.FanXiuOut = strFanXiuOut
        pai.CunCang = strCunCang
        pai.QuCang = strQuCang
        pai.ChuHuo = strChuHuo
        pai.WaiFaIn = strWaiFaIn
        pai.WaiFaOut = strWaiFaOut
        pai.AccIn = strAccIn
        pai.AccOut = strAccOut
        pai.RePairOut = strRePairOut
        pai.ZuheOut = strZuheOut

        '------------------------------------------存在有收有發情況下
        pai.ShouLiao1 = 0
        pai.JiaCun1 = 0
        pai.QuCun1 = 0
        pai.FaLiao1 = 0
        pai.CunHuo1 = 0
        pai.FanXiuIn1 = 0
        pai.FanXiuOut1 = 0
        pai.LiuBan1 = 0
        pai.SunHuai1 = 0
        pai.DiuShi1 = 0
        pai.BuNiang1 = 0
        pai.CunCang1 = 0
        pai.QuCang1 = 0
        pai.ChuHuo1 = 0
        pai.WaiFaIn1 = 0
        pai.WaiFaOut1 = 0
        pai.AccIn1 = 0
        pai.AccOut1 = 0
        pai.RePairOut1 = 0
        pai.ZuheOut1 = 0

        '------------------------------------------
        pai.PM_Date = DateEdit1.Text

        If pac.UpdateProductionCheck_Qty(pai) = True Then
            MsgBox("確認當前物料收發已完成審核!")
            UpdateCheck()  '審核信息
        Else
            MsgBox("當前確認操作失敗,請檢查原因!")
            Exit Sub
        End If


        Me.Close()

    End Sub

    Sub UpdateCheck()

        Dim pi As New ProductionFieldInfo
        Dim pc As New ProductionFieldControl

        pi.FP_NO = txtNO.Text
        pi.FP_Check = True

        pi.FP_CheckAction = InUserID
        pi.FP_CheckRemark = ""

        pc.ProductionField_UpdateCheck(pi)   '審核功能---暫時確認后就對應完成審核,如果以後需要重新開放出來,加入類型即可

        'If pc.ProductionField_UpdateCheck(pi) = True Then
        '    MsgBox("審核成功")
        'Else
        '    MsgBox("審核成功,請檢查原因!")
        'End If
        'Me.Close()
    End Sub
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        Select Case Label23.Text
            Case "CodeHouse"
                If Edit = False Then
                    Dim fpi As List(Of ProductionFieldInfo)
                    Dim fpc As New ProductionFieldControl

                    fpi = fpc.ProductionField_GetList(txtNO.Text, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
                    If fpi.Count = 0 Then

                        If CheckData() = True Then
                            DataNew()
                        End If
                    Else
                        MsgBox("單號已存在，" & vbCr & "請確定重新生成單號!", 64, "提示")
                        txtNO.Text = GetNO()

                    End If
                Else
                    If CheckData() = True Then
                        DataEdit()
                    End If

                End If
            Case "InCheck"
                If CheckEdit1.Checked = OldCheck Then
                    MsgBox("確認信息發生未改變!")
                    Exit Sub
                End If
                If CheckData() = True Then
                    UpdateInCheck()
                    'UpdateCheck()
                End If
        End Select

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    '@ 2012/1/6修改爲用正則表達式判斷輸入的是否爲數字
    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        If txtQty.Text <> "" Then
            Dim m As New System.Text.RegularExpressions.Regex("^[1-9]+\d*$")
            If m.IsMatch(txtQty.Text) Then

                Dim pc As New ProcessMainControl
                Dim pci As List(Of ProcessMainInfo)

                pci = pc.ProcessSub_GetList(Nothing, M_Code.EditValue, Nothing, Nothing, Nothing, Nothing)
                If pci.Count = 0 Then Exit Sub

                Dim AllWeight, strWeight, strG As Single

                strWeight = pci(0).PS_Weight  '克/個  單重
                strG = strWeight * txtQty.Text
                AllWeight = strG / 1000  '當前數量的重量(KG)
                txtWeight.Text = Format(AllWeight, "0.00") '(轉化為兩位小數)
            Else
                MsgBox("只能輸入正整數！", 64, "提示")
                txtQty.Text = Nothing
            End If
        End If
    End Sub


    Private Sub PM_M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PM_M_Code.EditValueChanged
        On Error Resume Next
        If PM_M_Code.EditValue = "" Or PM_M_Code.EditValue Is Nothing Then Exit Sub

        Dim ppc As New ProcessMainControl
        Dim ppi As List(Of ProcessMainInfo)
        ds.Tables("ProductType").Clear()
        ppi = ppc.ProcessMain_GetList2(cbType.EditValue, PM_M_Code.EditValue)
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
        gluType.EditValue = Nothing
        'gluType.EditValue = ds.Tables("ProductType").Rows(0)("PM_Type").ToString
    End Sub

    Private Sub gluType_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gluType.EditValueChanged
        'Dim pc As New ProcessMainControl
        'Dim pci As List(Of ProcessMainInfo)
        'pci = pc.ProcessMain_GetList(Nothing, PM_M_Code.EditValue, cbType.EditValue, gluType.EditValue, Nothing, True)
        'If pci.Count = 0 Then Exit Sub
        'ds.Tables("Process").Clear()
        'Dim i As Integer
        'For i = 0 To pci.Count - 1
        '    Dim row As DataRow
        '    row = ds.Tables("Process").NewRow

        '    row("PS_NO") = pci(i).PS_NO
        '    row("PS_Name") = pci(i).PS_Name

        '    ds.Tables("Process").Rows.Add(row)
        'Next
        If gluType.EditValue = "" Or gluType.EditValue Is Nothing Then Exit Sub

        LoadOutPS_Name()
        M_Code.EditValue = Nothing
        'M_Code.EditValue = ds.Tables("Process").Rows(0)("PS_NO")
    End Sub

    Private Sub txtWeight_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtWeight.KeyUp
        Dim m As New System.Text.RegularExpressions.Regex("^+?(\d+(\.\d*)?|\.\d+)$")  '顯示整數,正浮點數正則表達式

        If m.IsMatch(txtWeight.Text) = True Then

        Else

            txtWeight.Text = Nothing
            Exit Sub
        End If
    End Sub

    Private Sub M_Code_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles M_Code.EditValueChanged
        If M_Code.EditValue = "" Or M_Code.EditValue Is Nothing Then Exit Sub

        Dim fdc As New ProductionDPTWareInventoryControl
        Dim fdi As List(Of ProductionDPTWareInventoryInfo)

        fdi = fdc.ProductionDPTWareInventory_GetList(gluDep.EditValue, M_Code.EditValue, Nothing)
        If fdi.Count = 0 Then
            Label10.Text = 0
        Else
            If CheckEdit2.Checked = False Then
                Label10.Text = fdi(0).WI_Qty
            Else
                Label10.Text = fdi(0).WI_ReQty
            End If
        End If

        'txtQty.Text = Label10.Text
    End Sub


    Function CheckData() As Boolean
        CheckData = True
        If TextEdit1.Visible = True Then
            If TextEdit1.Text = "" Then
                MsgBox("刷卡人信息不能為空！")
                CheckData = False
                Exit Function
            End If
        End If
        If gluDep.EditValue = "" Then
            MsgBox("發出部門不能為空！")
            CheckData = False
            Exit Function
        End If
        If gluchangedep.EditValue = "" Then
            MsgBox("收入部門不能為空！")
            CheckData = False
            Exit Function
        End If
        If Len(txtQty.Text.Trim) = 0 Then
            MsgBox("數量信息不能為空！")
            CheckData = False
            Exit Function
        End If
        If Len(txtWeight.Text.Trim) = 0 Then
            MsgBox("重量信息不能為空！")
            CheckData = False
            Exit Function
        End If
        If M_Code.EditValue = "" Then
            MsgBox("工序信息不能為空！")
            CheckData = False
            Exit Function
        End If

        If AutoSchedule = False Then
            Dim psi As List(Of LFERP.Library.ProductionSchedule.ProductionScheduleInfo)
            Dim psc As New LFERP.Library.ProductionSchedule.ProductionScheduleControl

            psi = psc.ProductionSchedule_GetList(Nothing, cbType.EditValue, Nothing, PM_M_Code.EditValue, gluType.EditValue, DateEdit1.Text, DateEdit1.Text, Nothing)
            If psi.Count = 0 Then
                MsgBox("當前生產部不存在選定日期的生產計劃，請先添加生產計劃！")
                CheckData = False
                Exit Function
            Else
                CheckData = True
            End If
        End If


        Dim fdc As New ProductionDPTWareInventoryControl
        Dim fdi As List(Of ProductionDPTWareInventoryInfo)

        fdi = fdc.ProductionDPTWareInventory_GetList(gluDep.EditValue, M_Code.EditValue, Nothing)

        If fdi.Count = 0 Then
            MsgBox("當前發出部門工序數量為空,不允許審核！")
            CheckData = False
            Exit Function
        Else
            If CheckEdit2.Checked = False Then
                If fdi(0).WI_Qty >= txtQty.Text Then
                    CheckData = True
                Else
                    MsgBox("當前大貨數量小於發出數量！")
                    CheckData = False
                    Exit Function
                End If
            Else
                If fdi(0).WI_ReQty >= txtQty.Text Then
                    CheckData = True
                Else
                    MsgBox("當前返修數量小於發出數量！")
                    CheckData = False
                    Exit Function
                End If
            End If

        End If
    End Function

    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        TextEdit1.Text = ReadCard()  '讀取卡號信息
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Dim frm As New frmNmetalSampleException
        frm.ShowDialog()

        TextEdit1.Text = tempValue
        tempValue = ""
    End Sub

    '@ 2012/6/1 添加一復選框 允許扣減返修數
    Private Sub CheckEdit2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckEdit2.CheckedChanged
        M_Code_EditValueChanged(Nothing, Nothing)
    End Sub


End Class