using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

/*
하나의 '주문'의 모든 것을 나타내는 클래스
주문 재료, 시간 제한, 스페셜 오더 유무, 오더 만족 판별과 재료 추가 및 제거를 할 수 있는 속성과 메소드들이 있다
*/
public class Order
{
    // 오더의 제한사항 (제한시간, 알맞은 재료) 를 모두 맞춰 해결했으면 default인 true가 유지되고 아니라면 false로 바뀐다
    public bool isSat;

    // 스페셜 오더라면 true
    public bool isSpecialOrder;

    // 주문의 제한시간 설정
    public float timer;
    public string name;
    public List<string> ingredients = new List<string>();

    // 스페셜 오더시 재료를 추가, 제거할때 전달할 재료명
    public string addedIngredient;
    public string subedIngredient;

    // 재료명을 전달받아 오더에서 재료를 추가 제거 하는 함수
    public void addIngredient(string ingredient)
    {
        ingredients.Insert(Random.Range(1, ingredients.Count - 1), ingredient);
    }

    public void subIngredient(string ingredient)
    {
        ingredients.Remove(ingredient);
    }
}

public class GameManager : MonoBehaviour
{
    public bool isEnd = false;

    [SerializeField]
    private PlayUIManager UIManager;

    // 레시피 관련 UI 관리하는 RecipeDisplay 스크립트
    [SerializeField]
    private RecipeDisplay recipeDisplay;

    public float orderTime;

    // 오더는 한번에 하나만 받을 수 있다
    // 즉, 동시에 2개이상 존재할 수 없고, 다양한 위치에서 자주 접근해야 하기 때문에 정적변수 order를 이용한다
    public static Order order = new Order();

    // Start()에서 레시피 데이터에서 받아온 메뉴 종류를 담을 List
    List<string> menu = new List<string>();

    // 일반과 스페셜 오더 개수 설정
    public int normalOrderCnt;
    public int specialOrederCnt;

    // 디버깅 용으로 화면에 표시할 변수
    [SerializeField]
    private Text DebugLog;

    // 게임 경과 시간을 표시
    public Text gameTime;

    // 재료를 버린 횟수
    public int foodTrashCnt;

    // 불만고객 수
    public int unsatisfiedCunstomerCnt;

    // 스페셜 오더 완료 수
    public int specialOrderClearCnt;

    // 일반 오더 완료 수
    public int normalOrderClearCnt;

    // 게임이 시작되기 전에 GameManager의 변수 초기화
    private void Awake()
    {
        // 대기모드 전환 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        unsatisfiedCunstomerCnt = 0;

        specialOrderClearCnt = 0;

        normalOrderClearCnt = 0;

        foodTrashCnt = 0;
    }

    // 게임이 시작되자마자 데이터로 부터 메뉴 이름들을 가져옴
    void Start()
    {
        foreach (string name in DataLoader.recipes.Keys)
        {
            menu.Add(name);
        }
    }
    
    // 오더의 남은 시간을 계산하고 제한시간내에 해결하지 못하면 불만 오더로 바꿈
    private void Update()
    {
        if (isEnd)
        {
            return;
        }

        //손님 한 명 제한시간
        order.timer -= Time.deltaTime;

        if (order.timer < 0)
        {
            order.timer = 0;

            if (order.isSat)
            {
                unsatisfiedCunstomerCnt++;
                order.isSat = false;
            }
        }
    }

    // 새로운 오더를 만듦
    public void NewOrder()
    {
        // 남아있는 주문이 있는지 확인
        if (normalOrderCnt + specialOrederCnt <= 0)
        {
            DebugLog.text = "모든 주문 끝!";

            isEnd = true;

            return;
        }

        // 주문 초기화시 필요한 값들 설정
        order.timer = orderTime;
        order.isSat = true;

        // 일반 오더와 스페셜 오더를 결정해주는 분기
        if (Random.Range(0, normalOrderCnt + specialOrederCnt) > specialOrederCnt - 1)
        {
            order.isSpecialOrder = false;

            SelectRecipe();

            normalOrderCnt--;
        }
        else
        {
            order.isSpecialOrder = true;

            SelectRecipe();

            // 스페셜 오더면 임의의 재료를 하나를 추가하고 하나를 제거한다
            order.subedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.subIngredient(order.subedIngredient);

            order.addedIngredient = order.ingredients[Random.Range(1, order.ingredients.Count - 1)];
            order.addIngredient(order.addedIngredient);

            specialOrederCnt--;

            DebugLog.text = "SpecialOrder!";
        }
        
        // 생성된 오더에 따라 레시피를 표시
        recipeDisplay.GenRecipe();
        
        // 생성된 오더에 따라 고객 말풍선에 오더 표시
        UIManager.OrderInCol();
    }

    // 오더 이름에 따라 재료 정하기
    private void SelectRecipe()
    {
        order.name = menu[Random.Range(0, menu.Count)];

        order.subedIngredient = null;
        order.addedIngredient = null;
        order.ingredients.Clear();

        foreach (string str in DataLoader.recipes[order.name])
        {
            order.ingredients.Add(str);
            Debug.Log(str);
        }
    }
}