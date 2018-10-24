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
        fuelGauge.localScale = new Vector3(car.fuel / car.maxFuel, 1f, 1f);
        speedText.text = "Speed: " + car.speed;
    }

    public void Checkpoint()
    {

    }

    public void Manipulate()
    {

    }
}
