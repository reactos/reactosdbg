namespace RosDBG
{
    partial class BreakpointWindow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BreakpointWindow));
            this.grid = new System.Windows.Forms.DataGridView();
            this.columnEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.columnID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnCondition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.breakpointBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnAddBreakpoint = new System.Windows.Forms.Button();
            this.btnDeleteBreakpoint = new System.Windows.Forms.Button();
            this.btnEditBreakpoint = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.breakpointBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.AutoGenerateColumns = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.columnEnabled,
            this.columnID,
            this.columnAddress,
            this.columnType,
            this.columnCondition});
            this.grid.DataSource = this.breakpointBindingSource;
            this.grid.Dock = System.Windows.Forms.DockStyle.Top;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(318, 228);
            this.grid.StandardTab = true;
            this.grid.TabIndex = 1;
            this.grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellDoubleClick);
            this.grid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_CellFormatting);
            this.grid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_CellValueChanged);
            this.grid.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
            // 
            // columnEnabled
            // 
            this.columnEnabled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.columnEnabled.DataPropertyName = "Enabled";
            this.columnEnabled.HeaderText = "";
            this.columnEnabled.Name = "columnEnabled";
            this.columnEnabled.Width = 5;
            // 
            // columnID
            // 
            this.columnID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.columnID.DataPropertyName = "ID";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.columnID.DefaultCellStyle = dataGridViewCellStyle1;
            this.columnID.HeaderText = "ID";
            this.columnID.Name = "columnID";
            this.columnID.ReadOnly = true;
            this.columnID.Width = 5;
            // 
            // columnAddress
            // 
            this.columnAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnAddress.DataPropertyName = "Address";
            dataGridViewCellStyle2.Format = "X8";
            this.columnAddress.DefaultCellStyle = dataGridViewCellStyle2;
            this.columnAddress.HeaderText = "Address";
            this.columnAddress.Name = "columnAddress";
            this.columnAddress.ReadOnly = true;
            this.columnAddress.Width = 70;
            // 
            // columnType
            // 
            this.columnType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnType.DataPropertyName = "BreakpointType";
            dataGridViewCellStyle3.NullValue = null;
            this.columnType.DefaultCellStyle = dataGridViewCellStyle3;
            this.columnType.HeaderText = "Type";
            this.columnType.Name = "columnType";
            this.columnType.ReadOnly = true;
            this.columnType.Width = 56;
            // 
            // columnCondition
            // 
            this.columnCondition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.columnCondition.DataPropertyName = "Condition";
            this.columnCondition.HeaderText = "Condition";
            this.columnCondition.Name = "columnCondition";
            this.columnCondition.ReadOnly = true;
            this.columnCondition.Width = 76;
            // 
            // breakpointBindingSource
            // 
            this.breakpointBindingSource.DataSource = typeof(DebugProtocol.Breakpoint);
            // 
            // btnAddBreakpoint
            // 
            this.btnAddBreakpoint.Enabled = false;
            this.btnAddBreakpoint.Location = new System.Drawing.Point(12, 234);
            this.btnAddBreakpoint.Name = "btnAddBreakpoint";
            this.btnAddBreakpoint.Size = new System.Drawing.Size(48, 23);
            this.btnAddBreakpoint.TabIndex = 2;
            this.btnAddBreakpoint.Text = "&Add...";
            this.btnAddBreakpoint.UseVisualStyleBackColor = true;
            this.btnAddBreakpoint.Click += new System.EventHandler(this.btnAddBreakpoint_Click);
            // 
            // btnDeleteBreakpoint
            // 
            this.btnDeleteBreakpoint.Enabled = false;
            this.btnDeleteBreakpoint.Location = new System.Drawing.Point(66, 234);
            this.btnDeleteBreakpoint.Name = "btnDeleteBreakpoint";
            this.btnDeleteBreakpoint.Size = new System.Drawing.Size(48, 23);
            this.btnDeleteBreakpoint.TabIndex = 4;
            this.btnDeleteBreakpoint.Text = "&Delete";
            this.btnDeleteBreakpoint.UseVisualStyleBackColor = true;
            this.btnDeleteBreakpoint.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEditBreakpoint
            // 
            this.btnEditBreakpoint.Enabled = false;
            this.btnEditBreakpoint.Location = new System.Drawing.Point(120, 234);
            this.btnEditBreakpoint.Name = "btnEditBreakpoint";
            this.btnEditBreakpoint.Size = new System.Drawing.Size(48, 23);
            this.btnEditBreakpoint.TabIndex = 5;
            this.btnEditBreakpoint.Text = "&Edit...";
            this.btnEditBreakpoint.UseVisualStyleBackColor = true;
            this.btnEditBreakpoint.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // BreakpointWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 262);
            this.Controls.Add(this.btnEditBreakpoint);
            this.Controls.Add(this.btnDeleteBreakpoint);
            this.Controls.Add(this.btnAddBreakpoint);
            this.Controls.Add(this.grid);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BreakpointWindow";
            this.Text = "Breakpoints";
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.breakpointBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnAddBreakpoint;
        private System.Windows.Forms.BindingSource breakpointBindingSource;
        private System.Windows.Forms.Button btnDeleteBreakpoint;
        private System.Windows.Forms.Button btnEditBreakpoint;
        private System.Windows.Forms.DataGridViewCheckBoxColumn columnEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnID;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnCondition;

    }
}
