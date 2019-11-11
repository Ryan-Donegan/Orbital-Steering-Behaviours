using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBomber : AIFighter
{
    // Start is called before the first frame update
    void Start()
    {
        steeringBehaivours = gameObject.GetComponent<SteeringBehaivours>();
        rb = GetComponent<Rigidbody2D>();
        thrust.Play();
        thrustSteerR.Play();
        thrustSteerL.Play();
        fireCooldown = attackTime;
    }

    // Update is called once per frame
    void Update()
    {
        RotateThrust();
        fireCooldown = fireCooldown + Time.deltaTime;
    }

    public override void AiUpdate(Vector2[] playerPositions, Vector2 playerVelocity)
    {
        
        if (attackTime < fireCooldown)
        {
            if (CheckDistance(playerPositions[0]) < range)
            {
                Fire();
            }
            targetVector = steeringBehaivours.PursueTarget(rb.position, playerPositions, maxSpeed, range / 2);
            Rotate();
            rb.AddForce((transform.up) * (maxThrust));
        }
        else
        {
            targetVector = steeringBehaivours.FleeTarget(rb.position, playerPositions, maxSpeed);
            Rotate();
            rb.AddForce((transform.up) * (maxThrust));
        }
        ClampMaxSpeed();
    }

}
