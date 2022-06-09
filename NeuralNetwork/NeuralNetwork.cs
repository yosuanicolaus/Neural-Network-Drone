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

    public void Mutate(double rate = 0.025, double softRate = 0.25)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].Mutate(rate, softRate);
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
            for (int j = 0; j < Outputs.Length; j++)
            {
                Weights[i, j] = rng.NextDouble() * 2 - 1;

                if (i == 0)
                {
                    Biases[j] = rng.NextDouble() * 2 - 1;
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

        for (int i = 0; i < Outputs.Length; i++)
        {
            double sum = 0;
            for (int j = 0; j < Inputs.Length; j++)
            {
                sum += Inputs[j] * Weights[j, i];
            }

            if (sum > Biases[i])
            {
                Outputs[i] = 1;
            }
            else
            {
                Outputs[i] = 0;
            }
        }

        return Outputs;
    }

    public void Mutate(double rate = 0.025, double softRate = 0.25)
    {
        for (int i = 0; i < Inputs.Length; i++)
        {
            for (int j = 0; j < Outputs.Length; j++)
            {
                double rw = rng.NextDouble();
                if (rw < rate)
                {
                    Weights[i, j] = rng.NextDouble() * 2 - 1;
                }
                else if (rw < rate * 4)
                {
                    Weights[i, j] = SoftMutate(Weights[i, j], softRate);
                }
            }
            double rb = rng.NextDouble();
            if (rb < rate)
            {
                Biases[i] = rng.NextDouble() * 2 - 1;
            }
            else if (rb < rate * 4)
            {
                Biases[i] = SoftMutate(Biases[i], softRate);
            }
        }
    }

    double SoftMutate(double value, double rate)
    {
        // Lerp to <Random>(-1, 1) at a rate
        return value + ((rng.NextDouble() * 2 - 1) - value) * rate;
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
