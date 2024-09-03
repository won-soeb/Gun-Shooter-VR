using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;
    public Slider loadingBar;
    void Start()
    {
        resumeButton.onClick.AddListener(() => GameManager.Instance.CloseOptionUI());
        restartButton.onClick.AddListener(() => StartCoroutine(GameManager.Instance.LoadScene()));
        quitButton.onClick.AddListener(() => GameManager.Instance.GameExit());
    }
    private void Update()
    {
        loadingBar.value = GameManager.Instance.loadingGauge.value;
    }
}
