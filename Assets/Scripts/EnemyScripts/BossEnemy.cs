using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    //Variables
    public GameObject enemy;
    GameObject player;
    int currentPoint;
    private Rigidbody body;
    private AudioController PlayMusic;

    bool moving = false;
    int maxMoveCount = 3;
    int currMoveCount = 0;
    bool attacking = false;


    public float spawnerDelay = 15;
    float currentDelay;
    bool spawning = false;
    float currEnemiesSpawned = 0;
    float maxEnemiesSpawned = 1;

    public float chargeSpeed = 50f;
    public float speed = 10f;

    float maxHealth;
    public float health = 5000f;

    bool isInvincible = false;
    public float maxIvincibilityTime = 0.2f;
    float invincibleTimer;
    bool wallHit = false;

    bool isActive;
    bool activating;

    Transform indicator;

    // Start is called before the first frame update
    void Start()
    {
        // Assigning Variable Values
        player = GameObject.FindGameObjectWithTag("Player");
        PlayMusic = player.GetComponent<AudioController>();
        body = GetComponent<Rigidbody>();
        maxHealth = health;
        indicator = transform.Find("Indicator");
        moving = false;
        attacking = false;
        currentDelay = 0;
        isActive = false;
        activating = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay -= Time.deltaTime;
        if (health <= 0)
        {
            PlayMusic.BaseMusic();
            Destroy(this.gameObject);
            player.GetComponent<PlayerController>().AlterCash(50);
        }
        if (health > maxHealth / 2)
        {
            float r = 1 - (float)(health / maxHealth);
            indicator.GetComponent<Renderer>().material.color = new Color(r, 1f, 0f, 1f);
        }
        else
        {
            float g = (health / maxHealth * 2);
            indicator.GetComponent<Renderer>().material.color = new Color(1f, g, 0f, 1f);
        }


        if (isActive)
        {
            speed = 10f;
            if (!moving && !attacking && !spawning)
            {
                GameObject randomPoint = GetRandomPoint();
                StartCoroutine(Wander(randomPoint));
            }
            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer <= 0)
                {
                    ResetInvincible();
                }
            }
        }
        else
        {
            speed = 5f;
            if (!moving)
            {
                GameObject randomPoint = GetRandomIdlePoint();
                StartCoroutine(Wander(randomPoint));
            }
        }
    }

    public void ActivateBoss()
    {
        GameObject randomPoint = GameObject.FindGameObjectWithTag("Boss Active Point");
        activating = true;
        StartCoroutine(Wander(randomPoint));
    }

    GameObject GetRandomIdlePoint()
    {
        GameObject[] randomPoints = GameObject.FindGameObjectsWithTag("Boss Idle Point");
        int randPointNew = -1;
        randPointNew = Random.Range(0, randomPoints.Length);

        while (randPointNew == currentPoint)
        {
            randPointNew = Random.Range(0, randomPoints.Length);
        }
        currentPoint = randPointNew;

        return randomPoints[currentPoint];
    }

    GameObject GetRandomPoint()
    {
        GameObject[] randomPoints = GameObject.FindGameObjectsWithTag("Charger Point");
        int randPointNew = -1;
        randPointNew = Random.Range(0, randomPoints.Length);

        while (randPointNew == currentPoint)
        {
            randPointNew = Random.Range(0, randomPoints.Length);
        }
        currentPoint = randPointNew;

        return randomPoints[currentPoint];
    }

    Vector3 GetPointInRadious()
    {
        Vector3 offset = Random.insideUnitCircle * 10;
        offset.y = 0f;
        return transform.position + offset;
    }

    Vector3 GetRandomPosition()
    {
        Vector3 offset = Random.insideUnitCircle * 5;
        offset.y = 0f;
        return transform.position + offset;
    }
    IEnumerator ChargeAttack()
    {
        attacking = true;
        var playerPos = player.transform.position;
        Vector3 Direction = player.transform.position - transform.position;
        Direction.Normalize();
        Direction.y = 0;

        float timer = 1;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));
            yield return null;
        }
        float chargeTimer = 0.5f;
        while (chargeTimer > 0)
        {
            chargeTimer -= Time.deltaTime;
            if (wallHit == true)
            {
                chargeTimer = 0;
                wallHit = false;
            }
            else
                body.MovePosition(transform.position + Direction * chargeSpeed * Time.deltaTime);
            yield return null;
        }
        attacking = false;
        yield return null;
    }

    void SpawnEnemies()
    {
        spawning = false;
        for (int i = 0; i <= maxEnemiesSpawned; i++)
        {
            Spawner sp = player.transform.GetComponent<Spawner>();
            sp.IncrementEnemyCount();
            Instantiate(enemy, GetRandomPosition(), Quaternion.identity);
            currEnemiesSpawned++;
        }
    }

    IEnumerator Wander(GameObject walkPoint)
    {
        moving = true;
        float timer = 1.5f;
        if (activating)
            timer = 4;

            transform.LookAt(new Vector3(walkPoint.transform.position.x, transform.position.y, walkPoint.transform.position.z));
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Vector3 Direction = walkPoint.transform.position - transform.position;
            Direction.Normalize();
            Direction.y = 0;            

            body.MovePosition(transform.position + Direction * speed * Time.deltaTime);
            yield return null;
        }
        currMoveCount++;
        if (isActive)
        {
            if (currentDelay < 0)
            {
                currentDelay = spawnerDelay;
                SpawnEnemies();
            }
            else if (currMoveCount <= maxMoveCount)
            {
                StartCoroutine(Wander(GetRandomPoint()));
            }
            else
            {
                currMoveCount = 0;
                moving = false;
                StartCoroutine(ChargeAttack());
            }
        }
        else 
        {
            moving = false;
        }

        if (activating)
        {
            isActive = true;
            currentDelay = spawnerDelay;
        }
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            AlterHealth(-5, false);
        }

        if (other.tag == "Wall" || other.tag == "Door")
        {
            if (attacking)
                wallHit = true;
        }

        if (other.tag == "PlayerBody")
        {
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (GetChargeState())
            {
                playerController.AlterHealth(-25, true);
                print("charge hit");
            }
            else
            {
                playerController.AlterHealth(-10, true);
                print("boss hit");
            }
        }

        if (other.tag == "Enemy")
        {
            if (GetChargeState())
                Destroy(other.gameObject);
        }
        if (other.tag == "Charger Enemy")
        {
            if (GetChargeState())
                Destroy(other.gameObject);
        }
    }

    public void AlterHealth(float altHP, bool inv)
    {
        if (!isInvincible)
        {
            health += altHP;
            if (inv)
            {
                isInvincible = true;
                invincibleTimer = maxIvincibilityTime;
            }
        }
    }

    void ResetInvincible()
    {
        isInvincible = false;
    }

    public void KnockBack(float intensity)
    {
        Vector3 dir = transform.position - player.transform.position;
        dir.y = 0;
        dir.Normalize();
        body.velocity = dir * intensity;
    }

    public bool GetChargeState()
    {
        return attacking;
    }
}
