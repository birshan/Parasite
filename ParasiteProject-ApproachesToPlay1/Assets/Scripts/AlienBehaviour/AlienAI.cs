using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AlienAI : MonoBehaviour
{
    public VisionCone VisionCone;
    private AlienBaseState _currentState;
    public AlienDisguisedState DisguisedState = new AlienDisguisedState();  

    public Transform PlayerTransform;
    // public HintableObject HintableObject;
    public List<HintableObject> HintableObjects = new List<HintableObject>();

    public float RotationSpeed = 4.0f;

    public static float SuspicionLevel = 0.0f;

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
        _currentState = DisguisedState;
    }

    private void Update()
    {
        _currentState.UpdateState(this);
    }

    public void SwitchState(AlienBaseState newState)
    {
        _currentState = newState;
        _currentState.EnterState(this);
    }
    
   
}