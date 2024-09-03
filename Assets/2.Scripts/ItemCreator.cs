using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject refillItem;
    private GameObject item;
    //아이템이 생성되었는지 확인
    private bool itemCreated;

    private void Start()
    {
        CreateItem();
    }
    //Update로 더 자세하게 체크한다
    private void Update()
    {
        // 아이템이 없을 때 새로 생성
        if (item == null && !itemCreated)
        {
            CreateItem();
        }
    }
    private void CreateItem()
    {
        item = Instantiate(refillItem, transform.position, transform.rotation);
        item.GetComponent<Item>().rotate = true;
        //Item의 이벤트 로직
        Item isDestroy = item.GetComponent<Item>();
        isDestroy.OnDestroyed += () =>
        {
            item = null;
            itemCreated = false;
        };
        itemCreated = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (item == null && (other.CompareTag("Item") || other.CompareTag("Player")))
        {
            CreateItem();
        }
    }
}
