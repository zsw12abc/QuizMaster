using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    // https://www.dyhzdl.cn/k/doc/577964b4541810a6f524ccbff121dd36a32dc484.html
    [TextArea(2, 6)] [SerializeField] string question = "Enter new question text here";
    [SerializeField] string[] answers = new string[4];

    [SerializeField] private int correctAnswerIndex;

    public string GetQuestion()
    {
        return question;
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }
    
    public string GetAnswer(int index)
    {
        return answers[index];
    }
}