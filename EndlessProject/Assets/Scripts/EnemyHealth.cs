using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : BaseHealth {

    Material m;


    [SerializeField]
    private float flashDuration = 0.15f;

    protected Coroutine flash;

    protected virtual void Start()
    {
        m = GetComponent<SpriteRenderer>().material;   
    }


    public override void TakeDamage(int damage = 1)
    {

        base.TakeDamage(damage);
        soundEngine.soundMaster.PlaySound("impactSound", transform.position);
        if (hitPoints <= 0)
            return;

        if (flash != null)
            StopCoroutine(flash);

        flash = StartCoroutine(damageFlash());
    }

    public override void Death()
    {
        GetComponent<Enemy>().OnDeath();

        GetComponent<Collider2D>().enabled = false;
    }
    protected IEnumerator damageFlash()
    {
        float flashValue = 0;

        float maxValue = 0.75f;
        float target = maxValue;
        

        float timer = 0;
        m.SetFloat("_MaskAmount", flashValue);
        while (timer < flashDuration && gameObject)
        {
            timer += Time.deltaTime;
            flashValue = Mathf.MoveTowards(flashValue, target, Time.deltaTime * (maxValue / flashDuration)*2);
            m.SetFloat("_MaskAmount", flashValue);
            if (Mathf.Approximately(target, flashValue))
            {
                target = target == maxValue ? 0 : maxValue;

            }

            yield return null;
        }
        m.SetFloat("_MaskAmount", 0);
    }



}
