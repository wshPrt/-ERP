Imports System.data.Sql
Imports System.Data.SqlClient
Imports System.Data.Common
Namespace LFERP.Library.KnifeWare
    Public Class KnifeWareInventorySubControl
        Public Function KnifeWareInventorySub_Update(ByVal kwinfo As KnifeWareInventorySubInfo) As Boolean

            Try
                Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
                Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySub_Update")
                db.AddInParameter(dbComm, "@M_Code", DbType.String, kwinfo.M_Code)
                db.AddInParameter(dbComm, "@WH_ID", DbType.String, kwinfo.WH_ID)
                db.AddInParameter(dbComm, "@WI_SQty", DbType.Double, kwinfo.WI_SQty)
                db.AddInParameter(dbComm, "@WI_SReQty", DbType.Double, kwinfo.WI_SReQty)
                db.AddInParameter(dbComm, "@WI_All", DbType.Double, kwinfo.WI_All)
                db.ExecuteNonQuery(dbComm)
                KnifeWareInventorySub_Update = True
            Catch ex As Exception
                MsgBox(ex.Message)
                KnifeWareInventorySub_Update = False
            End Try
        End Function

        Public Function KnifeWareInventorySub_GetListItem(ByVal M_Code As String, ByVal WH_ID As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySub_GetListItem")

            db.AddInParameter(dbComm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@WH_ID", DbType.String, WH_ID)
            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function KnifeWareInventory_GetList(ByVal Type3ID As String, ByVal WH_ID As String, ByVal M_CodeList As String, ByVal KnifeID As String, ByVal M_Name As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventory_GetList")
            db.AddInParameter(dbComm, "@Type3ID", DbType.String, Type3ID)
            db.AddInParameter(dbComm, "@WH_ID", DbType.String, WH_ID)
            db.AddInParameter(dbComm, "@M_CodeList", DbType.String, M_CodeList)
            db.AddInParameter(dbComm, "@KnifeID", DbType.String, KnifeID)
            db.AddInParameter(dbComm, "@M_Name", DbType.String, M_Name)
            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function

        ''' <summary>
        ''' 2013-11-13
        ''' 姚駿新增
        ''' </summary>
        ''' <param name="M_Code"></param>
        ''' <param name="WH_ID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function KnifeWareInventorySub_GetListOne(ByVal M_Code As String, ByVal WH_ID As String) As KnifeWareInventorySubInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySub_GetListOne")
            db.AddInParameter(dbComm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@WH_ID", DbType.String, WH_ID)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    Return FillKnifeWareInventorySub(reader)
                End While
                Return Nothing
            End Using
        End Function

        Public Function KnifeWareInventorySub_GetList(ByVal M_Code As String, ByVal WH_ID As String) As KnifeWareInventorySubInfo
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySub_GetList")
            db.AddInParameter(dbComm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbComm, "@WH_ID", DbType.String, WH_ID)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    Return FillKnifeWareInventorySub(reader)
                End While
                Return Nothing
            End Using
        End Function

        ''Friend Function FillKnifeWareInventorySub(ByVal reader As IDataReader) As KnifeWareInventorySubInfo
        ''    On Error Resume Next
        ''    Dim kwinfo As New KnifeWareInventorySubInfo
        ''    kwinfo.M_Code = reader("M_Code").ToString()
        ''    kwinfo.WH_ID = reader("WH_ID").ToString()
        ''    kwinfo.WI_SQty = IIf(IsDBNull(reader("WI_SQty")), 0, CDbl(reader("WI_SQty")))
        ''    kwinfo.WI_SReQty = IIf(IsDBNull(reader("WI_SReQty")), 0, CDbl(reader("WI_SReQty")))

        ''    kwinfo.WI_Qty = IIf(IsDBNull(reader("WI_Qty")), 0, CDbl(reader("WI_Qty")))
        ''    kwinfo.WI_SafeQty = IIf(IsDBNull(reader("WI_SafeQty")), 0, CDbl(reader("WI_SafeQty")))
        ''    kwinfo.WI_UsableQty = IIf(IsDBNull(reader("WI_UsableQty")), 0, CDbl(reader("WI_UsableQty")))
        ''    kwinfo.WI_UserID = reader("WI_UserID").ToString()
        ''    kwinfo.PM_M_Name = reader("PM_M_Name").ToString()
        ''    kwinfo.WH_Name = reader("WH_Name").ToString()
        ''    kwinfo.M_Gauge = reader("M_Gauge").ToString()
        ''    kwinfo.M_Unit = reader("M_Unit").ToString()
        ''    kwinfo.WI_EditDate = CDate(reader("WI_EditDate").ToString())
        ''    Return kwinfo
        ''End Function

        Public Function FillKnifeWareInventorySub(ByVal reader As IDataReader) As KnifeWareInventorySubInfo
            Dim ai As New KnifeWareInventorySubInfo

            On Error Resume Next

            ai.Type3ID = reader("Type3ID").ToString
            ai.WH_ID = reader("WH_ID").ToString
            ai.Type3Name = reader("Type3Name").ToString

            ai.WI_SQty = reader("WI_SQty")
            ai.WI_SReQty = reader("WI_SReQty")
            '--------------------------------------------------

            If reader("WI_SQty") Is DBNull.Value Then
                ai.WI_SQty = 0
            Else
                ai.WI_SQty = CDbl(reader("WI_SQty"))
            End If

            If reader("WI_SReQty") Is DBNull.Value Then
                ai.WI_SReQty = 0
            Else
                ai.WI_SReQty = CDbl(reader("WI_SReQty"))
            End If

            If reader("WI_QtyAll") Is DBNull.Value Then
                ai.WI_QtyAll = 0
            Else
                ai.WI_QtyAll = CDbl(reader("WI_QtyAll"))
            End If

            If reader("B_Qty") Is DBNull.Value Then
                ai.B_Qty = 0
            Else
                ai.B_Qty = CDbl(reader("B_Qty"))
            End If

            ai.M_Code = reader("M_code").ToString
            ai.M_Name = reader("M_Name").ToString
            ai.M_Gauge = reader("M_Gauge").ToString
            ai.Type1ID = reader("Type1ID").ToString
            ai.Type1Name = reader("Type1Name").ToString
            ai.Type2ID = reader("Type2ID").ToString
            ai.Type2Name = reader("Type2Name").ToString

            ai.WI_Qty = IIf(IsDBNull(reader("WI_Qty")), 0, CDbl(reader("WI_Qty")))
            ai.WI_SafeQty = IIf(IsDBNull(reader("WI_SafeQty")), 0, CDbl(reader("WI_SafeQty")))
            ai.WI_UsableQty = IIf(IsDBNull(reader("WI_UsableQty")), 0, CDbl(reader("WI_UsableQty")))
            ai.WI_UserID = reader("WI_UserID").ToString()
            ai.PM_M_Name = reader("PM_M_Name").ToString()
            ai.WH_Name = reader("WH_Name").ToString()
            ai.M_Gauge = reader("M_Gauge").ToString()
            ai.M_Unit = reader("M_Unit").ToString()
            ai.WI_EditDate = CDate(reader("WI_EditDate").ToString())

            ai.B_Date = reader("B_Date")
            ai.R_Date = reader("R_Date")

            ai.NO_ReturnSum = CDbl(reader("NO_ReturnSum"))
            ai.WI_NOReturn = CDbl(reader("WI_NOReturn"))

            ai.KnNO = reader("KnNO").ToString                '單號
            ai.TypeB = reader("TypeB").ToString            '類型
            ai.KnQty = reader("KnQty")              '數量
            ai.KnDate = reader("KnDate")              '日期
            ai.KnType = reader("KnType").ToString              '屬性
            ai.KnInfo = reader("KnInfo").ToString             '備註
            ai.BackUpTime = reader("BackUpTime")         '備份時間
            Return ai
        End Function

        Public Function KnifeWareInventorySubTypeGroup_GetList(ByVal WH_ID As String, ByVal Type3ID As String, ByVal Type As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySubTypeGroup_GetList")

            db.AddInParameter(dbcomm, "@WH_ID", DbType.String, WH_ID) '借刀流水號
            db.AddInParameter(dbcomm, "@Type3ID", DbType.String, Type3ID) '借刀單號
            db.AddInParameter(dbcomm, "@Type", DbType.String, Type) '刀具編碼

            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbcomm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function

        ''' <summary>
        ''' 包含庫存為0的記錄
        ''' </summary>
        ''' <param name="WH_ID"></param>
        ''' <param name="Type3ID"></param>
        ''' <param name="Type"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function KnifeWareInventorySubTypeGroup_GetList1(ByVal WH_ID As String, ByVal Type3ID As String, ByVal Type As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySubTypeGroup_GetList1")

            db.AddInParameter(dbcomm, "@WH_ID", DbType.String, WH_ID) '借刀流水號
            db.AddInParameter(dbcomm, "@Type3ID", DbType.String, Type3ID) '借刀單號
            db.AddInParameter(dbcomm, "@Type", DbType.String, Type) '刀具編碼

            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbcomm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function KnifeWareInventorySubType_GetList(ByVal WH_ID As String, ByVal M_Code As String, ByVal Type3ID As String, ByVal Type2ID As String, ByVal Type1ID As String, ByVal Type As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySubType_GetList")

            db.AddInParameter(dbcomm, "@WH_ID", DbType.String, WH_ID)
            db.AddInParameter(dbcomm, "@M_Code", DbType.String, M_Code)
            db.AddInParameter(dbcomm, "@Type3ID", DbType.String, Type3ID)

            db.AddInParameter(dbcomm, "@Type2ID", DbType.String, Type2ID)
            db.AddInParameter(dbcomm, "@Type1ID", DbType.String, Type1ID)
            db.AddInParameter(dbcomm, "@Type", DbType.String, Type)

            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbcomm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function


        ''' <summary>
        ''' 呆滞物料函数调用
        ''' </summary>
        ''' <param name="dtStart"></param>
        ''' <param name="dtEnd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function KnifeWareInventoryDull_GetList(ByVal dtStart As String, ByVal dtEnd As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventoryDull_GetList")
            db.AddInParameter(dbComm, "@DateStart", DbType.String, dtStart)
            db.AddInParameter(dbComm, "@DateEnd", DbType.String, dtEnd)
            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function

        Public Function KnifeWareInventory_GetListType123(ByVal Type1ID As String, ByVal Type2ID As String, ByVal Type3ID As String, ByVal WH_ID As String, ByVal M_CodeList As String, ByVal KnifeID As String, ByVal M_Name As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventory_GetListType123")
            db.AddInParameter(dbComm, "@Type1ID", DbType.String, Type1ID)
            db.AddInParameter(dbComm, "@Type2ID", DbType.String, Type2ID)
            db.AddInParameter(dbComm, "@Type3ID", DbType.String, Type3ID)

            db.AddInParameter(dbComm, "@WH_ID", DbType.String, WH_ID)
            db.AddInParameter(dbComm, "@M_CodeList", DbType.String, M_CodeList)
            db.AddInParameter(dbComm, "@KnifeID", DbType.String, KnifeID)
            db.AddInParameter(dbComm, "@M_Name", DbType.String, M_Name)
            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function




        Public Function KnifeWareInventoryDull_NewGetList(ByVal dtStart As String, ByVal QtyType As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbComm As DbCommand = db.GetStoredProcCommand("KnifeWareInventoryDull_NewGetList")
            db.AddInParameter(dbComm, "@DateStart", DbType.String, dtStart)
            db.AddInParameter(dbComm, "@QtyType", DbType.String, QtyType)
            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbComm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function


#Region "庫存備份"
        Public Function WareInventoryBackUp_GetListType123(ByVal WH_ID As String, ByVal Type1ID As String, ByVal Type2ID As String, ByVal Type3ID As String, ByVal O_Date1 As String, ByVal O_Date2 As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetStoredProcCommand("WareInventoryBackUp_GetListType123")


            db.AddInParameter(dbcomm, "@WH_ID", DbType.String, WH_ID)                                      '倉庫ID
            db.AddInParameter(dbcomm, "@Type1ID", DbType.String, Type1ID)                                  '類型
            db.AddInParameter(dbcomm, "@Type2ID", DbType.String, Type2ID)                                  '屬性
            db.AddInParameter(dbcomm, "@Type3ID", DbType.String, Type3ID)                                  '刀具編碼
            db.AddInParameter(dbcomm, "@O_Date1", DbType.String, O_Date1)                                  '開始時間
            db.AddInParameter(dbcomm, "@O_Date2", DbType.String, O_Date2)                                  '結束時間


            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbcomm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using



        End Function
#End Region

#Region "數據搜索函數"
        Public Function KnifeWareInventorySearch_GetList(ByVal WH_ID As String, ByVal TypeB As String, ByVal KnifeType As String, ByVal M_Code As String, ByVal WIP_Date1 As String, ByVal WIP_Date2 As String) As List(Of KnifeWareInventorySubInfo)
            Dim db As New Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase(ConnStr)
            Dim dbcomm As DbCommand = db.GetStoredProcCommand("KnifeWareInventorySearch_GetList")

            db.AddInParameter(dbcomm, "@WH_ID", DbType.String, WH_ID)                '倉庫ID
            db.AddInParameter(dbcomm, "@TypeB", DbType.String, TypeB)                '類型   
            db.AddInParameter(dbcomm, "@KnifeType", DbType.String, KnifeType)        '屬性
            db.AddInParameter(dbcomm, "@M_Code", DbType.String, M_Code)              '刀具編碼
            db.AddInParameter(dbcomm, "@WIP_Date1", DbType.String, WIP_Date1)        '開始時間
            db.AddInParameter(dbcomm, "@WIP_Date2", DbType.String, WIP_Date2)        '結束時間

            Dim FeatureList As New List(Of KnifeWareInventorySubInfo)
            Using reader As IDataReader = db.ExecuteReader(dbcomm)
                While reader.Read
                    FeatureList.Add(FillKnifeWareInventorySub(reader))
                End While
                Return FeatureList
            End Using
        End Function
#End Region



    End Class
End Namespace