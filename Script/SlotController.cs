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

    //儅開始運行時，先默認顯示紅色，不然空著很奇怪www
    public void Start()
    {
        for(int i = 0;  i < prefabSpawn.Length; i++)
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
        int gotcoin = 0;
        int[] results = new int[_slotCount];
        bool isAllSame = true;
        for (int i = 0; i < _slotCount; i++)
        {
            results[i] = RandomNumber();
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

        if (isAllSame)
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
            gotcoin *= _magnification;
        }


        _playerData._playerData._LCoin += gotcoin;
        StartCoroutine(addCoinAnimation(gotcoin));
        turnButton.interactable = true;
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
}
