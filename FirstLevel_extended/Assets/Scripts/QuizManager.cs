using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    public TMP_Text frageText;
    public Button antwort1Button;
    public Button antwort2Button;
    private GameController gameController;
    private bool canSelectAnswer = true; // To prevent selecting answers while transitioning
    private Color normalColor; // Die normale Farbe der Antwortbuttons
    private int correctAnswer; // Variable, um die richtige Antwort zu speichern

    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
        antwort1Button.onClick.AddListener(OnAntwort1Click);
        antwort2Button.onClick.AddListener(OnAntwort2Click);
        normalColor = antwort1Button.GetComponent<Image>().color;

        // Setze die UI-Elemente auf unsichtbar
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
        // Generieren Sie zufällige Zahlen für die Additionsaufgabe
        int zahl1 = Random.Range(1, 101);  // Zufallszahl zwischen 1 und 100
        int zahl2 = Random.Range(1, 101);

        // Berechnen Sie die richtige Antwort
        int richtigeAntwort = zahl1 + zahl2;

        // Generieren Sie eine falsche Antwort (zufällige Zahl zwischen 1 und 200, um sicherzustellen, dass sie falsch ist)
        int falscheAntwort = Random.Range(1, 201);

        // Zufällig entscheiden, welche der beiden Antworten die richtige ist
        bool istAntwort1Richtig = Random.Range(0, 2) == 0;

        // Setzen Sie den Text und die Antworten in der UI
        frageText.text = $"Was ist {zahl1} + {zahl2}?";

        if (istAntwort1Richtig)
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

        // Zeigen Sie die UI-Elemente an
        SetQuizVisibility(true);

        // Pausieren Sie das Spiel während des Quiz
        Time.timeScale = 0;
        canSelectAnswer = true; // Enable answer selection

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
            // Überprüfe, ob die Antwort korrekt ist
            if (antwort1Button.GetComponentInChildren<TMP_Text>().text == correctAnswer.ToString())
            {
                EndQuiz();
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoints(1); // Beispiel: 1 Punkt für jede richtige Antwort
                }
            }
            else
            {
                // Hier verlierst du ein Leben, wenn die Antwort falsch ist
                gameController.LooseALife();
                EndQuiz();

            }
        }
    }

    private void OnAntwort2Click()
    {
        if (canSelectAnswer)
        {
            // Überprüfe, ob die Antwort korrekt ist
            if (antwort2Button.GetComponentInChildren<TMP_Text>().text == correctAnswer.ToString())
            {
                EndQuiz();
                ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
                if (scoreManager != null)
                {
                    scoreManager.AddPoints(1); // Beispiel: 1 Punkt für jede richtige Antwort
                }
            }
            else
            {
                gameController.LooseALife();
                EndQuiz();
            }
        }
    }

    void Update()
    {
        if (canSelectAnswer)
        {
            // Navigate through answers using W and S keys
            if (Input.GetKeyDown(KeyCode.W))
            {
                antwort1Button.Select();
                antwort1Button.GetComponent<Image>().color = Color.yellow; // Setze die Farbe der ausgewählten Antwort
                antwort2Button.GetComponent<Image>().color = normalColor; // Setze die Farbe der anderen Antwort auf die normale Farbe
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                antwort2Button.Select();
                antwort2Button.GetComponent<Image>().color = Color.yellow; // Setze die Farbe der ausgewählten Antwort
                antwort1Button.GetComponent<Image>().color = normalColor; // Setze die Farbe der anderen Antwort auf die normale Farbe
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }

        }
    }
}

