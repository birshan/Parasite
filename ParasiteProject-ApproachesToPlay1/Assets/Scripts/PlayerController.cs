using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VisionCone visionCone;
    
    private bool IsHintMode = false; //Tracks whether hinting mode or not, vision cones onluy visuble in hinting nmode


    // These variables used for converting mouse position to world to direct player vision cone direction
    public Camera mainCamera;
    private Vector3 lastMousePosition; //So we dont need to call method every frame, we only do it when mouse position changes between frames.
    void Start()
    {
        visionCone = GetComponentInChildren<VisionCone>();
        
    }

    void Update()
    {   
        // Toggle Focus mode on key press (e.g., 'F')
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsHintMode = !IsHintMode;
            ToggleHintMode(IsHintMode);
        }

        // if(MouseMoved())
        // {
        //     UpdateVisionConeDirection();
        // }

        // Hint at something with your vision - where your vision cone contracts to focus at something as a hint to another character
        if (Input.GetMouseButton(0))
        {
            visionCone.FocusCone(true); // Hinting at someting
        }
        else
        {
            visionCone.FocusCone(false); // Normal mode
            // UpdateVisionConeDirection();
            if(MouseMoved())
            {
                UpdateVisionConeDirection();
            }
        }

        
    }

    // Toggle hinting mode effects
    void ToggleHintMode(bool enable)
    {
        visionCone.isHintMode = enable; //passing that info to vision cone
        // Find all vision cones in scene to toggle visibility
        VisionCone[] allVisionCones = FindObjectsOfType<VisionCone>();
        foreach (VisionCone cone in allVisionCones)
        {
            cone.ToggleVisibility(enable);
        }
        // visionCone.ToggleVisibility(enable); // Show/hide the vision cone

        // (Optional) Add highlighting logic for hintable objects here
        if (enable)
        {
            Debug.Log("Hint mode activated");
            // Highlight hintable objects
        }
        else
        {
            Debug.Log("Hint mode deactivated");
            // Remove highlights from objects
        }
    }

    bool MouseMoved()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        if(currentMousePosition != lastMousePosition)
        {
            lastMousePosition = currentMousePosition;
            return true;
        }
        return false;
    }
    void UpdateVisionConeDirection()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
             // Calculate the direction to the mouse position
            Vector3 directionToMouse = (hit.point - transform.position).normalized;
            // Debug.Log("Direction to mouse: " + directionToMouse);
            // Rotate the player's transform to point toward the mouse (optional, only if needed)
            transform.forward = new Vector3(directionToMouse.x, 0, directionToMouse.z);

            // Rotate the vision cone to align with the player's forward direction
            // visionCone.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
    }
}
