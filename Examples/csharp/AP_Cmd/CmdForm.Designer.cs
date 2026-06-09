namespace AP_Cmd
{
	partial class CmdForm
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
			this.BTCopyLibToLib = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BTCopyLibToLib
			// 
			this.BTCopyLibToLib.Location = new System.Drawing.Point(12, 22);
			this.BTCopyLibToLib.Name = "BTCopyLibToLib";
			this.BTCopyLibToLib.Size = new System.Drawing.Size(141, 23);
			this.BTCopyLibToLib.TabIndex = 0;
			this.BTCopyLibToLib.Text = "Copy Lib to Lib";
			this.BTCopyLibToLib.UseVisualStyleBackColor = true;
			this.BTCopyLibToLib.Click += new System.EventHandler(this.BTCopyLibToLib_Click);
			// 
			// CmdForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(343, 262);
			this.Controls.Add(this.BTCopyLibToLib);
			this.Name = "CmdForm";
			this.Text = "CmdForm";
			this.Load += new System.EventHandler(this.CmdForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BTCopyLibToLib;
	}
}