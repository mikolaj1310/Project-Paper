using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisibility : MonoBehaviour
{
    //public GameObject Wall1;
    //public GameObject Wall2;
    //
    GameObject player;
    //public CapsuleCollider Object;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (var wall in walls)
        {            
            var wallColors = wall.GetComponent<MeshRenderer>().materials;

            foreach (var wallColor in wallColors)
            {
                if (wallColor.color.a == 0.3f)
                    wallColor.color = new Color(wallColor.color.r, wallColor.color.g, wallColor.color.b, 1f);
            }
            wall.gameObject.GetComponent<Renderer>().materials = wallColors;

            //children
            var children = wall.transform.childCount;
            for (int i = 0; i < children; i++)
            {
                if (wall.transform.GetChild(i).GetComponent<Renderer>() == true)
                {
                    var childMats = wall.transform.GetChild(i).GetComponent<Renderer>().materials;
                    foreach (var mat in childMats)
                    {
                        if (mat.color.a == 0.3f)
                            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1f);
                    }
                    wall.transform.GetChild(i).GetComponent<Renderer>().materials = childMats;
                }
            }
        }
        Collider[] colliders = Physics.OverlapSphere(player.transform.position, 7);
        foreach(var collider in colliders)
        {
            if (collider.transform.position.z < transform.position.z)
            {
                if (collider.tag == "Wall")
                {
                    var wallMats = collider.gameObject.GetComponent<Renderer>().materials;

                    foreach (var mat in wallMats)
                    {
                        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f);
                    }

                    collider.gameObject.GetComponent<Renderer>().materials = wallMats;

                    var children = collider.gameObject.transform.childCount;

                    for (int i = 0; i < children; i++)
                    {
                        if (collider.gameObject.transform.GetChild(i).GetComponent<Renderer>() == true)
                        {
                            var childMats = collider.gameObject.transform.GetChild(i).GetComponent<Renderer>().materials;
                            foreach (var mat in childMats)
                            {
                                if (mat.color.a == 1f)
                                    mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.3f);
                            }
                            collider.gameObject.transform.GetChild(i).GetComponent<Renderer>().materials = childMats;
                            collider.gameObject.GetComponent<Renderer>().materials = wallMats;
                        }
                    }
                }
            }
        }
    }
}
