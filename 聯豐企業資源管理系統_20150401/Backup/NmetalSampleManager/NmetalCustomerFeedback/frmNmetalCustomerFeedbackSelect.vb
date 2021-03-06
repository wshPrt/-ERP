Imports LFERP.DataSetting
Imports LFERP.Library.NmetalSampleManager.NmetalSampleCustomerFeedback

Public Class frmNmetalCustomerFeedbackSelect
#Region "属性"
    Dim ds As New DataSet
    Dim cc As New NmetalSampleCustomerFeedbackControler
    Public SampleList As New List(Of NmetalSampleCustomerFeedbackinfo)

    Private _EditItem As String
    Property EditItem() As String '属性
        Get
            Return _EditItem
        End Get
        Set(ByVal value As String)
            _EditItem = value
        End Set
    End Property
#End Region

#Region "载入窗体"
    Private Sub frmCustomerFeedbackSelect_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CreateTable()
        Loaddata(EditItem)
    End Sub
#End Region

#Region "创建临时表"
    Sub CreateTable()
        ds.Tables.Clear()
        With ds.Tables.Add("Sampletemp")
            .Columns.Add("YesNo", GetType(Boolean))
            .Columns.Add("SC_Edition", GetType(String))
            .Columns.Add("PM_M_Code", GetType(String))
            .Columns.Add("M_Code", GetType(String))
            .Columns.Add("M_Name", GetType(String))
            .Columns.Add("SC_SendNo", GetType(String))
            .Columns.Add("AutoID", GetType(Decimal))
            .Columns.Add("SO_ID", GetType(String))
        End With
        Grid1.DataSource = ds.Tables("Sampletemp")
    End Sub
#End Region

#Region "载入数据"
    Sub Loaddata(ByVal strCustomer As String)
        Dim cl As New List(Of NmetalSampleCustomerFeedbackinfo)
        cl = cc.NmetalSampleSend_GetList(strCustomer)
        If cl.Count <= 0 Then
            MsgBox("无記錄存在")
            Me.Close()
            Exit Sub
        End If

        Dim i As Integer
        ds.Tables("Sampletemp").Clear()

        For i = 0 To cl.Count - 1
            Dim row As DataRow
            row = ds.Tables("Sampletemp").NewRow
            row("YesNo") = False
            row("SC_SendNo") = cl(i).SC_SendNo
            row("SO_ID") = cl(i).SO_ID
            row("PM_M_Code") = cl(i).PM_M_Code
            row("M_Code") = cl(i).M_Code
            row("M_Name") = cl(i).M_Name
            row("SC_Edition") = cl(i).SC_Edition
            ds.Tables("Sampletemp").Rows.Add(row)
        Next
    End Sub
#End Region

#Region "按键事件"
    Private Sub SimpleButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SimpleButton1.Click
        Dim m As Integer
        Dim T As Boolean

        T = False
        For m = 0 To ds.Tables("Sampletemp").Rows.Count - 1
            If IIf(IsDBNull(ds.Tables("Sampletemp").Rows(m)("YesNo")), False, ds.Tables("Sampletemp").Rows(m)("YesNo")) Then
                T = True
            End If
        Next
        If T = False Then
            MsgBox("沒有选择资料无法保存,请选择！", MsgBoxStyle.Information, "溫馨提示")
            Exit Sub
        End If

        ''''''''''''''''''''''''''''''''''''''
        Dim i As Integer
        For i = 0 To ds.Tables("Sampletemp").Rows.Count - 1
            If ds.Tables("Sampletemp").Rows(i)("YesNo") Then
                With ds.Tables("Sampletemp")
                    Dim SamplePlanInfo As New NmetalSampleCustomerFeedbackinfo
                    SamplePlanInfo.SC_Edition = .Rows(i)("SC_Edition").ToString
                    SamplePlanInfo.PM_M_Code = .Rows(i)("PM_M_Code").ToString
                    SamplePlanInfo.M_Code = .Rows(i)("M_Code").ToString
                    SamplePlanInfo.M_Name = .Rows(i)("M_Name").ToString
                    SamplePlanInfo.SO_ID = .Rows(i)("SO_ID").ToString
                    SamplePlanInfo.SC_SendNo = .Rows(i)("SC_SendNo").ToString
                    SampleList.Add(SamplePlanInfo)
                End With
            End If
        Next
        Me.Close()
    End Sub
    Private Sub CheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Dim i As Int16
        For i = 0 To ds.Tables("Sampletemp").Rows.Count - 1
            If CheckBox1.Checked Then
                ds.Tables("Sampletemp").Rows(i)("YesNo") = True
            Else
                ds.Tables("Sampletemp").Rows(i)("YesNo") = False
            End If
        Next
    End Sub
#End Region
End Class