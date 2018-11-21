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

    [SerializeField]
    private AudioClip flyShoot;

    [SerializeField]
    private AudioClip flyDeath;

    [SerializeField]
    private AudioClip grapplingHookShoot;

    [SerializeField]
    private AudioClip grapplingHookLatch;

    [SerializeField]
    private AudioClip[] bossAttacks;

    [SerializeField]
    private AudioClip bossImpact;

    [SerializeField]
    private AudioClip[] bossGroan;


    [SerializeField]
    private AudioClip healthGet;

    [SerializeField]
    private AudioClip hurt;

    [SerializeField]
    private AudioClip heal;

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
            a.volume = 0.5f;
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
            a.volume = 0.5f;
        }
        else if (soundclip == "flyShoot")
        {
            a.clip = flyShoot;
            a.volume = 0.5f;
        }
        else if (soundclip == "flyDeath")
        {
            a.clip = flyDeath;
        }
        else if (soundclip == "hookShoot")
        {
            a.clip = grapplingHookShoot;
        }
        else if (soundclip == "hookLatch")
        {
            a.clip = grapplingHookLatch;
        }
        else if (soundclip == "bossAttackShort")
        {
            a.clip = bossAttacks[Random.Range(0, 2)];
        }
        else if (soundclip == "bossAttackDebris")
        {
            a.clip = bossAttacks[2];
        }
        else if (soundclip == "bossGroan")
        {
            a.clip = bossGroan[0];
        }
        else if (soundclip == "bossImpact")
        {
            a.clip = bossImpact;
        }
        else if (soundclip == "healthGet")
        {
            a.clip = healthGet;
        }
        else if (soundclip == "hurt")
        {
            a.clip = hurt;
            a.spatialBlend = 0;
        }
        else if (soundclip == "whew")
        {
            a.clip = heal;
        }

        a.maxDistance = 15;
        a.minDistance = 5;
        g.transform.position = position;
        a.outputAudioMixerGroup = mixer.FindMatchingGroups("FX")[0];
        a.Play();
        Destroy(g, a.clip.length + 1);
    }



}
