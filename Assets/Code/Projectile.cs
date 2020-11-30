using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject prefab;
    private Rigidbody _rb;
    private int damage;
    private int speed = 1;

    // Default Constructor
    public Projectile()
    {
        prefab = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        prefab.AddComponent<Rigidbody>();
        _rb = prefab.GetComponent<Rigidbody>();
    }
    
    public Projectile(Vector3 src)
    {
        prefab = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        prefab.AddComponent<Rigidbody>();
        _rb = prefab.GetComponent<Rigidbody>();
        prefab.transform.position = src;
    }

    public void launch(Vector3 dest)
    {
        _rb.velocity = (dest - prefab.transform.position).normalized * speed;
    }

    private void OnCollisionEnter(Collision other)
    {
        // If Environment Layer Collision
        if (other.gameObject.layer == 8)
        {
            Destroy(prefab);
        }
    }
}
