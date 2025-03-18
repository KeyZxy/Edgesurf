using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pool : MonoBehaviour
{
    private Vector2 lastDir;
    private Rigidbody2D _Rigid;
    public float force;
    private void Start()
    {
        _Rigid = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        lastDir = _Rigid.velocity.normalized;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.CompareTag("Player"))
        {
            PlayerController player = collisionGO.GetComponent<PlayerController>();
           
             if (player.isInvincible)
            {
               return;
            }
            collisionGO.GetComponent<Rigidbody2D>().AddForce(lastDir * force, ForceMode2D.Force);
            
        }
    }
}


    
    

