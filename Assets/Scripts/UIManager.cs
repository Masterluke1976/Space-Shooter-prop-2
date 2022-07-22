using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Image _LivesImg;

    [SerializeField]
    private Sprite[] _livesSprites;
   
    private GameManager _gameManager;

    [SerializeField]
    private int _maxAmmo = 15;

    [SerializeField]
    private Slider _slider;

    //7.20 tonight
    [SerializeField]
    private Text _missileText;
    private int _missileAmmo = 3;
    private int _missileMaxAmmo = 5;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0;
        _ammoText.text = "Ammo:" + 15 + "/" + _maxAmmo; 
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        //7.20 tonight
        _missileText.text = "Ammo: " + _missileMaxAmmo + "/" + _missileAmmo;

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo:" + ammo + "/" + _maxAmmo; 
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    
    public void UpdateSlider(float fuel)
    {
        _slider.value = fuel;

    }

    //7.20 tonight
    public void MaxMissilePickup(int pickup)
    {
        if (_missileAmmo >= 5)
        {
            _missileAmmo = 5;
        }
        else
        {
            _missileAmmo += pickup;
            if (_missileAmmo + pickup > 5)
            {
                _missileAmmo = 3;
            } 
        }

        _missileText.text = "Ammo: " + _missileMaxAmmo + "/" + _missileAmmo;

    }

    public void MissileUpdate(int missileshot)
    {
        _missileAmmo = missileshot;
        _missileText.text = "Ammo: " + _missileMaxAmmo + "/" + _missileAmmo;
    }
  
}
