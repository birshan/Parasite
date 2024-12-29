using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    public VisionCone VisionCone;
    private HumanBaseState _currentState;
    public HumanUnawareState UnawareState = new HumanUnawareState();    
    public Transform PlayerTransform;
    // public HintableObject HintableObject;
    public List<HintableObject> HintableObjects = new List<HintableObject>();
    public float RotationSpeed = 1.0f;
    public float AwarenessLevel = 0.0f;
    // public float LookAtHintDuration = 1.0f;
    private void OnEnable()
    {
        // HintableObject.OnHintEvent += HandleHintEvent;
        foreach (var hintableObject in FindObjectsOfType<HintableObject>())
        {
            HintableObjects.Add(hintableObject);
            hintableObject.OnHintEvent += HandleHintEvent;
        }
    }
    private void OnDisable() {
        // HintableObject.OnHintEvent -= HandleHintEvent;
        foreach (var hintableObject in HintableObjects)
        {
            hintableObject.OnHintEvent -= HandleHintEvent;
        }
        HintableObjects.Clear();
    }
    private void HandleHintEvent(Transform hintTransform)
    {
        if(_currentState!= null){
            _currentState.OnHintEvent(hintTransform);
        }
    }
    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        VisionCone = GetComponentInChildren<VisionCone>();
    }
    
    private void Start()
    {
        _currentState = UnawareState;
    }

    private void Update()
    {
        // Debug.Log("HumanAI Update");
        _currentState.UpdateState(this);
    }

    public void SwitchState(HumanBaseState newState)
    {
        _currentState = newState;
        _currentState.EnterState(this);
    }
   
}