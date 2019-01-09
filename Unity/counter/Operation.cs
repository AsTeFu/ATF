using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Operation : MonoBehaviour {

    private int maxNumber;
    protected int num1;
    protected int num2;
    //protected int answer;
    public bool isActive;

    protected delegate string StringSolution();
    protected delegate int GetAnswer();
    protected StringSolution getStringSolution;
    protected GetAnswer getAnswer;
    
    private Nums nums;
    [SerializeField] private RandomNum rnd;
    [SerializeField] private Answer answer;
    
    private void Awake() {
        nums = GetComponentInParent<Nums>();
        AwakeChild();
    }

    protected abstract void AwakeChild();

    public string GetSolution(ref int newAnswer) {
        do {
            GetRandomNumbers();
            newAnswer = getAnswer.Invoke();
        } while (answer.CheckAllAnswer(newAnswer));
        Debug.Log(newAnswer);
        return getStringSolution.Invoke();
    }
    
    protected void GetRandomNumbers() {
        maxNumber = nums.Difficult;

        num1 = rnd.GetRandomNumber(2, maxNumber + 1);
        num2 = rnd.GetRandomNumber(2, maxNumber + 1);
    }
    
}

[System.Serializable]
public class RandomNum {

    [SerializeField] private int depth = 10;
    [SerializeField] private int[] lastNums = new int[0];
    [SerializeField] private bool withoutZero = false;
    
    public RandomNum(int depth, bool withoutZero) {
        this.depth = depth < 1 ? 1 : depth;
        this.withoutZero = withoutZero;
    }

    public int GetRandomNumber(int min, int max) {
        int num = GetRandomNum(min, max);
        Add(num);
        return num;
    }

    private void Add(int num) {
        int[] tmp = new int[depth - 1 > lastNums.Length ? lastNums.Length : depth - 1];

        for (int i = 0; i < tmp.Length; i++) {
            tmp[i] = lastNums[lastNums.Length - (tmp.Length - i)];
        }

        lastNums = new int[lastNums == null ? 1 : tmp.Length + 1];

        for (int i = 0; i < tmp.Length; i++) {
            lastNums[i] = tmp[i];
        }

        lastNums[lastNums.Length - 1] = num;
    }
    private int GetRandomNum(int min, int max) {
        int num;
        do {
            num = Random.Range(min, max);
        }
        while (GetLastNum(num) || (withoutZero && (num == 0)));
        return num;
    }
    private bool GetLastNum(int newNum) {
        //int len = depth > lastNums.Length ? lastNums.Length : depth;

        foreach (int element in lastNums) {
            if (newNum == element)
                return true;
        }

        //for (int i = 0; i < len; i++) {
        //    if (newNum == lastNums[lastNums.Length - (i + 1)])
        //        return true;
        //}

        return false;
    }

    public void OutputArray() {
        string str = "";
        foreach (int element in lastNums) {
            str += element + ", ";
        }
        Debug.Log(str);
    }
}

[System.Serializable]
public class Answer {

    [SerializeField] private int depth = 5;
    [SerializeField] private int[] lastAnswers;

    private void Add(int answer) {
        int[] tmp = new int[depth - 1 > lastAnswers.Length ? lastAnswers.Length : depth - 1];

        for (int i = 0; i < tmp.Length; i++) {
            tmp[i] = lastAnswers[lastAnswers.Length - (tmp.Length - i)];
        }

        lastAnswers = new int[lastAnswers == null ? 1 : tmp.Length + 1];

        for (int i = 0; i < tmp.Length; i++) {
            lastAnswers[i] = tmp[i];
        }

        lastAnswers[lastAnswers.Length - 1] = answer;
    }
    public bool CheckAllAnswer(int answer) {
        foreach (int element in lastAnswers) {
            if (answer == element) {
                Debug.Log("Повтор " + element);
                return true;
            }
        }
        Add(answer);
        return false;
    }

}