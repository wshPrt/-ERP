Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql

Namespace LFERP.Library.SampleManager.SampleTransaction

    Public Class SampleTransactionControler
        Public Function SampleTransaction_Add(ByVal objinfo As SampleTransactionInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Add")

                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objinfo.Code_ID)
                db.AddInParameter(dbComm, "@Qty", DbType.Int16, objinfo.Qty)
                db.AddInParameter(dbComm, "@StatusType", DbType.String, objinfo.StatusType)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objinfo.Remark)
                db.AddInParameter(dbComm, "@AddUserID", DbType.String, objinfo.AddUserID)
                db.AddInParameter(dbComm, "@AddDate", DbType.DateTime, objinfo.AddDate)

                db.AddInParameter(dbComm, "@TR_ID", DbType.String, objinfo.TR_ID)
                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)

                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@SS_Edition", DbType.String, objinfo.SS_Edition)


                db.ExecuteNonQuery(dbComm)
                SampleTransaction_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleTransaction_Add = False
            End Try
        End Function

        Public Function SampleTransaction_Update(ByVal objinfo As SampleTransactionInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Update")
                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objinfo.Code_ID)
                db.AddInParameter(dbComm, "@Qty", DbType.Int16, objinfo.Qty)
                db.AddInParameter(dbComm, "@StatusType", DbType.String, objinfo.StatusType)
                db.AddInParameter(dbComm, "@Remark", DbType.String, objinfo.Remark)
                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.DateTime, objinfo.ModifyDate)

                db.AddInParameter(dbComm, "@TR_ID", DbType.String, objinfo.TR_ID)
                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@AutoID", DbType.Double, objinfo.AutoID)

                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@SS_Edition", DbType.String, objinfo.SS_Edition)


                db.ExecuteNonQuery(dbComm)
                SampleTransaction_Update = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleTransaction_Update = False
            End Try
        End Function

        Public Function SampleTransaction_UpdateCheck(ByVal objinfo As SampleTransactionInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_UpdateCheck")

                db.AddInParameter(dbComm, "@TR_ID", DbType.String, objinfo.TR_ID)
                db.AddInParameter(dbComm, "@CheckBit", DbType.Boolean, objinfo.CheckBit)
                db.AddInParameter(dbComm, "@CheckDate", DbType.Date, objinfo.CheckDate)
                db.AddInParameter(dbComm, "@CheckUserID", DbType.String, objinfo.CheckUserID)
                db.AddInParameter(dbComm, "@CheckRemark", DbType.String, objinfo.CheckRemark)
                db.ExecuteNonQuery(dbComm)
                SampleTransaction_UpdateCheck = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleTransaction_UpdateCheck = False
            End Try
        End Function

        Public Function SampleTransaction_Delete(ByVal AutoID As Decimal, ByVal TR_ID As String) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Delete")
                db.AddInParameter(dbComm, "@AutoID", DbType.Double, AutoID)
                db.AddInParameter(dbComm, "@TR_ID", DbType.Double, TR_ID)
                db.ExecuteNonQuery(dbComm)
                SampleTransaction_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleTransaction_Delete = False
            End Try
        End Function

        Public Function SampleTransaction_Getlist(ByVal StatusType As String, ByVal Code_ID As String, ByVal AutoID As String, ByVal TR_ID As String, ByVal SP_ID As String, ByVal SO_ID As String, ByVal SS_Edition As String) As List(Of SampleTransactionInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Getlist")

            db.AddInParameter(dbComm, "@TR_ID", DbType.String, TR_ID)
            db.AddInParameter(dbComm, "@StatusType", DbType.String, StatusType)
            db.AddInParameter(dbComm, "@Code_ID", DbType.String, Code_ID)
            db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
            db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)

            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)

            Dim FeatureList As New List(Of SampleTransactionInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleTransactionType(reader))
                End While
                Return FeatureList
            End Using
        End Function
        Public Function SampleTransactionMain_GetList(ByVal TR_ID As String) As List(Of SampleTransactionInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransactionMain_GetList")

            db.AddInParameter(dbComm, "@TR_ID", DbType.String, TR_ID)

            Dim FeatureList As New List(Of SampleTransactionInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleTransactionType(reader))
                End While
                Return FeatureList
            End Using
        End Function


        Public Function SampleTransaction_Get(ByVal TR_ID As String) As SampleTransactionInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Get")
            db.AddInParameter(dbComm, "@TR_ID", DbType.String, TR_ID)
            Dim FeatureList As New SampleTransactionInfo
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.TR_ID = reader("TR_ID").ToString
                End While
                Return FeatureList
            End Using
        End Function

        'Public Function SampleTransaction_Get(ByVal SP_ID As String) As SampleTransactionInfo
        '    Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
        '    Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_Get")
        '    db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
        '    Dim FeatureList As New SampleTransactionInfo
        '    Using reader As IDataReader = db.ExecuteReader(dbComm)
        '        While reader.Read
        '            FeatureList.SP_ID = reader("SP_ID").ToString
        '        End While
        '        Return FeatureList
        '    End Using
        'End Function
        'Public Function SampleTransaction_GetItem(ByVal SO_ID As String, ByVal SS_Edition As String) As List(Of SampleTransactionInfo)
        '    Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
        '    Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransaction_GetItem")
        '    db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
        '    db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)

        '    Dim FeatureList As New List(Of SampleTransactionInfo)
        '    Using reader As IDataReader = db.ExecuteReader(dbComm)
        '        While reader.Read
        '            FeatureList.Add(FillSampleTransactionType(reader))
        '        End While
        '        Return FeatureList
        '    End Using
        'End Function


        Friend Function FillSampleTransactionType(ByVal reader As IDataReader) As SampleTransactionInfo
            '对应取得的数据
            On Error Resume Next
            Dim objInfo As New SampleTransactionInfo

            objInfo.CheckBit = CBool(reader("CheckBit").ToString)
            objInfo.CheckDate = CDate(reader("CheckDate").ToString)
            objInfo.CheckRemark = reader("CheckRemark").ToString
            objInfo.CheckUserID = reader("CheckUserID").ToString
            objInfo.CheckUserName = reader("CheckUserName").ToString
            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.SP_ID = reader("SP_ID").ToString
            objInfo.TR_ID = reader("TR_ID").ToString

            objInfo.AddDate = CDate(reader("AddDate").ToString)
            objInfo.AddUserID = reader("AddUserID").ToString
            objInfo.AddUserName = reader("AddUserName").ToString
            objInfo.AutoID = CDbl(reader("AutoID").ToString)
            objInfo.Code_ID = reader("Code_ID").ToString
            objInfo.ModifyDate = CDate(reader("ModifyDate").ToString)
            objInfo.ModifyUserID = reader("ModifyUserID").ToString
            objInfo.Qty = CInt(reader("Qty"))
            objInfo.Remark = reader("Remark").ToString
            objInfo.StatusType = reader("StatusType").ToString

            objInfo.StatusTypeName = reader("StatusTypeName").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.SS_Edition = reader("SS_Edition").ToString
            objInfo.SP_IDItem = reader("SP_IDItem").ToString
            objInfo.IsTransferred = reader("IsTransferred").ToString

            Return objInfo
        End Function



        Public Function SampleTransactionType_GetList(ByVal StatusType As String, ByVal SYesNO As String) As List(Of SampleTransactionInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleTransactionType_GetList")

            db.AddInParameter(dbComm, "@StatusType", DbType.String, StatusType)
            db.AddInParameter(dbComm, "@SYesNO", DbType.String, SYesNO)

            Dim FeatureList As New List(Of SampleTransactionInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleTransactionType(reader))
                End While
                Return FeatureList
            End Using
        End Function

    End Class
End Namespace