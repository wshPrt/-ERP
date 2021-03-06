Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersMain
Imports LFERP.Library.NmetalSampleManager.NmetalSampleOrdersSub

Public Class frmNmetalSampleUpdateManual_Number
    Dim socon As New NmetalSampleOrdersMainControler
    Dim nsosc As New NmetalSampleOrdersSubControler
    Private _SO_ID As String
    Private _SS_Edition As String

    Property SO_ID() As String '属性
        Get
            Return _SO_ID
        End Get
        Set(ByVal value As String)
            _SO_ID = value
        End Set
    End Property
    Property SS_Edition() As String '属性
        Get
            Return _SS_Edition
        End Get
        Set(ByVal value As String)
            _SS_Edition = value
        End Set
    End Property
    Private Sub frmNmetalSampleUpdateManual_Number_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim solist As New List(Of NmetalSampleOrdersMainInfo)
        Dim nsos As New List(Of NmetalSampleOrdersSubInfo)
        solist = socon.NmetalSampleOrdersMain_GetList(SO_ID, Nothing, Nothing, Nothing, Nothing, Nothing, True)
        nsos = nsosc.NmetalSampleOrdersSub_GetList(SO_ID, Nothing)
        If solist.Count > 0 Then
            txtSO_ID.Text = SO_ID
            txtSS_Edition.Text = nsos(0).SS_Edition
            txtPM_M_Code.Text = solist(0).PM_M_Code
            txt_OldManual_Number.Text = solist(0).Manual_Number
        End If
    End Sub

    Private Sub btn_save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_save.Click
        If txtSO_ID.Text = String.Empty Then
            MsgBox("订单编号不能为空,请输入！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        If txtSS_Edition.Text = String.Empty Then
            MsgBox("版本号不能为空,请输入！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        If txtPM_M_Code.Text = String.Empty Then
            MsgBox("产品编号不能为空,请输入！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        If txt_OldManual_Number.Text = String.Empty Then
            MsgBox("旧手册编号不能为空,请输入！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        If txt_NewManual_Number.Text = String.Empty Then
            MsgBox("新手册编号不能为空,请输入！", MsgBoxStyle.Information, "提示")
            txt_NewManual_Number.Focus()
            Exit Sub
        End If
        Dim soinfo As New NmetalSampleOrdersMainInfo
        soinfo.SO_ID = txtSO_ID.Text
        soinfo.Manual_Number = txt_NewManual_Number.Text

        If socon.NmetalSampleOrdersMain_UpdateManual_Number(soinfo) = False Then
            MsgBox("手册编号更改失败！", MsgBoxStyle.Information, "提示")
            Exit Sub
        End If
        MsgBox("手册编号更改成功！", MsgBoxStyle.Information, "提示")
        Me.Close()
    End Sub
    ''' <summary>
    ''' 取消按钮事件
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btn_exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_exit.Click
        Me.Close()
    End Sub
End Class