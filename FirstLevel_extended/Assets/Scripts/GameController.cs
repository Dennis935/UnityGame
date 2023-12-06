using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives = 3; // The amount of lives (balls) we grant the player
    [SerializeField]
    private TMP_Text livesTextInfo;  // The score lable that shows the remaining lives
    [SerializeField]
    private GameObject ballPrefab; // The ball prefab that is to instantiate 
    [SerializeField]
    private Vector3 ballStartPosition; // The startposition of the ball
    [SerializeField]
    private Vector3 paddleStartPosition; // The startposition of the paddle
    [SerializeField]
    private PaddleControl paddle; // the paddle
    [SerializeField]
    private Canvas gameOverScreen; // the UI Canvas shown when the last ball is dropped 
    [SerializeField]
    private AudioSource audiosource;
    [SerializeField]
    private QuizManager quizManager;
    /*[SerializeField]
    private TMP_Text levelCompleteText;*/


    private void Start()
    {
        SetMathBricks();
        SpawnNewBall(); // Spawn a new Ball when new Game or Ball is dropped 
        InvokeRepeating("CheckForEndGame", 20, 3); // Check in intervals if all Bricks have been destroyed and then restart game
        gameOverScreen.GetComponent<Canvas>().enabled = false;
        quizManager.SetQuizVisibility(false);
        //levelCompleteText.enabled = false;
    }

    private void Update()
    {
        if (livesTextInfo != null)
        {
            livesTextInfo.text = "Lives: " + lives.ToString();
        }

        if (lives <= 0)
        {
            gameOverScreen.GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
            audiosource.Stop();
            SceneManager.LoadScene("HighscoreScene");
        }
    }

    private void SetMathBricks()
    {
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (Brick brick in bricks)
        {
            brick.SetIsMathBrick(UnityEngine.Random.Range(0, 5) == 0); // Zufällige Zuweisung
        }
    }

    public void LooseALife()
    {
        lives--;
        paddle.GetComponent<MeshRenderer>().enabled = false;

        if (lives > 0)
        {
            ReSetBallAndPaddle(); // Reset Ball and Paddle after Ball was dropped but still lives left
        }
    }

    public void ReSetBallAndPaddle()
    {
        if (GameObject.FindGameObjectWithTag("Ball"))
        {
            GameObject.FindGameObjectWithTag("Ball").transform.position = ballStartPosition;
        }
        else
        {
            Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
        }
        paddle.transform.position = paddleStartPosition;
        paddle.GetComponent<MeshRenderer>().enabled = true;
        paddle.SetNewBallsRigidBody();
    }

    public void CheckForEndGame()
    {
        GameObject brickContainer = GameObject.Find("Brick"); // Assuming Brick is the parent object
        int totalBricks = CountBricks(brickContainer);

        if (totalBricks == 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private int CountBricks(GameObject container)
    {
        int count = 0;

        foreach (Transform row in container.transform)
        {
            foreach (Transform brick in row)
            {
                if (brick.CompareTag("Brick"))
                {
                    count++;
                }
            }
        }

        Debug.Log("COUNT"+count);

        return count;
    }


    public void SpawnNewBall()
    {
        Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
        paddle.SetNewBallsRigidBody();
    }
}