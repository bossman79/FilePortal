namespace AP_Extract
{
	partial class EditBox
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
			this.BTCancel = new System.Windows.Forms.Button();
			this.BTOK = new System.Windows.Forms.Button();
			this.lbl1 = new System.Windows.Forms.Label();
			this.EB1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(244, 39);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(88, 24);
			this.BTCancel.TabIndex = 7;
			this.BTCancel.Text = "Cancel";
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(244, 7);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(88, 24);
			this.BTOK.TabIndex = 6;
			this.BTOK.Text = "OK";
			// 
			// lbl1
			// 
			this.lbl1.Location = new System.Drawing.Point(12, 15);
			this.lbl1.Name = "lbl1";
			this.lbl1.Size = new System.Drawing.Size(208, 16);
			this.lbl1.TabIndex = 5;
			this.lbl1.Text = "Enter Here";
			// 
			// EB1
			// 
			this.EB1.Location = new System.Drawing.Point(12, 39);
			this.EB1.Name = "EB1";
			this.EB1.Size = new System.Drawing.Size(208, 20);
			this.EB1.TabIndex = 4;
			this.EB1.Tag = "";
			// 
			// EditBox
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(341, 73);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Controls.Add(this.lbl1);
			this.Controls.Add(this.EB1);
			this.Name = "EditBox";
			this.Text = "Edit Box";
			this.Load += new System.EventHandler(this.EditBox_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.Button BTCancel;
		internal System.Windows.Forms.Button BTOK;
		internal System.Windows.Forms.Label lbl1;
		internal System.Windows.Forms.TextBox EB1;
	}
}