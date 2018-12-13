using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Nums : MonoBehaviour {

    [Header("Configuration")]
    [SerializeField]private Operation[] operations;
    public int CountOperation {
        get {
            return operations != null ? operations.Length : 0;
        }
    }
    public int timeForAnswer = 12;
    private int maxDifficult = 20;
    [Space(10)]
    [SerializeField] private int startDifficult = 10;
    private int currentDifficult = 10;
    private int stepDifficult;
    public int Difficult {
        get {
            return currentDifficult;
        }
        private set {
            if (currentDifficult < maxDifficult)
                currentDifficult = value;

            if (currentDifficult > maxDifficult)
                currentDifficult = maxDifficult;
        }
    }
    public TypeAnswer typeAnswer = TypeAnswer.Keyboard;
    public int Answer {
        get {
            return currentAnswer;
        }
    }
    [SerializeField] private RandomNum random;

    [Header("Text on game")]
    [SerializeField] private Text AllTimeText;
    [SerializeField] private Text ScoreText;

    [Header("Table")]
    [SerializeField] private Text solutionText;
    [SerializeField] private Image table;
    [SerializeField] private Color rightColor;
    [SerializeField] private Color wrongColor;
    private Color defColor;

    [Header("Time")]
    [SerializeField] private RectTransform lineTimer;
    [SerializeField] private RectTransform pixelWidth;
    private float currentTime;
    private float width;

    [Header("Solved")]
    private int currentWrongSolved = 0;
    [SerializeField] private int maxWrong = 3;


    private Operation currentOperation;
    private string currentSolution;
    private int currentAnswer;
    private int checkAnswer; // 1 - right 0 - hz -1 - wrong
    
    private string playerInput;

    private bool canInput;
    private bool isGame;

    [Header("Delays")]
    [SerializeField] private float delayBetweenSymbol;
    [SerializeField] private float timeRightAnswer;
    [SerializeField] private float timeWrongAnswer;
    
    [Header("Events")]
    public UnityEvent NextEvent;
    [Space(15)]
    public UnityEvent EndGameEvent;

    //Statistic
    private int currentSolved = 0;
    private float currentAllTime = 0;
    private float currentMindTime = 0;
    private float avgTimeSolved;
    private int score;

    [Header("Results")]
    [SerializeField] private Text currentSolvedText;
    [SerializeField] private Text currentAllTimeText;
    [SerializeField] private Text avgTimeSolvedText;
    [SerializeField] private Text scoreText;

    private void Start() {
        defColor = table.color;

        float t = 45f / pixelWidth.sizeDelta.x;
        width = Camera.main.ViewportToWorldPoint(new Vector3(1 - t, 0)).x - Camera.main.ViewportToWorldPoint(new Vector3(t, 0)).x;
    }

    public void StartGame() {
        if (operations.Length > 0) {
            currentSolved = 0;
            currentWrongSolved = 0;
            currentMindTime = 0;

            Difficult = startDifficult;
            stepDifficult = Mathf.CeilToInt((maxDifficult - startDifficult) / 20f);
            if (stepDifficult == 0) stepDifficult = 1;

            ScoreText.text = "0";

            isGame = true;
            StartCoroutine(AllTime());
            StartCoroutine(LineTimer());

            Next();
        }
    }

    #region Timer
    private int lastTime;
    private int second;
    private string helpChar;
    private IEnumerator AllTime() {
        currentAllTime = 0;
        lastTime = 0;
        while (isGame) {
            if (!isGame) yield break;

            if (currentAllTime > lastTime) {
                lastTime = (int)currentAllTime;
                second = lastTime % 60;
                if (second < 10) helpChar = "0";
                else helpChar = "";
                AllTimeText.text = (int)(lastTime / 60f) + ":" + helpChar + second;
            }

            currentAllTime += Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    private IEnumerator LineTimer() {
        while (isGame) {
            lineTimer.position = new Vector2(Mathf.Lerp(0, -width, currentTime / (timeForAnswer * 1.0f)), lineTimer.position.y);
            yield return null;
        }
    }

    private int boolAnswer;
    private void Next() {

        if (currentWrongSolved == maxWrong) {
            EndGame();
            if (EndGameEvent != null) {
                EndGameEvent.Invoke();
            }
            return;
        }
        
        canInput = true;
        checkAnswer = 0;
        
        currentOperation = operations[Random.Range(0, operations.Length)];
        currentSolution = currentOperation.GetSolution(ref currentAnswer);
        solutionText.text = currentSolution;

        if (TypeAnswer.Test == typeAnswer)
            NextEvent.Invoke();
        if (TypeAnswer.TrueFalse == typeAnswer) {
            boolAnswer = currentAnswer + (Random.Range(0, 2) == 0 ? random.GetRandomNumber(-5, 6) : 0);
            solutionText.text = currentSolution + boolAnswer;
        }

        StartCoroutine(WaitPlayerInput());
    }
    private IEnumerator WaitPlayerInput() {
        currentTime = 0;
        playerInput = "?";
        while (currentTime < timeForAnswer) {
            if (checkAnswer == 1) {

                StartCoroutine(RightAnswer());

                yield break;
            } else if (checkAnswer == -1) {

                StartCoroutine(WrongAnswer());

                yield break;
            }
            yield return null;
            currentTime += Time.deltaTime;
            currentMindTime += Time.deltaTime;
        }
        StartCoroutine(WrongAnswer());
    }
    private IEnumerator RightAnswer() {
        canInput = false;
        table.color = rightColor;
        float currTime = 0;

        //First delay
        currTime = 0;
        while (currTime < timeRightAnswer) {
            yield return null;
            currTime += Time.deltaTime;
        }
        
        //Delete answer delay
        while (playerInput.Length > 0) {
            currTime = 0;
            while (currTime < delayBetweenSymbol) {
                yield return null;
                currTime += Time.deltaTime;
            }

            playerInput = playerInput.Remove(playerInput.Length - 1);
            solutionText.text = currentSolution + playerInput;

            yield return null;
        }

        //Second delay
        currTime = 0;
        while (currTime < timeRightAnswer / 2) {
            yield return null;
            currTime += Time.deltaTime;
        }



        canInput = true;
        table.color = defColor;
        currentSolved++;
        Difficult += stepDifficult;

        ScoreText.text = currentSolved.ToString();

        lineTimer.localPosition = new Vector2(0, lineTimer.localPosition.y);

        Next();
    }
    private IEnumerator WrongAnswer() {
        canInput = false;
        table.color = wrongColor;
        float currTime = 0;

        VibroControl.Vibration();

        
        if (typeAnswer != TypeAnswer.TrueFalse) {
            currTime = 0;
            while (currTime < timeRightAnswer) {
                yield return null;
                currTime += Time.deltaTime;
            }

            while (playerInput.Length > 0) {
                currTime = 0;
                while (currTime < delayBetweenSymbol) {
                    yield return null;
                    currTime += Time.deltaTime;
                }

                playerInput = playerInput.Remove(playerInput.Length - 1);
                solutionText.text = currentSolution + playerInput;

                yield return null;
            }

            currTime = 0;
            while (currTime < timeRightAnswer / 2) {
                yield return null;
                currTime += Time.deltaTime;
            }

            string answerStr = currentAnswer.ToString();
            string currAnswer = "";
            for (int i = 0; i < answerStr.Length; i++) {
                currTime = 0;
                while (currTime < delayBetweenSymbol) {
                    yield return null;
                    currTime += Time.deltaTime;
                }

                currAnswer += answerStr[i];
                solutionText.text = currentSolution + currAnswer;

                yield return null;
            }
            
        }

        currTime = 0;
        while (currTime < timeWrongAnswer * 2) {
            yield return null;
            currTime += Time.deltaTime;
        }

        canInput = true;
        table.color = defColor;
        currentWrongSolved++;
        Difficult = startDifficult;

        lineTimer.localPosition = new Vector2(0, lineTimer.localPosition.y);

        Next();
    }
    
    public void InputPlayer(string num) {
        if (canInput) {
            if (playerInput.Length == 1 && playerInput[0] == '?') {
                if (num[0] != '0')
                    playerInput = num;
                else {
                    return;
                }
            } else {
                playerInput += num;
            }
            solutionText.text = currentSolution + playerInput;

            CheckAnswer();
        }
    }
    public void Clear() {
        if (canInput) {
            playerInput = "?";
            solutionText.text = currentSolution;
        }
    }
    public void InputTest(ButtonsTest button) {
        playerInput = button.number.ToString();
        solutionText.text = currentSolution + playerInput;
        checkAnswer = button.number == currentAnswer ? 1 : -1;
    }
    public void InputBool(bool bl) {
        if (bl == (boolAnswer == Answer)) {
            checkAnswer = 1;
        } else checkAnswer = -1;
    }

    private void CheckAnswer() {
        checkAnswer = int.Parse(playerInput) == currentAnswer ? 1 : 0;
        if (checkAnswer == 0 && playerInput.Length >= currentAnswer.ToString().Length)
            checkAnswer = -1;
    }
    
    public void SetTimeForAnswer(Slider slider) {
        timeForAnswer = (int)slider.value;
    }
    public void SetDifficult(Slider slider) {
        maxDifficult = (int)slider.value;
    }

    public void ChengeOperations(Operation op) {
        if (op.isActive) {
            op.isActive = false;
            RemoveOperation(op);
        } else {
            op.isActive = true;
            AddOperation(op);
        }
    }
    private void AddOperation(Operation op) {
        Operation[] tmp = operations;
        operations = new Operation[operations == null || operations.Length == 0 ? 1 : operations.Length + 1]; ;
        for (int i = 0; i < tmp.Length; i++) {
            operations[i] = tmp[i];
        }

        operations[operations.Length - 1] = op;
    }
    private void RemoveOperation(Operation op) {
        for (int i = 0; i < operations.Length; i++) {
            if (op.GetType() == operations[i].GetType()) {

                Operation[] tmp = operations;
                operations = new Operation[operations == null ? 0 : operations.Length - 1]; ;
                for (int j = 0, k = 0; j < tmp.Length; j++) {
                    if (j != i) {
                        operations[k] = tmp[j];
                        k++;
                    }
                }
            }
        }
    }
    
    public void EndGame() {
        //Avg
        avgTimeSolved = currentMindTime / (currentSolved * 1.0f);
        if (currentSolved == 0) avgTimeSolvedText.text = "0.00";
        else avgTimeSolvedText.text = avgTimeSolved.ToString("0.00");

        //Alltime
        lastTime = (int)currentMindTime;
        second = lastTime % 60;
        if (second < 10) helpChar = "0";
        else helpChar = "";
        currentAllTimeText.text = (int)(lastTime / 60f) + ":" + helpChar + second;

        //Solved
        currentSolvedText.text = currentSolved.ToString();


        //Score
        if (currentSolved == 0) scoreText.text = "Zero";
        else {
            float k = typeAnswer == TypeAnswer.Keyboard ? 2 : (typeAnswer == TypeAnswer.Test ? 1 : 0.5f);
            score = (int)((currentSolved * k) / (timeForAnswer - (int)avgTimeSolved));
            scoreText.text = score.ToString();
        }

        SetTotalTime();
        SetMaxScore();
        SetSolved();
        SetAvgTime();
    }

    private void SetAvgTime() {
        if (PlayerPrefs.HasKey("Solved") && PlayerPrefs.HasKey("TotalTime")) {
            string tmp = ((PlayerPrefs.GetInt("TotalTime") * 1.0f) / (PlayerPrefs.GetInt("Solved") * 1.0f)).ToString("0.00");
            PlayerPrefs.SetString("avgTime", tmp);
        }
    }
    private void SetTotalTime() {
        if (PlayerPrefs.HasKey("TotalTime")) {
            PlayerPrefs.SetInt("TotalTime", PlayerPrefs.GetInt("TotalTime") + lastTime);
        } else PlayerPrefs.SetInt("TotalTime", lastTime);
    }
    private void SetSolved() {
        if (PlayerPrefs.HasKey("Solved")) {
            PlayerPrefs.SetInt("Solved", PlayerPrefs.GetInt("Solved") + currentSolved);
        } else PlayerPrefs.SetInt("Solved", currentSolved);
    }
    private void SetMaxScore() {
        if (currentSolved > 0 && PlayerPrefs.HasKey("MaxScore") && score > PlayerPrefs.GetInt("MaxScore")) {
            PlayerPrefs.SetInt("MaxScore", score);
        } else if (currentSolved > 0 && !PlayerPrefs.HasKey("MaxScore")) {
            PlayerPrefs.SetInt("MaxScore", score);
        }
    }

}
