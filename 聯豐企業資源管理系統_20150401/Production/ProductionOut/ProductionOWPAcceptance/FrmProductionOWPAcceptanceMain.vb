Imports LFERP.SystemManager
Imports LFERP.Library.ProductionOWPAcceptance


Public Class FrmProductionOWPAcceptanceMain



    Private Sub popProceAcceptanceSeek_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceSeek.Click

        Dim mc As New ProductionOWPAcceptanceControl

        Dim myfrm As New FrmProductionOWPAcceptanceFind
        myfrm.ShowDialog()

        If tempValue = "F" Then
            Grid1.DataSource = mc.ProductionOWPAcceptance_GetList(Nothing, tempValue2, tempValue3, tempValue4, tempValue6, tempValue7, tempValue8, tempValue5, Nothing, Nothing, tempValue9, tempValue10, Nothing, Nothing)
        End If

        tempValue = Nothing
        tempValue2 = Nothing
        tempValue3 = Nothing
        tempValue4 = Nothing
        tempValue5 = Nothing
        tempValue6 = Nothing
        tempValue7 = Nothing
        tempValue8 = Nothing
        tempValue9 = Nothing
        tempValue10 = Nothing
    End Sub

    Private Sub popProceAcceptanceAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceAdd.Click
        ''暫收
        On Error Resume Next
        Edit = False
        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next
        MTypeName = "OWPAddEdit"
        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub popProceAcceptanceEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceEdit.Click
        ''修改
        On Error Resume Next
        Dim osc As New ProductionOWPAcceptanceControl
        Dim osilist As List(Of ProductionOWPAcceptanceInfo)

        osilist = osc.ProductionOWPAcceptance_GetList(Nothing, GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If osilist(0).A_Check = True Or osilist(0).A_AccCheck = True Then
            MsgBox("此验收單已審核或復核，不允許修改！")
            Exit Sub
        End If

        Edit = True

        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next

        MTypeName = "OWPAddEdit"
        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        tempValue2 = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString
        fr.Show()
    End Sub

    Private Sub popProceAcceptanceView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceView.Click
        ''查看
        On Error Resume Next

        If GridView1.RowCount = 0 Then Exit Sub
        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next

        MTypeName = "OWPView"
        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        tempValue2 = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString
        fr.Show()
    End Sub

    Private Sub popProceAcceptanceDel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceDel.Click
        Dim StrA As String
        StrA = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString


        Dim osc As New ProductionOWPAcceptanceControl
        Dim osilist As List(Of ProductionOWPAcceptanceInfo)

        osilist = osc.ProductionOWPAcceptance_GetList(Nothing, GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If osilist(0).A_Check = True Or osilist(0).A_AccCheck = True Then
            MsgBox("此验收單已審核或復核，不允許刪除！")
            Exit Sub
        End If

        If MsgBox("你確定刪除驗收單號為  '" & StrA & "'  的記錄嗎?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

            Dim mi As New ProductionOWPAcceptanceInfo
            Dim mc As New ProductionOWPAcceptanceControl
            mi.A_AcceptanceNO = StrA
            If mc.ProductionOWPAcceptance_Delete(mi.A_AcceptanceNO, Nothing) = True Then
                Grid1.DataSource = mc.ProductionOWPAcceptance_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            Else
                MsgBox("刪除失敗")
            End If

        End If
    End Sub

    Private Sub popProceAcceptanceCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceCheck.Click
        On Error Resume Next
        Dim osc As New ProductionOWPAcceptanceControl
        Dim osilist As List(Of ProductionOWPAcceptanceInfo)
        osilist = osc.ProductionOWPAcceptance_GetList(Nothing, GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        If osilist(0).A_Check = True Then
            MsgBox("此验收單已審核，不允許審核！")
            Exit Sub
        End If

        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next

        MTypeName = "OWPCheck"
        tempValue2 = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString

        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub popProceAcceptanceAccCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceAccCheck.Click
        On Error Resume Next
        Dim osc As New ProductionOWPAcceptanceControl
        Dim osilist As List(Of ProductionOWPAcceptanceInfo)

        osilist = osc.ProductionOWPAcceptance_GetList(Nothing, GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        If osilist(0).A_Check = False Then
            MsgBox("必須先驗收,會計部才能審核!")
            Exit Sub
        End If

        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next
        tempValue2 = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString
        MTypeName = "OWPAccCheck"
        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub

    Private Sub popProceAcceptanceRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptanceRef.Click
        Dim ac As New ProductionOWPAcceptanceControl
        Grid1.DataSource = ac.ProductionOWPAcceptance_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, DateAdd(DateInterval.Day, -7, CDate(Format(Now, "yyyy/MM/dd"))), CDate(Format(Now, "yyyy/MM/dd")), Nothing, Nothing)
    End Sub

    Private Sub FrmProductionOWPAcceptanceMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim ac As New ProductionOWPAcceptanceControl
        Grid1.DataSource = ac.ProductionOWPAcceptance_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, DateAdd(DateInterval.Day, -7, CDate(Format(Now, "yyyy/MM/dd"))), CDate(Format(Now, "yyyy/MM/dd")), Nothing, Nothing)

        PowerUser()
    End Sub


    '設置權限
    Sub PowerUser()

        Dim pmws As New PermissionModuleWarrantSubController
        Dim pmwiL As List(Of PermissionModuleWarrantSubInfo)

        popProceAcceptanceAdd.Enabled = False
        popProceAcceptanceEdit.Enabled = False
        popProceAcceptanceDel.Enabled = False

        popProceAcceptanceView.Enabled = False
        popProceAcceptanceCheck.Enabled = False
        popProceAcceptanceAccCheck.Enabled = False

        popProceAcceptanceRef.Enabled = False
        popProceAcceptanceSeek.Enabled = False
        popProceAcceptancePrint.Enabled = False
        QuitMenuItem.Enabled = False


        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150201")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceAdd.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150202")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceEdit.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150203")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceDel.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150204")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceView.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150205")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceCheck.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150206")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceAccCheck.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150207")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceRef.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150208")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptanceSeek.Enabled = True
        End If

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150209")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then popProceAcceptancePrint.Enabled = True
        End If


        ''QuitMenuItem

        pmwiL = pmws.PermissionModuleWarrantSub_GetList(InUserID, "88150210")
        If pmwiL.Count > 0 Then
            If pmwiL.Item(0).PMWS_Value = "是" Then QuitMenuItem.Enabled = True
        End If

    End Sub
    Private Sub popProceAcceptancePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles popProceAcceptancePrint.Click
        Dim ds As New DataSet

        If GridView1.RowCount = 0 Then Exit Sub

        Dim ltc1 As New CollectionToDataSet
        Dim ltc2 As New CollectionToDataSet
        Dim ltc3 As New CollectionToDataSet
        Dim ltc4 As New CollectionToDataSet

        Dim mcOutwardAcc As New ProductionOWPAcceptanceControl
        Dim mcSupplier As New LFERP.DataSetting.SuppliersControler
        Dim mcProces As New LFERP.Library.ProductProcess.ProcessMainControl
        Dim mcCompany As New LFERP.DataSetting.CompanyControler

        ds.Tables.Clear()

        Dim strA As String

        strA = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString

        ltc1.CollToDataSet(ds, "ProductionOWPAcceptance", mcOutwardAcc.ProductionOWPAcceptance_GetList(Nothing, strA, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        ltc2.CollToDataSet(ds, "Suppliers", mcSupplier.GetSuppliersList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "True"))
        ltc3.CollToDataSet(ds, "Processsub", mcProces.ProcessMain_GetList(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing))
        ltc4.CollToDataSet(ds, "Company", mcCompany.Company_Getlist(Nothing, "MG", Nothing, Nothing))

        PreviewRPT(ds, "rptProductionOWPAcceptance", "供應商送貨單", True, True)

        ltc1 = Nothing
        ltc2 = Nothing
    End Sub


    ''加取消審核 23012-6-30
    Private Sub QuitMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitMenuItem.Click
        On Error Resume Next
        Dim osc As New ProductionOWPAcceptanceControl
        Dim osilist As List(Of ProductionOWPAcceptanceInfo)
        osilist = osc.ProductionOWPAcceptance_GetList(Nothing, GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

        If osilist(0).A_Check = False Then
            MsgBox("此验收單未審核，不允許取消審核！")
            Exit Sub
        End If

        Dim fr As FrmProductionOWPAcceptance
        For Each fr In MDIMain.MdiChildren
            If TypeOf fr Is FrmProductionOWPAcceptance Then
                fr.Activate()
                Exit Sub
            End If
        Next

        MTypeName = "OWPCheck"
        tempValue2 = GridView1.GetFocusedRowCellValue("A_AcceptanceNO").ToString

        fr = New FrmProductionOWPAcceptance
        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()
    End Sub
End Class