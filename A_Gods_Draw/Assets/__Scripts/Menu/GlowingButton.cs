using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingButton : MonoBehaviour
{
        Renderer _renderer;
    Material MaterialInstance;

    private void Start() {
        _renderer = GetComponent<Renderer>();
        MaterialInstance = _renderer.material;
    }

    [SerializeField] bool IsActive = true;
    [SerializeField] float pulseSpeed = 2;
    [SerializeField] float clickSpeed = 5;
    [SerializeField] float hoverSpeed = 5;
    [SerializeField] float MaxPulseIntensity = 1;
    [SerializeField] float HoverIntensity = 2;
    [SerializeField] float ClickIntensity = 2;
    [SerializeField] bool pulse;
    [SerializeField] bool wasPulsing, wasClicked, isHovering, wasHovering, shouldEndPulse;
    [SerializeField] float intensity = 0;
    [SerializeField] float _time;
    // Update is called once per frame
    void Update()
    {
        if(!IsActive && intensity <= 0)
            return;

        if(shouldEndPulse && !pulse)
        {
            wasPulsing = true;
            shouldEndPulse = false;
        }

        if(pulse && !isHovering && !wasPulsing && IsActive)
        {
            _time += Time.deltaTime; 
            intensity = Mathf.PingPong(_time*pulseSpeed,MaxPulseIntensity);
            setIntensity(intensity);
            shouldEndPulse = true;

            // if(wasHovering)
            //     wasHovering = false;
        }
        else if (intensity > 0 && wasPulsing && !isHovering || !IsActive)
        {
            intensity -= Time.deltaTime*pulseSpeed;
            setIntensity(intensity);
            
            if(intensity <= 0)
            {
                intensity = 0;
                wasPulsing = false;
                _time = 0;
            }

            if(!IsActive)
                return;
        }

        if(wasClicked)
        {
            _time += Time.deltaTime;
            intensity = Mathf.PingPong(_time*clickSpeed,ClickIntensity);
            setIntensity(intensity);

            if(intensity > ClickIntensity-0.1)
            {
                wasClicked = false;
                wasPulsing = true;
            }
        }

        if(isHovering)
        {
            intensity += Time.deltaTime*hoverSpeed;
            intensity = Mathf.Clamp(intensity, 0, HoverIntensity);

            setIntensity(intensity);
        }
        if(wasHovering)
        {
            _time = HoverIntensity;

            wasPulsing = true;

            wasHovering = false;
        }
    }

    private void OnMouseDown() 
    {
        if(!IsActive)
            return;

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

    void OnMouseEnter()
    {
        if(!IsActive)
            return;

        isHovering = true;
    }

    void OnMouseExit()
    {
        if(!IsActive)
            return;

        if(isHovering)
            wasHovering = true;

        isHovering = false;
    }
}
