
#define TRIAL

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

#if TRIAL
using Microsoft.Win32;
#endif

namespace LavoroNET
{
  [ComVisible(true)]
  public class MainForm : Form
  {
    public string FilesPath = "files.js";
    public string BrowserPath = "browser.html";
    private IContainer components;
    private TabControl tabControlMain;
    private TabPage tabPageFileBrowser;
    private WebBrowser webBrowserFile;
    private TabPage tabPagePreview;
    private WebBrowser webBrowserPreview;
    private Label labelPreviewTitle;
    private TextBox textBoxPreviewTitle;
    private Label labelNavigate;
    private TextBox textBoxNavigate;
    private Button buttonNavigate;
    private Button buttonAddFile;

    #if TRIAL
    public void CheckRuns() {
		try {
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\OVG-Developers", true);
			
			int runs = -1;
			
			if (key != null && key.GetValue("Runs") != null) {
				runs = (int) key.GetValue("Runs");
			} else {
				key = Registry.CurrentUser.CreateSubKey("Software\\OVG-Developers");
			}
			
			runs = runs + 1;
			
			key.SetValue("Runs", runs);
			
			if (runs > 30) {
				System.Windows.Forms.MessageBox.Show("Number of runs expired.\n"
							+ "Please register the application (visit https://ovg-developers.mystrikingly.com/ for purchase).");
				
				Environment.Exit(0);
			}
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
	}
	
	public bool IsRegistered() {
		try {
			RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\OVG-Developers");
			
			if (key != null && key.GetValue("Registered") != null) {
				return true;
			}
		} catch (Exception e) {
			Console.WriteLine(e.Message);
		}
		
		return false;
	}
    #endif
    
    public MainForm() {
    	this.InitializeComponent();
    }

    public void UpdateFiles(string s)
    {
      File.WriteAllText(this.FilesPath, "var files = [" + s + "];", Encoding.UTF8);
      this.webBrowserFile.Navigate(new Uri(this.BrowserPath));
    }

    public void PreviewFile(string url, string cookie)
    {
      this.webBrowserPreview.Url = new Uri(url);
      this.webBrowserPreview.Document.Cookie = cookie;
      this.webBrowserPreview.Navigate(new Uri(url));
      this.tabControlMain.SelectedTab = this.tabPagePreview;
    }

    public void AddFile(string url, string cookie)
    {
      this.webBrowserFile.Document.InvokeScript("addFileNET", (object[]) new string[2]
      {
        url,
        cookie
      });
      this.webBrowserFile.Document.InvokeScript("updateFilesNET");
      this.webBrowserFile.Navigate(new Uri(this.BrowserPath));
    }

    public void AddFileFromInput()
    {
      this.webBrowserFile.Document.InvokeScript("updateFilesNET");
      this.webBrowserFile.Navigate(new Uri(this.BrowserPath));
    }

    private void MainFormLoad(object sender, EventArgs e)
    {
      this.FilesPath = Environment.CurrentDirectory + "\\" + this.FilesPath;
      this.BrowserPath = Environment.CurrentDirectory + "\\" + this.BrowserPath;
      this.webBrowserFile.ScriptErrorsSuppressed = true;
      this.webBrowserPreview.ScriptErrorsSuppressed = true;
      this.webBrowserFile.ObjectForScripting = this.webBrowserPreview.ObjectForScripting = (object) this;
      this.webBrowserFile.Url = new Uri(this.BrowserPath);
      this.MainFormResize((object) this, (EventArgs) null);
    }

    private void ButtonNavigateClick(object sender, EventArgs e)
    {
      this.webBrowserPreview.Url = new Uri(this.textBoxNavigate.Text);
    }

    private void WebBrowserPreviewDocumentCompleted(
      object sender,
      WebBrowserDocumentCompletedEventArgs e)
    {
      this.textBoxPreviewTitle.Text = this.webBrowserPreview.DocumentTitle;
    }

    private void ButtonAddFileClick(object sender, EventArgs e)
    {
      this.webBrowserFile.Document.InvokeScript("addFileNET", (object[]) new string[2]
      {
        this.webBrowserPreview.Url.AbsoluteUri.ToString(),
        this.webBrowserPreview.Document.Cookie != null ? this.webBrowserPreview.Document.Cookie.ToString() : ""
      });
      this.webBrowserFile.Document.InvokeScript("updateFilesNET");
      this.webBrowserFile.Navigate(new Uri(this.BrowserPath));
    }

    private void WebBrowserPreviewNavigated(object sender, WebBrowserNavigatedEventArgs e)
    {
      this.textBoxNavigate.Text = this.webBrowserPreview.Url.AbsoluteUri;
      this.textBoxPreviewTitle.Text = this.webBrowserPreview.DocumentTitle;
    }

    private void MainFormResize(object sender, EventArgs e)
    {
      this.tabControlMain.Width = this.Width;
      this.tabControlMain.Height = this.Height;
      this.tabPageFileBrowser.Width = this.tabControlMain.Width;
      this.tabPageFileBrowser.Height = this.tabControlMain.Height;
      this.webBrowserFile.Width = this.tabPageFileBrowser.Width;
      this.webBrowserFile.Height = this.tabPageFileBrowser.Height;
      this.tabPagePreview.Width = this.tabControlMain.Width;
      this.tabPagePreview.Height = this.tabControlMain.Height;
      this.webBrowserPreview.Width = this.tabPagePreview.Width;
      this.webBrowserPreview.Height = this.tabPagePreview.Height - 108;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
    	System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
    	this.tabControlMain = new System.Windows.Forms.TabControl();
    	this.tabPageFileBrowser = new System.Windows.Forms.TabPage();
    	this.webBrowserFile = new System.Windows.Forms.WebBrowser();
    	this.tabPagePreview = new System.Windows.Forms.TabPage();
    	this.buttonAddFile = new System.Windows.Forms.Button();
    	this.buttonNavigate = new System.Windows.Forms.Button();
    	this.textBoxNavigate = new System.Windows.Forms.TextBox();
    	this.labelNavigate = new System.Windows.Forms.Label();
    	this.textBoxPreviewTitle = new System.Windows.Forms.TextBox();
    	this.webBrowserPreview = new System.Windows.Forms.WebBrowser();
    	this.labelPreviewTitle = new System.Windows.Forms.Label();
    	this.tabControlMain.SuspendLayout();
    	this.tabPageFileBrowser.SuspendLayout();
    	this.tabPagePreview.SuspendLayout();
    	this.SuspendLayout();
    	// 
    	// tabControlMain
    	// 
    	this.tabControlMain.Controls.Add(this.tabPageFileBrowser);
    	this.tabControlMain.Controls.Add(this.tabPagePreview);
    	this.tabControlMain.Location = new System.Drawing.Point(0, 0);
    	this.tabControlMain.Name = "tabControlMain";
    	this.tabControlMain.SelectedIndex = 0;
    	this.tabControlMain.Size = new System.Drawing.Size(758, 430);
    	this.tabControlMain.TabIndex = 0;
    	// 
    	// tabPageFileBrowser
    	// 
    	this.tabPageFileBrowser.Controls.Add(this.webBrowserFile);
    	this.tabPageFileBrowser.Location = new System.Drawing.Point(4, 25);
    	this.tabPageFileBrowser.Name = "tabPageFileBrowser";
    	this.tabPageFileBrowser.Padding = new System.Windows.Forms.Padding(3);
    	this.tabPageFileBrowser.Size = new System.Drawing.Size(750, 401);
    	this.tabPageFileBrowser.TabIndex = 1;
    	this.tabPageFileBrowser.Text = "File Browser";
    	this.tabPageFileBrowser.UseVisualStyleBackColor = true;
    	// 
    	// webBrowserFile
    	// 
    	this.webBrowserFile.Dock = System.Windows.Forms.DockStyle.Fill;
    	this.webBrowserFile.Location = new System.Drawing.Point(3, 3);
    	this.webBrowserFile.MinimumSize = new System.Drawing.Size(20, 20);
    	this.webBrowserFile.Name = "webBrowserFile";
    	this.webBrowserFile.Size = new System.Drawing.Size(744, 395);
    	this.webBrowserFile.TabIndex = 0;
    	// 
    	// tabPagePreview
    	// 
    	this.tabPagePreview.Controls.Add(this.buttonAddFile);
    	this.tabPagePreview.Controls.Add(this.buttonNavigate);
    	this.tabPagePreview.Controls.Add(this.textBoxNavigate);
    	this.tabPagePreview.Controls.Add(this.labelNavigate);
    	this.tabPagePreview.Controls.Add(this.textBoxPreviewTitle);
    	this.tabPagePreview.Controls.Add(this.webBrowserPreview);
    	this.tabPagePreview.Controls.Add(this.labelPreviewTitle);
    	this.tabPagePreview.Location = new System.Drawing.Point(4, 25);
    	this.tabPagePreview.Name = "tabPagePreview";
    	this.tabPagePreview.Padding = new System.Windows.Forms.Padding(3);
    	this.tabPagePreview.Size = new System.Drawing.Size(750, 401);
    	this.tabPagePreview.TabIndex = 2;
    	this.tabPagePreview.Text = "Preview";
    	// 
    	// buttonAddFile
    	// 
    	this.buttonAddFile.Location = new System.Drawing.Point(628, 57);
    	this.buttonAddFile.Name = "buttonAddFile";
    	this.buttonAddFile.Size = new System.Drawing.Size(79, 23);
    	this.buttonAddFile.TabIndex = 6;
    	this.buttonAddFile.Text = "Add file";
    	this.buttonAddFile.UseVisualStyleBackColor = true;
    	this.buttonAddFile.Click += new System.EventHandler(this.ButtonAddFileClick);
    	// 
    	// buttonNavigate
    	// 
    	this.buttonNavigate.Location = new System.Drawing.Point(628, 20);
    	this.buttonNavigate.Name = "buttonNavigate";
    	this.buttonNavigate.Size = new System.Drawing.Size(79, 23);
    	this.buttonNavigate.TabIndex = 5;
    	this.buttonNavigate.Text = "Navigate";
    	this.buttonNavigate.UseVisualStyleBackColor = true;
    	this.buttonNavigate.Click += new System.EventHandler(this.ButtonNavigateClick);
    	// 
    	// textBoxNavigate
    	// 
    	this.textBoxNavigate.Location = new System.Drawing.Point(97, 20);
    	this.textBoxNavigate.Name = "textBoxNavigate";
    	this.textBoxNavigate.Size = new System.Drawing.Size(516, 22);
    	this.textBoxNavigate.TabIndex = 4;
    	// 
    	// labelNavigate
    	// 
    	this.labelNavigate.Location = new System.Drawing.Point(16, 20);
    	this.labelNavigate.Name = "labelNavigate";
    	this.labelNavigate.Size = new System.Drawing.Size(70, 23);
    	this.labelNavigate.TabIndex = 3;
    	this.labelNavigate.Text = "Navigate:";
    	// 
    	// textBoxPreviewTitle
    	// 
    	this.textBoxPreviewTitle.Location = new System.Drawing.Point(95, 57);
    	this.textBoxPreviewTitle.Name = "textBoxPreviewTitle";
    	this.textBoxPreviewTitle.ReadOnly = true;
    	this.textBoxPreviewTitle.Size = new System.Drawing.Size(518, 22);
    	this.textBoxPreviewTitle.TabIndex = 2;
    	// 
    	// webBrowserPreview
    	// 
    	this.webBrowserPreview.Location = new System.Drawing.Point(0, 108);
    	this.webBrowserPreview.MinimumSize = new System.Drawing.Size(20, 20);
    	this.webBrowserPreview.Name = "webBrowserPreview";
    	this.webBrowserPreview.Size = new System.Drawing.Size(747, 293);
    	this.webBrowserPreview.TabIndex = 1;
    	this.webBrowserPreview.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowserPreviewDocumentCompleted);
    	this.webBrowserPreview.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowserPreviewNavigated);
    	// 
    	// labelPreviewTitle
    	// 
    	this.labelPreviewTitle.Location = new System.Drawing.Point(16, 57);
    	this.labelPreviewTitle.Name = "labelPreviewTitle";
    	this.labelPreviewTitle.Size = new System.Drawing.Size(49, 23);
    	this.labelPreviewTitle.TabIndex = 0;
    	this.labelPreviewTitle.Text = "Title:";
    	// 
    	// MainForm
    	// 
    	this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
    	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
    	this.ClientSize = new System.Drawing.Size(762, 426);
    	this.Controls.Add(this.tabControlMain);
    	this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
    	this.Name = "MainForm";
    	this.Text = "LavoroNET";
    	this.Load += new System.EventHandler(this.MainFormLoad);
    	this.Shown += new System.EventHandler(this.MainFormShown);
    	this.Resize += new System.EventHandler(this.MainFormResize);
    	this.tabControlMain.ResumeLayout(false);
    	this.tabPageFileBrowser.ResumeLayout(false);
    	this.tabPagePreview.ResumeLayout(false);
    	this.tabPagePreview.PerformLayout();
    	this.ResumeLayout(false);

    }
		void MainFormShown(object sender, EventArgs e)
		{
			#if TRIAL
			if (!IsRegistered()) {
    			CheckRuns();
    		}
			#endif
		}
  }
}
