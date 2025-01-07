using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HumanAwareState: HumanBaseState
{
    private Transform currentHintTransform; // Stores the current hint transform
    private float lookAtHintDuration = 1f; // Duration to look at the hint
    private float lookAtHintTimer = 0; // Timer to track time looking at the hint
    private bool isHintEventTriggered = false; // Flag to track if currently looking at the hint
    private bool isRespondingToHint = false; // Flag to track if currently responding to hint
    private int currentAlienIndex = 0;
    private Vector3 currentlyLookedAtAlienDirection;
    private float lookAtAlienDirectionDuration = 1f;
    private float lookAtAlienDirectionTimer = 0;
    private Vector3 targetPosition;
    private bool shouldMove = false;
    private bool isArmed = false;
    private bool isKillTargetAquired = false;
    private Transform _alienTargetTrasform;
    private bool hasCarriedOutKillOrder = false;
    public override void EnterState(HumanAI state)
    {
        Debug.Log("Human Aware State Entered");

        Weapon.NotifyHumansAware(); //Will activate weapons
        HumanManager.RegisterAwareHuman(state);
    }

    public override void UpdateState(HumanAI state)
    {
        state.VisionCone.FocusNPCCone(true);

        if(!isKillTargetAquired)
        {
            lookAtAlienDirectionTimer -= Time.deltaTime;
            if(lookAtAlienDirectionTimer <= 0)
            {
                lookAtAlienDirectionTimer = lookAtAlienDirectionDuration;
                currentlyLookedAtAlienDirection = GetLookAtAlienDirections(state);
            }
            LookAtDirection(state, currentlyLookedAtAlienDirection);
        }
        else
        {
            Vector3 alienPos = _alienTargetTrasform.position;
            Vector3 lookDir = alienPos - state.transform.position;
            LookAtDirection(state, lookDir);
            if(!hasCarriedOutKillOrder)
            {
                hasCarriedOutKillOrder = true;
                state.StartCoroutine(DeactivateAlien(_alienTargetTrasform.gameObject));
            }
        }
        

        if(shouldMove)
        {
            state.transform.position = Vector3.MoveTowards(state.transform.position, targetPosition, state.MoveSpeed * Time.deltaTime);
            if(Vector3.Distance(state.transform.position, targetPosition) < 0.1f)
            {
                shouldMove = false;
            }
        }
    }

    public override void OnHintEvent(Transform hintTransform)
    {
        // Do nothing
        // Debug.Log("Human Unaware State OnHintEvent");

        // Set up the hint behavior
        currentHintTransform = hintTransform;
        lookAtHintTimer = lookAtHintDuration;
        isHintEventTriggered = true;
    }

    // WEAPON IS HINTED AT- MOVE TO WEAPON TO PICK IT UP
    public override void OnWeaponHintedEvent(Transform weaponTransform)
    {
        if(!isArmed){
            // throw new System.NotImplementedException();
            targetPosition = weaponTransform.position;
            shouldMove = true;
            isArmed = true;
        }
        
        
    }
    public override void OnKillOrder(Transform alienTransform)
    {
        if(!isKillTargetAquired)
        {
            isKillTargetAquired = true;
            _alienTargetTrasform = alienTransform;
            
        }
        // Shoot Gun sound effect

        //Turn vision cone red

        //Look at alien


        // Deactivate Alien 
        // alienTransform.gameObject.SetActive(false);

        //start couroutine to deactiveate alien after delay
        // StartCoroutine(DeactivateAlien(alienTransform));
    }

    private IEnumerator DeactivateAlien(GameObject alien)
    {
        yield return new WaitForSeconds(1.0f);
        alien.SetActive(false);
        GameManager.Instance.AliensDeactivated++;
    }    
    public override bool GetIsArmed()
    {
        return isArmed;
    }
    

    public override bool GetIsKillTargetAquired()
    {
        return isKillTargetAquired;
    }
    

    private void LookAtHint(HumanAI state)
    {
        if (currentHintTransform != null)
        {
            // Rotate to face the hintTransform
            Vector3 direction = currentHintTransform.position - state.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
        }
    }

    private void LookAtPlayer(HumanAI state)
    {
        if (state.PlayerTransform != null)
        {
            // Rotate to face the player
            Vector3 direction = state.PlayerTransform.position - state.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
        }
    }
    private void LookAtDirection(HumanAI state, Vector3 direction)
    {
        // Rotate to face the specified direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
    }

    private Vector3 GetLookAtAlienDirections(HumanAI state)
{
    // Ensure the Aliens list is not empty
    if (GameManager.Instance.Aliens == null || GameManager.Instance.Aliens.Count == 0)
    {
        // Debug.LogWarning("Aliens list is empty!");
        return Vector3.zero; // Return a default direction
    }

    // Get the current alien's position
    GameObject currentAlien = GameManager.Instance.Aliens[currentAlienIndex];
    Vector3 alienDirection = currentAlien.transform.position - state.transform.position;

    // Update the index to the next alien, looping back to the start if necessary
    currentAlienIndex = (currentAlienIndex + 1) % GameManager.Instance.Aliens.Count;
    // Debug.Log("Current Alien Index: " + currentAlienIndex);
    return alienDirection.normalized; // Return the normalized direction vector
}
    
}