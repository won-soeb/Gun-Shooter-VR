using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public abstract void Attack(bool isAttack);
    public abstract void Damage(float damage);
    public abstract void Die();
}
