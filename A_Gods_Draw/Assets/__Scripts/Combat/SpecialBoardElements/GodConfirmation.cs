using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodConfirmation : BoardElement
{
    Renderer _renderer;
    Material MaterialInstance;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        MaterialInstance = _renderer.material;
    }

    [SerializeField] float pulseSpeed = 2;
    [SerializeField] bool pulse;
    bool wasPulsing;
    [SerializeField] float intensity = 0;
    bool wasClicked;
    float _time;
    // Update is called once per frame
    void Update()
    {
        if(pulse)
        {
            _time += Time.deltaTime; 
            intensity = Mathf.PingPong(_time*pulseSpeed,1);
            setIntensity(intensity);
            wasPulsing = true;
        }
        else if (intensity > 0 && wasPulsing)
        {
            intensity -= Time.deltaTime*pulseSpeed;
            setIntensity(intensity);
            
            if(intensity <= 0)
            {
                intensity = 0;
                wasPulsing = false;
                _time = 0;
            }
        }

        if(wasClicked)
        {
            _time += Time.deltaTime;
            intensity = Mathf.PingPong(_time*pulseSpeed,1);
            setIntensity(intensity);

            if(intensity > 0.9)
            {
                wasClicked = false;
                wasPulsing = true;
            }
        }
    }

    private void OnMouseDown() {
        wasClicked = true;
        wasPulsing = false;
    }

    void setIntensity(float value)
    {
        float power = Mathf.SmoothStep(0,1, value);
        MaterialInstance.SetFloat("_GlowPower", power);
    }

    public void shouldGlow(bool state)
    {
        pulse = state;
    }
}