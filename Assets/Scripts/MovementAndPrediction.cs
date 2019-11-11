using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAndPrediction : MonoBehaviour
{

    //Variable Declaration
    public KeyCode Thrust;
    public Rigidbody2D rb;
    public KeyCode Brake;
    public KeyCode Left;
    public KeyCode Right;
    public float CurrentX;
    public float CurrentY;
    public Transform cam;
    private const int ArrayLength = 51;
    public int MaxSpeed = 30;
    private Vector2 PredictedPosition;
    private float Ux;
    private float Uy;
    private float Sx;
    private float Sy;
    private float Ax;
    private float Ay;
    private const float t = 0.1F;
    private Vector2 gravityFieldLocation = new Vector2(0, 0);
    private Vector2 PlayerVelocity;
    private GameObject[] gravityFields;
    private CircleCollider2D gravityCollider;
    private PointEffector2D gravityEffector;
    public GameObject AiController;
    public AiControl AiControlScript;
    public Vector2[] positions;
    public int thrust = 10;
    public ParticleSystem mainThrust, steerThrustR, steerThrustL;


    


    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        AiController = GameObject.Find("EnemyController");
        AiControlScript = AiController.GetComponent<AiControl>();
        mainThrust.Stop();
        steerThrustL.Stop();
        steerThrustR.Stop();
    }

    void Update()
    {
        //Graphical Representation of thrust
        if (Input.GetKeyDown(Thrust))
        {
            mainThrust.Play();
        }
        else if(Input.GetKeyUp(Thrust))
        {
            mainThrust.Stop();
        }

        if (Input.GetKeyDown(Left))
        {
            steerThrustL.Play();
        }
        else if (Input.GetKeyUp(Left))
        {
            steerThrustL.Stop();
        }

        if (Input.GetKeyDown(Right))
        {
            steerThrustR.Play();
        }
        else if (Input.GetKeyUp(Right))
        {
            steerThrustR.Stop();
        }
    }

    



    // Update is called on physics timestep
    void FixedUpdate()
    {

        //Array of future position co-ordinates, note the entire quantity may not nesacerily be needed by other objects but in the initial calculation such precision improves accuracy of the later co-ordinates
        positions = new Vector2[ArrayLength];

        //MOVEMENT:

        //Get Initial velocity
        Vector2 InitialVelocity = rb.velocity;

        if (Input.GetKey(Thrust))
        {
            //Thrust 'forward' (direction of local forward axis)
            rb.AddForce(transform.up * (thrust));
            
        }

        if (Input.GetKey(Brake))
        {
            //Thrust in opposite direction to initial velocity
            rb.AddForce((-InitialVelocity.normalized) * (thrust / 2));
        }

        if (Input.GetKey(Left))
        {
            //Rotate Anticlockwise
            transform.Rotate(Vector3.forward * 5);
        }

        if (Input.GetKey(Right))
        {
            //Rotate Clockwise
            transform.Rotate(Vector3.back * 5);
        }


        //Clamp Velocity to max speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, MaxSpeed);



        //MOTION PREDICTION:


        //Get Position
        CurrentX = transform.position.x;
        CurrentY = transform.position.y;

        //Set zero (first) value of position prediction array.
        positions[0] = transform.position;

        //Get initial Velocity X and Y components
        Ux = rb.velocity.x;
        Uy = rb.velocity.y;

        //Find all gravity sources
        gravityFields = GameObject.FindGameObjectsWithTag("GravityField");

        //Iterate through every item in positions array (from one), using the previous item to calculate each co-ordinate filling out 5 seconds worth of predictions in intervals of 0.1 seconds.
        for (int count = 1; count < ArrayLength; count = count + 1)
        {

            //Initially set acceleration components to zero.
            Ax = 0;
            Ay = 0;

            //Iterate through gravity Sources to identify acceleration due to gravity at point
            foreach (GameObject gravityField in gravityFields)
            {
                gravityFieldLocation = new Vector2(gravityField.transform.parent.position.x, gravityField.transform.parent.position.y);
                gravityCollider = gravityField.GetComponent<CircleCollider2D>();

                

                //GravityField at point
                if (((positions[count - 1]) - (gravityFieldLocation)).magnitude < gravityCollider.radius)
                {
                    gravityEffector = gravityField.GetComponent<PointEffector2D>();

                    //Calculate change in X and Y components of Acceleration due to gravity at point
                    //Note Current set up simualtes a constant force, but is easily modifiable to do a more realistic gravity simulation where the force is proportional to R
                    Ax = Ax + ((((positions[count - 1]) - (gravityFieldLocation)).normalized) * (gravityEffector.forceMagnitude)).x;
                    Ay = Ay + ((((positions[count - 1]) - (gravityFieldLocation)).normalized) * (gravityEffector.forceMagnitude)).y;

                }

            }


            //Use Suvat to calculate displacement components
            Sx = (Ux * t) + ((Ax * t * t) / 2);
            Sy = (Uy * t) + ((Ay * t * t) / 2);

            //Use Suvat to calculate new velocity components for next iteration of the loop.
            Ux = (Ux + (Ax * t));
            Uy = (Uy + (Ay * t));

            //Use displacement to calculate co-ordinates of player 0.1 seconds ahead of 'current' time.
            PredictedPosition = new Vector2(CurrentX + Sx, CurrentY + Sy);

            //Add this to array of co-ordinate predictions
            positions[count] = PredictedPosition;


            CurrentX = PredictedPosition.x;
            CurrentY = PredictedPosition.y;

            //Debug graphical representation of path of motion
            Debug.DrawLine(positions[count - 1], positions[count], Color.red);

        }



        PlayerVelocity = rb.velocity;
        AiControlScript.AiUpdateMethod(positions, PlayerVelocity);

        //CAMERA:

        //Set Camera Position to equal to current player position
        cam.position = new Vector3(positions[0].x, positions[0].y, -100);

    }
}
