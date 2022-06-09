using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject enemy;
    int enemyCount;
    public GameObject spawnerEnemy;
    int spawnerEnemyCount;
    public GameObject chargerEnemy;
    int chargerEnemyCount;
    public GameObject bossEnemy;
    int bossEnemyCount;

    public bool paused = false;
    private AudioController PlayMusic;

    public float waveEnemyCount = 3;
    public float waveFrequency = 5;
    public float difficulty = 1.1f;
    int currentlyAlive = 0;
    int currentWave = 1;
    int killCount = 0;

    float pausePeriod = 20;
    float pauseTimer = 20;

    float waveTimer = 30;
    float maxWaveTimer = 30;

    GameObject[] spawnPoints;

    public Text waveTimerText;

    void Start()
    {
        PlayMusic = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<AudioController>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
        waveEnemyCount = 3;
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 randPos = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        return randPos;
    }
    private Vector3 GetRandomBossPosition()
    {
        Vector3 randPos = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        randPos.y = 1.5f;
        return randPos;
    }

    //Starting and Stopping Coroutines
    void Update()
    {
        if (paused)
        {
            int timeAsInt = (int)pauseTimer;
            waveTimerText.text = "Wave In: " + timeAsInt.ToString() + "s";


                pauseTimer -= Time.deltaTime;
            if (pauseTimer < 0)
            {
                GameObject.FindGameObjectWithTag("Door").GetComponent<Doors>().CloseDoor();
                pauseTimer = pausePeriod;
                paused = false;
            }
        }
        if (!paused)
        {
            waveTimer -= Time.deltaTime;
            waveTimerText.text = "Wave: " + (currentWave - 1).ToString();
            if (currentlyAlive <= 0 || waveTimer <= 0)
            {
                waveTimer = maxWaveTimer;
                switch (currentWave)
                {
                    case 1:
                        enemyCount = 4;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 0;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 2:
                        enemyCount = 3;
                        spawnerEnemyCount = 1;
                        chargerEnemyCount = 0;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 3:
                        enemyCount = 4;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 4:
                        enemyCount = 2;
                        spawnerEnemyCount = 1;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 5:
                        GameObject.FindGameObjectWithTag("Door").GetComponent<Doors>().OpenDoor();
                        paused = true;
                        currentWave++;
                        break;
                    case 6:
                        enemyCount = 2;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 2;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 7:
                        enemyCount = 0;
                        spawnerEnemyCount = 2;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 8:
                        enemyCount = 6;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 2;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 9:
                        enemyCount = 4;
                        spawnerEnemyCount = 2;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 10:
                        GameObject.FindGameObjectWithTag("Door").GetComponent<Doors>().OpenDoor();
                        paused = true;
                        currentWave++;
                        break;
                    case 11:
                        enemyCount = 8;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 12:
                        enemyCount = 6;
                        spawnerEnemyCount = 2;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 13:
                        enemyCount = 10;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 1;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 14:
                        enemyCount = 0;
                        spawnerEnemyCount = 4;
                        chargerEnemyCount = 0;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 15:
                        GameObject.FindGameObjectWithTag("Door").GetComponent<Doors>().OpenDoor();
                        paused = true;
                        currentWave++;
                        break;
                    case 16:
                        enemyCount = 15;
                        spawnerEnemyCount = 0;
                        chargerEnemyCount = 2;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 17:
                        enemyCount = 8;
                        spawnerEnemyCount = 4;
                        chargerEnemyCount = 2;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 18:
                        enemyCount = 12;
                        spawnerEnemyCount = 3;
                        chargerEnemyCount = 3;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 19:
                        enemyCount = 10;
                        spawnerEnemyCount = 6;
                        chargerEnemyCount = 4;
                        StartCoroutine(SpawnWave(enemyCount, spawnerEnemyCount, chargerEnemyCount));
                        currentlyAlive += enemyCount + spawnerEnemyCount + chargerEnemyCount;
                        currentWave++;
                        break;
                    case 20:
                        GameObject.FindGameObjectWithTag("Door").GetComponent<Doors>().OpenDoor();
                        paused = true;
                        currentWave++;
                        break;
                    case 21:
                        GameObject.FindGameObjectWithTag("Boss Enemy").GetComponent<BossEnemy>().ActivateBoss();
                        GameObject.FindGameObjectWithTag("BossDoor").GetComponent<Doors>().OpenDoor();
                        PlayMusic.PlayBoss();
                        currentWave++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    IEnumerator SpawnWave(float enemyCount, float spawnerCount, float chargerCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(enemy, GetRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < spawnerCount; i++)
        {
            Instantiate(spawnerEnemy, GetRandomPosition(), Quaternion.identity);
        }
        for (int i = 0; i < chargerCount; i++)
        {
            Instantiate(chargerEnemy, GetRandomPosition(), Quaternion.identity);
        }
        yield return null;
    }

    public void Died()
    {
        killCount++;
        currentlyAlive--;
    }

    public void IncrementEnemyCount()
    {
        currentlyAlive++;
    }

    public int GetEnemyCount()
    {
        return currentlyAlive;
    }

    public float GetPauseTimer()
    {
        return pauseTimer;
    }

    public bool GetPaused()
    {
        return paused;
    }
}
