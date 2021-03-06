Imports System
Imports LFERP.SystemManager
Imports LFERP.Library.ProductProcess
Imports LFERP.Library.NmetalSampleManager.NmetalSampleProcess
Imports LFERP.Library.ProductionField
Imports LFERP.Library.NmetalSampleManager.NmetalSamplePace
Imports LFERP.Library.NmetalSampleManager.NmetalSampleProcessMain
Imports LFERP.Library.NmetalSampleManager.NmetalSampleSetting

Public Class frmNmetalSampleProcess
#Region "属性"
    Dim ds As New DataSet
#End Region

#Region "新增"
    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        On Error Resume Next
        'Edit = False
        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "产品资料工艺流程单"
        tempValue3 = "生產加工"
        fr = New frmNmetalSampleProcessAdd

        fr.EditItem = EditEnumType.ADD '新增
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
#End Region

#Region "修改"
    Private Sub cmdEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEdit.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub
        Dim pc As New NmetalSampleProcessControl
        Dim piL As List(Of NmetalSampleProcessInfo)
        piL = pc.NmetalSampleProcessMain_GetList1(GridView1.GetFocusedRowCellValue("Pro_NO"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If piL(0).Pro_Check Then
            MsgBox("此工艺流程单已审核或復核，不允許修改！", 0, "提示")
            Exit Sub
        End If

        'Edit = True
        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "产品资料工艺流程单"
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd

        fr.EditItem = EditEnumType.EDIT '修改
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
#End Region

#Region "删除"
    Private Sub cmdDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDel.Click
        If GridView1.RowCount = 0 Then Exit Sub
        Dim pc As New NmetalSampleProcessControl
        Dim piL As New List(Of NmetalSampleProcessInfo)
        piL = pc.NmetalSampleProcessMain_GetList1(GridView1.GetFocusedRowCellValue("Pro_NO"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If piL(0).Pro_Check Then
            MsgBox("此工艺流程单已审核或復核，不允許刪除！", 0, "提示")
            Exit Sub
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''
        Dim SPE As New NmetalSamplePaceControler
        Dim som As New List(Of NmetalSamplePaceInfo)
        Dim StrPM_M_Code, StrM_Code As String
        StrPM_M_Code = GridView1.GetFocusedRowCellValue("PM_M_Code").ToString
        StrM_Code = GridView1.GetFocusedRowCellValue("M_Code").ToString
        som = SPE.NmetalSamplePace_Getlist(Nothing, Nothing, Nothing, StrM_Code, StrPM_M_Code, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If som.Count > 0 Then
            MsgBox("存在样办进度无法刪除", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        '''''''''''''''''''''''''''''''''''''''''''''''
        If MsgBox("確定要刪除单号為:" & GridView1.GetFocusedRowCellValue("Pro_NO"), MsgBoxStyle.YesNo, "提示") = MsgBoxResult.No Then Exit Sub

        If pc.NmetalSampleProcessMain_Delete(GridView1.GetFocusedRowCellValue("Pro_NO")) Then '刪除主表
            If pc.NmetalSampleProcessSub_Delete(GridView1.GetFocusedRowCellValue("Pro_NO"), Nothing) Then ' 刪除子表
                MsgBox("已刪除成功！", , "提示")
            End If
        End If
        cmdRef_Click(Nothing, Nothing)
        SampleProcessSub.DataSource = pc.NmetalSampleProcessSub_GetList(GridView1.GetFocusedRowCellValue("Pro_NO"), Nothing, Nothing, Nothing, Nothing, Nothing)
    End Sub
#End Region

#Region "查看"
    Private Sub cmdView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdView.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub

        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "View"
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd
        fr.EditItem = EditEnumType.VIEW  '查看
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
#End Region

#Region "审核"
    Private Sub cmdCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCheck.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub

        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "Check"
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd
        fr.EditItem = EditEnumType.CHECK  '审核
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
#End Region

#Region "复制"
    Private Sub cmdCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopy.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub

        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "Copy"
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd
        fr.EditItem = EditEnumType.COPY  '复制
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
#End Region

#Region "窗体载入"

    Private Sub frmNmetalSampleProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Click

    End Sub
    Private Sub frmSampleProcess_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PowerUser()
        cmdRef_Click(Nothing, Nothing)
    End Sub
#End Region


#Region "刷新"
    Private Sub cmdRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRef.Click
        ''相關參數設置
        Dim pc As New NmetalSampleProcessControl
        Dim msi As New List(Of NmetalSampleSettingInfo)
        Dim msc As New NmetalSampleSettingController
        Dim StrCheck As String = Nothing
        Dim StrUser As String = Nothing

        msi = msc.NmetalSampleSetting_GetList(InUserID)
        If msi.Count > 0 Then
            '1.審核類型
            Select Case msi(0).SampleProcessCheck
                Case "0,1"
                    StrCheck = Nothing
                Case "1"
                    StrCheck = "True"
                Case "0"
                    StrCheck = "False"
            End Select
            '1.用戶選擇
            If msi(0).SampleOrdersCreateUserID = "All" Then
                StrUser = Nothing
            Else
                StrUser = msi(0).SampleOrdersCreateUserID
            End If

            GridSampleProcess.DataSource = pc.NmetalSampleProcessMain_GetList1(Nothing, Nothing, "生產加工", Nothing, msi(0).SampleProcessBeginDate, Nothing, StrUser, StrCheck)
        Else
            GridSampleProcess.DataSource = pc.NmetalSampleProcessMain_GetList1(Nothing, Nothing, "生產加工", Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
    End Sub
#End Region

#Region "表格事件"
    Private Sub GridView1_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView1.FocusedRowChanged
        ProcessDetail()
    End Sub
    Sub ProcessDetail()
        Dim pc As New NmetalSampleProcessMainControler
        Dim StrPro_No As String = GridView1.GetFocusedRowCellValue("Pro_NO")

        If StrPro_No <> String.Empty Then
            SampleProcessSub.DataSource = pc.NmetalSampleProcessSub_GetItem(StrPro_No, Nothing, Nothing, Nothing, Nothing, Nothing)
        End If
    End Sub
#End Region

#Region "查询"
    Private Sub cmdQuery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdQuery.Click
        Dim fr As New frmNmetalSampleView
        fr = New frmNmetalSampleView
        fr.lbl_Title.Text = "样办查询--工艺"
        fr.EditItem = "SampleProcess"
        fr.ShowDialog()
        If fr.SampleProcessList.Count = 0 Then
            GridSampleProcess.DataSource = Nothing
            Exit Sub
        Else
            GridSampleProcess.DataSource = fr.SampleProcessList
        End If
    End Sub
#End Region

#Region "列印"
    '自定义列印
    Private Sub cmdPrintCustom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrintCustom.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim ltm As New CollectionToDataSet
        Dim pc As New NmetalSampleProcessMainControler
        Dim SP As New NmetalSampleProcessControl
        Dim strSO_No As String = GridView1.GetFocusedRowCellValue("Pro_NO").ToString

        ltc.CollToDataSet(dss, "SampleProcessMain", SP.NmetalSampleProcessMain_GetList1(strSO_No, Nothing, "生產加工", Nothing, Nothing, Nothing, Nothing, Nothing))
        ltm.CollToDataSet(dss, "SampleProcessSub", pc.NmetalSampleProcessSub_GetReport(strSO_No, Nothing, Nothing, Nothing, Nothing, Nothing))
        PreviewRPT(dss, "rptSampleProcess", "工艺资料表", True, True)
        ltc = Nothing
        ltm = Nothing
    End Sub
    '全部列印
    Private Sub cmdPrintAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrintAll.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim ltm As New CollectionToDataSet
        Dim pc As New NmetalSampleProcessMainControler
        Dim SP As New NmetalSampleProcessControl
        Dim strSO_No As String = GridView1.GetFocusedRowCellValue("Pro_NO").ToString

        ltc.CollToDataSet(dss, "SampleProcessMain", SP.NmetalSampleProcessMain_GetList1(strSO_No, Nothing, "生產加工", Nothing, Nothing, Nothing, Nothing, Nothing))
        ltm.CollToDataSet(dss, "SampleProcessSub", pc.NmetalSampleProcessSub_GetItem(strSO_No, Nothing, Nothing, Nothing, Nothing, Nothing))

        PreviewRPT(dss, "rptSampleProcess", "工艺资料表", True, True)

        ltc = Nothing
        ltm = Nothing
    End Sub
    '启用列印
    Private Sub cmdPrintUsed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrintUsed.Click
        Dim dss As New DataSet
        Dim ltc As New CollectionToDataSet
        Dim ltm As New CollectionToDataSet
        Dim pc As New NmetalSampleProcessMainControler
        Dim SP As New NmetalSampleProcessControl
        Dim strSO_No As String = GridView1.GetFocusedRowCellValue("Pro_NO").ToString

        ltc.CollToDataSet(dss, "SampleProcessMain", SP.NmetalSampleProcessMain_GetList1(strSO_No, Nothing, "生產加工", Nothing, Nothing, Nothing, Nothing, Nothing))
        ltm.CollToDataSet(dss, "SampleProcessSub", pc.NmetalSampleProcessSub_GetItem(strSO_No, Nothing, Nothing, Nothing, Nothing, True))

        PreviewRPT(dss, "rptSampleProcess", "工艺资料表", True, True)

        ltc = Nothing
        ltm = Nothing
    End Sub
#End Region

#Region "权限设定"
    '设置权限
    Sub PowerUser()
        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860201")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdAdd.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860202")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdEdit.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860203")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdDel.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860204")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then cmdCheck.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860205")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Me.cmdPrintCustom.Enabled = True
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860212")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Me.cmdAddFile.Enabled = True
        End If

        '2014-02-15
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860214")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Me.ModifePS_BarCodeBit.Enabled = True
        End If

        '張偉   2014-07-07
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860216")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Me.tsm_AddOtherWeight.Enabled = True
        End If

    End Sub
    Private Sub cmdAddFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddFile.Click
        '調用此产品资料的文件
        If GridView1.RowCount = 0 Then Exit Sub
        Dim open, update, down, Edit, del, detail As Boolean

        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As New List(Of PermissionModuleWarrantSubInfo)

        If GridView1.RowCount = 0 Then Exit Sub
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860207")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then update = True
            If pmwiL.Item(0).PMWS_Value = "否" Then update = False
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860208")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then down = True
            If pmwiL.Item(0).PMWS_Value = "否" Then down = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860209")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Edit = True
            If pmwiL.Item(0).PMWS_Value = "否" Then Edit = False
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860206")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then del = True
            If pmwiL.Item(0).PMWS_Value = "否" Then del = False
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860210")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then detail = True
            If pmwiL.Item(0).PMWS_Value = "否" Then detail = False
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860211")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then open = True
            If pmwiL.Item(0).PMWS_Value = "否" Then open = False
        End If
        FileShow("8602", GridView2.GetFocusedRowCellValue("PS_NO").ToString, open, update, down, Edit, del, detail)
    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click
        '調用此产品资料的文件
        If GridView1.RowCount = 0 Then Exit Sub
        Dim open, update, down, Edit, del, detail As Boolean

        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As New List(Of PermissionModuleWarrantSubInfo)
        If GridView1.RowCount = 0 Then Exit Sub
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860207")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then update = True
            If pmwiL.Item(0).PMWS_Value = "否" Then update = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860208")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then down = True
            If pmwiL.Item(0).PMWS_Value = "否" Then down = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860209")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then Edit = True
            If pmwiL.Item(0).PMWS_Value = "否" Then Edit = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860206")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then del = True
            If pmwiL.Item(0).PMWS_Value = "否" Then del = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860210")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then detail = True
            If pmwiL.Item(0).PMWS_Value = "否" Then detail = False
        End If
        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "860211")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then open = True
            If pmwiL.Item(0).PMWS_Value = "否" Then open = False
        End If
        FileShow("8602", GridView2.GetFocusedRowCellValue("Pro_NO").ToString, open, update, down, Edit, del, detail)
    End Sub
#End Region

#Region "表数据转Excel"
    Private Sub cmdExcelB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelB.Click
        If GridView2.RowCount = 0 Then Exit Sub
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Title = "導出Excel"
        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
        Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
        If FiledialogResult = Windows.Forms.DialogResult.OK Then
            If ExportToExcelOld(SampleProcessSub, saveFileDialog.FileName) Then
                MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
            End If
        End If
    End Sub

    Private Sub cmdExcelA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExcelA.Click
        If GridView1.RowCount = 0 Then Exit Sub
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Title = "導出Excel"
        saveFileDialog.Filter = "Excel2003文件(*.xls)|*.xls"
        Dim FiledialogResult As DialogResult = saveFileDialog.ShowDialog(Me)
        If FiledialogResult = Windows.Forms.DialogResult.OK Then
            If ExportToExcelOld(GridSampleProcess, saveFileDialog.FileName) Then
                MsgBox("已成功導出到：" + saveFileDialog.FileName, MsgBoxStyle.Information, "提示")
            End If
        End If
    End Sub
#End Region

    Private Sub ModifePS_BarCodeBit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModifePS_BarCodeBit.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub
        ''Dim pc As New SampleProcessControl
        ''Dim piL As List(Of SampleProcessInfo)
        ''piL = pc.SampleProcessMain_GetList1(GridView1.GetFocusedRowCellValue("Pro_NO"), Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        ''If piL(0).Pro_Check Then
        ''    MsgBox("此工艺流程单已审核或復核，不允許修改自動收發！", 0, "提示")
        ''    Exit Sub
        ''End If

        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        'tempValue = "产品资料工艺流程单"
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd

        fr.EditItem = EditEnumType.ELSEONE  '修改
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub



    Private Sub colChkPS_ShowReportBit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles colChkPS_ShowReportBit.CheckedChanged
        '1.确认是否有数据
        If GridView1.RowCount = 0 Then
            Exit Sub
        End If
        '2.初值设定
        Dim spcon As New NmetalSampleProcessControl
        Dim strPS_NO As String = String.Empty
        strPS_NO = GridView2.GetFocusedRowCellValue("PS_NO")
        If strPS_NO = String.Empty Then
            Exit Sub
        End If
        '3.修改显示
        If spcon.NmetalSampleProcessSub_ShowReport(strPS_NO) = False Then
            MsgBox("修改报表显示错误！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
    End Sub
    ''' <summary>
    ''' 張偉
    ''' 2014-07-04
    ''' 修改增加重量
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsm_AddOtherWeight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsm_AddOtherWeight.Click
        On Error Resume Next
        If GridView1.RowCount = 0 Then Exit Sub


        Dim fr As frmNmetalSampleProcessAdd
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is frmNmetalSampleProcessAdd Then
                fr.Activate()
                Exit Sub
            End If
        Next
        tempValue2 = GridView1.GetFocusedRowCellValue("Pro_NO").ToString
        fr = New frmNmetalSampleProcessAdd

        fr.EditItem = EditEnumType.ELSETWO   '修改
        fr.MdiParent = MDIMain
        fr.Show()
    End Sub
End Class