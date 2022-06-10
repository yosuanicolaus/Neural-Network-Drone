using Godot;
using System;

public class Population : Godot.Object
{
    public Node[] Scenes;
    public PackedScene _scene;

    public int Generation = 0;
    public readonly int Size;
    public Population(PackedScene scene, int size)
    {
        _scene = scene;
        Size = size;
        Scenes = new Node[size];

        for (int i = 0; i < size; i++)
        {
            Scenes[i] = (Node)scene.Instance();
        }
    }

    public void Reinstance()
    {
        for (int i = 0; i < Size; i++)
        {
            Scenes[i].QueueFree();
            Scenes[i] = (Node)_scene.Instance();
        }
    }
}


public class DronePopulation : Population
{
    public Drone[] DroneScenes;
    Random rng = new Random();

    public double MutationRate = 0.025;
    public double SoftMutationRate = 0.1;

    NeuralNetwork[] brains;
    NeuralNetwork overallBestBrain;
    double[] fitness;
    double totalScore = 0;
    public double OverallBestScore = 0;

    public DronePopulation(PackedScene scene, int size) : base(scene, size)
    {
        DroneScenes = new Drone[size];
        brains = new NeuralNetwork[size];
        fitness = new double[size];

        for (int i = 0; i < size; i++)
        {
            DroneScenes[i] = (Drone)Scenes[i];
            brains[i] = DroneScenes[i].Brain;
        }
    }

    void Selection()
    {
        totalScore = 0;
        double bestScore = 0;
        NeuralNetwork bestBrain = DroneScenes[0].Brain;

        for (int i = 0; i < Size; i++)
        {
            double score = DroneScenes[i].Score;
            fitness[i] = score;
            totalScore += score;
            if (score > bestScore)
            {
                bestScore = score;
                bestBrain = DroneScenes[i].Brain;

                if (bestScore > OverallBestScore)
                {
                    OverallBestScore = bestScore;
                    overallBestBrain = bestBrain;
                }
            }
        }

        for (int i = 0; i < Size; i++)
        {
            if (i == 0 && overallBestBrain != null)
            {
                brains[i] = overallBestBrain.Copy();
            }
            brains[i] = bestBrain.Copy();
            brains[i].SoftMutate(SoftMutationRate);
        }
    }

    NeuralNetwork SelectBrain()
    {
        int i = -1;
        double r = rng.NextDouble() * totalScore;
        while (r > 0)
        {
            i++;
            r -= fitness[i];
        }
        return brains[i];
    }

    void Restart()
    {
        for (int i = 0; i < Size; i++)
        {
            Scenes[i].QueueFree();
            Scenes[i] = (Node)_scene.Instance();
            DroneScenes[i] = (Drone)Scenes[i];
            DroneScenes[i].Brain = brains[i];
        }
    }

    public void Reincarnate()
    {
        Generation++;
        Selection();
        Restart();
    }
}
