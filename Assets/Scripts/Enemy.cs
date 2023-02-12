using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private GameObject particlePrefab;
    public bool IsHitting { get; private set; }

    void Start()
    {
        _animator = GetComponent<Animator>(); 
    }
    public void StartHitting()
    {
        _animator.SetTrigger("Punch");
        IsHitting = true;
        StartCoroutine(KillCharacter());
    }

    IEnumerator KillCharacter()
    {
        yield return new WaitForSeconds(2);
        GameplayManager.Instance.KillJoiner();
        StartCoroutine(KillCharacter());
    }

    public void Die()
    {
        StopAllCoroutines();
        GameObject particle = Instantiate(particlePrefab, transform.position, transform.rotation);
        Destroy(particle, 1.5f);
        gameObject.SetActive(false);
    }
}
