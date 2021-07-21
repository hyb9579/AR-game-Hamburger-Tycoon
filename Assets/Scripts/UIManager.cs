using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using UnityEngine.SceneManagement;

using UnityEngine.Audio;


public class UIManager : MonoBehaviour
{

    public Slider volSlider;
    public AudioSource bgm;

    private float bgmSet = 70f;

    public GameObject tutoPanel1;
    public GameObject tutoPanel2;
    public GameObject tutoPanel3;

    public void SceneChange()
    {
        SceneManager.LoadScene("PlayScene");
    }

     public void MenuSceneChange()
    {
        SceneManager.LoadScene("GameMenuScene");
    }

    public void TutorialSceneChange()
    {
        SceneManager.LoadScene("TutorialScene");
    }


    public void BgmSlider()
    {

        bgm.volume = volSlider.value;

        bgmSet = volSlider.value;
        PlayerPrefs.SetFloat("bgmSet", bgmSet);

    }

    private void Start()
    {
        bgmSet = PlayerPrefs.GetFloat("bgmSet", 70f);
        volSlider.value = bgmSet;
        bgm.volume = volSlider.value;
    }

    private void Update()
    {
        //BgmSlider();
    }

    public void tutoPanel2Open()
    {
        tutoPanel1.SetActive(false);
        tutoPanel2.SetActive(true);
    }

    public void tutoPanel3Open()
    {
        tutoPanel2.SetActive(false);
        tutoPanel3.SetActive(true);
    }

}
