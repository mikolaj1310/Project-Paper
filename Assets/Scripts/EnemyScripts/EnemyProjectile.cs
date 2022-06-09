using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    GameObject player;
    float life;
    float speed;
    Vector3 direction;
    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 10f;
        life = 10f;

        playerPos = player.transform.position;
        direction = playerPos - transform.position;
        direction.y = 0;
        direction.Normalize();

        float angle = AngleBetweenTwoPoints(playerPos, transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 90, 0));
        transform.localScale *= 2;

    }

    // Update is called once per frame
    void Update()
    {
        if (life > 0)
        {
            life -= Time.deltaTime;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
            Destroy(this.gameObject);
    }
    float AngleBetweenTwoPoints(Vector3 b, Vector3 a)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
