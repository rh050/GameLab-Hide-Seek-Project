using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Ability : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string description;

    public abstract void UseAbility(GameObject player);
}
