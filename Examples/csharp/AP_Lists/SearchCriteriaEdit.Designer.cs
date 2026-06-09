namespace AP_Lists
{
	partial class SearchCriteriaEdit
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
			this.label1 = new System.Windows.Forms.Label();
			this.TBValue = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.CBSearchOp = new System.Windows.Forms.ComboBox();
			this.TBValue2 = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(340, 41);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 5;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(340, 12);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 4;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 43);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(34, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Value";
			// 
			// TBValue
			// 
			this.TBValue.Location = new System.Drawing.Point(76, 40);
			this.TBValue.Name = "TBValue";
			this.TBValue.Size = new System.Drawing.Size(235, 20);
			this.TBValue.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 17);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "Search Op";
			// 
			// CBSearchOp
			// 
			this.CBSearchOp.FormattingEnabled = true;
			this.CBSearchOp.Location = new System.Drawing.Point(77, 13);
			this.CBSearchOp.Name = "CBSearchOp";
			this.CBSearchOp.Size = new System.Drawing.Size(234, 21);
			this.CBSearchOp.TabIndex = 8;
			this.CBSearchOp.SelectedIndexChanged += new System.EventHandler(this.CBSearchOp_SelectedIndexChanged);
			// 
			// TBValue2
			// 
			this.TBValue2.Location = new System.Drawing.Point(76, 66);
			this.TBValue2.Name = "TBValue2";
			this.TBValue2.Size = new System.Drawing.Size(235, 20);
			this.TBValue2.TabIndex = 9;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 69);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Value2";
			// 
			// SearchCriteriaEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 98);
			this.Controls.Add(this.TBValue2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.CBSearchOp);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TBValue);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "SearchCriteriaEdit";
			this.Text = "Search Criteria Edit";
			this.Load += new System.EventHandler(this.SearchCriteriaEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TBValue;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox CBSearchOp;
		private System.Windows.Forms.TextBox TBValue2;
		private System.Windows.Forms.Label label3;
	}
}