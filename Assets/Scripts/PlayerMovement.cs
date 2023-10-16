using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5;
    public Rigidbody2D rb;
    private Vector2 moveDirection;

    public float dodgeDistance = 3f; // Distance to dodge
    public float dodgeDuration = 0.5f; // Duration of the dodge
    public float dodgeCooldown = 2f; // Cooldown between dodges

    private bool canDodge = true;
    // Update is called once per frame
    void Update()
    {
        ProcessInputs();

        if (Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            StartCoroutine(Dodge());
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;  
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
    }

    private IEnumerator Dodge()
    {
        canDodge = false;

        Vector3 dodgeDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0).normalized;
        Vector3 targetPosition = transform.position + dodgeDirection * dodgeDistance;

        float elapsedTime = 0f;

        while (elapsedTime < dodgeDuration)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / dodgeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;

        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }
}
