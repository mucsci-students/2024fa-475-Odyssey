using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject harpoonPrefab;
    public Transform firepoint;
    public float fireForce = 20f;

    public void Fire(){

        GameObject Harpoon = Instantiate(harpoonPrefab, firepoint.position, firepoint.rotation);
        Harpoon.GetComponent<Rigidbody2D>().AddForce(firepoint.up * fireForce,ForceMode2D.Impulse);
    }
}
