using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int lives = 3; 
    [SerializeField]
    private TMP_Text livesTextInfo;  
    [SerializeField]
    private GameObject ballPrefab; 
    [SerializeField]
    private Vector3 ballStartPosition; 
    [SerializeField]
    private Vector3 paddleStartPosition; 
    [SerializeField]
    private PaddleControl paddle; 
    [SerializeField]
    private Canvas gameOverScreen; 
    [SerializeField]
    private AudioSource audiosource;
    [SerializeField]
    private QuizManager quizManager;
    [SerializeField]
    private TMP_Text LevelCompleteText;


    private void Start()
    {
        SetMathBricks();
        SpawnNewBall(); 
        InvokeRepeating("CheckForEndGame", 20, 3); 
        gameOverScreen.GetComponent<Canvas>().enabled = false;
        LevelCompleteText.enabled = false;
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
            SavePlayerScore();
            gameOverScreen.GetComponent<Canvas>().enabled = true;
            //Time.timeScale = 0;
            audiosource.Stop();
            Invoke("LoadHighscoreSceneWithDelay", 3f);
        }
    }

    private void LoadHighscoreSceneWithDelay()
    {
        SceneManager.LoadScene("HighscoreScene");
    }

    private void SavePlayerScore()
    {
        string playerName = PlayerPrefs.GetString("PlayerName", "");

        PlayerPrefs.SetString("PlayerName", playerName);

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            PlayerPrefs.SetInt("Score", scoreManager.GetScore());
        }

        PlayerPrefs.Save();
    }



    private void SetMathBricks()
    {
        Brick[] bricks = FindObjectsOfType<Brick>();
        foreach (Brick brick in bricks)
        {
            brick.SetIsMathBrick(UnityEngine.Random.Range(0, 5) == 0);
        }
    }

    public void LooseALife()
    {
        lives--;
        paddle.GetComponent<MeshRenderer>().enabled = false;

        if (lives > 0)
        {
            ReSetBallAndPaddle(); 
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

        Debug.Log("COUNT" + count);

        return count;
    }

    public void CheckForEndGame()
    {
        GameObject[] brickRows = GameObject.FindGameObjectsWithTag("BrickRow");

        bool allEmpty = true;

        foreach (GameObject brickRow in brickRows)
        {
            if (brickRow.transform.childCount != 0)
            {
                Debug.Log("not empty");
                allEmpty = false;
                break;
            }
        }

        if (allEmpty)
        {
            SavePlayerScore();
            LevelCompleteText.enabled = true;

            Invoke("LoadNextSceneWithDelay", 3f);

        }
    }

    private void LoadNextSceneWithDelay()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;

       
        if (activeScene < 5)  
        {
            SceneManager.LoadScene(activeScene + 1);
        }
        else
        {
            SceneManager.LoadScene("HighscoreScene");
        }
    }


    public void SpawnNewBall()
    {
        Instantiate(ballPrefab, ballStartPosition, Quaternion.identity);
        paddle.SetNewBallsRigidBody();
    }

}