namespace AP_Cmd
{
	partial class CopyLibToLibForm
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
			this.LBLIncoming = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.BTOK = new System.Windows.Forms.Button();
			this.BTCancel = new System.Windows.Forms.Button();
            this.LPCMain = new Synergis.Adept.MainApi.LibPickerControl();
			this.CHCopyLibCard = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// LBLIncoming
			// 
			this.LBLIncoming.AutoSize = true;
			this.LBLIncoming.Location = new System.Drawing.Point(16, 19);
			this.LBLIncoming.Name = "LBLIncoming";
			this.LBLIncoming.Size = new System.Drawing.Size(35, 13);
			this.LBLIncoming.TabIndex = 1;
			this.LBLIncoming.Text = "label1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Destination Library";
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(281, 14);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 3;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(281, 43);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 4;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			// 
			// LPCMain
			// 
			this.LPCMain.Location = new System.Drawing.Point(12, 71);
			this.LPCMain.Name = "LPCMain";
			this.LPCMain.Size = new System.Drawing.Size(256, 28);
			this.LPCMain.TabIndex = 0;
			// 
			// CHCopyLibCard
			// 
			this.CHCopyLibCard.AutoSize = true;
			this.CHCopyLibCard.Location = new System.Drawing.Point(19, 105);
			this.CHCopyLibCard.Name = "CHCopyLibCard";
			this.CHCopyLibCard.Size = new System.Drawing.Size(109, 17);
			this.CHCopyLibCard.TabIndex = 5;
			this.CHCopyLibCard.Text = "Copy Library Card";
			this.CHCopyLibCard.UseVisualStyleBackColor = true;
			// 
			// CopyLibToLibForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(368, 137);
			this.Controls.Add(this.CHCopyLibCard);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.LBLIncoming);
			this.Controls.Add(this.LPCMain);
			this.Name = "CopyLibToLibForm";
			this.Text = "CopyLibToLibForm";
			this.Load += new System.EventHandler(this.CopyLibToLibForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private Synergis.Adept.MainApi.LibPickerControl LPCMain;
		private System.Windows.Forms.Label LBLIncoming;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.CheckBox CHCopyLibCard;
	}
}