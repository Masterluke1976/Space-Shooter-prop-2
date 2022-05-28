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
    private Image _LivesImg;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;

    private GameManager _gameManager;

    //5.10
    [SerializeField]
    private Text _ammoText;

    //5.24
    [SerializeField]
    private int _maxAmmo = 15;

    [SerializeField]
    private Slider _slider;

   
    
   

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score:" + 0;

        //5.10
        _ammoText.text = "Ammo:" + 15 + "/" + _maxAmmo; //5.24


        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

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
        _ammoText.text = "Ammo:" + ammo + "/" + _maxAmmo; //5.24 added player ammo count current/max
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

    //5.26
    public void UpdateSlider(float fuel)
    {
        _slider.value = fuel;
    }

    

   
    

    
}
