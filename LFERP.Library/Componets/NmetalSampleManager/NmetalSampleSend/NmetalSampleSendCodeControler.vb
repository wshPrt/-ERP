Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql

Namespace LFERP.Library.NmetalSampleManager.NmetalSampleSend
    Public Class NmetalSampleSendCodeControler
        Public Function NmetalSampleSendCode_Add(ByVal objinfo As NmetalSampleSendCodeInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_Add")

                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "@Code_Qty", DbType.Int32, objinfo.Code_Qty)
                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objinfo.Code_ID)

                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@AddUserID", DbType.String, objinfo.AddUserID)
                db.AddInParameter(dbComm, "@AddDate", DbType.DateTime, CDate(objinfo.AddDate))
                db.AddInParameter(dbComm, "@CodeType", DbType.String, objinfo.CodeType)

                db.AddInParameter(dbComm, "@SendWeight", DbType.Decimal, objinfo.SendWeight)
                db.AddInParameter(dbComm, "@CompWeight", DbType.Decimal, objinfo.CompWeight)


                db.ExecuteNonQuery(dbComm)
                NmetalSampleSendCode_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleSendCode_Add = False
            End Try
        End Function

        Public Function NmetalSampleSendCode_Update(ByVal objinfo As NmetalSampleSendCodeInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_Update")

                db.AddInParameter(dbComm, "@SP_ID", DbType.String, objinfo.SP_ID)
                db.AddInParameter(dbComm, "@SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "@SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "@Code_Qty", DbType.Int32, objinfo.Code_Qty)
                db.AddInParameter(dbComm, "@Code_ID", DbType.String, objinfo.Code_ID)
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.DateTime, objinfo.ModifyDate)
                db.AddInParameter(dbComm, "@AutoID", DbType.String, objinfo.AutoID)
                db.AddInParameter(dbComm, "@CodeType", DbType.String, objinfo.CodeType)

                db.AddInParameter(dbComm, "@SendWeight", DbType.Decimal, objinfo.SendWeight)
                db.AddInParameter(dbComm, "@CompWeight", DbType.Decimal, objinfo.CompWeight)

                db.ExecuteNonQuery(dbComm)
                NmetalSampleSendCode_Update = True

            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleSendCode_Update = False
            End Try
        End Function


        Public Function NmetalSampleSendCode_Delete(ByVal SP_ID As String, ByVal AutoID As String) As Boolean

            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_Delete")
                db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
                db.AddInParameter(dbComm, "@AutoID ", DbType.String, AutoID)
                db.ExecuteNonQuery(dbComm)
                NmetalSampleSendCode_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                NmetalSampleSendCode_Delete = False
            End Try
        End Function


        Public Function NmetalSampleSendCode_Getlist(ByVal SP_ID As String, ByVal SO_ID As String, ByVal SS_Edition As String, ByVal PM_M_Code As String, ByVal M_Code As String, ByVal AutoID As String) As List(Of NmetalSampleSendCodeInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_Getlist")

            db.AddInParameter(dbComm, "@SP_ID", DbType.String, SP_ID)
            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@Code_ID", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)


            Dim FeatureList As New List(Of NmetalSampleSendCodeInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillNmetalSampleSendCodeType(reader))
                End While
                Return FeatureList
            End Using
        End Function
        Public Function NmetalSampleSendCode_GetCount(ByVal SO_ID As String, ByVal SS_Edition As String) As Integer
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_GetCount")

            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)

            Dim StrCode_ID As Integer = 0
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    StrCode_ID = CInt(reader("NCount").ToString)
                End While
                NmetalSampleSendCode_GetCount = StrCode_ID
            End Using
        End Function
        Public Function NmetalSampleSendCode_GetID(ByVal Code_ID As String) As Boolean
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("NmetalSampleSendCode_GetID")

            db.AddInParameter(dbComm, "@Code_ID", DbType.String, Code_ID)

            Dim StrCode_ID As String = String.Empty
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    StrCode_ID = reader("Code_ID").ToString
                End While
                If StrCode_ID = String.Empty Then
                    Return False
                Else
                    Return True
                End If
            End Using
        End Function
        Friend Function FillNmetalSampleSendCodeType(ByVal reader As IDataReader) As NmetalSampleSendCodeInfo
            '对应取得的数据
            On Error Resume Next
            Dim objInfo As New NmetalSampleSendCodeInfo

            objInfo.SP_ID = reader("SP_ID").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.SS_Edition = reader("SS_Edition").ToString
            objInfo.Code_ID = reader("Code_ID").ToString
            objInfo.Code_Qty = CInt(reader("Code_Qty").ToString)
            objInfo.PM_M_Code = reader("PM_M_Code").ToString

            objInfo.AddDate = Format(CDate(reader("AddDate").ToString), "yyyy-MM-dd HH:mm:ss")
            objInfo.AddUserID = reader("AddUserID").ToString
            objInfo.ModifyUserID = reader("ModifyUserID").ToString
            objInfo.ModifyDate = CDate(reader("ModifyDate").ToString)
            objInfo.AutoID = reader("AutoID").ToString
            objInfo.CodeType = reader("CodeType").ToString

            objInfo.SendWeight = reader("SendWeight")
            objInfo.CompWeight = reader("CompWeight")



            Return objInfo
        End Function
    End Class
End Namespace

