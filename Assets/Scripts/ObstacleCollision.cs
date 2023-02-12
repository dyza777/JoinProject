using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.enabled = false;
            GameplayManager.Instance.GameLost();
        } else if (other.gameObject.CompareTag("Joiner"))
        {
            other.gameObject.GetComponent<Joiner>().Die();
        }
    }
}
