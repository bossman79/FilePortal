namespace AP_LibCard
{
	partial class CustomUi
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
			this.BTOK = new System.Windows.Forms.Button();
			this.BTCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.TBFieldName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.TBNewValue = new System.Windows.Forms.TextBox();
			this.CBOtherField = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.TBOtherValue = new System.Windows.Forms.TextBox();
			this.BTApply = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(372, 12);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 0;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(372, 41);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 1;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			this.BTCancel.Click += new System.EventHandler(this.BTCancel_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(166, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Field Name to edit with Custom UI";
			// 
			// TBFieldName
			// 
			this.TBFieldName.Location = new System.Drawing.Point(12, 25);
			this.TBFieldName.Name = "TBFieldName";
			this.TBFieldName.ReadOnly = true;
			this.TBFieldName.Size = new System.Drawing.Size(166, 20);
			this.TBFieldName.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 61);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(202, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "New Value - Press OK to return this value";
			// 
			// TBNewValue
			// 
			this.TBNewValue.Location = new System.Drawing.Point(12, 77);
			this.TBNewValue.Name = "TBNewValue";
			this.TBNewValue.Size = new System.Drawing.Size(163, 20);
			this.TBNewValue.TabIndex = 5;
			// 
			// CBOtherField
			// 
			this.CBOtherField.FormattingEnabled = true;
			this.CBOtherField.Location = new System.Drawing.Point(15, 41);
			this.CBOtherField.Name = "CBOtherField";
			this.CBOtherField.Size = new System.Drawing.Size(163, 21);
			this.CBOtherField.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Field Name";
			// 
			// TBOtherValue
			// 
			this.TBOtherValue.Location = new System.Drawing.Point(15, 96);
			this.TBOtherValue.Name = "TBOtherValue";
			this.TBOtherValue.Size = new System.Drawing.Size(163, 20);
			this.TBOtherValue.TabIndex = 8;
			// 
			// BTApply
			// 
			this.BTApply.Location = new System.Drawing.Point(194, 93);
			this.BTApply.Name = "BTApply";
			this.BTApply.Size = new System.Drawing.Size(75, 23);
			this.BTApply.TabIndex = 9;
			this.BTApply.Text = "Apply";
			this.BTApply.UseVisualStyleBackColor = true;
			this.BTApply.Click += new System.EventHandler(this.BTApply_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.BTApply);
			this.groupBox1.Controls.Add(this.CBOtherField);
			this.groupBox1.Controls.Add(this.TBOtherValue);
			this.groupBox1.Location = new System.Drawing.Point(15, 120);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(295, 152);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Other Field to Update";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 80);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "New Value";
			// 
			// CustomUi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(459, 284);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.TBNewValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TBFieldName);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Name = "CustomUi";
			this.Text = "Custom UI Form";
			this.Load += new System.EventHandler(this.CustomUi_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TBFieldName;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TBNewValue;
		private System.Windows.Forms.ComboBox CBOtherField;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox TBOtherValue;
		private System.Windows.Forms.Button BTApply;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
	}
}