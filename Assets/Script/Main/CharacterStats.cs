using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stat damage;
    public Stat maxHp;

    [SerializeField] private int currHp;

    protected virtual void Start()
    {
        currHp = maxHp.GetValue();
    }

    public virtual void TakeDamage(int damage)
    {
        currHp -= damage;

        if(currHp < 0)
        {
            Die();
        }
    }

    public virtual void DoDamage(CharacterStats targetStats)
    {
        int totalDamage = damage.GetValue();

        targetStats.TakeDamage(totalDamage);
    }

    protected virtual void Die()
    {
        throw new NotImplementedException();
    }
}
