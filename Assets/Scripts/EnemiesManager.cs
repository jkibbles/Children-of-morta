using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    public GameObject enemy;
    public Vector2 spawnArea;
    public float spawnTimer;
    float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            SpawnEnemy();
            timer = spawnTimer;
        }
    }

    private void SpawnEnemy()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 position = RandomPosition();

        position += player.transform.position;
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = position;
    }

    private Vector3 RandomPosition()
    {
        Vector3 position = new Vector3();

        float f = UnityEngine.Random.value > .5f ? -1f : 1f;
        if (UnityEngine.Random.value > .5f)
        {
            position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
            position.y = spawnArea.y * f;
        }
        else
        {
            position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
            position.x = spawnArea.x * f;
        }

        position.z = 0;

        return position;
    }
}
