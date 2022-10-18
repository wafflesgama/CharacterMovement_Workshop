using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Animator scoreAnim;
    public TextMeshProUGUI scoreText;
    void Start()
    {
        instance = this;
        scoreText.text = "0 - 0";
    }

    private void OnDestroy()
    {
        instance= null;
    }

   public void UpdateScore(int score1,int score2)
    {
        scoreText.text = $"{score1} - {score2}";
        scoreAnim.SetTrigger("Pop");
    }
}
