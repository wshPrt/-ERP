Imports LFERP.DataSetting
Imports LFERP.Library.Material
Imports LFERP.Library.Shared
Imports LFERP.Library.WareHouse

Module mdlPermissionModule

    Public Sub LoadModule(ByVal ModuleID As String)
        '依選擇的模塊調用子窗口

        Dim fr As New Form
        Select Case ModuleID


            '------------------物料編碼管理------------------------------- 
            Case "1001"  '物料編碼
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMaterialMain Then

                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMaterialMain
            Case "1003" ' "物料規格參數設置"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMaterialParamType Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMaterialParamType
            Case "1002" ' "物料規格設置"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMaterialParam Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMaterialParam
            Case "1004" '"物料類別設置"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMaterialTypeManger Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMaterialTypeManger
                '------------------公用信息查詢-------------------------------
            Case "110101"   '批次狀況--大貨
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmBatchDetailMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmBatchDetailMain

            Case "110102"   '批次狀況--配件
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPJBatchMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPJBatchMain
                '------------------產品資料管理-------------------------------
            Case "2001" '產品資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductMain

                '------------------訂單資料管理-------------------------------
            Case "3001" '訂單資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOrderMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOrderMain
            Case "30011" '客戶訂單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOrderCustomerMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOrderCustomerMain

            Case "300201"  '批次資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOrderSubMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOrderSubMain
            Case "300202"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOrdersSPMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOrdersSPMain
            Case "3003" '批次需求單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOrdersSubNeedMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOrdersSubNeedMain
                '------------------采購管理-------------------------------
                'Case "4001" '報價單管理
                '    For Each fr In MDIMain.MdiChildren
                '        If TypeOf fr Is frmQuotationMain Then
                '            fr.Activate()
                '            Exit Sub
                '        End If
                '    Next
                '    fr = New frmQuotationMain
            Case "400101" '報價單管理--大貨批次
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmQuotationDHMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmQuotationDHMain
            Case "400102" '報價單管理--樣辦
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmQuotationYBMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmQuotationYBMain
            Case "400103" '報價單管理--配件批次
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmQuotationPJMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmQuotationPJMain
            Case "400104" '報價單管理--物料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmQuotationWLMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmQuotationWLMain
                'Case "4002" '采購作業
                '    For Each fr In MDIMain.MdiChildren
                '        If TypeOf fr Is frmPurchaseMain Then
                '            fr.Activate()
                '            Exit Sub
                '        End If
                '    Next
                '    fr = New frmPurchaseMain
            Case "400201" '采購作業--大貨批次
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchaseDHMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchaseDHMain
            Case "400202" '采購作業--樣辦
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchaseYBMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchaseYBMain
            Case "400203" '采購作業--配件批次
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchasePJMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchasePJMain
            Case "400204" '采購作業--物料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchaseWLMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchaseWLMain
            Case "4003" '驗收作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmAcceptanceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmAcceptanceMain
            Case "4004" '退貨作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmRetrocedeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmRetrocedeMain
            Case "4005"  '更改單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmChangeMain
            Case "4006" '申購單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmApplyPurchaseMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmApplyPurchaseMain
                'frmApplyPurchaseMain.MdiParent = MDIMain
                'frmApplyPurchaseMain.WindowState = FormWindowState.Maximized
                'frmApplyPurchaseMain.Show()

            Case "4007" '范圍報價
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmQuotationFWMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmQuotationFWMain
            Case "4008" '手開採購單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmManualPurchaseMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmManualPurchaseMain
            Case "40081" '物料品質反饋
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmWareQualityMain Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                'fr = New frmWareQualityMain
                'For Each frmWareQualityMain In MDIMain.MdiChildren
                '    If TypeOf frmWareQualityMain Is frmWareQualityMain Then
                '        frmWareQualityMain.Activate()
                '        Exit Sub
                '    End If
                'Next
                frmWareQualityMain.MdiParent = MDIMain
                frmWareQualityMain.WindowState = FormWindowState.Maximized
                frmWareQualityMain.Show()
            Case "400901"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchaseStatusDHMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchaseStatusDHMain
            Case "400902"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPurchaseStatusPJMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPurchaseStatusPJMain
            Case "401001"

                For Each fr In MDIMain.MdiChildren     '採購交貨進度表
                    If TypeOf fr Is FrmPurchaseReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmPurchaseReport

            Case "401002"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FrmPurchaseReport2 Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmPurchaseReport2

            Case "401003"             '採購匯總表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FrmGroupSelect Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmGroupSelect


                '------------------倉庫管理-------------------------------
            Case "5001" '入庫作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareInputMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareInputMain
            Case "5002" '出庫作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareOutMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareOutMain
            Case "5003" '調拔作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareMoveMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareMoveMain
            Case "5004" '庫存管理
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmWareInventoryMain Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                'fr = New frmWareInventoryMain

                fr = frmWareInventoryMainNew
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub

            Case "5005" '貨架管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareShelvesMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareShelvesMain
            Case "5006" '盤點作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareInventoryCheckMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareInventoryCheckMain


            Case "50061" '倉庫庫存更改單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareChangeMain

            Case "500701" '倉庫拆分
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmWareChaiFen Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                'fr = New frmWareChaiFen
                Dim frm As New frmWareChaiFen1
                frm.ShowDialog()
                Exit Sub
            Case "500702" '倉庫拆合
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareChaiHe Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareChaiHe

            Case "500703"   '倉庫拆合記錄查詢
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareChaiControl Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareChaiControl

            Case "500801" '領取人員白名單

                tempValue3 = "50080101"
                tempValue4 = "倉庫領料白名單"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareHouseSelect Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareHouseSelect
                'Dim myfrm As New frmWareHouseSelect
                'myfrm.ShowDialog()
           

            Case "500802"
                '部門領料價值明細
                tempValue3 = "50080201"
                tempValue4 = "部門領料價值明細"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareOutRptSelect Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareOutRptSelect
            Case "500803"
                '在庫物料單價表
              
                tempValue4 = "50080301"

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareInventorySeek Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareInventorySeek

            Case "500804"
                '部門領料記錄
                tempValue3 = "50080401"
                tempValue4 = "部門領料記錄"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareOutRptSelect Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareOutRptSelect

            Case "500805"
                '部門領料金額匯總
                tempValue3 = "50080501"
                tempValue4 = "部門領料金額匯總"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareOutRptSelect Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareOutRptSelect
            Case "500806"
                tempValue3 = "50080601"
                tempValue4 = "安全庫存設置一覽表"
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmWareInventorySafeRpt Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                'fr = New frmWareInventorySafeRpt
                Dim frm As New frmWareInventorySafeRpt
                frm.ShowDialog()
                Exit Sub
            Case "500807"
                tempValue3 = "50080701"
                tempValue4 = "需申購物料一覽表"
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmWareInventorySafeRpt Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                Dim frm As New frmWareInventorySafeRpt
                frm.ShowDialog()
                Exit Sub
            Case "500808"
                tempValue3 = "50080801"
                tempValue4 = "出庫金額匯總"
                Dim frm As New frmWareOutTotalReport
                frm.ShowDialog()
                Exit Sub
            Case "500809"
                tempValue3 = "50080901"
                tempValue4 = "出庫匯總"
                Dim frm As New frmWareOutTotalReport
                frm.ShowDialog()
                Exit Sub
            Case "500810"
                tempValue3 = "50081001"
                tempValue4 = "停滯物料一覽表"
                Dim frm As New frmWareInventoryHaltRpt
                frm.ShowDialog()
                Exit Sub
            Case "500811"
                tempValue3 = "50081101"
                tempValue4 = "負數出入庫記錄表"
                Dim frm As New frmWareInventoryHaltRpt
                frm.ShowDialog()
                Exit Sub
            Case "500812"
                tempValue3 = "50081201"
                tempValue4 = "收發存匯總"
                Dim frm As New frmWareNumRpt
                frm.ShowDialog()
                Exit Sub
            Case "500813"
                tempValue3 = "50081301"
                tempValue4 = "出庫數據匯總"
                Dim frm As New frmWareInventoryOutRpt
                frm.ShowDialog()
                Exit Sub
            Case "500901"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareQCSendMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareQCSendMain

            Case "500902"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareQCRecoverMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareQCRecoverMain
            Case "500903"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareQCStatusMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareQCStatusMain


            Case "501001"

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareBorrowMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareBorrowMain
            Case "501002"

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareReturnMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareReturnMain

            Case "501003"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareBorrowChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareBorrowChangeMain

            Case "6001"

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMessageMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMessageMain
            Case "6002"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmBroadCastBrigade Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmBroadCastBrigade

                '------------------外發加工------------------------------
            Case "7001"   '外發加工
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutwardMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutwardMain
            Case "7002"   '外發加工驗收
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FormAcceptanceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FormAcceptanceMain
            Case "7003"   '外發返修作業
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmReWorkMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmReWorkMain
            Case "7004" '外發更改單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutwardChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutwardChangeMain

                '7005---外發報表匯總
            Case "700501"    '加工資料詳細查詢
                tempValue3 = "700501"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutwardReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutwardReport
            Case "700502" '供應商送貨資料查詢
                tempValue3 = "700502"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutwardReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutwardReport
            Case "700503" '未交回資料查詢
                tempValue3 = "700503"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutwardReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutwardReport

                '-----------------出廠管理--------------------------------
            Case "8001" '出廠單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmSingleFactoryMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmSingleFactoryMain
                '-----------------送貨管理--------------------------------
            Case "8002"
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is frmDeliveryNoteMain Then
                '        fr.Activate()
                '        Exit Select
                '    End If
                'Next
                'fr = New frmDeliveryNoteMain
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutWardsMain Then
                        fr.Activate()
                        Exit Select
                    End If
                Next
                fr = New frmOutWardsMain
                '------------------工藝流程管理------------------------------
            Case "8501" '產品工藝流程
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPDProductMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPDProductMain
            Case "8502" '客戶投訴管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPDProductComplaintMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPDProductComplaintMain
                '..............................................................

            Case "8801"  '生產計劃

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionScheduleMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionScheduleMain
            Case "88011"  '生產派工單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionBatchAllotMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionBatchAllotMain
            Case "8802" '開料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionKaiLiaoMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionKaiLiaoMain
            Case "88020" '退料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionKaiLiaoReturnMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionFieldReturnMaterialMain

            Case "88021" '工序組合管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionCombinationMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionCombinationMain
            Case "880301"   '生產進度--生產部
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionDetailMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionDetailMain 'frmProductionDetailMainMonthMain

            Case "880302"   '生產進度--總生產(公司)
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionFieldAllMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionFieldAllMain

            Case "880303"   '生產進度--生產部
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionDetailMainMonthMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionDetailMainMonthMain '
            Case "8804"  '物料收發管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionFieldCodeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionFieldCodeMain
            Case "88041"  '物料外發管理--(針對米亞-聯豐,聯豐-米亞生產流轉)
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionOutWardMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionOutWardMain

            Case "88042"  '物料更改單管理
                For Each fr In MDIMain.MdiChildren

                    If TypeOf fr Is frmProductionFieldChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionFieldChangeMain

            Case "8805" '生產倉庫管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionHouseMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionHouseMain
            Case "88051" '生產部門倉庫管理--記錄當前部門實際在產工序數量
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmDepartmentHouseMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmDepartmentHouseMain
            Case "8806" '生產倉庫調撥管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionHouseMoveMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionHouseMoveMain
            Case "8807" '生產倉庫出貨管理
                'For Each fr In MDIMain.MdiChildren
                '    If TypeOf fr Is ProductionWareOutMainA Then
                '        fr.Activate()
                '        Exit Sub
                '    End If
                'Next
                'fr = New ProductionWareOutMainA
                '李超修10.08
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionWareOutMainB Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionWareOutMainB


            Case "8808" '裝配退貨管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionRetrocedeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionRetrocedeMain
            Case "8809" '生產補退貨管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionReturnMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionReturnMain
                'Case "8810"   '物料收發組合信息管理查詢
                '    For Each fr In MDIMain.MdiChildren
                '        If TypeOf fr Is frmProductionMergeMain Then
                '            fr.Activate()
                '            Exit Sub
                '        End If
                '    Next
                '    fr = New frmProductionMergeMain
            Case "8810"   '配件倉庫出貨管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionWareShippedMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionWareShippedMain
            Case "8811" '部門工序結餘數管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionBalanceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionBalanceMain

            Case "8812" '工序加工要求信息
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionOutWardTypeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionOutWardTypeMain

            Case "8813" '申購單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionApplyMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionApplyMain

            Case "8814"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionShipmentMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionShipmentMain

            Case "881501" '生產外發加工管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionOutProcessMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionOutProcessMain

            Case "881502" '生產外發加工驗收管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FrmProductionOWPAcceptanceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmProductionOWPAcceptanceMain
            Case "881503" '生產外發返修管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionOutReturnMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionOutReturnMain

            Case "88150401"
                ''外發加工記錄

                tempValue3 = "88150401"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutReport

            Case "88150402"
                ''外發加工返回記錄
                tempValue3 = "88150402"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutReport

            Case "88150403"
                ''外發加工未完成記錄

                tempValue3 = "88150403"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmOutReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmOutReport

            Case "881601" '生產計件工藝管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPieceProcessMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPieceProcessMain '

            Case "881602" '組別名單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPieceWorkGroupMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPieceWorkGroupMain '
            Case "881603" '員工名單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionPiecePersonnelMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionPiecePersonnelMain

            Case "881604" '員工每日名單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionPiecePersonnelDayMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionPiecePersonnelDayMain

            Case "881605" '個人計件錄入  .
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionSumPiecePersonnelMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionSumPiecePersonnelMain

            Case "881606" '組別計件錄入
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionSumPieceWorkGroupMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionSumPieceWorkGroupMain

            Case "881607" '個人計時錄入
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionSumTimePersonnelMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionSumTimePersonnelMain

            Case "881608" '組別計時錄入

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is ProductionSumTimeWorkGroupMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New ProductionSumTimeWorkGroupMain



            Case "88160901" '計件工序單價表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPieceProcessReport Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPieceProcessReport


                ''
            Case "88160902" '組別名單基本表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is rptProductionPieceWorkGroupPersonnel Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New rptProductionPieceWorkGroupPersonnel

                tempValue = "B"

            Case "88160903" '組別名單每日表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is rptProductionPieceWorkGroupPersonnel Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New rptProductionPieceWorkGroupPersonnel

                tempValue = "D"

            Case "88160904" '個人名單基本表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is rptProductionPiecePersonnel Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New rptProductionPiecePersonnel

                tempValue = "B"

            Case "881610" '件薪計算
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPiecePayMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPiecePayMain

            Case "881611" '薪金工式編輯
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionFormula Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionFormula

            Case "881612" '計件,計時異常
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPieceUnusual Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPieceUnusual
            Case "881613" '計件錄入鎖定
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionSumLockMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionSumLockMain
            Case "881614" '組別薪金調整
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionPiecePayWGAdjustMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionPiecePayWGAdjustMain

            Case "882001"  '記錄報表信息--生產部門完成統計 s

                Dim frm As New frmProductionDeparmentReport
                frm.ShowDialog()
                Exit Sub

            Case "882002"
                Dim frm As New frmProductionDayReport
                frm.ShowDialog()
                Exit Sub
            Case "882003"

                Dim frm As New frmProductionSalesReport
                frm.ShowDialog()
                Exit Sub

                '------------------系統設置------------------------------
            Case "9001" '客戶資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FrmCuster Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmCuster

            Case "9002" '供應商資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmSupplier Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmSupplier

                'frmSuppliersOldRecord
            Case "900201" '供應商資料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmSuppliersOldRecord Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmSuppliersOldRecord

            Case "9003" '幣別匯率設置

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmCurrency Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmCurrency
                'Dim fir As New frmCurrency
                'fir.ShowDialog()
                'Exit Sub

            Case "9005" ''公司信息
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is FrmCompany Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New FrmCompany

                '------------------掃描文件管理------------------------------
            Case "9101" '客戶資料

                tw = New TwainLib.Twain()
                tw.Init(MDIMain.Handle)

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmScanManagerMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmScanManagerMain


            Case "9901" '用戶管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmUserManager Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmUserManager
            Case "9902" '用戶權限管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPermissionModuleUser Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPermissionModuleUser
            Case "9903" '用戶密碼更改
                tempValue = "更改密碼"
                Dim ftt As New FrmUserChange
                ftt.ShowDialog()
                tempValue = ""
                Exit Sub
            Case "9904" '用戶名更改
                tempValue = "更改名稱"
                Dim ftt As New FrmUserChange
                ftt.ShowDialog()
                tempValue = ""
                Exit Sub

            Case "9906" '部門管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmDepartment Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmDepartment
            Case "9907"
                Dim fm As New frmCodeRelated
                fm.ShowDialog()
                Exit Sub
            Case "9908" '設置用戶管理的部門權限
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionUserMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionUserMain
            Case "9909"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalProductionAutoUserMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalProductionAutoUserMain


                'frmProductionProductInner
            Case "9910"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmProductionProductInner Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmProductionProductInner


                ''2013-5-29 實驗室樣辦-----------------------------
            Case "8901"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleOrders Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleOrders
            Case "8902"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleProcess Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleProcess
            Case "8903"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePlan Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePlan
            Case "8904"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePaceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePaceMain
            Case "8905"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleSend Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleSend
            Case "8906"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalCustomerFeedback Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalCustomerFeedback
            Case "8907"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleCollection Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleCollection
            Case "8908"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleTransaction Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleTransaction
            Case "8909"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampInventory Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampInventory
            Case "8910"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePacking Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePacking
            Case "8911"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleSetting
            Case "8912"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleEmailSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleEmailSetting
            Case "8913"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmSampleBorrowMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmSampleBorrowMain
            Case "8914"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleDivert Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleDivert
            Case "8915"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampAlarm Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampAlarm
            Case "8916"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleStorage Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleStorage
                ''-------------------------------------------------
            Case "4801" 'MRP物料编码表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpMaterialCodeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpMaterialCodeMain
            Case "480201" 'Bom表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmBomMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmBomMain
            Case "480202" 'Bom展開
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmBomTree Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmBomTree
            Case "480301" 'MRP计划订单
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpForecastOrderMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpForecastOrderMain
            Case "480302" 'MRP預測订单浏览
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpForecastBrowse Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpForecastBrowse
            Case "480401" 'MRP主生產計劃
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpMpsMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpMpsMain
            Case "480402" 'MRP主生產計劃瀏覽
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpMpsBrowse Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpMpsBrowse
            Case "4805" 'MRP物料需求运算
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpInfoMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpInfoMain
            Case "4806" 'MRP库存记录表
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpWareHouseInfoMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpWareHouseInfoMain
            Case "4807" 'MRP請購申請
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpPurchaseRecordMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpPurchaseRecordMain
            Case "4808" 'MRP采購單
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpPurchaseOrderMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpPurchaseOrderMain
            Case "4809" 'MRP自定義公式
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmFormulaMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmFormulaMain
            Case "4810" 'MRP設置
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmMrpSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmMrpSetting

            Case "5101"

                fr = frmKnifeWareInputMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub

            Case "5102"

                fr = frmKnifeWareOutMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized

                Exit Sub
            Case "5103"

                fr = frmKnifeWareMoveMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub
            Case "5104"

                fr = frmKnifeWareInventoryMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub
            Case "5105"

                fr = frmKnifeBorrowMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub

            Case "5106"

                fr = frmKnifeReturnMain
                fr.Show()
                fr.MdiParent = MDIMain
                fr.WindowState = FormWindowState.Maximized
                Exit Sub

            Case "5107"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmKnifeChangeMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmKnifeChangeMain  'frmKnifeManager

            Case "5108"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmKnifeManager Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmKnifeManager

                ''
            Case "5109"
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmKnifeSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmKnifeSetting

                '--------------------------------------------------------------------------------------------
            Case "9401"             '2014-03-13   姚駿       陽極訂單模塊

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPositiveOrdersMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPositiveOrdersMain

            Case "9402"             '2014-03-13   姚駿      陽極送貨模塊

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmPositiveDeliverMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmPositiveDeliverMain

            Case "5011"             '2014-03-29   姚駿      陽極送貨模塊

                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmWareMoveCompanyMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmWareMoveCompanyMain
                '--------------------------------------------------------------------------------------------
                '貴金屬生產管理

            Case "8601"          '2014-06-17        張偉         樣辦訂單管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleOrders Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleOrders

            Case "8602"          '2014-06-17        張偉         產品資料工藝術流程
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleProcess Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleProcess
            Case "8603"         '2014-06-17        張偉         樣辦排期
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePlan Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePlan
            Case "8604"         '2014-06-17        張偉         樣辦收發
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePaceMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePaceMain
            Case "8605"         '2014-06-17        張偉         樣辦寄送
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleSend Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleSend
            Case "8606"         '2014-06-17        張偉         客戶反饋意見管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalCustomerFeedback Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalCustomerFeedback
            Case "8607"         '2014-06-17        張偉         條碼採集管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleCollection Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleCollection
            Case "8608"         '2014-06-17        張偉         成品狀態管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleTransaction Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleTransaction
            Case "8609"         '2014-06-17        張偉         每日盤點查詢
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleDepWeightChecks Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleDepWeightChecks
            Case "8610"         '2014-06-17        張偉         裝箱管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSamplePacking Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSamplePacking
            Case "8611"         '2014-06-17        張偉         樣板參數設置
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleSetting
            Case "8612"         '2014-06-17        張偉         郵件報警設置
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleEmailSetting Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleEmailSetting
            Case "8613"         '2014-06-17        張偉         生產領退料
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmSampleBorrowMain Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmSampleBorrowMain
            Case "8614"         '2014-06-17        張偉         條碼轉移管理
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleDivert Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleDivert
            Case "8615"         '2014-06-17        張偉         報警信息報務
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampAlarm Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampAlarm
            Case "8616"         '2014-06-17        張偉         貨架信息
                For Each fr In MDIMain.MdiChildren
                    If TypeOf fr Is frmNmetalSampleStorage Then
                        fr.Activate()
                        Exit Sub
                    End If
                Next
                fr = New frmNmetalSampleStorage
                '...............................................................




            Case Else
                '查無此窗口,退出
                Exit Sub
        End Select

        If ModuleID = "40081" Then
            Exit Sub
        End If

        fr.MdiParent = MDIMain
        fr.WindowState = FormWindowState.Maximized
        fr.Show()

    End Sub

End Module
