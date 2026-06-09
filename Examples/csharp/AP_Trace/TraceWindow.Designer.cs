namespace AP_Trace
{
	partial class TraceWindow
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
			this.CH_TraceLibCardChg = new System.Windows.Forms.CheckBox();
			this.CH_TraceSelChg = new System.Windows.Forms.CheckBox();
			this.TextBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// CH_TraceLibCardChg
			// 
			this.CH_TraceLibCardChg.Location = new System.Drawing.Point(183, 12);
			this.CH_TraceLibCardChg.Name = "CH_TraceLibCardChg";
			this.CH_TraceLibCardChg.Size = new System.Drawing.Size(176, 24);
			this.CH_TraceLibCardChg.TabIndex = 4;
			this.CH_TraceLibCardChg.Text = "Trace Library Card Change";
			// 
			// CH_TraceSelChg
			// 
			this.CH_TraceSelChg.Location = new System.Drawing.Point(15, 12);
			this.CH_TraceSelChg.Name = "CH_TraceSelChg";
			this.CH_TraceSelChg.Size = new System.Drawing.Size(152, 24);
			this.CH_TraceSelChg.TabIndex = 3;
			this.CH_TraceSelChg.Text = "Trace Selection Change";
			// 
			// TextBox1
			// 
			this.TextBox1.Location = new System.Drawing.Point(10, 42);
			this.TextBox1.Multiline = true;
			this.TextBox1.Name = "TextBox1";
			this.TextBox1.Size = new System.Drawing.Size(815, 384);
			this.TextBox1.TabIndex = 5;
			// 
			// TraceWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(837, 442);
			this.Controls.Add(this.TextBox1);
			this.Controls.Add(this.CH_TraceLibCardChg);
			this.Controls.Add(this.CH_TraceSelChg);
			this.Name = "TraceWindow";
			this.Text = "Trace Window";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TraceWindow_FormClosing);
			this.Load += new System.EventHandler(this.TraceWindow_Load);
			this.Resize += new System.EventHandler(this.TraceWindow_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.CheckBox CH_TraceLibCardChg;
		internal System.Windows.Forms.CheckBox CH_TraceSelChg;
		internal System.Windows.Forms.TextBox TextBox1;
	}
}