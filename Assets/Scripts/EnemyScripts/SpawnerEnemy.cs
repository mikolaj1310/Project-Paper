using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerEnemy : MonoBehaviour
{
    public GameObject enemy;
    GameObject player;
    int currentPoint;
    private Rigidbody body;

    bool moving = false;
    bool spawning = false;
    float spawnCooldown = 1;
    float maxSpawnCooldown = 7;

    int maxMoveCount = 3;
    int currMoveCount = 3;

    int maxEnemiesSpawned = 1;
    int currEnemiesSpawned = 0;

    public float moveSpeed = 10f;

    float maxHealth;
    public float health = 150f;

    bool isInvincible = false;
    public float maxIvincibilityTime = 0.2f;
    float invincibleTimer;

    Transform indicator;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody>();
        maxHealth = health;
        indicator = transform.Find("Indicator");
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(this.gameObject);
            player.gameObject.GetComponent<Spawner>().Died();
            player.GetComponent<PlayerController>().AlterCash(5);
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

        spawnCooldown -= Time.deltaTime;
        if (!moving && spawnCooldown < 0)
        {
            if (player.gameObject.GetComponent<Spawner>().GetEnemyCount() < 50)
                SpawnEnemies();
            spawnCooldown = maxSpawnCooldown;
        }    
        else if(!moving && spawnCooldown > 0)
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

    Vector3 GetRandomPosition()
    {
        Vector3 offset = Random.insideUnitCircle * 5;
        offset.y = 0f;
        return transform.position + offset;
    }

    void SpawnEnemies()
    {
        for (int i = 0; i <= maxEnemiesSpawned; i++)
        {
            Spawner sp = player.transform.GetComponent<Spawner>();
            sp.IncrementEnemyCount();
            Instantiate(enemy, GetRandomPosition(), Quaternion.identity);
            currEnemiesSpawned++;
        }
    }

    IEnumerator Wander(GameObject spawnPos)
    {
        moving = true;

        float timer = 3 + Random.Range(-1, 1); ;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            Vector3 Direction = spawnPos.transform.position - transform.position;
            transform.LookAt(new Vector3(spawnPos.transform.position.x, transform.position.y, spawnPos.transform.position.z));
            Direction.Normalize();
            Direction.y = 0;

            body.MovePosition(transform.position + Direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
        moving = false;
        yield return null;
    }

    GameObject GetRandomPoint()
    {
        GameObject[] randomPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
        int randPointNew = -1;
        randPointNew = Random.Range(0, randomPoints.Length);

        while (randPointNew == currentPoint)
        {
            randPointNew = Random.Range(0, randomPoints.Length);
        }
        currentPoint = randPointNew;

        return randomPoints[currentPoint];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            AlterHealth(-5, false);
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
}
