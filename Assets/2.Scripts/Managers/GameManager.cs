using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playUI, scoreUI, optionUI;
    public Slider hpGauge, speedUpGauge, loadingGauge;
    public Text weapon, ammo, scoreText, bestScoreText;
    public Button retryButton, quitButton;
    public int score = 0, bestScore = 0;
    public float skeletonHp = 5, skeletonAttack = 0f;
    public float flymonsterMaxHp = 10;
    public float flymonsterAttack = 5f;
    public float flymonsterGenScoreRate = 100;
    public GameObject[] guns;
    public GameObject[] interactors;
    public GameObject flymonster;
    private bool isPlayerDead = false;
    private int checkScore = 0;

    public static GameManager Instance = new GameManager();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        //최고점수 불러오기
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
        //Option창을 활성화하여 시간이 정지되었을 수 있으므로 재설정
        Time.timeScale = 1f;

        scoreUI.SetActive(false);
        loadingGauge.gameObject.SetActive(false);
        retryButton.onClick.AddListener(() =>
        {
            //SceneManager.LoadScene("MainScene");
            StartCoroutine(LoadScene());
        });
        quitButton.onClick.AddListener(() =>
        {
            GameExit();
        });
    }
    //Update UI
    #region UpdateUI
    public void UpdateHpbar(float hp)
    {
        hpGauge.value = hp;
    }
    public void UpdateSpeedUpbar(float speed)
    {
        speedUpGauge.value = speed;
    }
    public void UpdateWeaponName(int weapon)
    {
        this.weapon.text = player.GetComponent<Player>().gunData[weapon].gunName;
    }
    public void UpdateAmmo(int ammo)
    {
        GunData ammoData = player.GetComponent<Player>().gunData[ammo];
        this.ammo.text = string.Format("{0}/{1}", ammoData.currentAmmo, ammoData.maxAmmo);
    }
    public void UpdateScore(int score)
    {
        this.score += score;
        checkScore += score;
        //점수 당 flymonster등장 체크
        if (checkScore >= flymonsterGenScoreRate)
        {
            StartCoroutine(CreateFlyMonster());
            checkScore = 0;
        }
        //최고 점수 갱신
        if (bestScore <= this.score)
        {
            bestScore = this.score;
        }
        try
        {
            scoreText.text = this.score.ToString();
        }
        catch (Exception)
        {

        }
    }
    private void Update()
    {
        //Test
        //오른손 컨트롤러의 B버튼을 누르면
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            GameExit();
        }
        //OptionUI
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isPlayerDead) return;
            //옵션창이 활성화되어있다면
            if (optionUI.activeInHierarchy)
            {
                //창 닫기
                CloseOptionUI();
            }
            else
            {
                //창 열기
                OpenOptionUI();
            }
        }
    }
    public void OpenOptionUI()
    {
        SetInteractors(false);
        optionUI.SetActive(true);
        Invoke("Pause", 0.1f);//메소드를 지연시키지 않으면 UI측 Collider가 반응하지 않는 문제가 있음
    }
    public void CloseOptionUI()
    {
        SetInteractors(true);
        optionUI.SetActive(false);
        Time.timeScale = 1f;
    }
    private void Pause()
    {
        Time.timeScale = 0f;
    }
    #endregion
    public void GameOver()
    {
        isPlayerDead = true;
        //최고점수 저장
        PlayerPrefs.SetInt("BestScore", bestScore);
        bestScoreText.text = bestScore.ToString();

        playUI.SetActive(false);
        scoreUI.SetActive(true);
        SetInteractors(false);
    }
    private void SetInteractors(bool isActive)
    {
        player.GetComponent<Player>().enabled = isActive;
        player.GetComponent<MoveController>().enabled = isActive;

        foreach (var interactor in interactors)
        {
            interactor.SetActive(isActive);
        }

        if (!isActive)
        {
            //모든 총 오브젝트를 비활성화
            foreach (var gun in guns)
            {
                gun.SetActive(isActive);
            }
        }
        else
        {
            //Player가 설정한 총 오브젝트만 활성화
            guns[Player.weaponNum].SetActive(isActive);
        }
    }
    public IEnumerator LoadScene()
    {
        AsyncOperation loadAsyncScene = SceneManager.LoadSceneAsync("MainScene");
        loadingGauge.gameObject.SetActive(true);

        while (!loadAsyncScene.isDone)
        {
            loadingGauge.value = loadAsyncScene.progress;
            //Debug.Log("Loading : " + loadAsyncScene.progress * 100);
            yield return null;
        }
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중지
        EditorApplication.isPlaying = false;
#else
        // 빌드된 애플리케이션 종료
        Application.Quit();
#endif
    }
    private IEnumerator CreateFlyMonster()
    {
        yield return new WaitForSeconds(1);
        Instantiate(flymonster, player.transform.position, Quaternion.identity);
    }
}
