using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using unityroom.Api;

public class TypingManager : MonoBehaviour
{
    //�����L���O�{�[�h�ԍ��w��
    [SerializeField] int bordNum;

    //�ŏ��̖��o��ς݂�(se��炷�����f���邽��)
    bool started = false;

    //�Q�[���I��������
    bool finish = false;

    //�X�y�[�X�L�[��������
    bool spacePushed = false;

    //�p�����[�h��
    public bool english;

    //�������[�h��
    public bool longMode;

    //�������ԍ�
    public int long_num;

    //�������s�ڂ�
    int line_num = 0;

    //�o�ߎ���
    float deltaTime;

    //�����t�@�C���擾
    [SerializeField] AudioSource SE_keypush;
    [SerializeField] AudioSource SE_miss;
    [SerializeField] AudioSource SE_complete;
    [SerializeField] AudioSource SE_start;
    [SerializeField] AudioSource SE_finish;
    [SerializeField] AudioSource SE_result;
    [SerializeField] AudioSource SE_score;
    [SerializeField] AudioSource SE_Result;
    [SerializeField] AudioSource SE_dicision;

    //�X�R�A�p
    float correctKeyType;//�������ł����Ō���
    float missType;//�~�X�^�C�v��
    public float timeLimit;//�c�莞��
    private float nowTimeLimit;//�ω�����c�莞��
    float startTime;
    float finishTime;

    //�X�y�[�X�L�[�������Ă��������e�L�X�g�擾
    [SerializeField] TextMeshProUGUI Text_pleaseSpace;
    [SerializeField] TextMeshProUGUI Text_JP_pleaseSpace;

    //�X�R�A�e�L�X�g�擾
    [SerializeField] TextMeshProUGUI Text_correctType;
    [SerializeField] TextMeshProUGUI Text_missType;
    [SerializeField] TextMeshProUGUI Text_timeLimit;

    //���U���g�e�L�X�g�擾
    [SerializeField] TextMeshProUGUI Text_r_correct;
    [SerializeField] TextMeshProUGUI Text_r_miss;
    [SerializeField] TextMeshProUGUI Text_r_score;
    [SerializeField] TextMeshProUGUI Text_r_kps;
    [SerializeField] TextMeshProUGUI Text_r_accuracyRate;

    //���U���g�I�u�W�F�N�g�擾
    [SerializeField] GameObject ob_score;
    [SerializeField] GameObject ob_correct;
    [SerializeField] GameObject ob_miss;
    [SerializeField] GameObject ob_kps;
    [SerializeField] GameObject ob_accuracyRate;
    [SerializeField] GameObject ob_titleButton;
    [SerializeField] GameObject ob_retryButton;

    [SerializeField] GameObject ob_result;

    //�^�C�s���O����e�L�X�g�擾
    [SerializeField] TextMeshProUGUI fText; //�ӂ肪�ȃe�L�X�g
    [SerializeField] TextMeshProUGUI qText; //���e�L�X�g
    [SerializeField] TextMeshProUGUI rText;//���[�}���e�L�X�g

    //�^�C�s���O����e�L�X�g�I�u�W�F�N�g�擾
    [SerializeField] GameObject ob_fText;
    [SerializeField] GameObject ob_qText;

    //�X�^�[�g�e�L�X�g
    [SerializeField] TextMeshProUGUI Text_StartFinish;

    //�e�L�X�g�f�[�^��ǂݍ���
    [SerializeField] TextAsset furigana;
    [SerializeField] TextAsset question;

    //�e�L�X�g�f�[�^���i�[���邽�߂̃��X�g
    private List<string> _fList = new List<string>();
    private List<string> _qList = new List<string>();


    //�ꎞ�I�ɖ����i�[����z��
    private string f_tmp;
    private string q_tmp;
    private string r_tmp;

    //���ԍ�
    private int q_num;

    //���̉������ڂ�
    private int r_num;

    //�����Ă��邩�ǂ���
    bool isCorrect;

    //Shift�L�[������Ă��邩
    bool isShift = false;


    //���̃X�N���v�g
    private ChangeDictionary cd;//���͑Ή�
    private L_qr_countList qr_List;//�������J�E���g���X�g

    //�������̃J�E���g���X�g�i�[
    private List<int> qr_tmpList = new List<int>();

    //���[�}�����Ђ炪�Ȉꕶ�����ŃX���C�X "si","n","bu","n"
    private List<string> _romSliceList = new List<string>();
    //�t���K�i�̉������ڂ� {0,0,1,2,2,3}
    private List<int> _furiCountList = new List<int>();
    //���ł��Ă���ӂ肪�Ȉꕶ���̃��[�}���̉������ڂ� {0,1,0,0,1,0}
    private List<int> _romNumList = new List<int>();

    //���͂����L�[��KeyCode�擾
    KeyCode keycode;

    // Start is called before the first frame update
    void Start()
    {
        cd = GetComponent<ChangeDictionary>();

        if(longMode)
        {
            qr_List = GetComponent<L_qr_countList>();
            SetqrList();

        }
        

        //������
        missType = 0;
        correctKeyType = 0;
        nowTimeLimit = timeLimit;
        Text_timeLimit.text = nowTimeLimit.ToString();


        //�e�L�X�g�f�[�^�����X�g�ɓ����
        SetList();
    }
    bool gameStatus = false;

    // Update is called once per frame
    void Update()
    {
        Text_correctType.text = correctKeyType.ToString();
        Text_missType.text = missType.ToString();

        //esc�ōēǂݍ��݁A�Q�[�����s�O�Ȃ�^�C�g����ǂݍ���
        if (Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.LeftAlt)||Input.GetKeyDown(KeyCode.RightAlt))
        {
            if (spacePushed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadScene("Title");
            }
        }


        //�X�y�[�X�L�[�������ꂽ��X�^�[�g
        if (!gameStatus && !finish && !spacePushed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spacePushed = true;
                //�����ɃR���[�`���i�X�^�[�g�j
                StartCoroutine("SpacebarPushToStart");
            }

        }

            //���͂��ꂽ�Ƃ��ɔ��f
            if (Input.anyKeyDown && gameStatus && !finish)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) ||
                        Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) ||
                        Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                {
                    return;
                }

                isCorrect = false;

                int furiCount = _furiCountList[r_num];

                foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(code))
                    {
                        Debug.Log(code);
                        keycode = code;
                        break;
                    }
                }

                //���S�ɂ����Ă��琳��
                if (GetKeyReturn() == r_tmp[r_num].ToString())
                {
                    PerfectCorrect();
                }
                else if (r_tmp[r_num].ToString() != "A" && r_tmp[r_num].ToString() != "B" && r_tmp[r_num].ToString() != "C"
                    && r_tmp[r_num].ToString() != "D" && r_tmp[r_num].ToString() != "E" && r_tmp[r_num].ToString() != "F"
                    && r_tmp[r_num].ToString() != "G" && r_tmp[r_num].ToString() != "H" && r_tmp[r_num].ToString() != "I"
                    && r_tmp[r_num].ToString() != "J" && r_tmp[r_num].ToString() != "K" && r_tmp[r_num].ToString() != "L"
                    && r_tmp[r_num].ToString() != "M" && r_tmp[r_num].ToString() != "N" && r_tmp[r_num].ToString() != "O"
                    && r_tmp[r_num].ToString() != "P" && r_tmp[r_num].ToString() != "Q" && r_tmp[r_num].ToString() != "R"
                    && r_tmp[r_num].ToString() != "S" && r_tmp[r_num].ToString() != "T" && r_tmp[r_num].ToString() != "U"
                    && r_tmp[r_num].ToString() != "V" && r_tmp[r_num].ToString() != "W" && r_tmp[r_num].ToString() != "X"
                    && r_tmp[r_num].ToString() != "Y" && r_tmp[r_num].ToString() != "Z" && r_tmp[r_num].ToString() != " ")
                    if (Input.GetKeyDown(r_tmp[r_num].ToString()))
                    {
                        PerfectCorrect();
                    }


                    else
                    {

                        //���O��n��ł��Ă���A���݂�̓��͒���n�ȊO����͂����ꍇ
                        if (!Input.GetKeyDown("n") && f_tmp[furiCount].ToString() == "��" && r_tmp[r_num - 1].ToString() == "n")
                        {
                            //�����������΁A���n�ɂ��Ď��̕�����
                            if (f_tmp.Length - 1 > _furiCountList[r_num])
                            {
                                int nextMojiNum = _furiCountList[r_num] + 1;
                                string nextMoji = f_tmp[nextMojiNum].ToString();
                                int listNum = cd.dic[nextMoji].Count;

                                //�񕶎��p
                                //�����񕶎���܂ŕ���������ꍇ
                                if (f_tmp.Length - 1 > nextMojiNum)
                                {
                                    string nextMoji2 = f_tmp[nextMojiNum].ToString() + f_tmp[nextMojiNum + 1].ToString();
                                    if (cd.dic.ContainsKey(nextMoji2))
                                    {
                                        int listNum2 = cd.dic[nextMoji2].Count;
                                        nCheckNextMoji(nextMoji2, listNum2);
                                    }
                                }

                                //�������̕���������A����Ɠ���������ł����Ȃ�ok
                                if (f_tmp.Length - 1 >= nextMojiNum)
                                {

                                    if (!isCorrect)
                                    {
                                        nCheckNextMoji(nextMoji, listNum);
                                    }

                                }
                            }



                        }

                        //"��"�̌�⌟��
                        if (f_tmp[furiCount].ToString() == "��" && f_tmp.Length - 1 > _furiCountList[r_num])
                        {
                            int nextMojiNum = _furiCountList[r_num] + 1;
                            string nextMoji = f_tmp[nextMojiNum].ToString();

                            for (int i = 0; i < cd.dic[nextMoji].Count; i++)
                            {
                                string a = cd.dic[nextMoji][i][0].ToString();
                                if (Input.GetKeyDown(a))
                                {
                                    _romSliceList[_furiCountList[r_num]] = a;
                                    _romSliceList[_furiCountList[r_num + 1]] = cd.dic[nextMoji][i];
                                    //�ύX�����烊�X�g���ēx�\��������
                                    ReCreateList(_romSliceList);
                                    r_tmp = string.Join("", GetRomSliceListWithoutSkip());

                                    //true�ɂ���
                                    isCorrect = true;
                                    //����
                                    Correct();
                                }
                            }
                        }



                        //�����ɑΉ�������͕��@�����邩�m�F
                        //���遨����
                        //�Ȃ������s

                        //���ǂ̂ӂ肪�Ȃ�ł��Ȃ��Ƃ����Ȃ��̂��擾
                        string currentFuri = f_tmp[furiCount].ToString();

                        if (furiCount < f_tmp.Length - 1)
                        {
                            //�Q�������l��������⌟��
                            string addNextMoji = f_tmp[furiCount].ToString() + f_tmp[furiCount + 1].ToString();
                            CheckIrregularType(addNextMoji, furiCount, false);
                        }
                        if (!isCorrect)
                        {
                            //�P�����Ō�⌟��
                            string moji = f_tmp[furiCount].ToString();
                            CheckIrregularType(moji, furiCount, true);
                        }



                    }
                //�s�����̏ꍇ
                if (!isCorrect)
                {
                    //���s
                    Miss();
                }
            }
    }

    void nCheckNextMoji(string nextMoji, int listNum)
    {

        for (int i = 0; i < listNum; i++)
        {
            string b = cd.dic[nextMoji][i][0].ToString();
            if (Input.GetKeyDown(b) || GetKeyReturn() == b)
            {

                string a = cd.dic[nextMoji][i];

                //����a��0�Ԗڂ�n,y,�ꉹ�łȂ����ok
                if (a[0] != 'n' && a[0] != 'y' && a[0] != 'a' && a[0] != 'i' && a[0] != 'u' && a[0] != 'e' && a[0] != 'o')
                {
                    //romSliceList�ɑ}���ƕ\���̔��f
                    _romSliceList[_furiCountList[r_num]] = "n";
                    _romSliceList[_furiCountList[r_num] + 1] = a;


                    //�ύX�����烊�X�g���ēx�\��������
                    ReCreateList(_romSliceList);
                    r_tmp = string.Join("", GetRomSliceListWithoutSkip());

                    //true�ɂ���
                    isCorrect = true;
                    //����
                    Correct();

                    //�Ō�̕����ɐ���������
                    if (r_num >= r_tmp.Length)
                    {
                        //���ύX
                        AskQ();
                    }
                    break;
                }
            }
        }

    }

    void CheckIrregularType(string currentFuri, int furiCount, bool addSmallMoji)
    {

        if (cd.dic.ContainsKey(currentFuri))
        {
            List<string> stringList = cd.dic[currentFuri];
            Debug.Log(string.Join(",", stringList));


            for (int i = 0; i < stringList.Count; i++)
            {
                string rom = stringList[i];
                int romNum = _romNumList[r_num];

                bool preCheck = true;

                for (int j = 0; j < romNum; j++)
                {
                    if (rom[j] != _romSliceList[furiCount][j])
                    {
                        preCheck = false;
                    }
                }

                if (preCheck && Input.GetKeyDown(rom[romNum].ToString()))
                {
                    _romSliceList[furiCount] = rom;
                    r_tmp = string.Join("", GetRomSliceListWithoutSkip());

                    ReCreateList(_romSliceList);


                    //true�ɂ���
                    isCorrect = true;

                    if (addSmallMoji)
                    {
                        AddSmallMoji();
                    }

                    //����
                    Correct();

                    //�Ō�̕����ɐ���������
                    if (r_num >= r_tmp.Length)
                    {
                        //���ύX
                        AskQ();
                    }
                    break;
                }

            }
        }
    }

    void PerfectCorrect()
    {
        //true�ɂ���
        isCorrect = true;
        //����
        Correct();

        //�Ō�̕����ɐ���������
        if (r_num >= r_tmp.Length)
        {
            //���ύX
            AskQ();
        }

    }
    void SetList()
    {
        string[] _fArray = furigana.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        _fList.AddRange(_fArray);

        string[] _qArray = question.text.Split(new char[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        _qList.AddRange(_qArray);
    }

    void SetqrList()
    {
        int qr_ListNum = qr_List.qr_countList[long_num].Count;
        var qrList = qr_List.qr_countList[long_num];
        for(int i = 0; i < qr_ListNum; i++)
        {
            for(int j=0; j < qrList[i]; j++)
            {
                qr_tmpList.Add(i);
            }
        }
        Debug.Log(string.Join(",", qr_tmpList));
    }

    //�_��ȓ��͂������Ƃ��Ɏ��̕������������Ȃ珬������}������
    void AddSmallMoji()
    {
        int nextMojiNum = _furiCountList[r_num] + 1;

        //�������̕������Ȃ���Ώ��������Ȃ�
        if (f_tmp.Length - 1 < nextMojiNum)
        {
            return;
        }

        string nextMoji = f_tmp[nextMojiNum].ToString();
        string a = cd.dic[nextMoji][0];

        //����a��0�Ԗڂ�x�ł�l�ł��Ȃ���Ώ��������Ȃ�
        if (a[0] != 'x' && a[0] != 'l')
        {
            return;
        }

        //romSliceList�ɑ}���ƕ\���̔��f
        _romSliceList.Insert(nextMojiNum, a);
        //SKIP���폜����
        _romSliceList.RemoveAt(nextMojiNum + 1);

        //�ύX�����烊�X�g���ēx�\��������
        ReCreateList(_romSliceList);
        r_tmp = string.Join("", GetRomSliceListWithoutSkip());


    }

    //���[�}���쐬���Ă����֐�
    bool SKIP = false;
    void CreateRomSliceList(string moji)
    {
        _romSliceList.Clear();
        _furiCountList.Clear();
        _romNumList.Clear();



        for (int i = 0; i < moji.Length; i++)
        {

            string a = cd.dic[moji[i].ToString()][0];

            if (SKIP)
            {
                a = "SKIP";
                SKIP = false;
            }
            else if (moji[i].ToString() == "��" && i + 1 < moji.Length && moji[i + 1].ToString() != "�I" && moji[i + 1].ToString() != "?")
            {
                a = cd.dic[moji[i + 1].ToString()][0][0].ToString();
            }
            else if (i + 1 < moji.Length)
            {
                //���̕������܂߂Ď�������T��
                string addNextMoji = moji[i].ToString() + moji[i + 1].ToString();
                if (cd.dic.ContainsKey(addNextMoji))
                {
                    a = cd.dic[addNextMoji][0];
                    //SKIP��L���ɂ���
                    SKIP = true;
                }

            }

            _romSliceList.Add(a);

            if (a == "SKIP")
            {
                continue;
            }
            for (int j = 0; j < a.Length; j++)
            {
                _furiCountList.Add(i);
                _romNumList.Add(j);
            }
        }
        Debug.Log(string.Join(",", _romSliceList));
    }

    void ReCreateList(List<string> romList)
    {
        _furiCountList.Clear();
        _romNumList.Clear();

        for (int i = 0; i < romList.Count; i++)
        {
            string a = romList[i];
            if (a == "SKIP")
            {
                continue;
            }

            for (int j = 0; j < a.Length; j++)
            {
                _furiCountList.Add(i);
                _romNumList.Add(j);
            }
        }
        //Debug.Log(string.Join(",", _romSliceList));
    }

    //SKIP�Ȃ��̕\���������邽�߂�List����蒼��
    List<string> GetRomSliceListWithoutSkip()
    {
        List<string> returnList = new List<string>();
        foreach (string rom in _romSliceList)
        {
            if (rom == "SKIP")
            {
                continue;
            }
            returnList.Add(rom);
        }
        return returnList;
    }

    string GetKeyReturn()
    {
        //Shift�L�[�������Ă��邩
        isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        if (isShift)
        {
            //shift�����Ȃ�����͂̋L��
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                return "!";
            }
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                return "?";
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                return "%";
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                return "&";
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                return "(";
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                return ")";
            }
            //�A���t�@�x�b�g�啶��
            if (keycode == KeyCode.A || keycode == KeyCode.B || keycode == KeyCode.C
                || keycode == KeyCode.D || keycode == KeyCode.E || keycode == KeyCode.F
                || keycode == KeyCode.G || keycode == KeyCode.H || keycode == KeyCode.I
                || keycode == KeyCode.J || keycode == KeyCode.K || keycode == KeyCode.L
                || keycode == KeyCode.M || keycode == KeyCode.N || keycode == KeyCode.O
                || keycode == KeyCode.P || keycode == KeyCode.Q || keycode == KeyCode.R
                || keycode == KeyCode.S || keycode == KeyCode.T || keycode == KeyCode.U
                || keycode == KeyCode.V || keycode == KeyCode.W || keycode == KeyCode.X
                || keycode == KeyCode.Y || keycode == KeyCode.Z)
            {
                return keycode.ToString();
            }

        }

        //�X�y�[�X
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return " ";
        }

        //���{��z��ɍ��킹��
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            return "[";
        }
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            return "]";
        }

        return "";

    }

    //���o�肵�Ă����֐�
    void AskQ()
    {
        if (started)
        {
            if (longMode)
            {
                StartCoroutine("Finish");
                return;
            }
            SE_complete.Play();
        }
        else
            started = true;



        //0�����ڂɖ߂�
        r_num = 0;

        //���ԍ��������_���ɐ���
        if (!longMode)
            q_num = UnityEngine.Random.Range(0, _qList.Count);
        else
            q_num = long_num;

        //�ꎞ�I�Ɋi�[
        f_tmp = _fList[q_num];
        q_tmp = _qList[q_num];

        //���[�}�����쐬���ĂȂ���
        CreateRomSliceList(f_tmp);

        r_tmp = string.Join("", GetRomSliceListWithoutSkip());

        //�e�L�X�g��ύX
        fText.text = f_tmp;
        qText.text = q_tmp;
        rText.text = r_tmp;
    }

    //�������̏���
    void Correct()
    {

        Debug.Log("�����I");
        SE_keypush.Play();
        correctKeyType++;

        r_num++;

        if (longMode)
        {
            if(r_num >= r_tmp.Length )
            {
                finishTime = Time.time;
                Debug.Log("�v���I��");
                Debug.Log(finishTime);
                return;
            }

            //�ӂ肪��
            if (_furiCountList[r_num] < 15)
                fText.text = "<color=#7B7B7B>" + f_tmp.Substring(0, _furiCountList[r_num]) + "</color>" +
                            f_tmp.Substring(_furiCountList[r_num]);
            else
            {
                fText.text = "<color=#7B7B7B>" + f_tmp.Substring(_furiCountList[r_num] - 15, 15) + "</color>" +
                        f_tmp.Substring(_furiCountList[r_num]);
            }
                

        }

        if (r_tmp.Length > r_num)
        {
            //�����X�y�[�X�̎�
            if (r_tmp[r_num] == ' ')
            {
                if (longMode)
                {
                    if (r_num < 20)
                        rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                            "_" + r_tmp.Substring(r_num + 1);
                    else
                        rText.text = "<color=#7B7B7B>" + r_tmp.Substring(r_num - 20, 20) + "</color>" +
                        "_" + r_tmp.Substring(r_num + 1);

                }
                else
                {
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                                        "_" + r_tmp.Substring(r_num + 1);
                }

            }
            //�X�y�[�X�Ȃ��̎�
            else
            {
                if (longMode)
                {
                    if (r_num < 20)
                        rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" + r_tmp.Substring(r_num);
                    else
                        rText.text = "<color=#7B7B7B>" + r_tmp.Substring(r_num - 20, 20) + "</color>" + r_tmp.Substring(r_num);
                }
                else
                {
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                        r_tmp.Substring(r_num);
                }
            }
        }

        

        //�������[�h�̎��A���̃e�L�X�g������������
        if (longMode)
        {
            //�ӂ肪�ȉ������ڂ�
            int furi_num;
            furi_num = _furiCountList[r_num];

            line_num = qr_tmpList[furi_num] / 27;
            if (line_num > 4)
            {
                qText.text = "<color=#7B7B7B>" + q_tmp.Substring((line_num-4) * 27, qr_tmpList[furi_num] - (line_num-4) * 27)
                + "</color>" + q_tmp.Substring(qr_tmpList[furi_num]);
            }
            else
            {
                qText.text = "<color=#7B7B7B>" + q_tmp.Substring(0, qr_tmpList[furi_num])
                + "</color>" + q_tmp.Substring(qr_tmpList[furi_num]);
            }
            
        }

    }

    //�~�X���̏���
    void Miss()
    {
        if (longMode)
        {
            if (_furiCountList[r_num] < 15)
                fText.text = "<color=#7B7B7B>" + f_tmp.Substring(0, _furiCountList[r_num]) + "</color>" +
                        "<color=#FF0000>" + f_tmp.Substring(_furiCountList[r_num], 1) + "</color>" +
                        f_tmp.Substring(_furiCountList[r_num] + 1);
            else
                fText.text = "<color=#7B7B7B>" + f_tmp.Substring(_furiCountList[r_num] - 15, 15) + "</color>" +
                    "<color=#FF0000>" + f_tmp.Substring(_furiCountList[r_num], 1) + "</color>" +
                        f_tmp.Substring(_furiCountList[r_num] + 1);

        }

        if (r_tmp[r_num] == ' ')
        {
            if (longMode)
            {
                if (r_num > 20)
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                        "<color=#FF0000>" + "_" + "</color>" + r_tmp.Substring(r_num + 1);
                else
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(r_num - 20, 20) + "</color>" +
                    "<color=#FF0000>" + "_" + "</color>" + r_tmp.Substring(r_num + 1);
            }
            else
            {
                rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                "<color=#FF0000>" + "_" + "</color>" + r_tmp.Substring(r_num + 1);
            }

        }
        else
        {
            if (longMode)
            {
                if (r_num < 20)
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                            "<color=#FF0000>" + r_tmp.Substring(r_num, 1) + "</color>" + r_tmp.Substring(r_num + 1);
                else
                    rText.text = "<color=#7B7B7B>" + r_tmp.Substring(r_num - 20, 20) + "</color>" +
                        "<color=#FF0000>" + r_tmp.Substring(r_num, 1) + "</color>" + r_tmp.Substring(r_num + 1);

            }
            else
            {
                rText.text = "<color=#7B7B7B>" + r_tmp.Substring(0, r_num) + "</color>" +
                "<color=#FF0000>" + r_tmp.Substring(r_num, 1) + "</color>" + r_tmp.Substring(r_num + 1);
            }

        }

        //�ӂ肪�ȉ������ڂ�
        int furi_num;
        furi_num = _furiCountList[r_num];

        //�������[�h�̎��A���̃e�L�X�g������������
        if (longMode)
        {
            line_num = qr_tmpList[furi_num] / 27;
            if(line_num > 4)
            {
                qText.text = "<color=#7B7B7B>" + q_tmp.Substring((line_num-4) * 27, qr_tmpList[furi_num] - (line_num-4) * 27)
                + "</color>" + "<color=#FF0000>" + q_tmp.Substring(qr_tmpList[furi_num], 1) + "</color>" +
                q_tmp.Substring(qr_tmpList[furi_num] + 1);
            }
            else
            {
                qText.text = "<color=#7B7B7B>" + q_tmp.Substring(0, qr_tmpList[furi_num])
                + "</color>" + "<color=#FF0000>" + q_tmp.Substring(qr_tmpList[furi_num], 1) + "</color>" +
                q_tmp.Substring(qr_tmpList[furi_num] + 1);
            }
            
        }

        Debug.Log("�~�X�I");
        SE_miss.Play();
        missType++;
    }

    IEnumerator SpacebarPushToStart()
    {
        SE_start.Play();

        qText.text = "";
        rText.text = "";
        Text_pleaseSpace.text = "";
        Text_JP_pleaseSpace.text = "";
        Text_StartFinish.text = "�X�^�[�g�I";
        yield return new WaitForSeconds(1f);
        Text_StartFinish.text = "";

        if (longMode)
        {
            Debug.Log("�v���J�n");
            startTime = Time.time;
            Debug.Log(startTime);
        }
        else
        StartCoroutine("TimeLimitManager");

        if (english)
        {
            ob_fText.SetActive(false);
            ob_qText.SetActive(false);
        }

        //���o��
        AskQ();

        gameStatus = true;
    }

    IEnumerator TimeLimitManager()
    {
        for (int i = 0; i < timeLimit; i++)
        {
            yield return new WaitForSeconds(1f);
            nowTimeLimit--;
            Text_timeLimit.text = nowTimeLimit.ToString();
        }
        StartCoroutine("Finish");

    }

    IEnumerator Finish()
    {
           

        finish = true;
        gameStatus = false;
        qText.text = "";
        rText.text = "";
        fText.text = "";
        Text_StartFinish.text = "�I���I";
        SE_finish.Play();

        yield return new WaitForSeconds(1f);
        Text_StartFinish.text = "";

        StartCoroutine("Result");
    }

    IEnumerator Result()
    {
        float accuracyRate = (correctKeyType / (correctKeyType + missType));
        Debug.Log(accuracyRate);
        double ac_3 = Math.Pow(accuracyRate, 3);
        Debug.Log(ac_3);

        //���U���g�\��
        SE_Result.Play();
        ob_result.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //correct,miss,kps,���m���̏��ŊJ��
        Text_r_correct.text = correctKeyType.ToString();
        ob_correct.SetActive(true);
        SE_result.Play();
        yield return new WaitForSeconds(0.3f);

        Text_r_miss.text = missType.ToString();
        ob_miss.SetActive(true);
        SE_result.Play();
        yield return new WaitForSeconds(0.3f);

        float kps;
        if (longMode)
        {
            deltaTime = finishTime - startTime;
            Debug.Log("���v���ԏo��");
            Debug.Log(deltaTime);
            kps = correctKeyType / deltaTime;
        }
        else
        {
            kps = correctKeyType / timeLimit;
        }
        

        Text_r_kps.text = kps.ToString("N1");
        ob_kps.SetActive(true);
        SE_result.Play();
        yield return new WaitForSeconds(0.3f);

        Text_r_accuracyRate.text = (100 * accuracyRate).ToString("N2");
        ob_accuracyRate.SetActive(true);
        SE_result.Play();
        yield return new WaitForSeconds(0.7f);

        float kpm = kps * 60;

        //�X�R�A�\��
        int score=(int)(kpm * ac_3);
        Text_r_score.text = score.ToString();
        //�����L���O�o�^
        if (bordNum != 0)
        {
            UnityroomApiClient.Instance.SendScore(bordNum, score, ScoreboardWriteMode.HighScoreDesc);
        }

        ob_score.SetActive(true);
        SE_score.Play();
        yield return new WaitForSeconds(0.3f);

        //�{�^���\��
        ob_titleButton.SetActive(true);
        ob_retryButton.SetActive(true);

    }

    bool pushed = false;
    public void PushTitleButton()
    {
        if (!pushed)
            StartCoroutine("pushtitle");
    }

    IEnumerator pushtitle()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Title");
    }

    public void PushRetryButton()
    {
        if (!pushed)
            StartCoroutine("pushRetry");
    }

    IEnumerator pushRetry()
    {
        SE_dicision.Play();
        pushed = true;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
