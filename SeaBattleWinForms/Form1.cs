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

        private void PlacementShipsPlayer(object sender, EventArgs e)
        {
            seaBattleMainWindow.PlacementShipsPlayer(MousePosition);
        }

        private void KeyDownOrientation(object sender, KeyEventArgs e)
        {
            seaBattleMainWindow.KeyOrientation(e);

            if (seaBattleMainWindow.Orientation == false)
                labelOrientation.Text = "горизонтальная";
            else
                labelOrientation.Text = "вертикальная";
        }

        //private void seaBattleMainWindow_DragOver(object sender, DragEventArgs e)
        //{
        //    seaBattleMainWindow.DragShip(e);
        //}
    }
}
