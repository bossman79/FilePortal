namespace AP_Lists
{
	partial class ListsForm
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
			this.CBListType = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.LVMain = new System.Windows.Forms.ListView();
			this.BTAdd = new System.Windows.Forms.Button();
			this.BTEdit = new System.Windows.Forms.Button();
			this.BTDelete = new System.Windows.Forms.Button();
			this.BTEditContents = new System.Windows.Forms.Button();
			this.BTClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// CBListType
			// 
			this.CBListType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBListType.FormattingEnabled = true;
			this.CBListType.Location = new System.Drawing.Point(12, 32);
			this.CBListType.Name = "CBListType";
			this.CBListType.Size = new System.Drawing.Size(259, 21);
			this.CBListType.TabIndex = 0;
			this.CBListType.SelectedIndexChanged += new System.EventHandler(this.CBListType_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(50, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "List Type";
			// 
			// LVMain
			// 
			this.LVMain.FullRowSelect = true;
			this.LVMain.HideSelection = false;
			this.LVMain.Location = new System.Drawing.Point(12, 71);
			this.LVMain.Name = "LVMain";
			this.LVMain.Size = new System.Drawing.Size(473, 454);
			this.LVMain.TabIndex = 2;
			this.LVMain.UseCompatibleStateImageBehavior = false;
			this.LVMain.View = System.Windows.Forms.View.Details;
			this.LVMain.SelectedIndexChanged += new System.EventHandler(this.LVMain_SelectedIndexChanged);
			// 
			// BTAdd
			// 
			this.BTAdd.Location = new System.Drawing.Point(501, 128);
			this.BTAdd.Name = "BTAdd";
			this.BTAdd.Size = new System.Drawing.Size(92, 23);
			this.BTAdd.TabIndex = 3;
			this.BTAdd.Text = "Add";
			this.BTAdd.UseVisualStyleBackColor = true;
			this.BTAdd.Click += new System.EventHandler(this.BTAdd_Click);
			// 
			// BTEdit
			// 
			this.BTEdit.Location = new System.Drawing.Point(501, 177);
			this.BTEdit.Name = "BTEdit";
			this.BTEdit.Size = new System.Drawing.Size(92, 23);
			this.BTEdit.TabIndex = 4;
			this.BTEdit.Text = "Edit";
			this.BTEdit.UseVisualStyleBackColor = true;
			this.BTEdit.Click += new System.EventHandler(this.BTEdit_Click);
			// 
			// BTDelete
			// 
			this.BTDelete.Location = new System.Drawing.Point(501, 275);
			this.BTDelete.Name = "BTDelete";
			this.BTDelete.Size = new System.Drawing.Size(92, 23);
			this.BTDelete.TabIndex = 5;
			this.BTDelete.Text = "Delete";
			this.BTDelete.UseVisualStyleBackColor = true;
			this.BTDelete.Click += new System.EventHandler(this.BTDelete_Click);
			// 
			// BTEditContents
			// 
			this.BTEditContents.Location = new System.Drawing.Point(501, 226);
			this.BTEditContents.Name = "BTEditContents";
			this.BTEditContents.Size = new System.Drawing.Size(92, 23);
			this.BTEditContents.TabIndex = 6;
			this.BTEditContents.Text = "Edit Contents";
			this.BTEditContents.UseVisualStyleBackColor = true;
			this.BTEditContents.Click += new System.EventHandler(this.BTEditContents_Click);
			// 
			// BTClose
			// 
			this.BTClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTClose.Location = new System.Drawing.Point(512, 9);
			this.BTClose.Name = "BTClose";
			this.BTClose.Size = new System.Drawing.Size(92, 23);
			this.BTClose.TabIndex = 9;
			this.BTClose.Text = "Close";
			this.BTClose.UseVisualStyleBackColor = true;
			// 
			// ListsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(616, 537);
			this.Controls.Add(this.BTClose);
			this.Controls.Add(this.BTEditContents);
			this.Controls.Add(this.BTDelete);
			this.Controls.Add(this.BTEdit);
			this.Controls.Add(this.BTAdd);
			this.Controls.Add(this.LVMain);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.CBListType);
			this.MinimumSize = new System.Drawing.Size(500, 400);
			this.Name = "ListsForm";
			this.Text = "Lists Form";
			this.Load += new System.EventHandler(this.ListsForm_Load);
			this.Resize += new System.EventHandler(this.ListsForm_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox CBListType;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView LVMain;
		private System.Windows.Forms.Button BTAdd;
		private System.Windows.Forms.Button BTEdit;
		private System.Windows.Forms.Button BTDelete;
		private System.Windows.Forms.Button BTEditContents;
		private System.Windows.Forms.Button BTClose;

	}
}