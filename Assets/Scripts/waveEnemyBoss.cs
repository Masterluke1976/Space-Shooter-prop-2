using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveEnemyBoss : MonoBehaviour

{
    [SerializeField]
    private float _speed = 4.0f;
    private float _horizontalSpeed = 2;
    private float _fireRate = 1.0f;
    private float _canFire = -1;
    private float _randomX;

    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _enemyMissile;


    //private bool _isAlive = true;

    [SerializeField]
    private bool _moveHorizontaly = false;
    private bool _moveRight = false;
  


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

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();
        if (_moveHorizontaly == true)
        {
            MoveHorizontaly();
        }

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyMissile, transform.position, Quaternion.identity);
        }
       
        

        
    }


    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -10f)
        {

            _randomX = Random.Range(-1f, 1f);
            transform.position = new Vector3(_randomX, 10, 0);
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


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            if (other.tag == "Player")
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

        if (other.tag == "PlayerMissile")
        {
            Damage();
        }




    }
    void Damage()
    {
       
            if (_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            //_isAlive = false;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.6f);
        
    }


    void MoveHorizontaly()
    {
        if (_moveRight == true)
        {
            transform.Translate(new Vector3(1, 0, 0) * _horizontalSpeed * Time.deltaTime);
        }
            
        else if (_moveRight == false)
        {
            transform.Translate(new Vector3(-1, 0, 0) * _horizontalSpeed * Time.deltaTime);
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


}