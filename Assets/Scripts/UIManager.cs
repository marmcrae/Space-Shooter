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
    private Sprite [] _livesSprite;

    [SerializeField]
    private Image _LivesImage;

    [SerializeField]
    private Sprite[] _shieldSprite;

    [SerializeField]
    private Image _ShieldImage;

    private GameManager _gameManager;
    private Player _player;


    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "SCORE: " +  0;
        _gameOver.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
        if (_player == null)
        {
            Debug.Log("Player is NULL");
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
            Debug.Log("Update Shield UI called");
            _ShieldImage.gameObject.SetActive(true);
            _ShieldImage.sprite = _shieldSprite[currentShield];
            Debug.Log("current shield index is: " + currentShield);
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
