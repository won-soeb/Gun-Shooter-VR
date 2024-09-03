using System;
using UnityEngine;

public class MonsterSword : MonoBehaviour
{
    public Action PlaySound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().Damage(GameManager.Instance.skeletonAttack);
            PlaySound?.Invoke();
        }
    }
}
