using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyAiAbstract : MonoBehaviour
{

    protected SteeringBehaivours steeringBehaivours;
    protected Rigidbody2D rb;
    public int maxSpeed, maxThrust, maxTurn;
    protected float targetAngle;
    public ParticleSystem thrust, thrustSteerR, thrustSteerL;
    protected Vector2 targetVector;
    public int range;
    public int attackTime;

    // Start is called before the first frame update
    void Start()
    {
        steeringBehaivours = GetComponent<SteeringBehaivours>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Abstract method to be overloaded in all Enemy AI subclasses
    public abstract void AiUpdate(Vector2[] playerPositions, Vector2 playerVelocity);


    //Clamp Velocity to max speed
    protected void ClampMaxSpeed()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    protected float CheckDistance(Vector2 targetPosition)
    {
        return (rb.position - targetPosition).magnitude;
    }



}

    








