using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakObject : MonoBehaviour
{

    public GameObject Fractured;
    public float Shatter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "BiCPen")
            BreakAsset();
        if (other.tag == "Boss Enemy")
            BreakAsset();
        if (other.tag == "Charger Enemy")
            BreakAsset();
    }

    void BreakAsset()
    {
        GameObject Frac = Instantiate(Fractured, transform.position, transform.rotation);
        //foreach(Rigidbody RB in Frac.GetComponentsInChildren<Rigidbody>())
        //{
        //    Vector3 Force = (RB.transform.position = transform.position).normalized * Shatter;
        //    RB.AddForce(Force, ForceMode.Impulse);
        //}
        Destroy(gameObject);
    }

}
