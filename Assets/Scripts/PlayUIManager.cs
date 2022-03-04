using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

// PlayScene�� UI�� ����, �ֹ��� ��ġ�ϸ� �׶����� ��ũ��Ʈ�� Ȱ��ȭ�ȴ�
// ������, UI�� ���� ������ �������� �����ϴµ�, �̸� �����ϴ� ��Ʈ��Ʈ���� �Բ� Ȱ��ȭ �Ǵ� ���� �´°����� ���δ�
public class PlayUIManager : MonoBehaviour
{
    // ��ŷ�� ���õ� Rank ��ü ����
    [SerializeField]
    private Rank rank;

    // ���� ��� �ð� ǥ��
    [SerializeField]
    private Text timer;

    [SerializeField]
    private GameManager gameManager;

    // ���� �÷��� �� ����� ���� Ŭ����ÿ� ǥ���� �� ����
    [SerializeField]
    private GameObject[] stars;

    // ���� Ŭ���� �� ǥ���� �� ���� ����
    [SerializeField]
    private GameObject[] starsInClearPanel;

    // ���� ���� ǥ�� (��ǳ��)
    [SerializeField]
    private Text customerOrderText;

    // ������ �����ð� ǥ�� ��
    [SerializeField]
    private Scrollbar orderTimer;

    // ���� �Ϸ�� ������ �г�
    [SerializeField]
    private GameObject clearPanel;

    // ����� ���� Ŭ���� ������ŭ ���� �Ϸ�� ������ ��
    [SerializeField]
    private GameObject starSet;

    // ������ �� ��ŷ ����� ���� ���� �Է� �г�
    [SerializeField]
    private GameObject rewardPanel;

    // ��ŷ ������ �ȳ��ϴ� �г�
    [SerializeField]
    private GameObject rankPanel;

    // Ŭ���� �ð� ǥ��
    [SerializeField]
    private Text endTimeText;

    // Ŭ������ ���� ��
    [SerializeField]
    private Text customerCNT;

    // ������ �� ��ŷ�� ���� �÷��̾� �̸� �Է�
    [SerializeField]
    private InputField playerName;

    // ������ �� ��ŷ�� ���� ��ȭ��ȣ �Է�
    [SerializeField]
    private InputField phoneNumber;

    // ��ŷ�� ǥ���� �÷��̾� ���� ���̾ƿ�
    [SerializeField]
    private GameObject playerInfo;

    // ��ŷ�� ǥ���� �÷��̾� ���̾ƿ��� ��ġ�� UI
    [SerializeField]
    private GameObject playerInfoParent;

    // ������ ������ ���� ���� �Է� �гη� ���� ��ư
    [SerializeField]
    private GameObject rewardRegisterButton;

    // ��Ḧ �״� ��ư
    [SerializeField]
    private GameObject placementButton;

    // ���� �� ���� ����/�Ҹ����� ���� ��ǳ�� ǥ�ø� ���� �����ϴ� ��ü��
    public GameObject customerOrder;
    public GameObject customerSat;
    public GameObject customerUnSat;
    public GameObject customerWrong;

    // ����ε� �޴��� ��Ḧ �ѱ���� �ٲ��ֱ� ���� ��ųʸ�
    Dictionary<string, string> menuE2K;
    Dictionary<string, string> ingreE2K;

    [SerializeField]
    private Text DebugLog;

    // Ŭ����� ������ �г��� active���� �ƴ��� Ȯ���ϴ� ����
    // ���� �̷� ������ �� �ʿ� ���� Ŭ�����г��� �����ͼ� ���¸� Ȯ���ϴ°� ������ ����
    private bool isClearPanelOn = false;

    // ���� �÷���Ÿ��
    public float time = 0;
    
    // ���� �ʱ�ȭ
    // ���� Awake()���� �ؾ��� �ʱ�ȭ���� Ȯ���Ұ�
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

    // PlayerUIManager ��ũ��Ʈ�� Ȱ��ȭ �Ǹ� ���ο� ������ �����
    // ������ �� ��ũ��Ʈ�� UI�� �ٷ�� �������� �ۼ��� ��ũ��Ʈ�ε� �� �ڵ尡 ���⿡ �ִ°� ���� �ʴ°� ����
    private void OnEnable()
    {
        gameManager.NewOrder();
    }

    // ���� ��Ȳ�� ���� ���� UI���� �����Ŵ
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

    // ����� ���� Ŭ����� ǥ�õǴ� �� ���� �߰�
    /*
        PlayerUIManager ��ũ��Ʈ�� UI�� �����ϴµ�, �ٸ� ��ü �� ��ũ��Ʈ�� ���� ������ �� UI�� �����ϱ⵵�ϰ� 
        �ٸ� ��ü �� ��ũ��Ʈ���� PlayerUIManager �� ������ UI�� �����ϱ⵵ �ϴµ�,
        UI�� �ٲٴ� ��� �� �ΰ��� ����� �Բ� ����ϴ� ���� ������, �ϳ��� �����ϴ� ���� ������ Ȯ�� �ʿ�
    */
    public void AddStar()
    {
        stars[gameManager.specialOrderClearCnt - 1].SetActive(true);
    }

    // Ŭ���� �г��� ���� ����� ������ Ŭ���� �� ��ŭ Ȱ��ȭ
    private void SetStarsInClearPanel()
    {
        starSet.SetActive(true);

        for (int i = 0; i < gameManager.specialOrderClearCnt; i++)
        {
            starsInClearPanel[i].SetActive(true);
        }
    }

    // ���� ���� �ؽ�Ʈ ��ǳ���� ǥ���ϱ�
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

    // Ȩ��ư Ŭ���� �� �̵�
    public void OnHomeButton()
    {
        SceneManager.LoadScene("GameMenuScene");
    }

    // ��ŷ��ư Ŭ���� ��ŷ ������ �ҷ�����
    // ���������� ����� ��ŷ ǥ�� ���̾ƿ��� �����µڿ�
    // ��ŷ �����͸� ������ ��ġ
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

    // �絵�� ��ư Ŭ���� �� �ٽ� ����
    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ������ ��ư Ŭ���� ������ UI Ȱ��ȭ
    public void OnRewardButton()
    {
        clearPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    // ������ ��ư Ŭ���� Ŭ���� �г� Ȱ��ȭ
    public void OnExitButton()
    {
        rewardPanel.SetActive(false);
        rankPanel.SetActive(false);
        clearPanel.SetActive(true);
    }

    // ������ ���� �Է��� ���� ��� (������ ����) ��ư�� ������ ����
    // �Է��� ������ �����Ϳ� �����ϰ� ��ŷâ ǥ��
    // onRankingButton()���� �ڵ� ���� �� ������ ����
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