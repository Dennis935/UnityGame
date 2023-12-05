using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    //Make the variable editable in the Unity Editor
    [SerializeField]
    private float paddleSpeed = 2f; // The Speed by Which the Paddle can be moved left right

    [SerializeField]
    private float reflectingForce = 1f; // The Force the ball will get pushed by the Paddle

    [SerializeField]
    private float maxBallVelocity = 15f; // The Maximum Speed the Ball will accumulate when not set to contant speed

    [SerializeField]
    private bool useConstantBallSpeed = true;  // Due to Physics and the Bounciness of the Border/Paddle/Bricks the Ball accumulates speed
                                               // it is getting less controllable from these pushes to the rigidbody; controlling for contant speed
                                               // keeps the Ball more controllable for better playing experience

    [SerializeField]
    private float constantBallSpeed = 12f;

    [SerializeField]
    private Rigidbody ballRb; // The Ball's Rigidbody 

    [SerializeField]
    private float maxReflectionAngleInDegrees = 75f; // The Default Reflection Angle the Paddle reflects the Ball right or left from the centre

    [SerializeField]
    public GameObject topBorder; // Reference to the Top Cube Border -> Top reflects the Ball
    [SerializeField]
    public GameObject leftBorder; // Reference to the Left Cube Border -> Left reflects the Ball
    [SerializeField]
    public GameObject rightBorder; // Reference to the Right Cube Border -> Right reflects the Ball
    [SerializeField]
    public GameObject bottomBorder; // Reference to the Bottom Cube Border -> Bottom is a Trigger when Ball hits it Life is lost

    private float halfWidth; // Half of paddle's width
    private float leftLimit; // Left boundary for movement of paddle 
    private float rightLimit; // Right boundary for movement of paddle

    private void Start ()
    {
        // Calculate paddle's half width using its collider 
        halfWidth = GetComponent<Collider>().bounds.extents.x;

        // Define movement boundaries for paddle based on cube borders' position and their collider contents
        leftLimit = leftBorder.GetComponent<Collider>().bounds.max.x + halfWidth;
        rightLimit = rightBorder.GetComponent<Collider>().bounds.min.x - halfWidth;
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; //Allows a more smooth movement of the paddle
    }

    private void FixedUpdate()
    {
        // Do not process FixedUpdate when Game is stopped entirely
        if (Time.timeScale == 0f)
            return;

        // Move paddle with user input on the x axis controller independent from Input Hardware
        // Ändere die Bedingung für die Tasten A und D
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            // Ändere die Tastenabfrage auf "A" und "D"
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 newPosition = transform.position + new Vector3(horizontal * paddleSpeed * Time.deltaTime, 0, 0);

            // Make sure the paddle stays inside the borders
            newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);

            // Update the paddle's position
            transform.position = newPosition;
        }

        // Adjust the balls maximum velocity for control of difficulty / levels when not using contant ball speed
        if (ballRb !=null && ballRb.transform.position.y > 33)
        {
            if (ballRb.velocity.magnitude > maxBallVelocity)
            {
                ballRb.velocity = ballRb.velocity.normalized * maxBallVelocity;
            }
        }

        // Use Constant Ball Speed so the Game remains more controllable 
        if (useConstantBallSpeed)
        {
            ballRb.velocity = constantBallSpeed * (ballRb.velocity.normalized);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has the tag "Ball"
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Get the Rigidbody component of the ball
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            // Ensure we obtained the Rigidbody 
            if (ballRb)
            {
                // Get the point where the ball hit the paddle
                Vector3 hitPoint = collision.GetContact(0).point;

                // Determine the paddle's center 
                Vector3 paddleCenter = new Vector3(transform.position.x, transform.position.y);

                // Calculate horizontal offset where the ball hit relative to the paddle center
                float offset = hitPoint.x - paddleCenter.x;

                // Get half the width of the paddle using its collider's bounds
                float halfPaddleWidth = GetComponent<Collider>().bounds.size.x / 2;

                // Normalize offset between - 1 (left edge) and 1 (right edge)
                float normalizedOffset = offset / halfPaddleWidth;

                // Calculate reflection angle based on where the ball hit
                // Center is 0 degrees. Edges yield max angle set. 
                float reflectionAngleInDegrees = normalizedOffset * maxReflectionAngleInDegrees;

                // Convert angle from degrees to radians for trig calculations
                float reflectionAngleInRadians = reflectionAngleInDegrees * Mathf.Deg2Rad;

                // Calculate direction of reflection 
                Vector2 direction = new Vector3(Mathf.Sin(reflectionAngleInRadians), Mathf.Cos(reflectionAngleInRadians));

                // Apply velocity to ball based on reflection direction
                // Maintain speed but change direction and adjust applied force if desired with setting reflectingForce
                ballRb.AddForce(ballRb.velocity.magnitude * reflectingForce * direction, ForceMode.VelocityChange);

            }
        }
    }

    // When the Ball was dropped, it is reset to the initial position; after a 2 second wait it is pushed again
    public void SetNewBallsRigidBody()
    {
        ballRb = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();

        // Interpolate for better visual quality of physics rendering
        ballRb.interpolation = RigidbodyInterpolation.Interpolate;
        ballRb.velocity = Vector3.zero;
        ballRb.useGravity = false;
        Invoke(nameof(InitialDownPushToBall), 2f);
    }

    public void InitialDownPushToBall()
    {
        // Give initial push to the ball
        ballRb.useGravity = true;
        ballRb.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), -1f).normalized * 5f, ForceMode.Impulse);
    }
} 

