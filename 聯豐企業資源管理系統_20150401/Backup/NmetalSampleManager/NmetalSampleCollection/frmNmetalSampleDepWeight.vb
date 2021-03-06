Imports LFERP.Library.NmetalSampleManager.NmetalSampleCollection


Public Class frmNmetalSampleDepWeight

    Dim pncon As New NmetalSampleDepWeightControler

    Private Sub frmNmetalSampleDepWeight_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fc As New LFERP.Library.ProductionController.ProductionFieldControl
        GridControl2.DataSource = fc.ProductionFieldControl_GetList(InUserID, Nothing)
    End Sub

    Private Sub GridView2_FocusedRowChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView2.FocusedRowChanged

        If GridView2.SelectedRowsCount = 0 Then
            Exit Sub
        End If
        Dim strA As String
        strA = GridView2.GetFocusedRowCellValue("ControlDep").ToString
        Grid.DataSource = pncon.NmetalSampleDepWeight_GetList(strA, Nothing)

    End Sub

    ''' <summary>
    ''' 日期格式的转换
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridView1_CustomColumnDisplayText(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs) Handles GridView1.CustomColumnDisplayText
        If e.Column.Name = "gclAddDate" Or e.Column.Name = "gclModifyDate" Then
            If e.Value = Nothing Then
                e.DisplayText = ""
            End If
        End If
    End Sub
End Class