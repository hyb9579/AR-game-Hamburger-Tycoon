using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayUIManager : MonoBehaviour
{
    [SerializeField]
    private Rank rank;

    [SerializeField]
    private Text timer;

    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private GameObject[] stars;

    [SerializeField]
    private GameObject[] starsInClearPanel;

    [SerializeField]
    private Text customerOrderText;

    [SerializeField]
    private Scrollbar orderTimer;

    [SerializeField]
    private GameObject clearPanel;

    [SerializeField]
    private GameObject starSet;

    [SerializeField]
    private GameObject rewardPanel;

    [SerializeField]
    private GameObject rankPanel;

    [SerializeField]
    private Text endTimeText;

    [SerializeField]
    private Text customerCNT;

    [SerializeField]
    private InputField playerName;

    [SerializeField]
    private InputField phoneNumber;

    [SerializeField]
    private GameObject playerInfo;

    [SerializeField]
    private GameObject playerInfoParent;

    [SerializeField]
    private GameObject rewardRegisterButton;

    [SerializeField]
    private GameObject placementButton;

    public GameObject customerOrder;

    public GameObject customerSat;

    public GameObject customerUnSat;

    public GameObject customerWrong;

    Dictionary<string, string> menuE2K;

    Dictionary<string, string> ingreE2K;

    [SerializeField]
    private Text DebugLog;

    private bool isClearPanelOn = false;

    public float time = 0;
    private void Awake()
    {
        menuE2K = new Dictionary<string, string>()
        {
            {"Hamburger", "�ܹ���" },
            {"CheeseBurger", "ġ�����" },
            {"DoubleCheeseBurger", "����ġ�����" },
            {"TripleCheeseBurger", "Ʈ����ġ�����" },
            {"McChicken", "��ġŲ" },
            {"McSpicy", "�ƽ����̽�" },
            {"BulgogiBurger", "�Ұ�����" },
            {"DoubleBulgogiBurger", "����Ұ�����" },
            {"EggBulgogiBurger", "���׺Ұ�����" },
            {"BaconTomatoDelux", "�������丶��𷰽�" }
        };

        ingreE2K = new Dictionary<string, string>()
        {
            { "Patty", "��Ƽ" },
            { "BulgogiPatty", "�Ұ����Ƽ" },
            { "ChickenPatty", "ġŲ��Ƽ" },
            { "Ketchup", "��ø" },
            { "Cheese", "ġ��" },
            { "Lettuces", "����" },
            { "PurpleOnions", "����" },
            { "Tomatoes", "�丶��" },
            { "Bacons", "������" },
            { "FriedEgg", "����������" }
        };
    }

    private void OnEnable()
    {
        gameManager.NewOrder();
    }

    private void Update()
    {
        if (gameManager.isEnd)
        {
            placementButton.SetActive(false);

            if (!isClearPanelOn)
            {
                if(gameManager.specialOrderClearCnt < gameManager.specialOrederCnt || (gameManager.unsatisfiedCunstomerCnt > 0))
                {
                    rewardRegisterButton.SetActive(false);
                }

                clearPanel.SetActive(true);

                SetStarsInClearPanel();

                endTimeText.text = time.ToString();

                customerCNT.text = (gameManager.normalOrderClearCnt + gameManager.specialOrderClearCnt).ToString();

                isClearPanelOn = true;
            }

            return;
        }

        time += Time.deltaTime;

        int minutes = (int)(time / 60);
        float seconds = (time % 60);

        timer.text = $"{minutes}:{seconds}";

        orderTimer.size = GameManager.order.timer / gameManager.orderTime;
    }

    public void AddStar()
    {
        stars[gameManager.specialOrderClearCnt - 1].SetActive(true);
    }

    private void SetStarsInClearPanel()
    {
        starSet.SetActive(true);

        for (int i = 0; i < gameManager.specialOrderClearCnt; i++)
        {
            starsInClearPanel[i].SetActive(true);
        }
    }

    public void OrderInCol()
    {
        string inText;

        Debug.Log(GameManager.order.isSpecialOrder);
        if (!GameManager.order.isSpecialOrder)
        {
            inText = $"{menuE2K[GameManager.order.name]} �ּ���.";
        }
        else
        {
            inText = $"{menuE2K[GameManager.order.name]} �ּ���.\n�ٵ� {ingreE2K[GameManager.order.addedIngredient]} �ϳ� �߰��ϰ� {ingreE2K[GameManager.order.subedIngredient]} �ϳ� ���Կ� :P";
        }

        customerOrderText.text = inText;
    }

    public void OnHomeButton()
    {
        SceneManager.LoadScene("GameMenuScene");
    }

    public void OnRankingButton()
    {
        clearPanel.SetActive(false);
        rankPanel.SetActive(true);

        int i = 1;

        foreach (RankInfo tmpRankInfo in Rank.rankInfo)
        {
            GameObject playerInfoUI = Instantiate(playerInfo);

            Text[] tmpTXT = playerInfoUI.GetComponentsInChildren<Text>();

            playerInfoUI.transform.parent = playerInfoParent.transform;


            tmpTXT[0].text += tmpRankInfo.name + " ( " + tmpRankInfo.phoneNumber.Substring(tmpRankInfo.phoneNumber.Length - 4) + " )";
            tmpTXT[1].text += tmpRankInfo.playTime;
            tmpTXT[2].text = i.ToString();

            playerInfoUI = null;
            tmpTXT = null;
            i++;
        }
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnRewardButton()
    {
        clearPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    public void OnExitButton()
    {
        rewardPanel.SetActive(false);
        rankPanel.SetActive(false);
        clearPanel.SetActive(true);
    }

    public void OnRewardRegister()
    {
        string playerName = this.playerName.text;
        string phoneNumber = this.phoneNumber.text;

        RankInfo tmpRank = new RankInfo();
        tmpRank.name = playerName;
        tmpRank.phoneNumber = phoneNumber;
        tmpRank.playTime = time;

        Rank.rankInfo.Add(tmpRank);

        rank.Save();

        rewardPanel.SetActive(false);
        rankPanel.SetActive(true);

        int i = 1;

        foreach (RankInfo tmpRankInfo in Rank.rankInfo)
        {
            GameObject playerInfoUI = Instantiate(playerInfo);

            Text[] tmpTXT = playerInfoUI.GetComponentsInChildren<Text>();

            playerInfoUI.transform.parent = playerInfoParent.transform;


            tmpTXT[0].text += tmpRankInfo.name + " ( " + tmpRankInfo.phoneNumber.Substring(tmpRankInfo.phoneNumber.Length - 4) + " )";
            tmpTXT[1].text += tmpRankInfo.playTime;
            tmpTXT[2].text = i.ToString();

            playerInfoUI = null;
            tmpTXT = null;
            i++;
        }
    }
}