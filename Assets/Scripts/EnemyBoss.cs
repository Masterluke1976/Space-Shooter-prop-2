using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private int _speed = 2;
    private int _health;
    private int _shotsfired;
    private int _lives = 5;

    private bool _arrived = false;
    private bool _isAlive = true;

    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private GameObject _leftBossEngine, _rightBossEngine, _centerBossEngine;

    private float _firerate = 0.5f;
    private float _canfire = -1f;
    
    [SerializeField]
    private AudioSource _audio;

    [SerializeField]
    private Animator _anim;
    
    private Player _player;
    
    private SpawnManager _spawnManager;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 11.5f, 0);
        _shotsfired = 0;
        _health = 100;
        //_spawnManager = GameObject.Find("Spawn_Manger").GetComponent<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is null on the Boss Enemy");
        }
        if(_anim == null)
        {
            Debug.LogError("Animator is NUll on Enemy Boss");
        }
        if( _audio == null)
        {
            Debug.LogError("Animator is NUll on Enemy Boss");
        }
       

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        ShootingA();
            
    }

   

    void ShootingA()
    {
        if (_arrived == true && _canfire < Time.time && _isAlive == true)
        {
            _firerate = Random.Range(0.1f, 0.5f);
            _canfire = Time.time + _firerate;
            Vector3 direction = transform.position - _player.transform.position;
            direction.Normalize();
            float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            GameObject bossLaser = Instantiate(_laserPreFab, transform.position, Quaternion.Euler(0, 0, rotation - 90));
          
        }
        

    }

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if(transform.position.y <= 3.5f)
        {
            transform.position = new Vector3(0, 3.5f, 0);
            HasBossArrived();
        }
    }

    void HasBossArrived()
    {
        _arrived = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Damage(50);
            Destroy(other.gameObject);
        }
    }
    void Damage(int damageTaken)
    {
        _health -= damageTaken;
        
        if (_health < 75)
        {
            _leftBossEngine.SetActive(true);

        }
        if (_health < 50)
        {
            _rightBossEngine.SetActive(true);

        }
        if (_health < 30)
        {
           _centerBossEngine.SetActive(true);
        }
        if (_health < 0)
        {
            _isAlive = false;
            _anim.SetTrigger("OnEmemyDeath");
            _audio.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(_leftBossEngine);
            Destroy(_rightBossEngine);
            Destroy(_centerBossEngine);
            Destroy(this.gameObject, 2.8f);
        }
          
    }

        
   

    







}
