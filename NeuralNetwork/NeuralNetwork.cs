using System;

public class NeuralNetwork
{
    Level[] levels;

    public NeuralNetwork(int[] layers)
    {
        levels = new Level[layers.Length - 1];
        for (int i = 0; i < layers.Length - 1; i++)
        {
            levels[i] = new Level(layers[i], layers[i + 1]);
        }
    }

    public double[] FeedForward(double[] inputs)
    {
        double[] outputs = levels[0].FeedForward(inputs);

        for (int i = 1; i < levels.Length; i++)
        {
            outputs = levels[i].FeedForward(outputs);
        }

        return outputs;
    }

    public void Mutate(double rate)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].Mutate(rate);
        }
    }

    public void SoftMutate(double softRate)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].SoftMutate(softRate);
        }
    }

    public NeuralNetwork Copy()
    {
        NeuralNetwork copy = new NeuralNetwork(new int[levels.Length + 1]);
        for (int i = 0; i < levels.Length; i++)
        {
            copy.levels[i] = levels[i].Copy();
        }
        return copy;
    }
}

class Level
{
    double[] Inputs;
    double[] Outputs;
    double[] Biases;
    double[,] Weights;
    Random rng = new Random();

    public Level(int inputCount, int outputCount)
    {
        Inputs = new double[inputCount];
        Outputs = new double[outputCount];
        Biases = new double[outputCount];
        Weights = new double[inputCount, outputCount];

        Randomize();
    }

    private void Randomize()
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int o = 0; o < Outputs.Length; o++)
            {
                Weights[i, o] = rng.NextDouble() * 2 - 1;

                if (i == 0)
                {
                    Biases[o] = rng.NextDouble() * 2 - 1;
                }
            }
        }
    }

    public double[] FeedForward(double[] newInput)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            Inputs[i] = newInput[i];
        }

        for (int o = 0; o < Outputs.Length; o++)
        {
            double sum = 0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                sum += Inputs[i] * Weights[i, o];
            }

            if (sum > Biases[o])
            {
                Outputs[o] = 1;
            }
            else
            {
                Outputs[o] = 0;
            }
        }

        return Outputs;
    }

    public void Mutate(double rate)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int o = 0; o < Outputs.Length; o++)
            {
                if (i == 0)
                {
                    double rb = rng.NextDouble();
                    if (rb < rate)
                    {
                        Biases[o] = rng.NextDouble() * 2 - 1;
                    }
                    // else
                    // {
                    //     Biases[o] = SoftMutate(Biases[o], softRate);
                    // }
                }

                double rw = rng.NextDouble();
                if (rw < rate)
                {
                    Weights[i, o] = rng.NextDouble() * 2 - 1;
                }
                // else
                // {
                //     Weights[i, o] = SoftMutate(Weights[i, o], softRate);
                // }
            }
        }
    }

    public void SoftMutate(double softRate)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int o = 0; o < Outputs.Length; o++)
            {
                if (i == 0)
                {
                    Biases[o] = Lerp(Biases[o], rng.NextDouble() * 2 - 1, softRate);
                }

                Weights[i, o] = Lerp(Weights[i, o], rng.NextDouble() * 2 - 1, softRate);
            }
        }
    }

    static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * t;
    }

    public Level Copy()
    {
        Level copy = new Level(Inputs.Length, Outputs.Length);

        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int j = 0; j < Outputs.Length; j++)
            {
                copy.Weights[i, j] = Weights[i, j];

                if (i == 0)
                {
                    copy.Biases[j] = Biases[j];
                }
            }
        }
        return copy;
    }
}
