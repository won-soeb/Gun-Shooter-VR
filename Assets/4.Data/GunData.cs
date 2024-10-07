using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "GunData", order = 1)]
public class GunData : ScriptableObject
{
    public string gunName;
    public float damage;
    public int maxAmmo;
    public int currentAmmo;
}
