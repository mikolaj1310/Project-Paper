using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    GameObject player;
    float life;
    float speed;
    Vector3 mousePos;
    Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 20f;
        life = 0.25f;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition + player.transform.forward * 10);

        mousePos.x += Random.Range(-0.5f, 0.5f);
        mousePos.z += Random.Range(-0.5f, 0.5f);
        direction = mousePos - player.transform.position;
        direction.y = 0;
        direction.Normalize();

        float angle = AngleBetweenTwoPoints(transform.position, mousePos);
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle + 180, 0));
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
