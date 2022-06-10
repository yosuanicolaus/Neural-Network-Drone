using Godot;
using System;

public class Point : Area2D
{
    Vector2[] Positions = new Vector2[10] {
        new Vector2(596, 36),
        new Vector2(302, 559),
        new Vector2(851, 16),
        new Vector2(747, 495),
        new Vector2(753, 2),
        new Vector2(208, 451),
        new Vector2(717, 400),
        new Vector2(383, 171),
        new Vector2(862, 296),
        new Vector2(142, 490),
    };
    private int _positionIndex = 0;

    public void NextPointPosition()
    {
        Position = Positions[_positionIndex];
        _positionIndex++;
        if (_positionIndex >= Positions.Length)
        {
            _positionIndex = 0;
        }
    }

    void _on_Point_body_entered(object body)
    {
        var drone = body as Drone;
        if (drone.TargetPoint == this)
        {
            NextPointPosition();
            drone.HitTarget();
        }
    }
}
