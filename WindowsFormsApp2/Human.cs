using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AchtungDieCurve
{
    class Human : Player
    {
        Key ControlsLeft;
        Key ControlsRight;
        bool LeftQ;
        bool RightQ;
        public Human(string nick, Color col, Random rand, Key ConL, Key ConR) : base(nick, col, rand)
        {
            ControlsLeft = ConL;
            ControlsRight = ConR;
            LeftQ = false;
            RightQ = false;
        }
        public void Turning(int side)
        {
            if (side == 1) //left
            {
                LeftQ = true;
            }
            else //right
            {
                RightQ = true;
            }
        }
        public void SetFalseToTurning()
        {
            if (LeftQ)
            {
                TurnLeft();
                LeftQ = false;
            }
            if (RightQ)
            {
                TurnRight();
                RightQ = false;
            }
        }

        public Key getControlsLeft()
        {
            return ControlsLeft;
        }
        public Key getControlsRight()
        {
            return ControlsRight;
        }
    }
}
