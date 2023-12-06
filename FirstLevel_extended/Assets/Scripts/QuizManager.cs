using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TMP_Text frageText;
    public Button antwort1Button;
    public Button antwort2Button;
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
        normalColor = antwort1Button.GetComponent<Image>().color;

        SetQuizVisibility(false);
    }

    public void SetQuizVisibility(bool visible)
    {
        frageText.gameObject.SetActive(visible);
        antwort1Button.gameObject.SetActive(visible);
        antwort2Button.gameObject.SetActive(visible);
    }

    public void StartQuiz()
    {
        int zahl1 = Random.Range(1, 51);
        int zahl2 = Random.Range(1, 51);

        int richtigeAntwort = zahl1 + zahl2;

        falscheAntwort = richtigeAntwort + Random.Range(-5, 6);

        while (richtigeAntwort == falscheAntwort)
        {
            falscheAntwort = richtigeAntwort + Random.Range(-5, 6);
        }

        frageText.text = $"Was ist {zahl1} + {zahl2}?";

        if (Random.Range(0, 2) == 0)
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            correctAnswer = richtigeAntwort;
        }
        else
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
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

    void Update()
    {
        if (canSelectAnswer)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                antwort1Button.Select();
                antwort1Button.GetComponent<Image>().color = Color.yellow;
                antwort2Button.GetComponent<Image>().color = normalColor;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                antwort2Button.Select();
                antwort2Button.GetComponent<Image>().color = Color.yellow;
                antwort1Button.GetComponent<Image>().color = normalColor;
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
