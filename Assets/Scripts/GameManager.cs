using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles setting the game state and managing the ui screens
/// in the future ui handling should be done by a separate manager and more complex
/// screens like the end screen should have their own monobehaviour
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// singleton reference
    /// </summary>
    private static GameManager _instance = null;
    public static GameManager Instance { get { return _instance; } }
    // references to all needed ui objects
    [Header("UI References")]
    [SerializeField]
    private Button _startButton = null;
    [SerializeField]
    private Image _pauseBackground = null;
    [SerializeField]
    private Text _pauseTxt = null;
    [SerializeField]
    private CanvasGroup _endScreen = null;
    [SerializeField]
    private CanvasGroup _playerUI = null;
    [SerializeField]
    private Text _p1TimeTxt = null;
    [SerializeField]
    private Text _p1ScoreTxt = null;
    [SerializeField]
    private Text _p2TimeTxt = null;
    [SerializeField]
    private Text _p2ScoreTxt = null;
    [Header("End Screen")]
    [SerializeField]
    private Text _p1EndScoreTxt = null;
    [SerializeField]
    private Text _p2EndScoreTxt = null;
    [SerializeField]
    private Text _p1EndWinnerTxt = null;
    [SerializeField]
    private Text _p2EndWinnerTxt = null;
    // references to all gameplay objects for reseting them
    [Header("Other References")]
    [SerializeField]
    private CustomerManager _customerManager = null;
    [SerializeField]
    private Interactable[] _interactables = null;
    [SerializeField]
    private Player _player1 = null;
    [SerializeField]
    private Player _player2 = null;
    /// <summary>
    /// default timer for a player
    /// </summary>
    [Header("Variables")]
    [SerializeField]
    private float _gameTime = 900;

    /// <summary>
    /// player 1 time remaining
    /// </summary>
    private float _p1Time = 900;
    /// <summary>
    /// player 2 time remaining
    /// </summary>
    private float _p2Time = 900;
    /// <summary>
    /// player 1 score
    /// </summary>
    private int _p1Score = 0;
    /// <summary>
    /// player 2 score
    /// </summary>
    private int _p2Score = 0;
    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
        }
        _instance = this;
        // set timescale to 0 and wait for the start press
        Time.timeScale = 0;
        _p1ScoreTxt.text = $"Score: {_p1Score}";
        _p2ScoreTxt.text = $"Score: {_p2Score}";
    }

    /// <summary>
    /// reset everything ui, interactable, and player
    /// </summary>
    private void Reset()
    {
        _p1Time = _gameTime;
        _p2Time = _gameTime;
        _p1Score = 0;
        _p2Score = 0;
        _p1TimeTxt.text = $"Time: {Mathf.CeilToInt(_p1Time)}s";
        _p2TimeTxt.text = $"Time: {Mathf.CeilToInt(_p2Time)}s";
        _p1ScoreTxt.text = $"Score: {_p1Score}";
        _p2ScoreTxt.text = $"Score: {_p2Score}";
        _customerManager.Reset();
        for(int i = 0; i < _interactables.Length; i++)
        {
            _interactables[i].Reset();
        }
        _player1.Reset();
        _player2.Reset();
    }

    /// <summary>
    /// start the game from the start button
    /// </summary>
    public void StartGame()
    {
        Time.timeScale = 1;
        _startButton.gameObject.SetActive(false);
        _pauseBackground.gameObject.SetActive(false);
    }

    /// <summary>
    /// start the game from the end game screen
    /// </summary>
    public void RestartGame()
    {
        Reset();
        _playerUI.gameObject.SetActive(true);
        _endScreen.gameObject.SetActive(false);
        _pauseBackground.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// end the game, called when both timers hit 0
    /// pulls up end game screen
    /// </summary>
    public void EndGame()
    {
        Time.timeScale = 0;
        _playerUI.gameObject.SetActive(false);
        _endScreen.gameObject.SetActive(true);
        _pauseBackground.gameObject.SetActive(true);
        _p1EndScoreTxt.text = $"Score: {_p1Score}";
        _p2EndScoreTxt.text = $"Score: {_p2Score}";
        if (_p1Score > _p2Score)
        {
            _p1EndWinnerTxt.gameObject.SetActive(true);
            _p2EndWinnerTxt.gameObject.SetActive(false);
        }
        else if (_p1Score < _p2Score)
        {
            _p1EndWinnerTxt.gameObject.SetActive(false);
            _p2EndWinnerTxt.gameObject.SetActive(true);
        }
        else
        {
            _p1EndWinnerTxt.gameObject.SetActive(true);
            _p2EndWinnerTxt.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        _p1Time -= Time.deltaTime;
        _p2Time -= Time.deltaTime;
        if(_p1Time <= 0)
        {
            _p1Time = 0;
            _player1.LockPlayer();
        }
        if (_p2Time <= 0)
        {
            _p2Time = 0;
            _player2.LockPlayer();
        }
        if(_p1Time == 0 && _p2Time == 0)
        {
            EndGame();
        }
        _p1TimeTxt.text = $"Time: {Mathf.CeilToInt(_p1Time)}s";
        _p2TimeTxt.text = $"Time: {Mathf.CeilToInt(_p2Time)}s";
    }
    /// <summary>
    /// Add points to player 1 score, use negative to subtract points
    /// </summary>
    public void AddPointsP1(int points)
    {
        _p1Score += points;
        _p1ScoreTxt.text = $"Score: {_p1Score}";
    }
    /// <summary>
    /// Add points to player 2 score, use negative to subtract points
    /// </summary>
    public void AddPointsP2(int points)
    {
        _p2Score += points;
        _p2ScoreTxt.text = $"Score: {_p2Score}";
    }

    /// <summary>
    /// Add points to the player score, use negative to subtract points
    /// </summary>
    public void AddPoints(int points, Player player)
    {
        if (player == _player1)
        {
            AddPointsP1(points);
        }
        else if (player == _player2)
        {
            AddPointsP2(points);
        }
        else
        {
            Debug.LogWarning("Attempting to give points to unknown player");
        }
    }

    /// <summary>
    /// pause/unpause the game
    /// </summary>
    public void Pause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        _pauseBackground.gameObject.SetActive(pause);
        _pauseTxt.gameObject.SetActive(pause);
    }
}
