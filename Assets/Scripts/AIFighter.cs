using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFighter : AIHomingMissile
{
    protected float fireCooldown;
    public GameObject bullet;
    protected GameObject b;
    protected Rigidbody2D bRigidBody;

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
        if( CheckDistance(playerPositions[0]) < range && attackTime < fireCooldown)
        {
            Fire();
        }

        targetVector = steeringBehaivours.PursueTarget(rb.position, playerPositions, maxSpeed, range / 2);
        Rotate();
        rb.AddForce((transform.up) * (maxThrust));

        ClampMaxSpeed();
    }

    protected void Fire()
    {
        b = Instantiate(bullet, transform.position , transform.rotation);
        bRigidBody = b.GetComponent<Rigidbody2D>();
        bRigidBody.velocity = ((rb.velocity).normalized * 80);
        fireCooldown = 0;
    }
}
