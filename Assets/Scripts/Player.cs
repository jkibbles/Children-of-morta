using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp = 1000;
    public int currentHp = 1000;
    public HealthBar healthBar;
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
