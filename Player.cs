using Godot;

namespace Platformer3D;

public partial class Player : CharacterBody3D
{
    [Export] private float _moveSpeed = 4.0f;
    [Export] private float _jumpForce = 8.0f;
    [Export] private float _gravity = 20.0f;
    
    private float _facingAngle;

    private MeshInstance3D _model;

    public override void _Ready()
    {
        _model = GetNode<MeshInstance3D>("Model");
    }

    public override void _Process(double delta)
    {
        if (GlobalPosition.Y < -10)
        {
            GameOver();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Velocity;
        var input = Input.GetVector(
            "move_left",
            "move_right",
            "move_forward",
            "move_backward"
        );

        if (!IsOnFloor())
        {
            velocity.Y -= _gravity * (float) delta;
        } 
        else if (Input.IsActionPressed("jump"))
        {
            velocity.Y = _jumpForce;
        }
        
        velocity.X = input.X * _moveSpeed;
        velocity.Z = input.Y * _moveSpeed;
        
        Velocity = velocity;
        
        MoveAndSlide();
        RotateMode(input, delta);
    }

    private void RotateMode(Vector2 input, double delta)
    {
        var rotation = _model.Rotation;
        if (input.Length() > 0)
        {
            _facingAngle = new Vector2(input.Y, input.X).Angle();
        }
        
        rotation.Y = (float) Mathf.Lerp(_model.Rotation.Y, _facingAngle, 0.5f);
            
        _model.Rotation = rotation;
    }

    private void GameOver()
    {
        GetTree().ReloadCurrentScene();
    }
}