using Godot;
using System;

public class Main : Node2D
{
    PackedScene droneScene = (PackedScene)ResourceLoader.Load("res://Drone/Drone.tscn");
    PackedScene pointScene = (PackedScene)ResourceLoader.Load("res://Main/Point.tscn");

    DronePopulation Drones;
    Population Points;
    public int Size = 5;

    public override void _Ready()
    {
        Drones = new DronePopulation(droneScene, Size);
        Points = new Population(pointScene, Size);

        for (int i = 0; i < Size; i++)
        {
            Drone drone = Drones.DroneScenes[i];
            Point point = (Point)Points.Scenes[i];
            drone.TargetPoint = point;
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
            GetTree().ReloadCurrentScene();
        }
    }

    void onDroneCrashed()
    {
        GD.Print("Crashed!");
        Size--;
        if (Size == 0)
        {
            Size = Drones.Size;
            Drones.Reincarnate();
            // GetTree().ReloadCurrentScene();
        }
    }
}
