using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AchtungDieCurve
{
    
    class PC : Player
    {
        int TrainingCounter = 0;
        Thread recalculation;
        bool CanIGoThereRunning = false;
        bool recalculationRunning = false;
        int signOfLineOfInequality;
        Vector rightNormOfTheVector = new Vector();
        Vector positionAPC = new Vector();
        Vector positionAPCBig = new Vector();
        SoftmaxOutput so;
        Queue<int[]> R_input = new Queue<int[]>();
        Queue<int[]> L_input = new Queue<int[]>();
        int TurningCounter = 6;
        int TurningState = 0; //1 right, -1 left, 0 nonturning
        Neuron[] neurons = new Neuron[256];
        double[] coeficientsOfLine = new double[3];
        public PC(string nick, Color col, Random rand) : base(nick, col, rand)
        {
            InitilazeNeuronsArrays();
            so = new SoftmaxOutput();
            SetWb();
        }
        public void ResetPC()
        {
                TrainingCounter = 0;
                CanIGoThereRunning = false;
                recalculationRunning = false;
                TurningCounter = 6;
                TurningState = 0; //1 right, -1 left, 0 nonturning
                //neurons = new Neuron[256];
                coeficientsOfLine = new double[3];
                so = new SoftmaxOutput();
                SetWb();
        }
        private void SetWb()
        {
            String path = AppDomain.CurrentDomain.BaseDirectory + "neuronsCoeficients.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            double W, b;
            String[] numbers;
            for (int i = 0; i < neurons.Length; i++) 
            {
                for (int j = 0; j < neurons[i].W.Length; j++)
                {
                    numbers = lines[1 + i * 8 + j].Split(' ');
                    W = Double.Parse(numbers[0]);
                    b = 0;
                   
                        neurons[i].W[j] = W;
                        neurons[i].b[j] = b;

                }

            }


        }
        private void FindNextStepAI()
        {
            MapAPC map = WindowOfGame.GetApcMap();
            MapAPCBig mapBig = WindowOfGame.GetApcMapBig();
            GetInputReady(map, mapBig);
            so.SetSoftmaxOutput(neurons[0].W.Length, neurons);
            /*
             * pouze pokud chci trenovat neurnovou sit:
            TrainingCounter++;
            if (TrainingCounter%5 == 0)
            {
                TrainWs(so.GetDeltaWs());
            }*/

            if (!CanIGoThereRunning)
            {
                CanIGoThereRunning = true;
                WindowOfGame.Log("FindNextStepAI, can i go there?");
                if (!CanIGoThere()) {
                    WindowOfGame.Log("FindNextStepAI, i cant go there");
                    if (CanIGoToLeft())
                    {
                        TurningState = -1;
                    }
                    else
                    {
                        TurningState = 1;
                    }
                    TurningCounter = 3;
                }
                else
                {
                    switch (so.GetTheHighestOutput())
                    {
                        case 0: //nonturning, puvodne "hodne doleva"
                            TurningState = 0;
                            TurningCounter = 5;
                            break;
                        case 1://nonturning, puvodne "stredne doleva"
                            TurningState = 0;
                            TurningCounter = 5;
                            break;
                        case 2://short left
                            TurningState = -1;
                            TurningCounter = 4;
                            break;
                        case 3://nonturning
                            TurningState = 0;
                            TurningCounter = 5;
                            break;
                        case 4://short right
                            TurningState = 1;
                            TurningCounter = 4;
                            break;
                        case 5://nonturning, puvodne "stredne doprava"
                            TurningState = 0;
                            TurningCounter = 5;
                            break;
                        case 6://nonturning, puvodne "hodne doprava"
                            TurningState = 0;
                            TurningCounter = 5;
                            break;
                    }

                } }
            recalculationRunning = false;
            CanIGoThereRunning = false;
        }
        public void ExecuteTurning()
        {
            if (TurningCounter != 0)
            {
                switch (TurningState)
                {
                    case 1:
                        TurnLeft();
                        break;
                    case -1:
                        TurnRight();
                        break;
                }
                TurningCounter--;
            }
            else
            {
                TurningCounter = 100;
                recalculationRunning = true;
                recalculation = new Thread(this.FindNextStepAI);
                recalculation.Start();
            }
        }
        private void TrainWs(double[,] dWs)
        {
            double c = 1;
            if(neurons[10].W[3] > 500 || neurons[1].W[3] > 500 || neurons[250].W[3] > 500 || neurons[153].W[4] > 500)
            {
                c = (double)((double)1 / (double)100);
            }
            for (int i = 0; i < neurons.Length; i++)
            {
                for (int j = 0; j < neurons[i].W.Length; j++)
                {
                   
                        neurons[i].W[j] = c * (neurons[i].W[j] + dWs[j, i]);
                  
                }
            }
            String path = AppDomain.CurrentDomain.BaseDirectory + "neuronsCoeficients.txt";
            string[] lines = System.IO.File.ReadAllLines(path);
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < neurons.Length; i++)
                {
                    file.WriteLine("");
                    for (int j = 0; j < neurons[i].W.Length; j++)
                    {
                        file.WriteLine(neurons[i].W[j] + " " + neurons[i].b[j]);
                    }
                }
            }
        }
        private void GetInputReady(MapAPC map, MapAPCBig mapBig)
        {
            SetMeasureChanges(map, mapBig);
            SetRightNormOfTheVector();
            SetCoeficientsOfLine();
            SetSignOfLineOfInequality();
            BFSForApcMap();
            SetInput(map);
        }
        private void InitilazeNeuronsArrays()
        {
            for (int i = 0; i < neurons.Length; i++)
            {
                neurons[i].W = new double[7];
                neurons[i].b = new double[7];
            }
        }
        private void SetMeasureChanges(MapAPC map, MapAPCBig mapBig)
        {
            int a, b;
            map.GetSectorCoordinantes(out a, out b, this.GetPosition());
            positionAPC.x = a;
            positionAPC.y = b;
            mapBig.GetSectorCoordinantes(out a, out b, this.GetPosition());
            positionAPCBig.x = a;
            positionAPCBig.y = b;
        }
        private void SetInput(MapAPC map)
        {
            int[] field;
            if(L_input.Count > R_input.Count)
            {
                for (int i = 0; i < neurons.Length/2; i++)
                {
                    field = L_input.Dequeue();
                    neurons[2*i].x = map.GetFieldValue(field[0], field[1]);
                }
                int j = 0;
                while (R_input.Count > 0)
                {
                    field = R_input.Dequeue();
                    neurons[2 * j + 1].x = map.GetFieldValue(field[0], field[1]);
                    j++;
                }
                while(L_input.Count > 0)
                {
                    field = L_input.Dequeue();
                    neurons[2*j + 1].x = map.GetFieldValue(field[0], field[1]);
                    j++;
                }
            }
            else
            {
                for (int i = 0; i < neurons.Length / 2; i++)
                {
                    field = R_input.Dequeue();
                    neurons[2 * i + 1].x = map.GetFieldValue(field[0], field[1]);
                }
                int j = 0;
                while (L_input.Count > 0)
                {
                    field = L_input.Dequeue();
                    neurons[2 * j].x = map.GetFieldValue(field[0], field[1]);
                    j++;
                }
                while (R_input.Count > 0)
                {
                    field = R_input.Dequeue();
                    neurons[2 * j].x = map.GetFieldValue(field[0], field[1]);
                    j++;
                }
            }
        }
        private void SetRightNormOfTheVector()
        {
            Vector v = this.GetDirection();
            Vector u = new Vector();
            u.x = v.y;
            u.y = -v.x;
            rightNormOfTheVector = u;
        }
        private void SetCoeficientsOfLine()
        {
            Vector v = rightNormOfTheVector;
            coeficientsOfLine[0] = v.x;
            coeficientsOfLine[1] = v.y;
            coeficientsOfLine[2] = -(v.x * positionAPC.x + v.y * positionAPC.y);
        }
        private void SetSignOfLineOfInequality()
        {
            //Urceni leve a prave strany od primky prochazejici vektorem urcujici smer hrace
            signOfLineOfInequality = 1;
            //Pravo vzdy vetsi nebo rovno nez nula! Pokud je pravo mensi, prenasobuju -1
            if (((double)signOfLineOfInequality*(coeficientsOfLine[0] * (positionAPC.x + rightNormOfTheVector.x) + coeficientsOfLine[1] * (positionAPC.y + rightNormOfTheVector.y) + coeficientsOfLine[2]) < 0))
            {
                signOfLineOfInequality = -1 * signOfLineOfInequality;
            }
        }
        private void BFSForApcMap()
        {
            Queue<int[]> s = new Queue<int[]>();
            int[,] bfsMap = new int[16, 16];
            Vector v = SetFirstField();
            Vector u = SetSecondField(v);
            v = FixIndexesForBFS(v, positionAPC, positionAPC);
            u = FixIndexesForBFS(u, v, positionAPC);
            bfsMap[(int)v.x, (int)v.y] = -1;
            bfsMap[(int)positionAPC.x, (int)positionAPC.y] = -1;
            bfsMap[(int)u.x, (int)u.y] = 1;
            L_input.Enqueue(new int[] { (int)v.x, (int)v.y });
            L_input.Enqueue(new int[] { (int)u.x, (int)u.y });
            s.Enqueue(new int[] { (int)u.x, (int)u.y });
            while (s.Count > 0)
            {
                int[] coordinates = s.Dequeue();
                for (int i = -1; i <= 1; i++)
                {
                    for (int j= -1; j <= 1; j++)
                    {
                        if ((i != 0 || j != 0) && coordinates[0] + i >= 0 && coordinates[0] + i < 16 && coordinates[1] + j >= 0 && coordinates[1] + j < 16)
                        {
                            if (bfsMap[coordinates[0] + i, coordinates[1] + j] == 0)
                            {
                                bfsMap[coordinates[0] + i, coordinates[1] + j] = bfsMap[coordinates[0], coordinates[1]] + 1;
                                s.Enqueue(new int[] { coordinates[0] + i, coordinates[1] + j });
                                EnqueueToRLInputs(new int[] { coordinates[0] + i, coordinates[1] + j });
                            }
                        }
                    }
                }
            }
            L_input.Enqueue(new int[] { (int)positionAPC.x, (int)positionAPC.y });
        }
        private Vector FixIndexesForBFS(Vector need2fix, Vector fst, Vector scd)
        {
            while (need2fix.x < 0 || (need2fix.x == fst.x && need2fix.y == fst.y) || (need2fix.x == scd.x && need2fix.y == scd.y))
            {
                need2fix.x++;
            }
            while (need2fix.y < 0 || (need2fix.x == fst.x && need2fix.y == fst.y) || (need2fix.x == scd.x && need2fix.y == scd.y))
            {
                need2fix.x++;
            }
            while (need2fix.x >= 16 || (need2fix.x == fst.x && need2fix.y == fst.y) || (need2fix.x == scd.x && need2fix.y == scd.y))
            {
                need2fix.x--;
            }
            while (need2fix.y >= 16 || (need2fix.x == fst.x && need2fix.y == fst.y) || (need2fix.x == scd.x && need2fix.y == scd.y))
            {
                need2fix.x--;
            }
            return need2fix;
        }
        private void EnqueueToRLInputs(int[] input)
        {
            if(signOfLineOfInequality * (coeficientsOfLine[0] * input[0] + coeficientsOfLine[1] * input[1] + coeficientsOfLine[2]) >= 0)
            {
                L_input.Enqueue(input);
            }
            else
            {
                R_input.Enqueue(input);
            }
        }
        private Vector SetSecondField(Vector fstV)
        {
            Vector v = positionAPC;
            Vector u = new Vector();
            u.x = fstV.x + (fstV.x - v.x);
            u.y = fstV.y + (fstV.y - v.y);
            return u;
        }
        private Vector SetFirstField()
        {
            Vector positionFstF = new Vector();
            Vector[] pomocny = new Vector[8];
            pomocny[1].x = 0;
            pomocny[1].y = 1;
            pomocny[2].x = 1;
            pomocny[2].y = 1;
            pomocny[3].x = 1;
            pomocny[3].y = 0;
            pomocny[4].x = 0;
            pomocny[4].y = -1;
            pomocny[5].x = -1;
            pomocny[5].y = -1;
            pomocny[6].x = -1;
            pomocny[6].y = 0;
            pomocny[7].x = 1;
            pomocny[7].y = -1;
            pomocny[0].x = -1;
            pomocny[0].y = 1;
            double min = -1;
            for (int i = 0; i < 8; i++)
            {
               if(min == -1 || min > GetAngleBetweenTwoVectors(pomocny[i], this.GetDirection()))
                {
                    min = GetAngleBetweenTwoVectors(pomocny[i], this.GetDirection());
                    positionFstF.x = positionAPC.x + pomocny[i].x;
                    positionFstF.y = positionAPC.y + pomocny[i].y;
                }
            }
            return positionFstF;
        }
        private double GetAngleBetweenTwoVectors(Vector v, Vector u)
        {
            double a;
            a = Math.Acos((v.x * u.x + v.y * u.y) / (Math.Sqrt(v.x * v.x + v.y * v.y) * Math.Sqrt(u.x * u.x + u.y * u.y)));
            return a;

        }
        public bool CanIGoToLeft()
        {
            WindowOfGame.Log("CanIGoToLeft beg");
            Vector v = SetFirstField();
            Vector v1 = new Vector();
            Vector v2 = new Vector();
            Vector u = new Vector();
            int i = 4;
            u.x = positionAPCBig.x + i * (v.y - positionAPC.y);
            u.y = positionAPCBig.x - i * (v.x - positionAPC.x);
            v1 = FixIndexesForTurning(u);
            i = 1;
            u.x = positionAPCBig.x + i * (v.y - positionAPC.y);
            u.y = positionAPCBig.x - i * (v.x - positionAPC.x);
            v2 = FixIndexesForTurning(u);

            WindowOfGame.Log("My values: " + positionAPCBig.x + ", " + positionAPCBig.y);
            WindowOfGame.Log("CanIGoThere values: " + v.x + ", " + v.y);
            if (WindowOfGame.GetApcMapBig().GetFieldValue((int)v1.x, (int)v1.y) == 1 || WindowOfGame.GetApcMapBig().GetFieldValue((int)v2.x, (int)v2.y) == 1)
            {
                WindowOfGame.Log("false left");
                return false;
            }

            return true;
        }

        public bool CanIGoThere()
        {
            Vector v = SetFirstField();
            Vector v1 = new Vector();
            Vector v2 = new Vector();
            Vector u = new Vector();
            int i = 4;
                u.x = positionAPCBig.x + i * (v.x - positionAPC.x);
                u.y = positionAPCBig.x + i * (v.y - positionAPC.y);
                v1 = FixIndexesForTurning(u);
            i = 1;
            u.x = positionAPCBig.x + i * (v.x - positionAPC.x);
            u.y = positionAPCBig.x + i * (v.y - positionAPC.y);
            v2 =  FixIndexesForTurning(u);

            WindowOfGame.Log("My values: " + positionAPCBig.x + ", " + positionAPCBig.y);
                WindowOfGame.Log("CanIGoThere values: " + v.x + ", " + v.y);
                if (WindowOfGame.GetApcMapBig().GetFieldValue((int)v1.x, (int)v1.y) == 1 || WindowOfGame.GetApcMapBig().GetFieldValue((int)v2.x, (int)v2.y) == 1)
                {
                WindowOfGame.Log("false");
                    return false;
                }
            
            return true;
        }
        private Vector FixIndexesForTurning(Vector need2fix)
        {
            while (need2fix.x < 0)
            {
                need2fix.x++;
            }
            while (need2fix.y < 0)
            {
                need2fix.x++;
            }
            while (need2fix.x >= 50)
            {
                need2fix.x--;
            }
            while (need2fix.y >= 50)
            {
                need2fix.x--;
            }
            return need2fix;
        }
    }
}
