// Jaime Borges 2015
// AdvancedWebBrowser - Windows Control
// Based on code by Goga Claudia - WBrowser 2009

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml;

namespace AdvancedWebBrowser
{
    public partial class AdvancedWebBrowser: UserControl
    {
        string settingsXml = "settings.xml", historyXml = "history.xml";
        List<String> urls = new List<String>();
        XmlDocument settings = new XmlDocument();
        String homePage;
        CultureInfo currentCulture;
        private WebBrowser wb;

        private WebBrowser CreateWebBrowser(string URL)
        {
            WebBrowser browser = new WebBrowser();
            browser.ScriptErrorsSuppressed = true;
            browser.WebBrowserShortcutsEnabled = true;
            browser.Navigate(URL);
            browser.Dock = DockStyle.Fill;
            browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(AWB_ProgressChanged);
            browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(AWB_DocumentCompleted);
            browser.Navigating += new WebBrowserNavigatingEventHandler(AWB_Navigating);
            browser.CanGoBackChanged += new EventHandler(browser_CanGoBackChanged);
            browser.CanGoForwardChanged += new EventHandler(browser_CanGoForwardChanged);
            return browser;
        }

        public WebBrowser ActiveWebBrowser
        {
            get
            { return wb; }
            set
            { wb = value; }
        }

        public AdvancedWebBrowser()
        {
            InitializeComponent();
        }

        #region Form load/Closing/Closed

        //visible items
        private void setVisibility()
        {
            if (!File.Exists(settingsXml))
            {
                XmlElement r = settings.CreateElement("settings");
                settings.AppendChild(r);
                XmlElement el;

                el = settings.CreateElement("menuBar");
                el.SetAttribute("visible", "True");
                r.AppendChild(el);

                el = settings.CreateElement("adrBar");
                el.SetAttribute("visible", "True");
                r.AppendChild(el);

                el = settings.CreateElement("linkBar");
                el.SetAttribute("visible", "True");
                r.AppendChild(el);

                el = settings.CreateElement("favoritesPanel");
                el.SetAttribute("visible", "True");
                r.AppendChild(el);

                el = settings.CreateElement("SplashScreen");
                el.SetAttribute("checked", "True");
                r.AppendChild(el);

                el = settings.CreateElement("homepage");
                el.InnerText = "about:blank";
                r.AppendChild(el);

                el = settings.CreateElement("dropdown");
                el.InnerText = "15";
                r.AppendChild(el);
            }
            else
            {
                settings.Load(settingsXml);
                XmlElement r = settings.DocumentElement;
                menuBar.Visible = (r.ChildNodes[0].Attributes[0].Value.Equals("True"));
                adrBar.Visible = (r.ChildNodes[1].Attributes[0].Value.Equals("True"));
                homePage = r.ChildNodes[5].InnerText;
            }


            this.menuBarToolStripMenuItem.Checked = menuBar.Visible;
            this.commandBarToolStripMenuItem.Checked = adrBar.Visible;
            homePage = settings.DocumentElement.ChildNodes[5].InnerText;
        }

        private void AdvancedWebBrowser_Load(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Done";
            setVisibility();
            CheckForExistingEmulationOptons();
            addNewTab();
        }

        		//form closing
		private void AdvancedWebBrowser_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (browserTabControl.TabCount != 2)
			{
				DialogResult dlg_res = (new Close()).ShowDialog();

				if (dlg_res == DialogResult.No) { e.Cancel = true; closeTab(); }
				else if (dlg_res == DialogResult.Cancel) e.Cancel = true;
				else Application.ExitThread();
			}
		}

		//form closed
        private void AdvancedWebBrowser_FormClosed(object sender, FormClosedEventArgs e)
        {
            settings.Save(settingsXml);
            File.Delete("source.txt");
        }

        #endregion

        #region FAVORITES,LINKS,HISTORY METHODS

        //addHistory method
        private void addHistory(Uri url, string data)
        {
            XmlDocument myXml = new XmlDocument();
            int i = 1;
            XmlElement el = myXml.CreateElement("item");
            el.SetAttribute("url", url.ToString());
            el.SetAttribute("lastVisited", data);

            if (!File.Exists(historyXml))
            {
                XmlElement root = myXml.CreateElement("history");
                myXml.AppendChild(root);
                el.SetAttribute("times", "1");
                root.AppendChild(el);
            }
            else
            {
                myXml.Load(historyXml);

                foreach (XmlElement x in myXml.DocumentElement.ChildNodes)
                {
                    if (x.GetAttribute("url").Equals(url.ToString()))
                    {
                        i = int.Parse(x.GetAttribute("times")) + 1;
                        myXml.DocumentElement.RemoveChild(x);
                        break;
                    }
                }

                el.SetAttribute("times", i.ToString());
                myXml.DocumentElement.InsertBefore(el, myXml.DocumentElement.FirstChild);
            }

            myXml.Save(historyXml);
        }

        #endregion

        #region TABURI
        /*TAB-uri*/

        //addNewTab method
        private void addNewTab()
        {
            TabPage tpage = new TabPage();
            tpage.BorderStyle = BorderStyle.Fixed3D;
            browserTabControl.TabPages.Insert(browserTabControl.TabCount - 1, tpage);
            WebBrowser browser = CreateWebBrowser(homePage);//new WebBrowser();
            //browser.ScriptErrorsSuppressed = true;
            //browser.WebBrowserShortcutsEnabled = true;
            //browser.Navigate(homePage);
            tpage.Controls.Add(browser);
            //browser.Dock = DockStyle.Fill;
            browserTabControl.SelectTab(tpage);
            //browser.ProgressChanged += new WebBrowserProgressChangedEventHandler(AWB_ProgressChanged);
            //browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(AWB_DocumentCompleted);
            //browser.Navigating += new WebBrowserNavigatingEventHandler(AWB_Navigating);
            //browser.CanGoBackChanged += new EventHandler(browser_CanGoBackChanged);
            //browser.CanGoForwardChanged += new EventHandler(browser_CanGoForwardChanged);
            wb = browser;
        }

        //DocumentCompleted
        private void AWB_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser currentBrowser = getCurrentBrowser();
            this.toolStripStatusLabel1.Text = "Done";
            String text = "Blank Page";

            if (!currentBrowser.Url.ToString().Equals("about:blank"))
            {
                text = currentBrowser.Url.Host.ToString();
            }

            this.adrBarTextBox.Text = currentBrowser.Url.ToString();
            browserTabControl.SelectedTab.Text = text;

            if (!urls.Contains(currentBrowser.Url.Host.ToString()))
                urls.Add(currentBrowser.Url.Host.ToString());

            if (!currentBrowser.Url.ToString().Equals("about:blank") && currentBrowser.StatusText.Equals("Done"))
                addHistory(currentBrowser.Url, DateTime.Now.ToString(currentCulture));
        }
        //ProgressChanged    
        private void AWB_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.CurrentProgress <= e.MaximumProgress && e.CurrentProgress >= 0 && e.MaximumProgress > 0)
            { toolStripProgressBar1.Value = ((int)e.CurrentProgress / (int)e.MaximumProgress) * 100; }
            else
            { toolStripProgressBar1.Value = toolStripProgressBar1.Minimum; }

        }
        //Navigating
        private void AWB_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            this.toolStripStatusLabel1.Text = getCurrentBrowser().StatusText;

        }
        //closeTab method
        private void closeTab()
        {
            if (browserTabControl.TabCount != 2)
            {
                browserTabControl.TabPages.RemoveAt(browserTabControl.SelectedIndex);
            }

        }
        //selected index changed
        private void browserTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (browserTabControl.SelectedIndex == browserTabControl.TabPages.Count - 1) addNewTab();
            else
            {
                if (getCurrentBrowser().Url != null)
                    adrBarTextBox.Text = getCurrentBrowser().Url.ToString();
                else adrBarTextBox.Text = "about:blank";

                if (getCurrentBrowser().CanGoBack) toolStripButton1.Enabled = true;
                else toolStripButton1.Enabled = false;

                if (getCurrentBrowser().CanGoForward) toolStripButton2.Enabled = true;
                else toolStripButton2.Enabled = false;
            }
        }

        /* tab context menu */

        private void closeTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            closeTab();
        }
        private void duplicateTabToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (getCurrentBrowser().Url != null)
            {
                Uri dup_url = getCurrentBrowser().Url;
                addNewTab();
                getCurrentBrowser().Url = dup_url;

            }
            else addNewTab();
        }
        #endregion

        #region     TOOL CONTEXT MENU
        /* TOOL CONTEXT MENU*/

        //menu bar
        private void menuBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            menuBar.Visible = !menuBar.Visible;
            this.menuBarToolStripMenuItem.Checked = menuBar.Visible;
            settings.DocumentElement.ChildNodes[0].Attributes[0].Value = menuBar.Visible.ToString();
        }
        //address bar
        private void commandBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            adrBar.Visible = !adrBar.Visible;
            this.commandBarToolStripMenuItem.Checked = adrBar.Visible;
            settings.DocumentElement.ChildNodes[1].Attributes[0].Value = adrBar.Visible.ToString();
        }
        #endregion

        #region ADDRESS BAR
        /*ADDRESS BAR*/

        private WebBrowser getCurrentBrowser()
        {
            return (WebBrowser)browserTabControl.SelectedTab.Controls[0];
        }
        //ENTER
        private void adrBarTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getCurrentBrowser().Navigate(adrBarTextBox.Text);

            }
        }
        //select all from adr bar
        private void adrBarTextBox_Click(object sender, EventArgs e)
        {
            adrBarTextBox.SelectAll();
        }
        //show urls

        private void showUrl()
        {
            if (File.Exists(historyXml))
            {
                XmlDocument myXml = new XmlDocument();
                myXml.Load(historyXml);
                int i = 0;
                int num = int.Parse(settings.DocumentElement.ChildNodes[6].InnerText.ToString());
                foreach (XmlElement el in myXml.DocumentElement.ChildNodes)
                {
                    if (num <= i++) break;
                    else adrBarTextBox.Items.Add(el.GetAttribute("url").ToString());

                }
            }
        }

        private void adrBarTextBox_DropDown(object sender, EventArgs e)
        {
            adrBarTextBox.Items.Clear();
            showUrl();
        }
        //navigate on selected url 
        private void adrBarTextBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(adrBarTextBox.SelectedItem.ToString());
        }
        //canGoForwardChanged
        void browser_CanGoForwardChanged(object sender, EventArgs e)
        {
            toolStripButton2.Enabled = !toolStripButton2.Enabled;
        }
        //canGoBackChanged
        void browser_CanGoBackChanged(object sender, EventArgs e)
        {
            toolStripButton1.Enabled = !toolStripButton1.Enabled;
        }
        //back  
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoBack();
        }
        //forward
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoForward();
        }
        //go
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(adrBarTextBox.Text);

        }
        //refresh
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }
        //stop
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }

        //search
        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                if (googleSearch.Checked == true)
                    getCurrentBrowser().Navigate("http://google.com/search?q=" + searchTextBox.Text);
                else
                    getCurrentBrowser().Navigate("http://search.live.com/results.aspx?q=" + searchTextBox.Text);
        }

        private void googleSearch_Click(object sender, EventArgs e)
        {
            liveSearch.Checked = !googleSearch.Checked;
        }

        private void liveSearch_Click(object sender, EventArgs e)
        {
            googleSearch.Checked = !liveSearch.Checked;
        }

        #endregion

        #region FILE
        /*FILE*/

        //new tab
        private void newTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addNewTab();
        }
        //duplicate tab
        private void duplicateTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (getCurrentBrowser().Url != null)
            {
                Uri dup_url = getCurrentBrowser().Url;
                addNewTab();
                getCurrentBrowser().Url = dup_url;

            }
            else addNewTab();
        }
        //new window
        private void newWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AdvancedWebBrowser()).Show();

        }
        //close tab
        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeTab();
        }
        //open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new Open(getCurrentBrowser())).Show();
        }
        //page setup
        private void pageSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPageSetupDialog();
        }
        //save as
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowSaveAsDialog();
        }
        //print
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintDialog();

        }
        //print preview
        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPrintPreviewDialog();
        }
        //properties
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().ShowPropertiesDialog();
        }
        //send page by email
        private void pageByEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //getCurrentBrowser().Navigate("https://login.yahoo.com/config/login_verify2?&.src=ym");
            Process.Start("msimn.exe");
        }
        //send link by email
        private void linkByEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // getCurrentBrowser().Navigate("https://login.yahoo.com/config/login_verify2?&.src=ym");
            Process.Start("msimn.exe");
        }


        #endregion

        #region EDIT
        /*EDIT*/
        //cut
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Cut", false, null);

        }
        //copy
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Copy", false, null);

        }
        //paste
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("Paste", false, null);
        }
        //select all
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Document.ExecCommand("SelectAll", true, null);
        }
        #endregion

        #region VIEW

        /*Go to*/
        //drop down opening
        private void goToToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            backToolStripMenuItem.Enabled = getCurrentBrowser().CanGoBack;
            forwardToolStripMenuItem.Enabled = getCurrentBrowser().CanGoForward;

            while (goToMenuItem.DropDownItems.Count > 5)
                goToMenuItem.DropDownItems.RemoveAt(goToMenuItem.DropDownItems.Count - 1);

            foreach (string a in urls)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(a, null, goto_click);

                item.Checked = (getCurrentBrowser().Url.Host.ToString().Equals(a));

                goToMenuItem.DropDownItems.Add(item);
            }
        }
        private void goto_click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(sender.ToString());
        }
        //back
        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoBack();
        }
        //forward
        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().GoForward();
        }
        //home
        private void homePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Navigate(homePage);
        }
        /*Stop*/
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Stop();
        }
        /*Refresh*/
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            getCurrentBrowser().Refresh();
        }
        /*view source*/
        private void sourceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string source = "source.txt";
            StreamWriter writer = File.CreateText(source);
            writer.Write(getCurrentBrowser().DocumentText);
            writer.Close();
            Process.Start("notepad.exe", source);
        }
        //text size 
        private void textSizeToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //full screen
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!(this.FormBorderStyle == FormBorderStyle.None && this.WindowState == FormWindowState.Maximized))
            //{
            //    this.FormBorderStyle = FormBorderStyle.None;
            //    this.WindowState = FormWindowState.Maximized;
            //    this.TopMost = true;
            //    menuBar.Visible = false;
            //    adrBar.Visible = false;
            //}
            //else
            //{
            //    this.WindowState = FormWindowState.Normal;
            //    this.FormBorderStyle = FormBorderStyle.Sizable;
            //    this.TopMost = false;
            //    menuBar.Visible = (settings.DocumentElement.ChildNodes[0].Attributes[0].Value.Equals("True"));
            //    adrBar.Visible = (settings.DocumentElement.ChildNodes[1].Attributes[0].Value.Equals("True"));

            //}
        }

        #endregion

        #region TOOLS

        //internet options
        private void internetOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InternetOption intOp = new InternetOption(getCurrentBrowser().Url.ToString());
            if (intOp.ShowDialog() == DialogResult.OK)
            {
                if (!intOp.homepage.Text.Equals(""))
                {
                    homePage = intOp.homepage.Text;
                    settings.DocumentElement.ChildNodes[5].InnerText = intOp.homepage.Text;
                }
                if (intOp.deleteHistory.Checked == true)
                {
                    File.Delete(historyXml);

                }
                settings.DocumentElement.ChildNodes[6].InnerText = intOp.num.Value.ToString();
                //ActiveForm.ForeColor = intOp.forecolor;
                //ActiveForm.BackColor = intOp.backcolor;

                adrBar.BackColor = intOp.backcolor;
                //ActiveForm.Font = intOp.font;

                menuBar.Font = intOp.font;

                settings.Save(settingsXml);
            }
        }

        #region IE Emulation Options

        private void SetEmulationOption(BrowserEmulationVersion bev)
        {
            IEEmulationHelper.SetBrowserEmulationVersion(bev);
            MessageBox.Show("Application must be restarted to take effect.");
        }

        private void UncheckAllEmulationMenus()
        {
            mnuEmulationSetToHighestInstalled.Checked = false;
            mnuEmulationSet11Edge.Checked = false;
            mnuEmulationSet11.Checked = false;
            mnuEmulationSet10.Checked = false;
            mnuEmulationSet10Standards.Checked = false;
            mnuEmulationSet9.Checked = false;
            mnuEmulationSet9Standards.Checked = false;
            mnuEmulationSet8.Checked = false;
            mnuEmulationSet8Standards.Checked = false;
            mnuEmulationSet7.Checked = false;
        }

        private void mnuEmulationDelete_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            IEEmulationHelper.DeleteBrowserEmulationVersion();
            MessageBox.Show("Application must be restarted to take effect.");
        }

        private void CheckForExistingEmulationOptons()
        {
            bool tmp = IEEmulationHelper.IsBrowserEmulationSet();

            // nothing to do
            if (!tmp)
                return;

            // check which option is being used and check the appropriate menu item
            BrowserEmulationVersion bve = IEEmulationHelper.GetBrowserEmulationVersion();

            switch (bve)
            {
                case BrowserEmulationVersion.DefaultToHighestInstalledVersion:
                    mnuEmulationSetToHighestInstalled.Checked = true;
                    break;

                case BrowserEmulationVersion.Version11Edge:
                    mnuEmulationSet11Edge.Checked = true;
                    break;

                case BrowserEmulationVersion.Version11:
                    mnuEmulationSet11.Checked = true;
                    break;

                case BrowserEmulationVersion.Version10:
                    mnuEmulationSet10.Checked = true;
                    break;

                case BrowserEmulationVersion.Version10Standards:
                    mnuEmulationSet10Standards.Checked = true;
                    break;

                case BrowserEmulationVersion.Version9:
                    mnuEmulationSet9.Checked = true;
                    break;

                case BrowserEmulationVersion.Version9Standards:
                    mnuEmulationSet9Standards.Checked = true;
                    break;

                case BrowserEmulationVersion.Version8:
                    mnuEmulationSet8.Checked = true;
                    break;

                case BrowserEmulationVersion.Version8Standards:
                    mnuEmulationSet8Standards.Checked = true;
                    break;

                case BrowserEmulationVersion.Version7:
                    mnuEmulationSet7.Checked = true;
                    break;

                default:
                    break;
            }

            Console.WriteLine("");
        }


        private void mnuEmulationSetToHighestInstalled_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSetToHighestInstalled.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.DefaultToHighestInstalledVersion);
        }

        private void mnuEmulationSet11Edge_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet11Edge.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version11Edge);
        }

        private void mnuEmulationSet11_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet11.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version11);
        }

        private void mnuEmulationSet10_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet10.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version10);
        }

        private void mnuEmulationSet10Standards_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet10Standards.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version10Standards);
        }

        private void mnuEmulationSet9_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet9.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version9);
        }

        private void mnuEmulationSet9Standards_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet9Standards.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version9Standards);
        }

        private void mnuEmulationSet8_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet8.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version8);
        }

        private void mnuEmulationSet8Standards_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet8Standards.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version8Standards);
        }

        private void mnuEmulationSet7_Click(object sender, EventArgs e)
        {
            UncheckAllEmulationMenus();
            mnuEmulationSet7.Checked = true;
            SetEmulationOption(BrowserEmulationVersion.Version7);
        }

        #endregion


        #endregion

    }
}
