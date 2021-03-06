Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Namespace LFERP.Library.SampleManager.SampleBorrow
    Public Class SampleRepayController
        Friend Function FillSampleRepay(ByVal reader As IDataReader) As SampleRepayInfo
            On Error Resume Next
            Dim objInfo As New SampleRepayInfo
            objInfo.RepayID = reader("RepayID").ToString
            objInfo.BorrowID = reader("BorrowID").ToString
            objInfo.D_ID = reader("D_ID").ToString
            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.RepayQty = reader("RepayQty").ToString
            objInfo.Borrow_Qty = reader("Borrow_Qty").ToString
            objInfo.NoBorrow_Qty = reader("NoBorrow_Qty").ToString
            objInfo.RepayDate = IIf(IsDBNull(reader("RepayDate")), Nothing, Format(CDate(reader("RepayDate")), "yyyy/MM/dd HH:mm:ss"))
            objInfo.InCardID = reader("InCardID").ToString
            If reader("CheckBit") Is DBNull.Value Then
                objInfo.CheckBit = Nothing
            Else
                objInfo.CheckBit = reader("CheckBit")
            End If
            If reader("CheckDate") Is DBNull.Value Then
                objInfo.CheckDate = Nothing
            Else
                objInfo.CheckDate = Format(CDate(reader("CheckDate")), "yyyy/MM/dd")
            End If
            objInfo.CheckAction = reader("CheckAction").ToString
            objInfo.CreateUserID = reader("CreateUserID").ToString
            If reader("CreateDate") Is DBNull.Value Then
                objInfo.CreateDate = Nothing
            Else
                objInfo.CreateDate = Format(CDate(reader("CreateDate")), "yyyy/MM/dd")
            End If
            objInfo.ModifyUserID = reader("ModifyUserID").ToString
            If reader("ModifyDate") Is DBNull.Value Then
                objInfo.ModifyDate = Nothing
            Else
                objInfo.ModifyDate = Format(CDate(reader("ModifyDate")), "yyyy/MM/dd")
            End If
            objInfo.Remarks = reader("Remarks").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.D_Dep = reader("D_Dep").ToString
            objInfo.CheckUserName = reader("CheckUserName").ToString
            objInfo.CreateUserName = reader("CreateUserName").ToString
            objInfo.MaterialTypeID = reader("MaterialTypeID").ToString
            objInfo.MaterialTypeName = reader("MaterialTypeName").ToString
            objInfo.SO_SampleID = reader("SO_SampleID").ToString
            objInfo.Qty = reader("Qty")
            objInfo.Code_ID = reader("Code_ID").ToString

            objInfo.InputID = reader("InputID").ToString
            objInfo.InputType = reader("InputType").ToString
            objInfo.UseBit = reader("UseBit")
            Return objInfo
        End Function

        Public Function SampleRepay_GetList(ByVal RepayID As String, ByVal D_ID As String, ByVal PS_NO As String, ByVal InCardID As String, ByVal PM_M_Code As String, ByVal Where As String, ByVal ReportEmpty As Boolean) As List(Of SampleRepayInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_GetList")
            db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@InCardID", DbType.String, InCardID)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)

            db.AddInParameter(dbComm, "@Where", DbType.String, Where)

            Dim FeatureList As New List(Of SampleRepayInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleRepay(reader))
                End While
                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New SampleRepayInfo())
                End If
                Return FeatureList
            End Using
        End Function

        Public Function SampleRepayView_GetList(ByVal RepayID As String, ByVal D_ID As String, ByVal PS_NO As String, ByVal InCardID As String, ByVal PM_M_Code As String, ByVal Where As String, ByVal ReportEmpty As Boolean) As List(Of SampleRepayInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepayView_GetList")
            db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@InCardID", DbType.String, InCardID)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)

            db.AddInParameter(dbComm, "@Where", DbType.String, Where)

            Dim FeatureList As New List(Of SampleRepayInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleRepay(reader))
                End While
                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New SampleRepayInfo())
                End If
                Return FeatureList
            End Using
        End Function

        Public Function SampleRepay_Delete(ByVal RepayID As String, ByVal InputID As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_Delete")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)
                db.AddInParameter(dbComm, "@InputID", DbType.String, InputID)
                db.ExecuteNonQuery(dbComm)
                SampleRepay_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepay_Delete = False
            End Try
        End Function

        Public Function SampleRepay_Update(ByVal objinfo As SampleRepayInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_Update")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, objinfo.RepayID)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objinfo.Code_ID)
                db.AddInParameter(dbComm, "@Qty", DbType.Int32, objinfo.Qty)
                db.AddInParameter(dbComm, "@RepayDate", DbType.DateTime, objinfo.RepayDate)
                db.AddInParameter(dbComm, "@InCardID", DbType.String, objinfo.InCardID)
                db.AddInParameter(dbComm, "@Remarks", DbType.String, objinfo.Remarks)
                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.Date, objinfo.ModifyDate)

                db.ExecuteNonQuery(dbComm)
                SampleRepay_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepay_Update = False
            End Try
        End Function

        Public Function SampleRepaySet_Update(ByVal RepayID As String, ByVal InputID As String, ByVal BorrowBit As Boolean) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepaySet_Update")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)
                db.AddInParameter(dbComm, "@InputID", DbType.String, InputID)
                db.AddInParameter(dbComm, "@BorrowBit", DbType.Boolean, BorrowBit)

                db.ExecuteNonQuery(dbComm)
                SampleRepaySet_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepaySet_Update = False
            End Try
        End Function

        Public Function SampleRepayGet_Update(ByVal RepayID As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepayGet_Update")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)

                db.ExecuteNonQuery(dbComm)
                SampleRepayGet_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepayGet_Update = False
            End Try
        End Function

        Public Function SampleRepay_Add(ByVal objInfo As SampleRepayInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_Add")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, objInfo.RepayID)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objInfo.D_ID)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objInfo.PS_NO)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objInfo.SO_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objInfo.PM_M_Code)
                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objInfo.Code_ID)
                db.AddInParameter(dbComm, "@Qty", DbType.Int32, objInfo.Qty)
                db.AddInParameter(dbComm, "@RepayDate", DbType.Date, objInfo.RepayDate)
                db.AddInParameter(dbComm, "@InCardID", DbType.String, objInfo.InCardID)
                db.AddInParameter(dbComm, "@Remarks", DbType.String, objInfo.Remarks)

                db.AddInParameter(dbComm, "@InputType", DbType.String, objInfo.InputType)
                db.AddInParameter(dbComm, "@InputID", DbType.String, objInfo.InputID)

                db.AddInParameter(dbComm, "@CreateUserID", DbType.String, objInfo.CreateUserID)
                db.AddInParameter(dbComm, "@CreateDate", DbType.Date, objInfo.CreateDate)

                db.ExecuteNonQuery(dbComm)
                SampleRepay_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepay_Add = False
            End Try
        End Function

        Public Function SampleRepay_GetID() As String
            Try
                Dim ndate = "RI" + Format(Now(), "yyMM")
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_GetID")
                Using reader As IDataReader = db.ExecuteReader(dbComm)
                    If reader.Read Then
                        Return ndate + Mid((CInt(Mid(reader("RepayID").ToString, 7)) + 100001), 2)
                    Else
                        Return ndate + "00001"
                    End If
                End Using
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical, "中間層:SampleRepay_GetID方法出錯")
                Return Nothing
            End Try
        End Function

        Public Function SampleRepay_UpdateCheck(ByVal objinfo As SampleRepayInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepay_UpdateCheck")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, objinfo.RepayID)
                db.AddInParameter(dbComm, "@CheckUserID", DbType.String, objinfo.CheckAction)
                db.AddInParameter(dbComm, "@CheckBit", DbType.Boolean, objinfo.CheckBit)
                db.ExecuteNonQuery(dbComm)
                SampleRepay_UpdateCheck = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepay_UpdateCheck = False
            End Try
        End Function

        Public Function GetDeptInfo() As DataTable
            Dim ds As New DataSet
            Dim sqlStr As String = "select DPT_ID as DeptID,DPT_Name as DeptName from Department  "
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetSqlStringCommand(sqlStr)
            ds = db.ExecuteDataSet(dbcomm)
            If ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            Else
                Return Nothing
            End If
        End Function

        Public Function GetMaterialInfo() As DataTable
            Dim ds As New DataSet
            Dim sqlStr As String = "select M_Code,M_Name,M_Gauge,M_Unit  from MaterialCode  "
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetSqlStringCommand(sqlStr)
            ds = db.ExecuteDataSet(dbcomm)
            If ds.Tables.Count > 0 Then
                Return ds.Tables(0)
            Else
                Return Nothing
            End If
        End Function

#Region "子表数据"
        Public Function SampleRepaySub_Add(ByVal objInfo As SampleRepayInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepaySub_Add")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, objInfo.RepayID)
                db.AddInParameter(dbComm, "@BorrowID", DbType.String, objInfo.BorrowID)
                db.AddInParameter(dbComm, "@Borrow_Qty", DbType.Int64, objInfo.Borrow_Qty)
                db.AddInParameter(dbComm, "@NoBorrow_Qty", DbType.Int64, objInfo.NoBorrow_Qty)
                db.AddInParameter(dbComm, "@RepayQty", DbType.Int64, objInfo.RepayQty)
                db.AddInParameter(dbComm, "@CreateUserID", DbType.String, objInfo.CreateUserID)
                db.ExecuteNonQuery(dbComm)
                SampleRepaySub_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepaySub_Add = False
            End Try
        End Function

        Public Function SampleRepaySub_Update(ByVal objInfo As SampleRepayInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepaySub_Update")
                db.AddInParameter(dbComm, "@RepayID", DbType.String, objInfo.RepayID)
                db.AddInParameter(dbComm, "@BorrowID", DbType.String, objInfo.BorrowID)
                db.AddInParameter(dbComm, "@Borrow_Qty", DbType.Int64, objInfo.Borrow_Qty)
                db.AddInParameter(dbComm, "@NoBorrow_Qty", DbType.Int64, objInfo.NoBorrow_Qty)
                db.AddInParameter(dbComm, "@RepayQty", DbType.Int64, objInfo.RepayQty)
                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objInfo.ModifyUserID)
                db.ExecuteNonQuery(dbComm)
                SampleRepaySub_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleRepaySub_Update = False
            End Try
        End Function

        Public Function SampleRepaySub_GetList(ByVal RepayID As String) As List(Of SampleRepayInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleRepaySub_GetList")
            db.AddInParameter(dbComm, "@RepayID", DbType.String, RepayID)
            Dim FeatureList As New List(Of SampleRepayInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleRepay(reader))
                End While
                Return FeatureList
            End Using
        End Function

#End Region
    End Class
End Namespace