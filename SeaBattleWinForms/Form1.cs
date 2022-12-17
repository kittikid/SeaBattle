using System;
using System.Windows.Forms;

namespace SeaBattleWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (seaBattleMainWindow.Orientation == false)
                labelOrientation.Text = "горизонтальная";
            else
                labelOrientation.Text = "вертикальная";

            // расширенное окно для выбора цвета
            colorDialog1.FullOpen = true;
            // установка начального цвета для colorDialog
            colorDialog1.Color = this.BackColor;
            // добавляем возможность выбора цвета шрифта
            fontDialog1.ShowColor = true;
        }
        
        private void CreatePlayerShipsRandom(object sender, EventArgs e)
        {
            //seaBattleMainWindow.ConfigureShips2(true);
            seaBattleMainWindow.ConfigureShips(true);
        }

        private void CreateBotShipsRandom(object sender, EventArgs e)
        {
            //seaBattleMainWindow.ConfigureShips2(false);
            seaBattleMainWindow.ConfigureShips(false);
        }

        private void CreateShipsNonRandom(object sender, EventArgs e)
        {
            seaBattleMainWindow.FieldForShips();
        }

        private void GameIsStart(object sender, EventArgs e)
        {

            if (!seaBattleMainWindow.isPlaying)
            {
                seaBattleMainWindow.isPlaying = true;
                SwitchButtons(false);
                StartGame.Text = "Закончить игру";
            }
            else
            {
                seaBattleMainWindow.isPlaying = false;
                SwitchButtons(true);
                StartGame.Text = "Начать игру";
            }

        }

        public void SwitchButtons(bool value)
        {
            PlayerRandomButton.Enabled = value;
            BotRandomButton.Enabled = value;
            PlacementShips.Enabled = value;
        }

        private void PlayerShoot(object sender, MouseEventArgs e)
        {
            seaBattleMainWindow.Shoot(sender, e);
        }

        //private void PlacementShipsPlayer(object sender, EventArgs e)
        //{
        //    seaBattleMainWindow.PlacementShipsPlayer(MousePosition);
        //}

        private void KeyDownOrientation(object sender, KeyEventArgs e)
        {
            seaBattleMainWindow.KeyOrientation(e);

            if (seaBattleMainWindow.Orientation == false)
                labelOrientation.Text = "горизонтальная";
            else
                labelOrientation.Text = "вертикальная";
        }

        private void PlacementShipsPlayer(object sender, DragEventArgs e)
        {
            seaBattleMainWindow.PlacementShipsPlayer(MousePosition);
        }

        private void ChangeBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка цвета формы
            seaBattleMainWindow.UserBackColor = colorDialog1.Color;
        }

        private void ChangeFontColor_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            // установка минимальных и максимальных значений шрифта
            fontDialog1.MaxSize = 14;
            fontDialog1.MinSize = 8;
            // установка шрифта
            seaBattleMainWindow.UserFontFont = fontDialog1.Font;
            // установка цвета шрифта
            seaBattleMainWindow.UserFontColor = fontDialog1.Color;
        }

        //private void seaBattleMainWindow_DragOver(object sender, DragEventArgs e)
        //{
        //    seaBattleMainWindow.DragShip(e);
        //}
    }
}
