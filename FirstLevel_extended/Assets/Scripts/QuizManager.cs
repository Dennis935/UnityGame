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


    public void Start()
    {
        gameController = FindObjectOfType<GameController>();
        antwort1Button.onClick.AddListener(OnAntwort1Click);
        antwort2Button.onClick.AddListener(OnAntwort2Click);
        normalColor = antwort1Button.GetComponent<Image>().color;

        StartQuiz();
    }

    public void SetQuizVisibility(bool visible)
    {
        frageText.gameObject.SetActive(visible);
        antwort1Button.gameObject.SetActive(visible);
        antwort2Button.gameObject.SetActive(visible);
    }

    public void StartQuiz()
    {
        // Generieren Sie zuf�llige Zahlen f�r die Additionsaufgabe
        int zahl1 = Random.Range(1, 101);  // Zufallszahl zwischen 1 und 100
        int zahl2 = Random.Range(1, 101);

        // Berechnen Sie die richtige Antwort
        int richtigeAntwort = zahl1 + zahl2;

        // Generieren Sie eine falsche Antwort (zuf�llige Zahl zwischen 1 und 200, um sicherzustellen, dass sie falsch ist)
        int falscheAntwort = Random.Range(1, 201);

        // Zuf�llig entscheiden, welche der beiden Antworten die richtige ist
        bool istAntwort1Richtig = Random.Range(0, 2) == 0;

        // Setzen Sie den Text und die Antworten in der UI
        frageText.text = $"Was ist {zahl1} + {zahl2}?";

        if (istAntwort1Richtig)
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
        }
        else
        {
            antwort1Button.GetComponentInChildren<TMP_Text>().text = falscheAntwort.ToString();
            antwort2Button.GetComponentInChildren<TMP_Text>().text = richtigeAntwort.ToString();
        }

        // Zeigen Sie die UI-Elemente an
        SetQuizVisibility(true);

        // Pausieren Sie das Spiel w�hrend des Quiz
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
            EndQuiz();
            // Hier k�nnen Sie �berpr�fen, ob die Antwort korrekt ist
            // Zum Beispiel: if (antwort1Button.GetComponentInChildren<TMP_Text>().text == "Die richtige Antwort")
        }
    }

    private void OnAntwort2Click()
    {
        if (canSelectAnswer)
        {
            EndQuiz();
            // Hier k�nnen Sie �berpr�fen, ob die Antwort korrekt ist
            // Zum Beispiel: if (antwort2Button.GetComponentInChildren<TMP_Text>().text == "Die richtige Antwort")
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
                antwort1Button.GetComponent<Image>().color = Color.yellow; // Setze die Farbe der ausgew�hlten Antwort
                antwort2Button.GetComponent<Image>().color = normalColor; // Setze die Farbe der anderen Antwort auf die normale Farbe
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                antwort2Button.Select();
                antwort2Button.GetComponent<Image>().color = Color.yellow; // Setze die Farbe der ausgew�hlten Antwort
                antwort1Button.GetComponent<Image>().color = normalColor; // Setze die Farbe der anderen Antwort auf die normale Farbe
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Space))
            {
                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
            }

        }
    }
}

