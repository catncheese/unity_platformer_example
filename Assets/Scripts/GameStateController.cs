using TMPro;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _coinsText;

    private int _coinsCount;
    public int CoinsCount
    {
        get { return _coinsCount; }
        set 
        { 
            _coinsCount = value;
            _coinsText.text = _coinsCount.ToString();
        }
    }


    private static GameStateController _instance;
    public static GameStateController Instance
    {
        get
        {
            if (_instance == null)
                _instance = GameObject.FindAnyObjectByType<GameStateController>();

            return _instance;
        }
    }

    public void Start()
    {
        CoinsCount = 0;
    }
}
