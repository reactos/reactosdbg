using System;
using System.Collections; 
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing.Design;
using System.IO;
using System.IO.Ports;
using System.Globalization;
using DebugProtocol;

namespace RosDBG
{
    /// <summary>
    /// Use this form as a dialog to edit beakpoint data. DialogResult will be OK or Cancel;
    /// if OK, you can get the chosen data from the Breakpoint property.
    /// </summary>
    public partial class EditBreakpointDialog : Form
    {
        /// <summary>
        /// Chosen breakpoint data if OK pressed, or null if Cancel pressed
        /// </summary>
        public Breakpoint Breakpoint { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="breakpoint">Breakpoint data to prefill in the form (skipped if null)</param>
        public EditBreakpointDialog(Breakpoint breakpoint)
        {
            InitializeComponent();
            Breakpoint = breakpoint;
            
            radBPX.CheckedChanged += new EventHandler(radioButtonChanged);
            radBPM.CheckedChanged += new EventHandler(radioButtonChanged);
            radByte.CheckedChanged += new EventHandler(radioButtonChanged);
            radWord.CheckedChanged += new EventHandler(radioButtonChanged);
            radDword.CheckedChanged += new EventHandler(radioButtonChanged);
            radRead.CheckedChanged += new EventHandler(radioButtonChanged);
            radWrite.CheckedChanged += new EventHandler(radioButtonChanged);
            radReadWrite.CheckedChanged += new EventHandler(radioButtonChanged);
            radExecute.CheckedChanged += new EventHandler(radioButtonChanged);

            // Prefill the form with the data from the given breakpoint
            if (breakpoint != null)
            {
                switch (breakpoint.BreakpointType)
                {
                    case DebugProtocol.Breakpoint.BPType.Software:
                        radBPX.Select();
                        break;
                    case DebugProtocol.Breakpoint.BPType.Hardware:
                        radBPM.Select();
                        radExecute.Select();
                        break;
                    case DebugProtocol.Breakpoint.BPType.ReadWatch:
                        radBPM.Select();
                        radRead.Select();
                        break;
                    case DebugProtocol.Breakpoint.BPType.WriteWatch:
                        radBPM.Select();
                        radWrite.Select();
                        break;
                    case DebugProtocol.Breakpoint.BPType.AccessWatch:
                        radBPM.Select();
                        radReadWrite.Select();
                        break;
                }

                if (breakpoint.BreakpointType != DebugProtocol.Breakpoint.BPType.Software)
                {
                    switch (breakpoint.Length)
                    {
                        case 1: radByte.Select(); break;
                        case 2: radWord.Select(); break;
                        case 4: radDword.Select(); break;
                    }
                }

                txtAddress.Text = breakpoint.Address.ToString("X8");
                txtCondition.Text = breakpoint.Condition;
            }

        }

        #region Event handlers
        private void EditBreakpoint_Activated(object sender, EventArgs e)
        {
            ValidateForm();
            txtAddress.Focus();
            txtAddress.SelectAll();
        }

        private void radioButtonChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
                ValidateForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Breakpoint = null;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Breakpoint == null)
                    Breakpoint = new Breakpoint(-1);

                Breakpoint.Enabled = false;
                Breakpoint.Address = ulong.Parse(txtAddress.Text, NumberStyles.HexNumber);
                Breakpoint.Condition = txtCondition.Text;

                if (radBPX.Checked)
                {
                    Breakpoint.BreakpointType = DebugProtocol.Breakpoint.BPType.Software;
                    Breakpoint.Length = 1;
                }
                else if (radBPM.Checked)
                {
                    if (radRead.Checked)
                        Breakpoint.BreakpointType = DebugProtocol.Breakpoint.BPType.ReadWatch;
                    else if (radWrite.Checked)
                        Breakpoint.BreakpointType = DebugProtocol.Breakpoint.BPType.WriteWatch;
                    else if (radReadWrite.Checked)
                        Breakpoint.BreakpointType = DebugProtocol.Breakpoint.BPType.AccessWatch;
                    else if (radExecute.Checked)
                        Breakpoint.BreakpointType = DebugProtocol.Breakpoint.BPType.Hardware;

                    if (radByte.Checked) Breakpoint.Length = 1;
                    else if (radWord.Checked) Breakpoint.Length = 2;
                    else if (radDword.Checked) Breakpoint.Length = 4;
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception) { } // If something goes wrong, don't close
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            ulong answer;
            if (ulong.TryParse(txtAddress.Text, NumberStyles.HexNumber, null, out answer))
            {
                e.Cancel = false;
                txtAddress.BackColor = SystemColors.Window;
            }
            else
            {
                e.Cancel = true;
                txtAddress.BackColor = Color.Yellow;
            }
        }

        #endregion

        #region Utility methods
        private void ValidateForm()
        {
            bool addressFocused = txtAddress.Focused;
            gbxScope.Enabled = radBPM.Checked;
            gbxTrigger.Enabled = radBPM.Checked;
            if (radExecute.Checked) radByte.Select();
            if (addressFocused) txtAddress.Focus();
        }
        #endregion
    }
}
