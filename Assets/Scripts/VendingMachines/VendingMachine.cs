using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachine : MonoBehaviour
{
    string machineType;
    bool buying = false;
    GameObject UIElement;

    // Start is called before the first frame update
    void Start()
    {
        machineType = transform.tag;
        buying = false;
        UIElement = GameObject.FindGameObjectWithTag("UIUpgrade");
        UIElement.gameObject.GetComponent<Text>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            //print("buying");
            buying = true;
        }
        else
            buying = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (buying)
        {
            if (other.tag == "PlayerBody")
            {
                PlayerController pcont = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                PlayerCombat pcomb = GameObject.FindGameObjectWithTag("Weapon").GetComponent<PlayerCombat>();
                WeaponCollider weapColl = GameObject.FindGameObjectWithTag("Bic").GetComponent<WeaponCollider>();
                if (pcont.GetCash() >= 10)
                {
                    if (machineType == "BlueVendingMachine") //Blue - ammo
                    {
                        pcont.AlterCash(-10);
                        pcomb.SetMaxAmmo(10);
                        weapColl.baseAmmoGain += 0.1f;
                        print("Change");
                        UIElement.GetComponent<Text>().text = "Ink Ammo Increased";
                        StartCoroutine(ShowText());
                    }
                    if (machineType == "OrangeVendingMachine") //Orange - dash CD
                    {
                        pcont.AlterCash(-10);
                        pcont.SetDashCooldown(-0.15f);
                        UIElement.GetComponent<Text>().text = "Dodge Cooldown Decreased";
                        StartCoroutine(ShowText());
                    }
                    if (machineType == "GreenVendingMachine") //Green - Health
                    {
                        if (pcont.GetHealth() < pcont.GetMaxHealth())
                        {
                            pcont.AlterCash(-10);
                            pcont.AlterHealth(10, false);
                        }
                        UIElement.GetComponent<Text>().text = "Health Replenished";
                        StartCoroutine(ShowText());
                    }
                    if (machineType == "PurpleVendingMachine") //Purple - Attack Speed
                    {
                        pcont.AlterCash(-10);
                        weapColl.baseDamage += 0.2f;
                        pcomb.SetSwingSpeed(0.98f);
                        UIElement.GetComponent<Text>().text = "Attack Increased";
                        StartCoroutine(ShowText());
                    }
                    buying = false;
                }
            }
        }
    }

    IEnumerator ShowText()
    {
        UIElement.gameObject.GetComponent<Text>().enabled = true;
        yield return new WaitForSeconds(1);
        UIElement.gameObject.GetComponent<Text>().enabled = false;
    }
}
