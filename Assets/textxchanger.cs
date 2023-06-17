using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class textxchanger : MonoBehaviour
{
    Text txt;
    public GameObject inputField;
    public GameObject inputNameField;
    public GameObject submitButton;
    public GameObject giveUpButton;
    public GameObject startButton;
    public GameObject hintButton;
    public Text errorText;
    public Text correctText;
    public Text currentScoreText;
    public Text anagramText;
    public Text giveUpText;
    public Text lastMessageText;
    public Text enterNameText;
    public Text timerText;
    public Text hintMessageText;
    private List<string> targetWords = new List<string>();
    private int currentScore = 0;
    private string currentAnagram;
    private float timer = 0f;
    private bool timerRunning = false;
    private float totalTime = 0f;
    private string userName;
    private bool hintShown = false;
 
    void Start()
    {
        txt = gameObject.GetComponent<Text>();
        submitButton.SetActive(false);
        giveUpButton.SetActive(false);
        startButton.SetActive(true);
        inputField.SetActive(false);
        hintButton.SetActive(true);
        hintShown = false;
    }

    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    public void ShowHint()
    {
        if (hintShown)
        {
            return; 
        }
        hintShown = true;
        hintMessageText.text = "Try to rearrange the jumbled letters given on the screen\nto form a sensible word.\nYou have to solve 10 anagrams in total." +
                            "\nTry solve as many as possible before you give up\n" +
                            "ALL THE BEST!!";

        StartCoroutine(HideHintAfterDelay(20f));
    }

    private IEnumerator HideHintAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        hintMessageText.text = ""; 
        hintShown = false;
    }

    public void beginGame()
    {
        enterNameText.text = "";
        inputField.GetComponent<InputField>().text = "";
        userName = inputNameField.GetComponent<InputField>().text;
        inputNameField.SetActive(false);
        submitButton.SetActive(true);
        giveUpButton.SetActive(true);
        startButton.SetActive(false);
        inputField.SetActive(true);
        SetFirstAnagram();
        StartTimer();
        UpdateScoreText();
    }


    private void UpdateTimerText()
    {
        string timerString = FormatTimerString(timer);
        timerText.text = timerString;
    }

    private string FormatTimerString(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        int milliseconds = (int)((time * 1000) % 1000);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    private void StartTimer()
    {
        timerRunning = true;
        timer = 0f; 
    }

    private void StopTimer()
    {
        timerRunning = false;
        
    }

    public void submit()
    {
        string enteredText = inputField.GetComponent<InputField>().text.ToUpper();
          
        bool isCorrect = false;
        foreach (string word in targetWords)
        {
            if (enteredText.Contains(word))
            {
                isCorrect = true;
                break;
            }
        }
    
        if (isCorrect)
        {
            HandleCorrectAnswer("Correct!");
            StartCoroutine(ClearInputField());
        }
        else
        {
            ShowErrorMessage("Wrong Answer!");
        }
    }

    public void giveUp()
    {
        giveUpText.text = "GAME OVER!";
        lastMessageText.text = "You solved " + currentScore + "/10" + "! ";
        totalTime += timer;
        SaveTimerToFile("giveup", totalTime);
        ClearScreen();
        StopTimer();
    }

    private void winGame()
    {
        giveUpText.text = "CONGRATULATIONS!";
        giveUpText.color = Color.green;
        lastMessageText.text = "You solved " + currentScore + "/10" + "! ";
        totalTime += timer;
        SaveTimerToFile("wintime", totalTime);
        ClearScreen();
        StopTimer();
    }

    private void SaveTimerToFile(string stage, float time)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string folderPath = Path.Combine(desktopPath, userName);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string filePath;
        if(stage == "correct") {
            filePath = Path.Combine(folderPath, "level" + currentScore.ToString() + ".txt");
        } else { 
            currentScore ++;
            filePath = Path.Combine(folderPath, stage + currentScore.ToString() + ".txt");
              
        }
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(time.ToString());
        }
    }

    private void ClearScreen()
    {
        correctText.text = "";
        currentScoreText.text = "";
        anagramText.text = "";
        timerText.text = "";
        inputField.GetComponent<InputField>().text = "";
        inputField.SetActive(false);
        submitButton.SetActive(false);
        giveUpButton.SetActive(false);
        hintButton.SetActive(false);
    }

    private void ShowGiveUpMessage(string message)
    {
        giveUpText.text = message;
    }

    private IEnumerator ClearInputField()
    {
        yield return null;
        inputField.GetComponent<InputField>().text = "";
    }

    private void HandleCorrectAnswer(string message)
    {
        errorText.text = "";
        correctText.text = message;
        StartCoroutine(ClearCorrectMessage());
        currentScore++;
        UpdateScoreText();
        SaveTimerToFile("correct", timer);
        totalTime += timer;
        switch (currentScore)
        {
            case 1:
                SetSecondAnagram();
                break;
            case 2:
                SetThirdAnagram();
                break;
            case 3:
                SetFourthAnagram();
                break;
            case 4:
                SetFifthAnagram();
                break;
            case 5:
                SetSixthAnagram();
                break;
            case 6:
                SetSeventhAnagram();
                break;
            case 7:
                SetEigthAnagram();
                break;
            case 8:
                SetNigthAnagram();
                break;
            case 9:
                SetTenthAnagram();
                break;
            case 10:
                winGame();
                break;
        }
    }


    private void ShowErrorMessage(string message)
    {
        errorText.text = message;
        StartCoroutine(ClearErrorMessage());
    }

    private IEnumerator ClearErrorMessage()
    {
        yield return new WaitForSeconds(5f);
        errorText.text = "";
    }

    private IEnumerator ClearCorrectMessage()
    {
        yield return new WaitForSeconds(5f);
        correctText.text = "";
    }

    private void UpdateScoreText()
    {
        currentScoreText.text = "LEVEL: " + currentScore.ToString()+ "/10";
    }

    private void SetFirstAnagram()
    {
        targetWords = new List<string>() { "ACRE", "RACE", "CARE" };
        currentAnagram = "E  R  C  A";
        anagramText.text = currentAnagram;
    }

    private void SetSecondAnagram()
    {  
        targetWords = new List<string>() { "CRIME", "MICER","REMIC"};
        currentAnagram = "C  M  I  E  R";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetThirdAnagram()
    {  
        targetWords = new List<string>() { "SILENT", "LISTEN", "INLETS" };
        currentAnagram = "T  I  L  E  S  N";
        anagramText.text = currentAnagram;
        timer = 0f;
        
    }

    private void SetFourthAnagram()
    {
        targetWords = new List<string>() {"CREDIT", "DIRECT", "TRICED"};
        currentAnagram = "D  C  R  I  E  T";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

       private void SetFifthAnagram()
    {
        targetWords = new List<string>() {"SILENCE", "LICENSE"};
        currentAnagram = "E  I  S  N  C  L  E";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetSixthAnagram()
    {
        targetWords = new List<string>() {"CREATION", "REACTION"};
        currentAnagram = "A  C  O  E  T  R  N  I";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetSeventhAnagram()
    {
        
        targetWords = new List<string>() {"TAXIDERMY"};
        currentAnagram = "M  I  X  Y  R  A  T  E  D";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetEigthAnagram()
    {
        targetWords = new List<string>() {"XYLOPHONE"};
        currentAnagram = "O  Y  X  N  L  E  O  H P";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetNigthAnagram()
    {
        targetWords = new List<string>() {"FLABBERGASTED"};
        currentAnagram = "E  A  L  R  A  B  D  T  B  F  E  G  S";
        anagramText.text = currentAnagram;
        timer = 0f;
    }

    private void SetTenthAnagram()
    {
        targetWords = new List<string>() {"ONOMATOPOEIA"};
        currentAnagram = "P  I  N  O  M  A  O  E  O  T  O  A";
        anagramText.text = currentAnagram;
        timer = 0f;
    }
}
