using Godot;
using System;

public class Main : Node2D
{
    PackedScene droneScene = (PackedScene)ResourceLoader.Load("res://Drone/Drone.tscn");
    Population population;
    Area2D Point;
    Vector2[] Positions = new Vector2[10] {
        new Vector2(747, 495),
        new Vector2(302, 559),
        new Vector2(851, 16),
        new Vector2(717, 400),
        new Vector2(208, 451),
        new Vector2(753, 2),
        new Vector2(383, 171),
        new Vector2(862, 296),
        new Vector2(596, 36),
        new Vector2(142, 490),
    };
    private int _nextIndex = 0;

    public override void _Ready()
    {
        Point = GetNode<Area2D>("Point");
        NextPointPosition();
        population = new Population(droneScene, 25);

        foreach (Drone drone in population.Scenes)
        {
            drone.SetTarget(Point);
            AddChild(drone);
        }
    }

    void NextPointPosition()
    {
        Point.Position = Positions[_nextIndex];
        _nextIndex++;
        if (_nextIndex >= Positions.Length)
        {
            _nextIndex = 0;
        }
    }

    // disconnected
    void _on_Point_body_entered(object body)
    {
        var drone = body as Drone;
        if (drone != null)
        {
            drone.IncrementPoint();
            NextPointPosition();
            GD.Print("Point: " + drone.Point);
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
}
