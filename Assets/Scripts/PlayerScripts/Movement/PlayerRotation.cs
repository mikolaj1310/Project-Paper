using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    GameObject player;
    Camera cam;
    public float turnSpeed = 15;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWroldPos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        float angle = AngleBetweenTwoPoints(transform.position, mouseWroldPos);
        transform.rotation = Quaternion.Euler(new Vector3(0, -angle - 90, 0));
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
