namespace BombonesApp2026.Windows
{
    partial class frmEmpleadoAe
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
            components = new System.ComponentModel.Container();
            btnCancelar = new Button();
            btnOK = new Button();
            groupBox2 = new GroupBox();
            txtDocumento = new TextBox();
            txtApellido = new TextBox();
            label3 = new Label();
            label2 = new Label();
            txtNombre = new TextBox();
            label1 = new Label();
            groupBox1 = new GroupBox();
            cboLocalidad = new ComboBox();
            cboProvincia = new ComboBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox1 = new TextBox();
            label9 = new Label();
            txtEmail = new TextBox();
            label8 = new Label();
            label10 = new Label();
            label7 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtTelefono = new TextBox();
            label6 = new Label();
            errorProvider1 = new ErrorProvider(components);
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // btnCancelar
            // 
            btnCancelar.Image = Properties.Resources.cancel_24px;
            btnCancelar.Location = new Point(653, 416);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(75, 60);
            btnCancelar.TabIndex = 6;
            btnCancelar.Text = "Cancelar";
            btnCancelar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnCancelar.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            btnOK.Image = Properties.Resources.ok_24px;
            btnOK.Location = new Point(77, 416);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(75, 60);
            btnOK.TabIndex = 5;
            btnOK.Text = "OK";
            btnOK.TextImageRelation = TextImageRelation.ImageAboveText;
            btnOK.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtDocumento);
            groupBox2.Controls.Add(txtApellido);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(txtNombre);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(32, 22);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(565, 125);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Datos del Empleado";
            // 
            // txtDocumento
            // 
            txtDocumento.Location = new Point(96, 82);
            txtDocumento.MaxLength = 20;
            txtDocumento.Name = "txtDocumento";
            txtDocumento.Size = new Size(402, 23);
            txtDocumento.TabIndex = 1;
            // 
            // txtApellido
            // 
            txtApellido.Location = new Point(96, 50);
            txtApellido.MaxLength = 50;
            txtApellido.Name = "txtApellido";
            txtApellido.Size = new Size(402, 23);
            txtApellido.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(17, 85);
            label3.Name = "label3";
            label3.Size = new Size(73, 15);
            label3.TabIndex = 0;
            label3.Text = "Documento:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(17, 53);
            label2.Name = "label2";
            label2.Size = new Size(54, 15);
            label2.TabIndex = 0;
            label2.Text = "Apellido:";
            // 
            // txtNombre
            // 
            txtNombre.Location = new Point(96, 21);
            txtNombre.MaxLength = 50;
            txtNombre.Name = "txtNombre";
            txtNombre.Size = new Size(402, 23);
            txtNombre.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(17, 24);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 0;
            label1.Text = "Nombre:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cboLocalidad);
            groupBox1.Controls.Add(cboProvincia);
            groupBox1.Controls.Add(textBox2);
            groupBox1.Controls.Add(textBox3);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(label9);
            groupBox1.Controls.Add(txtEmail);
            groupBox1.Controls.Add(label8);
            groupBox1.Controls.Add(label10);
            groupBox1.Controls.Add(label7);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(txtTelefono);
            groupBox1.Controls.Add(label6);
            groupBox1.Location = new Point(32, 162);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(799, 209);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = " Datos Optativos del Cliente";
            // 
            // cboLocalidad
            // 
            cboLocalidad.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocalidad.FormattingEnabled = true;
            cboLocalidad.Location = new Point(99, 156);
            cboLocalidad.Name = "cboLocalidad";
            cboLocalidad.Size = new Size(399, 23);
            cboLocalidad.TabIndex = 2;
            // 
            // cboProvincia
            // 
            cboProvincia.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProvincia.FormattingEnabled = true;
            cboProvincia.Location = new Point(99, 125);
            cboProvincia.Name = "cboProvincia";
            cboProvincia.Size = new Size(399, 23);
            cboProvincia.TabIndex = 2;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(602, 82);
            textBox2.MaxLength = 20;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(141, 23);
            textBox2.TabIndex = 1;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(602, 159);
            textBox3.MaxLength = 20;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(141, 23);
            textBox3.TabIndex = 1;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(99, 82);
            textBox1.MaxLength = 20;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(399, 23);
            textBox1.TabIndex = 1;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(19, 124);
            label9.Name = "label9";
            label9.Size = new Size(59, 15);
            label9.TabIndex = 0;
            label9.Text = "Provincia:";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(99, 50);
            txtEmail.MaxLength = 50;
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(399, 23);
            txtEmail.TabIndex = 1;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(17, 159);
            label8.Name = "label8";
            label8.Size = new Size(61, 15);
            label8.TabIndex = 0;
            label8.Text = "Localidad:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(526, 162);
            label10.Name = "label10";
            label10.Size = new Size(70, 15);
            label10.TabIndex = 0;
            label10.Text = "Cod. Postal:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(520, 85);
            label7.Name = "label7";
            label7.Size = new Size(54, 15);
            label7.TabIndex = 0;
            label7.Text = "Número:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(17, 85);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 0;
            label4.Text = "Calle:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 53);
            label5.Name = "label5";
            label5.Size = new Size(39, 15);
            label5.TabIndex = 0;
            label5.Text = "Email:";
            // 
            // txtTelefono
            // 
            txtTelefono.Location = new Point(99, 21);
            txtTelefono.MaxLength = 20;
            txtTelefono.Name = "txtTelefono";
            txtTelefono.Size = new Size(399, 23);
            txtTelefono.TabIndex = 1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(17, 24);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 0;
            label6.Text = "Teléfono:";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // frmEmpleadoAe
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(890, 527);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Controls.Add(btnCancelar);
            Controls.Add(btnOK);
            MaximumSize = new Size(906, 566);
            MinimumSize = new Size(906, 566);
            Name = "frmEmpleadoAe";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "frmEmpleadoAe";
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnCancelar;
        private Button btnOK;
        private GroupBox groupBox2;
        private TextBox txtDocumento;
        private TextBox txtApellido;
        private Label label3;
        private Label label2;
        private TextBox txtNombre;
        private Label label1;
        private GroupBox groupBox1;
        private TextBox textBox1;
        private TextBox txtEmail;
        private Label label4;
        private Label label5;
        private TextBox txtTelefono;
        private Label label6;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label label9;
        private Label label8;
        private Label label10;
        private Label label7;
        private ErrorProvider errorProvider1;
        private ComboBox cboLocalidad;
        private ComboBox cboProvincia;
    }
}