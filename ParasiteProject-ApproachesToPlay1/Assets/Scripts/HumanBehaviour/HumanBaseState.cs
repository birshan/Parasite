using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HumanBaseState 
{

    public abstract void EnterState(HumanAI state);

    public abstract void UpdateState(HumanAI state);

    public abstract void OnHintEvent(Transform hintTransform);
}
