
namespace SeaBattleWinForms
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.StartGame = new System.Windows.Forms.Button();
            this.PlayerRandomButton = new System.Windows.Forms.Button();
            this.BotRandomButton = new System.Windows.Forms.Button();
            this.PlacementShips = new System.Windows.Forms.Button();
            this.labelText = new System.Windows.Forms.Label();
            this.labelOrientation = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.ChangeBackColor = new System.Windows.Forms.Button();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.ChangeFontColor = new System.Windows.Forms.Button();
            this.seaBattleMainWindow = new SeaBattle.SeaBattleMainWindow();
            this.SuspendLayout();
            // 
            // StartGame
            // 
            this.StartGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StartGame.Location = new System.Drawing.Point(440, 435);
            this.StartGame.Name = "StartGame";
            this.StartGame.Size = new System.Drawing.Size(150, 50);
            this.StartGame.TabIndex = 1;
            this.StartGame.Text = "Начать игру";
            this.StartGame.UseVisualStyleBackColor = true;
            this.StartGame.Click += new System.EventHandler(this.GameIsStart);
            // 
            // PlayerRandomButton
            // 
            this.PlayerRandomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlayerRandomButton.Location = new System.Drawing.Point(273, 501);
            this.PlayerRandomButton.Name = "PlayerRandomButton";
            this.PlayerRandomButton.Size = new System.Drawing.Size(150, 50);
            this.PlayerRandomButton.TabIndex = 2;
            this.PlayerRandomButton.Text = "Расставить корабли игрока";
            this.PlayerRandomButton.UseVisualStyleBackColor = true;
            this.PlayerRandomButton.Click += new System.EventHandler(this.CreatePlayerShipsRandom);
            // 
            // BotRandomButton
            // 
            this.BotRandomButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BotRandomButton.Location = new System.Drawing.Point(440, 501);
            this.BotRandomButton.Name = "BotRandomButton";
            this.BotRandomButton.Size = new System.Drawing.Size(150, 50);
            this.BotRandomButton.TabIndex = 3;
            this.BotRandomButton.Text = "Расставить корабли оппонента";
            this.BotRandomButton.UseVisualStyleBackColor = true;
            this.BotRandomButton.Click += new System.EventHandler(this.CreateBotShipsRandom);
            // 
            // PlacementShips
            // 
            this.PlacementShips.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.PlacementShips.Location = new System.Drawing.Point(605, 501);
            this.PlacementShips.Name = "PlacementShips";
            this.PlacementShips.Size = new System.Drawing.Size(150, 50);
            this.PlacementShips.TabIndex = 4;
            this.PlacementShips.Text = "Расставить корабли самому";
            this.PlacementShips.UseVisualStyleBackColor = true;
            this.PlacementShips.Click += new System.EventHandler(this.CreateShipsNonRandom);
            // 
            // labelText
            // 
            this.labelText.AutoSize = true;
            this.labelText.Location = new System.Drawing.Point(331, 385);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(71, 13);
            this.labelText.TabIndex = 5;
            this.labelText.Text = "Ориентация:";
            // 
            // labelOrientation
            // 
            this.labelOrientation.AutoSize = true;
            this.labelOrientation.Location = new System.Drawing.Point(408, 385);
            this.labelOrientation.Name = "labelOrientation";
            this.labelOrientation.Size = new System.Drawing.Size(35, 13);
            this.labelOrientation.TabIndex = 6;
            this.labelOrientation.Text = "label1";
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(311, 408);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(185, 13);
            this.labelInfo.TabIndex = 7;
            this.labelInfo.Text = "Для смены ориентации нажмите Z";
            // 
            // ChangeBackColor
            // 
            this.ChangeBackColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeBackColor.Location = new System.Drawing.Point(628, 399);
            this.ChangeBackColor.Name = "ChangeBackColor";
            this.ChangeBackColor.Size = new System.Drawing.Size(127, 40);
            this.ChangeBackColor.TabIndex = 8;
            this.ChangeBackColor.Text = "Поменять цвет фона";
            this.ChangeBackColor.UseVisualStyleBackColor = true;
            this.ChangeBackColor.Click += new System.EventHandler(this.ChangeBackColor_Click);
            // 
            // ChangeFontColor
            // 
            this.ChangeFontColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ChangeFontColor.Location = new System.Drawing.Point(628, 445);
            this.ChangeFontColor.Name = "ChangeFontColor";
            this.ChangeFontColor.Size = new System.Drawing.Size(127, 40);
            this.ChangeFontColor.TabIndex = 9;
            this.ChangeFontColor.Text = "Поменять цвет и стиль текста";
            this.ChangeFontColor.UseVisualStyleBackColor = true;
            this.ChangeFontColor.Click += new System.EventHandler(this.ChangeFontColor_Click);
            // 
            // seaBattleMainWindow
            // 
            this.seaBattleMainWindow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seaBattleMainWindow.isPlaying = false;
            this.seaBattleMainWindow.Location = new System.Drawing.Point(0, 0);
            this.seaBattleMainWindow.Margin = new System.Windows.Forms.Padding(0);
            this.seaBattleMainWindow.MaximumSize = new System.Drawing.Size(779, 569);
            this.seaBattleMainWindow.MinimumSize = new System.Drawing.Size(779, 596);
            this.seaBattleMainWindow.Name = "seaBattleMainWindow";
            this.seaBattleMainWindow.Orientation = false;
            this.seaBattleMainWindow.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.seaBattleMainWindow.Size = new System.Drawing.Size(779, 596);
            this.seaBattleMainWindow.TabIndex = 0;
            this.seaBattleMainWindow.Text = "seaBattleMainWindow1";
            this.seaBattleMainWindow.UserBackColor = System.Drawing.Color.LightBlue;
            this.seaBattleMainWindow.UserFontColor = System.Drawing.Color.Black;
            this.seaBattleMainWindow.UserFontFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.seaBattleMainWindow.Click += new System.EventHandler(this.PlacementShipsPlayer);
            this.seaBattleMainWindow.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PlayerShoot);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(779, 596);
            this.Controls.Add(this.ChangeFontColor);
            this.Controls.Add(this.ChangeBackColor);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.labelOrientation);
            this.Controls.Add(this.labelText);
            this.Controls.Add(this.PlacementShips);
            this.Controls.Add(this.BotRandomButton);
            this.Controls.Add(this.PlayerRandomButton);
            this.Controls.Add(this.StartGame);
            this.Controls.Add(this.seaBattleMainWindow);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(795, 635);
            this.MinimumSize = new System.Drawing.Size(795, 635);
            this.Name = "Form1";
            this.Text = "SeaBattle";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownOrientation);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private global::SeaBattle.SeaBattleMainWindow seaBattleMainWindow;
        private System.Windows.Forms.Button StartGame;
        private System.Windows.Forms.Button PlayerRandomButton;
        private System.Windows.Forms.Button BotRandomButton;
        private System.Windows.Forms.Button PlacementShips;
        private System.Windows.Forms.Label labelText;
        private System.Windows.Forms.Label labelOrientation;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Button ChangeBackColor;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button ChangeFontColor;
    }
}

