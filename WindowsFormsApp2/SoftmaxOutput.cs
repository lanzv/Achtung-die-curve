using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchtungDieCurve
{
    public class SoftmaxOutput
    {
        Output[] outputs;
        Neuron[] neurons;
        List<TrainingValues> DataSet = new List<TrainingValues>();

        public SoftmaxOutput()
        {
        }
        public void SetSoftmaxOutput(int numOut, Neuron[] neurnos2)
        {
            outputs = new Output[numOut];
            neurons = neurnos2;
            SetOutputsZ();
            SetOutputsY();
            //pouze pokud chci trenovat neuronovou sit:
            //SaveTrainingData(neurons, outputs);
        }
        private void SaveTrainingData(Neuron[] neurs, Output[] outs)
        {
            TrainingValues tv = new TrainingValues();
            tv.y = new double[outs.Length];
            for (int i = 0; i < outs.Length; i++)
            {
                tv.y[i] = outs[i].z;
            }
            tv.neurons = neurs;
            tv.t = WindowOfGame.GetTrainingValues(tv.y);
            DataSet.Add(tv);

        }
        private void SetOutputsZ()
        {
            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i].z = 0;
                for (int j = 0; j < neurons.Length; j++)
                {
                    outputs[i].z = outputs[i].z + (neurons[j].W[i] * neurons[j].x + neurons[j].b[i]);
                }
            }
        } 
        private void SetOutputsY()
        {
            double sum = 0;
            for (int i = 0; i < outputs.Length; i++)
            {
                sum = sum + outputs[i].z;
            }
            for (int i = 0; i < outputs.Length; i++)
            {
                outputs[i].y = outputs[i].z / sum;
            }
        }
        public double[,] GetDeltaWs()
        {
            double[,] dW = new double[outputs.Length, neurons.Length];
            double epsilon = 0.5;


            for (int i = 0; i < outputs.Length; i++)
            {
                for (int j = 0; j < neurons.Length; j++)
                {
                    dW[i, j] = 0;
                    foreach (TrainingValues set in DataSet){
                        dW[i, j] = dW[i, j] + set.neurons[j].x * Math.Pow(Math.Pow((set.t[i] - set.y[i]), 2), (double)((double)1 / (double)DataSet.Count));
                    }
                    dW[i, j] = epsilon * dW[i, j];
                }
            }
            DataSet = new List<TrainingValues>();
            return dW;
        }
        public int GetTheHighestOutput()
        {
            double maxOut = 0;
            int maxInd = 3; //jakoze index pro to jet rovne, je to jedno
            for (int i = 0; i < outputs.Length; i++)
            {
                if(outputs[i].y > maxOut)
                {
                    maxOut = outputs[i].y;
                    maxInd = i;
                }
            }
            return maxInd;
        }
    }
    

    struct Output
    {
        public double z; //prichod
        public double y; //vystup ... procenta deleno sto
    }
    struct TrainingValues
    {
        public Neuron[] neurons;
        public double[] y;   //actual
        public double[] t;   //expected
    }
}
