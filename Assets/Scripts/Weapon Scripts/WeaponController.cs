using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject currentWeapon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        
    }
}

public interface IWeapon
{
    public void PrimaryFire(Transform origin, Vector3 mousePosition);
    public void SecondaryFire(Transform origin, Vector3 mousePosition);

}