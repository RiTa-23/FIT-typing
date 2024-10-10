using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    bool pushed = false;//�{�^��������ς�

    //�Z���N�g���[�h
    [SerializeField] GameObject selectPanel;

    //�V�ѕ�
    [SerializeField] GameObject HowToPlayPanel;
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backButton;
    [SerializeField] TextMeshProUGUI pageNumText;
    int page = 1;

    //�����I��
    [SerializeField] GameObject LongSelectPanel;

    [SerializeField] AudioSource SE_dicision;
    // Start is called before the first frame update
    void Start()
    {
        selectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndButton();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            PushSizeChangeButton();
        }

        //�V�ѕ�
        if (howToPlay)
        {
            switch (page)
            {
                case 1: page1.SetActive(true); backButton.SetActive(false);
                    nextButton.SetActive(true);page2.SetActive(false); break;
                case 2: page2.SetActive(true); nextButton.SetActive(false);
                    backButton.SetActive(true); page1.SetActive(false); break;
            }
            pageNumText.text = page.ToString() + "/2";
        }
        else
        {
            page1.SetActive(false);
            page2.SetActive(false);
        }
    }


    //�X�^�[�g�{�^��
    public void pushStartButton()
    {
        SE_dicision.Play();
        selectPanel.SetActive(true);
    }

    bool howToPlay = false;
    //�V�ѕ��{�^��
    public void pushHowToPlayButton()
    {
        SE_dicision.Play();
        HowToPlayPanel.SetActive(true);
        howToPlay = true;
    }

    //���̃y�[�W
    public void pushNext()
    {
        SE_dicision.Play();
        page++;
    }
    //�O�̃y�[�W
    public void pushBack()
    {
        SE_dicision.Play();
        page--;
    }

    //�^�C�g���{�^��(�V�ѕ�)
    public void pushTitleButton1()
    {
        SE_dicision.Play();
        HowToPlayPanel.SetActive(false);
    }

    public void pushLsButton()
    {
        SE_dicision.Play();
        LongSelectPanel.SetActive(true);
    }

    public void prevButton()
    {
        SE_dicision.Play();
        LongSelectPanel.SetActive(false);
    }

    //�^�C�g���{�^���i�Z���N�g���[�h�j
    public void pushTitleButton()
    {
        SE_dicision.Play();
        selectPanel.SetActive(false);
    }

    //�Z���N�g���[�h
    public void PushWordButton()
    {
        if(!pushed)
        StartCoroutine("wordMode");
    }

    IEnumerator wordMode()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("GameScene");
    }

    public void PushSSButton()
    {
        if (!pushed)
            StartCoroutine("ssMode");
    }

    IEnumerator ssMode()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("ShortSentence");
    }

    public void PushLongNum0()
    {
        if (!pushed)
            StartCoroutine("lsMode");
    }

    IEnumerator lsMode()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence");
    }

    public void PushLongNum1()
    {
        if (!pushed)
            StartCoroutine("lsMode1");
    }

    IEnumerator lsMode1()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 1");
    }

    public void PushLongNum2()
    {
        if (!pushed)
            StartCoroutine("lsMode2");
    }

    IEnumerator lsMode2()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 2");
    }

    public void PushLongNum3()
    {
        if (!pushed)
            StartCoroutine("lsMode3");
    }

    IEnumerator lsMode3()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 3");
    }

    public void PushLongNum4()
    {
        if (!pushed)
            StartCoroutine("lsMode4");
    }

    IEnumerator lsMode4()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 4");
    }

    public void PushLongNum5()
    {
        if (!pushed)
            StartCoroutine("lsMode5");
    }

    IEnumerator lsMode5()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 5");
    }

    public void PushLongNum6()
    {
        if (!pushed)
            StartCoroutine("lsMode6");
    }

    IEnumerator lsMode6()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 6");
    }
    public void PushLongNum7()
    {
        if (!pushed)
            StartCoroutine("lsMode7");
    }

    IEnumerator lsMode7()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 7");
    }

    public void PushLongNum8()
    {
        if (!pushed)
            StartCoroutine("lsMode8");
    }

    IEnumerator lsMode8()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("LongSentence 8");
    }

    public void PushEnButton()
    {
        if (!pushed)
            StartCoroutine("enMode");
    }

    IEnumerator enMode()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("EnglishSentence");
    }

    public void PushSizeChangeButton()
    {
        SE_dicision.Play();
        switch (Screen.width)
        {
            case 1920:
                Screen.SetResolution(1344, 756, false);
                break;
            case 1344:
                Screen.SetResolution(960, 540, false);
                break;
            case 960:
                Screen.SetResolution(570, 324, false);
                break;
            case 570:
                Screen.SetResolution(1920, 1080, true);
                break;

        }
    }

    public void EndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

}
