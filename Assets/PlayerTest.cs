using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log("Damage al�nd�: " + amount);
        //if (health <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Oyuncu �ld�!");
        // Destroy(gameObject); vs.
    }
}
