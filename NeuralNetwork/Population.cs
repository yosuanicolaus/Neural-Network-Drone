using Godot;
using System;

public class Population : Godot.Object
{
    public Node[] Scenes;
    public PackedScene _scene;

    public readonly int Generation = 0;
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
    public double SoftMutationRate = 0.25;

    NeuralNetwork[] brains;
    double[] fitness;
    double totalScore = 0;

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

        for (int i = 0; i < Size; i++)
        {
            fitness[i] = DroneScenes[i].Score;
            totalScore += DroneScenes[i].Score;
        }

        for (int i = 0; i < Size; i++)
        {
            NeuralNetwork newBrain = SelectBrain().Copy();
            newBrain.Mutate(MutationRate, SoftMutationRate);
            brains[i] = newBrain;
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
        Selection();
        Restart();
    }
}
