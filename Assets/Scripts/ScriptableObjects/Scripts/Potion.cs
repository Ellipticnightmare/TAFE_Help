using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Items/Potion", order = 0)]
public class Potion : ScriptableObject
{
    public Sprite invImage;
    public int goldValue;
    public float effectTimer, healVal;
    public stat[] StatToRecover, StatToDamage;
    public effect[] EffectGain;
    public enum stat
    {
        blank,
        health,
        mana,
        stamina,
        speed
    };
    public enum effect
    {
        blank,
        recovery,
        recharge,
        catchyourbreath
    };
}