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
    public bool isVisible { get; private set; } = false; 

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();

        antwort1Button.onClick.AddListener(OnAntwort1Click);
        antwort2Button.onClick.AddListener(OnAntwort2Click);
        antwort3Button.onClick.AddListener(OnAntwort3Click);
        normalColor = antwort1Button.GetComponent<Image>().color;

        SetQuizVisibility(false);
    }

    public enum MathOperation
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
    }

    public void SetQuizVisibility(bool visible)
    {
        isVisible = visible;
        frageText.gameObject.SetActive(visible);
        antwort1Button.gameObject.SetActive(visible);
        antwort2Button.gameObject.SetActive(visible);
        antwort3Button.gameObject.SetActive(visible);
    }

    public void StartQuiz(MathOperation mathOperation)
    {
        int zahl1, zahl2;

        if (mathOperation == MathOperation.Multiplication)
        {
            zahl1 = Random.Range(2, 11); 
            zahl2 = Random.Range(2, 11);
        }
        else if (mathOperation == MathOperation.Division)
        {
            do
            {
                zahl1 = Random.Range(2, 101);  
                zahl2 = Random.Range(2, 21);   
            } while (zahl1 % zahl2 != 0);  
        }
        else
        {
            zahl1 = Random.Range(2, 101); 
            zahl2 = Random.Range(2, 101);
        }

        int richtigeAntwort;

        if (mathOperation == MathOperation.Addition)
        {
            richtigeAntwort = zahl1 + zahl2;
        }
        else if (mathOperation == MathOperation.Subtraction)
        {
            if (zahl1 >= zahl2)
            {
                richtigeAntwort = zahl1 - zahl2;
            }
            else
            {
                richtigeAntwort = zahl2 - zahl1;
                int temp = zahl1;
                zahl1 = zahl2;
                zahl2 = temp;
            }
        }
        else if (mathOperation == MathOperation.Multiplication)
        {
            richtigeAntwort = zahl1 * zahl2;
        }
        else if (mathOperation == MathOperation.Division)
        {
            richtigeAntwort = zahl1 / zahl2;
        }
        else
        {
            richtigeAntwort = 0;
        }

        // Ensure that falscheAntwort is not negative
        falscheAntwort = Mathf.Max(0, richtigeAntwort + Random.Range(-7, 8));

        while (richtigeAntwort == falscheAntwort)
        {
            falscheAntwort = Mathf.Max(0, richtigeAntwort + Random.Range(-7, 8));
        }

        frageText.text = (mathOperation == MathOperation.Addition) ?
            $"{zahl1} + {zahl2}?" :
            (mathOperation == MathOperation.Subtraction) ?
            $"{zahl1} - {zahl2}?" :
            (mathOperation == MathOperation.Multiplication) ?
            $"{zahl1} * {zahl2}?" :
            $"{zahl1} : {zahl2}?";

        int randomOption = Random.Range(0, 3);

        // alle 3 Antworten eindeutig sind
        do
        {
            correctAnswer = richtigeAntwort;
            antwort1Button.GetComponentInChildren<TMP_Text>().text = correctAnswer.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort3Button.GetComponentInChildren<TMP_Text>().text = Mathf.Max(0, falscheAntwort + richtigeAntwort + Random.Range(-5, 6)).ToString();
        } while (antwort2Button.GetComponentInChildren<TMP_Text>().text == antwort3Button.GetComponentInChildren<TMP_Text>().text ||
                 antwort1Button.GetComponentInChildren<TMP_Text>().text == antwort2Button.GetComponentInChildren<TMP_Text>().text ||
                 antwort1Button.GetComponentInChildren<TMP_Text>().text == antwort3Button.GetComponentInChildren<TMP_Text>().text);

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
