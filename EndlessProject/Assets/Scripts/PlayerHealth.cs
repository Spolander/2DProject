using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : BaseHealth {

    [SerializeField]
    float postHitIframes = 2f;

    private float lastHitTime;

    [SerializeField]
    private float flashCycles;

    Coroutine flash;

    Material m;

    [SerializeField]
    private Sprite healthSprite;

    [SerializeField]
    private Sprite healthGoneSprite;

    [SerializeField]
    Image[] healthBarSlots;

    [SerializeField]
    private GameObject healthExplosion;

    public static bool canTakeDamage = true;

    private void Start()
    {
        canTakeDamage = true;
        m = GetComponent<SpriteRenderer>().material;
        lastHitTime = -postHitIframes;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            TakeDamage();

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
        {
            TakeDamage();
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("enemyProjectile"))
        {

            TakeDamage();

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "health" && hitPoints < maxHitPoints)
        {
            Instantiate(healthExplosion, collision.transform.position, Quaternion.identity);
            Heal(1);
            Destroy(collision.gameObject);
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

        if (canTakeDamage == false)
            return;

        lastHitTime = Time.time;
        hitPoints -= damage;

        if (healthBarSlots != null)
        {
            if (healthBarSlots.Length > 0)
            {
                healthBarSlots[hitPoints].overrideSprite = healthGoneSprite;
            }
        }
           

        if (hitPoints <= 0)
        {
            Death();
            return;
        }
           

        if (flash != null)
            StopCoroutine(flash);

        flash = StartCoroutine(damageFlash());
        
    }

    public override void Heal(int amount)
    {
        base.Heal(amount);


        if (healthBarSlots != null)
        {
            if (healthBarSlots.Length > 0)
            {
                for (int i = 0; i < hitPoints; i++)
                {
                    healthBarSlots[i].overrideSprite = healthSprite;
                }
            }
        }

       
    }
}
