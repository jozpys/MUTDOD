namespace IndexMechanismAdminConsole
{
    partial class SearchForm
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.search = new System.Windows.Forms.Button();
            this.Atrybut = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Porownanie = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Wartosc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SearchObjectsNOAttribiutesNeeded = new System.Windows.Forms.Button();
            this.complexExtension = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.CausesValidation = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(41, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(344, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Klasa:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.CausesValidation = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Atrybut,
            this.Porownanie,
            this.Wartosc});
            this.dataGridView1.Location = new System.Drawing.Point(11, 75);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.Size = new System.Drawing.Size(374, 214);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dataGridView1_RowsAdded);
            // 
            // search
            // 
            this.search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.search.Location = new System.Drawing.Point(310, 295);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(75, 23);
            this.search.TabIndex = 3;
            this.search.Text = "Wyszukaj";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // Atrybut
            // 
            this.Atrybut.HeaderText = "Atrybut";
            this.Atrybut.Name = "Atrybut";
            // 
            // Porownanie
            // 
            this.Porownanie.HeaderText = "Porównanie";
            this.Porownanie.Name = "Porownanie";
            // 
            // Wartosc
            // 
            this.Wartosc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Wartosc.HeaderText = "Wartość";
            this.Wartosc.Name = "Wartosc";
            // 
            // SearchObjectsNOAttribiutesNeeded
            // 
            this.SearchObjectsNOAttribiutesNeeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchObjectsNOAttribiutesNeeded.Location = new System.Drawing.Point(180, 39);
            this.SearchObjectsNOAttribiutesNeeded.Name = "SearchObjectsNOAttribiutesNeeded";
            this.SearchObjectsNOAttribiutesNeeded.Size = new System.Drawing.Size(205, 23);
            this.SearchObjectsNOAttribiutesNeeded.TabIndex = 4;
            this.SearchObjectsNOAttribiutesNeeded.Text = "Wyszukaj wszystkie obiekty dnego typu";
            this.SearchObjectsNOAttribiutesNeeded.UseVisualStyleBackColor = true;
            this.SearchObjectsNOAttribiutesNeeded.Click += new System.EventHandler(this.SearchObjectsNOAttribiutesNeeded_Click);
            // 
            // complexExtension
            // 
            this.complexExtension.AutoSize = true;
            this.complexExtension.Location = new System.Drawing.Point(13, 44);
            this.complexExtension.Name = "complexExtension";
            this.complexExtension.Size = new System.Drawing.Size(113, 17);
            this.complexExtension.TabIndex = 5;
            this.complexExtension.Text = "Ekstensja złożona";
            this.complexExtension.UseVisualStyleBackColor = true;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 320);
            this.Controls.Add(this.complexExtension);
            this.Controls.Add(this.SearchObjectsNOAttribiutesNeeded);
            this.Controls.Add(this.search);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.DataGridViewComboBoxColumn Atrybut;
        private System.Windows.Forms.DataGridViewComboBoxColumn Porownanie;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wartosc;
        private System.Windows.Forms.Button SearchObjectsNOAttribiutesNeeded;
        private System.Windows.Forms.CheckBox complexExtension;
    }
}