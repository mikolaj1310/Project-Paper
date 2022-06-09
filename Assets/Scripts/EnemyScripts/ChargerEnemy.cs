using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerEnemy : MonoBehaviour
{
    GameObject player;
    private Rigidbody body;
    public GameObject projectile;

    float maxSpeed = 10f;
    float chargeSpeed = 50f;
    public bool killed = false;

    float maxHealth = 300f;
    public float health;

    bool charging = false;
    float chargeCooldown = 10;
    float maxChargeCooldown = 10;

    bool shooting = false;
    float shootingCooldown = 7;
    float maxShootingCooldown = 7;
    
    bool moving = false;
    float movingDuration = 0;
    float maxMovingDuration = 2;

    bool isInvincible = false;
    public float maxIvincibilityTime = 0.2f;
    float invincibleTimer;
    bool wallHit = false;
    int currentPoint;

    float ammo = 2;
    float maxAmmo = 2;

    Transform indicator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        body = this.GetComponent<Rigidbody>();
        maxSpeed = maxSpeed + Random.Range(-2f, 2f);
        health = maxHealth;
        indicator = transform.Find("Indicator");
        currentPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //PlayMusic.BaseMusic();
            Destroy(this.gameObject);
            player.gameObject.GetComponent<Spawner>().Died();
            player.GetComponent<PlayerController>().AlterCash(10);
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

        shootingCooldown -= Time.deltaTime;
        chargeCooldown -= Time.deltaTime;

        if (!moving && !shooting && !charging)
        {
            if (chargeCooldown < 0)
            {
                StartCoroutine(ChargeAttack());
                chargeCooldown = maxChargeCooldown;
            }
            else if(shootingCooldown < 0)
            {
                StartCoroutine(Shoot());
                ammo--;
                if (ammo < 0)
                {
                    shootingCooldown = maxShootingCooldown;
                    ammo = maxAmmo;
                }
            }
            else
            {
                GameObject randPoint = GetRandomPoint();
                StartCoroutine(Wander(randPoint));
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = true;
            }
        }
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

    IEnumerator Shoot()
    {
        shooting = true;
        float timer = 0.5f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
            yield return null;
        }
        Instantiate(projectile, transform.position, Quaternion.identity);
        shooting = false;
        yield return null;
    }

    IEnumerator ChargeAttack()
    {
        charging = true;
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
        charging = false;
        yield return null;
    }

    IEnumerator Wander(GameObject walkPoint)
    {
        moving = true;
        transform.LookAt(new Vector3(walkPoint.transform.position.x, transform.position.y, walkPoint.transform.position.z));
        while (movingDuration > 0)
        {
            movingDuration -= Time.deltaTime;
            Vector3 Direction = walkPoint.transform.position - transform.position;
            Direction.Normalize();
            Direction.y = 0;

            body.MovePosition(transform.position + Direction * maxSpeed * Time.deltaTime);
            yield return null;
        }
        moving = false;
        movingDuration = maxMovingDuration;
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
            if (charging)
                wallHit = true;
        }

        if (other.tag == "PlayerBody")
        {
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (charging)
            {
                playerController.AlterHealth(-15, true);
            }
            else
            {
                playerController.AlterHealth(-5, true);
            }
        }
    }

    public void AlterHealth(float amount, bool inv)
    {
        if (!isInvincible)
        {
            health += amount;
            if (inv)
            {
                isInvincible = true;
                invincibleTimer = maxIvincibilityTime;
            }
        }
    }
}
