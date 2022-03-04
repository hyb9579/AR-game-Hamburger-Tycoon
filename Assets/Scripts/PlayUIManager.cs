using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

// PlayScene의 UI를 관리, 주방을 배치하면 그때부터 스크립트가 활성화된다
// 하지만, UI는 씬이 시작한 순간부터 존재하는데, 이를 관리하는 스트립트또한 함께 활성화 되는 것이 맞는것으로 보인다
public class PlayUIManager : MonoBehaviour
{
    // 랭킹에 관련된 Rank 객체 참조
    [SerializeField]
    private Rank rank;

    // 게임 경과 시간 표시
    [SerializeField]
    private Text timer;

    [SerializeField]
    private GameManager gameManager;

    // 게임 플레이 중 스페셜 오더 클리어시에 표시할 별 참조
    [SerializeField]
    private GameObject[] stars;

    // 게임 클리어 후 표시할 별 묶음 참조
    [SerializeField]
    private GameObject[] starsInClearPanel;

    // 오더 내용 표시 (말풍선)
    [SerializeField]
    private Text customerOrderText;

    // 오더별 남은시간 표시 바
    [SerializeField]
    private Scrollbar orderTimer;

    // 게임 완료시 나오는 패널
    [SerializeField]
    private GameObject clearPanel;

    // 스페셜 오더 클리어 갯수만큼 게임 완료시 나오는 별
    [SerializeField]
    private GameObject starSet;

    // 리워드 및 랭킹 등록을 위한 정보 입력 패널
    [SerializeField]
    private GameObject rewardPanel;

    // 랭킹 정보를 안내하는 패널
    [SerializeField]
    private GameObject rankPanel;

    // 클리어 시간 표시
    [SerializeField]
    private Text endTimeText;

    // 클리어한 오더 수
    [SerializeField]
    private Text customerCNT;

    // 리워드 및 랭킹을 위한 플레이어 이름 입력
    [SerializeField]
    private InputField playerName;

    // 리워드 및 랭킹을 위한 전화번호 입력
    [SerializeField]
    private InputField phoneNumber;

    // 랭킹에 표시할 플레이어 정보 레이아웃
    [SerializeField]
    private GameObject playerInfo;

    // 랭킹에 표시할 플레이어 레이아웃을 배치할 UI
    [SerializeField]
    private GameObject playerInfoParent;

    // 리워드 수령을 위한 정보 입력 패널로 가는 버튼
    [SerializeField]
    private GameObject rewardRegisterButton;

    // 재료를 쌓는 버튼
    [SerializeField]
    private GameObject placementButton;

    // 오더 및 오더 만족/불만족에 따른 말풍선 표시를 위해 참조하는 객체들
    public GameObject customerOrder;
    public GameObject customerSat;
    public GameObject customerUnSat;
    public GameObject customerWrong;

    // 영어로된 메뉴와 재료를 한국어로 바꿔주기 위한 딕셔너리
    Dictionary<string, string> menuE2K;
    Dictionary<string, string> ingreE2K;

    [SerializeField]
    private Text DebugLog;

    // 클리어시 나오는 패널이 active인지 아닌지 확인하는 변수
    // 따로 이런 변수를 둘 필요 없이 클리어패널을 가져와서 상태를 확인하는게 맞을것 같다
    private bool isClearPanelOn = false;

    // 게임 플레이타임
    public float time = 0;
    
    // 변수 초기화
    // 굳이 Awake()에서 해야할 초기화인지 확인할것
    private void Awake()
    {
        menuE2K = new Dictionary<string, string>()
        {
            {"Hamburger", "햄버거" },
            {"CheeseBurger", "치즈버거" },
            {"DoubleCheeseBurger", "더블치즈버거" },
            {"TripleCheeseBurger", "트리플치즈버거" },
            {"McChicken", "맥치킨" },
            {"McSpicy", "맥스파이스" },
            {"BulgogiBurger", "불고기버거" },
            {"DoubleBulgogiBurger", "더블불고기버거" },
            {"EggBulgogiBurger", "에그불고기버거" },
            {"BaconTomatoDelux", "베이컨토마토디럭스" }
        };

        ingreE2K = new Dictionary<string, string>()
        {
            { "Patty", "패티" },
            { "BulgogiPatty", "불고기패티" },
            { "ChickenPatty", "치킨패티" },
            { "Ketchup", "케첩" },
            { "Cheese", "치즈" },
            { "Lettuces", "상추" },
            { "PurpleOnions", "양파" },
            { "Tomatoes", "토마토" },
            { "Bacons", "베이컨" },
            { "FriedEgg", "에그프라이" }
        };
    }

    // PlayerUIManager 스크립트가 활성화 되면 새로운 오더를 만든다
    // 하지만 이 스크립트는 UI만 다루는 목적으로 작성한 스크립트인데 이 코드가 여기에 있는건 맞지 않는것 같다
    private void OnEnable()
    {
        gameManager.NewOrder();
    }

    // 게임 상황에 따라 여러 UI들을 변경시킴
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

    // 스페셜 오더 클리어시 표시되는 별 갯수 추가
    /*
        PlayerUIManager 스크립트는 UI를 관리하는데, 다른 객체 및 스크립트로 부터 정보를 얻어서 UI를 변경하기도하고 
        다른 객체 및 스크립트에서 PlayerUIManager 를 참조해 UI를 변경하기도 하는데,
        UI를 바꾸는 방법 위 두가지 방법을 함께 사용하는 것이 좋은지, 하나로 통일하는 것이 좋은지 확인 필요
    */
    public void AddStar()
    {
        stars[gameManager.specialOrderClearCnt - 1].SetActive(true);
    }

    // 클리어 패널의 별을 스페셜 오더를 클리어 한 만큼 활성화
    private void SetStarsInClearPanel()
    {
        starSet.SetActive(true);

        for (int i = 0; i < gameManager.specialOrderClearCnt; i++)
        {
            starsInClearPanel[i].SetActive(true);
        }
    }

    // 오더 내용 텍스트 말풍선에 표시하기
    public void OrderInCol()
    {
        string inText;

        Debug.Log(GameManager.order.isSpecialOrder);
        if (!GameManager.order.isSpecialOrder)
        {
            inText = $"{menuE2K[GameManager.order.name]} 주세요.";
        }
        else
        {
            inText = $"{menuE2K[GameManager.order.name]} 주세요.\n근데 {ingreE2K[GameManager.order.addedIngredient]} 하나 추가하고 {ingreE2K[GameManager.order.subedIngredient]} 하나 뺄게요 :P";
        }

        customerOrderText.text = inText;
    }

    // 홈버튼 클릭시 씬 이동
    public void OnHomeButton()
    {
        SceneManager.LoadScene("GameMenuScene");
    }

    // 랭킹버튼 클릭시 랭킹 데이터 불러오기
    // 프리팹으로 저장된 랭킹 표시 레이아웃을 가져온뒤에
    // 랭킹 데이터를 가져와 배치
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

    // 재도전 버튼 클릭시 씬 다시 시작
    public void OnRetryButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 리워드 버튼 클릭스 리워드 UI 활성화
    public void OnRewardButton()
    {
        clearPanel.SetActive(false);
        rewardPanel.SetActive(true);
    }

    // 나가기 버튼 클릭시 클리어 패널 활성화
    public void OnExitButton()
    {
        rewardPanel.SetActive(false);
        rankPanel.SetActive(false);
        clearPanel.SetActive(true);
    }

    // 리워드 정보 입력후 정보 등록 (리워드 수령) 버튼을 누르면 실행
    // 입력한 정보를 데이터에 저장하고 랭킹창 표시
    // onRankingButton()으로 코드 줄일 수 있을것 같다
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