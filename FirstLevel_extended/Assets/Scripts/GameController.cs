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



    private void Start()
    {
        SpawnNewBall(); // Spawn a new Ball when new Game or Ball is dropped 
        InvokeRepeating("CheckForEndOfGame", 20, 3); // Check in intervals if all Bricks have been destroyed and then restart game
        gameOverScreen.GetComponent<Canvas>().enabled = false;
        quizManager.SetQuizVisibility(false);
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
        if (GameObject.Find("BrickLineC").transform.childCount == 0)
            SceneManager.LoadScene(0);
    }

    public void SpawnNewBall()
    {
        Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
        paddle.SetNewBallsRigidBody();
    }
}