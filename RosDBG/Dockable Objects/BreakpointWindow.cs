using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;
using DebugProtocol;
using WeifenLuo.WinFormsUI.Docking;

namespace RosDBG
{
    [DebugControl]
    public partial class BreakpointWindow : ToolWindow, IUseDebugConnection
    {
        DebugConnection mConnection;
        bool mRunning = true;
        ulong last_EIP;
        IList<Breakpoint> mBreakpoints;
        
        public BreakpointWindow()
        {
            InitializeComponent();
            grid.DataSource = mBreakpoints;
        }

        public void SetDebugConnection(DebugConnection conn)
        {
            mConnection = conn;
            conn.DebugRunningChangeEvent += DebugRunningChangeEvent;
            conn.DebugRegisterChangeEvent += new DebugRegisterChangeEventHandler(DebugRegisterChangeEvent);
            conn.DebugBreakpointChangeEvent += new DebugBreakpointChangeEventHandler(DebugBreakpointChangeEvent);
            mRunning = conn.Running;
            if (!mConnection.Running)
                mConnection.Debugger.GetBreakpoints();
        }

        #region Debugger events
        void DebugRegisterChangeEvent(object sender, DebugRegisterChangeEventArgs args)
        {
            last_EIP = args.Registers.Eip;
        }

        void DebugBreakpointChangeEvent(object sender, DebugBreakpointChangeEventArgs args)
        {
            mBreakpoints = args.Breakpoints;
            RefreshView();
        }

        void DebugRunningChangeEvent(object sender, DebugRunningChangeEventArgs args)
        {
            this.UIThread(delegate
            {
                mRunning = args.Running;
                grid.Enabled = !mRunning;
                btnAddBreakpoint.Enabled = !mRunning;
                btnDeleteBreakpoint.Enabled = !mRunning;
                btnEditBreakpoint.Enabled = !mRunning;
            });
        }
        #endregion

        #region Utility methods
        private void RefreshView()
        {
            this.UIThread(delegate
            {
                if (!mRunning)
                {
                    // Track selected row
                    DataGridViewSelectedRowCollection sel_rows = grid.SelectedRows;
                    int sel_row = (sel_rows.Count >= 1 ? sel_rows[0].Index : 0);

                    grid.DataSource = null;
                    grid.DataSource = mBreakpoints;
                    grid.Refresh();
                    
                    // Restore selected row, if possible
                    if (sel_row < grid.RowCount) grid.Rows[sel_row].Selected = true;
                }
            });
        }

        private string GetLengthString(int length)
        {
            switch (length)
            {
                case 1: return "byte";
                case 2: return "word";
                case 4: return "dword";
                default: return "";
            }
        }

        private void EditBreakpoint(Breakpoint storedBreakpoint)
        {
            using (EditBreakpointDialog dialog = new EditBreakpointDialog(storedBreakpoint))
            {
                dialog.ShowDialog();
                if (dialog.DialogResult == DialogResult.OK && dialog.Breakpoint != null)
                {
                    Breakpoint bp = dialog.Breakpoint;
                    if (storedBreakpoint.ID >= 0)
                        mConnection.Debugger.RemoveBreakpoint(storedBreakpoint.ID);
                    mConnection.Debugger.SetBreakpoint(bp);
                }
            }
        }

        #endregion

        #region User interface event handlers

        // See the note in the Remarks section at the MSDN entry for DataGridViewCheckboxColumn
        private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grid.CurrentCell is DataGridViewCheckBoxCell)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count && e.ColumnIndex == this.columnEnabled.Index)
            {
                DataGridViewCheckBoxCell cell = grid[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
                if (cell != null && !mConnection.Running)
                {
                    Breakpoint bp = grid.Rows[e.RowIndex].DataBoundItem as Breakpoint;
                    if ((bool)cell.Value)
                        mConnection.Debugger.EnableBreakpoint(bp.ID);
                    else
                        mConnection.Debugger.DisableBreakpoint(bp.ID);
                }
            }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < grid.Rows.Count && e.RowIndex < mBreakpoints.Count)
            {
                Breakpoint storedBreakpoint = mBreakpoints[e.RowIndex];
                EditBreakpoint(storedBreakpoint);
            }
        }

        private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Breakpoint bp = grid.Rows[e.RowIndex].DataBoundItem as Breakpoint;
            if (e.ColumnIndex == columnType.Index)
            {
                switch (bp.BreakpointType)
                {
                    case Breakpoint.BPType.Software: e.Value = "execute"; break;
                    case Breakpoint.BPType.Hardware: e.Value = "exec " + GetLengthString(bp.Length); break;
                    case Breakpoint.BPType.ReadWatch: e.Value = "read " +GetLengthString(bp.Length); break;
                    case Breakpoint.BPType.WriteWatch: e.Value = "write " + GetLengthString(bp.Length); break;
                    case Breakpoint.BPType.AccessWatch: e.Value = "r/w " + GetLengthString(bp.Length); break;
                    default: e.Value = "?"; break;
                }
            }
        }

        private void btnAddBreakpoint_Click(object sender, EventArgs e)
        {
            Breakpoint orig_bp = new Breakpoint(-1, Breakpoint.BPType.Software, last_EIP, 1, "");
            EditBreakpoint(orig_bp);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count >= 1)
            {   
                Breakpoint bp = grid.SelectedRows[0].DataBoundItem as Breakpoint;
                mConnection.Debugger.RemoveBreakpoint(bp.ID);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (grid.SelectedRows.Count >= 1)
            {
                Breakpoint storedBreakpoint = grid.SelectedRows[0].DataBoundItem as Breakpoint;
                EditBreakpoint(storedBreakpoint);
            }
        }
        #endregion
    }
}
