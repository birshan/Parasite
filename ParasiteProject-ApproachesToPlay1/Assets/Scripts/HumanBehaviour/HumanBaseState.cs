using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class HumanBaseState 
{

    public abstract void EnterState(HumanAI state);

    public abstract void UpdateState(HumanAI state);

    public abstract void OnHintEvent(Transform hintTransform);

    public abstract void OnWeaponHintedEvent(Transform weaponTransform);
    public abstract void OnKillOrder(Transform alienTransform);

    public abstract bool GetIsArmed();

    public abstract bool GetIsKillTargetAquired();
  
}
