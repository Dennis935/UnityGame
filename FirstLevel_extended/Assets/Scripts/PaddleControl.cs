using UnityEngine;

public class PaddleControl : MonoBehaviour
{
    [SerializeField]
    private float paddleSpeed = 2f; 

    [SerializeField]
    private float reflectingForce = 1f; 

    [SerializeField]
    private float maxBallVelocity = 15f; 

    [SerializeField]
    private bool useConstantBallSpeed = true;  

    [SerializeField]
    private float constantBallSpeed = 12f;

    [SerializeField]
    private Rigidbody ballRb; 

    [SerializeField]
    private float maxReflectionAngleInDegrees = 75f;
    [SerializeField]
    public GameObject topBorder; 
    [SerializeField]
    public GameObject leftBorder;
    [SerializeField]
    public GameObject rightBorder; 
    [SerializeField]
    public GameObject bottomBorder; 

    private float halfWidth; 
    private float leftLimit; 
    private float rightLimit; 

    private void Start ()
    {
        halfWidth = GetComponent<Collider>().bounds.extents.x;

        leftLimit = leftBorder.GetComponent<Collider>().bounds.max.x + halfWidth;
        rightLimit = rightBorder.GetComponent<Collider>().bounds.min.x - halfWidth;
        GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate; //Allows a more smooth movement of the paddle
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;

       
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            float horizontal = Input.GetAxis("Horizontal");
            Vector3 newPosition = transform.position + new Vector3(horizontal * paddleSpeed * Time.deltaTime, 0, 0);

            newPosition.x = Mathf.Clamp(newPosition.x, leftLimit, rightLimit);

            transform.position = newPosition;
        }

        if (ballRb !=null && ballRb.transform.position.y > 33)
        {
            if (ballRb.velocity.magnitude > maxBallVelocity)
            {
                ballRb.velocity = ballRb.velocity.normalized * maxBallVelocity;
            }
        }

        if (useConstantBallSpeed)
        {
            ballRb.velocity = constantBallSpeed * (ballRb.velocity.normalized);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody ballRb = collision.gameObject.GetComponent<Rigidbody>();

            if (ballRb)
            {
                Vector3 hitPoint = collision.GetContact(0).point;

                Vector3 paddleCenter = new Vector3(transform.position.x, transform.position.y);

                float offset = hitPoint.x - paddleCenter.x;

                float halfPaddleWidth = GetComponent<Collider>().bounds.size.x / 2;

                float normalizedOffset = offset / halfPaddleWidth;

                float reflectionAngleInDegrees = normalizedOffset * maxReflectionAngleInDegrees;

                float reflectionAngleInRadians = reflectionAngleInDegrees * Mathf.Deg2Rad;

                Vector2 direction = new Vector3(Mathf.Sin(reflectionAngleInRadians), Mathf.Cos(reflectionAngleInRadians));

                ballRb.AddForce(ballRb.velocity.magnitude * reflectingForce * direction, ForceMode.VelocityChange);

            }
        }
    }

    public void SetNewBallsRigidBody()
    {
        ballRb = GameObject.FindGameObjectWithTag("Ball").GetComponent<Rigidbody>();

        ballRb.interpolation = RigidbodyInterpolation.Interpolate;
        ballRb.velocity = Vector3.zero;
        ballRb.useGravity = false;
        Invoke(nameof(InitialDownPushToBall), 2f);
    }

    public void InitialDownPushToBall()
    {
        ballRb.useGravity = true;
        ballRb.AddForce(new Vector2(Random.Range(-0.5f, 0.5f), -1f).normalized * 5f, ForceMode.Impulse);
    }
} 

