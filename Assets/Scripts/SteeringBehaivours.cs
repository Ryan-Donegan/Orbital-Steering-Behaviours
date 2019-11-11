using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringBehaivours : MonoBehaviour
{
    //This Class is a personal implementation of Steering Behaivours taken from the paper Steering Behaviors For Autonomous Characters by Craig W. Reynolds
    //Accessed at: https://www.red3d.com/cwr/papers/1999/gdc99steer.pdf
    //Note: Due to current implementation of steering behaivour enemies are expected to always be experiencing a forward thrust, even when this is sub optimal. Notable in the homing mine and when cutting out thrust when facing away from players predicted position.
    //To this end need to look into changing the vector mathematics to take advantage of the characters low g simulation.


    public MovementAndPrediction movementAndPrediction;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Simple Seek Steering Behaivour
    public Vector2 SeekPoint(Vector2 positionVector, Vector2 targetVector, int maxSpeed)
    {
        Vector2 desiredVector;
        Vector2 steeringVector;

        Debug.DrawLine(positionVector, targetVector, Color.green);
        Debug.DrawLine(positionVector, (positionVector + rb.velocity), Color.red);

        desiredVector = ( (positionVector - targetVector).normalized ) * maxSpeed;
        steeringVector = desiredVector - rb.velocity;

        Debug.DrawLine(positionVector, positionVector - steeringVector, Color.blue);
        return steeringVector;
    }

    //Modified Seek Steering Behaivour for Offset Seek
    //Note: Could be further modified in future to include direction component of offset so as to allow Autonomous Characters to actively collaborate on manouveres such as flanking
    public Vector2 SeekPoint(Vector2 positionVector, Vector2 targetVector, int maxSpeed, int offsetRadius)
    {
        Vector2 desiredVector;
        Vector2 steeringVector;
        Vector2 offsetVector;
        offsetVector = (positionVector - targetVector);
        offsetVector = offsetVector * (offsetRadius / offsetVector.magnitude);
   

        desiredVector = offsetVector.normalized * maxSpeed;
        steeringVector = desiredVector - rb.velocity;
        return steeringVector;
    }

    //Simple Flee Steering Behaivour using Inverse of Seek
    public Vector2 FleePoint(Vector2 positionVector, Vector2 targetVector, int maxSpeed)
    {
        Vector2 desiredVector;

        desiredVector = SeekPoint(positionVector, targetVector, maxSpeed);
        desiredVector = desiredVector * -1;
        return desiredVector;
    }

    //Pursuit Steering Behaivour when given array of predicted future positions in an interval of 0.1 seconds
    public Vector2 PursueTarget(Vector2 positionVector, Vector2[] targetPositions, int maxSpeed)
    {
        Vector2 desiredVector;
        int timeIndex;

        timeIndex = (int)((10 * (positionVector - targetPositions[0]).magnitude) / maxSpeed);
        if (timeIndex >= targetPositions.Length)
        {
            timeIndex = targetPositions.Length - 1;
        }
        Vector2 targetVector = targetPositions[timeIndex];
        desiredVector = SeekPoint(positionVector, targetVector, maxSpeed);
        return desiredVector;
    }

    //Modified Pursuit Steering Behaivour when given array of predicted future positions in an interval of 0.1 seconds
    //Note the only change is to pass the new paramater into the SeekPoint method, which handles it.
    public Vector2 PursueTarget(Vector2 positionVector, Vector2[] targetPositions, int maxSpeed, int offsetRadius)
    {
        Vector2 desiredVector;
        int timeIndex;

        timeIndex = (int)((10 * (positionVector - targetPositions[0]).magnitude) / maxSpeed);
        if (timeIndex >= targetPositions.Length)
        {
            timeIndex = targetPositions.Length - 1;
        }
        Vector2 targetVector = targetPositions[timeIndex];
        desiredVector = SeekPoint(positionVector, targetVector, maxSpeed, offsetRadius);
        return desiredVector;
    }

    //Simple Evade Steering Behaivour using Inverse of Pursuit
    public Vector2 FleeTarget(Vector2 positionVector, Vector2[] targetVectors, int maxSpeed)
    {
        Vector2 desiredVector;

        desiredVector = PursueTarget(positionVector, targetVectors, maxSpeed);
        desiredVector = desiredVector * -1;
        return desiredVector;
    }



}

    


