using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 2;

    
      

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _ShieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShielsdActive = false;

    [SerializeField]
    private int _score;

    private UIManager _uiManager;

    //variable for audio clip
    [SerializeField]
    private AudioClip _laserSoundClip;
    private AudioSource _audioSource;

    //5.10 shield strength visual 3 hit

    private int _shieldStrength = 3;
    [SerializeField]
    private SpriteRenderer _shieldRenderer;

    //5.10 ammo count
    private int _totalLasers = 15;
    [SerializeField]
    private int _currentLasers;

    private CameraShake _camerShake;



   

    

    


    

    

    
    // Start is called before the first frame update
    void Start()
    { 
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _camerShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();

        //5.10 amount count 
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
        //5.10
        Calculatespeed();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    //thruster boost left shiftkey 5.10
    private void Calculatespeed()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            _speed *= _speedMultiplier;

        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = 3.5f;
            
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

        //5.10 ammo

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





        // if (_isTripleShotActive == true)
        // {
        // Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        // }
        //else
        //{
        // Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        // }

        //play the laser audio clip
        // _audioSource.Play();
    }

    public void Damage()
    {
        
        if (_isShielsdActive == true)
        {
            //5.10
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


            // _isShielsdActive = false;
            // _ShieldVisualizer.SetActive(false);
            // disable the visualizer
            // return;
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

        //5.10 turn shield on again 
        _shieldStrength = 3;

        _ShieldVisualizer.SetActive(true);
    }

    //5.10 shield visulizer color
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

    //5.10 ammo count
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
