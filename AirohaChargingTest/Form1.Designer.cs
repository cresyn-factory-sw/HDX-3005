namespace AirohaChargingTest
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tb_LvCcCurrentAtVbat = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_HvCvCurrentAtVbat = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lb_lv_current_vbus_upper = new System.Windows.Forms.Label();
            this.lb_lv_current_vbus_lower = new System.Windows.Forms.Label();
            this.lb_lv_current_vbat_upper = new System.Windows.Forms.Label();
            this.lb_lv_current_vbat_lower = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_LvCcCurrentAtVbus = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_hv_current_vbus_upper = new System.Windows.Forms.Label();
            this.lb_hv_current_vbus_lower = new System.Windows.Forms.Label();
            this.lb_hv_current_vbat_upper = new System.Windows.Forms.Label();
            this.lb_hv_current_vbat_lower = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_HvCvCurrentAtVbus = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lbResult = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lb_lowtemp_upper = new System.Windows.Forms.Label();
            this.lb_lowtemp_lower = new System.Windows.Forms.Label();
            this.lb_normaltemp_upper = new System.Windows.Forms.Label();
            this.lb_normaltemp_lower = new System.Windows.Forms.Label();
            this.lb_hightemp_upper = new System.Windows.Forms.Label();
            this.lb_hightemp_lower = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_resultThmLow = new System.Windows.Forms.TextBox();
            this.tb_resultThmNormal = new System.Windows.Forms.TextBox();
            this.tb_resultThmHigh = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lb_sleep_current_upper = new System.Windows.Forms.Label();
            this.lb_sleep_current_lower = new System.Windows.Forms.Label();
            this.tb_SleepCurrentAtVbat = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current at Vbat (A)";
            // 
            // tb_LvCcCurrentAtVbat
            // 
            this.tb_LvCcCurrentAtVbat.Location = new System.Drawing.Point(230, 27);
            this.tb_LvCcCurrentAtVbat.Name = "tb_LvCcCurrentAtVbat";
            this.tb_LvCcCurrentAtVbat.Size = new System.Drawing.Size(100, 21);
            this.tb_LvCcCurrentAtVbat.TabIndex = 3;
            this.tb_LvCcCurrentAtVbat.TabStop = false;
            this.tb_LvCcCurrentAtVbat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current at Vbat (A)";
            // 
            // tb_HvCvCurrentAtVbat
            // 
            this.tb_HvCvCurrentAtVbat.Location = new System.Drawing.Point(230, 30);
            this.tb_HvCvCurrentAtVbat.Name = "tb_HvCvCurrentAtVbat";
            this.tb_HvCvCurrentAtVbat.Size = new System.Drawing.Size(100, 21);
            this.tb_HvCvCurrentAtVbat.TabIndex = 3;
            this.tb_HvCvCurrentAtVbat.TabStop = false;
            this.tb_HvCvCurrentAtVbat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lb_lv_current_vbus_upper);
            this.groupBox1.Controls.Add(this.lb_lv_current_vbus_lower);
            this.groupBox1.Controls.Add(this.lb_lv_current_vbat_upper);
            this.groupBox1.Controls.Add(this.lb_lv_current_vbat_lower);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.tb_LvCcCurrentAtVbus);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tb_LvCcCurrentAtVbat);
            this.groupBox1.Location = new System.Drawing.Point(12, 91);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(421, 102);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Low Voltage(C.C.)";
            // 
            // lb_lv_current_vbus_upper
            // 
            this.lb_lv_current_vbus_upper.AutoSize = true;
            this.lb_lv_current_vbus_upper.Location = new System.Drawing.Point(348, 68);
            this.lb_lv_current_vbus_upper.Name = "lb_lv_current_vbus_upper";
            this.lb_lv_current_vbus_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_lv_current_vbus_upper.TabIndex = 4;
            this.lb_lv_current_vbus_upper.Text = "label5";
            // 
            // lb_lv_current_vbus_lower
            // 
            this.lb_lv_current_vbus_lower.AutoSize = true;
            this.lb_lv_current_vbus_lower.Location = new System.Drawing.Point(170, 68);
            this.lb_lv_current_vbus_lower.Name = "lb_lv_current_vbus_lower";
            this.lb_lv_current_vbus_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_lv_current_vbus_lower.TabIndex = 4;
            this.lb_lv_current_vbus_lower.Text = "label5";
            // 
            // lb_lv_current_vbat_upper
            // 
            this.lb_lv_current_vbat_upper.AutoSize = true;
            this.lb_lv_current_vbat_upper.Location = new System.Drawing.Point(348, 30);
            this.lb_lv_current_vbat_upper.Name = "lb_lv_current_vbat_upper";
            this.lb_lv_current_vbat_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_lv_current_vbat_upper.TabIndex = 4;
            this.lb_lv_current_vbat_upper.Text = "label5";
            // 
            // lb_lv_current_vbat_lower
            // 
            this.lb_lv_current_vbat_lower.AutoSize = true;
            this.lb_lv_current_vbat_lower.Location = new System.Drawing.Point(170, 30);
            this.lb_lv_current_vbat_lower.Name = "lb_lv_current_vbat_lower";
            this.lb_lv_current_vbat_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_lv_current_vbat_lower.TabIndex = 4;
            this.lb_lv_current_vbat_lower.Text = "label5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current at Vbus (A)";
            // 
            // tb_LvCcCurrentAtVbus
            // 
            this.tb_LvCcCurrentAtVbus.Location = new System.Drawing.Point(230, 65);
            this.tb_LvCcCurrentAtVbus.Name = "tb_LvCcCurrentAtVbus";
            this.tb_LvCcCurrentAtVbus.Size = new System.Drawing.Size(100, 21);
            this.tb_LvCcCurrentAtVbus.TabIndex = 3;
            this.tb_LvCcCurrentAtVbus.TabStop = false;
            this.tb_LvCcCurrentAtVbus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_hv_current_vbus_upper);
            this.groupBox2.Controls.Add(this.lb_hv_current_vbus_lower);
            this.groupBox2.Controls.Add(this.lb_hv_current_vbat_upper);
            this.groupBox2.Controls.Add(this.lb_hv_current_vbat_lower);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tb_HvCvCurrentAtVbat);
            this.groupBox2.Controls.Add(this.tb_HvCvCurrentAtVbus);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(421, 100);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "High Voltage (C.V.)";
            // 
            // lb_hv_current_vbus_upper
            // 
            this.lb_hv_current_vbus_upper.AutoSize = true;
            this.lb_hv_current_vbus_upper.Location = new System.Drawing.Point(348, 71);
            this.lb_hv_current_vbus_upper.Name = "lb_hv_current_vbus_upper";
            this.lb_hv_current_vbus_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_hv_current_vbus_upper.TabIndex = 4;
            this.lb_hv_current_vbus_upper.Text = "label5";
            // 
            // lb_hv_current_vbus_lower
            // 
            this.lb_hv_current_vbus_lower.AutoSize = true;
            this.lb_hv_current_vbus_lower.Location = new System.Drawing.Point(170, 71);
            this.lb_hv_current_vbus_lower.Name = "lb_hv_current_vbus_lower";
            this.lb_hv_current_vbus_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_hv_current_vbus_lower.TabIndex = 4;
            this.lb_hv_current_vbus_lower.Text = "label5";
            // 
            // lb_hv_current_vbat_upper
            // 
            this.lb_hv_current_vbat_upper.AutoSize = true;
            this.lb_hv_current_vbat_upper.Location = new System.Drawing.Point(348, 33);
            this.lb_hv_current_vbat_upper.Name = "lb_hv_current_vbat_upper";
            this.lb_hv_current_vbat_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_hv_current_vbat_upper.TabIndex = 4;
            this.lb_hv_current_vbat_upper.Text = "label5";
            // 
            // lb_hv_current_vbat_lower
            // 
            this.lb_hv_current_vbat_lower.AutoSize = true;
            this.lb_hv_current_vbat_lower.Location = new System.Drawing.Point(170, 33);
            this.lb_hv_current_vbat_lower.Name = "lb_hv_current_vbat_lower";
            this.lb_hv_current_vbat_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_hv_current_vbat_lower.TabIndex = 4;
            this.lb_hv_current_vbat_lower.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Current at Vbus (A)";
            // 
            // tb_HvCvCurrentAtVbus
            // 
            this.tb_HvCvCurrentAtVbus.Location = new System.Drawing.Point(230, 68);
            this.tb_HvCvCurrentAtVbus.Name = "tb_HvCvCurrentAtVbus";
            this.tb_HvCvCurrentAtVbus.Size = new System.Drawing.Size(100, 21);
            this.tb_HvCvCurrentAtVbus.TabIndex = 3;
            this.tb_HvCvCurrentAtVbus.TabStop = false;
            this.tb_HvCvCurrentAtVbus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStart.Location = new System.Drawing.Point(12, 458);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(789, 83);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "START";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lbResult
            // 
            this.lbResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbResult.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbResult.Location = new System.Drawing.Point(439, 12);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(362, 369);
            this.lbResult.TabIndex = 7;
            this.lbResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbStatus
            // 
            this.lbStatus.Font = new System.Drawing.Font("굴림", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbStatus.Location = new System.Drawing.Point(12, 393);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(789, 53);
            this.lbStatus.TabIndex = 8;
            this.lbStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lb_lowtemp_upper);
            this.groupBox3.Controls.Add(this.lb_lowtemp_lower);
            this.groupBox3.Controls.Add(this.lb_normaltemp_upper);
            this.groupBox3.Controls.Add(this.lb_normaltemp_lower);
            this.groupBox3.Controls.Add(this.lb_hightemp_upper);
            this.groupBox3.Controls.Add(this.lb_hightemp_lower);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.tb_resultThmLow);
            this.groupBox3.Controls.Add(this.tb_resultThmNormal);
            this.groupBox3.Controls.Add(this.tb_resultThmHigh);
            this.groupBox3.Location = new System.Drawing.Point(12, 321);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 60);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Thermistor Cut-Off";
            // 
            // lb_lowtemp_upper
            // 
            this.lb_lowtemp_upper.AutoSize = true;
            this.lb_lowtemp_upper.Location = new System.Drawing.Point(348, 95);
            this.lb_lowtemp_upper.Name = "lb_lowtemp_upper";
            this.lb_lowtemp_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_lowtemp_upper.TabIndex = 9;
            this.lb_lowtemp_upper.Text = "label5";
            this.lb_lowtemp_upper.Visible = false;
            // 
            // lb_lowtemp_lower
            // 
            this.lb_lowtemp_lower.AutoSize = true;
            this.lb_lowtemp_lower.Location = new System.Drawing.Point(170, 95);
            this.lb_lowtemp_lower.Name = "lb_lowtemp_lower";
            this.lb_lowtemp_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_lowtemp_lower.TabIndex = 10;
            this.lb_lowtemp_lower.Text = "label5";
            this.lb_lowtemp_lower.Visible = false;
            // 
            // lb_normaltemp_upper
            // 
            this.lb_normaltemp_upper.AutoSize = true;
            this.lb_normaltemp_upper.Location = new System.Drawing.Point(348, 65);
            this.lb_normaltemp_upper.Name = "lb_normaltemp_upper";
            this.lb_normaltemp_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_normaltemp_upper.TabIndex = 7;
            this.lb_normaltemp_upper.Text = "label5";
            this.lb_normaltemp_upper.Visible = false;
            // 
            // lb_normaltemp_lower
            // 
            this.lb_normaltemp_lower.AutoSize = true;
            this.lb_normaltemp_lower.Location = new System.Drawing.Point(170, 65);
            this.lb_normaltemp_lower.Name = "lb_normaltemp_lower";
            this.lb_normaltemp_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_normaltemp_lower.TabIndex = 8;
            this.lb_normaltemp_lower.Text = "label5";
            this.lb_normaltemp_lower.Visible = false;
            // 
            // lb_hightemp_upper
            // 
            this.lb_hightemp_upper.AutoSize = true;
            this.lb_hightemp_upper.Location = new System.Drawing.Point(348, 27);
            this.lb_hightemp_upper.Name = "lb_hightemp_upper";
            this.lb_hightemp_upper.Size = new System.Drawing.Size(38, 12);
            this.lb_hightemp_upper.TabIndex = 5;
            this.lb_hightemp_upper.Text = "label5";
            // 
            // lb_hightemp_lower
            // 
            this.lb_hightemp_lower.AutoSize = true;
            this.lb_hightemp_lower.Location = new System.Drawing.Point(170, 27);
            this.lb_hightemp_lower.Name = "lb_hightemp_lower";
            this.lb_hightemp_lower.Size = new System.Drawing.Size(38, 12);
            this.lb_hightemp_lower.TabIndex = 6;
            this.lb_hightemp_lower.Text = "label5";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "Low Temp. Cut-Off";
            this.label7.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(15, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "Normal Temp.";
            this.label6.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "High Temp. Cut-Off";
            // 
            // tb_resultThmLow
            // 
            this.tb_resultThmLow.Location = new System.Drawing.Point(230, 92);
            this.tb_resultThmLow.Name = "tb_resultThmLow";
            this.tb_resultThmLow.Size = new System.Drawing.Size(100, 21);
            this.tb_resultThmLow.TabIndex = 3;
            this.tb_resultThmLow.TabStop = false;
            this.tb_resultThmLow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_resultThmLow.Visible = false;
            // 
            // tb_resultThmNormal
            // 
            this.tb_resultThmNormal.Location = new System.Drawing.Point(230, 62);
            this.tb_resultThmNormal.Name = "tb_resultThmNormal";
            this.tb_resultThmNormal.Size = new System.Drawing.Size(100, 21);
            this.tb_resultThmNormal.TabIndex = 3;
            this.tb_resultThmNormal.TabStop = false;
            this.tb_resultThmNormal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_resultThmNormal.Visible = false;
            // 
            // tb_resultThmHigh
            // 
            this.tb_resultThmHigh.Location = new System.Drawing.Point(230, 24);
            this.tb_resultThmHigh.Name = "tb_resultThmHigh";
            this.tb_resultThmHigh.Size = new System.Drawing.Size(100, 21);
            this.tb_resultThmHigh.TabIndex = 3;
            this.tb_resultThmHigh.TabStop = false;
            this.tb_resultThmHigh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(715, 386);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "Rev.250922_V1";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lb_sleep_current_upper);
            this.groupBox5.Controls.Add(this.lb_sleep_current_lower);
            this.groupBox5.Controls.Add(this.tb_SleepCurrentAtVbat);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Location = new System.Drawing.Point(12, 12);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(421, 66);
            this.groupBox5.TabIndex = 15;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Leakage Current (uA)";
            // 
            // lb_sleep_current_upper
            // 
            this.lb_sleep_current_upper.AutoSize = true;
            this.lb_sleep_current_upper.Location = new System.Drawing.Point(348, 33);
            this.lb_sleep_current_upper.Name = "lb_sleep_current_upper";
            this.lb_sleep_current_upper.Size = new System.Drawing.Size(44, 12);
            this.lb_sleep_current_upper.TabIndex = 4;
            this.lb_sleep_current_upper.Text = "label11";
            this.lb_sleep_current_upper.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lb_sleep_current_lower
            // 
            this.lb_sleep_current_lower.AutoSize = true;
            this.lb_sleep_current_lower.Location = new System.Drawing.Point(170, 34);
            this.lb_sleep_current_lower.Name = "lb_sleep_current_lower";
            this.lb_sleep_current_lower.Size = new System.Drawing.Size(44, 12);
            this.lb_sleep_current_lower.TabIndex = 4;
            this.lb_sleep_current_lower.Text = "label11";
            this.lb_sleep_current_lower.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_SleepCurrentAtVbat
            // 
            this.tb_SleepCurrentAtVbat.Location = new System.Drawing.Point(230, 30);
            this.tb_SleepCurrentAtVbat.Name = "tb_SleepCurrentAtVbat";
            this.tb_SleepCurrentAtVbat.Size = new System.Drawing.Size(100, 21);
            this.tb_SleepCurrentAtVbat.TabIndex = 3;
            this.tb_SleepCurrentAtVbat.TabStop = false;
            this.tb_SleepCurrentAtVbat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 34);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(127, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "Leakage Current (uA)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 574);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_LvCcCurrentAtVbat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_HvCvCurrentAtVbat;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_LvCcCurrentAtVbus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_HvCvCurrentAtVbus;
        private System.Windows.Forms.Label lb_lv_current_vbus_upper;
        private System.Windows.Forms.Label lb_lv_current_vbus_lower;
        private System.Windows.Forms.Label lb_lv_current_vbat_upper;
        private System.Windows.Forms.Label lb_lv_current_vbat_lower;
        private System.Windows.Forms.Label lb_hv_current_vbus_upper;
        private System.Windows.Forms.Label lb_hv_current_vbus_lower;
        private System.Windows.Forms.Label lb_hv_current_vbat_upper;
        private System.Windows.Forms.Label lb_hv_current_vbat_lower;
        private System.Windows.Forms.Label lbResult;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_resultThmLow;
        private System.Windows.Forms.TextBox tb_resultThmNormal;
        private System.Windows.Forms.TextBox tb_resultThmHigh;
        private System.Windows.Forms.Label lb_hightemp_upper;
        private System.Windows.Forms.Label lb_hightemp_lower;
        private System.Windows.Forms.Label lb_lowtemp_upper;
        private System.Windows.Forms.Label lb_lowtemp_lower;
        private System.Windows.Forms.Label lb_normaltemp_upper;
        private System.Windows.Forms.Label lb_normaltemp_lower;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label lb_sleep_current_upper;
        private System.Windows.Forms.Label lb_sleep_current_lower;
        private System.Windows.Forms.TextBox tb_SleepCurrentAtVbat;
        private System.Windows.Forms.Label label10;
    }
}

