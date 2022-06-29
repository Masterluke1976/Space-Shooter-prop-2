using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    private bool _isTargetAvailable;

    //private Rigidbody2D rb;
    [SerializeField]
    private float _speed = 3f;

    
    


    // Start is called before the first frame update
    void Start()
    {
        
       if (_target != null)
       {
            _isTargetAvailable = true;
           // Debug.LogError("Target is null");
       } 
       else
       {
            _isTargetAvailable = false;
            //return;
       }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation =Quaternion.AngleAxis(angle - 90,Vector3.forward);

        transform.Translate(transform.right * _speed * Time.deltaTime);  


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
            Destroy(this.gameObject);
        }
    }


}
