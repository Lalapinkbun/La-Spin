using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SlotController : MonoBehaviour
{
    //測試L幣，過後是要被代替的
    public int testCoin = 1;
    public Text CoinText;
    public PlayerDataObject _playerData;
    public GameDataManager _gameDataManager;

    //機器基礎資訊
    private int _baseCoin = 1;
    public int _slotCount = 1;
    public int _magnification = 1;
    [Tooltip("put the gameobject that named \"Slot\"")]
    public SpriteRenderer[] prefabSpawn;
    [Tooltip("put the gameobject that named \"Square\"")]
    public GameObject[] slotPlaceholder;

    //收集圖案
    public Sprite RedGem;
    public Sprite BlueGem;
    public Sprite GreenGem;

    //其他
    public Text addLostCoinText;
    public GameObject addLostCoin;
    public Text Log;
    public Button turnButton;
    public Button backToLobbyButton;

    public AchievementUIController uIController;

    public int[] TestResult;

    //對於成就的變數
    public int[] previousResult;
    public int repetition = 0;
    public int pantaKill = 0;
    public int Harderrrrr = 0;
    public int pairKing = 0;
    public bool isFourTimesOnThisTurn = false;

    //儅開始運行時，先默認顯示紅色，不然空著很奇怪www
    public void Start()
    {
        previousResult = new int[_slotCount];
        TestResult = new int[_slotCount];
        for (int i = 0;  i < prefabSpawn.Length; i++)
        {
            prefabSpawn[i].sprite = RedGem;
        }
        Log.text = string.Empty;
        _baseCoin = _slotCount;
    }

    //即時顯示錢的的咨詢
    public void Update()
    {
        CoinText.text = _playerData._playerData._LCoin.ToString();
    }

    public void TurnOne()
    {
        //Debug.Log(_playerData);
        //Debug.Log(_playerData._playerData);
        _gameDataManager.PlaySFX(0, true, false);
        Log.text = string.Empty;
        int i = _baseCoin * _magnification;
        if (isNotEnoughLCoin(_playerData._playerData._LCoin - i))
        {
            _playerData._playerData._LCoin -= i;
        }
        else
        {
            Debug.LogAssertion("not enough L Coin, pls choose lower magnification");
            Log.text = "not enough L Coin, pls choose lower magnification";
            return;
        }
        StartCoroutine(addCoinAnimation(-i));
        StartCoroutine(SpinAll());
        turnButton.interactable = false;
        backToLobbyButton.interactable = false;

        //成就1
        if (_playerData._playerData.GettingAchievement(0))
        {
            //Debug.Log($"Got the Achievement! {_playerData._playerData.GetAchievement(0)}");
            AchiAnimationCall(0);
        }
    }

    bool isNotEnoughLCoin(int LCoin)
    {
        if (LCoin < 0) return false;
        else return true;
    }

    int RandomNumber()
    {
        return Random.Range(0, 3);
    }

    IEnumerator SpinAll()
    {
        List<Coroutine> spins = new List<Coroutine>();

        for (int s = 0; s < _slotCount; s++)
        {
            spins.Add(StartCoroutine(SpinAnimation(s)));
        }

        // 等全部轉完
        foreach (var spin in spins)
            yield return spin;

        ResultCalculation(); // 在Animation之後只需要呼叫一次
    }

    //最終計算和演示
    public void ResultCalculation()
    {
        _gameDataManager.StopSFX();
        isFourTimesOnThisTurn = false;
        int gotcoin = 0;
        int gotcoinAchi = 0;
        int[] results = new int[_slotCount];
        bool isAllSame = true;
        for (int i = 0; i < _slotCount; i++)
        {
            results[i] = RandomNumber();
            //results[i] = 0;
            /*
            switch (TestResult[i])
            {
                case 0:
                    results[i] = TestResult[i]; break;
                case 1:
                    results[i] = TestResult[i]; break;
                case 2:
                    results[i] = TestResult[i]; break;
                case 3:
                    results[i] = TestResult[i]; break;
                case 4:
                    results[i] = TestResult[i]; break;
                default: 
                    results[i] = 0; break;
            }*/

            switch (results[i])
            {
                case 0:
                    Debug.Log("Red");
                    prefabSpawn[i].sprite = RedGem;
                    break;
                case 1:
                    Debug.Log("Blue");
                    prefabSpawn[i].sprite = BlueGem;
                    break;
                case 2:
                    Debug.Log("Green");
                    prefabSpawn[i].sprite = GreenGem;
                    break;
                default:
                    Debug.Log("somehow it out of range");
                    break;
            }
        }

        //檢測同花
        for (int i = 1; i < _slotCount; i++)
        {
            if (results[i] != results[0])
            {
                isAllSame = false;
                break;
            }
        }

        if (isAllSame && _slotCount >= 2)
        {
            //然後就看哪個符號同步，然後之前加的值直接替代掉。
            switch (results[0])
            {
                case 0:
                    gotcoin = 1 * (_slotCount * 5) * _magnification;
                    break;
                case 1:
                    gotcoin = 3 * (_slotCount * 2) * _magnification;
                    break;
                case 2:
                    gotcoin = 5 * (_slotCount * 2) * _magnification;
                    break;
                default:
                    break;
            }
        }
        else
        {
            for (int i = 0; i < results.Length; i++)
            {
                switch (results[i])
                {
                    case 0:
                        gotcoin += 1;
                        break;
                    case 1:
                        gotcoin += 3;
                        break;
                    case 2:
                        gotcoin += 5;
                        break;
                }
            }
            gotcoinAchi = gotcoin;
            gotcoin *= _magnification;
        }

        _playerData._playerData._LCoin += gotcoin;
        StartCoroutine(addCoinAnimation(gotcoin));
        turnButton.interactable = true;
        backToLobbyButton.interactable = true;

        //isAllSame = true;
        //成就2-11
        if (isAllSame && _slotCount == 2 && !_playerData._playerData.AchievementState(1))
        {
            _playerData._playerData.GettingAchievement(1);
            AchiAnimationCall(1);
        }
        if (isAllSame && _slotCount == 3 && !_playerData._playerData.AchievementState(2))
        {
            _playerData._playerData.GettingAchievement(2);
            AchiAnimationCall(2);
        }
        if (isAllSame && _slotCount == 4 && !_playerData._playerData.AchievementState(3))
        {
            _playerData._playerData.GettingAchievement(3);
            AchiAnimationCall(3);
            isFourTimesOnThisTurn = true;
        }
        if (isAllSame && _slotCount == 5 && !_playerData._playerData.AchievementState(4))
        {
            _playerData._playerData.GettingAchievement(4);
            AchiAnimationCall(4);
        }
        if (_playerData._playerData._LCoin >= 10000 && !_playerData._playerData.AchievementState(5))
        {
            _playerData._playerData.GettingAchievement(5);
            AchiAnimationCall(5);
        }
        if (gotcoinAchi == 23 && !_playerData._playerData.AchievementState(6))
        {
            _playerData._playerData.GettingAchievement(6);
            AchiAnimationCall(6);
        }
        if (gotcoinAchi == 5 && _slotCount >= 3 && !_playerData._playerData.AchievementState(7))
        {
            _playerData._playerData.GettingAchievement(7);
            AchiAnimationCall(7);
        }
        if (_slotCount == 3 && results[0] == 0 && results[1] == 1 && results[2] == 2 && !_playerData._playerData.AchievementState(8))
        {
            _playerData._playerData.GettingAchievement(8);
            AchiAnimationCall(8);
        }
        if (_slotCount == 3 && results[0] == 2 && results[1] == 1 && results[2] == 0 && !_playerData._playerData.AchievementState(9))
        {
            _playerData._playerData.GettingAchievement(9);
            AchiAnimationCall(9);
        }
        if (_slotCount == 5 && CheckFullHouse(results) && !_playerData._playerData.AchievementState(10))
        {
            _playerData._playerData.GettingAchievement(10);
            AchiAnimationCall(10);
        }
        //成就11-22
        if (_slotCount == 1 && !_playerData._playerData.AchievementState(11))
        {
            bool gotit = false;

            //對於Repitition的設計
            if (repetition == 0)
            {
                repetition++;
            }
            else
            {
                for (int i = 0; i < _slotCount; i++)
                {
                    if (results[i] == previousResult[i])
                    {
                        gotit = true;
                        //Debug.Log($"{results[i]} & {previousResult[i]} this is same");
                    }
                    else
                    {
                        //Debug.Log($"{results[i]} & {previousResult[i]} The Color is not same");
                    }
                }

                if (gotit)
                {
                    repetition++;
                }
                else
                {
                    repetition = 0;
                }
            }

            if (repetition == 3)
            {
                Debug.Log("Repitition足夠了，獲得成就");
                _playerData._playerData.GettingAchievement(11);
                AchiAnimationCall(11);
            }

            Debug.Log("Current repitition = " + repetition);
        }
        if (_slotCount == 1 && !_playerData._playerData.AchievementState(12))
        {

            bool gotit = false;

            //對於Repitition的設計
            if (pantaKill == 0)
            {
                pantaKill++;
            }
            else
            {
                for (int i = 0; i < _slotCount; i++)
                {
                    if (results[i] == previousResult[i])
                    {
                        gotit = true;
                        //Debug.Log($"{results[i]} & {previousResult[i]} this is same");
                    }
                    else
                    {
                        //Debug.Log($"{results[i]} & {previousResult[i]} The Color is not same");
                    }
                }

                if (gotit)
                {
                    pantaKill++;
                }
                else
                {
                    pantaKill = 0;
                }
            }

            if (pantaKill == 5)
            {
                Debug.Log("pantaKill足夠了，獲得成就");
                _playerData._playerData.GettingAchievement(11);
                AchiAnimationCall(11);
            }

            Debug.Log("Current pantaKill = " + pantaKill);
        }
        if (_slotCount == 5 && !_playerData._playerData.AchievementState(13))
        {

            bool gotit = true;

            if (Harderrrrr == 0 && isAllSame)
            {
                Harderrrrr++;
                Debug.Log("Current Harderrrrr = " + Harderrrrr);
            }
            else
            {
                for (int i = 0; i < _slotCount; i++)
                {
                    if (results[i] != previousResult[i])
                    {
                        gotit = false;
                        //Debug.Log($"{results[i]} & {previousResult[i]} The Color is not same");
                    }
                    else
                    {
                        //Debug.Log($"{results[i]} & {previousResult[i]} this is same");
                    }
                }

                if (gotit && isAllSame)
                {
                    Harderrrrr++;
                    Debug.Log("Current Harderrrrr = " + Harderrrrr);
                }
                else
                {
                    Harderrrrr = 0;
                    Debug.Log("Current Harderrrrr = " + Harderrrrr);
                }
            }

            if (Harderrrrr == 2)
            {
                Debug.Log("Harderrrrr足夠了，獲得成就");
                _playerData._playerData.GettingAchievement(13);
                AchiAnimationCall(13);
            }
        }
        if (_slotCount == 4 && !_playerData._playerData.AchievementState(14))
        {
            bool firstPair = false;
            bool secondPair = false;

            //檢測第一組是不是Pair
            if (results[0] == results[1])
            {
                firstPair = true;
            }
            //檢測第二組是不是Pair，且要計算和第一組的顔色不一樣
            if (results[2] == results[3] && results[2] != results[0])
            {
                secondPair = true;
            }
            if (firstPair && secondPair)
            {
                _playerData._playerData.GettingAchievement(14);
                AchiAnimationCall(14);
            }
        }
        if (_slotCount == 4 && !_playerData._playerData.AchievementState(15))
        {
            bool firstPair = false;
            bool other = false;

            //檢測第一組的第一個和最後一個是不是同樣的圖案
            if (results[0] == results[3])
            {
                firstPair = true;
            }
            //剩下的作爲第二組檢測是不是和第一組的顔色不一樣
            if (results[1] != results[0] && results[2] != results[0])
            {
                other = true;
            }

            if (firstPair && other)
            {
                _playerData._playerData.GettingAchievement(15);
                AchiAnimationCall(15);
            }
        }
        if (_slotCount == 2 && !_playerData._playerData.AchievementState(16))
        {
            //對於Pair King的設計

            bool gotit = true;
            if (pairKing == 0 && isAllSame)
            {
                pairKing++;
                Debug.Log("Current pairKing = " + pairKing);
            }
            else
            {
                for (int i = 0; i < _slotCount; i++)
                {
                    if (results[i] != previousResult[i])
                    {
                        gotit = false;
                        //Debug.Log($"{results[i]} & {previousResult[i]} The Color is not same");
                    }
                    else
                    {
                        //Debug.Log($"{results[i]} & {previousResult[i]} this is same");
                    }
                }

                if (gotit && isAllSame)
                {
                    pairKing++;
                    Debug.Log("Current Pair King = " + pairKing);
                }
                else
                {
                    pairKing = 0;
                    Debug.Log("Current Pair King = " + pairKing);
                }
            }

            if (pairKing == 3)
            {
                Debug.Log("Pair King足夠了，獲得成就");
                _playerData._playerData.GettingAchievement(16);
                AchiAnimationCall(16);
            }
        }
        if (_playerData._playerData._LCoin >= 151 && !_playerData._playerData.AchievementState(17))
        {
            _playerData._playerData.GettingAchievement(17);
            AchiAnimationCall(17);
        }
        if (_slotCount == 4 && !_playerData._playerData.AchievementState(18))
        {
            bool firstPair = false;
            bool other = false;

            //檢測是否第一組的第一個至第三個都是同樣的圖案
            if (results[0] == results[1] && results[0] == results[2])
            {
                firstPair = true;
            }
            //剩下的作爲第二組檢測是不是和第一組的顔色不一樣
            if (results[3] != results[0])
            {
                other = true;
            }

            if (firstPair && other)
            {
                _playerData._playerData.GettingAchievement(18);
                AchiAnimationCall(18);
            }
        }
        if (_playerData._playerData._LCoin >= 100000 && !_playerData._playerData.AchievementState(19))
        {
            _playerData._playerData.GettingAchievement(19);
            AchiAnimationCall(19);
        }
        if (_slotCount == 5 && !_playerData._playerData.AchievementState(20))
        {
            int[] theResultMustBe = {0, 1, 2, 1, 0};
            bool gotit = true;

            for (int i = 0; i < _slotCount; i++)
            {
                if (results[i] == theResultMustBe[i])
                {
                    //Debug.Log($"{results[i]} & {theResultMustBe[i]} this is same");
                }
                else
                {
                    gotit = false;
                    //Debug.Log($"{results[i]} & {theResultMustBe[i]} The Color is not same");
                }
            }

            if (gotit)
            {
                _playerData._playerData.GettingAchievement(20);
                AchiAnimationCall(20);
            }
        }
        if (isAllSame && _slotCount == 4 && !_playerData._playerData.AchievementState(21) && _playerData._playerData.AchievementState(3))
        {
            if (results[0] == 1 && !isFourTimesOnThisTurn)
            {
                _playerData._playerData.GettingAchievement(21);
                AchiAnimationCall(21);
            }
        }

        //所以判定跑完了，如果沒其他事情就把這一次的成績記錄一下
        previousResult = results;

    }

    //會顯示扣了還是加了多少L幣
    IEnumerator addCoinAnimation(int coincount)
    {
        if (coincount > 0)
        {
            addLostCoin.SetActive(true);
            addLostCoinText.text = $"+{coincount.ToString()}";
            yield return new WaitForSeconds(0.5f);
            addLostCoin.SetActive(false);
        }
        else if (coincount < 0)
        {
            addLostCoin.SetActive(true);
            addLostCoinText.text = $"{coincount.ToString()}";
            yield return new WaitForSeconds(0.5f);
            addLostCoin.SetActive(false);
        }

        yield return null;
    }

    IEnumerator SpinAnimation(int slot)
    {
        for (int i = 0; i < 20; i++) // 模擬轉動
        {
            prefabSpawn[slot].sprite = RandomSprite();
            yield return new WaitForSeconds(0.05f);
        }
    }

    Sprite RandomSprite()
    {
        Sprite lastSprite = null;
        Sprite newSprite;
        do
        {
            int rand = RandomNumber();
            switch (rand)
            {
                case 0: newSprite = RedGem; break;
                case 1: newSprite = BlueGem; break;
                default: newSprite = GreenGem; break;
            }
        } while (newSprite == lastSprite); // 避免重複
        lastSprite = newSprite;
        return newSprite;
    }

    //更改倍率
    public void ChangeMagnification(int magnification)
    {
        _magnification = magnification + 1; //因爲dropdown的值是從0開始，所以必須+1
    }

    public void AchiAnimationCall(int id)
    {
        Debug.Log($"Got the Achievement! {_playerData._playerData.GetAchievement(id)}");
        uIController.AchievementAnimation(_playerData._playerData.GetAchievementName(id), _playerData._playerData.GetAchievementMemeDec(id));
        _gameDataManager.PlaySFX(1, false, true);
    }

    public bool CheckFullHouse(int[] results)
    {
        int fullhousecount = 0;
        int fullhouseslotleft = 5;
        bool fullhousefirst = false;
        bool fullhouselast = false;
        int ban = -1;
        //检测FullHouse - 1
        for (int i = 0; i <= fullhouseslotleft - 3; i++)
        { 
            if (i == 1)
            {
                if (results[i] == results[0])
                {
                    fullhousecount++;
                }
                else
                {
                    fullhousecount--;
                }
            }
            else
            {
                if (results[i] == results[0])
                {
                    fullhousecount++;
                }
            }
            Debug.Log(i + ".: " + fullhousecount);
        }
        Debug.Log($"First Run For: {fullhousecount}");
        switch (fullhousecount)
        {
            case 2:
                fullhouseslotleft = 3;
                fullhousefirst = true;
                ban = results[0];
                break;
            case 3:
                fullhouseslotleft = 2;
                fullhousefirst = true;
                ban = results[0];
                break;
            default:
                fullhousefirst = false;
                break;
        }
        //检测FullHouse - 2
        fullhousecount = 0;
        if (fullhousefirst)
        {
            Debug.Log("Full House first are pass");
            if (fullhouseslotleft == 3)
            {
                for (int i = 2; i < _slotCount; i++)
                {
                    if (results[i] == results[2] && results[i] != ban)
                    {
                        fullhousecount++;
                    }
                    Debug.Log(i + ".: " + fullhousecount);
                }
            }
            else
            {
                for (int i = 3; i < _slotCount; i++)
                {
                    if (results[i] == results[3] && results[i] != ban)
                    {
                        fullhousecount++;
                    }
                    Debug.Log(i + ".: " + fullhousecount);
                }
            }
            if (fullhousecount == fullhouseslotleft)
            {
                fullhouselast = true;
                Debug.Log("Full House last are pass");
            }
        }
        Debug.Log($"Last Run For: {fullhousecount}");
        if (fullhousefirst && fullhouselast)
        {
            return true;
        }
        return false;
    }
}
