using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    Player playerScript;
    GameObject player;
    public float speed;

    private bool isPlayer = false;
    private float distance;

    public int maxHp = 1000;
    public int currentHp = 1000;
    public HealthBar healthBar;
    public float barSpeed = 10f;  // The speed at which the bar follows the target

    public int damage = 1;

    public bool isRanged;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    public float fireRange = 10f;
    public float fireSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        isRanged = UnityEngine.Random.Range(0, 2) == 1;
        if (isRanged)
            InvokeRepeating("Shoot", 0f, 1f / fireRate);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPlayer)
            DetectPlayer();
        if (isPlayer && !isRanged)
            Attack();
        if(isRanged)
            rb.velocity = Vector2.zero;
    }
    private void FixedUpdate()
    {
        if (!isPlayer && !isRanged)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            gameObject.GetComponent<Rigidbody2D>().velocity = direction * speed;
        }

        if (healthBar.gameObject != null)
        {
            Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            desiredPosition.y += .7f;
            Vector3 smoothedPosition = Vector3.Lerp(healthBar.gameObject.transform.position, desiredPosition, barSpeed);
            healthBar.gameObject.transform.position = smoothedPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            isPlayer = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            isPlayer = false;
        }
    }

    private void Attack()
    {
        if (playerScript == null)
        {
            playerScript = player.GetComponent<Player>();
        }

        playerScript.TakeDamage(damage);
    }

    void DetectPlayer()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    private void Shoot()
    {
        // Check if player is in range
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < fireRange)  // Adjust this range as needed
        {
            rb.isKinematic = true;
            isRanged = true;
            // Instantiate a projectile
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            // Calculate direction to the player
            Vector2 direction = (player.transform.position - firePoint.position).normalized;

            // Set the projectile's velocity based on the direction and speed
            projectile.GetComponent<Rigidbody2D>().velocity = direction * fireSpeed;// Adjust speed as needed

            // Destroy the projectile after a certain time to prevent clutter
            Destroy(projectile, 3f); // Adjust destroy time as needed
        }
        else
        {
            rb.isKinematic = false;
            isRanged = false;
        }

    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Debug.Log("Ded");
        }
        healthBar.SetState(currentHp, maxHp);
    }
}
