Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.NmetalSampleManager.NmetalSampleCollection
Imports LFERP.Library.NmetalSampleManager.NmetalSamplePace
Imports LFERP.Library.NmetalSampleManager.NmetalSampleSetting
Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Imports LFERP.Library.NmetalSampleManager.NmetalSampleWareInventory
Imports LFERP.Library.PieceProcess
Imports LFERP.Library.NmetalSampleManager.NmetalSampInventoryCheck
Imports LFERP.Library.NmetalSampleManager.NmetalSampleTransaction
Imports LFERP.Library.KnifeWareInventoryCheck



Public Class frmNmetalSampleCollection

#Region "緩存"
    Dim ds As New DataSet
    Dim scCon As New NmetalSampleCollectionControler

    '2014-04-21  姚駿
    Dim somcon As New NmetalSampleOrdersMainControler
    Dim pncon As New PersonnelControl
    Dim m_SamTransCtrl As New NmetalSampleTransactionControler

    Private m_BolFalg As Boolean

    Private strD_ID As String
    Private strTID As String

#End Region

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmSampleCollection_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '2014-04-21  姚駿
        'cmdRef_Click(Nothing, Nothing)

        PowerUser()

        '2014-04-21  姚駿
        '1加载部门
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

        '3加載狀態
        Dim samTransInfo As New List(Of NmetalSampleTransactionInfo)
        samTransInfo = m_SamTransCtrl.NmetalSampleTransactionType_GetList(Nothing, Nothing)
        chkStatusType.DataSource = samTransInfo
    End Sub
    '设置权限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "890701")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdAdd.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "890702")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdDel.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "890703")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdInventoryDrep.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "890705")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdInv.Enabled = True
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim fr As New frmNmetalSampleBarcode
        fr = New frmNmetalSampleBarcode
        fr.Lbl_Title.Text = "条码採集"
        fr.EditItem = "SampleCollection"
        'fr.EditValue = GridView3.GetFocusedRowCellValue("PM_M_Code")
        'fr.SO_ID = GridView3.GetFocusedRowCellValue("SO_ID")
        'fr.SS_Edition = GridView3.GetFocusedRowCellValue("SS_Edition")
        'fr.SS_Qty = GridView3.GetFocusedRowCellValue("SP_Qty")
        'fr.SP_ID = GridView3.GetFocusedRowCellValue("SP_ID")
        fr.ShowDialog()
        frmSampleCollection_Load(Nothing, Nothing)
    End Sub

    Private Sub cmdDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        If GridView3.RowCount = 0 Then Exit Sub
        Dim StrAutoID As String
        StrAutoID = GridView3.GetFocusedRowCellValue("AutoID").ToString()

        Dim spc As New NmetalSampleCollectionControler
        Dim spl As New List(Of NmetalSampleCollectionInfo)
        spl = spc.NmetalSampleCollection_Getlist(Nothing, Nothing, StrAutoID, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If spl.Count <= 0 Then
            MsgBox("數據已刪除！")
            Exit Sub
        End If
        If spl(0).StatusType <> String.Empty Then
            MsgBox("存在样办交易資料,無法刪除", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        '----------------------------------------------------------
        If MsgBox("確定要刪除名称為： '" & GridView3.GetFocusedRowCellValue("Code_ID").ToString & "' 的條碼嗎?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            If scCon.NmetalSampleCollection_Delete(Nothing, StrAutoID) = True Then
                MsgBox("刪除当前記錄信息成功！", 60, "提示")
                frmSampleCollection_Load(Nothing, Nothing)
            Else
                MsgBox("刪除当前选定記錄失敗，请檢查原因！", 60, "提示")
                Exit Sub
            End If
        End If
    End Sub

    Private Sub cmdRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRef.Click

        '2014-04-21  姚骏
        If MessageBox.Show("数据较多是否执行刷新?", "查询", MessageBoxButtons.OKCancel) = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Dim msi As New List(Of NmetalSampleSettingInfo)
        Dim msc As New NmetalSampleSettingController

        Dim StrCheck As String = Nothing
        Dim StrUser As String = Nothing
        Dim StrD_ID As String = Nothing
        Dim StrType As String = Nothing
        Dim StrOrdersBeginDate As String = Nothing

        msi = msc.NmetalSampleSetting_GetList(InUserID)
        If msi.Count > 0 Then
            '1.審核類型
            Select Case msi(0).SampleCollectionCheck
                Case "0,1"
                    StrCheck = Nothing
                Case "1"
                    StrCheck = "True"
                Case "0"
                    StrCheck = "False"
            End Select

            '1.用戶選擇
            If msi(0).SampleCollectionCreateUserID = "All" Then
                StrUser = Nothing
            Else
                StrUser = msi(0).SampleCollectionCreateUserID
            End If
            '2.部门選擇
            If msi(0).SampleCollectionD_ID = "All" Then
                StrD_ID = Nothing
            Else
                StrD_ID = msi(0).SampleCollectionD_ID
            End If
            '2.状态選擇
            If msi(0).SampleCollectionStatusType = "All" Then
                StrType = Nothing
            Else
                StrType = msi(0).SampleCollectionStatusType
            End If
            gridSampleCollection.DataSource = scCon.NmetalSampleCollection_Getlist(StrType, Nothing, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, StrD_ID, msi(0).SampleCollectionBeginDate, Nothing, StrUser, Nothing)
        Else
            gridSampleCollection.DataSource = scCon.NmetalSampleCollection_Getlist(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        Me.GridView3.ActiveFilterString = " [StatusType]<>'T' "
    End Sub

    Private Sub GridView3_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView3.FocusedRowChanged
        If GridView3.RowCount > 0 Then
            Try
                Dim SPQC As New NmetalSamplePaceQueryController
                Dim strCode_ID As String = GridView3.GetFocusedRowCellValue("Code_ID").ToString
                ' Me.GridBarCode.DataSource = SPQC.SamplePaceQuery_Getlist(Nothing, Nothing, strCode_ID, Nothing, Nothing, Nothing, True)
                Me.GridBarCode.DataSource = SPQC.NmetalSamplePaceQueryA_Getlist(strCode_ID)
            Catch
            End Try
        End If
    End Sub

    Private Sub cmdQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdQuery.Click
        Dim myfrm As New frmNmetalSampleCollectionQ
        Dim spc As New NmetalSampleCollectionControler
        myfrm.ShowDialog()

        If Mid(tempValue2, 1, 3) = "[A]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_Getlist(Nothing, tempValue, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        If Mid(tempValue2, 1, 3) = "[B]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_Getlist(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, tempValue, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
        If Mid(tempValue2, 1, 3) = "[C]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_Getlist(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, tempValue, Nothing, Nothing, Nothing, Nothing)
        End If
        If Mid(tempValue2, 1, 3) = "[D]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_Getlist(tempValue, Nothing, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If

        '2014-04-21  姚骏
        If Mid(tempValue2, 1, 3) = "[E]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_GetListByNewCondition(Nothing, Nothing, tempValue, Nothing)
        End If

        '2014-04-21  姚骏
        If Mid(tempValue2, 1, 3) = "[F]" Then
            gridSampleCollection.DataSource = spc.NmetalSampleCollection_GetListByNewCondition(Nothing, Nothing, Nothing, tempValue)
        End If

        tempValue = ""
        tempValue2 = ""
    End Sub

    Private Sub cmdExcelA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelA.Click
        If GridView3.RowCount = 0 Then Exit Sub
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Title = "導出Excel"
        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
        Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
        If FiledialogResult = Windows.Forms.DialogResult.OK Then
            If ExportToExcelOld(gridSampleCollection, saveFileDialog.FileName) Then
                MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
            End If
        End If
    End Sub

    Private Sub cmdExcelB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelB.Click
        If GridView2.RowCount = 0 Then Exit Sub
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Title = "導出Excel"
        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
        Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
        If FiledialogResult = Windows.Forms.DialogResult.OK Then
            If ExportToExcelOld(GridBarCode, saveFileDialog.FileName) Then
                MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
            End If
        End If
    End Sub

    Private Sub cmdInventoryDrep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInventoryDrep.Click
        On Error Resume Next
        Dim fr As frmNmetalSampInventoryCheckLoginMain
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampInventoryCheckLoginMain Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmNmetalSampInventoryCheckLoginMain
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub cmdInv_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdInv.Click
        On Error Resume Next
        Dim fr As frmNmetalSampInventoryMain
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampInventoryMain Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmNmetalSampInventoryMain
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub gridSampleCollection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gridSampleCollection.Click

    End Sub
#Region "控件全选"
    ''' <summary>
    ''' '2014-04-21  姚駿
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub chkD_IDAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkD_IDAll.CheckedChanged
        For i As Integer = 0 To Me.chkD_ID.ItemCount - 1
            If chkD_IDAll.Checked Then
                chkD_ID.SetItemChecked(i, True)
            Else
                chkD_ID.SetItemChecked(i, False)
            End If
        Next
    End Sub

    Private Sub chkPaceType_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPaceType.CheckedChanged
        For i As Integer = 0 To Me.chkStatusType.ItemCount - 1
            If chkPaceType.Checked Then
                chkStatusType.SetItemChecked(i, True)
            Else
                chkStatusType.SetItemChecked(i, False)
            End If
        Next
    End Sub

#End Region

#Region "控件返回值"
    ''' <summary>
    ''' 2014-04-21  姚駿
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub gluD_ID_QueryResultValue(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.QueryResultValueEventArgs) Handles gluD_ID.QueryResultValue
        Dim str As String = String.Empty
        strD_ID = String.Empty
        For i As Integer = 0 To chkD_ID.ItemCount - 1
            If chkD_ID.GetItemChecked(i) = True Then
                str += chkD_ID.GetItemText(i) + ","
                strD_ID += "'" + chkD_ID.GetItemValue(i) + "',"

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
    Private Sub gluStatusType_QueryResultValue(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.QueryResultValueEventArgs) Handles gluStatusType.QueryResultValue
        Dim str As String = String.Empty
        strTID = String.Empty
        For i As Integer = 0 To chkStatusType.ItemCount - 1
            If chkStatusType.GetItemChecked(i) = True Then
                str += chkStatusType.GetItemText(i) + ","
                strTID += "'" + chkStatusType.GetItemValue(i) + "',"

            End If
        Next
        If str.Length > 1 Then
            str = str.Remove(str.LastIndexOf(","), 1)
            strTID = strTID.Remove(strTID.LastIndexOf(","), 1)
        End If

        gluStatusType.ToolTipTitle = chkStatusType.Text
        gluStatusType.ToolTip = str
        e.Value = str
    End Sub
#End Region

#Region "查詢"
    ''' <summary>
    ''' 查詢
    ''' 2014-04-21
    ''' 姚駿
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSearch.Click


        If gluPM_M_Code.Text = String.Empty And gluD_ID.Text = String.Empty And gluStatusType.Text = String.Empty Then
            MsgBox("部门样办单号和状态不能同时为空", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If

        gridSampleCollection.DataSource = Nothing
        GridBarCode.DataSource = Nothing

        Dim strSampleID As String = Nothing                  '样办单号
        Dim strDep As String = Nothing                       '部門
        Dim strStatus As String = Nothing                    '狀態


        If gluPM_M_Code.Text <> String.Empty And gluPM_M_Code.EditValue <> "全部" Then
            strSampleID = gluPM_M_Code.EditValue
        End If

        If strD_ID <> String.Empty Then
            strDep = strD_ID
        End If

        If strTID <> String.Empty Then
            strStatus = strTID
        End If

        Dim sampleTempInfo As New List(Of NmetalSampleCollectionInfo)
        sampleTempInfo = scCon.NmetalSampleCollection_GetListByNewCondition(strDep, strStatus, strSampleID, Nothing)

        If sampleTempInfo.Count <= 0 Then
            MsgBox("查询数据不存在！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        gridSampleCollection.DataSource = sampleTempInfo

    End Sub
#End Region

    ''' <summary>
    ''' 部门查看
    ''' 2014-06-27          Mark
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsm_DepQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_DepQuery.Click
        On Error Resume Next
        Dim fr As frmNmetalSampleDepWeight
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleDepWeight Then
                fr.Activate()
                Exit Sub
            End If
        Next
        fr = New frmNmetalSampleDepWeight
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub tsm_leiyin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_leiyin.Click
        'Dim pncon As New NmetalSampleCollectionControler
        'Dim nswc As New NmetalSamplePaceQueryController
        'Dim dss As New DataSet
        'Dim ltc As New CollectionToDataSet
        'Dim ltc2 As New CollectionToDataSet
        'Dim strSO_No As String
        'strSO_No = GridView3.GetFocusedRowCellValue("Code_ID").ToString

        'ltc.CollToDataSet(dss, "NmetalSampleCollection", pncon.NmetalSampleCollection_Getlist(Nothing, strSO_No, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        'ltc2.CollToDataSet(dss, "NmetalSamplePace", nswc.NmetalSamplePaceQueryB_Getlist(strSO_No))


        'PreviewRPT(dss, "Report1", "资料表", True, True)
        'ltc = Nothing
        'ltc2 = Nothing
        On Error Resume Next
        Dim fr As New frmNmetalSampleCollectionQuery
        fr = New frmNmetalSampleCollectionQuery
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Normal
        fr.Show()
    End Sub

    Private Sub btn_reportbtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_reportbtn.Click
        Dim kwic As New KnifeWareInventoryCheckControl
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet
        Dim strSO_No As String
        strSO_No = txt_NumberNo.EditValue

        ltc.CollToDataSet(dss, "KnifeWareInventoryCheck", kwic.KnifeWareInventoryCheck_GetList(strSO_No, Nothing))
        ltc2.CollToDataSet(dss, "KnifeWareInventoryCheckProcess", kwic.KnifeWareInventoryCheckProcess_GetList(strSO_No, Nothing, Nothing))


        PreviewRPT(dss, "rptKnifeWareInventoryCheck", "刀具库盘点表", True, True)
        ltc = Nothing
        ltc2 = Nothing
    End Sub
End Class