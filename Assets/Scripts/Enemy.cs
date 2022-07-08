using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    private bool _isAlive = true;

    [SerializeField]
    private bool _moveHorizontaly = false;
    private bool _moveRight = false;
    private float _horizontalSpeed = 2;

    
    [SerializeField]
    private GameObject _enemyShield;
    [SerializeField]
    private bool _enemyShieldActive = false;
    [SerializeField]
    private GameObject _enemyMissile;

    private bool _fireBackwards = false;

    
    private Vector3 _playerPos = Vector3.zero;

    
    private bool _ramme = false;
    private bool _arrived = false;
    private float _ramSpeed = 5f;
    private float _ramDistance = 5f;

    private float _randomX;




    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL ");
        }

        if (_moveHorizontaly == true)
        {
            StartCoroutine(SwitchDirectionRoutine());
        }
        
        if (_enemyShield != null)
        {
            EnableShield();
        }
       


        
    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();
        if (_moveHorizontaly == true)
        {
            MoveHorizontaly();
        }
        if (_player != null)
        {
            if (transform.position.y > _player.transform.position.y)
            {
                _fireBackwards = false;
            }
            else
            {
                _fireBackwards = true;
            }
        }
        FireLaser();
        //6.27
        if (_player != null)
        {
            RamPlayer();
        }
        


       
    }
    

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            
            _randomX = Random.Range(-8f, 8f);
           transform.position = new Vector3(_randomX, 10, 0);
        }
        //6.27
        float distance = Vector3.Distance(transform.position, _playerPos);
        if (distance < 0.1f)
        {
            _arrived = true;
        }
        if (_ramme && !_arrived)
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerPos, _ramSpeed * Time.deltaTime);
        }
        else if (!_ramme || (_ramme && _arrived))
        {
            transform.Translate(new Vector3(_randomX, -1, 0) * _speed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            if(other.tag == "Player")
            {
                Damage();
            }


        }
           if (other.tag == "Laser")
           {

            if (other.transform.GetComponent<Laser>().IsEnemyLaser() == false)
            {
                Destroy(other.gameObject);
                Damage();
            }
         
           }

        


    }
    void Damage()
    {
        if (_enemyShieldActive)
        {
            _enemyShield.SetActive(false);
            _enemyShieldActive = false;
        }
        else
        {

            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        }

        
        
    }
    
        
    void MoveHorizontaly()
    {
        if (_moveRight == true)
            transform.Translate(new Vector3(1, 0, 0) * _horizontalSpeed * Time.deltaTime);
        else if (_moveRight == false)
            transform.Translate(new Vector3(-1, 0, 0) * _horizontalSpeed * Time.deltaTime);
    }

    void FireLaser()
    {
        if (Time.time > _canFire && _isAlive == true)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser(true);
                lasers[i].AssignBackwardsFire(_fireBackwards);

            }

        }
    }

    IEnumerator SwitchDirectionRoutine()
    {
        while (_moveHorizontaly == true)
        {
            _moveRight = true;
            yield return new WaitForSeconds(3);
            _moveRight = false;
            yield return new WaitForSeconds(3);
        }
    }

    

    private void EnableShield()
    {
        int randomPick = Random.Range(0, 6);

       if(randomPick <= 4)
       {
          _enemyShield.SetActive(true);
          _enemyShieldActive = true;
       }
       else
       {
            _enemyShield.SetActive(false);
            _enemyShieldActive = false;
       }
        

        
    }

    
    void RamPlayer()
    {
        float dist = Vector3.Distance(transform.position, _player.transform.position);
        if (!_ramme)
        {
            if (dist <= _ramDistance)
            {
                _ramme = true;
                _playerPos = _player.transform.position;
            }
        }
         
         

    }



  

    

   
}
