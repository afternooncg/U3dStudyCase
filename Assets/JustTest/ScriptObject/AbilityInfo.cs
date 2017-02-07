using UnityEngine;
using System.Collections;

public class AbilityInfo : ScriptableObject {

    public enum Type
    {
        Unknown,
        FireBall,
        FireWall,
        IceBall,
        DarkForce,
        Heal,
        Poison,
    }

    public Type type = Type.Unknown;
    public float point = 10.0f;
    public float affectRange = 100.0f;
}
