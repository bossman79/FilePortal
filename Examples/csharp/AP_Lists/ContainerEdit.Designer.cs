namespace AP_Lists
{
	partial class ContainerEdit
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
			this.label2 = new System.Windows.Forms.Label();
			this.TBName = new System.Windows.Forms.TextBox();
			this.TBComment = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(197, 27);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 2;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(197, 56);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 3;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(2, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(2, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Comment";
			// 
			// TBName
			// 
			this.TBName.Location = new System.Drawing.Point(5, 25);
			this.TBName.Name = "TBName";
			this.TBName.Size = new System.Drawing.Size(176, 20);
			this.TBName.TabIndex = 0;
			this.TBName.TextChanged += new System.EventHandler(this.TBName_TextChanged);
			// 
			// TBComment
			// 
			this.TBComment.Location = new System.Drawing.Point(5, 64);
			this.TBComment.Name = "TBComment";
			this.TBComment.Size = new System.Drawing.Size(176, 20);
			this.TBComment.TabIndex = 1;
			this.TBComment.TextChanged += new System.EventHandler(this.TBComment_TextChanged);
			// 
			// ContainerEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 96);
			this.Controls.Add(this.TBComment);
			this.Controls.Add(this.TBName);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ContainerEdit";
			this.Text = "ListEdit";
			this.Load += new System.EventHandler(this.ListEdit_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox TBName;
		private System.Windows.Forms.TextBox TBComment;
	}
}