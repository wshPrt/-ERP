﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKnifeWareOutPutExport
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnOK = New DevExpress.XtraEditors.SimpleButton
        Me.lblExportExcel = New DevExpress.XtraEditors.LabelControl
        Me.DtEditSelect = New DevExpress.XtraEditors.DateEdit
        CType(Me.DtEditSelect.Properties, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Appearance.Font = New System.Drawing.Font("新細明體", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.btnOK.Appearance.Options.UseFont = True
        Me.btnOK.Image = Global.LFERP.My.Resources.Resources.SharingRequestAllow
        Me.btnOK.Location = New System.Drawing.Point(193, 10)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(75, 25)
        Me.btnOK.TabIndex = 164
        Me.btnOK.Text = "確定(&O)"
        '
        'lblExportExcel
        '
        Me.lblExportExcel.Appearance.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExportExcel.Appearance.ForeColor = System.Drawing.Color.Blue
        Me.lblExportExcel.Appearance.Options.UseFont = True
        Me.lblExportExcel.Appearance.Options.UseForeColor = True
        Me.lblExportExcel.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None
        Me.lblExportExcel.Location = New System.Drawing.Point(142, 5)
        Me.lblExportExcel.Name = "lblExportExcel"
        Me.lblExportExcel.Size = New System.Drawing.Size(37, 34)
        Me.lblExportExcel.TabIndex = 163
        Me.lblExportExcel.Text = "導出" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Excel"
        '
        'DtEditSelect
        '
        Me.DtEditSelect.EditValue = Nothing
        Me.DtEditSelect.Location = New System.Drawing.Point(15, 13)
        Me.DtEditSelect.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.DtEditSelect.Name = "DtEditSelect"
        Me.DtEditSelect.Properties.Appearance.Font = New System.Drawing.Font("新細明體", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.DtEditSelect.Properties.Appearance.Options.UseFont = True
        Me.DtEditSelect.Properties.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)})
        Me.DtEditSelect.Properties.Mask.EditMask = "y"
        Me.DtEditSelect.Size = New System.Drawing.Size(120, 22)
        Me.DtEditSelect.TabIndex = 162
        '
        'frmKnifeWareOutPutExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 41)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.lblExportExcel)
        Me.Controls.Add(Me.DtEditSelect)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmKnifeWareOutPutExport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "選擇"
        CType(Me.DtEditSelect.Properties, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOK As DevExpress.XtraEditors.SimpleButton
    Friend WithEvents lblExportExcel As DevExpress.XtraEditors.LabelControl
    Friend WithEvents DtEditSelect As DevExpress.XtraEditors.DateEdit
End Class
