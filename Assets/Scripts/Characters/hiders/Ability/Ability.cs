using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string abilityName;
    public string description;
    public abstract void UseAbility(GameObject player);
}
