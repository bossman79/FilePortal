namespace AP_LibCard
{
	partial class LibCardConfig
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
            this.TBFieldsToDisallow = new System.Windows.Forms.TextBox();
            this.BTOK = new System.Windows.Forms.Button();
            this.BTCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TBFieldsToCustomUi = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TBLoseFocus = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TBLoseFocusUpdate = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TBFieldsToDisallow
            // 
            this.TBFieldsToDisallow.Location = new System.Drawing.Point(13, 27);
            this.TBFieldsToDisallow.Multiline = true;
            this.TBFieldsToDisallow.Name = "TBFieldsToDisallow";
            this.TBFieldsToDisallow.Size = new System.Drawing.Size(259, 153);
            this.TBFieldsToDisallow.TabIndex = 0;
            // 
            // BTOK
            // 
            this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTOK.Location = new System.Drawing.Point(410, 12);
            this.BTOK.Name = "BTOK";
            this.BTOK.Size = new System.Drawing.Size(75, 23);
            this.BTOK.TabIndex = 1;
            this.BTOK.Text = "OK";
            this.BTOK.UseVisualStyleBackColor = true;
            this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
            // 
            // BTCancel
            // 
            this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BTCancel.Location = new System.Drawing.Point(410, 41);
            this.BTCancel.Name = "BTCancel";
            this.BTCancel.Size = new System.Drawing.Size(75, 23);
            this.BTCancel.TabIndex = 2;
            this.BTCancel.Text = "Cancel";
            this.BTCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Add Field Names to Disallow Edit (s_basefld from fm100schema)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 211);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(317, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Add Field Names to use Custom UI (s_basefld from fm100schema)";
            // 
            // TBFieldsToCustomUi
            // 
            this.TBFieldsToCustomUi.Location = new System.Drawing.Point(13, 227);
            this.TBFieldsToCustomUi.Multiline = true;
            this.TBFieldsToCustomUi.Name = "TBFieldsToCustomUi";
            this.TBFieldsToCustomUi.Size = new System.Drawing.Size(259, 153);
            this.TBFieldsToCustomUi.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 400);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "When this field loses focus,";
            // 
            // TBLoseFocus
            // 
            this.TBLoseFocus.Location = new System.Drawing.Point(172, 397);
            this.TBLoseFocus.Name = "TBLoseFocus";
            this.TBLoseFocus.Size = new System.Drawing.Size(100, 20);
            this.TBLoseFocus.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 431);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Update the value in this field.";
            // 
            // TBLoseFocusUpdate
            // 
            this.TBLoseFocusUpdate.Location = new System.Drawing.Point(172, 428);
            this.TBLoseFocusUpdate.Name = "TBLoseFocusUpdate";
            this.TBLoseFocusUpdate.Size = new System.Drawing.Size(100, 20);
            this.TBLoseFocusUpdate.TabIndex = 9;
            // 
            // LibCardConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 477);
            this.Controls.Add(this.TBLoseFocusUpdate);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TBLoseFocus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TBFieldsToCustomUi);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTCancel);
            this.Controls.Add(this.BTOK);
            this.Controls.Add(this.TBFieldsToDisallow);
            this.Name = "LibCardConfig";
            this.Text = "LibCardConfig";
            this.Load += new System.EventHandler(this.LibCardConfig_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox TBFieldsToDisallow;
		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TBFieldsToCustomUi;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TBLoseFocus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TBLoseFocusUpdate;
	}
}