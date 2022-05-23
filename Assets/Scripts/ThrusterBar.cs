using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    public Slider thrusterBar;
    private int _maxThruster = 100;
    private int _curentThruster;
    // Start is called before the first frame update
    void Start()
    {
        _curentThruster = _maxThruster;
        thrusterBar.maxValue = _maxThruster;
        thrusterBar.value = _maxThruster;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseThruster(int amount)
    {
        if (_curentThruster - amount >=0)
        {
            _curentThruster -= amount;
            thrusterBar.value = _curentThruster;
        }
        else
        {
            Debug.Log("No Thrusters");
        }
    }
}
