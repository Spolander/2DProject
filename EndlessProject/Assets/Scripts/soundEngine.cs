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

    [SerializeField]
    private AudioClip spiderScream;

    [SerializeField]
    private AudioClip spiderDeath;

    [SerializeField]
    private AudioClip boulderHit;

    [SerializeField]
    private AudioClip impactSound;

    public AudioMixer mixer;

    private void Awake()
    {
        soundMaster = this;
    }

    public void VictorySound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void PlaySound(string soundclip,Vector3 position)
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
        else if (soundclip == "spiderScream")
        {
            a.clip = spiderScream;
        }
        else if (soundclip == "spiderDeath")
        {
            a.clip = spiderDeath;
        }
        else if (soundclip == "boulderHit")
        {
            a.clip = boulderHit;
        }
        else if (soundclip == "impactSound")
        {
            a.clip = impactSound;
            a.volume = 0.7f;
        }

        g.transform.position = position;
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("FX")[0];
        a.Play();
        Destroy(g, a.clip.length + 1);
    }



}
