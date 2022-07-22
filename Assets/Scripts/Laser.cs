using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    private bool _isEnemyLaser = false;
    private bool _isBackwardsFire =false;

    

    
    // Update is called once per frame
    void Update()
    {
         if (_isEnemyLaser == false || _isBackwardsFire == true)
         {
           MoveUp();
         }
         else
         {
           MoveDown();
         }
    }

   public void MoveUp()
    {
                
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        
         if (transform.position.y > 8f)
         {

           if (transform.parent != null)
           {
               Destroy(transform.parent.gameObject);
           }
           Destroy(this.gameObject);
         }
    }

    void MoveDown()
    {
         transform.Translate(Vector3.down * _speed * Time.deltaTime);
         
         if (transform.position.y < -8f)
         {

             if (transform.parent != null)
             {
                 Destroy(transform.parent.gameObject);
             }
                Destroy(this.gameObject);
         }
    }

    public void AssignEnemyLaser(bool value)
    {
        _isEnemyLaser = value;
        
    }
    public bool IsEnemyLaser()
    {
        return _isEnemyLaser;
    }
    public void AssignBackwardsFire(bool value)
    {
        _isBackwardsFire = value;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }
            if (transform.parent.tag == ("laser"))
            {
                Destroy (transform.parent.gameObject);
            }
            Destroy(this.gameObject);
            
            
        }
        
    }

    
}
