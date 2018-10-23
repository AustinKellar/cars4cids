using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public float _maxGaugeSize;

    public PlayerMovement car;
    public Text speedText;
    public RectTransform fuelGauge;
    public Button checkpointButton;
    public Button manipulateButton;


	void Start ()
    {
        car = GameObject.FindObjectOfType<PlayerMovement>();
        _maxGaugeSize = fuelGauge.rect.width;
        speedText.text = "Speed: " + car.speed;
	}
	
	void Update ()
    {
        Debug.Log("fuel: " + car.fuel + " maxFuel: " + car.maxFuel);
        fuelGauge.rect.Set(fuelGauge.rect.x, 
                           fuelGauge.rect.y, 
                           (car.fuel / car.maxFuel) * _maxGaugeSize, 
                           fuelGauge.rect.height);
        speedText.text = "Speed: " + car.speed;
    }

    public void Checkpoint()
    {

    }

    public void Manipulate()
    {

    }
}
