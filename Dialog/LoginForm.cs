using System;
using System.Windows.Forms;
using System.Collections.Generic;
using WFExceptions;
using WFDatabase;
using GeneLibrary.Items;
using GeneLibrary.Common;
using System.IO;
using System.Linq;


namespace GeneLibrary.Dialog
{
    public partial class LogOnForm : Form
    {
        // Constructors
        public LogOnForm()
        {
            InitializeComponent();
        }

        // Events handers
        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Properties.Settings.Default.defServer;
            userList = (SaveList)WFSerialize.Deserialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\users.dmp");

            if (userList != null)
            {
                List<string> sl = new List<string>();
                foreach (SaveItem si in userList.ListItems)
                {
                    cmsUsers.Items.Add(si.Name);
                    sl.Add(si.Name);
                }
                textBoxLogin.AutoCompleteCustomSource.AddRange(sl.ToArray());
            }
            else
            {
                userList = new SaveList();
            }

        }
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                // Проверка заполненности текстовых полей формы
                if (String.IsNullOrEmpty(textBoxLogin.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyLogin);
                if (String.IsNullOrEmpty(textBoxServer.Text.Trim()))
                    throw new WFException(ErrType.Message, ErrorsMsg.EmptyServer);

                // Инициируем новое соединение с базой
                LogOnBase logonBase = GateFactory.LogOnBase();
                if (GateFactory.DB().IsActive())
                    logonBase.ReEnter(textBoxLogin.Text, textBoxPassword.Text, textBoxServer.Text);
                else
                {
                    GateFactory.DB().Close();
                    logonBase.Enter(textBoxLogin.Text, textBoxPassword.Text, textBoxServer.Text);
                }

                Tools.MainForm().RefreshMenu();

                SaveItem si = new SaveItem(logonBase.Id, logonBase.LogOn, logonBase.LogOn);
                userList.Add(si);
                WFSerialize.Serialize(Path.GetDirectoryName(Application.ExecutablePath) + "\\users.dmp", userList);
            }
            catch (WFException)
            {
                throw;
            }
            catch (Exception err)
            {
                throw new WFException(ErrType.Message, ErrorsMsg.ConnectError, err);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

            this.DialogResult = DialogResult.OK;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnOthers_Click(object sender, EventArgs e)
        {
            cmsUsers.Show(btnOthers, 0, btnOthers.Height + 2);
        }
        private void cmsUsers_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            this.textBoxLogin.Text = e.ClickedItem.Text;
            SetFocusOnTextBoxPassword();
        }
        private void LogOnForm_KeyDown(object sender, KeyEventArgs e)
        {
            bool legalControl = textBoxPassword.Focused || textBoxServer.Focused;

            if (e.KeyCode == Keys.Enter && legalControl)
                btnConnect_Click(null, null);
            else if (e.KeyCode == Keys.Enter && textBoxLogin.Focused)
                SetFocusOnTextBoxPassword();
        }

        // Private methods
        private void SetFocusOnTextBoxPassword()
        {
            if (textBoxPassword.CanFocus)
                textBoxPassword.Focus();
        }

        // Private members
        private SaveList userList;

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

    }
}
