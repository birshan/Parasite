using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanAI : MonoBehaviour
{
    public VisionCone VisionCone;
    private HumanBaseState _currentState;
    public HumanUnawareState UnawareState = new HumanUnawareState();    
    public HumanAwareState AwareState = new HumanAwareState();
    public Transform PlayerTransform;
    // public HintableObject HintableObject;
    public List<HintableObject> HintableObjects = new List<HintableObject>();
    // public List<Weapon> Weapons = new List<Weapon>();// ---  Moved to HumanManager
    public float RotationSpeed = 1.0f;
    public float AwarenessLevel = 0.0f;
    public float MoveSpeed = 3.0f;
    public bool isKillTargetAquired = false;
    // public bool isArmed = false; //have to get it from the aware state so made getter method
    // public float LookAtHintDuration = 1.0f;
    private void OnEnable()
    {
        // HintableObject.OnHintEvent += HandleHintEvent;
        foreach (var hintableObject in FindObjectsOfType<HintableObject>())
        {
            HintableObjects.Add(hintableObject);
            hintableObject.OnHintEvent += HandleHintEvent;
        }

        // foreach (var weapon in FindObjectsOfType<Weapon>())
        // {
        //     Weapons.Add(weapon);
        //     weapon.OnWeaponHintedEvent += HandleWeaponHintEvent;
        // }
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

    public void HandleWeaponHintEvent(Transform weaponTransform)
    {
        if(_currentState!= null){
            _currentState.OnWeaponHintedEvent(weaponTransform);
        }
    }
    public void OrderKillEvent(Transform alienTransform)
    {
        if(_currentState!= null){
            _currentState.OnKillOrder(alienTransform);
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
        if(GameManager.Instance.IsInTutorialScene())
        {
            if(Tutorial.TutorialStage == 0){
                AwarenessLevel = 0.0f;
            }
        }
    }

    public void SwitchState(HumanBaseState newState)
    {
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public bool GetIsArmed()
    {
        return _currentState.GetIsArmed();
    }
    
    public bool GetIsKillTargetAquired()
    {
        return _currentState.GetIsKillTargetAquired();
    }
   
}