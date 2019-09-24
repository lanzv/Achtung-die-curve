using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Timers;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AchtungDieCurve
{

    public partial class WindowOfGame : Form
    {
        public static MapAPC mapApc;
        public static MapAPCBig mapApcBig;
        static Label enterMsg;
        static bool EndOfGame;
        static WindowOfPlayerSettings wops = new WindowOfPlayerSettings();
        static Player[] playersForm;
        static Pen[] snake = new Pen[6];
        static Graphics g;
        static PictureBox panel = new PictureBox();
        static Bitmap bmp;
        static Color bcColor = Color.FromArgb(150, 50, 50, 50);
        static Form f;
        static Image bmpIm;
        static Game hra;
        static PictureBox[] head = new PictureBox[6];
        public WindowOfGame(Player[] playersF, Game hra2, WindowOfPlayerSettings wops2)
        {

            f = this;
            playersForm = playersF;
            wops = wops2;

            InitializeComponent();

            enterMsg = label1;
            panel = pictureBox1;
            
            this.WindowState = FormWindowState.Maximized;

            panel.Width = this.Width;
            panel.Height = this.Height;
            panel.BackColor = bcColor;

            bmp = new Bitmap(10000, 10000);
            g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            CreatingHeads();
            CreatingPlayers(playersF);

            hra = hra2;
            hra.ResetGame();

        }

        private static void SetMapAPC()
        {
            mapApc = new MapAPC(f.Height, f.Width);
            mapApcBig = new MapAPCBig(f.Height, f.Width);
        }
        private void CreatingHeads()
        {
            head[0] = pictureBox2;
            head[1] = pictureBox3;
            head[2] = pictureBox4;
            head[3] = pictureBox5;
            head[4] = pictureBox7;
            head[5] = pictureBox8;
            for (int i = 0; i < head.Length; i++)
            {
                head[i].Size = new Size(10, 10);
                head[i].BackColor = Color.FromArgb(255, 152, 250, 31);
            }
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
            
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
        public static MapAPC GetApcMap()
        {
            return mapApc;
        }
        public static MapAPCBig GetApcMapBig()
        {
            return mapApcBig;
        }
        public static void CreatingBorders()
        {
            EndOfGame = false;
            Pen border = new Pen(Color.WhiteSmoke, 20);
            g.DrawLine(border, new Point(0, 0), new Point(0, panel.Height));
            g.DrawLine(border, new Point(panel.Width, panel.Height), new Point(0, panel.Height));
            g.DrawLine(border, new Point(panel.Width, panel.Height), new Point(panel.Width, 0));
            g.DrawLine(border, new Point(0, 0), new Point(panel.Width, 0));
            SetMapAPC();
        }
        private void CreatingPlayers(Player[] playersF)
        {

            playersForm = playersF;
            for (int i = 0; i < snake.Length; i++)
            {
                snake[i] = new Pen(playersForm[i].GetColor(), 5);
                snake[i].StartCap = snake[i].EndCap = System.Drawing.Drawing2D.LineCap.Round;
            }

        }
        public static void SetEndOfGame(bool b)
        {
            enterMsg.Text = "Pro pokračování stiskněte ENTER!";
            EndOfGame = true;
        }
        public static void SizesOfWindow(out int w, out int h)
        {
            w = panel.Width;
            h = panel.Height;
        }
        public static Bitmap GetBmp()
        {
            try
            {
                Log("before clone");
                Bitmap newBmp = Clone<Bitmap>(bmp);
                Log("after clone");
                return newBmp;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static void Draw()
        {

            if (hra.GetTicks() > 10)
                KillingSnakes();
            for (int i = 0; i < snake.Length; i++)
            {
                if (!hra.HasPause(playersForm[i]))
                {
                    g.DrawLine(snake[i], playersForm[i].GetPrevPosition(), playersForm[i].GetPosition());
                }
                if (playersForm[i].GetPosition().X > 5 && playersForm[i].GetPosition().Y > 5)
                {
                    head[i].Left = playersForm[i].GetPosition().X - 5;
                    head[i].Top = playersForm[i].GetPosition().Y - 5;
                }

            }
            if (hra.GetTicks() % 2 == 0)
            {
                for (int i = 0; i < playersForm.Length; i++)
                {

                    mapApc.DisableSector(playersForm[i].GetPosition());
                    mapApcBig.DisableSector(playersForm[i].GetPosition());
                }
            }

            panel.Image = bmp;


        }
        public static double[] GetTrainingValues(double[] y)
        {
            String tString = "";
            String yString = "";
            String[] numbers;
            hra.PauseGame();
            double[] t = new double[y.Length];
            for (int i = 0; i < y.Length; i++)
            {
                yString = yString + "; " + y[i];
            }
            tString = ShowMyDialogBox(yString);
            Log(tString);
            numbers = tString.Split(' ');
            for (int i = 0; i < t.Length; i++)
            {
                t[i] = Double.Parse(numbers[i]);
            }
            hra.UnpauseGame();
            return t;

        }
        public static String ShowMyDialogBox(String s)
        {
            Form1 testDialog = new Form1(s);
            String a = "";
            
            testDialog.ShowDialog(f);
            for (int i = 0; i < 3; i++)
            {
                if (testDialog.r[i].Checked)
                {
                    a = testDialog.tb[i].Text;
                }
            }


            testDialog.Dispose();
            return a;
        }
        private static void KillingSnakes()
        {
            Color c;
            for (int i = 0; i < snake.Length; i++)
            {
                c = bmp.GetPixel(playersForm[i].GetPosition().X +  2*(playersForm[i].GetPosition().X - playersForm[i].GetPrevPosition().X), playersForm[i].GetPosition().Y +  2*(playersForm[i].GetPosition().Y - playersForm[i].GetPrevPosition().Y));
                if ( c != Color.FromArgb(150, 50, 50, 50) && c != Color.FromArgb(0, 0, 0, 0))
                {
                    
                    if (!playersForm[i].IsDead())
                    {
                        playersForm[i].SetDead(true);
                        if (!hra.GivePoints(playersForm[i]))
                        {
                            SetEndOfGame(true);
                        }
                    }
                   

                }
            }
        }

        public static void Log(string a)
        {
            //bez komentaru pouze pro jednoho PC hrace, jinak je potreba zakomentovat nebo pridat podminku pro aktualni pouzivani souboru
            /*
            String path = AppDomain.CurrentDomain.BaseDirectory + "log.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path))
            {
                foreach (string line in lines)
                {
                    file.WriteLine(line);
                }
                file.WriteLine(a);

            }
            */
            
        }
        public static void PrintMessage(string a)
        {
            hra.PauseGame();
            MessageBox.Show(a);
            hra.UnpauseGame();

        }
        private static void KillProgram()
        {
            WindowOfResults wor = new WindowOfResults(playersForm, hra, wops, mapApc);
            wor.Show();
            f.Close();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.Enter) && EndOfGame)
            {
                KillProgram();
            }
            for (int i = 0; i < snake.Length; i++)
            {
                
                if (playersForm[i] is Human)
                {
                    if (Keyboard.IsKeyDown(((Human) playersForm[i]).getControlsLeft()))
                    {
                        ((Human)playersForm[i]).Turning(1);
                    }
                    if (Keyboard.IsKeyDown(((Human)playersForm[i]).getControlsRight()))
                    {
                        ((Human)playersForm[i]).Turning(0);
                    }
                }
            }
        }
    }
    
}



