using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;


public class EndScreen : MonoBehaviour{
    
    [SerializeField] TextMeshProUGUI finalScoreText;
    ScoreKeeper scoreKeeper;


    void Awake(){
        
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
    }


    public void ShowFinalScore(){

        finalScoreText.text = "Congratulations!\nYou Got a score of %"+ scoreKeeper.CalculateScore();
    }
}
