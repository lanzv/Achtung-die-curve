using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AchtungDieCurve
{
    public class Player
    {
        double DegreeRot = 0.3;
        String nickname;
        int score;
        Color colour;
        Vector direction;
        Vector position;
        Vector prevPosition;
        int movingLength = 5;
        bool dead;
        Pause pause;

        public Player(String nick, Color col, Random rand)
        {
            score = 0;
            dead = false;
            nickname = nick;
            colour = col;
            CreateDirection(rand);
            pause.fstTick = -1;
            pause.lastTick = -1;

        }
        public int GetScore()
        {
            return score;
        }
        public void ResetPlayer()
        {
            CreateDirection(new Random());
            dead = false;
            pause.fstTick = -1;
            pause.lastTick = -1;
        }
        public void SetPause(int tick, Random rand)
        {
            pause.fstTick = tick + rand.Next(30, 100);
            pause.lastTick = pause.fstTick + rand.Next(3, 5);
            Thread.Sleep(20);

        }
        public Pause GetPause()
        {
            return pause;
        }
        private void CreateDirection(Random rand)
        {

            direction.x = (double)((double)rand.Next(0, 1000 * movingLength) / (double)1000);
            direction.y = (double)movingLength - (double)Math.Sqrt(direction.x);

            if (rand.Next(0, 2) == 1)
                direction.x = -1 * direction.x;
            if (rand.Next(0, 2) == 1)
                direction.y = -1 * direction.y;

        }
        public void CreatePosition(Random rand)
        {

            int w, h;
            WindowOfGame.SizesOfWindow(out w, out h);
            position.x = rand.Next(100, w - 100);
            position.y = rand.Next(100, h - 100);
            prevPosition.x = position.x;
            prevPosition.y = position.y;

        }
        public String GetNick()
        {
            return nickname;
        }
        public void SetDead(bool changeDead)
        {
            dead = changeDead;
        }
        public bool IsDead()
        {
            return dead;
        }
        public void Move()
        {
            prevPosition.x = position.x;
            prevPosition.y = position.y;
            position.x = position.x + direction.x;
            position.y = position.y + direction.y;
        }
        protected Vector GetDirection()
        {
            return direction;
        }
        public Color GetColor()
        {
            return colour;
        }
        protected void TurnLeft()
        {
            double x = direction.x;
            double y = direction.y;
            direction.x = x * Math.Cos(DegreeRot) + y * Math.Sin(DegreeRot);
            direction.y = -x * Math.Sin(DegreeRot) + y * Math.Cos(DegreeRot);
        }
        protected void TurnRight()
        {
            double x = direction.x;
            double y = direction.y;
            direction.x = x * Math.Cos(DegreeRot) - y * Math.Sin(DegreeRot);
            direction.y = x * Math.Sin(DegreeRot) + y * Math.Cos(DegreeRot);
        }
        public Point GetPrevPosition()
        {
            Point p = new Point((int)prevPosition.x, (int)prevPosition.y);
            return p;
        }
        public Point GetPosition()
        {
            Point p = new Point((int)position.x, (int)position.y);
            return p;
        }
        public void AddScore(int i)
        {
            score = score + i;
        }
    }
}
