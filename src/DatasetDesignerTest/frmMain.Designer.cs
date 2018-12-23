namespace DatasetDesignerTest
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TestBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.TestDataSet1 = new DatasetDesignerTest.TESTDataSet1();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.TestTableAdapter = new DatasetDesignerTest.TESTDataSet1TableAdapters.TESTTableAdapter();
            this.button3 = new System.Windows.Forms.Button();
            this.database2DataSet = new DatasetDesignerTest.Database2DataSet();
            this.tESTBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.tESTTableAdapter1 = new DatasetDesignerTest.Database2DataSetTableAdapters.TESTTableAdapter();
            this.識別碼DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mYNAMEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.database2DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tESTBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.識別碼DataGridViewTextBoxColumn,
            this.mYNAMEDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.tESTBindingSource1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(500, 425);
            this.dataGridView1.TabIndex = 0;
            // 
            // TestBindingSource
            // 
            this.TestBindingSource.DataMember = "TEST";
            this.TestBindingSource.DataSource = this.TestDataSet1;
            // 
            // TestDataSet1
            // 
            this.TestDataSet1.DataSetName = "TESTDataSet1";
            this.TestDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(536, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 44);
            this.button1.TabIndex = 1;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(536, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 44);
            this.button2.TabIndex = 1;
            this.button2.Text = "Don\'t Connect DB";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TestTableAdapter
            // 
            this.TestTableAdapter.ClearBeforeFill = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(536, 114);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(158, 44);
            this.button3.TabIndex = 2;
            this.button3.Text = "Access DB";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // database2DataSet
            // 
            this.database2DataSet.DataSetName = "Database2DataSet";
            this.database2DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tESTBindingSource1
            // 
            this.tESTBindingSource1.DataMember = "TEST";
            this.tESTBindingSource1.DataSource = this.database2DataSet;
            // 
            // tESTTableAdapter1
            // 
            this.tESTTableAdapter1.ClearBeforeFill = true;
            // 
            // 識別碼DataGridViewTextBoxColumn
            // 
            this.識別碼DataGridViewTextBoxColumn.DataPropertyName = "識別碼";
            this.識別碼DataGridViewTextBoxColumn.HeaderText = "識別碼";
            this.識別碼DataGridViewTextBoxColumn.Name = "識別碼DataGridViewTextBoxColumn";
            this.識別碼DataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // mYNAMEDataGridViewTextBoxColumn
            // 
            this.mYNAMEDataGridViewTextBoxColumn.DataPropertyName = "MY_NAME";
            this.mYNAMEDataGridViewTextBoxColumn.HeaderText = "MY_NAME";
            this.mYNAMEDataGridViewTextBoxColumn.Name = "mYNAMEDataGridViewTextBoxColumn";
            this.mYNAMEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 450);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmMain";
            this.Text = "Dataset Designer Test";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TestDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.database2DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tESTBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private TESTDataSet1 TestDataSet1;
        private System.Windows.Forms.BindingSource TestBindingSource;
        private TESTDataSet1TableAdapters.TESTTableAdapter TestTableAdapter;
        private System.Windows.Forms.Button button3;
        private Database2DataSet database2DataSet;
        private System.Windows.Forms.BindingSource tESTBindingSource1;
        private Database2DataSetTableAdapters.TESTTableAdapter tESTTableAdapter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 識別碼DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mYNAMEDataGridViewTextBoxColumn;

    }
}

