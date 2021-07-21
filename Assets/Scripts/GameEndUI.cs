using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour
{

    public GameObject gameManager;



    [SerializeField]
    private Text playTime;

    [SerializeField]
    private Text satCustomer;


    [SerializeField]
    private GameObject firstStar;
    [SerializeField]
    private GameObject secondStar;
    [SerializeField]
    private GameObject thirdStar;


    [SerializeField]
    private Button replayButton;


    public Image failedImage;
    public void RewardPanelSetting()
    {
        playTime.text = gameManager.GetComponent<GameManager>().gameTime.text;
        satCustomer.text = (20 - (gameManager.GetComponent<GameManager>().unsatisfiedCunstomerCnt)).ToString();

    }

    public void SetStars()
    {
        if(gameManager.GetComponent<GameManager>().specialOrderClearCnt == 1)
        {
            firstStar.SetActive(true);
        }
        else if (gameManager.GetComponent<GameManager>().specialOrderClearCnt ==2)
        {
            secondStar.SetActive(true);

        }
        else if (gameManager.GetComponent<GameManager>().specialOrderClearCnt ==3)
            thirdStar.SetActive(true);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
