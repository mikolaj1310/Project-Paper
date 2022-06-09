using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    //Variables
    GameObject Player;
    private Rigidbody Rigid;
    private Vector3 Movement;

    public float Acceleration = 5f;
    public float MaxSpeed = 4f;

    public bool Killed = false;

    float maxHealth;
    public float Health = 60f;
    bool isInvincible = false;
    public float maxIvincibilityTime = 0.2f;
    float invincibleTimer;
    Transform indicator;

    // Start is called before the first frame update
    void Start()
    {
        // Assigning Variable Values
        Player = GameObject.FindGameObjectWithTag("Player");
        Rigid = this.GetComponent<Rigidbody>();
        MaxSpeed = MaxSpeed + Random.Range(-2f, 2f);
        Acceleration = Acceleration + Random.Range(-2f, 2f);
        maxHealth = Health;
        indicator = transform.Find("Indicator");
    }

    // Update is called once per frame
    void Update()
    {
        if (Health <= 0)
        {
            Destroy(this.gameObject);
            Player.gameObject.GetComponent<Spawner>().Died();
            Killed = true;
            Player.GetComponent<PlayerController>().AlterCash(2);
        }
        if (Health > maxHealth / 2)
        {
            float r = 1 - (float)(Health / maxHealth);
            indicator.GetComponent<Renderer>().material.color = new Color(r, 1f, 0f, 1f);
        }
        else
        {
            float g = (Health / maxHealth * 2);
            indicator.GetComponent<Renderer>().material.color = new Color(1f, g, 0f, 1f);
        }



        //print(Health);
        //Enemy Looking and Moving in the Player's Direction
        Vector3 Direction = Player.transform.position - transform.position;
        
        transform.LookAt(new Vector3(Player.transform.position.x, transform.position.y, Player.transform.position.z));
        Direction.Normalize();
        Direction.y = 0;
        Movement = Direction;
        if(isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                ResetInvincible();
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            AlterHealth(-5, false);
            KnockBack(2);
        }

        if (other.tag == "PlayerBody")
        {
            PlayerController playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            playerController.AlterHealth(-5, true);
        }

    }

    private void FixedUpdate()
    {
        //Move the Enemy
        MoveCharacter(Movement);
    }

    void MoveCharacter(Vector3 Direction)
    {
        //Enemy Movement Speed
        Rigid.AddForce(Direction * Acceleration * 100 * Time.deltaTime);

        Rigid.velocity = new Vector3(
            Mathf.Clamp(Rigid.velocity.x, -MaxSpeed, MaxSpeed), Rigid.velocity.y,
            Mathf.Clamp(Rigid.velocity.z, -MaxSpeed, MaxSpeed));
    }

    public void AlterHealth(float altHP, bool inv)
    {
        if (!isInvincible)
        {
            Health += altHP;
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
        Vector3 dir = transform.position - Player.transform.position;
        dir.y = 0;
        dir.Normalize();
        Rigid.velocity = dir * intensity;
    }
}
