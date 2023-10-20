namespace KKTCMB
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var dataGridViewCellStyle1 = new DataGridViewCellStyle();
            var dataGridViewCellStyle2 = new DataGridViewCellStyle();
            dataGridView = new DataGridView();
            Birim = new DataGridViewTextBoxColumn();
            Sembol = new DataGridViewTextBoxColumn();
            İsim = new DataGridViewTextBoxColumn();
            Alış = new DataGridViewTextBoxColumn();
            Satış = new DataGridViewTextBoxColumn();
            EAlış = new DataGridViewTextBoxColumn();
            ESatış = new DataGridViewTextBoxColumn();
            dateGroupBox = new GroupBox();
            dateTimePicker = new DateTimePicker();
            getButton = new Button();
            showButton = new Button();
            saveButton = new Button();
            dataGroupBox = new GroupBox();
            appGroupBox = new GroupBox();
            exitButton = new Button();
            aboutButton = new Button();
            sourceGroupBox = new GroupBox();
            sourceComboBox = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            dateGroupBox.SuspendLayout();
            dataGroupBox.SuspendLayout();
            appGroupBox.SuspendLayout();
            sourceGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToResizeColumns = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Columns.AddRange(new DataGridViewColumn[] { Birim, Sembol, İsim, Alış, Satış, EAlış, ESatış });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView.Location = new Point(12, 75);
            dataGridView.MultiSelect = false;
            dataGridView.Name = "dataGridView";
            dataGridView.ReadOnly = true;
            dataGridView.RowTemplate.Height = 25;
            dataGridView.Size = new Size(779, 325);
            dataGridView.TabIndex = 7;
            // 
            // Birim
            // 
            Birim.HeaderText = "Birim";
            Birim.Name = "Birim";
            Birim.ReadOnly = true;
            Birim.Resizable = DataGridViewTriState.False;
            Birim.Width = 65;
            // 
            // Sembol
            // 
            Sembol.HeaderText = "Sembol";
            Sembol.Name = "Sembol";
            Sembol.ReadOnly = true;
            Sembol.Resizable = DataGridViewTriState.False;
            Sembol.Width = 65;
            // 
            // İsim
            // 
            İsim.HeaderText = "İsim";
            İsim.Name = "İsim";
            İsim.ReadOnly = true;
            İsim.Resizable = DataGridViewTriState.False;
            İsim.Width = 169;
            // 
            // Alış
            // 
            Alış.HeaderText = "Döviz Alış";
            Alış.Name = "Alış";
            Alış.ReadOnly = true;
            Alış.Resizable = DataGridViewTriState.False;
            Alış.Width = 105;
            // 
            // Satış
            // 
            Satış.HeaderText = "Döviz Satış";
            Satış.Name = "Satış";
            Satış.ReadOnly = true;
            Satış.Resizable = DataGridViewTriState.False;
            Satış.Width = 105;
            // 
            // EAlış
            // 
            EAlış.HeaderText = "Efektif Alış";
            EAlış.Name = "EAlış";
            EAlış.ReadOnly = true;
            EAlış.Resizable = DataGridViewTriState.False;
            EAlış.Width = 105;
            // 
            // ESatış
            // 
            ESatış.HeaderText = "Efektif Satış";
            ESatış.Name = "ESatış";
            ESatış.ReadOnly = true;
            ESatış.Resizable = DataGridViewTriState.False;
            ESatış.Width = 105;
            // 
            // dateGroupBox
            // 
            dateGroupBox.Controls.Add(dateTimePicker);
            dateGroupBox.Location = new Point(12, 10);
            dateGroupBox.Name = "dateGroupBox";
            dateGroupBox.Size = new Size(209, 59);
            dateGroupBox.TabIndex = 1;
            dateGroupBox.TabStop = false;
            dateGroupBox.Text = "Tarih";
            // 
            // dateTimePicker
            // 
            dateTimePicker.Location = new Point(6, 22);
            dateTimePicker.MinDate = new DateTime(2001, 11, 27, 0, 0, 0, 0);
            dateTimePicker.Name = "dateTimePicker";
            dateTimePicker.Size = new Size(193, 23);
            dateTimePicker.TabIndex = 0;
            dateTimePicker.Value = new DateTime(2023, 10, 12, 0, 0, 0, 0);
            dateTimePicker.CloseUp += dateTimePicker_CloseUp;
            // 
            // getButton
            // 
            getButton.Location = new Point(6, 22);
            getButton.Name = "getButton";
            getButton.Size = new Size(75, 23);
            getButton.TabIndex = 2;
            getButton.Text = "Al";
            getButton.UseVisualStyleBackColor = true;
            getButton.Click += getButton_Click;
            // 
            // showButton
            // 
            showButton.Enabled = false;
            showButton.Location = new Point(168, 22);
            showButton.Name = "showButton";
            showButton.Size = new Size(75, 23);
            showButton.TabIndex = 4;
            showButton.Text = "Göster";
            showButton.UseVisualStyleBackColor = true;
            showButton.Click += showButton_Click;
            // 
            // saveButton
            // 
            saveButton.Enabled = false;
            saveButton.Location = new Point(87, 22);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 23);
            saveButton.TabIndex = 3;
            saveButton.Text = "Kaydet";
            saveButton.UseVisualStyleBackColor = true;
            saveButton.Click += saveButton_Click;
            // 
            // dataGroupBox
            // 
            dataGroupBox.Controls.Add(getButton);
            dataGroupBox.Controls.Add(saveButton);
            dataGroupBox.Controls.Add(showButton);
            dataGroupBox.Location = new Point(368, 10);
            dataGroupBox.Name = "dataGroupBox";
            dataGroupBox.Size = new Size(249, 59);
            dataGroupBox.TabIndex = 2;
            dataGroupBox.TabStop = false;
            dataGroupBox.Text = "Veriler";
            // 
            // appGroupBox
            // 
            appGroupBox.Controls.Add(exitButton);
            appGroupBox.Controls.Add(aboutButton);
            appGroupBox.Location = new Point(623, 10);
            appGroupBox.Name = "appGroupBox";
            appGroupBox.Size = new Size(168, 59);
            appGroupBox.TabIndex = 3;
            appGroupBox.TabStop = false;
            appGroupBox.Text = "Uygulama";
            // 
            // exitButton
            // 
            exitButton.Location = new Point(87, 22);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(75, 23);
            exitButton.TabIndex = 6;
            exitButton.Text = "Çıkış";
            exitButton.UseVisualStyleBackColor = true;
            exitButton.Click += exitButton_Click;
            // 
            // aboutButton
            // 
            aboutButton.Location = new Point(6, 22);
            aboutButton.Name = "aboutButton";
            aboutButton.Size = new Size(75, 23);
            aboutButton.TabIndex = 5;
            aboutButton.Text = "Hakkında";
            aboutButton.UseVisualStyleBackColor = true;
            aboutButton.Click += aboutButton_Click;
            // 
            // sourceGroupBox
            // 
            sourceGroupBox.Controls.Add(sourceComboBox);
            sourceGroupBox.Location = new Point(227, 10);
            sourceGroupBox.Name = "sourceGroupBox";
            sourceGroupBox.Size = new Size(135, 59);
            sourceGroupBox.TabIndex = 5;
            sourceGroupBox.TabStop = false;
            sourceGroupBox.Text = "Kaynak";
            // 
            // sourceComboBox
            // 
            sourceComboBox.FormattingEnabled = true;
            sourceComboBox.Items.AddRange(new object[] { "KKTCMB", "TCMB" });
            sourceComboBox.Location = new Point(6, 22);
            sourceComboBox.Name = "sourceComboBox";
            sourceComboBox.Size = new Size(121, 23);
            sourceComboBox.TabIndex = 1;
            sourceComboBox.Text = "KKTCMB";
            sourceComboBox.TextChanged += genericValueChangedEvent;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 411);
            Controls.Add(sourceGroupBox);
            Controls.Add(appGroupBox);
            Controls.Add(dataGroupBox);
            Controls.Add(dateGroupBox);
            Controls.Add(dataGridView);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MaximumSize = new Size(815, 450);
            MinimumSize = new Size(815, 450);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Merkez Bankası Döviz Kurları";
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            dateGroupBox.ResumeLayout(false);
            dataGroupBox.ResumeLayout(false);
            appGroupBox.ResumeLayout(false);
            sourceGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView;
        private GroupBox dateGroupBox;
        private Button getButton;
        private Button showButton;
        private DateTimePicker dateTimePicker;
        private Button saveButton;
        private GroupBox dataGroupBox;
        private GroupBox appGroupBox;
        private Button exitButton;
        private Button aboutButton;
        private GroupBox sourceGroupBox;
        private ComboBox sourceComboBox;
        private DataGridViewTextBoxColumn Birim;
        private DataGridViewTextBoxColumn Sembol;
        private DataGridViewTextBoxColumn İsim;
        private DataGridViewTextBoxColumn Alış;
        private DataGridViewTextBoxColumn Satış;
        private DataGridViewTextBoxColumn EAlış;
        private DataGridViewTextBoxColumn ESatış;
    }
}