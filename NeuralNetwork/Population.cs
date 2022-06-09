using Godot;
using System;

public class Population : Godot.Object
{
    public Node[] Scenes;

    public readonly int Generation = 0;
    public readonly int Size;
    public float MutationRate = 0.025f;

    public Population(PackedScene scene, int size)
    {
        Size = size;
        Scenes = new Node[size];

        for (int i = 0; i < size; i++)
        {
            Scenes[i] = (Node)scene.Instance();
        }
    }
}


public class DronePopulation : Population
{
    public Drone[] DroneScenes;
    Random rng = new Random();
    NeuralNetwork[] brains;

    public DronePopulation(PackedScene scene, int size) : base(scene, size)
    {
        DroneScenes = new Drone[size];
        brains = new NeuralNetwork[size];
        for (int i = 0; i < size; i++)
        {
            DroneScenes[i] = (Drone)Scenes[i];
            brains[i] = DroneScenes[i].Brain;
        }
    }

    void Selection()
    {

    }

    void UpdateBrains()
    {

    }

    public void Reincarnate()
    {
        Selection();
        UpdateBrains();
    }

    // void Selection()
    // {
    //     float totalScore = 0f;
    //     float[] scores = new float[Size];

    //     foreach (Node scene in Scenes)
    //     {
    //         var drone = (Drone)scene;
    //         drone.CalculateScore();
    //         totalScore += drone.Score;
    //     }

    // }

    // NeuralNetwork PoolSelection(float[] scores, float totalScore)
    // {
    //     int i = -1;
    //     float r = (float)rng.NextDouble() * totalScore;

    //     while (r > 0)
    //     {
    //         i++;
    //         r -= scores[i];
    //     }

    //     return brains[i];
    // }

    // public void Reincarnate()
    // {
    //     Selection();

    //     for (int i = 0; i < Size; i++)
    //     {
    //         Scenes[i].QueueFree();
    //         Scenes[i] = (Node)_scene.Instance();
    //         Drone drone = (Drone)Scenes[i];
    //         drone.Brain = brains[i];
    //     }
    // }
}
