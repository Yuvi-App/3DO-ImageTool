<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.fbdInputFolder = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnCreateImages = New System.Windows.Forms.Button()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.fbdNewBMPDIR = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnGetInfo = New System.Windows.Forms.Button()
        Me.btnCleanupInfo = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnCreateAnimCEls = New System.Windows.Forms.Button()
        Me.chkbxAnimHeader = New System.Windows.Forms.CheckBox()
        Me.btnCreateANIM = New System.Windows.Forms.Button()
        Me.fbsOrgANIMDir = New System.Windows.Forms.FolderBrowserDialog()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCreateImages
        '
        Me.btnCreateImages.Location = New System.Drawing.Point(40, 51)
        Me.btnCreateImages.Name = "btnCreateImages"
        Me.btnCreateImages.Size = New System.Drawing.Size(133, 23)
        Me.btnCreateImages.TabIndex = 0
        Me.btnCreateImages.Text = "Create CEL's"
        Me.btnCreateImages.UseVisualStyleBackColor = True
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.ItemHeight = 15
        Me.ListBox1.Location = New System.Drawing.Point(12, 11)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(351, 409)
        Me.ListBox1.TabIndex = 1
        '
        'btnGetInfo
        '
        Me.btnGetInfo.Location = New System.Drawing.Point(40, 22)
        Me.btnGetInfo.Name = "btnGetInfo"
        Me.btnGetInfo.Size = New System.Drawing.Size(133, 23)
        Me.btnGetInfo.TabIndex = 2
        Me.btnGetInfo.Text = "Get CEL's Info"
        Me.btnGetInfo.UseVisualStyleBackColor = True
        '
        'btnCleanupInfo
        '
        Me.btnCleanupInfo.Location = New System.Drawing.Point(40, 80)
        Me.btnCleanupInfo.Name = "btnCleanupInfo"
        Me.btnCleanupInfo.Size = New System.Drawing.Size(133, 23)
        Me.btnCleanupInfo.TabIndex = 3
        Me.btnCleanupInfo.Text = "Cleanup CEL's Info"
        Me.btnCleanupInfo.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.btnGetInfo)
        Me.GroupBox1.Controls.Add(Me.btnCleanupInfo)
        Me.GroupBox1.Controls.Add(Me.btnCreateImages)
        Me.GroupBox1.Location = New System.Drawing.Point(369, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(217, 117)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Create CEL's"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(18, 84)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 15)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "3."
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 15)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "2."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(16, 15)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "1."
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnCreateAnimCEls)
        Me.GroupBox2.Controls.Add(Me.chkbxAnimHeader)
        Me.GroupBox2.Controls.Add(Me.btnCreateANIM)
        Me.GroupBox2.Location = New System.Drawing.Point(369, 135)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(217, 114)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Create ANIM"
        '
        'btnCreateAnimCEls
        '
        Me.btnCreateAnimCEls.Location = New System.Drawing.Point(40, 22)
        Me.btnCreateAnimCEls.Name = "btnCreateAnimCEls"
        Me.btnCreateAnimCEls.Size = New System.Drawing.Size(133, 23)
        Me.btnCreateAnimCEls.TabIndex = 2
        Me.btnCreateAnimCEls.Text = "Create CEL's"
        Me.btnCreateAnimCEls.UseVisualStyleBackColor = True
        '
        'chkbxAnimHeader
        '
        Me.chkbxAnimHeader.AutoSize = True
        Me.chkbxAnimHeader.Checked = True
        Me.chkbxAnimHeader.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkbxAnimHeader.Location = New System.Drawing.Point(56, 51)
        Me.chkbxAnimHeader.Name = "chkbxAnimHeader"
        Me.chkbxAnimHeader.Size = New System.Drawing.Size(98, 19)
        Me.chkbxAnimHeader.TabIndex = 1
        Me.chkbxAnimHeader.Text = "ANIM Header"
        Me.chkbxAnimHeader.UseVisualStyleBackColor = True
        '
        'btnCreateANIM
        '
        Me.btnCreateANIM.Location = New System.Drawing.Point(42, 76)
        Me.btnCreateANIM.Name = "btnCreateANIM"
        Me.btnCreateANIM.Size = New System.Drawing.Size(133, 23)
        Me.btnCreateANIM.TabIndex = 0
        Me.btnCreateANIM.Text = "Create ANIM's"
        Me.btnCreateANIM.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(512, 402)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 15)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "Yuviapp v1.0"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(598, 426)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.ListBox1)
        Me.Name = "Form1"
        Me.Text = "3DO - Image Tool"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents fbdInputFolder As FolderBrowserDialog
    Friend WithEvents btnCreateImages As Button
    Friend WithEvents ListBox1 As ListBox
    Friend WithEvents fbdNewBMPDIR As FolderBrowserDialog
    Friend WithEvents btnGetInfo As Button
    Friend WithEvents btnCleanupInfo As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents chkbxAnimHeader As CheckBox
    Friend WithEvents btnCreateANIM As Button
    Friend WithEvents btnCreateAnimCEls As Button
    Friend WithEvents fbsOrgANIMDir As FolderBrowserDialog
    Friend WithEvents Label4 As Label
End Class
