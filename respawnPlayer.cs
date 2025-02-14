using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnPlayer : MonoBehaviour
{
    public Transform spawnPoint; // Точка, где игрок будет респауниться

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spike"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Помещаем игрока в точку респауна
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation; // Если нужно, восстанавливаем ориентацию
    }
}