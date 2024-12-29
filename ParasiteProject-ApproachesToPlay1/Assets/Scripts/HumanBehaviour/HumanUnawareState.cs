using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanUnawareState: HumanBaseState
{
    private Transform currentHintTransform; // Stores the current hint transform
    private float lookAtHintDuration = 1f; // Duration to look at the hint
    private float lookAtHintTimer = 0; // Timer to track time looking at the hint
    private bool isHintEventTriggered = false; // Flag to track if currently looking at the hint
    private bool isRespondingToHint = false; // Flag to track if currently responding to hint
    
    public override void EnterState(HumanAI state)
    {
        Debug.Log("Human Unaware State Entered");
    }

    public override void UpdateState(HumanAI state)
    {
        // Debug.Log("Human Unaware State Update");
        // Check if the player is in the vision cone
        if(state.VisionCone.IsObjectInVision(state.PlayerTransform))
        {
            // Debug.Log("Player is within vision cone of human");
            // If player is in vision cone then player hints can be seen

            //if the OnHintEvent is triggered then turn to look at the hintTransform for a brief half a seconf and then turn to look bnack at the player
            if (isHintEventTriggered)
            {

                isRespondingToHint = true;
                
            }
            
        }

        if (isRespondingToHint)
        {
            // Look at the hint
            LookAtHint(state);
            lookAtHintTimer -= Time.deltaTime;
            if (lookAtHintTimer <= 0)
            {
                // Hint received- awareness increased
                state.AwarenessLevel = state.AwarenessLevel + 0.4f;
                UIManager.Instance.UpdateAwarenessBar(state.AwarenessLevel); // This will only work if theres an awareness bar for each human
                isHintEventTriggered = false;
                isRespondingToHint = false;
            }
        }
        
        else
        {
            // Regular behavior when not looking at a hint
            LookAtPlayer(state);
        }

        //If taken enough hints, then transition to aware state : TBD
    }

    public override void OnHintEvent(Transform hintTransform)
    {
        // Do nothing
        Debug.Log("Human Unaware State OnHintEvent");

        // Set up the hint behavior
        currentHintTransform = hintTransform;
        lookAtHintTimer = lookAtHintDuration;
        isHintEventTriggered = true;
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
    
}