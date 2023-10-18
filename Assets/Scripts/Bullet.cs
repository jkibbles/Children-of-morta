using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damageAmount = 10;
    Player playerScript;

    private void Start()
    {
        playerScript= GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            playerScript.TakeDamage(damageAmount);
            Destroy(gameObject);
        }

    }
}
