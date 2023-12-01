using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
public class Quiz : MonoBehaviour{
    
    [Header("Questions")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] List<QuestionSO> questions = new List<QuestionSO>();
    QuestionSO currentQuestion;

    [Header("Answers")]
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    bool hasAnsweredEarly = true;

    [Header("Buttons")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] Image timerImage;
    Timer timer;

    [Header("Scoring")]
    [SerializeField] TextMeshProUGUI scoreText;
    ScoreKeeper scoreKeeper;

    [Header("Slider")]
    [SerializeField] Slider progressBar;

    public bool  isComplete;


    

    void Awake(){
        
        timer = FindObjectOfType<Timer>();
        scoreKeeper = FindAnyObjectByType<ScoreKeeper>();
        progressBar.maxValue = questions.Count;
        progressBar.value = 0;
    }
    
    void Update(){

        timerImage.fillAmount = timer.fillFraction;

        if (progressBar.value == progressBar.maxValue){

            isComplete = true;
            return;
        }
        
        if(timer.loadNextQuestion){
            
            GetNextQuestion();
            timer.loadNextQuestion = false;
        }

        else if(!hasAnsweredEarly && !timer.isAnsweringQuestion){
            
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }
    
    
    public void OnAnswerSelected(int index){
        
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timer.CancelTimer();
        scoreText.text = "Score: %" + scoreKeeper.CalculateScore();
    }


    void GetNextQuestion(){
        
        if(questions.Count>0){
            
            GetRandomQuestion();
             DisplayQuestion();
            SetButtonState(true);
            SetDefaultButtonSprites();
            progressBar.value++;
            scoreKeeper.IncremenetQuestionSeen();
        }
    }   


    void DisplayAnswer(int index){
        
        Image buttonImage;
        if(index == currentQuestion.GetCorrectAnswerIndex()){
            
            questionText.text= "Correct";
            buttonImage = answerButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncremenetCorrectAnswers();
            
        }

        else{

            correctAnswerIndex = currentQuestion.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestion.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was;\n"+ correctAnswer;
            buttonImage = answerButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            
        }
    }


    void DisplayQuestion(){

        questionText.text = currentQuestion.GetQuestion();

        for (int i = 0; i<answerButtons.Length; i++){

            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestion.GetAnswer(i);
        }
    }


    void SetButtonState(bool state){
        
        for(int i =0; i < answerButtons.Length ; i++){
            
            Button button = answerButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }


    void SetDefaultButtonSprites(){
        for(int i = 0; i < answerButtons.Length;i++){
           
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }


    void GetRandomQuestion(){
       
        int index = Random.Range(0,questions.Count);
        currentQuestion = questions[index];
        
        if(questions.Contains(currentQuestion)){
           
            questions.Remove(currentQuestion);
        }
    }
}