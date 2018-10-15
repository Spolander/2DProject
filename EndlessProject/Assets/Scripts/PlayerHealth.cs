using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth {

    [SerializeField]
    float postHitIframes = 2f;

    private float lastHitTime;

    [SerializeField]
    private float flashCycles;

    Coroutine flash;

    Material m;

    private void Start()
    {
        m = GetComponent<SpriteRenderer>().material;
        lastHitTime = -postHitIframes;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

                TakeDamage();
            
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("enemyProjectile"))
        {

                TakeDamage();
            
        }
    }

    IEnumerator damageFlash()
    {
        float flashValue = 0;

        float maxValue = 0.75f;
        float target = maxValue;


        float timer = 0;
        m.SetFloat("_MaskAmount", flashValue);
        while (timer < postHitIframes && gameObject)
        {
            timer += Time.deltaTime;
            flashValue = Mathf.MoveTowards(flashValue, target, Time.deltaTime * (maxValue / postHitIframes) * 2*flashCycles);
            m.SetFloat("_MaskAmount", flashValue);
            if (Mathf.Approximately(target, flashValue))
            {
                target = target == maxValue ? 0 : maxValue;

            }

            yield return null;
        }
        m.SetFloat("_MaskAmount", 0);
    }
    public override void TakeDamage(int damage = 1)
    {
        if (Time.time < lastHitTime + postHitIframes)
            return;

        lastHitTime = Time.time;
        hitPoints -= damage;

        if (hitPoints <= 0)
        {
            Death();
            return;
        }
           

        if (flash != null)
            StopCoroutine(flash);

        flash = StartCoroutine(damageFlash());
        
    }
}
