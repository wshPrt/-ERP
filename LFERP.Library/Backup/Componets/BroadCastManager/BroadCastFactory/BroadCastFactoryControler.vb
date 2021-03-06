Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql

Namespace LFERP.Library.BroadCastManager.BroadCastFactory
    Public Class BroadCastFactoryControler
        Public Function BroadCastFactory_Add(ByVal objinfo As BroadCastFactoryInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("BroadCastFactory_Add")

                db.AddInParameter(dbComm, "Fac_Name", DbType.String, objinfo.Fac_Name)
                db.AddInParameter(dbComm, "Fac_no", DbType.String, objinfo.Fac_no)
                db.AddInParameter(dbComm, "Fac_AdduserID", DbType.Int32, objinfo.Fac_AdduserID)
                db.AddInParameter(dbComm, "Fac_Adddate", DbType.String, CDate(objinfo.Fac_Adddate))
                'db.AddInParameter(dbComm, "Fac_ModifyUserID", DbType.Int32, objinfo.Fac_ModifyUserID)
                'db.AddInParameter(dbComm, "Fac_ModifyDate", DbType.String, CDate(objinfo.Fac_ModifyDate))

                db.ExecuteNonQuery(dbComm)
                BroadCastFactory_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                BroadCastFactory_Add = False
            End Try
        End Function

        Public Function BroadCastFactory_Update(ByVal objinfo As BroadCastFactoryInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("BroadCastFactory_Update")

                db.AddInParameter(dbComm, "Fac_Name", DbType.String, objinfo.Fac_Name)
                db.AddInParameter(dbComm, "Fac_no", DbType.String, objinfo.Fac_no)
                'db.AddInParameter(dbComm, "Fac_AdduserID", DbType.Int32, objinfo.Fac_AdduserID)
                'db.AddInParameter(dbComm, "Fac_Adddate", DbType.String, CDate(objinfo.Fac_Adddate))
                db.AddInParameter(dbComm, "Fac_ModifyUserID", DbType.Int32, objinfo.Fac_ModifyUserID)
                db.AddInParameter(dbComm, "Fac_ModifyDate", DbType.String, CDate(objinfo.Fac_ModifyDate))


                db.ExecuteNonQuery(dbComm)
                BroadCastFactory_Update = True

            Catch ex As Exception
                MsgBox(ex.Message)
                BroadCastFactory_Update = False
            End Try
        End Function


        Public Function BroadCastFactory_Delete(ByVal AutoID As String) As Boolean

            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("BroadCastFactory_Delete")
                db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
                db.ExecuteNonQuery(dbComm)
                BroadCastFactory_Delete = True
            Catch ex As Exception
                MsgBox(ex.Message)
                BroadCastFactory_Delete = False
            End Try
        End Function
        Public Function BroadCastFactory_Getlist(ByVal Fac_no As String) As List(Of BroadCastFactoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("BroadCastFactory_Getlist")

            db.AddInParameter(dbComm, "@Fac_no", DbType.String, Fac_no)

            Dim FeatureList As New List(Of BroadCastFactoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillBroadCastFactoryType(reader))
                End While
                Return FeatureList
            End Using
        End Function
        Public Function BroadCastBrigade_GetListItem(ByVal M_KEY As String, ByVal Type As String) As List(Of BroadCastFactoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("BroadCastBrigade_GetListItem")
            db.AddInParameter(dbComm, "@M_KEY", DbType.String, M_KEY)
            db.AddInParameter(dbComm, "@Type", DbType.String, Type)


            Dim FeatureList As New List(Of BroadCastFactoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillBroadCastFactoryType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Friend Function FillBroadCastFactoryType(ByVal reader As IDataReader) As BroadCastFactoryInfo
            '对应取得的数据
            On Error Resume Next
            Dim objInfo As New BroadCastFactoryInfo

            objInfo.AutoID = reader("AutoID").ToString
            objInfo.Fac_Adddate = CDate(reader("Fac_Adddate").ToString)
            objInfo.Fac_AdduserID = reader("Fac_AdduserID").ToString
            objInfo.Fac_ModifyDate = CDate(reader("Fac_ModifyDate").ToString)
            objInfo.Fac_ModifyUserID = reader("Fac_ModifyUserID").ToString
            objInfo.Fac_Name = reader("Fac_Name").ToString
            objInfo.M_Name = reader("M_Name").ToString
            objInfo.M_Code = reader("M_Code").ToString
            objInfo.M_PID = reader("M_PID").ToString
            objInfo.M_KEY = reader("M_KEY").ToString
            objInfo.U_Name = reader("U_Name").ToString
            objInfo.Type = reader("Type").ToString
            objInfo.Fac_no = reader("Fac_no").ToString
            Return objInfo
        End Function

    End Class
End Namespace



