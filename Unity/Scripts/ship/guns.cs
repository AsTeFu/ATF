using UnityEngine;
using System.Collections;

public class GunLazer : MonoBehaviour {

    [Header("Levels Gun")]
    //Ограничители
    [SerializeField] private int minLevel = 0;
    [SerializeField] private int maxLevel = 9;
    
    [Space(15)]
    [SerializeField] private int levelLazers = 0; //Уровень лазера влияет на кол-во лазеров и их урон
    public int LevelLazers {
        get {
            return levelLazers;
        }
        set {
            if (value >= 0 && value <= 10)
                levelLazers = value;
            else
                print("Неверное присвоения уровня");
        }               
    }

    [Header("Damage")]
    [SerializeField][Tooltip("Урон")] private int damage;
    public int Damage {
        get {
            return damage;
        }
        private set
        {
            damage = value;
        }
    }
    [Header("Delays")]
    [SerializeField][Tooltip("Задержка выстрела")] private float delayShoot;
    public float DelayShoots {
        get {
            return delayShoot;
        }
        private set {
            delayShoot = value;
        }
    }

    private bool canShoots = true;

    [Header("Objects")]
    [SerializeField] private Transform[] levelObjects; //Разделение по уровням
    [SerializeField] private GameObject bullet;
    private Transform[] bulletSpawn;

    [Header("Pool")]
    [SerializeField][Tooltip("Число пуль в пулле")] private int spawnCount = 5; // Recomend value - 5;
    [SerializeField] private Transform poolLazers;
    private GameObject [,] poolBullet;
    private Bullet[,] rendrerBullet;
    private int indexBullet = 0;

    private Vector3 posPool = new Vector3(110, 110, 110);


    void Awake() {
        damage = Mathf.Abs(damage);
        levelObjects = new Transform[maxLevel + 1]; //Инициализация массива уровней оружия
        for (int i = 0; i < maxLevel + 1; i++) {
            levelObjects[i] = transform.GetChild(i); //Заполнение массива уровней
        }
        levelObjects[levelLazers].gameObject.SetActive(true); //Активация нужного уровня

        bulletSpawn = new Transform[levelObjects[levelLazers].transform.childCount]; //Инициализация массива отдельного уровня для спавна
        poolBullet = new GameObject[bulletSpawn.Length, spawnCount]; //Массив объектов
        rendrerBullet = new Bullet[bulletSpawn.Length, spawnCount]; //Массив траилрендера объектов
        
        for (int i = 0; i < levelObjects[levelLazers].transform.childCount; i++) {
            bulletSpawn[i] = levelObjects[levelLazers].transform.GetChild(i);
        }
        

        if (levelLazers < minLevel || levelLazers > maxLevel)
            levelLazers = 0;
    }

    void Start() {
        StartCoroutine(ShootsStart());
    }

    IEnumerator ShootsStart() {
        //Спавн пулей
        for (int x = 0; x < spawnCount; x++) { // Цикл создает пули
            for (int i = 0, y = 0; i < bulletSpawn.Length; i++, y++) { // Цикл числа пушек
                if (indexBullet == spawnCount)
                    indexBullet = 0;
                poolBullet[i, x] = Instantiate(bullet, bulletSpawn[i].position, bulletSpawn[i].rotation, poolLazers);
                //print("Создан " + x + " снаряд " + i + " орудия");
                rendrerBullet[i, x] = poolBullet[i, x].GetComponent<Bullet>();
                rendrerBullet[i, x].ToPool(true);
            }
            yield return new WaitForSeconds(delayShoot);
        }

        StartCoroutine(Shoots()); //Переход в стрельбу из пула
    }

    IEnumerator Shoots() {
        //Стрельба из пула
        while (canShoots) {
            for (int i = 0; i < bulletSpawn.Length; i++) {
                if (indexBullet == spawnCount)
                    indexBullet = 0;
                poolBullet[i, indexBullet].transform.position = bulletSpawn[i].position;
                rendrerBullet[i, indexBullet].ToPool(true);
                //poolBullet[i, indexBullet].GetComponent<Bullet>().ToPool(true);
            }
            indexBullet++;
            yield return new WaitForSeconds(delayShoot);
        }
    }
}


/*
    

    public class Gun : MonoBehaviour {

    [Header("Damage")]
    [SerializeField][Tooltip("Урон")] private float damage;
    [Header("Delays")]
    [SerializeField][Tooltip("Задержка выстрела")] private float delayShoot;
    public float DelayShoots {
        get {
            return delayShoot;
        }
        private set {
            delayShoot = value;
        }
    }

    private bool canShoots = true;

    [Header("Objects")]
    [SerializeField] private Transform[] bulletSpawn;
    [SerializeField] private GameObject bullet;

    [Header("Pool")]
    [SerializeField][Tooltip("Число пуль в пулле")] private int spawnCount = 7;
    private GameObject[,] poolBullet; //= new GameObject[10, 20];
    private int indexBullet = 0;

    private Vector3 posPool = new Vector3(110, 110, 110);


    void Awake() {
        damage = Mathf.Abs(damage);
        poolBullet = new GameObject[bulletSpawn.Length, spawnCount];
    }

    void Start() {
        StartCoroutine(ShootsStart());
    }

    IEnumerator ShootsStart() {
        for (int x = 0; x < spawnCount; x++) {
            for (int i = 0; i < bulletSpawn.Length; i++, indexBullet++) {
                if (indexBullet == spawnCount)
                    indexBullet = 0;
                poolBullet[i, indexBullet] = (GameObject)Instantiate(bullet, bulletSpawn[i].position, Quaternion.identity);
                poolBullet[i, indexBullet].GetComponent<Bullet>().ToPool(true);
            }
            yield return new WaitForSeconds(delayShoot);
        }

        StartCoroutine(Shoots());

          for (int i = 0; i < bulletSpawn.Length; i++)
            {
                for (int y = 0; y < spawnCount; y++) {
                    poolBullet[i, y] = (GameObject)Instantiate(bullet, posPool, Quaternion.identity);
                    //poolBullet[i, y].GetComponent<Bullet>().ToPool(false);
                }

            }

             
            
    }

    IEnumerator Shoots()
{
    while (canShoots)
    {
        for (int i = 0; i < bulletSpawn.Length; i++, indexBullet++)
        {
            if (indexBullet == spawnCount)
                indexBullet = 0;
            poolBullet[i, indexBullet].transform.position = bulletSpawn[i].position;
            poolBullet[i, indexBullet].GetComponent<Bullet>().ToPool(true);
            print("Орудие " + i + " выстрелило снарядом " + indexBullet);
        }
        yield return new WaitForSeconds(delayShoot);
    }
}
}


    */
