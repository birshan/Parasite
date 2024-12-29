using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    public float coneRadius = 5f;
    public float coneAngle = 45f;
    public int segments = 20;
    private Mesh mesh;
    private MeshRenderer meshRenderer;
    public float detectionInterval = 0.1f; // How often to check for objects
    private float detectionTimer = 0f;

    public bool isHintMode;
    public bool isFocused;
    // Hintable Objects
    public LayerMask hintableLayer;

    public event Action<Transform> OnHintActivatedEvent; 
    void Start() {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled= false;// Hide the cone by default
        GenerateCone();
    }

    // Hintable Objects
    private void Update(){
        detectionTimer += Time.deltaTime;
        if(detectionTimer >= detectionInterval){
            detectionTimer = 0f;
            DetectHintableObjects();
        }
    }
    private void DetectHintableObjects()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, coneRadius, hintableLayer);

        foreach (var hit in hitObjects)
        {
            Vector3 directionToObject = hit.transform.position - transform.position;
            float angle = Vector3.Angle(transform.forward, directionToObject);

            if (angle <= coneAngle / 2)
            {
                HintableObject hintableObject = hit.GetComponent<HintableObject>();
                if (hintableObject != null && hintableObject.activeHintableObject)
                {
                    if(isHintMode && isFocused)
                    {
                        hintableObject.Focus(Time.deltaTime);
                        // hintableObject.Focus();
                    }                    
                }
            }
        }
    }
    void GenerateCone(){
        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Center point

        float angleStep = coneAngle / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -coneAngle / 2 + i * angleStep;
            float rad = Mathf.Deg2Rad * angle;

            vertices[i + 1] = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad)) * coneRadius;
            if (i < segments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    public void UpdateCone(float newRadius, float newAngle){
        coneRadius = newRadius;
        coneAngle = newAngle;
        GenerateCone();
    }
    public void FocusCone(bool focus){
        
        float targetRadius = focus ?7f:5f;
        float targetAngle = focus ? 10f:45f;

        UpdateCone(Mathf.Lerp(coneRadius, targetRadius, Time.deltaTime * 5f), Mathf.Lerp(coneAngle, targetAngle, Time.deltaTime * 5f));
        isFocused = focus;
    }

     public void ToggleVisibility(bool isVisible)
    {
        meshRenderer.enabled = isVisible; // Show/hide the cone
    }

    public bool IsObjectInVision(Transform target){
        // Debug.Log("Checking if object is in vision cone");
        // Debug.Log("Target position: "+target.position);
        // Debug.Log("Transform position: "+transform.position);
        Vector3 directionToTarget = target.position - transform.position;
        // Debug.Log("Direction to target: "+directionToTarget);
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);  
        if(angleToTarget<=coneAngle/2 && directionToTarget.magnitude <= coneRadius){
            return true;
        }
        return false;
    }
}
