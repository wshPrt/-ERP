Module mdlSetting
    Public Edit As Boolean   '區分新增/修改
    Public EditSub As Boolean '區分子項新增/
    Public RefreshT As Boolean   '是否刷新


    Public InUser As String '= "管理員" '當前用戶
    Public InUserID As String '用戶編號
    Public strInDPT_ID As String '記錄當前用戶所屬部門編號
    Public strInDepID As String '記錄當前用戶所屬生產部門編號
    Public strInDepName As String   '記錄當前用戶所屬生產部門名稱
    Public strInUserRank As String  '記錄當前用戶生產權限級別
    Public strInUserType As String  '記錄當前用戶所屬生產工藝類型
    Public strInFacID As String '記錄當前用戶所屬廠別編號
    Public strInCompany As String '記錄當前用戶所屬公司數字代號
    Public ErpUser As New ERPSafe      '用戶權限列表
    Public tempValue As String    '臨時字符串,用於各模塊
    Public tempValue2 As String    '臨時字符串,用於各模塊
    Public tempValue3 As String '臨時字符串,用於各模塊
    Public tempValue4 As String '臨時字符串,用於各模塊
    Public tempValue5 As String '臨時字符串,用於各模塊
    Public tempValue6 As String '臨時字符串,用於各模塊
    Public tempValue7 As String '臨時字符串,用於各模塊
    Public tempValue8 As String '臨時字符串,用於各模塊
    Public tempValue9 As String '臨時字符串,用於各模塊
    Public tempValue10 As String '臨時字符串,用於各模塊
    Public tempValue11 As String '臨時字符串,用於各模塊
    Public tempValue12 As String '臨時字符串,用於各模塊
    Public tempValue13 As String '臨時字符串,用於各模塊
    Public tempAPID As String '臨時ID字符串,用於申購單模塊
    Public tempAPNum As String '臨時申購物料字符串,用於申購單模塊
    Public tempAPCode As String '臨時物料編碼字符串,用於申購單模塊
    Public tempAPName As String '臨時物料名稱字符串,用於申購單模塊
    Public tempAPGauge As String '臨時物料規格字符串,用於申購單模塊

    Public tempAPDateStart As String '臨時物料申購日期字符串,用於申購單模塊
    Public tempAPDateEnd As String '臨時物料申購日期字符串,用於申購單模塊
    Public tempAPDPTID As String '臨時物料申購部門ID,用於申購單模塊
    Public tempCode As String  '臨時物料編碼
    Public tempCode1 As String   '臨時物料編碼
    Public MTypeName As String  '臨時模塊功能名稱
    Public UserName As String '記錄登入用戶名
    Public CardNo As String  '記錄刷卡號
    Public arlM_Code As New ArrayList '記錄物料編號
    Public arlPS_NO As New ArrayList '記錄工序編號
    Public arl1 As New ArrayList '臨時數組
    Public arl2 As New ArrayList '臨時數組
    Public strPM_M_Code As String '記錄產品編號
    Public strPM_Type As String '記錄配件名稱(類型)
    Public strPM_TypeNO As String '記錄配件編號(類型)
    Public strPO_Type1 As String '記錄外發類型
    Public strName As String '記錄員工姓名
    Public strDepID As String '記錄員工所屬生產部門ID
    Public strPayType As String '記錄員工薪金類型
    Public strInFacIDFull As String
    Public strInDepIDFull As String  ''用戶所屬部門全稱
    Public strRefCard As String '記錄是否需要刷卡
    Public strUserLoginTime As String '記錄用戶登入時間



    Public DoubleNormalDays As Double '上班天數
    Public DoubleExtraHours As Double '平時加班
    Public DoubleWeekTime As Double '假日加班
    Public strMsg As String '計件薪金工式判斷有誤用到

    Public WareWarningMsgShowBZ As String

    Public strVer As String    '程序版本號
    Public isClickButton As Boolean  '記錄是否單擊了某個按鈕
    Public isOpenCOM As Boolean '記錄是否已打開COM口

    Public ReadComm As Double  ''發卡器端口
    Public strJIYU As String = "別名" '機玉修改為別名

    Public WeightTime As Decimal  ''稱重全局孌量
#Region "公共的操作類型"
    Public Enum EditEnumType
        ' <summary>
        ' 新增
        ' </summary>
        ADD = 0
        '<summary>
        ' 修改　
        '</summary>
        EDIT = 1
        '<summary>
        ' 刪除　
        '</summary>
        DEL = 2
        '<summary>
        ' 查看　
        '</summary>
        VIEW = 3
        '<summary>
        ' 查詢　
        '</summary>
        QUERY = 4
        '<summary>
        ' 打印　
        '</summary>
        PRINT = 5
        '<summary>
        ' 審核　
        '</summary>
        CHECK = 6
        '<summary>
        ' 審核　
        '</summary>
        RECHECK = 7
        '<summary>
        ' 刷新　
        '</summary>
        REF = 8
        '<summary>
        ' 刷新　
        '</summary>
        INCHECK = 9
        '<summary>
        ' 其他1　
        '</summary>
        ELSEONE = 10
        '<summary>
        ' 其他2
        '</summary>
        ELSETWO = 11
        '<summary>
        ' 复制
        '</summary>
        COPY = 12
        ' <summary>
        ' 发料
        ' </summary>
        OUT = 13
    End Enum
    Public Sub ConrotlExportExcel(ByVal obj As Object)
        Try
            If obj Is Nothing Then
                Exit Sub
            End If
            If TryCast(obj, DevExpress.XtraGrid.GridControl) Is Nothing Then
                If obj.nodes.count < 1 Then
                    MsgBox("沒有可導出的數據", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If
            Else
                If obj.MainView.RowCount < 1 Then
                    MsgBox("沒有可導出的數據", MsgBoxStyle.Information, "提示")
                    Exit Sub
                End If
            End If

            Dim sfd As New SaveFileDialog
            sfd.DefaultExt = ".xls"
            sfd.Filter = "Excel Files|*.xls|All Files|*.*"
            If sfd.ShowDialog() = Windows.Forms.DialogResult.OK Then
                obj.ExportToXls(sfd.FileName)
                MsgBox("已成功導出", MsgBoxStyle.Information, "提示")
            End If
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Information, "提示")
        End Try
    End Sub
    Public Function GetDateCrossStr(ByVal dateSend As Date) As String
        Return mdlSetting.GetTimeSpan(dateSend)
    End Function
#Region "取得指定日期的星期所包含的日期段"
    Public Function GetTimeSpan(ByRef paraDate As Date) As String
        Dim i As Int16
        i = DatePart(DateInterval.Weekday, paraDate) - 1
        Dim firstDayOfWeek, lastDayOfWeek As String
        If Year(paraDate) <> Year(paraDate.AddDays(-i)) Then
            firstDayOfWeek = Year(paraDate).ToString + ".01.01"
            paraDate = CDate(Year(paraDate).ToString + ".01.01")
        Else
            firstDayOfWeek = Format(paraDate.AddDays(-i), "yyyy.MM.dd")
        End If

        If Year(paraDate) <> Year(paraDate.AddDays(6 - i)) Then
            lastDayOfWeek = Year(paraDate).ToString + ".12.31"
            paraDate = CDate(Year(paraDate).ToString + ".12.31")
        Else
            lastDayOfWeek = Format(paraDate.AddDays(6 - i), "yyyy.MM.dd")
        End If

        Return firstDayOfWeek + "-" + lastDayOfWeek
    End Function
#End Region
    Public Function ExportToExcelOld(ByVal GridObject As DevExpress.XtraGrid.GridControl, ByVal FilePath As String) As Boolean
        Try
            GridObject.ExportToExcelOld(FilePath)
            ExportToExcelOld = True
        Catch
            ExportToExcelOld = False
        End Try
    End Function


    Public Function EditEnumValue(ByVal strEditType As Integer) As String
        Dim strEditEnumValue As String = String.Empty
        Select Case strEditType
            Case EditEnumType.ADD
                strEditEnumValue = "--新增"
            Case EditEnumType.EDIT
                strEditEnumValue = "--修改"
            Case EditEnumType.DEL
                strEditEnumValue = "--刪除"
            Case EditEnumType.VIEW
                strEditEnumValue = "--查看"
            Case EditEnumType.QUERY
                strEditEnumValue = "--查詢"
            Case EditEnumType.PRINT
                strEditEnumValue = "--打印"
            Case EditEnumType.CHECK
                strEditEnumValue = "--審核"
            Case EditEnumType.RECHECK
                strEditEnumValue = "--復審"
            Case EditEnumType.REF
                strEditEnumValue = "--刷新"
            Case EditEnumType.INCHECK
                strEditEnumValue = "--確認"
            Case EditEnumType.ELSEONE
                strEditEnumValue = "--供應商選擇"
            Case EditEnumType.ELSETWO
                strEditEnumValue = "--其他2"
            Case EditEnumType.COPY
                strEditEnumValue = "--复制"
            Case EditEnumType.OUT
                strEditEnumValue = "--发料"
        End Select
        Return strEditEnumValue
    End Function

#End Region
End Module
