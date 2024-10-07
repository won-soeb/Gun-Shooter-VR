using UnityEngine;

public class ItemManager : SingletonManager<ItemManager>
{
    public GameObject recoveryItem;
    public GameObject pistolItem;
    public GameObject shotGunItem;
    public GameObject machineGunItem;

    public GameObject CreateItem()
    {
        int num = Random.Range(0, 10);
        switch (num)
        {
            case 0:
                return Instantiate(recoveryItem);
            case 1:
                return Instantiate(pistolItem);
            case 2:
                return Instantiate(shotGunItem);
            case 3:
                return Instantiate(machineGunItem);
            default:
                return null;
        }
    }
}
