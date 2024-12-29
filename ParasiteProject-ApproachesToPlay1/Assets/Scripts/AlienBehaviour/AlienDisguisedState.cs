using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AlienDisguisedState : AlienBaseState
{
    private Transform currentHintTransform; // Stores the current hint transform
    private float lookAtHintDuration = 1f; // Duration to look at the hint
    private float lookAtHintTimer = 0; // Timer to track time looking at the hint
    private float lookAtPlayerDuration = 3f; // Duration to look at the player
    private float lookAtPlayerTimer = 0; // Timer to track time looking at the player
    private float responseDelayDuration = 2f; // Delay before responding to the hint
    private float responseDelayTimer = 0; // Timer to track the response delay
    private bool isLookingAtPlayer = false; // Flag to track if currently looking at the player
    private bool isHintEventTriggered = false; // Flag to track if currently looking at the hint
    private bool isRespondingToHint = false; // Flag to track if currently responding to hint
    private float randomDirectionChangeInterval = 3f; // Interval between direction changes
    private float randomDirectionTimer = 0; // Timer to track random direction changes
    private Vector3 currentRandomDirection; // Stores the current random direction

    public override void EnterState(AlienAI state)
    {
        Debug.Log("Alien Disguised State Entered");
        randomDirectionTimer = randomDirectionChangeInterval; // Initialize the timer
        currentRandomDirection = GetNewRandomDirection(state); // Set an initial random direction
    }

    private IEnumerator WaitToRespond(AlienAI state)
    {
        if (!isRespondingToHint) // Prevent multiple coroutines from running
        {
            yield return new WaitForSeconds(1);
            isRespondingToHint = true;
        }
    }
    public override void UpdateState(AlienAI state)
    {
        if (state.VisionCone.IsObjectInVision(state.PlayerTransform))
        {
            Debug.Log("Player is within vision cone of Alien");

            if (isHintEventTriggered)
            {
                // isRespondingToHint = true;4
                state.StartCoroutine(WaitToRespond(state));
            }
        }

        if (isRespondingToHint)
        {
            // Look at the hint after the delay
            LookAtHint(state);
            lookAtHintTimer -= Time.deltaTime;
            if (lookAtHintTimer <= 0)
            {
                //Got caught - Hint received
                AlienAI.SuspicionLevel = AlienAI.SuspicionLevel + 0.4f;
                UIManager.Instance.UpdateSuspicionBar(AlienAI.SuspicionLevel);
                isHintEventTriggered = false;
                isRespondingToHint = false;
                isLookingAtPlayer = true;
                lookAtPlayerTimer = lookAtPlayerDuration;
            }
        }
        else if (isLookingAtPlayer)
        {
            // Look at the player for a few seconds
            LookAtPlayer(state);
            lookAtPlayerTimer -= Time.deltaTime;
            // while lo0oking at player if player still hints at another object then full suspicion level
            if(isHintEventTriggered)
            {
                AlienAI.SuspicionLevel = 1.0f;
                UIManager.Instance.UpdateSuspicionBar(AlienAI.SuspicionLevel);
                // VISION CONE TURN RED ??

                
            }
            if (lookAtPlayerTimer <= 0)
            {
                isLookingAtPlayer = false;
            }
        }
        else
        {
            // Default behavior: look in random directions, updating every 3 seconds
            randomDirectionTimer -= Time.deltaTime;
            if (randomDirectionTimer <= 0)
            {
                randomDirectionTimer = randomDirectionChangeInterval;
                currentRandomDirection = GetNewRandomDirection(state);
            }
            LookAtDirection(state, currentRandomDirection);
        }
    }

    public override void OnHintEvent(Transform hintTransform)
    {
        Debug.Log("Alien Disguised State OnHintEvent");

        // Set up hint behavior
        currentHintTransform = hintTransform;
        lookAtHintTimer = lookAtHintDuration;
        responseDelayTimer = responseDelayDuration; // Start the delay timer
        isHintEventTriggered = true;
    }

    private void LookAtHint(AlienAI state)
    {
        if (currentHintTransform != null)
        {
            // Rotate to face the hintTransform
            Vector3 direction = currentHintTransform.position - state.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
        }
    }

    private void LookAtPlayer(AlienAI state)
    {
        if (state.PlayerTransform != null)
        {
            // Rotate to face the player
            Vector3 direction = state.PlayerTransform.position - state.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
        }
    }

    private void LookAtDirection(AlienAI state, Vector3 direction)
    {
        // Rotate to face the specified direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        state.transform.rotation = Quaternion.Slerp(state.transform.rotation, targetRotation, Time.deltaTime * state.RotationSpeed);
    }

    private int randomDirectionCallCount = 0; // Counter to track the number of calls

    private Vector3 GetNewRandomDirection(AlienAI state)
    {
        randomDirectionCallCount++;

        // Every 4th call, return the direction towards the player
        if (randomDirectionCallCount % 4 == 0 && state.PlayerTransform != null)
        {
            Vector3 playerDirection = (state.PlayerTransform.position - state.transform.position).normalized;
            return playerDirection;
        }

        // Otherwise, return a random direction
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

}
