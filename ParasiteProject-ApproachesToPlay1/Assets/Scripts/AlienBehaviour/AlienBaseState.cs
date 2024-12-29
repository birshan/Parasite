using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AlienBaseState 
{
    public abstract void EnterState(AlienAI state);

    public abstract void UpdateState(AlienAI state);
    public abstract void OnHintEvent(Transform hintTransform);
}
