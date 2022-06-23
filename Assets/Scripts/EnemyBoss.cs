using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    private int _speed = 2;
    private bool _arrived = false;
    [SerializeField]
    private GameObject _laserPreFab;
    private float _firerate = 0.5f;
    private float _canfire = -1f;
    private int _shotsfired;
    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private Animator _anim;
    private Player _player;
    private int _health;
    private bool _isAlive;
    [SerializeField]
    private GameObject _damage1;
    [SerializeField]
    private GameObject _damage2;
    [SerializeField]
    private GameObject _damage3;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 11.5f, 0);
        _shotsfired = 0;
        _health = 100;
        _spawnManager = GameObject.Find("Spawn_Manger").GetComponent<SpawnManager>();
        _player = GameObject.Find("PLayer").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
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

    IEnumerator ShootingRoutine()
    {
        yield return new WaitForSeconds(2.2f);
        while(_arrived == true && _shotsfired < 5)
        {
            ShootingA();
            yield return new WaitForSeconds(5.0f);
            //ShootingB();
            //yield return new WaitForSeconds(5.0f);
        }
    }

    void ShootingA()
    {
        if (_arrived == true && _canfire < Time.time && _isAlive == true)
            _firerate = Random.Range(0.3f, 0.75f);
            _canfire = Time.time - _firerate;
            Vector3 direction = transform.position - _player.transform.position;
            direction.Normalize();
            float rotation = Mathf.Atan2(direction.x, direction.x)* Mathf.Rad2Deg;
            GameObject bossLaser = Instantiate(_laserPreFab, transform.position, Quaternion.Euler(0, 0, rotation - 9));
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
            Damage(5);
            Destroy(other.gameObject);
        }
    }
    void Damage(int damageTaken)
    {
        _health -= damageTaken;
        if (_health < 0)
        {
            _isAlive = false;
            //_player.Scorefromenemy(250);
            //_spawnManager.BossDestroy();
            _anim.SetTrigger("OnEmemyDeath");
            _audio.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.8f);
        }
    }

        


}
