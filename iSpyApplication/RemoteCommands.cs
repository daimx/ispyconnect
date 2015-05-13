using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class RemoteCommands : Form
    {
        public RemoteCommands()
        {
            InitializeComponent();
            RenderResources();
        }

        private void ManualAlertsLoad(object sender, EventArgs e)
        {
            RenderCommands();

            if (lbManualAlerts.Items.Count > 0)
                lbManualAlerts.SelectedIndex = 0;
        }

        private void RenderResources()
        {
            Text = LocRm.GetString("RemoteCommands");
            btnAddCommand.Text = LocRm.GetString("Add");
            btnDelete.Text = LocRm.GetString("Delete");            
            label45.Text = LocRm.GetString("forExamples");
            label82.Text = LocRm.GetString("YouCanTriggerRemoteComman");
            linkLabel3.Text = LocRm.GetString("Reset");
            
        }


        private void RenderCommands()
        {
            lbManualAlerts.Items.Clear();
            foreach (objectsCommand oc in MainForm.RemoteCommands)
            {
                string n = oc.name;
                if (n.StartsWith("cmd_"))
                {
                    n = LocRm.GetString(oc.name);
                }
                lbManualAlerts.Items.Add(oc.id + ": " + n);
            }
        }

        private void BtnAddCommandClick(object sender, EventArgs e)
        {
            using (var arc = new AddRemoteCommand())
            {
                if (arc.ShowDialog(this) == DialogResult.OK)
                {
                    RenderCommands();
                }
                
            }           
        }

        private void BtnDeleteClick(object sender, EventArgs e)
        {
            if (lbManualAlerts.SelectedIndex > -1)
            {
                string al = lbManualAlerts.SelectedItem.ToString();
                al = al.Substring(0, al.IndexOf(":", StringComparison.Ordinal)).Trim();
                objectsCommand oc = MainForm.RemoteCommands.FirstOrDefault(p => p.id == Convert.ToInt32(al));
                if (oc != null)
                {
                    MainForm.RemoteCommands.Remove(oc);
                    RenderCommands();
                }
            }
        }

        private void lbManualAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbManualAlerts.SelectedIndex>-1)
            {
               string al = lbManualAlerts.SelectedItem.ToString();
                al = al.Substring(0, al.IndexOf(":", StringComparison.Ordinal)).Trim();
                objectsCommand oc = MainForm.RemoteCommands.FirstOrDefault(p => p.id == Convert.ToInt32(al));
                if (oc != null)
                {
                    string s = oc.command;
                    if (!String.IsNullOrEmpty(oc.emitshortcut))
                    {
                        if (oc.emitshortcut != "")
                            s = oc.emitshortcut + " & " + oc.command;
                    }
                    lblCommand.Text = s;
                }

            }
        }
        
        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MessageBox.Show(LocRm.GetString("AreYouSure"), LocRm.GetString("Confirm"), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            MainForm.InitRemoteCommands();
            RenderCommands();
        }
    }
}