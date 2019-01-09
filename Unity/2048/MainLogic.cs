using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ConditionItem {
    Empty = 1,  
    First = 2,
    Second = 4,
    Third = 8,
    Fourth = 16,
    Fifth = 32,
    Sixth = 64,
    Seventh = 128,
    Eighth = 256, 
    Ninth = 512, 
    Tenth = 1024,
    Eleventh = 2048
}

public class MainLogic : MonoBehaviour {

    private int[,] gameField;
    private bool[,] newItems;
    private Item[,] items;

    private int score;
    public int Score {
        get {
            return score;
        }
    }

    //[SerializeField] private GameObject item;
    [SerializeField] [Range(0, 100)] private int probabilitySpawnHighItem;

    private Grid grid;
    private int sizeGrid;

    [Space(20)]
    [SerializeField] private UnityEvent startGameEvent;
    [SerializeField] private UnityEvent ScoreEvent;

    private void Awake() {
        grid = GetComponentInChildren<Grid>();
    }

    public void CreateGameField(int size) {
        gameField = new int[size, size];

        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                gameField[i, j] = 1;
            }
        }
    }

    private void Start() {
        startGameEvent?.Invoke();
        CreateGameField(grid.SizeGrid);
        items = grid.grid;
        sizeGrid = grid.SizeGrid;
    }

    public void StartGame() {
        CreateGameField(grid.SizeGrid);
        for (int i = 0; i < sizeGrid; i++) {
            for (int j = 0; j < sizeGrid; j++) {
                items[i, j].TransitionEmpty();
            }
        }

        score = 0;
        ScoreEvent?.Invoke();

        CreateNewItem();
        CreateNewItem();
    }
    

    private void CreateNewItem() {
        (int, int) coordinate  = GetRandomEmptyCell();
        int startValue = Random.Range(0, 100) > probabilitySpawnHighItem ? 4 : 2;

        gameField[coordinate.Item1, coordinate.Item2] = startValue;
        items[coordinate.Item1, coordinate.Item2].Create((ConditionItem)startValue);
    }
    private (int, int) GetRandomEmptyCell() {
        (int, int) index;

        do {
            index = (Random.Range(0, sizeGrid), Random.Range(0, sizeGrid));
        } while (gameField[index.Item1, index.Item2] != 1);

        return index;
    }
    

    public float delay;

    private IEnumerator UpCorutine() {
        newItems = new bool[sizeGrid, sizeGrid];
        for (int i = 0; i < sizeGrid; i++) {
            for (int j = 0; j < sizeGrid; j++) {
                newItems[i, j] = false;
            }
        }
        
        bool isMove = false;
        for (int x = 0; x < sizeGrid; x++) {
            for (int y = 1; y < sizeGrid; y++) {
                if (gameField[x, y] > 1) {
                    int newY = y;
                    int upY = y - 1;

                    while (newY > 0 && (gameField[x, upY] == 1 || gameField[x, newY] == gameField[x, upY])) {
                        if (gameField[x, upY] == 1) {

                            gameField[x, upY] = gameField[x, newY];
                            gameField[x, newY] = 1;

                            items[x, upY].Transition((ConditionItem)gameField[x, upY], (x, upY));
                            items[x, newY].TransitionEmpty();
                            newY--;
                            upY--;

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                        } else if (gameField[x, newY] == gameField[x, upY] && !newItems[x, upY]) {
                            gameField[x, upY] *= 2;
                            gameField[x, newY] = 1;

                            score += gameField[x, upY];
                            ScoreEvent?.Invoke();

                            if (upY == 0) newItems[x, upY] = true;

                            items[x, upY].Transition((ConditionItem)gameField[x, upY], (x, upY));
                            items[x, newY].TransitionEmpty();

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                            break;
                        }
                        if (!isMove)
                            isMove = true;
                        yield return null;
                    }
                }
            }
        }
        if (isMove) {
            CreateNewItem();
        }
    }
    private IEnumerator DownCorutine() {
        newItems = new bool[sizeGrid, sizeGrid];
        for (int i = 0; i < sizeGrid; i++) {
            for (int j = 0; j < sizeGrid; j++) {
                newItems[i, j] = false;
            }
        }

        bool isMove = false;
        for (int x = sizeGrid - 1; x >= 0; x--) {
            for (int y = sizeGrid - 2; y >= 0; y--) {
                if (gameField[x, y] > 1) {
                    int newY = y;
                    int downY = y + 1;

                    while (newY < sizeGrid - 1 && (gameField[x, downY] == 1 || gameField[x, newY] == gameField[x, downY])) {
                        if (gameField[x, downY] == 1) {

                            gameField[x, downY] = gameField[x, newY];
                            gameField[x, newY] = 1;

                            items[x, downY].Transition((ConditionItem)gameField[x, downY], (x, downY));
                            items[x, newY].TransitionEmpty();
                            newY++;
                            downY++;

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                        } else if (gameField[x, newY] == gameField[x, downY] && !newItems[x, downY]) {
                            gameField[x, downY] *= 2;
                            gameField[x, newY] = 1;

                            score += gameField[x, downY];
                            ScoreEvent?.Invoke();

                            if (downY == sizeGrid - 1) newItems[x, downY] = true;

                            items[x, downY].Transition((ConditionItem)gameField[x, downY], (x, downY));
                            items[x, newY].TransitionEmpty();

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                            break;
                        }
                        if (!isMove)
                            isMove = true;
                        yield return null;
                    }
                }
            }
        }
        if (isMove)
            CreateNewItem();
    }
    private IEnumerator LeftCorutine() {
        newItems = new bool[sizeGrid, sizeGrid];
        for (int i = 0; i < sizeGrid; i++) {
            for (int j = 0; j < sizeGrid; j++) {
                newItems[i, j] = false;
            }
        }

        bool isMove = false;
        for (int y = 0; y < sizeGrid; y++) {
            for (int x = 1; x < sizeGrid; x++) {
                if (gameField[x, y] > 1) {
                    int newX = x;
                    int leftX = x - 1;

                    while (newX > 0 && (gameField[leftX, y] == 1 || gameField[newX, y] == gameField[leftX, y])) {
                        if (gameField[leftX, y] == 1) {

                            gameField[leftX, y] = gameField[newX, y];
                            gameField[newX, y] = 1;

                            items[leftX, y].Transition((ConditionItem)gameField[leftX, y], (leftX, y));
                            items[newX, y].TransitionEmpty();
                            newX--;
                            leftX--;

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                        } else if (gameField[newX, y] == gameField[leftX, y] && !newItems[leftX, y]) {
                            gameField[leftX, y] *= 2;
                            gameField[newX, y] = 1;

                            score += gameField[leftX, y];
                            ScoreEvent?.Invoke();

                            if (leftX == 0) newItems[leftX, y] = true;

                            items[leftX, y].Transition((ConditionItem)gameField[leftX, y], (leftX, y));
                            items[newX, y].TransitionEmpty();

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                            break;
                        }
                        if (!isMove)
                            isMove = true;
                        yield return null;
                    }
                }
            }
        }
        if (isMove)
            CreateNewItem();
    }
    private IEnumerator RightCorutine() {
        newItems = new bool[sizeGrid, sizeGrid];
        for (int i = 0; i < sizeGrid; i++) {
            for (int j = 0; j < sizeGrid; j++) {
                newItems[i, j] = false;
            }
        }
        
        bool isMove = false;
        for (int y = sizeGrid - 1; y >= 0; y--) {
            for (int x = sizeGrid - 2; x >= 0; x--) {
                if (gameField[x, y] > 1) {
                    int newX = x;
                    int rightX = x + 1;

                    while (newX < sizeGrid - 1 && (gameField[rightX, y] == 1 || gameField[newX, y] == gameField[rightX, y])) {
                        if (gameField[rightX, y] == 1) {

                            gameField[rightX, y] = gameField[newX, y];
                            gameField[newX, y] = 1;

                            items[rightX, y].Transition((ConditionItem)gameField[rightX, y], (rightX, y));
                            items[newX, y].TransitionEmpty();
                            newX++;
                            rightX++;

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                        } else if (gameField[newX, y] == gameField[rightX, y] && !newItems[rightX, y]) {
                            gameField[rightX, y] *= 2;
                            gameField[newX, y] = 1;

                            score += gameField[rightX, y];
                            ScoreEvent?.Invoke();

                            if (rightX == sizeGrid - 1) newItems[rightX, y] = true;

                            items[rightX, y].Transition((ConditionItem)gameField[rightX, y], (rightX, y));
                            items[newX, y].TransitionEmpty();

                            float del = 0;
                            while (del < delay) {
                                del += Time.deltaTime;
                                yield return null;
                            }

                            break;
                        }
                        if (!isMove)
                            isMove = true;
                        yield return null;
                    }
                }
            }
        }
        if (isMove) {
            CreateNewItem();
        }
    }


    public void Up() {
        StartCoroutine(UpCorutine());
    }
    public void Left() {
        StartCoroutine(LeftCorutine());
    }
    public void Right() {
        StartCoroutine(RightCorutine());
    }
    public void Down() {
        StartCoroutine(DownCorutine()); 
    }
}


/*for (int j = 0; j < sizeGrid; j++) {

                if (gameField[i, j] > 1) {

                    int upJ, newJ = j;
                    for (upJ = j - 1; upJ >= 0; upJ--, newJ--) {
                        if (gameField[i, upJ] == 1) {

                            gameField[i, upJ] = gameField[i, newJ];
                            gameField[i, newJ] = 1;

                            items[i, newJ].Transition(ConditionItem.Empty);
                            items[i, upJ].Transition((ConditionItem)gameField[i, upJ]);
                            continue;

                        }
                        if (gameField[i, newJ] == gameField[i, upJ]) {

                            gameField[i, upJ] *= 2;
                            gameField[i, newJ] = 1;

                            items[i, newJ].Transition(ConditionItem.Empty);
                            items[i, upJ].Transition((ConditionItem)gameField[i, upJ]);

                            break;
                        } else {
                        }
                    }
                    
                }
            }
        } */
