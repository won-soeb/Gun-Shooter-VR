using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    public GameObject wall;
    public GameObject monsterGenerator;
    public Button startButton;
    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            wall.SetActive(false);
            monsterGenerator.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
