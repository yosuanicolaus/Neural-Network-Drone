using Godot;
using System;

public class Drone : RigidBody2D
{
    float ThrusterAngularSpeed = 240f;
    float ThrusterForce = 300f;
    private float _maxRotationDegree = 75f;

    public int Point = 0;
    public float Time = 0f;

    Vector2 LeftDirection = new Vector2();
    Vector2 RightDirection = new Vector2();
    Vector2 LeftForce = new Vector2();
    Vector2 RightForce = new Vector2();
    Vector2 Velocity = new Vector2();

    Node2D LeftThruster;
    Node2D RightThruster;
    Node2D LTTarget;
    Node2D RTTarget;
    public Node2D TargetPoint;

    NeuralNetwork Brain = new NeuralNetwork(new int[] { 8, 10, 10, 4 });
    double[] Inputs = new double[8];
    double[] Outputs = new double[4];

    public override void _Ready()
    {
        Position = GetViewportRect().Size / 2;
        LeftThruster = GetNode<Node2D>("Left");
        RightThruster = GetNode<Node2D>("Right");
        LTTarget = GetNode<Node2D>("Left/Target");
        RTTarget = GetNode<Node2D>("Right/Target");
    }

    public override void _PhysicsProcess(float delta)
    {
        FeedBrain(delta);
        Activate(delta);
        ClampDegrees();
        GetDirections(delta);
    }

    void FeedBrain(float delta)
    {
        Inputs[0] = TargetPoint.Position.x - Position.x;
        Inputs[1] = TargetPoint.Position.y - Position.y;
        Inputs[2] = LinearVelocity.x;
        Inputs[3] = LinearVelocity.y;
        Inputs[4] = AngularVelocity;
        Inputs[5] = LeftThruster.GlobalRotation;
        Inputs[6] = RightThruster.GlobalRotation;
        Inputs[7] = GlobalRotation;

        Outputs = Brain.FeedForward(Inputs);
    }

    void Activate(float delta)
    {
        if (Outputs[0] > 0.5)
        {
            LeftThruster.RotationDegrees += ThrusterAngularSpeed * delta;
        }
        else
        {
            LeftThruster.RotationDegrees -= ThrusterAngularSpeed * delta;
        }
        if (Outputs[1] > 0.5)
        {
            RightThruster.RotationDegrees += ThrusterAngularSpeed * delta;
        }
        else
        {
            RightThruster.RotationDegrees -= ThrusterAngularSpeed * delta;
        }
        if (Outputs[2] > 0.5)
        {
            ApplyImpulse(LeftThruster.Position, LeftForce);
        }
        if (Outputs[3] > 0.5)
        {
            ApplyImpulse(RightThruster.Position, RightForce);
        }
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

    void ClampDegrees()
    {
        LeftThruster.RotationDegrees = Mathf.Clamp
        (LeftThruster.RotationDegrees, -_maxRotationDegree, _maxRotationDegree);
        RightThruster.RotationDegrees = Mathf.Clamp
        (RightThruster.RotationDegrees, -_maxRotationDegree, _maxRotationDegree);
    }
}
