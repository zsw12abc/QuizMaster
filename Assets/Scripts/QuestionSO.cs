using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Quiz Question", fileName = "New Question")]
public class QuestionSO : ScriptableObject
{
    // https://www.dyhzdl.cn/k/doc/577964b4541810a6f524ccbff121dd36a32dc484.html
    [TextArea(2, 6)]
    [SerializeField] string question = "Enter new question text here";
    
}