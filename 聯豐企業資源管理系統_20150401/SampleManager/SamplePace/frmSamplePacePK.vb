Imports LFERP.Library.SampleManager.SamplePacking
Imports LFERP.Library.SampleManager.SamplePace
Public Class frmSamplePacePK
#Region "属性"
    Private spcon As New SamplePackingController
    Private saCon As New SamplePaceControler
    Private _PM_M_Code As String '產品編號
    Private _D_ID As String '產品編號
    Private _SE_ID As String
    Private _AutoID As String
    Private _Qty As Integer
    Private _SE_TypeName As String
    Property AutoID() As String '属性
        Get
            Return _AutoID
        End Get
        Set(ByVal value As String)
            _AutoID = value
        End Set
    End Property
    Property PM_M_Code() As String '属性
        Get
            Return _PM_M_Code
        End Get
        Set(ByVal value As String)
            _PM_M_Code = value
        End Set
    End Property
    Property D_ID() As String '属性
        Get
            Return _D_ID
        End Get
        Set(ByVal value As String)
            _D_ID = value
        End Set
    End Property
    Property Qty() As String '属性
        Get
            Return _Qty
        End Get
        Set(ByVal value As String)
            _Qty = value
        End Set
    End Property
    Property SE_ID() As String '属性
        Get
            Return _SE_ID
        End Get
        Set(ByVal value As String)
            _SE_ID = value
        End Set
    End Property
    Property SE_TypeName() As String '属性
        Get
            Return _SE_TypeName
        End Get
        Set(ByVal value As String)
            _SE_TypeName = value
        End Set
    End Property

#End Region

#Region "按键事件"
    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSave.Click
        If DataCheck() = True Then
            Exit Sub
        End If
        '1.生成装箱单号主档
        Dim strPK_ID As String = GetPK_ID()
        Dim SSI As New SamplePackingInfo
        SSI.PK_ID = strPK_ID
        SSI.Remark = String.Empty
        SSI.AddUserID = InUserID
        SSI.Code_ID = Me.txtCode_ID.Text '裝箱條碼
        SSI.PM_M_Code = PM_M_Code
        SSI.Shelves_ID = String.Empty
        SSI.PackingDate = Format(Now, "yyyy/MM/dd")
        SSI.PackingUserID = InUser
        SSI.PackingType = "成品"
        SSI.Qty = Qty
        SSI.D_ID = D_ID
        SSI.SE_ID = AutoID
        SSI.Remark = txt_remark.Text
        SSI.AddDate = Format(Now, "yyyy/MM/dd")
        SSI.SealCode_ID = Me.txtSealCode_ID.Text

        Select Case SE_TypeName
            Case "良品"
                SSI.UseCount = 0
            Case "完工"
                SSI.UseCount = 0
        End Select

        SSI.UsePKCount = 0
        If spcon.SamplePacking_Add(SSI) = False Then
            MsgBox("添加失敗，请檢查原因！")
            Exit Sub
        End If


        '2.生成装箱明细子档
        Dim somlist As New List(Of SamplePaceInfo)
        somlist = saCon.SamplePaceBarCode_Getlist(Nothing, Nothing, SE_ID)
        If somlist.Count = 0 Then
            Exit Sub
        Else
            Dim i As Integer
            For i = 0 To somlist.Count - 1
                Dim spInfo As New SamplePackingInfo
                spInfo.PK_ID = strPK_ID
                If somlist(i).ClientBarcode <> String.Empty Then
                    spInfo.Code_ID = somlist(i).ClientBarcode
                Else
                    spInfo.Code_ID = somlist(i).Code_ID
                End If
                spInfo.PB_ID = ""
                spInfo.Qty = somlist(i).Qty
                spInfo.Remark = somlist(i).SE_Remark
                If spcon.SamplePackingSub_Add(spInfo) = False Then
                    MsgBox("添加失敗，请檢查原因！")
                    Exit Sub
                End If
            Next
        End If

        '3.直接审核
        Dim pkinfo As New SamplePackingInfo
        pkinfo.PK_ID = strPK_ID
        pkinfo.CheckBit = True
        pkinfo.CheckDate = Format(Now, "yyyy/MM/dd").ToString
        pkinfo.CheckUserID = InUserID
        pkinfo.CheckRemark = String.Empty
        If spcon.SamplePacking_UpdateCheck(pkinfo) = False Then
            MsgBox("审核失敗，请檢查原因！")
            Exit Sub
        End If

        '4.收發單號填入裝箱單號
        Dim spkInfo As New SamplePaceInfo
        spkInfo.AutoID = AutoID
        spkInfo.PK_Code_ID = Me.txtCode_ID.Text
        spkInfo.SealCode_ID = Me.txtSealCode_ID.Text
        If saCon.SamplePacePk_Update(spkInfo) = False Then
            MsgBox("修改裝箱單號失敗，请檢查原因！")
            Exit Sub
        End If

        MessageBox.Show("生成裝箱單號：" + strPK_ID)
        Me.Close()
    End Sub
#End Region

#Region "自动流水号"
    Function GetPK_ID() As String
        Dim oc As New SamplePackingController
        Dim oi As New SamplePackingInfo
        Dim ndate As String = "PK" + Format(Now(), "yyMMdd")
        oi = oc.SamplePacking_Get(ndate)
        If oi Is Nothing Then
            GetPK_ID = "PK" + Format(Now, "yyMMdd") + "0001"
        Else
            GetPK_ID = "PK" + Format(Now, "yyMMdd") + Mid(CInt(Mid(oi.PK_ID, 9)) + 100000000001, 9)
        End If
    End Function
#End Region

#Region "控件事件"
    Private Sub txtCode_ID_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCode_ID.KeyDown
        If e.KeyCode = Keys.Enter Then
            '1.不能为空
            If Me.txtCode_ID.Text = String.Empty Then
                MessageBox.Show("裝箱條碼不能為空!", "提示")
                txtCode_ID.Focus()
            End If
            '2.不能相同
            Dim splist As New List(Of SamplePackingInfo)
            splist = spcon.SamplePacking_GetList(Nothing, Nothing, Nothing, Me.txtCode_ID.Text, Nothing, Nothing, Nothing, Nothing)
            If splist.Count > 0 Then
                MessageBox.Show("裝箱條碼已經存在!", "提示")
                txtCode_ID.Text = String.Empty
                txtCode_ID.Focus()
            End If
            Me.txtSealCode_ID.Focus()
        End If
    End Sub

    'Private Sub txtCode_ID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCode_ID.Leave
    '    If Me.txtCode_ID.Text = String.Empty Then
    '        MessageBox.Show("裝箱條碼不能為空!", "提示")
    '        txtCode_ID.Focus()
    '    End If
    '    '2.不能相同
    '    Dim splist As New List(Of SamplePackingInfo)
    '    splist = spcon.SamplePacking_GetList(Nothing, Nothing, Nothing, Me.txtCode_ID.Text, Nothing, Nothing, Nothing, Nothing)
    '    If splist.Count > 0 Then
    '        MessageBox.Show("裝箱條碼已經存在!", "提示")
    '        txtCode_ID.Text = String.Empty
    '        txtCode_ID.Focus()
    '    End If
    '    Me.txtSealCode_ID.Focus()
    'End Sub

    Private Sub txtSealCode_ID_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSealCode_ID.KeyDown
        If e.KeyCode = Keys.Enter Then
            If Me.txtSealCode_ID.Text = String.Empty Then
                MessageBox.Show("封箱條碼不能為空!", "提示")
                txtSealCode_ID.Focus()
            End If
        End If
    End Sub

    'Private Sub txtSealCode_ID_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSealCode_ID.Leave
    '    If Me.txtSealCode_ID.Text = String.Empty Then
    '        MessageBox.Show("封箱條碼不能為空!", "提示")
    '        txtSealCode_ID.Focus()
    '    End If
    'End Sub
#End Region

#Region "数据不能为空"
    Function DataCheck() As Boolean
        DataCheck = False
        If Me.txtCode_ID.Text = String.Empty Then
            MessageBox.Show("裝箱條碼不能為空!", "提示")
            txtCode_ID.Focus()
            DataCheck = True
            Exit Function
        End If

        If Me.txtSealCode_ID.Text = String.Empty Then
            MessageBox.Show("封箱條碼不能為空!", "提示")
            txtSealCode_ID.Focus()
            DataCheck = True
            Exit Function
        End If

        Dim splist As New List(Of SamplePackingInfo)
        splist = spcon.SamplePacking_GetList(Nothing, Nothing, Nothing, Me.txtCode_ID.Text, Nothing, Nothing, Nothing, Nothing)
        If splist.Count > 0 Then
            MessageBox.Show("裝箱條碼已經存在!", "提示")
            txtCode_ID.Text = String.Empty
            txtCode_ID.Focus()
            DataCheck = True
            Exit Function
        End If
    End Function
#End Region
End Class