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
using System.Windows.Input;

namespace AchtungDieCurve
{
    public partial class WindowOfPlayerSettings : Form
    {
        WindowOfMainMenu womm = new WindowOfMainMenu();
        Player[] players = new Player[6];
        int checkedIndex;
        int phaseCB = 0;
        Font Dfont;
        Form f;
        ColorDialog[] cDialogs = new ColorDialog[6];
        Button[] bDialogs = new Button[6];
        TextBox[] nicks = new TextBox[6];
        CheckBox[] PCH = new CheckBox[6];
        Key[] k = new Key[]{ Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J, Key.K, Key.L, Key.M, Key.N, Key.O,
                Key.P, Key.Q, Key.R, Key.S, Key.T, Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5,
                Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.Right, Key.Left, Key.Up, Key.Down, Key.Space};
        WOPSControl[] wopscontrols = new WOPSControl[6];

        public WindowOfPlayerSettings(WindowOfMainMenu womm2)
        {

            womm = womm2;
            InitializeComponent();
            CreatingNicks();
            CreatingWOPSControl();
            CreatingPCH();
            CreatingColorDialogs();
            f = this;

        }
        public WindowOfPlayerSettings()
        {
            InitializeComponent();
            CreatingNicks();
            CreatingWOPSControl();
            CreatingPCH();
            CreatingColorDialogs();
            f = this;
            
        }
        private void CreatingWOPSControl()
        {
            wopscontrols[0].panel = tableLayoutPanel1;
            wopscontrols[0].LeftC = label8;
            wopscontrols[0].RightC = label7;
            wopscontrols[1].panel = tableLayoutPanel2;
            wopscontrols[1].LeftC = label10;
            wopscontrols[1].RightC = label9;
            wopscontrols[2].panel = tableLayoutPanel3;
            wopscontrols[2].LeftC = label12;
            wopscontrols[2].RightC = label11;
            wopscontrols[3].panel = tableLayoutPanel4;
            wopscontrols[3].LeftC = label14;
            wopscontrols[3].RightC = label13;
            wopscontrols[4].panel = tableLayoutPanel5;
            wopscontrols[4].LeftC = label16;
            wopscontrols[4].RightC = label15;
            wopscontrols[5].panel = tableLayoutPanel6;
            wopscontrols[5].LeftC = label17;
            wopscontrols[5].RightC = label18;
            Dfont = wopscontrols[0].LeftC.Font;
            for (int i = 0; i < wopscontrols.Length; i++)
            {
                wopscontrols[i].LeftC.Text = "Určete klávesu DOLEVA.";
                wopscontrols[i].RightC.Text = "";
                wopscontrols[i].panel.Hide();
            }
        }
        private void CreatingPCH()
        {
            PCH[0] = checkBox1;
            PCH[1] = checkBox2;
            PCH[2] = checkBox3;
            PCH[3] = checkBox4;
            PCH[4] = checkBox5;
            PCH[5] = checkBox6;
            for (int i = 0; i < PCH.Length; i++)
            {
                PCH[i].Checked = true;
                PCH[i].CheckedChanged += new EventHandler(PCH_Change);
            }
        }
        private void CreatingNicks()
        {
            nicks[0] = textBox1;
            nicks[1] = textBox2;
            nicks[2] = textBox3;
            nicks[3] = textBox4;
            nicks[4] = textBox5;
            nicks[5] = textBox6;
        }
        private void CreatingColorDialogs()
        {
            cDialogs[0] = colorDialog1;
            cDialogs[1] = colorDialog2;
            cDialogs[2] = colorDialog3;
            cDialogs[3] = colorDialog4;
            cDialogs[4] = colorDialog5;
            cDialogs[5] = colorDialog6;
            bDialogs[0] = button3;
            bDialogs[1] = button4;
            bDialogs[2] = button5;
            bDialogs[3] = button6;
            bDialogs[4] = button7;
            bDialogs[5] = button8;
            SetColorsForDialogs();
            for (int i = 0; i < cDialogs.Length; i++)
            {
                bDialogs[i].BackColor = cDialogs[i].Color;
                bDialogs[i].Size = new Size(40, 40);
                bDialogs[i].Click += new EventHandler(cDialogs_Click);
            }


        }
        private void SetColorsForDialogs()
        {
            cDialogs[0].Color = Color.FromArgb(255, 0, 0, 255);
            cDialogs[1].Color = Color.FromArgb(255, 255, 0, 0);
            cDialogs[2].Color = Color.FromArgb(255, 255, 255, 0);
            cDialogs[3].Color = Color.FromArgb(255, 128, 0, 50);
            cDialogs[4].Color = Color.FromArgb(255, 0, 255, 0);
            cDialogs[5].Color = Color.FromArgb(255, 255, 0, 255);
        }


        private void cDialogs_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            for (int i = 0; i < bDialogs.Length; i++)
            {

                if (btn == bDialogs[i])
                {
                    cDialogs[i].ShowDialog();
                    bDialogs[i].BackColor = cDialogs[i].Color;
                }
            }

        }
        private void PCH_Change(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            for (int i = 0; i < PCH.Length; i++)
            {
                if (PCH[i] == cb && !PCH[i].Checked)
                {
                    phaseCB = 1;
                    checkedIndex = i;
                    wopscontrols[i].panel.Show();
                    LockAllWidgets();
                }
                else if(PCH[i].Checked)
                {
                    wopscontrols[i].LeftC.Text = "Určete klávesu DOLEVA.";
                    wopscontrols[i].RightC.Text = "";
                    wopscontrols[i].LeftC.Font = Dfont;
                    wopscontrols[i].RightC.Font = Dfont;
                    wopscontrols[i].panel.Hide();
                }
            }
        }
        private void LockAllWidgets()
        {
            for (int i = 0; i < PCH.Length; i++)
            {
                PCH[i].Enabled = false;
                nicks[i].Enabled = false;
                bDialogs[i].Enabled = false;
            }
            Start.Enabled = false;
            CLose.Enabled = false;
        }
        private void UnlockAllWidgets()
        {
            for (int i = 0; i < PCH.Length; i++)
            {
                PCH[i].Enabled = true;
                nicks[i].Enabled = true;
                bDialogs[i].Enabled = true;
            }
            Start.Enabled = true;
            CLose.Enabled = true;
        }

        private void WindowOfPlayerSettings_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (phaseCB == 1)
            {
                foreach (Key element in k){
                    if (Keyboard.IsKeyDown(element)){
                        wopscontrols[checkedIndex].L = element;
                        phaseCB = 2;
                        wopscontrols[checkedIndex].LeftC.Text = element.ToString();
                        wopscontrols[checkedIndex].LeftC.Font = new Font("Segoe Script", 20, FontStyle.Bold);
                        wopscontrols[checkedIndex].RightC.Text = "Určete klávesu DOPRAVA.";
                    }
                }


            }
            else if(phaseCB == 2)
            {
                foreach (Key element in k)
                {
                    if (Keyboard.IsKeyDown(element)){
                        wopscontrols[checkedIndex].R = element;
                        phaseCB = 0;
                        UnlockAllWidgets();
                        wopscontrols[checkedIndex].RightC.Text = element.ToString();
                        wopscontrols[checkedIndex].RightC.Font = new Font("Segoe Script", 20, FontStyle.Bold);
                    }
                }
                
            }
        }
        private void SetPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                Thread.Sleep(20);
                if (PCH[i].Checked)
                {
                    players[i] = new PC(nicks[i].Text, cDialogs[i].Color, new Random());
                }
                else
                {
                    players[i] = new Human(nicks[i].Text, cDialogs[i].Color, new Random(), wopscontrols[i].L, wopscontrols[i].R);
                }
            }
        }
        private void Start_Click(object sender, EventArgs e)
        {
            SetPlayers();
            Game hra = new Game(players);
            WindowOfGame wog = new WindowOfGame(players, hra, this);
            wog.Show();
            this.Hide();
        }

        private void Close_Click(object sender, EventArgs e)
        {
            womm.Show();
            this.Close();
        }
    }
    struct WOPSControl
    {
        public Key L;
        public Key R;
        public TableLayoutPanel panel;
        public Label LeftC;
        public Label RightC;
    }
}
