using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    Player playerScript;
    GameObject player;
    public float speed;
    public string[] elementTags;  // Array of tags

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

    private float timer = 0f;
    public float triggerInterval = 1f;  // Interval in seconds
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        RandomEnemy();
        if (isRanged)
            InvokeRepeating("Shoot", 0f, 1f / fireRate);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isPlayer)
            DetectPlayer();
        if (isPlayer && !isRanged)
        {
            timer += Time.deltaTime;

            // Check if the timer exceeds the desired interval
            if (timer >= triggerInterval)
            {
                // Call the function you want to trigger
                Attack();

                // Reset the timer
                timer = 0f;
            }
        }
            
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
        else if (isPlayer && !isRanged)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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

    public void RandomEnemy()
    {
        isRanged = UnityEngine.Random.Range(0, 2) == 1;
        // Get a random index within the range of the tags array
        int randomIndex = UnityEngine.Random.Range(0, elementTags.Length);

        // Assign the random tag to the GameObject
        gameObject.tag = elementTags[randomIndex];

        if (gameObject.tag == "Fire")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
        }
        else if(gameObject.tag == "Water")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (gameObject.tag == "Earth")
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.4f, 0.2f);
        }
        else if (gameObject.tag == "Lightning")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }

    }
}
