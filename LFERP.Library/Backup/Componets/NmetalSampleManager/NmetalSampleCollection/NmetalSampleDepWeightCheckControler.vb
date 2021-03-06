Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Namespace LFERP.Library.NmetalSampleManager.NmetalSampleCollection
    ''' <summary>
    ''' 部門重量表操作
    ''' Mark
    ''' 2014-07-01
    ''' </summary>
    ''' <remarks></remarks>
    Public Class NmetalSampleDepWeightCheckControler
        ''' <summary>
        ''' 新增
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_Add(ByVal objinfo As NmetalSampleDepWeightCheckInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_Add")

                db.AddInParameter(dbComm, "@ChangeNO", DbType.String, objinfo.ChangeNO)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@PM_Type", DbType.String, objinfo.PM_Type)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)

                db.AddInParameter(dbComm, "@DepWightOld", DbType.Decimal, objinfo.DepWightOld)
                db.AddInParameter(dbComm, "@DepWightNew", DbType.Decimal, objinfo.DepWightNew)
                db.AddInParameter(dbComm, "@AddAction", DbType.String, objinfo.AddAction)
                db.AddInParameter(dbComm, "@AddDate", DbType.Date, objinfo.AddDate)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objinfo.Remark)

                db.AddInParameter(dbComm, "@SubRemark", DbType.String, objinfo.SubRemark)
                db.AddInParameter(dbComm, "@ProductWightOld", DbType.Decimal, objinfo.ProductWightOld)

                db.ExecuteNonQuery(dbComm)
                NmetalSampleDepWeightCheck_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleDepWeightCheck_Add = False
            End Try
        End Function
        ''' <summary>
        ''' 更新
        ''' </summary>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_Update(ByVal objinfo As NmetalSampleDepWeightCheckInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_Update")
                db.AddInParameter(dbComm, "@AutoID", DbType.String, objinfo.AutoID)
                db.AddInParameter(dbComm, "@ChangeNO", DbType.String, objinfo.ChangeNO)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@PM_Type", DbType.String, objinfo.PM_Type)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)

                db.AddInParameter(dbComm, "@DepWightOld", DbType.Decimal, objinfo.DepWightOld)
                db.AddInParameter(dbComm, "@DepWightNew", DbType.Decimal, objinfo.DepWightNew)
                db.AddInParameter(dbComm, "@AddAction", DbType.String, objinfo.AddAction)
                db.AddInParameter(dbComm, "@AddDate", DbType.Date, objinfo.AddDate)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objinfo.Remark)

                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.Date, objinfo.ModifyDate)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@SubRemark", DbType.String, objinfo.SubRemark)
                db.AddInParameter(dbComm, "@ProductWightOld", DbType.String, objinfo.ProductWightOld)

                db.ExecuteNonQuery(dbComm)
                NmetalSampleDepWeightCheck_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleDepWeightCheck_Update = False
            End Try
        End Function
        ''' <summary>
        ''' 審核
        ''' </summary>
        ''' <param name="objinfo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_Check(ByVal objinfo As NmetalSampleDepWeightCheckInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_Check")
                db.AddInParameter(dbComm, "@ChangeNO", DbType.String, objinfo.ChangeNO)
                db.AddInParameter(dbComm, "@CheckStatus", DbType.Boolean, objinfo.CheckStatus)
                db.AddInParameter(dbComm, "@CheckAction", DbType.String, objinfo.CheckAction)
                db.AddInParameter(dbComm, "@CheckDate", DbType.Date, objinfo.CheckDate)
                db.AddInParameter(dbComm, "@CheckRemark", DbType.String, objinfo.CheckRemark)
                db.ExecuteNonQuery(dbComm)
                NmetalSampleDepWeightCheck_Check = True

            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleDepWeightCheck_Check = False
            End Try
        End Function
        ''' <summary>
        ''' 刪除
        ''' </summary>
        ''' <param name="ChangeNO"></param>
        ''' <param name="AutoID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_Delete(ByVal AutoID As String, ByVal ChangeNO As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_Delete")
                db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
                db.AddInParameter(dbComm, "@ChangeNO", DbType.String, ChangeNO)
                db.ExecuteNonQuery(dbComm)
                NmetalSampleDepWeightCheck_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleDepWeightCheck_Delete = False
            End Try
        End Function
        ''' <summary>
        '''獲取編號
        ''' </summary>
        ''' <param name="ChangeNO"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_GetID(ByVal ChangeNO As String) As NmetalSampleDepWeightCheckInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_GetID")

            db.AddInParameter(dbComm, "@ChangeNO", DbType.String, ChangeNO)

            Dim FeatureList As New NmetalSampleDepWeightCheckInfo
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.ChangeNO = reader("ChangeNO").ToString
                End While
                Return FeatureList
            End Using
        End Function
        ''' <summary>
        ''' 查詢
        ''' </summary>
        ''' <param name="AutoID"></param>
        ''' <param name="ChangeNO"></param>
        ''' <param name="PM_M_Code"></param>
        ''' <param name="D_ID"></param>
        ''' <param name="AddAction"></param>
        ''' <param name="StartDate"></param>
        ''' <param name="EndDate"></param>
        ''' <param name="CheckStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function NmetalSampleDepWeightCheck_GetList(ByVal AutoID As String, ByVal ChangeNO As String, ByVal PM_M_Code As String, ByVal D_ID As String, ByVal AddAction As String, ByVal StartDate As String, ByVal EndDate As String, ByVal CheckStatus As String) As List(Of NmetalSampleDepWeightCheckInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleDepWeightCheck_GetList")

            db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
            db.AddInParameter(dbComm, "@ChangeNO", DbType.String, ChangeNO)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@AddAction", DbType.String, AddAction)

            db.AddInParameter(dbComm, "@StartDate", DbType.DateTime, StartDate)
            db.AddInParameter(dbComm, "@EndDate", DbType.DateTime, EndDate)
            db.AddInParameter(dbComm, "@CheckStatus", DbType.String, CheckStatus)



            Dim FeatureList As New List(Of NmetalSampleDepWeightCheckInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleDepWeightCheck(reader))
                End While
                Return FeatureList
            End Using
        End Function
        ''' <summary>
        ''' 綁定數據
        ''' </summary>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Friend Function FillNmetalSampleDepWeightCheck(ByVal reader As IDataReader) As NmetalSampleDepWeightCheckInfo
            On Error Resume Next

            Dim objInfo As New NmetalSampleDepWeightCheckInfo

            objInfo.AutoID = reader("AutoID").ToString
            objInfo.ChangeNO = reader("ChangeNO").ToString
            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.PM_Type = reader("PM_Type").ToString
            objInfo.D_ID = reader("D_ID").ToString

            objInfo.DepWightOld = reader("DepWightOld")
            objInfo.DepWightNew = reader("DepWightNew")
            objInfo.AddAction = reader("AddAction").ToString
            objInfo.AddDate = CDate(reader("AddDate").ToString)
            objInfo.Remark = reader("Remark").ToString

            objInfo.CheckStatus = reader("CheckStatus")
            objInfo.CheckAction = reader("CheckAction").ToString
            objInfo.CheckDate = CDate(reader("CheckDate").ToString)
            objInfo.CheckRemark = reader("CheckRemark").ToString
            objInfo.DepName = reader("DepName").ToString

            objInfo.Add_Name = reader("Add_Name").ToString
            objInfo.Check_Name = reader("Check_Name").ToString
            objInfo.Modify_Name = reader("Modify_Name").ToString
            objInfo.ModifyDate = reader("ModifyDate").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.PS_Name = reader("PS_Name").ToString
            objInfo.SubRemark = reader("SubRemark").ToString

            objInfo.ProductWightOld = reader("ProductWightOld")
            Return objInfo
        End Function


    End Class
End Namespace
