using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace AchtungDieCurve
{
    public class Game
    {
        static Player[] players;
        static int ranking = 1;
        static System.Timers.Timer aTimer;
        static int ticks;
        static int rotateRadius = 1;
        static int speed = 150;

        public Game(Player[] playersForm)
        {
            players = playersForm;
            aTimer = new System.Timers.Timer(speed);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            ticks = 0;
        }
        public void PauseGame()
        {
            aTimer.Stop();
        }
        public void UnpauseGame()
        {
            aTimer.Start();
        }
        public void ResetGame()
        {
            for (int i = 0; i < players.Length; i++)
            {
                Thread.Sleep(20);
                players[i].ResetPlayer();
                if (players[i] is PC)
                    ((PC)players[i]).ResetPC();
            }
            ranking = 1;
            ticks = 0;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ticks++;
            if(ticks == 2)
            {
                WindowOfGame.CreatingBorders();
                for (int i = 0; i < players.Length; i++)
                {
                    Thread.Sleep(20);
                    players[i].CreatePosition(new Random());
                }
            }
            if(ticks > 2)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (!players[i].IsDead())
                    {
                        if (ticks % rotateRadius == 0)
                        {

                            if (players[i] is Human)
                                ((Human)players[i]).SetFalseToTurning();
                            else
                                ((PC)players[i]).ExecuteTurning();
                        }
                        players[i].Move();
                    }
                }
                WindowOfGame.Draw();
            }
        }
        public bool GivePoints(Player p)
        {
            if(ranking < 6)
            {
                p.AddScore(ranking);
            }
            ranking++;
            if(ranking == 6)
            {
                for (int i = 0; i < players.Length; i++)
                {
                    if (!players[i].IsDead())
                    {
                        players[i].AddScore(ranking);
                    }
                }
                aTimer.Enabled = false;
                aTimer.Stop();
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool HasPause(Player p)
        {
            if (p.GetPause().lastTick < ticks)
            {
                p.SetPause(ticks, new Random());
            }
            else if (p.GetPause().fstTick < ticks)
            {
                return true;
            }
            return false;
        }
        public int GetTicks()
        {
            return ticks;
        }
    }
}
