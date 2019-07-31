using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AchtungDieCurve
{
    public partial class WindowOfResults : Form
    {
        Player[] playersForm;
        Game hra;
        ResultPanels[] rp = new ResultPanels[6];
        WindowOfPlayerSettings wops = new WindowOfPlayerSettings();

        public WindowOfResults(Player[] playersF)
        {
            playersForm = playersF;
            InitializeComponent();
        }
        public WindowOfResults(Player[] playersF, Game hra2, WindowOfPlayerSettings wops2)
        {
            InitializeComponent();
            playersForm = playersF;
            hra = hra2;
            wops = wops2;
            CreateRP();
            SortResults();
        }

        public WindowOfResults(Player[] playersF, Game hra2, WindowOfPlayerSettings wops2, MapAPC mapApc)
        {
            InitializeComponent();
            playersForm = playersF;
            hra = hra2;
            wops = wops2;
            CreateRP();
            SortResults();
        }
        private void CreateRP()
        {
            rp[0].panel = tableLayoutPanel1;
            rp[0].name = label8;
            rp[0].score = label7;
            rp[1].panel = tableLayoutPanel2;
            rp[1].name = label3;
            rp[1].score = label2;
            rp[2].panel = tableLayoutPanel3;
            rp[2].name = label5;
            rp[2].score = label4;
            rp[3].panel = tableLayoutPanel4;
            rp[3].name = label9;
            rp[3].score = label6;
            rp[4].panel = tableLayoutPanel5;
            rp[4].name = label11;
            rp[4].score = label10;
            rp[5].panel = tableLayoutPanel6;
            rp[5].name = label13;
            rp[5].score = label12;
        }
        private void NextGame_Click(object sender, EventArgs e)
        {
            WindowOfGame wog = new WindowOfGame(playersForm, hra, wops);
            wog.Show();
            this.Close();

        }

        private void Menu_Click(object sender, EventArgs e)
        {
            wops.Show();
            this.Close();
        }
        private void SortResults()
        {
            IEnumerable<Player> query = playersForm.OrderByDescending(player => player.GetScore());
            int i = 0;
            foreach (Player p in query)
            {
                rp[i].name.Text = p.GetNick();
                rp[i].score.Text = p.GetScore().ToString();
                rp[i].name.ForeColor = p.GetColor();
                rp[i].score.ForeColor = p.GetColor();
                i++;
            }
        }

    }
    struct ResultPanels
    {
        public TableLayoutPanel panel;
        public Label name;
        public Label score;
    }

}
