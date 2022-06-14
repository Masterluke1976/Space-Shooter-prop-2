using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 10;
    [SerializeField]
    private float _maxFuel = 5f;
    [SerializeField]
    private float _currentFuel;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;


    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _ShieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;


    [SerializeField]
    private int _lives = 3;
    private int _totalLasers = 15;
    [SerializeField]
    private int _currentLasers;
    [SerializeField]
    private int _score;
    private int _shieldStrength = 3;


    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShielsdActive = false;


    

    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    [SerializeField]
    private SpriteRenderer _shieldRenderer;

    private CameraShake _camerShake;

    private SpawnManager _spawnManager;

    public Slider ThrusterSlider;

    private UIManager _uiManager;



    // Start is called before the first frame update
    void Start()
    { 
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _camerShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        _currentFuel = _maxFuel;
        ThrusterSlider.maxValue = _maxFuel;

        _currentLasers = _totalLasers;

        
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on the player is NULL ");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        
       // Calculatespeed();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }


        //6.14
        if (Input.GetKey(KeyCode.LeftShift) && _currentFuel > 0) 
        {
            Debug.Log("Stamina down");
            _speed = _speedMultiplier;
            _currentFuel -= Time.deltaTime;

  
        }
        else 
        {
            _speed = 3.5f;
            _currentFuel += Time.deltaTime;
        }

        ThrusterSlider.value = _currentFuel;
        if(_currentFuel >= 5)
        {
            _currentFuel = 5;
        }

    }

    

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime);
        

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

   
    

    void FireLaser()
    {
          _canFire = Time.time + _fireRate;

        if (_currentLasers > 0)
        {
             if (_isTripleShotActive == true)
             {
               Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
             }
            else
            {
              Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

             _audioSource.Play();
             AmmoCount(1);
        }

    }

    public void Damage()
    {
        
        if (_isShielsdActive == true)
        {
            
            _shieldStrength --;
            if (_shieldStrength == 2)
            {
                StartCoroutine(ShieldHitVisual());
                return;
            } 
            else if (_shieldStrength == 1)
            {
                StartCoroutine(ShieldHitVisual());
                return;
            }
            if (_shieldStrength < 1)
            {
                _isShielsdActive = false;
                _ShieldVisualizer.SetActive(false);
                
                 return;
            }

        }


        _lives -= 1;
        _camerShake.CamShake();

        CheckLives();
       
        
        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);

        }
    }
    private void CheckLives()
    {
        if (_lives == 3)
        {
            _leftEngine.SetActive(false);
            _rightEngine.SetActive(false);
        }
        else if (_lives == 2)
        {
            _leftEngine.SetActive(true);
            _rightEngine.SetActive(false);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
            _leftEngine.SetActive(true);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }
    
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
       
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
       
        _isShielsdActive = true;

        _shieldStrength = 3;

        _ShieldVisualizer.SetActive(true);
    }

    
    IEnumerator ShieldHitVisual()
    {
        _shieldRenderer.color = Color.red;
        yield return new WaitForSeconds(.5f);
        _shieldRenderer.color = Color.white;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

   
    private void AmmoCount(int amount)
    {
        _currentLasers -= amount;
        if (_uiManager != null)
        {
            _uiManager.UpdateAmmo(_currentLasers);
        }
        if (_currentLasers < 1)
        {
            _currentLasers = 0;
        }
    }

    public void AmmoActive()
    {
        _currentLasers = 15;
        if (_uiManager != null)
        {
            _uiManager.UpdateAmmo(_currentLasers);
        }
    }

    public void HealthActive()
    {
      
        if (_lives < 3)
        {
            _lives += 1;
            _uiManager.UpdateLives(_lives);
            CheckLives();
        }

    }

    

    

    


   

    
    
}
