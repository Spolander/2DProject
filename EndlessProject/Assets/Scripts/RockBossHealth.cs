using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RockBossHealth : EnemyHealth {

    int quarterHealth;
    int lastHp;
    int startingHP;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private AudioSource bossMusic;

    [SerializeField]
    private Image whiteBackground;

    [SerializeField]
    private AudioSource levelComplete;

    [SerializeField]
    private StageCompleteAnimation sca;
    protected override void Start()
    {
        base.Start();
        startingHP = hitPoints;
        lastHp = hitPoints;
        quarterHealth = hitPoints / 4;
    }

    public override void TakeDamage(int damage = 1)
    {

        if (hitPoints <= 0)
            return;

        hitPoints -= damage;

        if (flash != null)
            StopCoroutine(flash);

        flash = StartCoroutine(damageFlash());

        if (hitPoints <= lastHp - quarterHealth)
        {
            lastHp = hitPoints;
            GetComponent<RockBoss>().ChangePhase(RockBoss.AttackPhase.RockStorm);
            soundEngine.soundMaster.PlaySound("bossGroan", transform.position);
        }

        if (hitPoints <= 0)
        {
            PlayerHealth.canTakeDamage = false;
            GetComponent<RockBoss>().enabled = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<RockBoss>().ClearDebris();
            StartCoroutine(deathAnimation());
        }

    }

    IEnumerator deathAnimation()
    {

        float deathTime = 5;
        float timer = 0;
        float explosionInterval = 0.15f;
        float spawnTimer = explosionInterval;

        Vector2 randomPos = new Vector2(1, 1);
        while (timer < deathTime)
        {
            spawnTimer += Time.deltaTime;
            bossMusic.volume = Mathf.MoveTowards(bossMusic.volume, 0, Time.deltaTime);

            if (timer > 1 && bossMusic.isPlaying)
                bossMusic.Stop();

            if (spawnTimer >= explosionInterval)
            {
                soundEngine.soundMaster.PlaySound("rockExplosion", transform.position);
                GameObject g = (GameObject)Instantiate(explosionPrefab, transform.position + new Vector3(Random.Range(-randomPos.x, randomPos.x), Random.Range(-randomPos.y, randomPos.y), 0), Quaternion.identity);
                g.GetComponent<SpriteRenderer>().sortingOrder = 6;
                spawnTimer = 0;
            }

            if (timer > deathTime / 2)
            {
                Color c = Color.white;
                c.a = Mathf.Lerp(0, 1,(timer-(deathTime/2))/(deathTime/2));
                whiteBackground.color = c;
            }
            timer += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1);

        float lerp = 0;

        foreach (SpriteRenderer r in GetComponentsInChildren<SpriteRenderer>())
            r.enabled = false;
        while (lerp < 1)
        {
            lerp += Time.deltaTime;
            Color c = Color.white;
            c.a = Mathf.Lerp(1, 0, lerp);
            whiteBackground.color = c;
            yield return null;
        }
        sca.ActivateAnimation();
        levelComplete.Play();
        Destroy(gameObject);
        
    }
}
