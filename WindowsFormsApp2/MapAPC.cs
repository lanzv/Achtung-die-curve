using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchtungDieCurve
{
    public class MapAPC
    {
        int[,] mapa = new int[16, 16];
        int h, w;


        public MapAPC(int h1, int w1)
        {
            h = h1;
            w = w1;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (i == 0 || i == 15 || j == 0 || j == 15)
                    {
                        mapa[i, j] = 1;
                    }
                    else
                    {
                        mapa[i, j] = 0; //empty sector
                    }
                }
            }
        }
        public void DisableSector(Point PixCoor)
        {
            int a, b;
            GetSectorCoordinantes(out a, out b, PixCoor);
            mapa[a, b] = 1; 
        }
        public void GetSectorCoordinantes(out int a, out int b, Point PixCoor)
        {
            int a1 = h / 16;
            int b1 = w / 16;
            a = PixCoor.Y / a1;
            b = PixCoor.X / b1;
        }
        public bool IsDisable(Point PixCoor)
        {
            int a, b;
            GetSectorCoordinantes(out a, out b, PixCoor);
            if (mapa[a, b] == 1)
                return true;
            else
                return false;
        }
        public int GetFieldValue(int x, int y)
        {
            return mapa[x, y];
        }
        public String Read()
        {
            string s = "";
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    s = s + mapa[i, j].ToString();
                }
                s = s + "\n";
            }
            return s;
        }

    }
}
