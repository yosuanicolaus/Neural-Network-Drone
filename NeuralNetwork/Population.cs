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