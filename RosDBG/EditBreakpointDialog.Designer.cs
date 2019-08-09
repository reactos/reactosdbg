namespace RosDBG
{
    partial class EditBreakpointDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtCondition = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.gbxScope = new System.Windows.Forms.GroupBox();
            this.radDword = new System.Windows.Forms.RadioButton();
            this.radWord = new System.Windows.Forms.RadioButton();
            this.radByte = new System.Windows.Forms.RadioButton();
            this.gbxTrigger = new System.Windows.Forms.GroupBox();
            this.radExecute = new System.Windows.Forms.RadioButton();
            this.radReadWrite = new System.Windows.Forms.RadioButton();
            this.radWrite = new System.Windows.Forms.RadioButton();
            this.radRead = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.gbxType = new System.Windows.Forms.GroupBox();
            this.radBPM = new System.Windows.Forms.RadioButton();
            this.radBPX = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.gbxScope.SuspendLayout();
            this.gbxTrigger.SuspendLayout();
            this.gbxType.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtCondition);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtAddress);
            this.splitContainer1.Panel1.Controls.Add(this.gbxScope);
            this.splitContainer1.Panel1.Controls.Add(this.gbxTrigger);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.gbxType);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnOK);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Size = new System.Drawing.Size(330, 200);
            this.splitContainer1.SplitterDistance = 167;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtCondition
            // 
            this.txtCondition.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCondition.Location = new System.Drawing.Point(82, 135);
            this.txtCondition.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.txtCondition.Name = "txtCondition";
            this.txtCondition.Size = new System.Drawing.Size(239, 20);
            this.txtCondition.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Condition:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(81, 99);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.txtAddress.MaxLength = 8;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(58, 20);
            this.txtAddress.TabIndex = 1;
            this.txtAddress.Validating += new System.ComponentModel.CancelEventHandler(this.txtAddress_Validating);
            // 
            // gbxScope
            // 
            this.gbxScope.Controls.Add(this.radDword);
            this.gbxScope.Controls.Add(this.radWord);
            this.gbxScope.Controls.Add(this.radByte);
            this.gbxScope.Location = new System.Drawing.Point(251, 12);
            this.gbxScope.Name = "gbxScope";
            this.gbxScope.Size = new System.Drawing.Size(70, 117);
            this.gbxScope.TabIndex = 4;
            this.gbxScope.TabStop = false;
            this.gbxScope.Text = "Scope";
            // 
            // radDword
            // 
            this.radDword.AutoSize = true;
            this.radDword.Location = new System.Drawing.Point(6, 65);
            this.radDword.Name = "radDword";
            this.radDword.Size = new System.Drawing.Size(59, 17);
            this.radDword.TabIndex = 2;
            this.radDword.Text = "DWord";
            this.radDword.UseVisualStyleBackColor = true;
            // 
            // radWord
            // 
            this.radWord.AutoSize = true;
            this.radWord.Location = new System.Drawing.Point(6, 42);
            this.radWord.Name = "radWord";
            this.radWord.Size = new System.Drawing.Size(51, 17);
            this.radWord.TabIndex = 1;
            this.radWord.Text = "Word";
            this.radWord.UseVisualStyleBackColor = true;
            // 
            // radByte
            // 
            this.radByte.AutoSize = true;
            this.radByte.Checked = true;
            this.radByte.Location = new System.Drawing.Point(6, 19);
            this.radByte.Name = "radByte";
            this.radByte.Size = new System.Drawing.Size(46, 17);
            this.radByte.TabIndex = 0;
            this.radByte.TabStop = true;
            this.radByte.Text = "Byte";
            this.radByte.UseVisualStyleBackColor = true;
            // 
            // gbxTrigger
            // 
            this.gbxTrigger.Controls.Add(this.radExecute);
            this.gbxTrigger.Controls.Add(this.radReadWrite);
            this.gbxTrigger.Controls.Add(this.radWrite);
            this.gbxTrigger.Controls.Add(this.radRead);
            this.gbxTrigger.Location = new System.Drawing.Point(148, 12);
            this.gbxTrigger.Name = "gbxTrigger";
            this.gbxTrigger.Size = new System.Drawing.Size(97, 117);
            this.gbxTrigger.TabIndex = 3;
            this.gbxTrigger.TabStop = false;
            this.gbxTrigger.Text = "Trigger";
            // 
            // radExecute
            // 
            this.radExecute.AutoSize = true;
            this.radExecute.Location = new System.Drawing.Point(6, 88);
            this.radExecute.Name = "radExecute";
            this.radExecute.Size = new System.Drawing.Size(72, 17);
            this.radExecute.TabIndex = 3;
            this.radExecute.Text = "Execution";
            this.radExecute.UseVisualStyleBackColor = true;
            // 
            // radReadWrite
            // 
            this.radReadWrite.AutoSize = true;
            this.radReadWrite.Location = new System.Drawing.Point(6, 65);
            this.radReadWrite.Name = "radReadWrite";
            this.radReadWrite.Size = new System.Drawing.Size(81, 17);
            this.radReadWrite.TabIndex = 2;
            this.radReadWrite.Text = "Read/Write";
            this.radReadWrite.UseVisualStyleBackColor = true;
            // 
            // radWrite
            // 
            this.radWrite.AutoSize = true;
            this.radWrite.Location = new System.Drawing.Point(6, 42);
            this.radWrite.Name = "radWrite";
            this.radWrite.Size = new System.Drawing.Size(50, 17);
            this.radWrite.TabIndex = 1;
            this.radWrite.Text = "Write";
            this.radWrite.UseVisualStyleBackColor = true;
            // 
            // radRead
            // 
            this.radRead.AutoSize = true;
            this.radRead.Checked = true;
            this.radRead.Location = new System.Drawing.Point(6, 19);
            this.radRead.Name = "radRead";
            this.radRead.Size = new System.Drawing.Size(51, 17);
            this.radRead.TabIndex = 0;
            this.radRead.TabStop = true;
            this.radRead.Text = "Read";
            this.radRead.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Address:";
            // 
            // gbxType
            // 
            this.gbxType.Controls.Add(this.radBPM);
            this.gbxType.Controls.Add(this.radBPX);
            this.gbxType.Location = new System.Drawing.Point(12, 12);
            this.gbxType.Name = "gbxType";
            this.gbxType.Size = new System.Drawing.Size(130, 73);
            this.gbxType.TabIndex = 2;
            this.gbxType.TabStop = false;
            this.gbxType.Text = "Type";
            // 
            // radBPM
            // 
            this.radBPM.AutoSize = true;
            this.radBPM.Location = new System.Drawing.Point(9, 44);
            this.radBPM.Name = "radBPM";
            this.radBPM.Size = new System.Drawing.Size(103, 17);
            this.radBPM.TabIndex = 1;
            this.radBPM.Text = "Hardware (BPM)";
            this.radBPM.UseVisualStyleBackColor = true;
            // 
            // radBPX
            // 
            this.radBPX.AutoSize = true;
            this.radBPX.Checked = true;
            this.radBPX.Location = new System.Drawing.Point(9, 21);
            this.radBPX.Name = "radBPX";
            this.radBPX.Size = new System.Drawing.Size(97, 17);
            this.radBPX.TabIndex = 0;
            this.radBPX.TabStop = true;
            this.radBPX.Text = "Software (BPX)";
            this.radBPX.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(252, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(171, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // EditBreakpointDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(330, 200);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditBreakpointDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add or edit breakpoint";
            this.Activated += new System.EventHandler(this.EditBreakpoint_Activated);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.gbxScope.ResumeLayout(false);
            this.gbxScope.PerformLayout();
            this.gbxTrigger.ResumeLayout(false);
            this.gbxTrigger.PerformLayout();
            this.gbxType.ResumeLayout(false);
            this.gbxType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.GroupBox gbxScope;
        private System.Windows.Forms.RadioButton radDword;
        private System.Windows.Forms.RadioButton radWord;
        private System.Windows.Forms.RadioButton radByte;
        private System.Windows.Forms.GroupBox gbxTrigger;
        private System.Windows.Forms.RadioButton radExecute;
        private System.Windows.Forms.RadioButton radReadWrite;
        private System.Windows.Forms.RadioButton radWrite;
        private System.Windows.Forms.RadioButton radRead;
        private System.Windows.Forms.GroupBox gbxType;
        private System.Windows.Forms.RadioButton radBPM;
        private System.Windows.Forms.RadioButton radBPX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCondition;
        private System.Windows.Forms.Button btnOK;
    }
}
