using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    
    [SerializeField] GameObject health;

    public void SetHP(float hpNormalized){
        health.transform.localScale = new Vector3(hpNormalized, 1f);
    }

// Reduce la barra de vida de manera gradual
    public IEnumerator setHPSmooth(float newHP){
        float currentHP = health.transform.localScale.x;
        float changeAmt = currentHP - newHP;

        while (currentHP - newHP > Mathf.Epsilon){
            currentHP -= changeAmt * Time.deltaTime;
            health.transform.localScale = new Vector3(currentHP, 1f);
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1f);
    }
}
