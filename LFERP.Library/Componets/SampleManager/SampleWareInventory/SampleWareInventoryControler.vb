Imports System.Data.SqlClient
Imports System.Data.Common
Imports System.Data.Sql
Namespace LFERP.Library.SampleManager.SampleWareInventory
    Public Class SampleWareInventoryControler
        Public Function SampleWareInventory_Update(ByVal objinfo As SampleWareInventoryInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleWareInventory_Update")
                dbComm.CommandTimeout = 0
                db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, objinfo.PM_M_Code)
                db.AddInParameter(dbComm, "@M_Code", DbType.String, objinfo.M_Code)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@SWI_Qty", DbType.Int32, objinfo.SWI_Qty)

                db.AddInParameter(dbComm, "@ModifyUserID", DbType.String, objinfo.ModifyUserID)
                db.AddInParameter(dbComm, "@ModifyDate", DbType.DateTime, objinfo.ModifyDate)
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)

                db.ExecuteNonQuery(dbComm)
                SampleWareInventory_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleWareInventory_Update = False
            End Try
        End Function

        Public Function SampleWareInventory_Getlist(ByVal PM_M_Code As String, ByVal M_Code As String, ByVal PS_NO As String, ByVal AutoID As String, ByVal ReportEmpty As Boolean, ByVal D_ID As String) As List(Of SampleWareInventoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleWareInventory_Getlist")
            dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@AutoID", DbType.String, AutoID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)

            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While

                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New SampleWareInventoryInfo())
                End If

                Return FeatureList
            End Using
        End Function
        Public Function SampleWareInventoryPS_NO_GetList(ByVal PS_NO As String) As List(Of SampleWareInventoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleWareInventoryPS_NO_GetList")
            dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)

            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleWareInventoryA_Getlist(ByVal D_ID As String, ByVal PS_NO As String, ByVal PM_M_Code As String, ByVal ReportEmpty As Boolean) As List(Of SampleWareInventoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleWareInventoryA_Getlist")
            dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)
            db.AddInParameter(dbComm, "@PS_NO", DbType.String, PS_NO)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)

            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While

                If FeatureList.Count <= 0 And ReportEmpty Then
                    FeatureList.Add(New SampleWareInventoryInfo())
                End If

                Return FeatureList
            End Using
        End Function



        Friend Function FillSampleWareInventoryType(ByVal reader As IDataReader) As SampleWareInventoryInfo
            '对应取得的数据
            On Error Resume Next
            Dim objInfo As New SampleWareInventoryInfo
            objInfo.AutoID = reader("AutoID").ToString

            objInfo.PM_M_Code = reader("PM_M_Code").ToString
            objInfo.PS_NO = reader("PS_NO").ToString
            objInfo.SWI_Qty = reader("SWI_Qty")
            objInfo.OutQty = reader("OutQty")
            objInfo.InQty = reader("InQty")
            objInfo.BarCodeCount = reader("BarCodeCount")

            objInfo.LoseCount = reader("LoseCount")
            objInfo.DamageCount = reader("DamageCount")
            objInfo.FinishedCount = reader("FinishedCount")
            objInfo.ReturnCount = reader("ReturnCount")
            objInfo.SendCount = reader("SendCount")

            objInfo.M_Code = reader("M_Code").ToString

            objInfo.PS_Name = reader("PS_Name").ToString
            objInfo.D_Dep = reader("D_Dep").ToString
            objInfo.D_ID = reader("D_ID").ToString

            objInfo.AddUserID = reader("AddUserID").ToString
            objInfo.AddUserName = reader("AddUserName").ToString
            objInfo.AddDate = CDate(reader("AddDate").ToString)
            objInfo.ModifyDate = CDate(reader("ModifyDate").ToString)
            objInfo.ModifyUserID = reader("ModifyUserID").ToString
            objInfo.SO_SampleID = reader("SO_SampleID").ToString
            objInfo.MaterialTypeName = reader("MaterialTypeName").ToString
            objInfo.Borrow_Qty = reader("Borrow_Qty")
            objInfo.SO_ID = reader("SO_ID").ToString
            objInfo.RepayQty = reader("RepayQty")
            objInfo.AvailableQty = reader("AvailableQty")
            objInfo.NoBorrow_Qty = reader("NoBorrow_Qty")
            Return objInfo
        End Function


        '
        Public Function SampleProcessInventory_GetList(ByVal Pro_Type As String, ByVal PM_M_Code As String, ByVal PM_Type As String) As List(Of SampleWareInventoryInfo)

            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleProcessInventory_GetList")
            'dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@Pro_Type", DbType.String, Pro_Type)
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@PM_Type", DbType.String, PM_Type)

            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While

                Return FeatureList
            End Using
        End Function

        Public Function SampleOrdersMainInvent_GetList(ByVal PM_M_Code As String, ByVal MaterialTypeID As String, ByVal SO_SampleID As String, ByVal D_ID As String) As List(Of SampleWareInventoryInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersMainInvent_GetList")
            dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@MaterialTypeID", DbType.String, MaterialTypeID)
            db.AddInParameter(dbComm, "@SO_SampleID", DbType.String, SO_SampleID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)


            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleOrdersMainInventA_GetList(ByVal PM_M_Code As String, ByVal MaterialTypeID As String, ByVal SO_SampleID As String, ByVal D_ID As String) As List(Of SampleWareInventoryInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleOrdersMainInventA_GetList")
            dbComm.CommandTimeout = 0
            db.AddInParameter(dbComm, "@PM_M_Code", DbType.String, PM_M_Code)
            db.AddInParameter(dbComm, "@MaterialTypeID", DbType.String, MaterialTypeID)
            db.AddInParameter(dbComm, "@SO_SampleID", DbType.String, SO_SampleID)
            db.AddInParameter(dbComm, "@D_ID", DbType.String, D_ID)

            Dim FeatureList As New List(Of SampleWareInventoryInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillSampleWareInventoryType(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function SampleWareInventoryChange_Add(ByVal objinfo As SampleWareInventoryInfo) As Boolean
            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("SampleWareInventoryChange_Add")
                dbComm.CommandTimeout = 0
                db.AddInParameter(dbComm, "@D_ID", DbType.String, objinfo.D_ID)
                db.AddInParameter(dbComm, "@PS_NO", DbType.String, objinfo.PS_NO)
                db.AddInParameter(dbComm, "@SWI_QtyQ", DbType.Int32, objinfo.SWI_QtyQ)
                db.AddInParameter(dbComm, "@SWI_QtyH", DbType.Int32, objinfo.SWI_QtyH)

                db.AddInParameter(dbComm, "@AddUserID", DbType.String, objinfo.AddUserID)
                db.AddInParameter(dbComm, "@AddDate", DbType.DateTime, objinfo.AddDate)

                db.ExecuteNonQuery(dbComm)
                SampleWareInventoryChange_Add = True
            Catch ex As Exception
                MsgBox(ex.Message)
                SampleWareInventoryChange_Add = False
            End Try
        End Function

    End Class
End Namespace

