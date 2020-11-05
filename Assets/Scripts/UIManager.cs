using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
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

    private GameManager _gameManager;
    private Player _player;
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "SCORE: " +  0;
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
    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "SCORE: " + playerScore.ToString();    
    }


    public void UpdateAmmo(int playerAmmo)
    {
        _ammoText.text = "CURRENT AMMO: " + playerAmmo.ToString();

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

        IEnumerator UpdateLevelCoroutine()
        {
            yield return new WaitForSeconds(2f);
        }
    }


    public void UpdateLives(int currentLives)
    {
        _LivesImage.sprite = _livesSprite[currentLives];
        
        if(currentLives == 0)
        {
            GameOverSequence();
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
