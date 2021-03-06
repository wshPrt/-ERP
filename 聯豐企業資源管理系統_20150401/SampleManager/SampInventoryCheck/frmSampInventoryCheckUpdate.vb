Imports LFERP.DataSetting
Imports LFERP.Library.SampleManager.SampleOrdersMain
Imports LFERP.Library.SampleManager.SampleCollection
Imports LFERP.Library.PieceProcess
Imports LFERP.Library.SampleManager.SampleProcess
Imports LFERP.Library.SampleManager.SampleWareInventory
Imports LFERP.Library.SampleManager.SampInventoryCheck

Public Class frmSampInventoryCheckUpdate
    Dim socon As New SampleOrdersMainControler
    Dim scCon As New SampleCollectionControler
    Dim pncon As New PersonnelControl
    Dim SwCon As New SampleWareInventoryControler
    Dim prcon As New SampleProcessControl
    Dim sicom As New SampInventoryCheckControl

    Private _SO_ID As String
    'Private _PM_M_Code As String
    Private _SS_Edition As String
    'Private _SO_OrderQty As Integer
    Private intSO_NoSendQty As Integer
    Private intSO_OrderQty As Integer

    Property SO_ID() As String '属性
        Get
            Return _SO_ID
        End Get
        Set(ByVal value As String)
            _SO_ID = value
        End Set
    End Property
    'Property PM_M_Code() As String '属性
    '    Get
    '        Return _PM_M_Code
    '    End Get
    '    Set(ByVal value As String)
    '        _PM_M_Code = value
    '    End Set
    'End Property
    Property SS_Edition() As String '属性
        Get
            Return _SS_Edition
        End Get
        Set(ByVal value As String)
            _SS_Edition = value
        End Set
    End Property

    Private Sub frmCustomerFeedbackSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim pmlist As New List(Of PersonnelInfo) '部門分享
        pmlist = pncon.FacBriSearch_GetList(Nothing, Nothing, Nothing, Nothing)
        gluStateD_ID.Properties.DisplayMember = "DepName"
        gluStateD_ID.Properties.ValueMember = "DepID"
        gluStateD_ID.Properties.DataSource = pmlist

        gluEndD_ID.Properties.DisplayMember = "DepName"
        gluEndD_ID.Properties.ValueMember = "DepID"
        gluEndD_ID.Properties.DataSource = pmlist

        ''-------------------------------------------------------------
        'Dim solist As New List(Of SampleOrdersMainInfo)
        'solist = socon.SampleOrdersMain_GetList(SO_ID, Nothing, Nothing, Nothing, Nothing, Nothing, True)
        'If solist.Count > 0 Then
        '    txtCode_ID.Text = SO_ID
        '    'txtSS_Edition.Text = SS_Edition
        '    'txtPM_M_Code.Text = solist(0).PM_M_Code
        '    'txtSO_OrderQty.Text = solist(0).SO_OrderQty

        '    intSO_NoSendQty = solist(0).SO_NoSendQty
        '    intSO_OrderQty = solist(0).SO_OrderQty
        'End If

    End Sub
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        '1判空處理
        Dim strCode_ID As String = txtCode_ID.Text

        Dim strStatusType As String = String.Empty

        Label6.Text = String.Empty
        If txtCode_ID.Text = String.Empty Then
            MsgBox("條碼不能为空,请输入！", MsgBoxStyle.Information, "溫馨提示")
            txtCode_ID.Focus()
            Exit Sub
        End If
        '----------------------------------------條碼是否存在
        If scCon.SampleCollection_GetID(strCode_ID) = False Then
            txtCode_ID.Text = String.Empty
            txtCode_ID.Focus()
            MsgBox(strCode_ID & ",此條碼採集表中不存在!")
            Exit Sub
        End If

        Dim siclist As New List(Of SampInventoryCheckInfo)
        siclist = sicom.SampInventoryCheckLoginTime_GetList(strCode_ID)
        If siclist.Count > 0 Then
            txtCode_ID.Text = String.Empty
            txtCode_ID.Focus()
            MsgBox("此條碼在限定時間內重複操作！", MsgBoxStyle.Information, "溫馨提示")
            Exit Sub
        End If
        '-----------------------------------------------------------調整前
        Dim Sclist As List(Of SampleCollectionInfo)
        Sclist = scCon.SampleCollection_Getlist(Nothing, strCode_ID, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        If Sclist.Count >= 1 Then
            If Sclist(0).StatusType = "Z" Then
                strStatusType = "Z"
                If Me.gluStateD_ID.Text = String.Empty Or gluStateD_ID.EditValue = String.Empty Then
                    MsgBox("調整前條碼部門不能為空,請檢查原因！", MsgBoxStyle.Information, "溫馨提示")
                    Exit Sub
                End If
                If Me.gluStatePS_NO.Text = String.Empty Or Me.gluStatePS_NO.EditValue = String.Empty Then
                    MsgBox("調整前條碼工序不能為空,請檢查原因！", MsgBoxStyle.Information, "溫馨提示")
                    Exit Sub
                End If
            End If
        End If
        '-----------------------------------------------------------調整后
        If Me.gluEndD_ID.Text = String.Empty Or gluEndD_ID.EditValue = String.Empty Then
            MsgBox("調整后部門不能為空,请输入！", MsgBoxStyle.Information, "溫馨提示")
            gluEndD_ID.Focus()
            Exit Sub
        End If

        If Me.gluEndPS_NO.Text = String.Empty Or Me.gluEndPS_NO.EditValue = String.Empty Then
            MsgBox("調整后工序不能为空,请输入！", MsgBoxStyle.Information, "溫馨提示")
            gluEndPS_NO.Focus()
            Exit Sub
        End If

        If (Me.gluEndD_ID.EditValue = gluStateD_ID.EditValue) And (Me.gluEndPS_NO.EditValue = gluStatePS_NO.EditValue) Then
            'MsgBox("部門相同與工序相同不能調整！", MsgBoxStyle.Information, "溫馨提示")
            'Exit Sub
            Dim sicinfo As New SampInventoryCheckInfo
            sicinfo.Code_ID = strCode_ID
            sicinfo.OutD_ID = Me.gluStateD_ID.EditValue
            sicinfo.OutPS_NO = Me.gluStatePS_NO.EditValue
            sicinfo.Qty = 1
            sicinfo.InD_ID = Me.gluEndD_ID.EditValue
            sicinfo.InPS_NO = Me.gluEndPS_NO.EditValue
            sicinfo.AddDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
            sicinfo.AddAction = InUserID
            If sicom.SampInventoryCheckLogin_Add(sicinfo) = False Then
                MsgBox("保存記錄失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
                Exit Sub
            End If
            Label6.Text = "部門相同與工序相同不能調整"
            txtCode_ID.Text = String.Empty
            txtCode_ID.Focus()
            Exit Sub
        End If

        '3-----------------------------------------------------------庫存處理
        Dim SwInfo As New SampleWareInventoryInfo
        Dim SwList As New List(Of SampleWareInventoryInfo)
        Dim intinQty As Integer = 0
        Dim intOutQty As Integer = 0
        SwList = SwCon.SampleWareInventory_Getlist(Nothing, Nothing, gluStatePS_NO.EditValue, Nothing, False, gluStateD_ID.EditValue)
        If SwList.Count > 0 Then
            intOutQty = SwList(0).SWI_Qty
        Else
            If strStatusType = "Z" Then
                MsgBox("部門庫存不存在!", MsgBoxStyle.Information, "提示")
                Exit Sub
            End If
        End If
        '3.1出庫
        If strStatusType = "Z" Then
            SwInfo.SWI_Qty = intOutQty - 1
            SwInfo.ModifyDate = Format(Now, "yyyy/MM/dd")
            SwInfo.ModifyUserID = InUserID
            SwInfo.D_ID = Me.gluStateD_ID.EditValue
            SwInfo.PS_NO = Me.gluStatePS_NO.EditValue
            If SwCon.SampleWareInventory_Update(SwInfo) = False Then
                MsgBox("發料扣賬失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
                Exit Sub
            End If
        End If
        '3.2入庫
        SwList = SwCon.SampleWareInventory_Getlist(Nothing, Nothing, gluEndPS_NO.EditValue, Nothing, False, gluEndD_ID.EditValue)
        If SwList.Count > 0 Then
            intinQty = SwList(0).SWI_Qty
        End If

        SwInfo.SWI_Qty = intinQty + 1
        SwInfo.ModifyDate = Format(Now, "yyyy/MM/dd")
        SwInfo.ModifyUserID = InUserID
        SwInfo.D_ID = Me.gluEndD_ID.EditValue
        SwInfo.PS_NO = Me.gluEndPS_NO.EditValue
        If SwCon.SampleWareInventory_Update(SwInfo) = False Then
            MsgBox("收料入賬失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        '4-----------------------------------------------------------修改部門

        If scCon.SampleCollection_UpdateC(strCode_ID, Me.gluEndD_ID.EditValue) = False Then
            MsgBox("部門修改錯誤,請檢查原因!", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If

        If scCon.SampleCollection_UpdateA(strCode_ID, "Z") = False Then
            MsgBox("狀態修改錯誤,請檢查原因!", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If


        '5.保存記錄
        Dim siinfo As New SampInventoryCheckInfo
        siinfo.Code_ID = strCode_ID
        siinfo.OutD_ID = Me.gluStateD_ID.EditValue
        siinfo.OutPS_NO = Me.gluStatePS_NO.EditValue
        siinfo.Qty = 1
        siinfo.InD_ID = Me.gluEndD_ID.EditValue
        siinfo.InPS_NO = Me.gluEndPS_NO.EditValue
        siinfo.AddDate = Format(Now, "yyyy/MM/dd HH:mm:ss")
        siinfo.AddAction = InUserID

        If sicom.SampInventoryCheckLogin_Add(siinfo) = False Then
            MsgBox("保存記錄失敗,請檢查原因!", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If

        MsgBox("盤點調整完成！", MsgBoxStyle.Information, "溫馨提示")
        txtCode_ID.Text = String.Empty
        Me.gluStateD_ID.EditValue = String.Empty
        Me.gluStatePS_NO.EditValue = String.Empty
        Me.gluEndPS_NO.EditValue = String.Empty

        'Me.Close()
    End Sub

    Private Sub SimpleButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton2.Click
        Me.Close()
    End Sub

    Private Sub txtCode_ID_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode_ID.KeyDown
        If e.KeyCode = Keys.Enter Then

            Dim strCode_ID As String = txtCode_ID.Text
            Dim strPM_M_Code As String = String.Empty
            Dim strD_ID As String = String.Empty
            Dim strPS_NO As String = String.Empty
            If txtCode_ID.Text = String.Empty Then
                MsgBox("條碼不能为空,请输入！", MsgBoxStyle.Information, "溫馨提示")
                txtCode_ID.Focus()
                Exit Sub
            End If

            Dim siclist As New List(Of SampInventoryCheckInfo)
            siclist = sicom.SampInventoryCheckLoginTime_GetList(strCode_ID)
            If siclist.Count > 0 Then
                txtCode_ID.Text = String.Empty
                txtCode_ID.Focus()
                MsgBox("此條碼在限定時間內重複操作！", MsgBoxStyle.Information, "溫馨提示")
                Exit Sub
            End If
            '----------------------------------------條碼是否存在
            Dim Sclist As List(Of SampleCollectionInfo)
            Sclist = scCon.SampleCollection_Getlist(Nothing, strCode_ID, Nothing, Nothing, Nothing, Nothing, False, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
            If Sclist.Count >= 1 Then
                strPM_M_Code = Sclist(0).PM_M_Code
                strD_ID = Sclist(0).D_ID

                If (Sclist(0).StatusType <> "Z") And (Sclist(0).StatusType <> String.Empty) Then
                    txtCode_ID.Text = String.Empty
                    txtCode_ID.Focus()
                    MsgBox(strCode_ID & ",此條碼採集表中不是在產狀態!")
                    Exit Sub
                End If
            Else
                txtCode_ID.Text = String.Empty
                txtCode_ID.Focus()
                MsgBox(strCode_ID & ",此條碼採集表中不存在!")
                Exit Sub
            End If
            '----------------------------------------工藝
            Dim splist As New List(Of SampleProcessInfo)
            splist = prcon.SampleProcessMain_GetList(Nothing, strPM_M_Code, Nothing, Nothing, Nothing, Nothing, Nothing)

            gluStatePS_NO.Properties.ValueMember = "PS_NO"
            gluStatePS_NO.Properties.DisplayMember = "PS_Name"
            gluStatePS_NO.Properties.DataSource = splist

            gluEndPS_NO.Properties.ValueMember = "PS_NO"
            gluEndPS_NO.Properties.DisplayMember = "PS_Name"
            gluEndPS_NO.Properties.DataSource = splist

            '初始部門
            Dim silist As New List(Of SampInventoryCheckInfo)
            silist = sicom.SampInventoryCheckUpdate_GetList(strCode_ID)
            If silist.Count > 0 Then
                If silist(0).InPS_NO <> String.Empty Then
                    strPS_NO = silist(0).InPS_NO

                Else
                    strPS_NO = silist(0).OutPS_NO
                End If
            End If

            gluStateD_ID.EditValue = strD_ID
            gluStatePS_NO.EditValue = strPS_NO
            gluEndD_ID.EditValue = strD_ID
            Me.gluEndD_ID.Focus()
        End If
    End Sub
End Class