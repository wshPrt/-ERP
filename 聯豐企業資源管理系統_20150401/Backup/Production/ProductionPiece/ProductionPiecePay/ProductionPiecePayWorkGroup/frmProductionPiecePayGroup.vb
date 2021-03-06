Imports LFERP.Library.ProductionPiecePayWGMain
Imports LFERP.Library.ProductionPiecePayWGSub

Imports LFERP.Library.ProductionPieceFormula
Imports System.Collections.Specialized


Public Class frmProductionPiecePayGroup

    Dim ds As New DataSet
    Dim ds1 As New DataSet

    Dim strCaoType As String  '載入的操作類型
    Dim strPayFacID As String
    Dim strPayDepID As String
    Dim Load_OK As String
    Dim TempTextstr As String


    Private Sub frmProductionPiecePayGroup_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strCaoType = tempValue
        LabPY_ID.Text = tempValue2

        tempValue = Nothing
        tempValue2 = Nothing

        CreateTables()

        Select Case strCaoType

            Case "edit"
                LoadData(Trim(LabPY_ID.Text))
                LabelCaption.Text = "承包計件薪金-修改"

                chkPY_Check.Visible = False
            Case "check"
                LoadData(Trim(LabPY_ID.Text))
                LabelCaption.Text = "承包計件薪金-審核"
            Case "view"
                LabelCaption.Text = "承包計件薪金-查看"

                btnOK.Visible = False
                chkPY_Check.Visible = False
                SButtonSum.Visible = False
                ButtonDetail.Visible = False
                LoadData(Trim(LabPY_ID.Text))

        End Select

        Me.Text = "個人計件薪金"


        txtPY_PieceAllSum.Focus()
        txtPY_PieceAllSum.Select()

        Load_OK = "OK"
    End Sub


    Function LoadData(ByVal _PY_ID As String) As Boolean

        Dim objInfo As New ProductionPiecePayWGMainInfo
        Dim objList As New List(Of ProductionPiecePayWGMainInfo)
        Dim oc As New ProductionPiecePayWGMainControl
        objList = oc.ProductionPiecePayWGMain_GetList(Nothing, _PY_ID, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        LoadData = False

        If objList.Count = 1 Then
        Else
            MsgBox("沒有數據.")
            Exit Function
        End If

        LoadData = True

        txtG_NO.Text = objList(0).G_NO.ToString 'G_NO                     * nvarchar(20)         /組別編號
        txtG_Name.Text = objList(0).PY_G_Name.ToString '組別名稱

        DatePY_YYMM.EditValue = Format(CDate(objList(0).PY_YYMM.ToString), "yyyy年MM月")

        TxtPY_CompleteSum.Text = objList(0).PY_CompleteSum  'PY_CompleteSum           * real                 /完成總金額

        TxtPY_UseSum.Text = objList(0).PY_UseSum '                    * real                 /耗用總金額
        TxtPY_TimeAllSum.Text = objList(0).PY_TimeAllSum '            * real                 /計時總金額
        txtPY_PieceAllSum.Text = objList(0).PY_PieceAllSum '       * real                 /計件總金額

        TxtPY_CompensateSum.Text = objList(0).PY_CompensateSum '         * real                 /應補金額
        txtPY_SubtractSum.Text = objList(0).PY_SubtractSum '           * real                 /應扣金額
        txtPY_BonusSum.Text = objList(0).PY_BonusSum '              * real                 /獎金

        TxtPY_TimeSum.Text = objList(0).PY_TimeSum '               * real                 /計時金額
        txtPY_PieceSum.Text = objList(0).PY_PieceSum '              * real                 /計件金額


        txtPY_Remark.Text = objList(0).PY_Remark '                * nvarchar(MAX)        /備注]


        chkPY_Check.Checked = objList(0).PY_Check 'PY_Check                 * bit                  /審核


        strPayFacID = objList(0).FacID.ToString  '廠別
        txtFacID.Text = objList(0).PY_FacName

        strPayDepID = objList(0).DepID.ToString  '部門
        txtDepID.Text = objList(0).PY_DepName

        txtG_NO_OUTSum.Text = objList(0).G_NO_OUTSum
        txtG_NO_InSum.Text = objList(0).G_NO_InSum


        ''以上主表數據-----------------------------------------------------------------------------------------------------------

        Dim objInfoSub As New ProductionPiecePayWGSubInfo
        Dim objListSub As New List(Of ProductionPiecePayWGSubInfo)
        Dim ocSub As New ProductionPiecePayWGSubControl
        objListSub = ocSub.ProductionPiecePayWGSub_GetList(Nothing, _PY_ID, Nothing, Nothing)

        Dim i As Integer

        If objListSub.Count <= 0 Then
            MsgBox("無人員數據！")
            Exit Function
        End If

        ds.Tables("PiecePayWGSub").Clear()

        txtMeritedHoursSum.Text = 0
        txtPiecePaySum.Text = 0
        txtBonusSum.Text = 0
        txtMeritedPaySum.Text = 0

        For i = 0 To objListSub.Count - 1
            Dim row1 As DataRow = ds.Tables("PiecePayWGSub").NewRow

            row1("AutoID") = objListSub(i).AutoID '/自動編號ID
            row1("PY_ID") = objListSub(i).PY_ID ' /單號
            row1("Per_NO") = objListSub(i).Per_NO
            row1("Per_Name") = objListSub(i).Per_Name


            row1("Per_DayPrice") = objListSub(i).Per_DayPrice ''          /日薪
            row1("PYS_OnDutyDays") = objListSub(i).PYS_OnDutyDays ' '          /上班天數
            row1("PYS_UsualOverTime") = objListSub(i).PYS_UsualOverTime '           /平時加班小時數
            row1("PYS_HolidayOVerTime") = objListSub(i).PYS_HolidayOVerTime '             /節假日加班小時數
            row1("PYS_Proportion") = objListSub(i).PYS_Proportion.ToString  '          /工時比例
            'MsgBox(objListSub(i).PYS_Proportion.ToString)

            row1("PYS_Bonus") = objListSub(i).PYS_Bonus '             /獎金
            row1("PYS_AllHours") = objListSub(i).PYS_AllHours '             /總工時
            row1("PYS_MeritedHours") = objListSub(i).PYS_MeritedHours '                /應計工時
            row1("PYS_TimePay") = objListSub(i).PYS_TimePay '              /計時工資
            row1("PYS_PiecePay") = objListSub(i).PYS_PiecePay '               /計件工資

            row1("PYS_MeritedPay") = objListSub(i).PYS_MeritedPay '                /應得工資
            row1("PYS_Remark") = objListSub(i).PYS_Remark '        /備注

            ' /在指定組工作的天數 
            row1("PYS_WorkWGDay") = objListSub(i).PYS_WorkWGDay
            row1("Per_PayType") = objListSub(i).Per_PayType  ''薪金類型

            row1("PYS_FormulaID") = objListSub(i).PYS_FormulaID  '        /計算公式編號

            If objListSub(i).PYS_FormulaID = "" Then
                If objListSub(i).Per_PayType = "日薪" Then  ''第一次載入時 根據薪金類型載入
                    row1("PYS_FormulaID") = "日薪"
                ElseIf objListSub(i).Per_PayType = "計件" Then
                    row1("PYS_FormulaID") = "計件"
                End If
            End If

            ds.Tables("PiecePayWGSub").Rows.Add(row1)

            ''++++++++++++++++最底下的合計信息+++++++++++++++++++++++++++++++
            txtMeritedHoursSum.Text = Math.Round(CSng(txtMeritedHoursSum.Text)) + objListSub(i).PYS_MeritedHours '應計工時
            txtPiecePaySum.Text = Math.Round(CSng(txtPiecePaySum.Text) + objListSub(i).PYS_PiecePay) '計件工資
            txtBonusSum.Text = Math.Round(CSng(txtBonusSum.Text) + objListSub(i).PYS_Bonus) '/獎金
            txtMeritedPaySum.Text = Math.Round(CSng(txtMeritedPaySum.Text) + objListSub(i).PYS_MeritedPay) ' /應得工資
        Next

    End Function


    Private Sub CreateTables()
        ds.Tables.Clear()

        With ds.Tables.Add("PiecePayWGSub")
            .Columns.Add("AutoID", GetType(String)) '/自動編號ID
            .Columns.Add("PY_ID", GetType(String)) ' /單號
            .Columns.Add("Per_NO", GetType(String))  'Per_NO                   * nvarchar(20)         /廠證編號
            .Columns.Add("Per_Name", GetType(String)) 'Per_Name                 * nvarchar(20)         /姓名
            .Columns.Add("PYS_FormulaID", GetType(String))  'PYS_FormulaID            * nvarchar(20)         /計算公式編號

            .Columns.Add("Per_DayPrice", GetType(Double)) ''Per_DayPrice             * nvarchar(20)           /日薪
            .Columns.Add("PYS_OnDutyDays", GetType(Double)) ' 'PYS_OnDutyDays           * real                 /上班天數
            .Columns.Add("PYS_UsualOverTime", GetType(Double))  'PYS_UsualOverTime        * real                 /平時加班小時數
            .Columns.Add("PYS_HolidayOVerTime", GetType(Double)) 'PYS_HolidayOVerTime      * real                 /節假日加班小時數
            .Columns.Add("PYS_Proportion", GetType(Double))  'PYS_Proportion           * real                 /工時比例

            .Columns.Add("PYS_Bonus", GetType(Double)) 'PYS_Bonus                * real                 /獎金
            .Columns.Add("PYS_AllHours", GetType(Double)) 'PYS_AllHours             * real                 /總工時
            .Columns.Add("PYS_MeritedHours", GetType(Double)) 'PYS_MeritedHours         * real                 /應計工時
            .Columns.Add("PYS_TimePay", GetType(Double)) ' 'PYS_TimePay              * real                 /計時工資
            .Columns.Add("PYS_PiecePay", GetType(Double))  ''PYS_PiecePay             * real                 /計件工資

            .Columns.Add("PYS_MeritedPay", GetType(Double)) 'PYS_MeritedPay           * real                 /應得工資
            .Columns.Add("PYS_Remark", GetType(String))  'PYS_Remark                * nvarchar(MAX)        /備注

            .Columns.Add("PYS_WorkWGDay", GetType(Double))
            .Columns.Add("Per_PayType", GetType(String))

        End With
        Grid1.DataSource = ds.Tables("PiecePayWGSub")


        'With ds.Tables.Add("formula")
        '    .Columns.Add("formulaNO", GetType(String))
        '    .Columns.Add("formulaName", GetType(String))
        'End With

        'RepositoryItemLookUpEdit1.DisplayMember = "formulaNO"
        'RepositoryItemLookUpEdit1.ValueMember = "formulaNO"
        'RepositoryItemLookUpEdit1.DataSource = ds.Tables("formula")

        ' AddRow()
        ' RepositoryItemLookUpEdit1.PopupWidth = 800

        Dim pfc As New ProductionPieceFormulaControl
        RepositoryItemLookUpEdit1.DisplayMember = "FormulaName"
        RepositoryItemLookUpEdit1.ValueMember = "FormulaName"
        RepositoryItemLookUpEdit1.DataSource = pfc.ProductionPieceFormula_GetList(Nothing, Nothing, Nothing, "True")
        RepositoryItemLookUpEdit1.PopupWidth = 800

    End Sub


    Sub AddRow() ''設計工式開放時放入數據庫中    停用
        Dim row As DataRow
        ds.Tables("formula").Clear()

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "計件"
        row("formulaName") = "計件"
        ds.Tables("formula").Rows.Add(row)

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "日薪"
        row("formulaName") = "日薪 * 上班天數 + 日薪 / 8 * 1.2 * 平日加班"
        ds.Tables("formula").Rows.Add(row)

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "日薪(車鑽鑼/其它)"
        row("formulaName") = "日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + 假日加班 / 8) * 5"
        ds.Tables("formula").Rows.Add(row)

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "日薪(磨房/磨光機)"
        row("formulaName") = "日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + 假日加班 / 8) * 25"
        ds.Tables("formula").Rows.Add(row)

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "日薪(批鋒)"
        row("formulaName") = "日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + 假日加班 / 8) * 12"
        ds.Tables("formula").Rows.Add(row)

        row = ds.Tables("formula").NewRow()
        row("formulaNO") = "日薪(不補)"
        row("formulaName") = "日薪 * 上班天數 + 11.64 * 平日加班 + 15.52 * 假日加班"
        ds.Tables("formula").Rows.Add(row)

        ''計件   0
        ''日薪----日薪 * 上班天數 + 日薪 / 8 * 1.2 * 平日加班
        ''日薪(車鑽鑼/其它)     日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 5) 
        ''日薪(磨房/磨光機)     日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 25) 
        ''日薪(批鋒)            日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 12)
        ''日薪(不補)            日薪 * 上班天數 + 11.64 * 平日加班 + 15.52 * 假日加班
    End Sub


    Private Sub SaveEdit()
        Dim pi As New ProductionPiecePayWGMainInfo
        Dim pc As New ProductionPiecePayWGMainControl

        pi.PY_ID = Trim(LabPY_ID.Text)

        ''-------------------------------
        pi.FacID = strPayFacID   ''廠別編號
        pi.DepID = strPayDepID   ''部門編號

        ''-----------------------------------------------------------------
        pi.G_NO = txtG_NO.Text '組別編號
        pi.PY_YYMM = Format(CDate(DatePY_YYMM.EditValue), "yyyy/MM") '年/月
        pi.PY_CompleteSum = CDbl(TxtPY_CompleteSum.Text) '/完成總金額
        pi.PY_UseSum = CDbl(TxtPY_UseSum.Text) '耗用總金額
        pi.PY_TimeAllSum = CDbl(TxtPY_TimeAllSum.Text) '計時總金額

        pi.PY_PieceAllSum = CDbl(txtPY_PieceAllSum.Text) ' /計件總金額
        pi.PY_CompensateSum = CDbl(TxtPY_CompensateSum.Text) '應補金額
        pi.PY_SubtractSum = CDbl(txtPY_SubtractSum.Text) '應扣金額

        pi.PY_BonusSum = CDbl(txtPY_BonusSum.Text) '獎金
        pi.PY_TimeSum = CDbl(TxtPY_TimeSum.Text) '計時金額
        pi.PY_PieceSum = CDbl(txtPY_PieceSum.Text) '計件金額

        pi.PY_Remark = txtPY_Remark.Text '  /備注

        pi.PY_ModifyUserID = InUserID
        pi.PY_ModifyDate = Format(Now, "yyyy/MM/dd")

        pi.G_NO_OUTSum = CDbl(txtG_NO_OUTSum.Text)
        pi.G_NO_InSum = CDbl(txtG_NO_InSum.Text)

        If pc.ProductionPiecePayWGMain_Update(pi) = True Then
        Else
            MsgBox("主表保存失敗!")
            Exit Sub
        End If

        ''--------保存子表------------------------------------------------
        Dim piS As New ProductionPiecePayWGSubInfo
        Dim pcS As New ProductionPiecePayWGSubControl

        Dim j As Integer

        For j = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1

            piS.AutoID = ds.Tables("PiecePayWGSub").Rows(j)("AutoID")

            piS.PY_ID = Trim(LabPY_ID.Text)  '編號
            piS.Per_NO = ds.Tables("PiecePayWGSub").Rows(j)("Per_NO")
            piS.Per_Name = ds.Tables("PiecePayWGSub").Rows(j)("Per_Name")
            If ds.Tables("PiecePayWGSub").Rows(j)("PYS_FormulaID") Is DBNull.Value Then
            Else
                piS.PYS_FormulaID = ds.Tables("PiecePayWGSub").Rows(j)("PYS_FormulaID") '   /計算公式編號
            End If

            piS.Per_DayPrice = ds.Tables("PiecePayWGSub").Rows(j)("Per_DayPrice") '日薪
            piS.PYS_OnDutyDays = ds.Tables("PiecePayWGSub").Rows(j)("PYS_OnDutyDays") '上班天數
            piS.PYS_UsualOverTime = ds.Tables("PiecePayWGSub").Rows(j)("PYS_UsualOverTime") '  /平時加班小時數
            piS.PYS_HolidayOVerTime = ds.Tables("PiecePayWGSub").Rows(j)("PYS_HolidayOVerTime") '  /節假日加班小時數
            piS.PYS_Proportion = ds.Tables("PiecePayWGSub").Rows(j)("PYS_Proportion").ToString  '  /工時比例

            ' MsgBox(piS.PYS_Proportion)
            piS.PYS_Bonus = ds.Tables("PiecePayWGSub").Rows(j)("PYS_Bonus") '  /  /獎金
            piS.PYS_AllHours = ds.Tables("PiecePayWGSub").Rows(j)("PYS_AllHours") '  /總工時
            piS.PYS_MeritedHours = ds.Tables("PiecePayWGSub").Rows(j)("PYS_MeritedHours") '  /應計工時
            piS.PYS_TimePay = ds.Tables("PiecePayWGSub").Rows(j)("PYS_TimePay") '     /計時工資
            piS.PYS_PiecePay = ds.Tables("PiecePayWGSub").Rows(j)("PYS_PiecePay") '    /計件工資

            piS.PYS_MeritedPay = ds.Tables("PiecePayWGSub").Rows(j)("PYS_MeritedPay") '        /應得工資

            If ds.Tables("PiecePayWGSub").Rows(j)("PYS_Remark") Is DBNull.Value Then
            Else
                piS.PYS_Remark = ds.Tables("PiecePayWGSub").Rows(j)("PYS_Remark") '    / /備注
            End If
            ''-------------------------
            piS.PYS_WorkWGDay = ds.Tables("PiecePayWGSub").Rows(j)("PYS_WorkWGDay")

            If ds.Tables("PiecePayWGSub").Rows(j)("Per_PayType") Is DBNull.Value Then
            Else
                piS.Per_PayType = ds.Tables("PiecePayWGSub").Rows(j)("Per_PayType") ' 薪金類型
            End If

            If pcS.ProductionPiecePayWGSub_Update(piS) = True Then
            Else
                MsgBox("部分數據保存失敗，請檢查！")
                Exit Sub
            End If
        Next

        If strCaoType = "check" Then
        Else
            MsgBox("保存成功!")
            Me.Close()
        End If

    End Sub


    Private Sub SaveCheck()
        Dim pi2 As New ProductionPiecePayWGMainInfo
        Dim pc2 As New ProductionPiecePayWGMainControl

        If LabPY_ID.Text <> "" Then
        Else
            MsgBox("編號為空，請檢查.")
            Exit Sub
        End If

        pi2.PY_ID = LabPY_ID.Text
        pi2.PY_Check = chkPY_Check.Checked  'PL_Check                 * bit                   /審核
        pi2.PY_CheckUserID = InUserID 'PL_CheckUserID           * nvarchar(20)          /審核編號
        pi2.PY_CheckDate = Format(Now, "yyyy/MM/dd") 'PL_CheckDate             * datetime              /審核日期

        If pc2.ProductionPiecePayWGMain_Updatecheck(pi2) = True Then
            MsgBox("審核成功!")
            Me.Close()
        Else
            MsgBox("審核失敗!")
        End If

    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If strCaoType = "check" Then
            SaveEdit()
            SaveCheck()
        End If

        If strCaoType = "edit" Then
            SaveEdit()
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    ''' <summary>
    ''' 檢查計算/存盤時的數據是否導入正確
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function Check_Date() As Boolean
        Check_Date = True

        Dim i As Integer

        If ds.Tables("PiecePayWGSub").Rows.Count <= 0 Then
            Check_Date = False
            Exit Function
        End If

        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1

            If ds.Tables("PiecePayWGSub").Rows(i)("Per_DayPrice") Is DBNull.Value Then '日薪
                MsgBox("日薪輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                Check_Date = False
                Exit Function
            End If


            If ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") Is DBNull.Value Then '上班天數
                MsgBox("上班天數輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                Check_Date = False
                Exit Function
            End If

            If ds.Tables("PiecePayWGSub").Rows(i)("PYS_UsualOverTime") Is DBNull.Value Then '  /平時加班小時數
                MsgBox("平時加班小時數輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                Check_Date = False
                Exit Function
            End If

            If ds.Tables("PiecePayWGSub").Rows(i)("PYS_HolidayOVerTime") Is DBNull.Value Then '  /節假日加班小時數
                MsgBox("節假日加班小時數輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                Check_Date = False
                Exit Function
            End If

            If ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID") Is DBNull.Value Then '  /  /計算公式編號
                MsgBox("計算公式輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                GridView1.FocusedColumn = GridView1.Columns("PYS_FormulaID")
                Check_Date = False
                Exit Function
            End If

            If ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID") <> "計件" Then
            Else
                If ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion") Is DBNull.Value Then '  /工時比例
                    MsgBox("工時比例輸入有誤!")
                    GridView1.FocusedRowHandle = i '移、至錯誤碼行
                    GridView1.FocusedColumn = GridView1.Columns("PYS_Proportion")
                    Check_Date = False
                    Exit Function
                ElseIf CDbl(ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion")) > 1 Then
                    MsgBox("工時比例不能大于1,請重新輸入!!")
                    GridView1.FocusedRowHandle = i '移、至錯誤碼行
                    GridView1.FocusedColumn = GridView1.Columns("PYS_Proportion")
                    Check_Date = False
                    Exit Function
                ElseIf CDbl(ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion")) <= 0 Then
                    MsgBox("工時比例不能小於等於0,請重新輸入!!")
                    GridView1.FocusedRowHandle = i '移、至錯誤碼行
                    GridView1.FocusedColumn = GridView1.Columns("PYS_Proportion")
                    Check_Date = False
                    Exit Function
                End If

                If ds.Tables("PiecePayWGSub").Rows(i)("PYS_Bonus") Is DBNull.Value Then '  /  /獎金
                    MsgBox("獎金輸入有誤!")
                    GridView1.FocusedRowHandle = i '移、至錯誤碼行
                    Check_Date = False
                    Exit Function
                End If
            End If

            If ds.Tables("PiecePayWGSub").Rows(i)("Per_PayType") Is DBNull.Value Then '  /  /計算公式編號
                MsgBox("薪金類型輸入有誤!")
                GridView1.FocusedRowHandle = i '移、至錯誤碼行
                GridView1.FocusedColumn = GridView1.Columns("Per_PayType")
                Check_Date = False
                Exit Function
            End If
        Next

    End Function

    'Private Sub SButtonSum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SButtonSum.Click

    '    If Check_Date() = True Then
    '    Else
    '        Exit Sub
    '    End If

    '    On Error Resume Next   ''主要對對 CSng 時，或文本框中有 字母就會出差

    '    ''計算出工計件工資 
    '    Dim i As Integer
    '    txtPiecePaySum.Text = 0
    '    txtMeritedPaySum.Text = 0
    '    txtPY_PieceSum.Text = 0
    '    TxtPY_TimeSum.Text = 0
    '    txtMeritedHoursSum.Text = 0
    '    txtPY_BonusSum.Text = 0
    '    txtBonusSum.Text = 0

    '    '1    完成額=計時總額+計件+應補 ----------------------------------------- 
    '    Dim 完成額, 計件, 計時總額, 應補, 應扣 As Single

    '    計件 = CSng(txtPY_PieceAllSum.Text)
    '    計時總額 = CSng(TxtPY_TimeAllSum.Text)
    '    應補 = CSng(TxtPY_CompensateSum.Text)
    '    應扣 = CSng(txtPY_SubtractSum.Text)

    '    完成額 = 計件 + 計時總額 + 應補 - 應扣
    '    TxtPY_CompleteSum.Text = 完成額

    '    '2    ---------------------------------------------------------------------------------------------
    '    Dim 日薪, 上班天數, 平日加班, 假日加班, 工時比例, 獎金, 總工時, 應計工時, 計時工資 As Single
    '    Dim 薪金類型, 計算工式 As String

    '    For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
    '        日薪 = ds.Tables("PiecePayWGSub").Rows(i)("Per_DayPrice") '日薪
    '        上班天數 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") '上班天數
    '        平日加班 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_UsualOverTime") '  /平時加班小時數

    '        假日加班 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_HolidayOVerTime") '  /節假日加班小時數
    '        工時比例 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion") '  /工時比例
    '        獎金 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Bonus") '  /  /獎金

    '        ''  總工時=上班天數*8+加班時數+假日加班
    '        總工時 = Math.Round(上班天數 * 8 + 平日加班 + 假日加班, 1)
    '        ds.Tables("PiecePayWGSub").Rows(i)("PYS_AllHours") = 總工時
    '        ''  '為計件時就計算,應計工時=總工時*工時比例
    '        薪金類型 = ds.Tables("PiecePayWGSub").Rows(i)("Per_PayType")  ''為計日薪時就不用算

    '        If 薪金類型 = "計件" Then
    '            應計工時 = Math.Round(總工時 * 工時比例)
    '            txtMeritedHoursSum.Text = CSng(txtMeritedHoursSum.Text) + 應計工時 ''合計
    '        Else
    '            應計工時 = 0
    '        End If
    '        ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedHours") = 應計工時

    '        ''--------------------------------------------------------------------
    '        ' '獎金
    '        txtBonusSum.Text = CSng(txtBonusSum.Text) + 獎金

    '        ''--------------------------------------------------------------------
    '        '計算計時工資

    '        計算工式 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID")
    '        Select Case 計算工式
    '            Case "計件"
    '                計時工資 = 0
    '            Case "日薪"
    '                計時工資 = Math.Round(日薪 * 上班天數 + 日薪 / 8 * 1.2 * 平日加班)
    '            Case "日薪(車鑽鑼/其它)"
    '                計時工資 = Math.Round(日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 5)
    '            Case "日薪(磨房/磨光機)"
    '                計時工資 = Math.Round(日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 25)
    '            Case "日薪(批鋒)"
    '                計時工資 = Math.Round(日薪 * 上班天數 + 7.76 * 平日加班 + 10.34 * 假日加班 - (上班天數 + Math.Round(假日加班 / 8, 0)) * 12)
    '            Case "日薪(不補)"
    '                計時工資 = Math.Round(日薪 * 上班天數 + 11.64 * 平日加班 + 15.52 * 假日加班)
    '        End Select
    '        ds.Tables("PiecePayWGSub").Rows(i)("PYS_TimePay") = 計時工資

    '        ''計算在組別中進行計時工作的工資-------------------------------------------
    '        If 薪金類型 = "日薪" Then
    '            TxtPY_TimeSum.Text = CSng(TxtPY_TimeSum.Text) + 計時工資
    '        End If
    '    Next
    '    ''---------------------------------------------
    '    txtPY_BonusSum.Text = txtBonusSum.Text '獎金
    '    '部門承包計件額=計件完成額-物料耗用額-計時工資-其它(獎金)
    '    txtPY_PieceSum.Text = Math.Round(CSng(TxtPY_CompleteSum.Text) - CSng(TxtPY_UseSum.Text) - CSng(TxtPY_TimeSum.Text) - CSng(txtPY_BonusSum.Text))

    '    ''----------以下要據計件額進行，金客分配---------------------------------------------------------------------------------------------------

    '    Dim 獎金1, 應計工時1, 計時工資1, 計件工資1, 應得工資1 As Single
    '    Dim 計算工式1 As String

    '    Dim 計件工資2, 節余 As Single

    '    For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
    '        獎金1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Bonus") '  /  /獎金
    '        應計工時1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedHours")
    '        計時工資1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_TimePay")
    '        計算工式1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID")

    '        '計件工資=計件額/總應計工時*應計工時
    '        If CSng(txtPY_PieceSum.Text) = 0 Or CSng(txtMeritedHoursSum.Text) = 0 Or 應計工時1 = 0 Then
    '            計件工資1 = 0
    '        Else
    '            計件工資1 = Math.Round(CSng(txtPY_PieceSum.Text) / CSng(txtMeritedHoursSum.Text) * 應計工時1)
    '        End If

    '        計件工資2 = 計件工資2 + 計件工資1

    '        Select Case 計算工式1
    '            Case "計件"
    '                應得工資1 = 計件工資1 + 獎金1
    '            Case Else
    '                應得工資1 = 計時工資1 + 獎金1
    '        End Select

    '        ds.Tables("PiecePayWGSub").Rows(i)("PYS_PiecePay") = 計件工資1 '    /計件工資
    '        ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedPay") = 應得工資1     '  /應得工資

    '        txtPiecePaySum.Text = CSng(txtPiecePaySum.Text) + 計件工資1
    '        txtMeritedPaySum.Text = CSng(txtMeritedPaySum.Text) + 應得工資1
    '    Next


    '    ''-------------------------------------***---------------------------------------------------------
    '    Dim k As Integer
    '    ''因為四舍五入時，計件工資， 有可能與實際分配后的總和有出入 ,把節余 加給工時比例最高的
    '    Dim 薪金類型2 As String
    '    Dim 工時比例2, temp As Single

    '    節余 = CSng(txtPY_PieceSum.Text) - 計件工資2

    '    If 節余 <= 0 Then Exit Sub

    '    For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
    '        薪金類型2 = ds.Tables("PiecePayWGSub").Rows(i)("Per_PayType")  ''為計日薪時就不用算

    '        If 薪金類型2 = "計件" Then
    '            工時比例2 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion") '  /工時比例

    '            If 工時比例2 > temp Then
    '                temp = 工時比例2
    '                k = i
    '            End If
    '        End If
    '    Next

    '    ds.Tables("PiecePayWGSub").Rows(k)("PYS_PiecePay") = ds.Tables("PiecePayWGSub").Rows(k)("PYS_PiecePay") + 節余 '    /計件工資
    '    ds.Tables("PiecePayWGSub").Rows(k)("PYS_MeritedPay") = ds.Tables("PiecePayWGSub").Rows(k)("PYS_MeritedPay") + 節余    '  /應得工資

    '    txtPiecePaySum.Text = CSng(txtPiecePaySum.Text) + 節余
    '    txtMeritedPaySum.Text = CSng(txtMeritedPaySum.Text) + 節余
    '    ''---------------------------------------***-----------------------------------------------------------
    'End Sub

    Private Sub ButtonDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDetail.Click
        Dim strStat_Date, strEnd_Date As String
        ''---------------------------------------------------------------------------------
        Dim intInputMonth, intInputYear As Integer '这是你输入的月份                                '|
        intInputMonth = Val(Format(CDate(DatePY_YYMM.EditValue), "MM"))                 '| 
        intInputYear = Val(Format(CDate(DatePY_YYMM.EditValue), "yyyy"))

        Dim dt As New DateTime(intInputYear, intInputMonth, 1)                 '|
        '计算该月份的天数
        Dim days As Integer = dt.AddMonths(1).DayOfYear - dt.DayOfYear                '|
        If days <= 0 Or days > 31 Then
            days = 31
        End If
        strStat_Date = (dt.AddDays(0).ToString("yyyy/MM/dd"))                         '|
        strEnd_Date = (dt.AddDays(days - 1).ToString("yyyy/MM/dd"))                   '|
        ''---------------------------------------------------------------------------------

        Dim strCompany As String
        strCompany = Mid(strInDPT_ID, 1, 4)   '獲得登錄者所屬公司ID,以返回公司名稱，LOGO

        ds1.Tables.Clear()
        Dim ltc1 As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet
        Dim ltc3 As New CollectionToDataSet
        Dim ltc4 As New CollectionToDataSet
        Dim ltc5 As New CollectionToDataSet

        Dim mcCompany As New LFERP.DataSetting.CompanyControler
        Dim PPG As New LFERP.Library.ProductionSumPieceWorkGroup.ProductionSumPieceWorkGroupControl
        Dim PTG As New LFERP.Library.ProductionSumTimeWorkGroup.ProductionSumTimeWorkGroupControl
        Dim PPPS As New LFERP.Library.ProductionPieceProcess.ProductionPieceProcessControl
        Dim PPPa As New LFERP.Library.ProductionPiecePersonnel.ProductionPiecePersonnelControl

        If PPG.ProductionSumPieceWorkGroup_GetList(Nothing, Nothing, Nothing, txtG_NO.Text, strPayDepID, strPayFacID, Nothing, Nothing, Nothing, Nothing, Nothing, strStat_Date, Nothing, strEnd_Date, UserName).Count <= 0 Then
            ltc1.CollToDataSet(ds1, "ProductionSumPieceWorkGroup", PPG.NothingNew)
        Else
            ltc1.CollToDataSet(ds1, "ProductionSumPieceWorkGroup", PPG.ProductionSumPieceWorkGroup_GetList(Nothing, Nothing, Nothing, txtG_NO.Text, strPayDepID, strPayFacID, Nothing, Nothing, Nothing, Nothing, Nothing, strStat_Date, Nothing, strEnd_Date, UserName))
        End If

        If PTG.ProductionSumTimeWorkGroup_GetList(Nothing, Nothing, txtG_NO.Text, strPayDepID, strPayFacID, strStat_Date, Nothing, strEnd_Date, Nothing, Nothing).Count <= 0 Then
            ltc2.CollToDataSet(ds1, "ProductionSumTimeWorkGroup", PTG.NothingNew)
        Else
            ltc2.CollToDataSet(ds1, "ProductionSumTimeWorkGroup", PTG.ProductionSumTimeWorkGroup_GetList(Nothing, Nothing, txtG_NO.Text, strPayDepID, strPayFacID, strStat_Date, Nothing, strEnd_Date, Nothing, Nothing))
        End If

        ltc3.CollToDataSet(ds1, "ProductionPieceProcess", PPPS.ProductionPieceProcess_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        ltc4.CollToDataSet(ds1, "Company", mcCompany.Company_Getlist(strCompany, Nothing, Nothing, Nothing))
        ltc5.CollToDataSet(ds1, "ProductionPiecePersonnel", PPPa.ProductionPiecePersonnel_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))

        ' PreviewRPT1(ds1, "rptProductionWorkGroupSumPieceTime", "承包計件表打印", strInUserRank, strInUserRank, True, True)
        PreviewRPT1(ds1, "rptProductionWorkGroupSumPieceTimeCollect", "承包計件匯總表打印", strInUserRank, strInUserRank, True, True)

        ltc1 = Nothing
        ltc2 = Nothing
        ltc3 = Nothing
        ltc4 = Nothing
        ltc5 = Nothing

    End Sub

    Private Sub txtPY_PieceAllSum_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPY_PieceAllSum.EditValueChanged, TxtPY_TimeAllSum.EditValueChanged, TxtPY_CompensateSum.EditValueChanged, txtPY_SubtractSum.EditValueChanged, TxtPY_UseSum.EditValueChanged
        If Load_OK = "OK" Then
            SButtonSum_Click(Nothing, Nothing)
        End If
    End Sub

    Private Sub txtPY_PieceAllSum_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPY_PieceAllSum.KeyUp, TxtPY_TimeAllSum.KeyUp, TxtPY_CompensateSum.KeyUp, txtPY_SubtractSum.KeyUp, TxtPY_UseSum.KeyUp
        'Dim m As New System.Text.RegularExpressions.Regex("^+?(\d+(\.\d*)?|\.\d+)$")  '顯示整數,正浮點數正則表達式

        'If sender.Text = "" Then
        '    TempTextstr = ""
        '    Exit Sub
        'End If

        'If m.IsMatch(sender.Text) = True Then
        '    TempTextstr = sender.Text
        'Else
        '    sender.Text = TempTextstr
        '    sender.SelectionStart = Len(sender.Text)
        '    Exit Sub
        'End If
    End Sub


    Private Sub SButtonSum_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SButtonSum.Click

        If Check_Date() = True Then
        Else
            Exit Sub
        End If

        On Error Resume Next   ''主要對對 CSng 時，或文本框中有 字母就會出差

        ''計算出工計件工資 
        Dim i As Integer
        txtPiecePaySum.Text = 0
        txtMeritedPaySum.Text = 0
        txtPY_PieceSum.Text = 0
        TxtPY_TimeSum.Text = 0
        txtMeritedHoursSum.Text = 0
        txtPY_BonusSum.Text = 0
        txtBonusSum.Text = 0

        Dim SCompleteSum As Single '完成額
        Dim SPY_PieceAllSum As Single  '計件
        Dim SPY_TimeAllSum As Single '計時總額
        Dim SPY_CompensateSum As Single '應補
        Dim SPY_SubtractSum As Single '應扣

        Dim SG_NO_InSum As Single
        Dim SG_NO_OUTSum As Single



        '1    完成額=計時總額+計件+應補 ----------------------------------------- 
        ' Dim 完成額, 計件, 計時總額, 應補, 應扣 As Single

        SPY_PieceAllSum = CSng(txtPY_PieceAllSum.Text)
        SPY_TimeAllSum = CSng(TxtPY_TimeAllSum.Text)
        SPY_CompensateSum = CSng(TxtPY_CompensateSum.Text)
        SPY_SubtractSum = CSng(txtPY_SubtractSum.Text)

        SG_NO_InSum = CSng(txtG_NO_InSum.Text)
        SG_NO_OUTSum = CSng(txtG_NO_OUTSum.Text)

        '  SCompleteSum = SPY_PieceAllSum + SPY_TimeAllSum + SPY_CompensateSum + SG_NO_InSum - SPY_SubtractSum - SG_NO_OUTSum  '完成額=計件 + 計時總額 + 應補 - 應扣
        SCompleteSum = SPY_PieceAllSum + SPY_TimeAllSum + SPY_CompensateSum + SG_NO_InSum  '完成額=計件 + 計時總額 + 應補
        TxtPY_CompleteSum.Text = SCompleteSum

        '2    ---------------------------------------------------------------------------------------------
        Dim SPer_DayPrice As Single '日薪
        Dim SPYS_OnDutyDays As Single '上班天數
        Dim SPYS_UsualOverTime As Single '平時加班小時數
        Dim SPYS_HolidayOVerTime As Single '節假日加班小時數
        Dim SPYS_Proportion As Single '工時比例
        Dim SPYS_Bonus As Single '獎金

        Dim SPYS_AllHours As Single '總工時
        Dim SMeritedHoursSum As Single '應計工時
        Dim SPYS_MeritedHours As Single '計時工資

        Dim strPayType As String '薪金類型
        Dim strFormula As String '計算工式

        'Dim 日薪, 上班天數, 平日加班, 假日加班, 工時比例, 獎金, 總工時, 應計工時, 計時工資 As Single
        'Dim 薪金類型, 計算工式 As String

        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
            SPer_DayPrice = ds.Tables("PiecePayWGSub").Rows(i)("Per_DayPrice") '日薪
            SPYS_OnDutyDays = ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") '上班天數
            SPYS_UsualOverTime = ds.Tables("PiecePayWGSub").Rows(i)("PYS_UsualOverTime") '  /平時加班小時數

            SPYS_HolidayOVerTime = ds.Tables("PiecePayWGSub").Rows(i)("PYS_HolidayOVerTime") '  /節假日加班小時數
            SPYS_Proportion = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion") '  /工時比例
            SPYS_Bonus = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Bonus") '  /  /獎金

            ''  總工時=上班天數*8+加班時數+假日加班
            SPYS_AllHours = Math.Round(SPYS_OnDutyDays * 8 + SPYS_UsualOverTime + SPYS_HolidayOVerTime, 1)
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_AllHours") = SPYS_AllHours
            ''  '為計件時就計算,應計工時=總工時*工時比例
            strPayType = ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID").ToString   ''為計日薪時就不用算

            If strPayType = "計件" Then
                SMeritedHoursSum = Math.Round(SPYS_AllHours * SPYS_Proportion)
                txtMeritedHoursSum.Text = CSng(txtMeritedHoursSum.Text) + SMeritedHoursSum ''合計
            Else
                SMeritedHoursSum = 0
            End If
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedHours") = SMeritedHoursSum

            ''--------------------------------------------------------------------
            ' '獎金
            txtBonusSum.Text = CSng(txtBonusSum.Text) + SPYS_Bonus

            ''#################################################################################################
            '計算計時工資

            strFormula = ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID")

            Dim pfc As New ProductionPieceFormulaControl   ''得到自己編輯的工式,利用自編輯的工式計算
            Dim pfl As New List(Of ProductionPieceFormulaInfo)
            pfl = pfc.ProductionPieceFormula_GetList(Nothing, Nothing, strFormula, "True") '查詢出已啟用的

            Dim expression As String = pfl(0).Formula
            Dim parameters As NameValueCollection = New NameValueCollection()

            parameters.Add("日薪", SPer_DayPrice.ToString)
            parameters.Add("上班天數", SPYS_OnDutyDays.ToString)
            parameters.Add("平日加班", SPYS_UsualOverTime.ToString)
            parameters.Add("假日加班", SPYS_HolidayOVerTime.ToString)

            Dim results() As Decimal = Calculator.Eval(expression, parameters)
            SPYS_MeritedHours = results(0)

            ds.Tables("PiecePayWGSub").Rows(i)("PYS_TimePay") = SPYS_MeritedHours

            ''##########################################################################################
            ''計算在組別中進行計時工作的工資


            If strPayType = "計件" Then
            Else
                TxtPY_TimeSum.Text = CSng(TxtPY_TimeSum.Text) + SPYS_MeritedHours
            End If
        Next
        ''---------------------------------------------
        txtPY_BonusSum.Text = txtBonusSum.Text '獎金
        '部門承包計件額=計件SCompleteSum-物料耗用額-計時工資-其它(獎金)
        ' txtPY_PieceSum.Text = Math.Round(CSng(TxtPY_CompleteSum.Text) - CSng(TxtPY_UseSum.Text) - CSng(TxtPY_TimeSum.Text) - CSng(txtPY_BonusSum.Text))
        txtPY_PieceSum.Text = Math.Round(CSng(TxtPY_CompleteSum.Text) - CSng(TxtPY_UseSum.Text) - CSng(TxtPY_TimeSum.Text) - CSng(txtPY_BonusSum.Text) - CSng(txtPY_SubtractSum.Text) - CSng(txtG_NO_OUTSum.Text))
        ' - SPY_SubtractSum - SG_NO_OUTSum


        ''----------以下要據計件額進行，金額分配---------------------------------------------------------------------------------------------------

        Dim SPYS_Bonus1, SMeritedHoursSum1, SPYS_MeritedHours1, SPYS_MeritedPiece1, SPYS_MeritedPay As Single
        Dim SPYS_MeritedPiece2, SSave As Single
        Dim strFormula1 As String


        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
            SPYS_Bonus1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Bonus") '  /  /獎金
            SMeritedHoursSum1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedHours")
            SPYS_MeritedHours1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_TimePay")
            strFormula1 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_FormulaID")

            '計件工資=計件額/總應計工時*應計工時
            If CSng(txtPY_PieceSum.Text) = 0 Or CSng(txtMeritedHoursSum.Text) = 0 Or SMeritedHoursSum1 = 0 Then
                SPYS_MeritedPiece1 = 0
            Else
                SPYS_MeritedPiece1 = Math.Round(CSng(txtPY_PieceSum.Text) / CSng(txtMeritedHoursSum.Text) * SMeritedHoursSum1)
            End If

            SPYS_MeritedPiece2 = SPYS_MeritedPiece2 + SPYS_MeritedPiece1

            Select Case strFormula1
                Case "計件"
                    SPYS_MeritedPay = SPYS_MeritedPiece1 + SPYS_Bonus1
                Case Else
                    SPYS_MeritedPay = SPYS_MeritedHours1 + SPYS_Bonus1
            End Select

            ds.Tables("PiecePayWGSub").Rows(i)("PYS_PiecePay") = SPYS_MeritedPiece1 '    /計件工資
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_MeritedPay") = SPYS_MeritedPay     '  /應得工資

            txtPiecePaySum.Text = CSng(txtPiecePaySum.Text) + SPYS_MeritedPiece1
            txtMeritedPaySum.Text = CSng(txtMeritedPaySum.Text) + SPYS_MeritedPay
        Next

        ''暫時停用
        ''-------------------------------------***---------------------------------------------------------
        ' ''Dim k As Integer
        ' '' ''因為四舍五入時，計件工資， 有可能與實際分配后的總和有出入 ,把節余 加給工時比例最高的(無論是多還是少)
        '' ''Dim 薪金類型2 As String
        ' ''Dim strPayType2 As String
        ' ''Dim SPYS_Proportion2, temp As Single

        ' ''SSave = CSng(txtPY_PieceSum.Text) - SPYS_MeritedPiece2

        ' ''If SSave = 0 Then Exit Sub

        ' ''For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
        ' ''    strPayType2 = ds.Tables("PiecePayWGSub").Rows(i)("Per_PayType")  ''為計日薪時就不用算

        ' ''    If strPayType2 = "計件" Then
        ' ''        SPYS_Proportion2 = ds.Tables("PiecePayWGSub").Rows(i)("PYS_Proportion") '  /工時比例

        ' ''        If SPYS_Proportion2 > temp Then
        ' ''            temp = SPYS_Proportion2
        ' ''            k = i
        ' ''        End If
        ' ''    End If
        ' ''Next

        ' ''ds.Tables("PiecePayWGSub").Rows(k)("PYS_PiecePay") = ds.Tables("PiecePayWGSub").Rows(k)("PYS_PiecePay") + SSave '    /計件工資
        ' ''ds.Tables("PiecePayWGSub").Rows(k)("PYS_MeritedPay") = ds.Tables("PiecePayWGSub").Rows(k)("PYS_MeritedPay") + SSave    '  /應得工資

        ' ''txtPiecePaySum.Text = CSng(txtPiecePaySum.Text) + SSave
        ' ''txtMeritedPaySum.Text = CSng(txtMeritedPaySum.Text) + SSave
        ''---------------------------------------***-----------------------------------------------------------
    End Sub


    Private Sub ADDToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ADDToolStripMenuItem.Click
        'PYS_OnDutyDays  PiecePayWGSub
        If ds.Tables("PiecePayWGSub").Rows.Count <= 0 Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") = Val(ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays")) + 1
        Next



    End Sub

    Private Sub PLUSToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PLUSToolStripMenuItem.Click
        If ds.Tables("PiecePayWGSub").Rows.Count <= 0 Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") = Val(ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays")) - 1
        Next
    End Sub


    Private Sub RToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RToolStripMenuItem.Click

        If ds.Tables("PiecePayWGSub").Rows.Count <= 0 Then
            Exit Sub
        End If

        Dim i As Integer
        For i = 0 To ds.Tables("PiecePayWGSub").Rows.Count - 1

            LoadKQSumMonth(ds.Tables("PiecePayWGSub").Rows(i)("Per_NO"), Format(CDate(DatePY_YYMM.EditValue), "yyyMM"))  ''暫時用三月份的 

            '上班天數
            ds.Tables("PiecePayWGSub").Rows(i)("PYS_OnDutyDays") = DoubleNormalDays
        Next

    End Sub

    Private Sub RepositoryItemCalcEdit1_Spin(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.Controls.SpinEventArgs) Handles RepositoryItemCalcEdit1.Spin, RepositoryItemCalcEdit2.Spin, RepositoryItemCalcEdit3.Spin, RepositoryItemCalcEdit5.Spin, RepositoryItemCalcEdit6.Spin, RepositoryItemCalcEdit7.Spin
        e.Handled = True
    End Sub
End Class


