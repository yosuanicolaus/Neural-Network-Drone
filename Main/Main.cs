using Godot;
using System;

public class Main : Node2D
{
    PackedScene droneScene = (PackedScene)ResourceLoader.Load("res://Drone/Drone.tscn");
    PackedScene pointScene = (PackedScene)ResourceLoader.Load("res://Main/Point.tscn");

    DronePopulation Drones;
    Population Points;
    public int Size = 100;

    public override void _Ready()
    {
        Drones = new DronePopulation(droneScene, Size);
        Points = new Population(pointScene, Size);
        StartSimulation();
    }

    void StartSimulation()
    {
        for (int i = 0; i < Size; i++)
        {
            Drone drone = Drones.DroneScenes[i];
            Point point = (Point)Points.Scenes[i];
            point.NextPointPosition();
            drone.SetTarget(point);
            drone.Connect("Crashed", this, "onDroneCrashed");
            AddChild(drone);
            AddChild(point);
        }
    }

    public override void _Input(InputEvent @event)
    {
        // For fast reload.
        if (@event.IsActionPressed("ui_cancel"))
        {
            Restart();
        }
    }

    void onDroneCrashed()
    {
        Size--;
        if (Size == 0)
        {
            Restart();
        }
    }

    void Restart()
    {
        Size = Drones.Size;
        Drones.Reincarnate();
        Points.Reinstance();
        StartSimulation();
    }
}
