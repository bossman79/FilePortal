namespace AP_Lists
{
	partial class WidthEdit
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
			this.NUD1 = new System.Windows.Forms.NumericUpDown();
			this.BTCancel = new System.Windows.Forms.Button();
			this.BTOK = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NUD1)).BeginInit();
			this.SuspendLayout();
			// 
			// NUD1
			// 
			this.NUD1.Location = new System.Drawing.Point(12, 34);
			this.NUD1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.NUD1.Name = "NUD1";
			this.NUD1.Size = new System.Drawing.Size(120, 20);
			this.NUD1.TabIndex = 0;
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(161, 41);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 3;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(161, 12);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 2;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 13);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Width";
			// 
			// WidthEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(254, 82);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Controls.Add(this.NUD1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "WidthEdit";
			this.Text = "Width Edit";
			this.Load += new System.EventHandler(this.WidthEdit_Load);
			((System.ComponentModel.ISupportInitialize)(this.NUD1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NumericUpDown NUD1;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Label label2;
	}
}