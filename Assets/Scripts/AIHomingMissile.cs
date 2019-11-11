using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHomingMissile : EnemyAiAbstract
{

    // Start is called before the first frame update
    void Start()
    {
        steeringBehaivours = gameObject.GetComponent<SteeringBehaivours>();
        rb = GetComponent<Rigidbody2D>();
        thrust.Play();
        thrustSteerR.Play();
        thrustSteerL.Play();
    }

    private void Update()
    {
        RotateThrust();
    }
    public override void AiUpdate(Vector2[] playerPositions, Vector2 playerVelocity)
    {
        targetVector = steeringBehaivours.PursueTarget(rb.position, playerPositions, maxSpeed);
        Rotate();
        rb.AddForce((transform.up) * (maxThrust));
        ClampMaxSpeed();
    }

    public void Rotate()
    {
        targetAngle = Vector2.SignedAngle(transform.up, targetVector);
        transform.Rotate(0, 0, (maxTurn / -targetAngle));
    }

    public void RotateThrust()
    {
        if (targetAngle > 0 && (thrustSteerL.isPlaying))
        {
            thrustSteerR.Play();
            thrustSteerL.Stop();
        }
        else if ((targetAngle < 0) && (thrustSteerR.isPlaying))
        {
            thrustSteerL.Play();
            thrustSteerR.Stop();
        }
    }
}
