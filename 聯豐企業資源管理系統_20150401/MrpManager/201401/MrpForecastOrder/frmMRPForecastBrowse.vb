Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.Product
Imports LFERP.Library.Production.ProuctionWareOutB
Imports LFERP.Library.MrpManager.MrpForecastOrder
Imports LFERP.Library.MrpManager.MrpSetting
Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Imports Microsoft.Office.Interop
Imports DevExpress.XtraPrinting
Imports LFERP.Library.MrpManager.Bom_M
Imports LFERP.Library.MrpManager.Bom_D
Imports LFERP.Library.MrpManager.MrpMaterialCode
Imports System.Threading
Public Class frmMRPForecastBrowse

#Region "字段與實例"
    '中間表
    Dim dt As New DataTable
    Dim MrpCon As New MrpForecastOrderController
    Dim ds As New DataSet
    Dim MMICcon As New MrpMaterialCodeController
    Dim MrpList As New List(Of MrpMaterialCodeInfo)
    Dim MrpcInfo As New MrpMaterialCodeInfo


    Dim _StrSelect As String
    Dim type As Int16 = 0
    Dim _StrSource As String
    Private Property StrSource() As String
        Get
            Return _StrSource
        End Get
        Set(ByVal value As String)
            _StrSource = value
        End Set
    End Property
    Private Property StrSelect() As String
        Get
            Return _StrSelect
        End Get
        Set(ByVal value As String)
            _StrSelect = value
        End Set
    End Property
#End Region

#Region "頁面加載"
    Private Sub frmMRPForecastBrowse_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '權限判斷
        PowerUser()
        '控件賦值與數據綁定
        txtMO_CusterID.Properties.DisplayMember = "MO_CusterName"    'txt
        txtMO_CusterID.Properties.ValueMember = "MO_CusterID"   'EditValue
        Dim MrpInfo As New MrpForecastOrderInfo
        MrpInfo.MO_CusterID = "*全部*"
        MrpInfo.MO_CusterName = "*全部*"
        Dim MrpInfoList As New List(Of MrpForecastOrderInfo)
        MrpInfoList = MrpCon.CusterGetName(Nothing, Nothing)
        MrpInfoList.Insert(0, MrpInfo)
        txtMO_CusterID.Properties.DataSource = MrpInfoList
        '來源別
        gueSource.Properties.DisplayMember = "MC_Source"    'txt
        gueSource.Properties.ValueMember = "MC_SourceID"   'EditValue
        MrpList = MMICcon.MrpSource_GetList(Nothing, Nothing)
        gueSource.Text = "*全部*"
        gueSource.EditValue = "ALL"
        MrpcInfo.MC_SourceID = "ALL"
        MrpcInfo.MC_Source = "*全部*"
        MrpList.Insert(0, MrpcInfo)
        gueSource.Properties.DataSource = MrpList
        Dim BOCon As New Bom_MController
        Dim BoInfo As New Bom_MInfo
        GLU_MCode.Properties.DisplayMember = "M_Name"
        GLU_MCode.Properties.ValueMember = "ParentGroup"
        Dim MrpInfo2 As New Bom_MInfo
        MrpInfo2.ParentGroup = "*全部*"
        MrpInfo2.M_Name = "*全部*"
        MrpInfo2.M_Gauge = "*全部*"
        MrpInfo2.M_Unit = "*全部*"
        MrpInfo2.M_Source = "*全部*"
        Dim MrpInfoList2 As New List(Of Bom_MInfo)
        MrpInfoList2 = BOCon.Bom_M_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        MrpInfoList2.Insert(0, MrpInfo2)
        GLU_MCode.Properties.DataSource = MrpInfoList2
        txt_Type.Text = "顯示模式：        產品信息表 與    產品明細表"
        '加載個人的默認設置
        Dim MrpSetCon As New MrpSettingController
        Dim MrpSet As New MrpSettingInfo
        '判斷數據來源是否為空
        If MrpSetCon.MrpSetting_GetList(InUserID).Count > 0 Then
            MrpSet = MrpSetCon.MrpSetting_GetList(InUserID)(0)
        Else
            MrpSet.forecastBrowserBeginDate = Year(Now) & "/01/01"
            MrpSet.forecastBrowserEndDate = Year(Now) & "/12/31"
        End If
        '加載數據
        det_StartDate.DateTime = MrpSet.forecastBrowserBeginDate
        det_EndDate.DateTime = MrpSet.forecastBrowserEndDate
        LoadSub(txtSelect.Text, MrpSet.forecastBrowserBeginDate, MrpSet.forecastBrowserEndDate, Nothing, Nothing, Nothing)
        LoadSub2(txtSelect.Text, MrpSet.forecastBrowserBeginDate, MrpSet.forecastBrowserEndDate, Nothing, Nothing, Nothing)
    End Sub
#End Region

#Region "菜單欄功能"
    '導出Excle
    Private Sub cms_ToExcelChild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cms_ToExcelChild.Click
        If BandedGridView1.RowCount <= 0 Then
            Exit Sub
        End If
        'table列頭名稱處理
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim filePath As String
            filePath = FolderBrowserDialog1.SelectedPath
            filePath += "\YourExcel.xls"
            BandedGridView1.ExportToXls(filePath)
            Process.Start(filePath)
        End If
    End Sub
#End Region

#Region "按鈕功能"
    
    '查詢按鈕
    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFind.Click
        Dim date1 As Date
        Dim date2 As Date
        Dim StrCusterID As String
        '條件賦值
        If txtMO_CusterID.Text = String.Empty Or txtMO_CusterID.Text = "*全部*" Then
            StrCusterID = Nothing
        Else
            StrCusterID = txtMO_CusterID.EditValue
        End If
        date1 = det_StartDate.DateTime
        date2 = det_EndDate.DateTime
        StrSelect = txtSelect.EditValue
        If gueSource.EditValue = "ALL" Or gueSource.EditValue = String.Empty Then
            StrSource = Nothing
        Else
            StrSource = gueSource.EditValue
        End If
        '進行查詢
        LoadSub(StrSelect, date1, date2, StrCusterID, GLU_MCode.EditValue, StrSource)
        LoadSub2(StrSelect, date1, date2, StrCusterID, GLU_MCode.EditValue, StrSource)
    End Sub
#End Region

#Region "設置權限"
    '設置權限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010201")
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmNew.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010202")
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmEdit.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010203") '審核
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmDelete.Enabled = True
        'End If
        'pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "48010204") '確認審核
        'If pmwiL.Count > 0 Then
        '    If pmwiL.Item(0).PMWS_Value = "是" Then tsmCheck.Enabled = True
        'End If
    End Sub
#End Region

#Region "方法與事件"
    '得到時間周數的起止日期
    Public Function GetDateCrossStr(ByVal SrtCross As String) As String
        Try
            Dim first As DateTime
            Dim last As DateTime
            first = DateTime.MinValue
            last = DateTime.MinValue
            Dim Year As Int16 = CInt(Mid(SrtCross, 1, 4))
            Dim Week As Int16
            If Mid(SrtCross, 7, 1) = "周" Then
                Week = CInt(Mid(SrtCross, 6, 1))
            Else
                Week = CInt(Mid(SrtCross, 6, 2))
            End If
            If Year < 1700 Or Year > 9999 Then
                ' 年份超限
                Return False
            End If
            If Week < 0 Or Week > 53 Then
                ' 周数错误
                Return False
            End If
            Dim startDay As New DateTime(Year, 1, 1) ' //该年第一天
            Dim endDay As New DateTime(Year, 12, 31)
            Dim dayOfWeek As Int16 = 0

            If Convert.ToInt32(startDay.DayOfWeek.ToString("d")) > 0 Then
                dayOfWeek = Convert.ToInt32(startDay.DayOfWeek.ToString("d"))  '//该年第一天为星期几
            End If
            If dayOfWeek = 7 Then
                dayOfWeek = 0
            End If
            If Week = 0 Then
                first = startDay
                If dayOfWeek = 6 Then
                    last = first
                Else
                    last = startDay.AddDays((6 - dayOfWeek))
                End If
            Else
                first = startDay.AddDays((7 - dayOfWeek) + (Week - 2) * 7) '/Week周的起始日期
                last = first.AddDays(6)
                If last > endDay Then
                    last = endDay
                End If
            End If
            If first > endDay Then
                'startDayOfWeeks不在该年范围内
                '输入周数大于本年最大周数
                Return False
            End If
            Return Format(first, "MM/dd") + "-" + Format(last, "MM/dd")
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function
    '判斷日期是本年的第幾周
    Private Function WeekOfYear(ByVal date1 As Date) As Integer
        Dim WN As Integer
        Dim date2 = Year(date1) & "/01/01"
        WN = DateDiff(DateInterval.WeekOfYear, date2, date1, Microsoft.VisualBasic.FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstFullWeek)
        Return WN
    End Function
    '加載方法
    Private Sub LoadSub(ByVal StrSelect As String, ByVal date1 As Date, ByVal date2 As Date, ByVal CusterID As String, ByVal M_Code As String, ByVal Source As String)
        '清空控件
        Dim MrpSetCon As New MrpSettingController
        Dim MrpSetinfo As New MrpSettingInfo
        Dim i As Integer
        GridControl1.DataSource = Nothing
        BandedGridView1.Bands.Clear()
        BandedGridView1.Columns.Clear()
        If IsDBNull(M_Code) = True Then
            M_Code = Nothing
        End If
        If M_Code = "*全部*" Then
            M_Code = Nothing

        End If
        If CusterID = "*全部*" Then
            CusterID = Nothing
        End If
        Select Case StrSelect
            Case "[A]:周數"
                If MrpCon.GetWeekAllInfo(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt = MrpCon.GetWeekAllInfo(date1, date2, CusterID, M_Code, Source).Tables(0)

                Dim col As New DataColumn
                dt.Columns.Add(col)
                Dim j As Integer
                'table中每一行中數量求和，添加總計列
                Dim sum As Double = 0
                For i = 0 To dt.Rows.Count - 1
                    For j = 6 To dt.Columns.Count - 2
                        If IsDBNull(dt.Rows(i)(j)) = False Then
                            sum = sum + dt.Rows(i)(j)
                        End If
                    Next
                    dt.Rows(i)(col) = sum
                    sum = 0
                Next
                dt.Columns(dt.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt.Columns.Count - 1
                    Try
                        If CInt(dt.Columns(i).ColumnName) >= 0 Then
                            dt.Columns(i).ColumnName = Mid((dt.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt.Columns(i).ColumnName), 5, 2) + "周"
                        End If
                    Catch ex As Exception
                    End Try
                Next
                If dt.Rows.Count <= 0 Then
                    Exit Sub
                End If
                '固定塊的添加
                BandedGridView1.Bands.AddBand("產品信息")
                '將table數據放入Grid控件()
                GridControl1.DataSource = dt
                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 4
                    BandedGridView1.Bands(0).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next

                If dt.Columns.Count < 8 Then
                    Exit Sub
                End If
                Dim Sr As String = dt.Columns(6).ColumnName
                BandedGridView1.Bands.AddBand(Mid((dt.Columns(6).ColumnName), 1, 5))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(GetDateCrossStr(dt.Columns(6).ColumnName))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(6).ColumnName))
                BandedGridView1.Columns(dt.Columns(6).ColumnName).Width = 100
                For i = 7 To dt.Columns.Count - 2
                    If CInt(Mid((dt.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView1.Bands.AddBand(Mid((dt.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(GetDateCrossStr(SrtCross))
                        '指定相關列的位置
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        BandedGridView1.Bands(NumBands).Children.AddBand(GetDateCrossStr(SrtCross))
                        BandedGridView1.Bands(NumBands).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    End If
                Next
            Case "[B]:月份"
                If MrpCon.GetMonthAllInfo(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt = MrpCon.GetMonthAllInfo(date1, date2, CusterID, M_Code, Source).Tables(0)
                Dim col As New DataColumn
                dt.Columns.Add(col)
                Dim j As Integer
                'table中每一行中數量求和，添加總計列
                Dim sum As Double = 0
                For i = 0 To dt.Rows.Count - 1
                    For j = 6 To dt.Columns.Count - 2
                        If IsDBNull(dt.Rows(i)(j)) = False Then
                            sum = sum + dt.Rows(i)(j)
                        End If
                    Next
                    dt.Rows(i)(col) = sum
                    sum = 0
                Next
                dt.Columns(dt.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt.Columns.Count - 1
                    Try
                        If CInt(dt.Columns(i).ColumnName) >= 0 Then
                            dt.Columns(i).ColumnName = Mid((dt.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt.Columns(i).ColumnName), 5, 2) + "月"
                        End If
                    Catch ex As Exception
                    End Try
                Next
                If dt.Rows.Count <= 0 Then
                    Exit Sub
                End If
                '固定塊的添加
                BandedGridView1.Bands.AddBand("產品信息")
                '將table數據放入Grid控件()

                GridControl1.DataSource = dt
                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 4
                    BandedGridView1.Bands(0).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next
                If dt.Columns.Count < 7 Then
                    Exit Sub
                End If
                Dim Sr As String = dt.Columns(6).ColumnName
                BandedGridView1.Bands.AddBand(Mid((dt.Columns(6).ColumnName), 1, 5))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(Mid((dt.Columns(6).ColumnName), 6, 3))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(6).ColumnName))
                BandedGridView1.Columns(dt.Columns(6).ColumnName).Width = 100
                For i = 7 To dt.Columns.Count - 2
                    If CInt(Mid((dt.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView1.Bands.AddBand(Mid((dt.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(Mid(dt.Columns(i).ColumnName, 6, 3))
                        '指定相關列的位置
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        BandedGridView1.Bands(NumBands).Children.AddBand(Mid(dt.Columns(i).ColumnName, 6, 3))
                        BandedGridView1.Bands(NumBands).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    End If
                Next
            Case "[C]:周數【客戶】"
                If MrpSetCon.MrpSetting_GetList(InUserID).Count <= 0 Then
                    Exit Sub
                End If
                MrpSetinfo = MrpSetCon.MrpSetting_GetList(InUserID)(0)
                If CusterID = "*全部*" Then
                    CusterID = Nothing
                End If
                'table調用方法賦值
                If MrpCon.GetWeekInfo(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt = MrpCon.GetWeekInfo(date1, date2, CusterID, M_Code, Source).Tables(0)

                Dim col As New DataColumn
                dt.Columns.Add(col)
                Dim j As Integer
                'table中每一行中數量求和，添加總計列
                Dim sum As Double = 0
                For i = 0 To dt.Rows.Count - 1
                    For j = 8 To dt.Columns.Count - 2
                        If IsDBNull(dt.Rows(i)(j)) = False Then
                            sum = sum + dt.Rows(i)(j)
                        End If
                    Next
                    dt.Rows(i)(col) = sum
                    sum = 0
                Next
                dt.Columns(dt.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt.Columns.Count - 1
                    Try
                        If CInt(dt.Columns(i).ColumnName) >= 0 Then
                            dt.Columns(i).ColumnName = Mid((dt.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt.Columns(i).ColumnName), 5, 2) + "周"
                        End If
                    Catch ex As Exception
                    End Try
                Next
                If dt.Rows.Count <= 0 Then
                    Exit Sub
                End If
                '固定塊的添加
                BandedGridView1.Bands.AddBand("產品信息")
                BandedGridView1.Bands.AddBand("客戶信息")
                BandedGridView1.Bands(1).Name = "客戶信息"
                '將table數據放入Grid控件()
                GridControl1.DataSource = dt
                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 4
                    BandedGridView1.Bands(0).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next
                For i = 5 To 6
                    BandedGridView1.Bands(1).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next
                If dt.Columns.Count < 10 Then
                    Exit Sub
                End If
                Dim Sr As String = dt.Columns(8).ColumnName
                BandedGridView1.Bands.AddBand(Mid((dt.Columns(8).ColumnName), 1, 5))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(GetDateCrossStr(dt.Columns(8).ColumnName))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(8).ColumnName))
                BandedGridView1.Columns(dt.Columns(8).ColumnName).Width = 100
                For i = 9 To dt.Columns.Count - 2
                    If CInt(Mid((dt.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView1.Bands.AddBand(Mid((dt.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(GetDateCrossStr(SrtCross))
                        '指定相關列的位置
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        BandedGridView1.Bands(NumBands).Children.AddBand(GetDateCrossStr(SrtCross))
                        BandedGridView1.Bands(NumBands).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    End If
                Next
            Case "[D]:月份【客戶】"
                'CusterID 值處理
                If CusterID = "*全部*" Then
                    CusterID = Nothing
                End If

                'table調用方法賦值
                If MrpCon.GetMonthInfo(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt = MrpCon.GetMonthInfo(date1, date2, CusterID, M_Code, Source).Tables(0)
                'table中每一行中數量求和，添加總計列
                Dim col As New DataColumn
                dt.Columns.Add(col)

                Dim j As Integer
                Dim sum As Double = 0
                For i = 0 To dt.Rows.Count - 1
                    For j = 8 To dt.Columns.Count - 2
                        If IsDBNull(dt.Rows(i)(j)) = False Then
                            sum = sum + dt.Rows(i)(j)
                        End If
                    Next
                    dt.Rows(i)(col) = sum
                    sum = 0
                Next
                dt.Columns(dt.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt.Columns.Count - 1
                    Try
                        If CInt(dt.Columns(i).ColumnName) >= 0 Then
                            dt.Columns(i).ColumnName = Mid((dt.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt.Columns(i).ColumnName), 5, 2) + "月"
                        End If
                    Catch ex As Exception
                    End Try
                Next
                If dt.Rows.Count <= 0 Then
                    Exit Sub
                End If
                '固定塊的添加
                BandedGridView1.Bands.AddBand("產品信息")
                BandedGridView1.Bands.AddBand("客戶信息")
                BandedGridView1.Bands(1).Name = "客戶信息"
                '將table數據放入Grid控件()
                GridControl1.DataSource = dt

                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 4
                    BandedGridView1.Bands(0).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next

                For i = 5 To 6
                    BandedGridView1.Bands(1).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                Next
                If dt.Columns.Count < 10 Then
                    Exit Sub
                End If
                BandedGridView1.Bands.AddBand(Mid(dt.Columns(8).ColumnName, 1, 5))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(Mid(dt.Columns(8).ColumnName, 6, 3))
                BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(8).ColumnName))
                BandedGridView1.Columns(dt.Columns(8).ColumnName).Width = 100
                For i = 9 To dt.Columns.Count - 2
                    If CInt(Mid((dt.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView1.Bands.AddBand(Mid((dt.Columns(i).ColumnName), 1, 5))
                        '添加子塊
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand(Mid((dt.Columns(i).ColumnName), 6, 3))
                        '指定相關列的位置
                        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView1.Bands.Count - 1
                        Dim SrtCross As String = dt.Columns(i).ColumnName
                        BandedGridView1.Bands(NumBands).Children.AddBand(Mid((dt.Columns(i).ColumnName), 6, 3))
                        BandedGridView1.Bands(NumBands).Children(time).Columns.Add(BandedGridView1.Columns(dt.Columns(i).ColumnName))
                        BandedGridView1.Columns(dt.Columns(i).ColumnName).Width = 100
                    End If
                Next
        End Select
        '添加總計列
        BandedGridView1.Bands.AddBand("總計")
        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children.AddBand("數量")
        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Children(0).Columns.Add(BandedGridView1.Columns(BandedGridView1.Columns.Count - 1))
        '特定列的位置指定
        BandedGridView1.Columns("產品編號").Width = 130
        BandedGridView1.Columns("產品名稱").Width = 130
        Dim RepositoryItemMemoExEdit1 As New DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit
        BandedGridView1.Columns("規格").ColumnEdit = RepositoryItemMemoExEdit1
        BandedGridView1.Columns("規格").Width = 50
        BandedGridView1.Columns("來源碼").ColumnEdit = RepositoryItemMemoExEdit1
        BandedGridView1.Columns("來源碼").Width = 50
        RepositoryItemMemoExEdit1.ShowIcon = False
        BandedGridView1.Columns("MC_Source").Visible = False
        '列設置不可編輯與只讀
        For i = 0 To BandedGridView1.Columns.Count - 1
            BandedGridView1.Columns(i).OptionsColumn.ReadOnly = True
        Next
        For i = 0 To BandedGridView1.Columns.Count - 1
            BandedGridView1.Columns(i).OptionsColumn.AllowEdit = False
        Next
        BandedGridView1.Columns("規格").OptionsColumn.AllowEdit = True
        BandedGridView1.Columns("來源碼").OptionsColumn.AllowEdit = True
        BandedGridView1.Bands(0).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        If BandedGridView1.Bands(1).Name = "客戶信息" Then
            BandedGridView1.Bands(1).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        End If
        BandedGridView1.Bands(BandedGridView1.Bands.Count - 1).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right
    End Sub

    Private Sub LoadSub2(ByVal StrSelect As String, ByVal date1 As Date, ByVal date2 As Date, ByVal CusterID As String, ByVal M_Code As String, ByVal Source As String)
        '清空控件
        GridControl2.DataSource = Nothing
        BandedGridView2.Bands.Clear()
        BandedGridView2.Columns.Clear()
        Dim dt2 As New DataTable
        If IsDBNull(M_Code) = True Then
            M_Code = Nothing
        End If
        If M_Code = "*全部*" Then
            M_Code = Nothing
        End If
        If CusterID = "*全部*" Then
            CusterID = Nothing
        End If
        Dim MrpOrderCon As New MrpForecastOrderController
        Dim i As Integer
        Select Case StrSelect
            Case "[A]:周數"
                'table中每一行中數量求和，添加總計列
                If MrpOrderCon.GetWeekAllInfoChild(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt2 = MrpOrderCon.GetWeekAllInfoChild(date1, date2, CusterID, M_Code, Source).Tables(0)
                If dt2.Rows.Count <= 0 Then
                    Exit Sub
                End If
                Dim col As New DataColumn
                dt2.Columns.Add(col)
                Dim j As Integer
                Dim sum As Double = 0
                For i = 0 To dt2.Rows.Count - 1
                    For j = 7 To dt2.Columns.Count - 2
                        If IsDBNull(dt2.Rows(i)(j)) = False Then
                            sum = sum + dt2.Rows(i)(j)
                        End If
                    Next
                    dt2.Rows(i)(col) = sum
                    sum = 0
                Next
                dt2.Columns(dt2.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt2.Columns.Count - 1
                    Try
                        If CInt(dt2.Columns(i).ColumnName) >= 0 Then
                            dt2.Columns(i).ColumnName = Mid((dt2.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt2.Columns(i).ColumnName), 5, 2) + "周"
                        End If
                    Catch ex As Exception
                    End Try
                Next
                '固定塊的添加
                BandedGridView2.Bands.AddBand("產品信息")
                '將table數據放入Grid控件()
                GridControl2.DataSource = dt2
                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 5
                    BandedGridView2.Bands(0).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next
                If dt2.Columns.Count < 9 Then
                    Exit Sub
                End If
                Dim Sr As String = dt2.Columns(7).ColumnName
                BandedGridView2.Bands.AddBand(Mid((dt2.Columns(7).ColumnName), 1, 5))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(GetDateCrossStr(dt2.Columns(7).ColumnName))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(7).ColumnName))
                BandedGridView2.Columns(dt2.Columns(7).ColumnName).Width = 100
                For i = 8 To dt2.Columns.Count - 2
                    If CInt(Mid((dt2.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt2.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView2.Bands.AddBand(Mid((dt2.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(GetDateCrossStr(SrtCross))
                        '指定相關列的位置
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        BandedGridView2.Bands(NumBands).Children.AddBand(GetDateCrossStr(SrtCross))
                        BandedGridView2.Bands(NumBands).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    End If
                Next

            Case "[B]:月份"
                'table中每一行中數量求和，添加總計列
                If MrpOrderCon.GetMonthAllInfoChild(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt2 = MrpOrderCon.GetMonthAllInfoChild(date1, date2, CusterID, M_Code, Source).Tables(0)
                If dt2.Rows.Count <= 0 Then
                    Exit Sub
                End If
                Dim col As New DataColumn
                dt2.Columns.Add(col)
                Dim j As Integer
                Dim sum As Double = 0
                For i = 0 To dt2.Rows.Count - 1
                    For j = 7 To dt2.Columns.Count - 2
                        If IsDBNull(dt2.Rows(i)(j)) = False Then
                            sum = sum + dt2.Rows(i)(j)
                        End If
                    Next
                    dt2.Rows(i)(col) = sum
                    sum = 0
                Next
                dt2.Columns(dt2.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt2.Columns.Count - 1
                    Try
                        If CInt(dt2.Columns(i).ColumnName) >= 0 Then
                            dt2.Columns(i).ColumnName = Mid((dt2.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt2.Columns(i).ColumnName), 5, 2) + "月"
                        End If
                    Catch ex As Exception
                    End Try
                Next

                '固定塊的添加
                BandedGridView2.Bands.AddBand("產品信息")
                '將table數據放入Grid控件()
                GridControl2.DataSource = dt2

                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 5
                    BandedGridView2.Bands(0).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next

                If dt2.Columns.Count < 9 Then
                    Exit Sub
                End If
                Dim Sr As String = dt2.Columns(7).ColumnName
                BandedGridView2.Bands.AddBand(Mid((dt2.Columns(7).ColumnName), 1, 5))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(Mid(dt2.Columns(7).ColumnName, 6, 3))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(7).ColumnName))
                BandedGridView2.Columns(dt2.Columns(7).ColumnName).Width = 100
                For i = 8 To dt2.Columns.Count - 2
                    If CInt(Mid((dt2.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt2.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView2.Bands.AddBand(Mid((dt2.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(Mid(dt2.Columns(i).ColumnName, 6, 3))
                        '指定相關列的位置
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        BandedGridView2.Bands(NumBands).Children.AddBand(Mid(dt2.Columns(i).ColumnName, 6, 3))
                        BandedGridView2.Bands(NumBands).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    End If
                Next

            Case "[C]:周數【客戶】"
                'table中每一行中數量求和，添加總計列
                If MrpOrderCon.GetWeekInfoChild(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt2 = MrpOrderCon.GetWeekInfoChild(date1, date2, CusterID, M_Code, Source).Tables(0)
                If dt2.Rows.Count <= 0 Then
                    Exit Sub
                End If
                Dim col As New DataColumn
                dt2.Columns.Add(col)
                Dim j As Integer
                Dim sum As Double = 0
                For i = 0 To dt2.Rows.Count - 1
                    For j = 9 To dt2.Columns.Count - 2
                        If IsDBNull(dt2.Rows(i)(j)) = False Then
                            sum = sum + dt2.Rows(i)(j)
                        End If
                    Next
                    dt2.Rows(i)(col) = sum
                    sum = 0
                Next
                dt2.Columns(dt2.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt2.Columns.Count - 1
                    Try
                        If CInt(dt2.Columns(i).ColumnName) >= 0 Then
                            dt2.Columns(i).ColumnName = Mid((dt2.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt2.Columns(i).ColumnName), 5, 2) + "周"
                        End If
                    Catch ex As Exception
                    End Try
                Next

                '固定塊的添加
                BandedGridView2.Bands.AddBand("產品信息")
                BandedGridView2.Bands.AddBand("客戶信息")
                BandedGridView2.Bands(1).Name = "客戶信息"
                '將table數據放入Grid控件()
                GridControl2.DataSource = dt2
                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 5
                    BandedGridView2.Bands(0).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next
                For i = 6 To 7
                    BandedGridView2.Bands(1).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next
                If dt2.Columns.Count < 11 Then
                    Exit Sub
                End If
                Dim Sr As String = dt2.Columns(9).ColumnName
                BandedGridView2.Bands.AddBand(Mid((dt2.Columns(9).ColumnName), 1, 5))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(GetDateCrossStr(dt2.Columns(9).ColumnName))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(9).ColumnName))
                BandedGridView2.Columns(dt2.Columns(9).ColumnName).Width = 100
                For i = 10 To dt2.Columns.Count - 2
                    If CInt(Mid((dt2.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt2.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView2.Bands.AddBand(Mid((dt2.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(GetDateCrossStr(SrtCross))
                        '指定相關列的位置
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        BandedGridView2.Bands(NumBands).Children.AddBand(GetDateCrossStr(SrtCross))
                        BandedGridView2.Bands(NumBands).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    End If
                Next
            Case "[D]:月份【客戶】"
                'table中每一行中數量求和，添加總計列
                If MrpOrderCon.GetMonthInfoChild(date1, date2, CusterID, M_Code, Source).Tables.Count <= 0 Then
                    Exit Sub
                End If
                dt2 = MrpOrderCon.GetMonthInfoChild(date1, date2, CusterID, M_Code, Source).Tables(0)
                If dt2.Rows.Count <= 0 Then
                    Exit Sub
                End If
                Dim col As New DataColumn
                dt2.Columns.Add(col)
                Dim j As Integer
                Dim sum As Double = 0
                For i = 0 To dt2.Rows.Count - 1
                    For j = 9 To dt2.Columns.Count - 2
                        If IsDBNull(dt2.Rows(i)(j)) = False Then
                            sum = sum + dt2.Rows(i)(j)
                        End If
                    Next
                    dt2.Rows(i)(col) = sum
                    sum = 0
                Next
                dt2.Columns(dt2.Columns.Count - 1).ColumnName = "總計"
                'table列頭名稱處理
                For i = 0 To dt2.Columns.Count - 1
                    Try
                        If CInt(dt2.Columns(i).ColumnName) >= 0 Then
                            dt2.Columns(i).ColumnName = Mid((dt2.Columns(i).ColumnName), 1, 4) + "年" + Mid((dt2.Columns(i).ColumnName), 5, 2) + "月"
                        End If
                    Catch ex As Exception
                    End Try
                Next

                '固定塊的添加
                BandedGridView2.Bands.AddBand("產品信息")
                BandedGridView2.Bands.AddBand("客戶信息")
                BandedGridView2.Bands(1).Name = "客戶信息"
                '將table數據放入Grid控件()
                GridControl2.DataSource = dt2

                Dim time As Int16 = 0
                '指定相關列的位置
                For i = 0 To 5
                    BandedGridView2.Bands(0).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next
                For i = 6 To 7
                    BandedGridView2.Bands(1).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                Next
                If dt2.Columns.Count < 11 Then
                    Exit Sub
                End If
                BandedGridView2.Bands.AddBand(Mid((dt2.Columns(9).ColumnName), 1, 5))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(Mid(dt2.Columns(9).ColumnName, 6, 3))
                BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(9).ColumnName))
                BandedGridView2.Columns(dt2.Columns(9).ColumnName).Width = 100
                For i = 10 To dt2.Columns.Count - 2
                    If CInt(Mid((dt2.Columns(i).ColumnName), 1, 4)) > CInt(Mid((dt2.Columns(i - 1).ColumnName), 1, 4)) Then
                        time = 0
                        '添加對應塊
                        BandedGridView2.Bands.AddBand(Mid((dt2.Columns(i).ColumnName), 1, 5))
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        '添加子塊
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand(Mid(dt2.Columns(i).ColumnName, 6, 3))
                        '指定相關列的位置
                        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        '設置列的寬度
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    Else
                        time = time + 1
                        Dim NumBands As Integer = BandedGridView2.Bands.Count - 1
                        Dim SrtCross As String = dt2.Columns(i).ColumnName
                        BandedGridView2.Bands(NumBands).Children.AddBand(Mid(dt2.Columns(i).ColumnName, 6, 3))
                        BandedGridView2.Bands(NumBands).Children(time).Columns.Add(BandedGridView2.Columns(dt2.Columns(i).ColumnName))
                        BandedGridView2.Columns(dt2.Columns(i).ColumnName).Width = 100
                    End If
                Next
        End Select
        '添加總計列
        BandedGridView2.Bands.AddBand("總計")
        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children.AddBand("數量")
        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Children(0).Columns.Add(BandedGridView2.Columns(BandedGridView2.Columns.Count - 1))

        '特定列的位置指定
        BandedGridView2.Columns("產品編號").Width = 130
        BandedGridView2.Columns("物料編號").Width = 130

        Dim RepositoryItemMemoExEdit1 As New DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit
        BandedGridView2.Columns("物料規格").ColumnEdit = RepositoryItemMemoExEdit1
        BandedGridView2.Columns("來源碼").ColumnEdit = RepositoryItemMemoExEdit1
        BandedGridView2.Columns("來源碼").Width = 50
        BandedGridView2.Columns("物料規格").Width = 50
        RepositoryItemMemoExEdit1.ShowIcon = False
        BandedGridView2.Columns("MC_Source").Visible = False

        '列設置不可編輯與只讀
        For i = 0 To BandedGridView2.Columns.Count - 1
            BandedGridView2.Columns(i).OptionsColumn.ReadOnly = True
        Next
        For i = 0 To BandedGridView2.Columns.Count - 1
            BandedGridView2.Columns(i).OptionsColumn.AllowEdit = False
        Next
        BandedGridView2.Columns("物料規格").OptionsColumn.AllowEdit = True
        BandedGridView2.Columns("來源碼").OptionsColumn.AllowEdit = True

        BandedGridView2.Bands(0).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        If BandedGridView2.Bands(1).Name = "客戶信息" Then
            BandedGridView2.Bands(1).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left
        End If
        BandedGridView2.Bands(BandedGridView2.Bands.Count - 1).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right

    End Sub
    ''' <summary>
    ''' Excel 導入按鈕
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        If BandedGridView1.RowCount > 0 Then
            If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim filePath As String
                filePath = FolderBrowserDialog1.SelectedPath
                filePath += "\YourExcel1.xls"
                BandedGridView1.ExportToXls(filePath)
                Process.Start(filePath)
            End If
        End If

        If BandedGridView2.RowCount > 0 Then
            If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim filePath As String
                filePath = FolderBrowserDialog1.SelectedPath
                filePath += "\YourExcel2.xls"
                BandedGridView2.ExportToXls(filePath)
                Process.Start(filePath)
            End If
        End If

    End Sub
    ''' <summary>
    ''' 模式顯示按鈕
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub stb_Sort_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles stb_Sort.Click
        If type < 2 Then
            type += 1
        Else
            type = 0
        End If
        Select Case type
            Case 0
                SplitContainer1.Panel1Collapsed = False
                SplitContainer1.Panel2Collapsed = False
                txt_Type.Text = "顯示模式：        產品信息表 與    產品明細表"
            Case 1
                SplitContainer1.Panel1Collapsed = False
                SplitContainer1.Panel2Collapsed = True
                txt_Type.Text = "顯示模式：        產品信息表"
            Case 2
                SplitContainer1.Panel1Collapsed = True
                SplitContainer1.Panel2Collapsed = False
                txt_Type.Text = "顯示模式：        產品明細表"
        End Select


    End Sub
    ''' <summary>
    ''' 菜單欄—子表Excel導出
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        If BandedGridView2.RowCount <= 0 Then
            Exit Sub
        End If
        If FolderBrowserDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim filePath As String
            filePath = FolderBrowserDialog1.SelectedPath
            filePath += "\YourExcel.xls"
            BandedGridView2.ExportToXls(filePath)
            Process.Start(filePath)
        End If
    End Sub
    ''' <summary>
    ''' 子表的動態過濾
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BandedGridView1_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles BandedGridView1.FocusedRowChanged
        Me.BandedGridView2.ActiveFilterString = "[產品編號] = '" & BandedGridView1.GetFocusedRowCellValue("產品編號") & "'"
    End Sub

#End Region

End Class