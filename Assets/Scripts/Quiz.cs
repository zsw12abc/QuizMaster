using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
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

    // Start is called before the first frame update
    void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetNextQuestion();
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

    private void DisplayQuestion()
    {
        _questionText.text = currentQuestion.GetQuestion();
        correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);

        SetButtonState(false);
        timer.CancelTimer();
    }

    private void DisplayAnswer(int index)
    {
        var buttonImage = answerButtons[index].GetComponent<Image>();
        if (index == currentQuestion.GetCorrectAnswerIndex())
        {
            _questionText.text = "Correct!";
            buttonImage.sprite = correctAnswerSprite;
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
        }
    }

    void GetRandomQuestion()
    {
        int randomIndex = UnityEngine.Random.Range(0, questions.Count);
        currentQuestion = questions[randomIndex];
        if (questions.Contains(currentQuestion))
        {
            questions.Remove(currentQuestion);
        }
    }

    void SetDefaultButtonSprites()
    {
        foreach (var t in answerButtons)
        {
            t.GetComponent<Image>().sprite = defaultAnswerSprite;
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
}