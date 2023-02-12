using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joiner : MonoBehaviour
{
    private PlayerController _playerController;
    private GameObject _player;
    private Animator _animator;
    [SerializeField] private GameObject particlePrefab;

    // Update is called once per frame
    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (GameplayManager.Instance.IsFinalFightStarted) return;
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Joiner"))
        {
            JoinToPlayer();
        }
    }

    public void JoinToPlayer()
    {
        _playerController.enabled = true;
        transform.SetParent(_player.transform);
        gameObject.tag = "Joiner";
    }

    public void RunToEnemy(Vector3 enemyPos)
    {
        transform.LookAt(enemyPos);
        _animator.SetFloat("Speed", 1);
    }

    public void FightEnemy(GameObject enemy)
    {
        transform.LookAt(enemy.transform.position);
        _animator.SetTrigger("Punch");
    }

    public void Die()
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(particle, 1.5f);
        gameObject.SetActive(false);
    }

    public void StartCelebrating()
    {
        _animator.SetTrigger("Win");
    }
}
