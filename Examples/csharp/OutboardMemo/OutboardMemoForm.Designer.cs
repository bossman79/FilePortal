namespace OutboardMemo
{
	partial class OutboardMemoForm
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
			this.BTGetJpg = new System.Windows.Forms.Button();
			this.BTSetJpg = new System.Windows.Forms.Button();
			this.BTGetApiMemo = new System.Windows.Forms.Button();
			this.BTSetApiMemo = new System.Windows.Forms.Button();
			this.EBApiMemo = new System.Windows.Forms.TextBox();
			this.BTGetTextMemo = new System.Windows.Forms.Button();
			this.BTSetTextMemo = new System.Windows.Forms.Button();
			this.BTGetThumbnail = new System.Windows.Forms.Button();
			this.BTSetEmf = new System.Windows.Forms.Button();
			this.Label3 = new System.Windows.Forms.Label();
			this.BTSetBmp = new System.Windows.Forms.Button();
			this.Label2 = new System.Windows.Forms.Label();
			this.BTClose = new System.Windows.Forms.Button();
			this.STStartUpPath = new System.Windows.Forms.Label();
			this.Label1 = new System.Windows.Forms.Label();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.GroupBox3 = new System.Windows.Forms.GroupBox();
			this.GroupBox4 = new System.Windows.Forms.GroupBox();
			this.STFilePath = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.BTClearThumbnail = new System.Windows.Forms.Button();
			this.RTBRichTextMemo = new System.Windows.Forms.RichTextBox();
			this.GroupBox1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// BTGetJpg
			// 
			this.BTGetJpg.Location = new System.Drawing.Point(140, 583);
			this.BTGetJpg.Name = "BTGetJpg";
			this.BTGetJpg.Size = new System.Drawing.Size(96, 23);
			this.BTGetJpg.TabIndex = 38;
			this.BTGetJpg.Text = "Get JPG";
			this.BTGetJpg.Click += new System.EventHandler(this.BTGetJpg_Click);
			// 
			// BTSetJpg
			// 
			this.BTSetJpg.Location = new System.Drawing.Point(28, 583);
			this.BTSetJpg.Name = "BTSetJpg";
			this.BTSetJpg.Size = new System.Drawing.Size(96, 23);
			this.BTSetJpg.TabIndex = 37;
			this.BTSetJpg.Text = "Set JPG";
			this.BTSetJpg.Click += new System.EventHandler(this.BTSetJpg_Click);
			// 
			// BTGetApiMemo
			// 
			this.BTGetApiMemo.Location = new System.Drawing.Point(140, 519);
			this.BTGetApiMemo.Name = "BTGetApiMemo";
			this.BTGetApiMemo.Size = new System.Drawing.Size(96, 23);
			this.BTGetApiMemo.TabIndex = 36;
			this.BTGetApiMemo.Text = "Get Api Memo";
			this.BTGetApiMemo.Click += new System.EventHandler(this.BTGetApiMemo_Click);
			// 
			// BTSetApiMemo
			// 
			this.BTSetApiMemo.Location = new System.Drawing.Point(28, 519);
			this.BTSetApiMemo.Name = "BTSetApiMemo";
			this.BTSetApiMemo.Size = new System.Drawing.Size(96, 23);
			this.BTSetApiMemo.TabIndex = 35;
			this.BTSetApiMemo.Text = "Set Api Memo";
			this.BTSetApiMemo.Click += new System.EventHandler(this.BTSetApiMemo_Click);
			// 
			// EBApiMemo
			// 
			this.EBApiMemo.Location = new System.Drawing.Point(28, 495);
			this.EBApiMemo.Name = "EBApiMemo";
			this.EBApiMemo.Size = new System.Drawing.Size(216, 20);
			this.EBApiMemo.TabIndex = 34;
			this.EBApiMemo.Text = "EBApiMemo";
			// 
			// BTGetTextMemo
			// 
			this.BTGetTextMemo.Location = new System.Drawing.Point(123, 182);
			this.BTGetTextMemo.Name = "BTGetTextMemo";
			this.BTGetTextMemo.Size = new System.Drawing.Size(96, 23);
			this.BTGetTextMemo.TabIndex = 33;
			this.BTGetTextMemo.Text = "Get Text Memo";
			this.BTGetTextMemo.Click += new System.EventHandler(this.BTGetTextMemo_Click);
			// 
			// BTSetTextMemo
			// 
			this.BTSetTextMemo.Location = new System.Drawing.Point(11, 182);
			this.BTSetTextMemo.Name = "BTSetTextMemo";
			this.BTSetTextMemo.Size = new System.Drawing.Size(96, 23);
			this.BTSetTextMemo.TabIndex = 32;
			this.BTSetTextMemo.Text = "Set Text Memo";
			this.BTSetTextMemo.Click += new System.EventHandler(this.BTSetTextMemo_Click);
			// 
			// BTGetThumbnail
			// 
			this.BTGetThumbnail.Location = new System.Drawing.Point(228, 88);
			this.BTGetThumbnail.Name = "BTGetThumbnail";
			this.BTGetThumbnail.Size = new System.Drawing.Size(102, 23);
			this.BTGetThumbnail.TabIndex = 30;
			this.BTGetThumbnail.Text = "Get Thumbnail";
			this.BTGetThumbnail.Click += new System.EventHandler(this.BTGetThumbnail_Click);
			// 
			// BTSetEmf
			// 
			this.BTSetEmf.Location = new System.Drawing.Point(36, 214);
			this.BTSetEmf.Name = "BTSetEmf";
			this.BTSetEmf.Size = new System.Drawing.Size(75, 23);
			this.BTSetEmf.TabIndex = 29;
			this.BTSetEmf.Text = "Set EMF";
			this.BTSetEmf.Click += new System.EventHandler(this.BTSetEmf_Click);
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(20, 198);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(232, 16);
			this.Label3.TabIndex = 28;
			this.Label3.Text = "Enhanced Metafile Thumbnail : Set.emf";
			// 
			// BTSetBmp
			// 
			this.BTSetBmp.Location = new System.Drawing.Point(36, 166);
			this.BTSetBmp.Name = "BTSetBmp";
			this.BTSetBmp.Size = new System.Drawing.Size(75, 23);
			this.BTSetBmp.TabIndex = 27;
			this.BTSetBmp.Text = "Set BMP";
			this.BTSetBmp.Click += new System.EventHandler(this.BTSetBmp_Click);
			// 
			// Label2
			// 
			this.Label2.Location = new System.Drawing.Point(20, 150);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(232, 16);
			this.Label2.TabIndex = 26;
			this.Label2.Text = "Bitmap Thumbnail : Set.bmp";
			// 
			// BTClose
			// 
			this.BTClose.Location = new System.Drawing.Point(279, 8);
			this.BTClose.Name = "BTClose";
			this.BTClose.Size = new System.Drawing.Size(75, 23);
			this.BTClose.TabIndex = 25;
			this.BTClose.Text = "Close";
			this.BTClose.Click += new System.EventHandler(this.BTClose_Click);
			// 
			// STStartUpPath
			// 
			this.STStartUpPath.Location = new System.Drawing.Point(12, 34);
			this.STStartUpPath.Name = "STStartUpPath";
			this.STStartUpPath.Size = new System.Drawing.Size(304, 16);
			this.STStartUpPath.TabIndex = 24;
			this.STStartUpPath.Text = "STStartUpPath";
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(12, 15);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(100, 16);
			this.Label1.TabIndex = 23;
			this.Label1.Text = "Start Up Path";
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.BTClearThumbnail);
			this.GroupBox1.Controls.Add(this.BTGetThumbnail);
			this.GroupBox1.Location = new System.Drawing.Point(12, 126);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(336, 120);
			this.GroupBox1.TabIndex = 39;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Thumbnail";
			// 
			// GroupBox2
			// 
			this.GroupBox2.Controls.Add(this.RTBRichTextMemo);
			this.GroupBox2.Controls.Add(this.BTSetTextMemo);
			this.GroupBox2.Controls.Add(this.BTGetTextMemo);
			this.GroupBox2.Location = new System.Drawing.Point(12, 254);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(336, 211);
			this.GroupBox2.TabIndex = 40;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Rich Text Memo";
			// 
			// GroupBox3
			// 
			this.GroupBox3.Location = new System.Drawing.Point(12, 471);
			this.GroupBox3.Name = "GroupBox3";
			this.GroupBox3.Size = new System.Drawing.Size(336, 80);
			this.GroupBox3.TabIndex = 41;
			this.GroupBox3.TabStop = false;
			this.GroupBox3.Text = "Api Memo";
			// 
			// GroupBox4
			// 
			this.GroupBox4.Location = new System.Drawing.Point(12, 559);
			this.GroupBox4.Name = "GroupBox4";
			this.GroupBox4.Size = new System.Drawing.Size(336, 56);
			this.GroupBox4.TabIndex = 42;
			this.GroupBox4.TabStop = false;
			this.GroupBox4.Text = "Api Memo File : Set.jpg / Get.jpg";
			// 
			// STFilePath
			// 
			this.STFilePath.Location = new System.Drawing.Point(12, 82);
			this.STFilePath.Name = "STFilePath";
			this.STFilePath.Size = new System.Drawing.Size(304, 16);
			this.STFilePath.TabIndex = 44;
			this.STFilePath.Text = "STFilePath";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(12, 63);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 16);
			this.label5.TabIndex = 43;
			this.label5.Text = "File Path";
			// 
			// BTClearThumbnail
			// 
			this.BTClearThumbnail.Location = new System.Drawing.Point(228, 40);
			this.BTClearThumbnail.Name = "BTClearThumbnail";
			this.BTClearThumbnail.Size = new System.Drawing.Size(102, 23);
			this.BTClearThumbnail.TabIndex = 31;
			this.BTClearThumbnail.Text = "Clear Thumbnail";
			this.BTClearThumbnail.Click += new System.EventHandler(this.BTClearThumbnail_Click);
			// 
			// RTBRichTextMemo
			// 
			this.RTBRichTextMemo.Location = new System.Drawing.Point(6, 20);
			this.RTBRichTextMemo.Name = "RTBRichTextMemo";
			this.RTBRichTextMemo.Size = new System.Drawing.Size(324, 156);
			this.RTBRichTextMemo.TabIndex = 34;
			this.RTBRichTextMemo.Text = "";
			// 
			// OutboardMemoForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(378, 625);
			this.Controls.Add(this.STFilePath);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.BTGetJpg);
			this.Controls.Add(this.BTSetJpg);
			this.Controls.Add(this.BTGetApiMemo);
			this.Controls.Add(this.BTSetApiMemo);
			this.Controls.Add(this.EBApiMemo);
			this.Controls.Add(this.BTSetEmf);
			this.Controls.Add(this.Label3);
			this.Controls.Add(this.BTSetBmp);
			this.Controls.Add(this.Label2);
			this.Controls.Add(this.BTClose);
			this.Controls.Add(this.STStartUpPath);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.GroupBox2);
			this.Controls.Add(this.GroupBox3);
			this.Controls.Add(this.GroupBox4);
			this.Name = "OutboardMemoForm";
			this.Text = "Outboard Memo";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OutboardMemoForm_FormClosing);
			this.Load += new System.EventHandler(this.OutboardMemoForm_Load);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.Button BTGetJpg;
		internal System.Windows.Forms.Button BTSetJpg;
		internal System.Windows.Forms.Button BTGetApiMemo;
		internal System.Windows.Forms.Button BTSetApiMemo;
		internal System.Windows.Forms.TextBox EBApiMemo;
		internal System.Windows.Forms.Button BTGetTextMemo;
		internal System.Windows.Forms.Button BTSetTextMemo;
		internal System.Windows.Forms.Button BTGetThumbnail;
		internal System.Windows.Forms.Button BTSetEmf;
		internal System.Windows.Forms.Label Label3;
		internal System.Windows.Forms.Button BTSetBmp;
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.Button BTClose;
		internal System.Windows.Forms.Label STStartUpPath;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.GroupBox GroupBox1;
		internal System.Windows.Forms.GroupBox GroupBox2;
		internal System.Windows.Forms.GroupBox GroupBox3;
		internal System.Windows.Forms.GroupBox GroupBox4;
		internal System.Windows.Forms.Label STFilePath;
		internal System.Windows.Forms.Label label5;
		internal System.Windows.Forms.Button BTClearThumbnail;
		private System.Windows.Forms.RichTextBox RTBRichTextMemo;
	}
}

