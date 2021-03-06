Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql

Namespace LFERP.Library.SampleManager.SampleOrdersSub

    Public Class SampleOrdersSubControler

        Public Function SampleOrdersSub_Add(ByVal objinfo As SampleOrdersSubInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersSub_Add")
                db.AddInParameter(dbComm, "SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "SWI_Qty", DbType.Int32, objinfo.SWI_Qty)
                db.AddInParameter(dbComm, "SS_OrderQty", DbType.Int32, objinfo.SS_OrderQty)
                db.AddInParameter(dbComm, "SS_Price", DbType.Double, objinfo.SS_Price)
                db.AddInParameter(dbComm, "SS_Remark", DbType.String, objinfo.SS_Remark)
                db.AddInParameter(dbComm, "CO_ID", DbType.String, objinfo.CO_ID)

                db.ExecuteNonQuery(dbComm)
                SampleOrdersSub_Add = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleOrdersSub_Add = False
            End Try

        End Function

        Public Function SampleOrdersSub_Update(ByVal objinfo As SampleOrdersSubInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersSub_Update")
                db.AddInParameter(dbComm, "AutoID", DbType.String, objinfo.AutoID)
                db.AddInParameter(dbComm, "SO_ID", DbType.String, objinfo.SO_ID)
                db.AddInParameter(dbComm, "SS_Edition", DbType.String, objinfo.SS_Edition)
                db.AddInParameter(dbComm, "SWI_Qty", DbType.Int32, objinfo.SWI_Qty)
                db.AddInParameter(dbComm, "SS_OrderQty", DbType.Int32, objinfo.SS_OrderQty)
                db.AddInParameter(dbComm, "SS_Price", DbType.Double, objinfo.SS_Price)
                db.AddInParameter(dbComm, "SS_Remark", DbType.String, objinfo.SS_Remark)
                db.AddInParameter(dbComm, "CO_ID", DbType.String, objinfo.CO_ID)

                db.ExecuteNonQuery(dbComm)
                SampleOrdersSub_Update = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleOrdersSub_Update = False
            End Try
        End Function


        Public Function SampleOrdersSub_DeleteItem(ByVal AutoID As String) As Boolean

            Try

                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersSub_DeleteItem")

                db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)

                db.ExecuteNonQuery(dbComm)
                SampleOrdersSub_DeleteItem = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleOrdersSub_DeleteItem = False
            End Try
        End Function

        Public Function SampleOrdersSub_Delete(ByVal SO_ID As String) As Boolean

            Try

                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersSub_Delete")

                db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)

                db.ExecuteNonQuery(dbComm)
                SampleOrdersSub_Delete = True

            Catch ex As Exception
                MsgBox(ex.Message)
                SampleOrdersSub_Delete = False
            End Try
        End Function

        Public Function SampleOrdersSub_GetList(ByVal SO_ID As String, ByVal SS_Edition As String) As List(Of SampleOrdersSubInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersSub_GetList")

            db.AddInParameter(dbComm, "@SO_ID", DbType.String, SO_ID)
            db.AddInParameter(dbComm, "@SS_Edition", DbType.String, SS_Edition)


            Dim FeatureList As New List(Of SampleOrdersSubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleOrdersSubType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Friend Function FillSampleOrdersSubType(ByVal reader As IDataReader) As SampleOrdersSubInfo
            '对应取得的数据
            On Error Resume Next

            Dim objInfo As New SampleOrdersSubInfo

            objInfo.AutoID = reader("AutoID").ToString
            objInfo.CO_ID = reader("CO_ID").ToString
            objInfo.SS_Edition = reader("SS_Edition").ToString
            objInfo.SWI_Qty = CInt(reader("SWI_Qty").ToString)
            objInfo.SO_NoSendQty = CInt(reader("SO_NoSendQty").ToString)
            objInfo.SS_OrderQty = CInt(reader("SS_OrderQty").ToString)
            objInfo.SS_Price = CDbl(reader("SS_Price").ToString)
            objInfo.SS_Remark = reader("SS_Remark").ToString
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.SO_Closed = CBool(reader("SO_Closed").ToString)
            objInfo.SO_SampleID = reader("SO_SampleID").ToString
            Return objInfo
        End Function

    End Class

End Namespace