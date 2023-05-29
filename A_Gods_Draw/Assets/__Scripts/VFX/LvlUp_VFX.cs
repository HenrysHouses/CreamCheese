using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlUp_VFX : MonoBehaviour
{
    [SerializeField] Texture2D[] NumberTextures;
    [SerializeField] Renderer MainRenderer;
    [SerializeField] ParticleSystem particle1;
    Material MainMat;
    float fillT, fillSpeed = 0.5f, fadeSpeed = 3;
    public int CurrentLevel, NextLevel;
    // Start is called before the first frame update
    void Start()
    {
        MainMat = MainRenderer.material;
        StartCoroutine(VFX_Animation());
        setTexture(CurrentLevel);
    }

    IEnumerator VFX_Animation()
    {
        while(fillT < 1)
        {
            fillT += Time.deltaTime * fillSpeed;
            setFill(fillT);
            yield return new WaitForEndOfFrame();
        }
        particle1.Play();
        setTexture(NextLevel);

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => particle1.particleCount == 0);

        fillT = 2;
        while(fillT > 0)
        {
            fillT -= Time.deltaTime * fadeSpeed;
            setTransparency(fillT);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject, 2);
    }

    void setTransparency(float value)
    {
        MainMat.SetFloat("_GlowPower", Mathf.Clamp(value, 0, 2));
    }

    void setFill(float value)
    {
        MainMat.SetFloat("_GlowFill", Mathf.Clamp01(value));
    }

    void setTexture(int level)
    {
        MainMat.SetTexture("_IconTexturte", NumberTextures[level]);
    }
}
