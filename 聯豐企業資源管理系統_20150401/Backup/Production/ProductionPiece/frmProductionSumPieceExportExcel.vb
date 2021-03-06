Imports LFERP.DataSetting
Imports System.Data.SqlClient
Imports LFERPDB


Public Class frmProductionSumPieceExportExcel
    Dim ds As New DataSet


    '@Per_Class        nvarchar(10)='B',
    '       @Per_Resign       bit=1

    Private Function SqlProc2(ByVal DepID As String, ByVal DateStart As String, ByVal DateEnd As String, ByVal Type As String, ByVal Per_Class As String, ByVal Per_Resign As String) As DataSet
        Dim strCon As String = ""
        Dim DD As New LFERPDB.LFERPDataBase
        strCon = DD.LoadConnStr
        ''-------------------------------------------------------
        Dim myConn As New SqlConnection(strCon)
        myConn.Open()
        '定义命令对象，并使用储存过程 
        Dim myCommand As New SqlClient.SqlCommand
        myCommand.CommandType = CommandType.StoredProcedure
        myCommand.CommandText = "ProductionSumPieceExportExcel1"
        myCommand.Connection = myConn
        '定义一个数据适配器，并设置参数 
        Dim myDapter As New SqlClient.SqlDataAdapter(myCommand)

        myDapter.SelectCommand.Parameters.Add("@DateStart", SqlDbType.VarChar, 50).Value = DateStart
        myDapter.SelectCommand.Parameters.Add("@DateEnd", SqlDbType.VarChar, 50).Value = DateEnd
        myDapter.SelectCommand.Parameters.Add("@Type", SqlDbType.VarChar, 50).Value = Type

        If DepID <> "" Then
            myDapter.SelectCommand.Parameters.Add("@DepID", SqlDbType.VarChar, 50).Value = DepID
        End If

        '2013-6-5---------------------------------------------------------------------------------------
        If Per_Class <> Nothing Then
            myDapter.SelectCommand.Parameters.Add("@Per_Class", SqlDbType.VarChar, 50).Value = Per_Class
        End If

        If Per_Resign <> Nothing Then
            myDapter.SelectCommand.Parameters.Add("@Per_Resign", SqlDbType.Bit, 50).Value = Per_Resign
        End If

        '定义一个数据集对象，并填充数据集 
        Dim myDataSet As New DataSet
        Try
            myDapter.Fill(myDataSet)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return myDataSet

    End Function


    Public Function DtToXls(ByVal Table As DataTable, ByVal DefFileName As String, ByVal OfficeType As String)
        Dim MyOleDbCn As New System.Data.OleDb.OleDbConnection
        Dim MyOleDbCmd As New System.Data.OleDb.OleDbCommand
        Dim MyTable As New DataTable
        Dim intRowsCnt, intColsCnt As Integer
        Dim strSql As String, strFlName As String
        Dim Fso As New System.Object
        If Table Is Nothing Then
            MessageBox.Show("未取得數據，無法導出 ", "導出錯誤 ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Function
        End If
        MyTable = Table
        If MyTable.Rows.Count = 0 Then
            MessageBox.Show("未取得數據，無法導出 ", "導出錯誤 ", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Function
        End If
        Dim FileName As String
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Title = "保存為 "
        ' SaveFileDialog.Filter = "txt files (*.xls))|*.xls"

        If OfficeType = "2007" Then
            ' SaveFileDialog.Filter = "(*.xls)|*.xls|(*.xlsx)|*.xlsx"
            SaveFileDialog.Filter = "(*.xls)|*.xls"

        Else
            SaveFileDialog.Filter = "(*.xlsx)|*.xlsx"


        End If

        SaveFileDialog.FileName = DefFileName
        If (SaveFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK) Then
            FileName = SaveFileDialog.FileName
        Else
            Exit Function
            '   TODO:   在此加入開啟檔案的程式碼。 
        End If
        If FileName = " " Then Exit Function
        strFlName = FileName
        If Dir(FileName, FileAttribute.Directory) <> "" Then
            Kill(FileName)
        End If
        Try
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            'MyOleDbCn.ConnectionString   =   "Provider=Microsoft.Jet.OleDb.4.0; "   &   _ 
            '"Data   Source= "   &   strFlName   &   "; "   &   _ 
            '"Extended   ProPerties= " "Excel   8.0;HDR=Yes; " " " 

            If OfficeType = "2007" Then
                MyOleDbCn.ConnectionString = "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + strFlName + ";Extended Properties='Excel 8.0; HDR=Yes; '"    ''''; //此连接只能操作Excel2007之前(.xls)文件
            ElseIf OfficeType = "2010" Then
                MyOleDbCn.ConnectionString = "Provider=Microsoft.Ace.OleDb.12.0;" & "data source=" + strFlName & ";Extended Properties='Excel 12.0; HDR=Yes;'"
            End If

            MyOleDbCn.Open()
            MyOleDbCmd.Connection = MyOleDbCn
            MyOleDbCmd.CommandType = CommandType.Text




            '第一行插入列标题 
            strSql = "CREATE   TABLE   " & DefFileName & "( "
            For intColsCnt = 0 To MyTable.Columns.Count - 1
                If intColsCnt <> MyTable.Columns.Count - 1 Then
                    If ChangeChar(MyTable.Columns(intColsCnt).Caption) = "數量" Then
                        strSql = strSql & ChangeChar(MyTable.Columns(intColsCnt).Caption) & "   double, "
                    Else
                        strSql = strSql & ChangeChar(MyTable.Columns(intColsCnt).Caption) & "   text, "
                    End If


                Else
                    strSql = strSql & ChangeChar(MyTable.Columns(intColsCnt).Caption) & "   text) "
                End If
            Next
            MyOleDbCmd.CommandText = strSql
            MyOleDbCmd.ExecuteNonQuery()




            '插入各行  最后一行不能為廠証編號
            For intRowsCnt = 0 To MyTable.Rows.Count - 1
                strSql = "INSERT   INTO   " & DefFileName & "   VALUES(  "
                For intColsCnt = 0 To MyTable.Columns.Count - 1

                    'If intColsCnt <> MyTable.Columns.Count - 1 Then
                    '    strSql = strSql & ChangeChar(MyTable.Rows(intRowsCnt).Item(intColsCnt)) & " ', ' "
                    'Else
                    '    strSql = strSql & ChangeChar(MyTable.Rows(intRowsCnt).Item(intColsCnt)) & " ') "
                    'End If
                    If ChangeChar(MyTable.Columns(intColsCnt).Caption) = "數量" Then
                        strSql = strSql & " " & ChangeChar(MyTable.Rows(intRowsCnt).Item(intColsCnt)) & " , "
                    Else
                        If intColsCnt <> MyTable.Columns.Count - 1 Then
                            strSql = strSql & " ' " & ChangeChar(MyTable.Rows(intRowsCnt).Item(intColsCnt)) & "',"
                        Else
                            strSql = strSql & " ' " & ChangeChar(MyTable.Rows(intRowsCnt).Item(intColsCnt)) & "')"
                        End If
                    End If
                Next



                MyOleDbCmd.CommandText = strSql
                MyOleDbCmd.ExecuteNonQuery()
            Next
            MessageBox.Show("数据已经成功导入EXCEL文件 " & strFlName, "数据导出 ", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ErrCode As Exception
            MsgBox(strSql)
            MsgBox("错误信息： " & ErrCode.Message & vbCrLf & vbCrLf & _
            "引发事件： " & ErrCode.TargetSite.ToString, MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "错误来源： " & ErrCode.Source)
            Exit Function
        Finally
            MyOleDbCmd.Dispose()
            MyOleDbCn.Close()
            MyOleDbCn.Dispose()
            'Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try

    End Function

    Public Function ChangeChar(ByVal Sqlchar) As String

        If Convert.IsDBNull(Sqlchar) Then
            ChangeChar = "   "
            Exit Function
        End If
        Dim tStr As String
        tStr = Replace(Sqlchar, " ' ", Chr(39) + Chr(39))
        tStr = Replace(tStr, "| ", "_ ")
        ChangeChar = tStr
    End Function

    Private Sub frmProductionSumPieceExportExcel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cobType.EditValue = tempValue
        tempValue = Nothing

        CreateTable()
        Load_Fac()

        Me.End_Date.EditValue = Format(Now, "yyyy/MM/dd")
        Me.Start_Date.EditValue = Format(Now, "yyyy/MM/dd")

        lueFacID.EditValue = "*"

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub



    Sub Load_Fac()
        ' 加載廠別名稱()
        Dim fc As New FacControler
        Dim fcl As New List(Of FacInfo)
        Dim i As Integer

        fcl = fc.GetFacList(strInFacID, Nothing)

        If fcl.Count <= 0 Then Exit Sub

        ds.Tables("TFac").Clear()

        If strInFacID = Nothing Then
            Dim row As DataRow
            row = ds.Tables("TFac").NewRow
            row("FacName") = "全部"
            row("FacID") = "*"
            ds.Tables("TFac").Rows.Add(row)
        End If

        For i = 0 To fcl.Count - 1
            Dim row1 As DataRow
            row1 = ds.Tables("TFac").NewRow
            row1("FacName") = fcl(i).FacName
            row1("FacID") = fcl(i).FacID
            ds.Tables("TFac").Rows.Add(row1)
        Next
    End Sub

    Sub Load_Dep()
        ' 加載廠別名稱()
        Dim dc As New DepartmentControler
        Dim dil As New List(Of DepartmentInfo)
        Dim i As Integer

        If lueFacID.EditValue = "*" Then
            dil = dc.BriName_GetList(strInDepID, Nothing, Nothing)
        Else
            dil = dc.BriName_GetList(strInDepID, Nothing, lueFacID.EditValue)
        End If
        If dil.Count <= 0 Then Exit Sub

        ds.Tables("TDep").Clear()

        If strInDepID = Nothing Then
            Dim row As DataRow
            row = ds.Tables("TDep").NewRow
            row("DepName") = "全部"
            row("DepID") = "*"
            ds.Tables("TDep").Rows.Add(row)
        End If

        For i = 0 To dil.Count - 1
            Dim row1 As DataRow
            row1 = ds.Tables("TDep").NewRow
            row1("DepName") = dil(i).DepName
            row1("DepID") = dil(i).DepID
            ds.Tables("TDep").Rows.Add(row1)
        Next

    End Sub

    Sub CreateTable()
        ds.Tables.Clear()

        With ds.Tables.Add("TFac")
            .Columns.Add("FacName", GetType(String))
            .Columns.Add("FacID", GetType(String))
        End With

        lueFacID.Properties.DisplayMember = "FacName"
        lueFacID.Properties.ValueMember = "FacID"
        lueFacID.Properties.DataSource = ds.Tables("TFac")

        With ds.Tables.Add("TDep")
            .Columns.Add("DepName", GetType(String))
            .Columns.Add("DepID", GetType(String))
        End With

        lueDepID.Properties.DisplayMember = "DepName"
        lueDepID.Properties.ValueMember = "DepID"
        lueDepID.Properties.DataSource = ds.Tables("TDep")

    End Sub

    Private Sub lueFacID_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lueFacID.EditValueChanged
        If lueFacID.EditValue Is Nothing Then Exit Sub
        Load_Dep()

    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim _lueDepID As String = ""
        If lueDepID.EditValue Is Nothing Then
        ElseIf lueDepID.EditValue <> "*" Then
            _lueDepID = lueDepID.EditValue
        End If

        Dim _Per_Class As String = Nothing
        Dim _Per_Resign As String = Nothing



        If ComboBoxReg.Text = "" Or ComboBoxReg.Text = "全部" Then
            _Per_Resign = Nothing
        ElseIf ComboBoxReg.Text = "辭工" Then
            _Per_Resign = "True"
        ElseIf ComboBoxReg.Text = "在職" Then
            _Per_Resign = "False"
        End If

        If cboPer_Class.Text = "" Or cboPer_Class.Text = "全部" Then
            _Per_Class = Nothing
        Else
            _Per_Class = cboPer_Class.Text
        End If



        On Error Resume Next

        DtToXls(SqlProc2(_lueDepID, Format(CDate(Start_Date.EditValue), "yyyy/MM/dd"), Format(CDate(Me.End_Date.EditValue), "yyyy/MM/dd"), Me.cobType.EditValue, _Per_Class, _Per_Resign).Tables(0), cobType.EditValue, "2007")



    End Sub

    Private Sub cobType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cobType.SelectedIndexChanged
        If cobType.Text = "個人計件" Then
            LabReg.Enabled = True
            ComboBoxReg.Enabled = True
            LabPerClass.Enabled = True
            cboPer_Class.Enabled = True
        ElseIf cobType.Text = "個人計時" Then
            LabReg.Enabled = True
            ComboBoxReg.Enabled = True
            LabPerClass.Enabled = False
            cboPer_Class.Enabled = False
        Else
            LabReg.Enabled = False
            ComboBoxReg.Enabled = False
            LabPerClass.Enabled = False
            cboPer_Class.Enabled = False
        End If
    End Sub
End Class