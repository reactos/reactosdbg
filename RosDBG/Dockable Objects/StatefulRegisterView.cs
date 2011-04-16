using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DebugProtocol;
using WeifenLuo.WinFormsUI.Docking;

namespace RosDBG
{
    /// <summary>
    /// Dockable view of general purpose registers, flags and segment registers.
    /// Replaces old RegisterView.
    /// Features automatic highlighting of fields that changed since last instruction.
    /// </summary>
    [DebugControl, BuildAtStartup]
    public partial class StatefulRegisterView : ToolWindow, IUseDebugConnection
    {
        StatefulX86Registers mStatefulRegisters;
        DebugConnection mConnection;
        Color defaultBackColor, highlightColor;

        public StatefulRegisterView()
        {
            InitializeComponent();

            mStatefulRegisters = new StatefulX86Registers();
            defaultBackColor = txtEAX.BackColor;
            highlightColor = Color.Lavender;

            // Register update event handlers
            mStatefulRegisters.EAX.Updated += new EventHandler(EAX_Updated);
            mStatefulRegisters.EBX.Updated += new EventHandler(EBX_Updated);
            mStatefulRegisters.ECX.Updated += new EventHandler(ECX_Updated);
            mStatefulRegisters.EDX.Updated += new EventHandler(EDX_Updated);
            mStatefulRegisters.ESI.Updated += new EventHandler(ESI_Updated);
            mStatefulRegisters.EDI.Updated += new EventHandler(EDI_Updated);

            mStatefulRegisters.EBP.Updated += new EventHandler(EBP_Updated);
            mStatefulRegisters.ESP.Updated += new EventHandler(ESP_Updated);
            mStatefulRegisters.EIP.Updated += new EventHandler(EIP_Updated);

            mStatefulRegisters.CS.Updated += new EventHandler(CS_Updated);
            mStatefulRegisters.DS.Updated += new EventHandler(DS_Updated);
            mStatefulRegisters.ES.Updated += new EventHandler(ES_Updated);
            mStatefulRegisters.SS.Updated += new EventHandler(SS_Updated);
            mStatefulRegisters.FS.Updated += new EventHandler(FS_Updated);
            mStatefulRegisters.GS.Updated += new EventHandler(GS_Updated);

            mStatefulRegisters.CarryFlag.Updated += new EventHandler(CarryFlag_Updated);
            mStatefulRegisters.ParityFlag.Updated += new EventHandler(ParityFlag_Updated);
            mStatefulRegisters.AdjustFlag.Updated += new EventHandler(AdjustFlag_Updated);
            mStatefulRegisters.ZeroFlag.Updated += new EventHandler(ZeroFlag_Updated);
            mStatefulRegisters.SignFlag.Updated += new EventHandler(SignFlag_Updated);
            mStatefulRegisters.TrapFlag.Updated += new EventHandler(TrapFlag_Updated);
            mStatefulRegisters.InterruptEnableFlag.Updated += new EventHandler(InterruptEnableFlag_Updated);
            mStatefulRegisters.DirectionFlag.Updated += new EventHandler(DirectionFlag_Updated);
            mStatefulRegisters.IOPrivilegeLevel.Updated += new EventHandler(IOPrivilegeLevel_Updated);
            mStatefulRegisters.NestedTaskFlag.Updated += new EventHandler(NestedTaskFlag_Updated);
            mStatefulRegisters.ResumeFlag.Updated += new EventHandler(ResumeFlag_Updated);
            mStatefulRegisters.Virtual8086ModeFlag.Updated += new EventHandler(Virtual8086ModeFlag_Updated);
            mStatefulRegisters.AlignmentCheck.Updated += new EventHandler(AlignmentCheck_Updated);
            mStatefulRegisters.VirtualInterruptFlag.Updated += new EventHandler(VirtualInterruptFlag_Updated);
            mStatefulRegisters.VirtualInterruptPending.Updated += new EventHandler(VirtualInterruptPending_Updated);
            mStatefulRegisters.AllowCPUID.Updated += new EventHandler(AllowCPUID_Updated);
        }

        public void SetDebugConnection(DebugConnection conn)
        {
            mConnection = conn;
            mConnection.DebugRegisterChangeEvent += DebugRegisterChangeEvent;
            mConnection.DebugRunningChangeEvent += DebugRunningChangeEvent;
            mConnection.DebugConnectionModeChangedEvent += DebugConnectionModeChangedEvent;
            if (!mConnection.Running)
            {
                mConnection.Debugger.GetRegisterUpdate();
                mConnection.Debugger.GetProcesses();
            }
        }

        #region DebugConnection event handlers
        void DebugConnectionModeChangedEvent(object sender, DebugConnectionModeChangedEventArgs args)
        {
            if (mConnection.ConnectionMode == DebugConnection.Mode.ClosedMode)
                ClearRegisters();
        }

        void DebugRunningChangeEvent(object sender, DebugRunningChangeEventArgs args)
        {
            EnableView(!args.Running);
        }

        void DebugRegisterChangeEvent(object sender, DebugRegisterChangeEventArgs args)
        {
            UpdateView(args.Registers);
        }
        #endregion
        
        #region Field coloring event handlers
        void EAX_Updated(object sender, EventArgs e)
        {
            SetField(txtEAX, mStatefulRegisters.EAX);
        }

        void EBX_Updated(object sender, EventArgs e)
        {
            SetField(txtEBX, mStatefulRegisters.EBX);
        }

        void ECX_Updated(object sender, EventArgs e)
        {
            SetField(txtECX, mStatefulRegisters.ECX);
        }

        void EDX_Updated(object sender, EventArgs e)
        {
            SetField(txtEDX, mStatefulRegisters.EDX);
        }

        void ESI_Updated(object sender, EventArgs e)
        {
            SetField(txtESI, mStatefulRegisters.ESI);
        }

        void EDI_Updated(object sender, EventArgs e)
        {
            SetField(txtEDI, mStatefulRegisters.EDI);
        }

        void EBP_Updated(object sender, EventArgs e)
        {
            SetField(txtEBP, mStatefulRegisters.EBP);
        }

        void ESP_Updated(object sender, EventArgs e)
        {
            SetField(txtESP, mStatefulRegisters.ESP);
        }

        void EIP_Updated(object sender, EventArgs e)
        {
            SetField(txtEIP, mStatefulRegisters.EIP);
        }

        void CS_Updated(object sender, EventArgs e)
        {
            SetField(txtCS, mStatefulRegisters.CS);
        }

        void DS_Updated(object sender, EventArgs e)
        {
            SetField(txtDS, mStatefulRegisters.DS);
        }

        void ES_Updated(object sender, EventArgs e)
        {
            SetField(txtES, mStatefulRegisters.ES);
        }

        void SS_Updated(object sender, EventArgs e)
        {
            SetField(txtSS, mStatefulRegisters.SS);
        }
        
        void FS_Updated(object sender, EventArgs e)
        {
            SetField(txtFS, mStatefulRegisters.FS);
        }

        void GS_Updated(object sender, EventArgs e)
        {
            SetField(txtGS, mStatefulRegisters.GS);
        }

        void CarryFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtCF, mStatefulRegisters.CarryFlag);
        }

        void ParityFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtPF, mStatefulRegisters.ParityFlag);
        }

        void AdjustFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtAF, mStatefulRegisters.AdjustFlag);
        }

        void ZeroFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtZF, mStatefulRegisters.ZeroFlag);
        }

        void SignFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtSF, mStatefulRegisters.SignFlag);
        }

        void TrapFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtTF, mStatefulRegisters.TrapFlag);
        }

        void InterruptEnableFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtIF, mStatefulRegisters.InterruptEnableFlag);
        }

        void DirectionFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtDF, mStatefulRegisters.DirectionFlag);
        }

        void IOPrivilegeLevel_Updated(object sender, EventArgs e)
        {
            SetField(txtIOPL, mStatefulRegisters.IOPrivilegeLevel);
        }

        void NestedTaskFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtNT, mStatefulRegisters.NestedTaskFlag);
        }

        void ResumeFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtRF, mStatefulRegisters.ResumeFlag);
        }

        void Virtual8086ModeFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtVM, mStatefulRegisters.Virtual8086ModeFlag);
        }

        void AlignmentCheck_Updated(object sender, EventArgs e)
        {
            SetField(txtAC, mStatefulRegisters.AlignmentCheck);
        }

        void VirtualInterruptFlag_Updated(object sender, EventArgs e)
        {
            SetField(txtVIF, mStatefulRegisters.VirtualInterruptFlag);
        }

        void VirtualInterruptPending_Updated(object sender, EventArgs e)
        {
            SetField(txtVIP, mStatefulRegisters.VirtualInterruptPending);
        }

        void AllowCPUID_Updated(object sender, EventArgs e)
        {
            SetField(txtID, mStatefulRegisters.AllowCPUID);
        }

        #endregion

        #region GUI manipulation (must run on UI thread)
        void ClearRegisters()
        {
            this.UIThread(delegate
            {
                for (int i = 1; i <= 2; i++) // Could be moved into StatefulX86Registers
                {
                    mStatefulRegisters.EAX.Set(0);
                    mStatefulRegisters.EBX.Set(0);
                    mStatefulRegisters.ECX.Set(0);
                    mStatefulRegisters.EDX.Set(0);
                    mStatefulRegisters.ESI.Set(0);
                    mStatefulRegisters.EDI.Set(0);

                    mStatefulRegisters.EBP.Set(0);
                    mStatefulRegisters.ESP.Set(0);
                    mStatefulRegisters.EIP.Set(0);

                    mStatefulRegisters.CS.Set(0);
                    mStatefulRegisters.DS.Set(0);
                    mStatefulRegisters.ES.Set(0);
                    mStatefulRegisters.SS.Set(0);
                    mStatefulRegisters.FS.Set(0);
                    mStatefulRegisters.GS.Set(0);

                    mStatefulRegisters.Flags.Set(0);
                }
            });
        }

        void EnableView(bool enabled)
        {
            this.UIThread(delegate
            {
                pnlMain.Enabled = enabled;
            });
        }

        void UpdateView(Registers regs)
        {
            this.UIThread(delegate
            {
                if (regs != null)
                {
                    mStatefulRegisters.EIP.Set((UInt32)regs.Eip);

                    mStatefulRegisters.EAX.Set((UInt32)regs.Eax);
                    mStatefulRegisters.EBX.Set((UInt32)regs.Ebx);
                    mStatefulRegisters.ECX.Set((UInt32)regs.Ecx);
                    mStatefulRegisters.EDX.Set((UInt32)regs.Edx);
                    mStatefulRegisters.ESI.Set((UInt32)regs.Esi);
                    mStatefulRegisters.EBP.Set((UInt32)regs.Ebp);
                    mStatefulRegisters.ESP.Set((UInt32)regs.Esp);
                    
                    mStatefulRegisters.CS.Set((UInt16)regs.Cs);
                    mStatefulRegisters.DS.Set((UInt16)regs.Ds);
                    mStatefulRegisters.ES.Set((UInt16)regs.Es);
                    mStatefulRegisters.SS.Set((UInt16)regs.Ss);
                    mStatefulRegisters.FS.Set((UInt16)regs.Fs);
                    mStatefulRegisters.GS.Set((UInt16)regs.Gs);

                    // Triggers update of all the individual flags
                    mStatefulRegisters.Flags.Set((UInt32)regs.Eflags);
                }
            });
        }

        void SetField<T>(TextBox textbox, StatefulVariable<T> val) where T: struct, IComparable
        {
            this.UIThread(delegate
            {
                if (val is StatefulVariable<UInt32>)
                    textbox.Text = string.Format("{0:X8}", val.CurrentValue);

                else if (val is StatefulVariable<UInt16>)
                    textbox.Text = string.Format("{0:X4}", val.CurrentValue);

                else if (val is StatefulVariable<Boolean>)
                    textbox.Text = string.Format("{0}", Convert.ToBoolean(val.CurrentValue) ? "1" : "0");

                else
                    textbox.Text = Convert.ToString(val.CurrentValue);

                if (mStatefulRegisters.EIP.HasChanged)
                    textbox.BackColor = (val.HasChanged ? highlightColor : defaultBackColor);
            });
        }

        #endregion
    }
}
