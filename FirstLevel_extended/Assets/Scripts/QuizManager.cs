using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TMP_Text frageText;
    public Button antwort1Button;
    public Button antwort2Button;
    public Button antwort3Button;
    private GameController gameController;
    private bool canSelectAnswer = true;
    private Color normalColor;
    private int correctAnswer;
    private int falscheAntwort;

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();

        antwort1Button.onClick.AddListener(OnAntwort1Click);
        antwort2Button.onClick.AddListener(OnAntwort2Click);
        antwort3Button.onClick.AddListener(OnAntwort3Click);
        normalColor = antwort1Button.GetComponent<Image>().color;

        SetQuizVisibility(false);
    }

    public void SetQuizVisibility(bool visible)
    {
        frageText.gameObject.SetActive(visible);
        antwort1Button.gameObject.SetActive(visible);
        antwort2Button.gameObject.SetActive(visible);
        antwort3Button.gameObject.SetActive(visible);
    }

    public void StartQuiz()
    {
        int zahl1 = Random.Range(1, 51);
        int zahl2 = Random.Range(1, 51);

        int richtigeAntwort = zahl1 + zahl2;

        falscheAntwort = richtigeAntwort + Random.Range(-7, 8);

        while (richtigeAntwort == falscheAntwort)
        {
            falscheAntwort = richtigeAntwort + Random.Range(-7, 8);
        }

        frageText.text = $"{zahl1} + {zahl2}?";

        int randomOption = Random.Range(0, 3);

        if (randomOption == 0)
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort3Button.GetComponentInChildren<TMP_Text>().text = (falscheAntwort + Random.Range(-5, 6)).ToString();
            correctAnswer = richtigeAntwort;
        }
        else if (randomOption == 1)
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
            antwort3Button.GetComponentInChildren<TMP_Text>().text = (falscheAntwort + Random.Range(-5, 6)).ToString();
            correctAnswer = richtigeAntwort;
        }
        else
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = (falscheAntwort + Random.Range(-5, 6)).ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort3Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
            correctAnswer = richtigeAntwort;
        }

        SetQuizVisibility(true);

        Time.timeScale = 0;
        canSelectAnswer = true;
    }

    private void EndQuiz()
    {
        SetQuizVisibility(false);
        Time.timeScale = 1;
    }

    private void OnAntwort1Click()
    {
        if (canSelectAnswer)
        {
            if (antwort1Button.GetComponentInChildren<TMP_Text>().text == correctAnswer.ToString())
            {
                EndQuiz();
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoints(1);
                }
            }
            else
            {
                gameController.LooseALife();
                EndQuiz();
            }
        }
    }

    private void OnAntwort2Click()
    {
        if (canSelectAnswer)
        {
            if (antwort2Button.GetComponentInChildren<TMP_Text>().text == correctAnswer.ToString())
            {
                EndQuiz();
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoints(1);
                }
            }
            else
            {
                gameController.LooseALife();
                Debug.Log("Leben abgezogen");
                EndQuiz();
            }
        }
    }

    private void OnAntwort3Click()
    {
        if (canSelectAnswer)
        {
            if (antwort3Button.GetComponentInChildren<TMP_Text>().text == correctAnswer.ToString())
            {
                EndQuiz();
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoints(1);
                }
            }
            else
            {
                gameController.LooseALife();
                Debug.Log("Leben abgezogen");
                EndQuiz();
            }
        }
    }

    void Update()
    {
        if (canSelectAnswer)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                antwort1Button.Select();
                antwort1Button.GetComponent<Image>().color = Color.yellow;
                antwort2Button.GetComponent<Image>().color = normalColor;
                antwort3Button.GetComponent<Image>().color = normalColor;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (antwort1Button.GetComponent<Image>().color == Color.yellow)
                {
                    antwort2Button.Select();
                    antwort1Button.GetComponent<Image>().color = normalColor;
                    antwort2Button.GetComponent<Image>().color = Color.yellow;
                    antwort3Button.GetComponent<Image>().color = normalColor;
                }
                else if (antwort2Button.GetComponent<Image>().color == Color.yellow)
                {
                    antwort3Button.Select();
                    antwort1Button.GetComponent<Image>().color = normalColor;
                    antwort2Button.GetComponent<Image>().color = normalColor;
                    antwort3Button.GetComponent<Image>().color = Color.yellow;
                }
                // Add any additional logic here if needed for handling more buttons
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                if (UnityEngine.EventSystems.EventSystem.current != null &&
                    UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject != null)
                {
                    UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }
}
