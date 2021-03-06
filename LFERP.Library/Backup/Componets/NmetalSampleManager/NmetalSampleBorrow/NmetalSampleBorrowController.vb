Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Namespace LFERP.Library.NmetalSampleManager.NmetalSampleBorrow

    Public Class NmetalSampleBorrowController
        Friend Function FillNmetalSampleBorrow(ByVal reader As IDataReader) As NmetalSampleBorrowInfo
            On Error Resume Next
            Dim objInfo As New NmetalSampleBorrowInfo
            objInfo.AutoID = reader("AutoID").ToString
            objInfo.BorrowID = reader("BorrowID").ToString
            objInfo.D_ID = reader("D_ID").ToString
            objInfo.BorrowDate = IIf(IsDBNull(reader("BorrowDate")), Nothing, Format(CDate(reader("BorrowDate")), "yyyy/MM/dd"))
            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            If IsDBNull(reader("Borrow_Qty")) = False Then
                objInfo.Borrow_Qty = reader("Borrow_Qty")
            End If
            If IsDBNull(reader("NoBorrow_Qty")) = False Then
                objInfo.NoBorrow_Qty = reader("NoBorrow_Qty")
            End If

            objInfo.OutCardID = reader("OutCardID").ToString
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

            objInfo.CheckUserID = reader("CheckUserID").ToString
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
            objInfo.CheckUserName = reader("CheckUserName").ToString
            objInfo.CreateUserName = reader("CreateUserName").ToString
            objInfo.D_Dep = reader("D_Dep").ToString
            objInfo.Remark = reader("Remark").ToString
            objInfo.MaterialTypeID = reader("MaterialTypeID").ToString
            objInfo.MaterialTypeName = reader("MaterialTypeName").ToString
            objInfo.SO_SampleID = reader("SO_SampleID").ToString
            objInfo.RepayQty = reader("RepayQty")
            objInfo.PS_Name = reader("PS_Name").ToString
            Return objInfo
        End Function

        Public Function NmetalSampleBorrowD_ID_GetList(ByVal D_ID As String) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrowD_ID_GetList")

            If D_ID <> Nothing Then
                db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            End If
            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function NmetalSampleBorrowM_Code_GetList(ByVal OutD_ID As String, ByVal InD_ID As String) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrowM_Code_GetList")

            If OutD_ID <> Nothing Then
                db.AddInParameter(dbComm, "@OutD_ID", DbType.String, OutD_ID)
            End If

            If InD_ID <> Nothing Then
                db.AddInParameter(dbComm, "@InD_ID", DbType.String, InD_ID)
            End If

            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function NmetalSampleBorrowPM_M_Code_GetList(ByVal PM_M_Code As String) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrowPM_M_Code_GetList")

            If PM_M_Code <> Nothing Then
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            End If

            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function NmetalSampleBorrow_GetList(ByVal BorrowID As String, ByVal D_ID As String, ByVal PS_NO As String, ByVal PM_M_Code As String, ByVal OutCardID As String, ByVal Where As String, ByVal ReportEmpty As Boolean) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_GetList")
            db.AddInParameter(dbComm, "@BorrowID", DbType.String, BorrowID)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@OutCardID", DbType.String, OutCardID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@Where", DbType.String, Where)

            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New NmetalSampleBorrowInfo())
                End If
                Return FeatureList
            End Using

        End Function

        Public Function NmetalSampleBorrowA_GetList(ByVal D_ID As String, ByVal PS_NO As String, ByVal PM_M_Code As String, ByVal OutCardID As String, ByVal Where As String, ByVal ReportEmpty As Boolean) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrowA_GetList")

            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@OutCardID", DbType.String, OutCardID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@Where", DbType.String, Where)


            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New NmetalSampleBorrowInfo())
                End If
                Return FeatureList
            End Using

        End Function

        Public Function NmetalSampleBorrow_Delete(ByVal AutoID As Decimal, ByVal BorrowID As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_Delete")
                If AutoID <> Nothing Then
                    db.AddInParameter(dbComm, "@AutoID", DbType.Decimal, AutoID)
                End If
                If BorrowID <> Nothing Then
                    db.AddInParameter(dbComm, "@BorrowID", DbType.String, BorrowID)
                End If
                db.ExecuteNonQuery(dbComm)
                NmetalSampleBorrow_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleBorrow_Delete = False
            End Try
        End Function


        Public Function NmetalSampleBorrow_Update(ByVal objinfo As NmetalSampleBorrowInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_Update")

                db.AddInParameter(dbComm, "@BorrowID", DbType.String, objinfo.BorrowID)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@MaterialTypeID", DbType.String, objinfo.MaterialTypeID)
                db.AddInParameter(dbComm, "@Borrow_Qty", DbType.Int64, objinfo.Borrow_Qty)
                db.AddInParameter(dbComm, "@NoBorrow_Qty", DbType.Int64, objinfo.NoBorrow_Qty)
                db.AddInParameter(dbComm, "@BorrowDate", DbType.Date, objinfo.BorrowDate)
                db.AddInParameter(dbComm, "@OutCardID", DbType.String, objinfo.OutCardID)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objinfo.Remark)
                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.Date, objinfo.ModifyDate)

                db.ExecuteNonQuery(dbComm)
                NmetalSampleBorrow_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleBorrow_Update = False
            End Try
        End Function
        Public Function NmetalSampleBorrow_UpdateCheck(ByVal objinfo As NmetalSampleBorrowInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_Update")
                If objinfo.AutoID <> Nothing Then
                    db.AddInParameter(dbComm, "@AutoID ", DbType.Decimal, objinfo.AutoID)
                End If
                If objinfo.BorrowID <> Nothing Then
                    db.AddInParameter(dbComm, "@BorrowID", DbType.String, objinfo.BorrowID)
                End If
                db.AddInParameter(dbComm, "@CheckUserID", DbType.String, objinfo.CheckUserID)
                If objinfo.ModifyDate <> Nothing Then
                    db.AddInParameter(dbComm, "@CheckBit", DbType.Boolean, objinfo.CheckBit)
                End If
                db.ExecuteNonQuery(dbComm)
                NmetalSampleBorrow_UpdateCheck = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleBorrow_UpdateCheck = False
            End Try
        End Function

        Public Function NmetalSampleBorrow_GetNewID() As String
            Try
                Dim ndate = "RB" + Format(Now(), "yyMM")
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_GetID")
                Using reader As IDataReader = db.ExecuteReader(dbComm)
                    If reader.Read Then
                        Return ndate + Mid((CInt(Mid(reader("BorrowID").ToString, 7)) + 100001), 2)
                    Else
                        Return ndate + "00001"
                    End If
                End Using
            Catch ex As Exception
                MsgBox(ex.Message)
                Return Nothing
            End Try
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


        Public Function NmetalSampleBorrow_Add(ByVal objInfo As NmetalSampleBorrowInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_Add")

                db.AddInParameter(dbComm, "@BorrowID", DbType.String, objInfo.BorrowID)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objInfo.SO_ID)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objInfo.D_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objInfo.PM_M_Code)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objInfo.PS_NO)
                db.AddInParameter(dbComm, "@MaterialTypeID", DbType.String, objInfo.MaterialTypeID)
                db.AddInParameter(dbComm, "@Borrow_Qty", DbType.Int64, objInfo.Borrow_Qty)
                db.AddInParameter(dbComm, "@NoBorrow_Qty", DbType.Int64, objInfo.NoBorrow_Qty)
                db.AddInParameter(dbComm, "@BorrowDate", DbType.Date, objInfo.BorrowDate)
                db.AddInParameter(dbComm, "@OutCardID", DbType.String, objInfo.OutCardID)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objInfo.Remark)
                db.AddInParameter(dbComm, "@CreateUserID", DbType.String, objInfo.CreateUserID)
                db.AddInParameter(dbComm, "@CreateDate", DbType.Date, objInfo.CreateDate)


                db.ExecuteNonQuery(dbComm)
                NmetalSampleBorrow_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleBorrow_Add = False
            End Try
        End Function

        Public Function NmetalSampleBorrow_UpdateNoBorrowQty(ByVal BorrowID As String, ByVal NoBorrow_Qty As Integer) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleBorrow_UpdateNoBorrowQty")
                db.AddInParameter(dbComm, "@BorrowID", DbType.String, BorrowID)
                If NoBorrow_Qty <> Nothing Then
                    db.AddInParameter(dbComm, "@NoBorrow_Qty", DbType.Int64, NoBorrow_Qty)
                End If
                db.ExecuteNonQuery(dbComm)
                NmetalSampleBorrow_UpdateNoBorrowQty = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleBorrow_UpdateNoBorrowQty = False
            End Try
        End Function

        Public Function NmetalSampleThrough_GetList(ByVal AutoID As Decimal, ByVal BorrowID As String, ByVal OutD_ID As String, ByVal InD_ID As String, ByVal PM_M_Code As String, ByVal OutCardID As String, ByVal CheckBit As String, ByVal OutDate As Date, ByVal Indate As Date) As List(Of NmetalSampleBorrowInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleThrough_GetList")

            If AutoID <> Nothing Then
                db.AddInParameter(dbComm, "@AutoID", DbType.Decimal, AutoID)
            End If
            If BorrowID <> Nothing Then
                db.AddInParameter(dbComm, "@BorrowID", DbType.String, BorrowID)
            End If
            If OutD_ID <> Nothing Then
                db.AddInParameter(dbComm, "@OutD_ID", DbType.String, OutD_ID)
            End If
            If InD_ID <> Nothing Then
                db.AddInParameter(dbComm, "@InD_ID", DbType.String, InD_ID)
            End If
            If PM_M_Code <> Nothing Then
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            End If
            If OutCardID <> Nothing Then
                db.AddInParameter(dbComm, "@OutCardID", DbType.String, OutCardID)
            End If
            If CheckBit <> Nothing Then
                db.AddInParameter(dbComm, "@CheckBit", DbType.Boolean, CheckBit)
            End If
            If OutDate <> Nothing Then
                db.AddInParameter(dbComm, "@OutDate", DbType.Date, OutDate)
            End If

            Dim FeatureList As New List(Of NmetalSampleBorrowInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleBorrow(reader))
                End While
                Return FeatureList
            End Using
        End Function
    End Class
End Namespace