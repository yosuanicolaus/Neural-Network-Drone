using Godot;
using System;

public class Drone : RigidBody2D
{
    [Export] float ThrusterAngularSpeed = 240f;
    [Export] float ThrusterForce = 300f;
    [Export] float ForceLimit = 100f;
    private float _maxRotationDegree = 75f;

    Vector2 LeftDirection = new Vector2();
    Vector2 RightDirection = new Vector2();
    Vector2 LeftForce = new Vector2();
    Vector2 RightForce = new Vector2();

    Node2D LeftThruster;
    Node2D RightThruster;
    Node2D LTTarget;
    Node2D RTTarget;

    public override void _Ready()
    {
        LeftThruster = GetNode<Node2D>("Left");
        RightThruster = GetNode<Node2D>("Right");
        LTTarget = GetNode<Node2D>("Left/Target");
        RTTarget = GetNode<Node2D>("Right/Target");
    }

    public override void _PhysicsProcess(float delta)
    {
        GetDirections(delta);
        GetHumanControl(delta);

        // ApplyImpulse(LeftThruster.Position, LeftForce);
        // ApplyImpulse(RightThruster.Position, RightForce);
    }

    void GetDirections(float delta)
    {
        LeftForce = Vector2.Zero;
        RightForce = Vector2.Zero;

        LeftDirection = (LTTarget.GlobalPosition - LeftThruster.GlobalPosition).Normalized();
        RightDirection = (RTTarget.GlobalPosition - RightThruster.GlobalPosition).Normalized();

        LeftForce = LeftDirection * ThrusterForce * delta;
        RightForce = RightDirection * ThrusterForce * delta;
    }

    private void GetHumanControl(float delta)
    {
        if (Input.IsActionPressed("ui_left"))
        {
            LeftThruster.RotationDegrees += ThrusterAngularSpeed * delta;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            RightThruster.RotationDegrees += ThrusterAngularSpeed * delta;
        }
        if (Input.IsActionPressed("ui_up"))
        {
            LeftThruster.RotationDegrees -= ThrusterAngularSpeed * delta;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            RightThruster.RotationDegrees -= ThrusterAngularSpeed * delta;
        }

        LeftThruster.RotationDegrees = Mathf.Clamp(LeftThruster.RotationDegrees, -_maxRotationDegree, _maxRotationDegree);
        RightThruster.RotationDegrees = Mathf.Clamp(RightThruster.RotationDegrees, -_maxRotationDegree, _maxRotationDegree);
    }
}
