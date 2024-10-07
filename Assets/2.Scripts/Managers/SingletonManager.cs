using UnityEngine;

public class SingletonManager<T> : MonoBehaviour where T : SingletonManager<T>
{
    private static T _instance;

    // �̱��� �ν��Ͻ��� �������� ������Ƽ
    public static T Instance
    {
        get
        {
            // �ν��Ͻ��� ������ �����Ͽ� ��ȯ
            if (_instance == null)
            {
                GameObject singletonObject = new GameObject(typeof(T).Name);
                _instance = singletonObject.AddComponent<T>();
                DontDestroyOnLoad(singletonObject);
            }
            return _instance;
        }
    }

    // �ʱ�ȭ
    protected virtual void Awake()
    {
        // �ν��Ͻ��� �̹� ������ �ߺ� ���� ����
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // ���� �ν��Ͻ��� ����
            _instance = this as T;
        }
    }
}
