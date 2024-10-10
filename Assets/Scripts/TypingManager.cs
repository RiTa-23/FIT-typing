using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using unityroom.Api;

public class TypingManager : MonoBehaviour
{
    //ランキングボード番号指定
    [SerializeField] int bordNum;

    //最初の問題出題済みか(seを鳴らすか判断するため)
    bool started = false;

    //ゲーム終了したか
    bool finish = false;

    //スペースキー押したか
    bool spacePushed = false;

    //英文モードか
    public bool english;

    //長文モードか
    public bool longMode;

    //長文問題番号
    public int long_num;

    //長文何行目か
    int line_num = 0;

    //経過時間
    float deltaTime;

    //音源ファイル取得
    [SerializeField] AudioSource SE_keypush;
    [SerializeField] AudioSource SE_miss;
    [SerializeField] AudioSource SE_complete;
    [SerializeField] AudioSource SE_start;
    [SerializeField] AudioSource SE_finish;
    [SerializeField] AudioSource SE_result;
    [SerializeField] AudioSource SE_score;
    [SerializeField] AudioSource SE_Result;
    [SerializeField] AudioSource SE_dicision;

    //スコア用
    float correctKeyType;//正しく打った打鍵数
    float missType;//ミスタイプ数
    public float timeLimit;//残り時間
    private float nowTimeLimit;//変化する残り時間
    float startTime;
    float finishTime;

    //スペースキーを押してくださいテキスト取得
    [SerializeField] TextMeshProUGUI Text_pleaseSpace;
    [SerializeField] TextMeshProUGUI Text_JP_pleaseSpace;

    //スコアテキスト取得
    [SerializeField] TextMeshProUGUI Text_correctType;
    [SerializeField] TextMeshProUGUI Text_missType;
    [SerializeField] TextMeshProUGUI Text_timeLimit;

    //リザルトテキスト取得
    [SerializeField] TextMeshProUGUI Text_r_correct;
    [SerializeField] TextMeshProUGUI Text_r_miss;
    [SerializeField] TextMeshProUGUI Text_r_score;
    [SerializeField] TextMeshProUGUI Text_r_kps;
    [SerializeField] TextMeshProUGUI Text_r_accuracyRate;

    //リザルトオブジェクト取得
    [SerializeField] GameObject ob_score;
    [SerializeField] GameObject ob_correct;
    [SerializeField] GameObject ob_miss;
    [SerializeField] GameObject ob_kps;
    [SerializeField] GameObject ob_accuracyRate;
    [SerializeField] GameObject ob_titleButton;
    [SerializeField] GameObject ob_retryButton;

    [SerializeField] GameObject ob_result;

    //タイピングお題テキスト取得
    [SerializeField] TextMeshProUGUI fText; //ふりがなテキスト
    [SerializeField] TextMeshProUGUI qText; //問題テキスト
    [SerializeField] TextMeshProUGUI rText;//ローマ字テキスト

    //タイピングお題テキストオブジェクト取得
    [SerializeField] GameObject ob_fText;
    [SerializeField] GameObject ob_qText;

    //スタートテキスト
    [SerializeField] TextMeshProUGUI Text_StartFinish;

    //テキストデータを読み込む
    [SerializeField] TextAsset furigana;
    [SerializeField] TextAsset question;

    //テキストデータを格納するためのリスト
    private List<string> _fList = new List<string>();
    private List<string> _qList = new List<string>();


    //一時的に問題を格納する配列
    private string f_tmp;
    private string q_tmp;
    private string r_tmp;

    //問題番号
    private int q_num;

    //問題の何文字目か
    private int r_num;

    //合っているかどうか
    bool isCorrect;

    //Shiftキー押されているか
    bool isShift = false;


    //他のスクリプト
    private ChangeDictionary cd;//入力対応
    private L_qr_countList qr_List;//長文問題カウントリスト

    //長文問題のカウントリスト格納
    private List<int> qr_tmpList = new List<int>();

    //ローマ字をひらがな一文字ずつでスライス "si","n","bu","n"
    private List<string> _romSliceList = new List<string>();
    //フリガナの何文字目か {0,0,1,2,2,3}
    private List<int> _furiCountList = new List<int>();
    //今打っているふりがな一文字のローマ字の何文字目か {0,1,0,0,1,0}
    private List<int> _romNumList = new List<int>();

    //入力したキーのKeyCode取得
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
        

        //初期化
        missType = 0;
        correctKeyType = 0;
        nowTimeLimit = timeLimit;
        Text_timeLimit.text = nowTimeLimit.ToString();


        //テキストデータをリストに入れる
        SetList();
    }
    bool gameStatus = false;

    // Update is called once per frame
    void Update()
    {
        Text_correctType.text = correctKeyType.ToString();
        Text_missType.text = missType.ToString();

        //escで再読み込み、ゲーム実行前ならタイトルを読み込む
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


        //スペースキーが押されたらスタート
        if (!gameStatus && !finish && !spacePushed)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                spacePushed = true;
                //ここにコルーチン（スタート）
                StartCoroutine("SpacebarPushToStart");
            }

        }

            //入力されたときに判断
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

                //完全にあってたら正解
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

                        //直前にnを打っており、現在んの入力中でn以外を入力した場合
                        if (!Input.GetKeyDown("n") && f_tmp[furiCount].ToString() == "ん" && r_tmp[r_num - 1].ToString() == "n")
                        {
                            //条件が合えば、んをnにして次の文字へ
                            if (f_tmp.Length - 1 > _furiCountList[r_num])
                            {
                                int nextMojiNum = _furiCountList[r_num] + 1;
                                string nextMoji = f_tmp[nextMojiNum].ToString();
                                int listNum = cd.dic[nextMoji].Count;

                                //二文字用
                                //もし二文字先まで文字がある場合
                                if (f_tmp.Length - 1 > nextMojiNum)
                                {
                                    string nextMoji2 = f_tmp[nextMojiNum].ToString() + f_tmp[nextMojiNum + 1].ToString();
                                    if (cd.dic.ContainsKey(nextMoji2))
                                    {
                                        int listNum2 = cd.dic[nextMoji2].Count;
                                        nCheckNextMoji(nextMoji2, listNum2);
                                    }
                                }

                                //もし次の文字があり、それと同じ文字を打ったならok
                                if (f_tmp.Length - 1 >= nextMojiNum)
                                {

                                    if (!isCorrect)
                                    {
                                        nCheckNextMoji(nextMoji, listNum);
                                    }

                                }
                            }



                        }

                        //"っ"の候補検索
                        if (f_tmp[furiCount].ToString() == "っ" && f_tmp.Length - 1 > _furiCountList[r_num])
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
                                    //変更したらリストを再度表示させる
                                    ReCreateList(_romSliceList);
                                    r_tmp = string.Join("", GetRomSliceListWithoutSkip());

                                    //trueにする
                                    isCorrect = true;
                                    //正解
                                    Correct();
                                }
                            }
                        }



                        //辞書に対応する入力方法があるか確認
                        //ある→正解
                        //なし→失敗

                        //今どのふりがなを打たないといけないのか取得
                        string currentFuri = f_tmp[furiCount].ToString();

                        if (furiCount < f_tmp.Length - 1)
                        {
                            //２文字を考慮した候補検索
                            string addNextMoji = f_tmp[furiCount].ToString() + f_tmp[furiCount + 1].ToString();
                            CheckIrregularType(addNextMoji, furiCount, false);
                        }
                        if (!isCorrect)
                        {
                            //１文字で候補検索
                            string moji = f_tmp[furiCount].ToString();
                            CheckIrregularType(moji, furiCount, true);
                        }



                    }
                //不正解の場合
                if (!isCorrect)
                {
                    //失敗
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

                //もしaの0番目がn,y,母音でなければok
                if (a[0] != 'n' && a[0] != 'y' && a[0] != 'a' && a[0] != 'i' && a[0] != 'u' && a[0] != 'e' && a[0] != 'o')
                {
                    //romSliceListに挿入と表示の反映
                    _romSliceList[_furiCountList[r_num]] = "n";
                    _romSliceList[_furiCountList[r_num] + 1] = a;


                    //変更したらリストを再度表示させる
                    ReCreateList(_romSliceList);
                    r_tmp = string.Join("", GetRomSliceListWithoutSkip());

                    //trueにする
                    isCorrect = true;
                    //正解
                    Correct();

                    //最後の文字に正解したら
                    if (r_num >= r_tmp.Length)
                    {
                        //問題変更
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


                    //trueにする
                    isCorrect = true;

                    if (addSmallMoji)
                    {
                        AddSmallMoji();
                    }

                    //正解
                    Correct();

                    //最後の文字に正解したら
                    if (r_num >= r_tmp.Length)
                    {
                        //問題変更
                        AskQ();
                    }
                    break;
                }

            }
        }
    }

    void PerfectCorrect()
    {
        //trueにする
        isCorrect = true;
        //正解
        Correct();

        //最後の文字に正解したら
        if (r_num >= r_tmp.Length)
        {
            //問題変更
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

    //柔軟な入力をしたときに次の文字が小文字なら小文字を挿入する
    void AddSmallMoji()
    {
        int nextMojiNum = _furiCountList[r_num] + 1;

        //もし次の文字がなければ処理をしない
        if (f_tmp.Length - 1 < nextMojiNum)
        {
            return;
        }

        string nextMoji = f_tmp[nextMojiNum].ToString();
        string a = cd.dic[nextMoji][0];

        //もしaの0番目がxでもlでもなければ処理をしない
        if (a[0] != 'x' && a[0] != 'l')
        {
            return;
        }

        //romSliceListに挿入と表示の反映
        _romSliceList.Insert(nextMojiNum, a);
        //SKIPを削除する
        _romSliceList.RemoveAt(nextMojiNum + 1);

        //変更したらリストを再度表示させる
        ReCreateList(_romSliceList);
        r_tmp = string.Join("", GetRomSliceListWithoutSkip());


    }

    //ローマ字作成してくれる関数
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
            else if (moji[i].ToString() == "っ" && i + 1 < moji.Length && moji[i + 1].ToString() != "！" && moji[i + 1].ToString() != "?")
            {
                a = cd.dic[moji[i + 1].ToString()][0][0].ToString();
            }
            else if (i + 1 < moji.Length)
            {
                //次の文字も含めて辞書から探す
                string addNextMoji = moji[i].ToString() + moji[i + 1].ToString();
                if (cd.dic.ContainsKey(addNextMoji))
                {
                    a = cd.dic[addNextMoji][0];
                    //SKIPを有効にする
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

    //SKIPなしの表示をさせるためのListを作り直す
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
        //Shiftキーを押しているか
        isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);


        if (isShift)
        {
            //shift押しながら入力の記号
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
            //アルファベット大文字
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

        //スペース
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return " ";
        }

        //日本語配列に合わせる
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

    //問題出題してくれる関数
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



        //0文字目に戻す
        r_num = 0;

        //問題番号をランダムに生成
        if (!longMode)
            q_num = UnityEngine.Random.Range(0, _qList.Count);
        else
            q_num = long_num;

        //一時的に格納
        f_tmp = _fList[q_num];
        q_tmp = _qList[q_num];

        //ローマ字を作成してつなげる
        CreateRomSliceList(f_tmp);

        r_tmp = string.Join("", GetRomSliceListWithoutSkip());

        //テキストを変更
        fText.text = f_tmp;
        qText.text = q_tmp;
        rText.text = r_tmp;
    }

    //正解時の処理
    void Correct()
    {

        Debug.Log("正解！");
        SE_keypush.Play();
        correctKeyType++;

        r_num++;

        if (longMode)
        {
            if(r_num >= r_tmp.Length )
            {
                finishTime = Time.time;
                Debug.Log("計測終了");
                Debug.Log(finishTime);
                return;
            }

            //ふりがな
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
            //次がスペースの時
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
            //スペースなしの時
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

        

        //長文モードの時、問題のテキストも動きをつける
        if (longMode)
        {
            //ふりがな何文字目か
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

    //ミス時の処理
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

        //ふりがな何文字目か
        int furi_num;
        furi_num = _furiCountList[r_num];

        //長文モードの時、問題のテキストも動きをつける
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

        Debug.Log("ミス！");
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
        Text_StartFinish.text = "スタート！";
        yield return new WaitForSeconds(1f);
        Text_StartFinish.text = "";

        if (longMode)
        {
            Debug.Log("計測開始");
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

        //問題出題
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
        Text_StartFinish.text = "終了！";
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

        //リザルト表示
        SE_Result.Play();
        ob_result.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        //correct,miss,kps,正確率の順で開示
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
            Debug.Log("合計時間出力");
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

        //スコア表示
        int score=(int)(kpm * ac_3);
        Text_r_score.text = score.ToString();
        //ランキング登録
        if (bordNum != 0)
        {
            UnityroomApiClient.Instance.SendScore(bordNum, score, ScoreboardWriteMode.HighScoreDesc);
        }

        ob_score.SetActive(true);
        SE_score.Play();
        yield return new WaitForSeconds(0.3f);

        //ボタン表示
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
