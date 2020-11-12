using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _gameOver;

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _maxAmmoText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Text _levelText;

    [SerializeField]
    private Sprite [] _livesSprite;

    [SerializeField]
    private Image _LivesImage;

    [SerializeField]
    private Sprite[] _shieldSprite;

    [SerializeField]
    private Image _ShieldImage;

    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Image _thrusterBackground;
    [SerializeField]
    private Text _thrusterText;

    [SerializeField]
    private Slider _healthSlider;
    [SerializeField]
    private Image _healthBackground;
    [SerializeField]
    private Text _healthText;

    private float _currentHealth;
    private float _maxHealth = 100f;

    private float _currentThrust;
    private float _maxThrust = 100f;

    private GameManager _gameManager;
    private Player _player;
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        _gameOver.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_gameManager == null)
        {
            Debug.Log("GameManager is NULL");
        }
        if (_player == null)
        {
            Debug.Log("Player is NULL");
        }
        if( _spawnManager == null)
        {
            Debug.Log("Spawn Manager is NULL");
        }

        _ShieldImage.gameObject.SetActive(false);

        _scoreText.text = "SCORE: " + 0;
        _thrusterText.text = "Thruster";
        _thrusterBackground.color = Color.blue;
        _healthText.text = "Health";
        _ammoText.text = "CURRENT AMMO: " + _player._ammo.ToString();
        _maxAmmoText.text = "MAX AMMO: " + _player._maxAmmo.ToString();
    }

    private void Update()
    {
        ThrusterUpdate();
        HealthUpdate();
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE: " + playerScore.ToString();    
    }


    public void UpdateAmmo(int playerAmmo)
    {
        _ammoText.text = "CURRENT AMMO: " + playerAmmo.ToString();
        _maxAmmoText.text = "MAX AMMO: " + _player._maxAmmo.ToString();

        if (playerAmmo == 0)
        {
            StartCoroutine(LowAmmoFlicker());
        }
        IEnumerator LowAmmoFlicker()
        {
            while (true)
            {
                _ammoText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                _ammoText.gameObject.SetActive(false);
                yield return new WaitForSeconds(1f);
                _ammoText.gameObject.SetActive(false);
            }
        }
    }

    public void UpdateLevel(int waveCount)
    {    
        _levelText.text = "LEVEL " + waveCount.ToString() + " CLEARED!";
        _levelText.gameObject.SetActive(true);

        StartCoroutine(UpdateLevelCoroutine());
    }
    IEnumerator UpdateLevelCoroutine()
    {
        yield return new WaitForSeconds(2f);
        _levelText.gameObject.SetActive(false);
    }


    public void WinnerText()
    {
        _levelText.text = "YOU WIN!!";
        _levelText.gameObject.SetActive(true);
        StartCoroutine(UpdateWinnerCoroutine());
        GameOverSequence();
    }
    IEnumerator UpdateWinnerCoroutine()
    {
        yield return new WaitForSeconds(10f);
        _levelText.gameObject.SetActive(false);
    }


    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _livesSprite[currentLives];
        
        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    public void ThrusterUpdate()
    {

            _currentThrust = _player.playerThrust;

            float fillAmount = _currentThrust / _maxThrust;
            _thrusterBackground.fillAmount = fillAmount;
            _thrusterText.text = "Thruster";
           
        if(_currentThrust < 25f)
        {    
            _thrusterText.text = "Thruster is low!! Press X to refill";
            _thrusterBackground.color = Color.red;
        }
        else
        {
            _thrusterBackground.color = Color.blue; ;
            _thrusterText.text = "Thruster";
        }   
    }

    public void HealthUpdate()
    {

        _currentHealth = _player.playerHealth;
        float fillAmount = _currentHealth / _maxHealth;
        _healthBackground.fillAmount = fillAmount;
        _healthText.text = "Health";

        if (_currentHealth < 25f)
        {
            _healthText.color = Color.red;
        }
        else
        {
            _healthText.color = Color.white;
        }

    }


    public void UpdateShield(int currentShield)
    {
        if (_player.isShieldsActive == true && currentShield > 0)
        {
            _ShieldImage.gameObject.SetActive(true);
            _ShieldImage.sprite = _shieldSprite[currentShield];
        }
        else
        {
            _ShieldImage.gameObject.SetActive(false);
        }          
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOver.gameObject.SetActive(true);
        }    
    }
}
