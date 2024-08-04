
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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
      ComponentResourceManager resources = new ComponentResourceManager(typeof (MainForm));
      this.tabControlMain = new TabControl();
      this.tabPageFileBrowser = new TabPage();
      this.webBrowserFile = new WebBrowser();
      this.tabPagePreview = new TabPage();
      this.buttonAddFile = new Button();
      this.buttonNavigate = new Button();
      this.textBoxNavigate = new TextBox();
      this.labelNavigate = new Label();
      this.textBoxPreviewTitle = new TextBox();
      this.webBrowserPreview = new WebBrowser();
      this.labelPreviewTitle = new Label();
      this.tabControlMain.SuspendLayout();
      this.tabPageFileBrowser.SuspendLayout();
      this.tabPagePreview.SuspendLayout();
      this.SuspendLayout();
      this.tabControlMain.Controls.Add((Control) this.tabPageFileBrowser);
      this.tabControlMain.Controls.Add((Control) this.tabPagePreview);
      this.tabControlMain.Location = new Point(0, 0);
      this.tabControlMain.Name = "tabControlMain";
      this.tabControlMain.SelectedIndex = 0;
      this.tabControlMain.Size = new Size(758, 430);
      this.tabControlMain.TabIndex = 0;
      this.tabPageFileBrowser.Controls.Add((Control) this.webBrowserFile);
      this.tabPageFileBrowser.Location = new Point(4, 25);
      this.tabPageFileBrowser.Name = "tabPageFileBrowser";
      this.tabPageFileBrowser.Padding = new Padding(3);
      this.tabPageFileBrowser.Size = new Size(750, 401);
      this.tabPageFileBrowser.TabIndex = 1;
      this.tabPageFileBrowser.Text = "File Browser";
      this.tabPageFileBrowser.UseVisualStyleBackColor = true;
      this.webBrowserFile.Dock = DockStyle.Fill;
      this.webBrowserFile.Location = new Point(3, 3);
      this.webBrowserFile.MinimumSize = new Size(20, 20);
      this.webBrowserFile.Name = "webBrowserFile";
      this.webBrowserFile.Size = new Size(744, 395);
      this.webBrowserFile.TabIndex = 0;
      this.tabPagePreview.Controls.Add((Control) this.buttonAddFile);
      this.tabPagePreview.Controls.Add((Control) this.buttonNavigate);
      this.tabPagePreview.Controls.Add((Control) this.textBoxNavigate);
      this.tabPagePreview.Controls.Add((Control) this.labelNavigate);
      this.tabPagePreview.Controls.Add((Control) this.textBoxPreviewTitle);
      this.tabPagePreview.Controls.Add((Control) this.webBrowserPreview);
      this.tabPagePreview.Controls.Add((Control) this.labelPreviewTitle);
      this.tabPagePreview.Location = new Point(4, 25);
      this.tabPagePreview.Name = "tabPagePreview";
      this.tabPagePreview.Padding = new Padding(3);
      this.tabPagePreview.Size = new Size(750, 401);
      this.tabPagePreview.TabIndex = 2;
      this.tabPagePreview.Text = "Preview";
      this.buttonAddFile.Location = new Point(628, 57);
      this.buttonAddFile.Name = "buttonAddFile";
      this.buttonAddFile.Size = new Size(79, 23);
      this.buttonAddFile.TabIndex = 6;
      this.buttonAddFile.Text = "Add file";
      this.buttonAddFile.UseVisualStyleBackColor = true;
      this.buttonAddFile.Click += new EventHandler(this.ButtonAddFileClick);
      this.buttonNavigate.Location = new Point(628, 20);
      this.buttonNavigate.Name = "buttonNavigate";
      this.buttonNavigate.Size = new Size(79, 23);
      this.buttonNavigate.TabIndex = 5;
      this.buttonNavigate.Text = "Navigate";
      this.buttonNavigate.UseVisualStyleBackColor = true;
      this.buttonNavigate.Click += new EventHandler(this.ButtonNavigateClick);
      this.textBoxNavigate.Location = new Point(97, 20);
      this.textBoxNavigate.Name = "textBoxNavigate";
      this.textBoxNavigate.Size = new Size(516, 22);
      this.textBoxNavigate.TabIndex = 4;
      this.labelNavigate.Location = new Point(16, 20);
      this.labelNavigate.Name = "labelNavigate";
      this.labelNavigate.Size = new Size(70, 23);
      this.labelNavigate.TabIndex = 3;
      this.labelNavigate.Text = "Navigate:";
      this.textBoxPreviewTitle.Location = new Point(95, 57);
      this.textBoxPreviewTitle.Name = "textBoxPreviewTitle";
      this.textBoxPreviewTitle.ReadOnly = true;
      this.textBoxPreviewTitle.Size = new Size(518, 22);
      this.textBoxPreviewTitle.TabIndex = 2;
      this.webBrowserPreview.Location = new Point(0, 108);
      this.webBrowserPreview.MinimumSize = new Size(20, 20);
      this.webBrowserPreview.Name = "webBrowserPreview";
      this.webBrowserPreview.Size = new Size(747, 293);
      this.webBrowserPreview.TabIndex = 1;
      this.webBrowserPreview.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WebBrowserPreviewDocumentCompleted);
      this.webBrowserPreview.Navigated += new WebBrowserNavigatedEventHandler(this.WebBrowserPreviewNavigated);
      this.labelPreviewTitle.Location = new Point(16, 57);
      this.labelPreviewTitle.Name = "labelPreviewTitle";
      this.labelPreviewTitle.Size = new Size(49, 23);
      this.labelPreviewTitle.TabIndex = 0;
      this.labelPreviewTitle.Text = "Title:";
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(762, 426);
      this.Controls.Add((Control) this.tabControlMain);
      this.Icon = (Icon) resources.GetObject("$this.Icon");
      this.Name = "MainForm";
      this.Text = "LavoroNET";
      this.Load += new EventHandler(this.MainFormLoad);
      this.Resize += new EventHandler(this.MainFormResize);
      this.tabControlMain.ResumeLayout(false);
      this.tabPageFileBrowser.ResumeLayout(false);
      this.tabPagePreview.ResumeLayout(false);
      this.tabPagePreview.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
