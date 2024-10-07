using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonManager<GameManager>
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

    private void Start()
    {
        //�ְ����� �ҷ�����
        if (PlayerPrefs.HasKey("BestScore"))
        {
            bestScore = PlayerPrefs.GetInt("BestScore");
        }
        //Optionâ�� Ȱ��ȭ�Ͽ� �ð��� �����Ǿ��� �� �����Ƿ� �缳��
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
        //���� �� flymonster���� üũ
        if (checkScore >= flymonsterGenScoreRate)
        {
            StartCoroutine(CreateFlyMonster());
            checkScore = 0;
        }
        //�ְ� ���� ����
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
        //������ ��Ʈ�ѷ��� B��ư�� ������
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            GameExit();
        }
        //OptionUI
        if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isPlayerDead) return;
            //�ɼ�â�� Ȱ��ȭ�Ǿ��ִٸ�
            if (optionUI.activeInHierarchy)
            {
                //â �ݱ�
                CloseOptionUI();
            }
            else
            {
                //â ����
                OpenOptionUI();
            }
        }
    }
    public void OpenOptionUI()
    {
        SetInteractors(false);
        optionUI.SetActive(true);
        Invoke("Pause", 0.1f);//�޼ҵ带 ������Ű�� ������ UI�� Collider�� �������� �ʴ� ������ ����
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
        //�ְ����� ����
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
            //��� �� ������Ʈ�� ��Ȱ��ȭ
            foreach (var gun in guns)
            {
                gun.SetActive(isActive);
            }
        }
        else
        {
            //Player�� ������ �� ������Ʈ�� Ȱ��ȭ
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
        // ����Ƽ �����Ϳ��� ���� ����
        EditorApplication.isPlaying = false;
#else
        // ����� ���ø����̼� ����
        Application.Quit();
#endif
    }
    private IEnumerator CreateFlyMonster()
    {
        yield return new WaitForSeconds(1);
        Instantiate(flymonster, player.transform.position, Quaternion.identity);
    }
}
