using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchtungDieCurve
{
    public class MapAPCBig
    {
        int[,] mapa = new int[50, 50];
        int h, w;


        public MapAPCBig(int h1, int w1)
        {
            h = h1;
            w = w1;
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (i == 0 || i == 49 || j == 0 || j == 49)
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
            int a1 = h / 50;
            int b1 = w / 50;
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

    }
}
