using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFightStarter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameplayManager.Instance.StartFinalFight();
        gameObject.SetActive(false);
    }
}
