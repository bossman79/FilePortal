namespace AP_MgrEdit
{
	partial class MgrEditForm
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
			this.BTGo = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.TBFieldId = new System.Windows.Forms.TextBox();
			this.TBValue = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// BTGo
			// 
			this.BTGo.Location = new System.Drawing.Point(288, 80);
			this.BTGo.Name = "BTGo";
			this.BTGo.Size = new System.Drawing.Size(75, 23);
			this.BTGo.TabIndex = 0;
			this.BTGo.Text = "Go";
			this.BTGo.UseVisualStyleBackColor = true;
			this.BTGo.Click += new System.EventHandler(this.BTGo_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(287, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Field Id of a restriced field ( a value from fm*fields.s_FieldId )";
			// 
			// TBFieldId
			// 
			this.TBFieldId.Location = new System.Drawing.Point(15, 32);
			this.TBFieldId.Name = "TBFieldId";
			this.TBFieldId.Size = new System.Drawing.Size(248, 20);
			this.TBFieldId.TabIndex = 3;
			// 
			// TBValue
			// 
			this.TBValue.Location = new System.Drawing.Point(15, 83);
			this.TBValue.Name = "TBValue";
			this.TBValue.Size = new System.Drawing.Size(248, 20);
			this.TBValue.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 60);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(119, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Restricted Value to Add";
			// 
			// MgrEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(390, 136);
			this.Controls.Add(this.TBValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.TBFieldId);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BTGo);
			this.Name = "MgrEditForm";
			this.Text = "Mgr Edit";
			this.Load += new System.EventHandler(this.MgrEditForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BTGo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TBFieldId;
		private System.Windows.Forms.TextBox TBValue;
		private System.Windows.Forms.Label label2;
	}
}