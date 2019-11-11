using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHomingMine : EnemyAiAbstract
{

    // Start is called before the first frame update
    void Start()
    {
        steeringBehaivours = gameObject.GetComponent<SteeringBehaivours>();
        rb = GetComponent<Rigidbody2D>();
        thrust.Play();
    }

    public override void AiUpdate(Vector2[] playerPositions, Vector2 playerVelocity)
    {
        targetVector = steeringBehaivours.PursueTarget(rb.position, playerPositions, maxSpeed);

        rb.AddForce((targetVector.normalized) * (-maxThrust));
        Thrust(targetVector);
        ClampMaxSpeed();
    }

    private void Thrust(Vector2 thrustDirection)
    {
        thrust.transform.rotation = Quaternion.LookRotation(thrustDirection * -1);

    }
}
