using UnityEngine;
// ____añadido
using UnityEngine.UI;
// ______finalizado

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private int coins;

    // ____añadido
    [SerializeField] private int stars;
    private int _totalStars;
    private bool _isGameFinished = false;

    [SerializeField] private Text _coinsText;
    [SerializeField] private Text _starsText;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private GameObject _victoryPanel;
    // ______finalizado

    private bool _isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // ____borrado (antes: AudioManager.Instance.StartSoundtrack(); ahora la música la arranca sola el AudioManager en su Start)
        // ______finalizado

        // ____añadido
        _totalStars = FindObjectsByType<Star>(FindObjectsSortMode.None).Length;
        _coinsText.text = coins.ToString();
        _starsText.text = stars + " / " + _totalStars;
        _victoryPanel.SetActive(false);
        // ______finalizado
    }

    public void AddCoin()
    {
        coins += 1;

        // ____añadido
        _coinsText.text = coins.ToString();
        // ______finalizado
    }

    // ____añadido
    public void AddStar()
    {
        stars += 1;
        _starsText.text = stars + " / " + _totalStars;

        if (stars >= _totalStars)
        {
            Victory();
        }
    }

    void Victory()
    {
        _isGameFinished = true;
        _isPaused = true;
        _victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        _healthSlider.maxValue = maxHealth;
        _healthSlider.value = currentHealth;
    }
    // ______finalizado

    public void Pause()
    {
        // ____añadido
        if (_isGameFinished)
        {
            return;
        }
        // ______finalizado

        if (_isPaused)
        {
            _isPaused = false;
            // ____cambiado (antes: AudioManager.Instance.StartSoundtrack();)
            AudioManager.Instance.ResumeSoundtrack();
            // ______finalizado
            Time.timeScale = 1;
        }
        else
        {
            _isPaused = true;
            AudioManager.Instance.PauseSoundtrack();

            Time.timeScale = 0;
        }
        CanvasManager.Instance.ChangeCanvasStatus(CanvasManager.Instance.pauseCanvas, CanvasManager.Instance.resumeButton);
    }
    public bool IsPaused()
    {
        return _isPaused;
    }

    //---- AÑADIDO ----
    public void GameOver()
    {
        Debug.Log("GAME OVER");

        // ____añadido
        _isGameFinished = true;
        Invoke("LoadGameOverScene", 1.5f);
        // ______finalizado
    }
    //---- FIN AÑADIDO ----

    // ____añadido
    void LoadGameOverScene()
    {
        SceneLoader.Instance.ChangeScene("Game Over");
    }
    // ______finalizado

}