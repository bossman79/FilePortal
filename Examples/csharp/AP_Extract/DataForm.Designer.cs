namespace AP_Extract
{
	partial class DataForm
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
			this.EBData2 = new System.Windows.Forms.TextBox();
			this.EBData1 = new System.Windows.Forms.TextBox();
			this.CBType = new System.Windows.Forms.ComboBox();
			this.lblData2 = new System.Windows.Forms.Label();
			this.lblData1 = new System.Windows.Forms.Label();
			this.lblType = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(396, 53);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 25;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.Click += new System.EventHandler(this.BTCancel_Click);
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(396, 21);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 24;
			this.BTOK.Text = "OK";
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// EBData2
			// 
			this.EBData2.Location = new System.Drawing.Point(148, 73);
			this.EBData2.Name = "EBData2";
			this.EBData2.Size = new System.Drawing.Size(224, 20);
			this.EBData2.TabIndex = 23;
			this.EBData2.Text = "EBData2";
			this.EBData2.TextChanged += new System.EventHandler(this.EBData2_TextChanged);
			// 
			// EBData1
			// 
			this.EBData1.Location = new System.Drawing.Point(148, 41);
			this.EBData1.Name = "EBData1";
			this.EBData1.Size = new System.Drawing.Size(224, 20);
			this.EBData1.TabIndex = 22;
			this.EBData1.Text = "EBData1";
			this.EBData1.TextChanged += new System.EventHandler(this.EBData1_TextChanged);
			// 
			// CBType
			// 
			this.CBType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBType.Location = new System.Drawing.Point(148, 9);
			this.CBType.Name = "CBType";
			this.CBType.Size = new System.Drawing.Size(224, 21);
			this.CBType.TabIndex = 21;
			this.CBType.SelectedIndexChanged += new System.EventHandler(this.CBType_SelectedIndexChanged);
			// 
			// lblData2
			// 
			this.lblData2.Location = new System.Drawing.Point(12, 73);
			this.lblData2.Name = "lblData2";
			this.lblData2.Size = new System.Drawing.Size(136, 23);
			this.lblData2.TabIndex = 20;
			this.lblData2.Text = "Item";
			// 
			// lblData1
			// 
			this.lblData1.Location = new System.Drawing.Point(12, 41);
			this.lblData1.Name = "lblData1";
			this.lblData1.Size = new System.Drawing.Size(136, 23);
			this.lblData1.TabIndex = 19;
			this.lblData1.Text = "Section";
			// 
			// lblType
			// 
			this.lblType.Location = new System.Drawing.Point(12, 9);
			this.lblType.Name = "lblType";
			this.lblType.Size = new System.Drawing.Size(136, 23);
			this.lblType.TabIndex = 18;
			this.lblType.Text = "Type";
			// 
			// DataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(495, 104);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Controls.Add(this.EBData2);
			this.Controls.Add(this.EBData1);
			this.Controls.Add(this.CBType);
			this.Controls.Add(this.lblData2);
			this.Controls.Add(this.lblData1);
			this.Controls.Add(this.lblType);
			this.Name = "DataForm";
			this.Text = "Data Form";
			this.Load += new System.EventHandler(this.DataForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		internal System.Windows.Forms.Button BTCancel;
		internal System.Windows.Forms.Button BTOK;
		internal System.Windows.Forms.TextBox EBData2;
		internal System.Windows.Forms.TextBox EBData1;
		internal System.Windows.Forms.ComboBox CBType;
		internal System.Windows.Forms.Label lblData2;
		internal System.Windows.Forms.Label lblData1;
		internal System.Windows.Forms.Label lblType;
	}
}