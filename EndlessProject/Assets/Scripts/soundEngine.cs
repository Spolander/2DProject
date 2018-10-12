using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class soundEngine : MonoBehaviour
{

    public static soundEngine soundMaster;

    [SerializeField]
    private AudioClip[] gun1;

    [SerializeField]
    private AudioClip rockExplosion;


    public AudioMixer mixer;

    private void Awake()
    {
        soundMaster = this;
    }

    public void VictorySound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void PlaySound(string soundclip)
    {
        GameObject g = new GameObject("Sound clip");
        AudioSource a = g.AddComponent<AudioSource>();

        a.spatialBlend = 0.25f;
        a.dopplerLevel = 0;
        if (soundclip == "gun1")
        {
            a.spatialBlend = 0;
            a.clip = gun1[Random.Range(0, gun1.Length)];
            a.volume = 1f;
        }
        else if (soundclip == "rockExplosion")
        {
            a.clip = rockExplosion;
        }
       
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("FX")[0];
        a.Play();
        Destroy(g, a.clip.length + 1);
    }



}
