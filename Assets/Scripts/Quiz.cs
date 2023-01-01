using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Quiz : MonoBehaviour
{
    [Header(("Questions"))] [SerializeField]
    TextMeshProUGUI _questionText;

    [SerializeField] private List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")] [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly;

    [Header("Button Colors")] [SerializeField]
    Sprite defaultAnswerSprite;

    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")] [SerializeField] Image timerImage;
    Timer timer;


    [Header("Scoring")] [SerializeField] TextMeshProUGUI scoreText;

    ScoreKeeper scoreKeeper;

    [Header("ProgressBar")] [SerializeField]
    Slider progressBar;

    public bool isComplete;

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadNextQuestion)
        {
            hasAnsweredEarly = false;
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }
        else if (!hasAnsweredEarly && !timer.isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = $"Score: {scoreKeeper.CalculateScore()}%";

        if (progressBar.value == progressBar.maxValue)
        {
            isComplete = true;
        }
    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            _questionText.text = "Correct!";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            _questionText.text = $"Wrong! The correct answer is {currentQuestion.GetAnswer(currentQuestion.GetCorrectAnswerIndex())}";
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    void GetNextQuestion()
    {
        if (questions.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionSeen();
        }
    }

    void GetRandomQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    private void DisplayQuestion()
    {
        _questionText.text = currentQuestion.GetQuestion();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }

    void SetButtonState(bool state)
    {
        foreach (var t in answerButtons)
        {
            var button = t.GetComponent<Button>();
            button.interactable = state;
        }
    }

    void SetDefaultButtonSprites()
    {
        foreach (var t in answerButtons)
        {
            t.GetComponent<Image>().sprite = defaultAnswerSprite;
        }
    }
}