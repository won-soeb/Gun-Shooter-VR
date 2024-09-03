using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public GameObject refillItem;
    private GameObject item;
    //�������� �����Ǿ����� Ȯ��
    private bool itemCreated;

    private void Start()
    {
        CreateItem();
    }
    //Update�� �� �ڼ��ϰ� üũ�Ѵ�
    private void Update()
    {
        // �������� ���� �� ���� ����
        if (item == null && !itemCreated)
        {
            CreateItem();
        }
    }
    private void CreateItem()
    {
        item = Instantiate(refillItem, transform.position, transform.rotation);
        item.GetComponent<Item>().rotate = true;
        //Item�� �̺�Ʈ ����
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
