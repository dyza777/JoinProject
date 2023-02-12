using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance { get; private set; }
    [SerializeField] private GameObject[] joinersArray;
    [SerializeField] private PlayerController player;
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject LoseScreen;
    public bool CanMoveLeft { get; private set; } = true;
    public bool CanMoveRight { get; private set; } = true;
    public bool IsFinalFightStarted { get; private set; } = false;
    private float fightRange = 10f;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Update()
    {
        if (IsFinalFightStarted)
        {
            HandleFinalFight();
            return;
        }
        SetCanMoveSideways();
    }

    private void HandleFinalFight()
    {
        var closestCharacterPosition = FindClosestCharacterPosition();
        if (Vector3.Distance(closestCharacterPosition, enemy.transform.position) > fightRange)
        {
            MoveCharactersToEnemy();
        } else
        {
            FightEnemy();
        }
    }

    private Vector3 FindClosestCharacterPosition()
    {
        var closestCharacterPosition = player.transform.position;

        for (int i = 0; i < joinersArray.Length; i++)
        {
            if (joinersArray[i].transform.parent != player.gameObject.transform || !joinersArray[i].activeInHierarchy) continue;

            if (Mathf.Abs(joinersArray[i].transform.position.z - enemy.transform.position.z) < Mathf.Abs(closestCharacterPosition.z - enemy.transform.position.z)) {
                closestCharacterPosition = joinersArray[i].transform.position;
            }
        }

        return closestCharacterPosition;
    }

    private void MoveCharactersToEnemy()
    {
        for (int i = 0; i < joinersArray.Length; i++)
        {
            if (joinersArray[i].activeInHierarchy)
            {
                joinersArray[i].GetComponent<Joiner>().RunToEnemy(enemy.transform.position);
            }
        }
    }

    private void FightEnemy()
    {
        if (!enemy.IsHitting)
        {
            enemy.StartHitting();
            for (int i = 0; i < joinersArray.Length; i++)
            {
                StartCoroutine(CountdownToWin());
                if (joinersArray[i].activeInHierarchy)
                {
                    joinersArray[i].GetComponent<Joiner>().FightEnemy(enemy.gameObject);
                }
            }
        }
    }

    public void GameLost()
    {
        player.enabled = false;
        player.gameObject.GetComponent<Animator>().SetTrigger("Die");
        LoseScreen.SetActive(true);
    }

    IEnumerator CountdownToWin()
    {
        player.gameObject.GetComponent<Animator>().SetTrigger("Punch");
        yield return new WaitForSeconds(5);

        enemy.Die();
        for (int i = 2; i < player.transform.childCount; i++)
        {
            if (player.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                player.transform.GetChild(i).GetComponent<Joiner>().StartCelebrating();
            }
        }
        player.GetComponent<Animator>().SetTrigger("Win");
        yield return new WaitForSeconds(1);

        WinScreen.SetActive(true);
    }

    public void KillJoiner()
    {
        int lastJoinerIndex = -1;

        for (int i= joinersArray.Length - 1; i >=0; i--)
        {
            if (joinersArray[i].activeInHierarchy && joinersArray[i].transform.parent == player.transform)
            {
                lastJoinerIndex = i;
                break;
            }
        }

        if (lastJoinerIndex < 0)
        {
            GameLost();
            return;
        }

        joinersArray[lastJoinerIndex].GetComponent<Joiner>().Die();
    }

    public void SetCanMoveSideways()
    {
        for (int i = 0; i < joinersArray.Length; i++)
        {
            if (joinersArray[i].transform.parent == player.gameObject.transform)
            {
                if (joinersArray[i].transform.position.x <= -player.CharacterMovingBorder)
                {
                    CanMoveLeft = false;
                    break;
                }
                else if (joinersArray[i].transform.position.x >= player.CharacterMovingBorder)
                {
                    CanMoveRight = false;
                    break;
                }
                else
                {
                    CanMoveLeft = true;
                    CanMoveRight = true;
                }
            }
        }
    }

    public void StartFinalFight()
    {
        IsFinalFightStarted = true;

        player.enabled = false;

        for (int i = 0; i < joinersArray.Length; i++)
        {
            if (joinersArray[i].transform.parent == player.gameObject.transform)
            {
                joinersArray[i].GetComponent<PlayerController>().enabled = false;
            }
        }
    }
}
