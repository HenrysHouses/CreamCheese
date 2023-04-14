using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.Effects;
using FMODUnity;
using HH.MultiSceneTools;

public class DeathCrumbling : MonoBehaviour
{
    private Light _light;
    public bool dying, dead, trulyDead;
    private Animator anim;
    public CameraEffects camEffs;
    public GameObject music;
    public EventReference stonerumble;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _light = GetComponentInChildren<Light>();
        camEffs = Camera.main.GetComponent<CameraEffects>();

    }

    // Update is called once per frame
    void Update()
    {

        if (!dying)
            return;

        _light.intensity = Mathf.PingPong(Time.time * 100, 80);
        CameraEffects.ShakeOnce(0.1f, 1.5f);
        anim.SetBool("Dying", true);
      //  music.SetActive(false);


        // if (trulyDead && Input.anyKeyDown)
        if (trulyDead)
        {
            // LoadingScreen.Instance.EnterLoadingScreen("CutScene", collectionLoadMode.Difference);
            MultiSceneLoader.loadCollection("MainMenu", collectionLoadMode.Difference);
            GameManager.instance.PlayerTracker.resetHealth();
            GameManager.instance.PlayerTracker.CurrentRunes.Clear();
            CardQuantityContainer newSave = new CardQuantityContainer();
            GameSaver.SaveData(newSave);

        }

        if (dead)
            return;

        dead = true;
        SoundPlayer.PlaySound(stonerumble, gameObject);
    }
}
