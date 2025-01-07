using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HintableObject : MonoBehaviour
{
    public Material defaultMaterial; // The material used when the object is not being focused
    public Material highlightedMaterial; // The material used when the object is highlighted
    private Renderer objectRenderer;
    private Transform playerTransform;
    public bool activeHintableObject = true;

    public event Action<Transform> OnHintEvent;

    // Event triggered when the hint is activated
    // public static event Action<HintableObject> OnHintActivated;
    // Start is called before the first frame update
    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        SetHighlighted(true); // Start in a highlighted state
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Call this method when the object is being focused on
    public void Focus(float deltaTime)
    {
        OnHintEvent?.Invoke(transform); // WIll hint through vision cone DetectHintableObjects method - only player will click to focus visino cone, so eventhough everyone has the vision cone it will work
        SetHighlighted(false);
        activeHintableObject = false;
    }

    // private void ActivateHint()
    // {
    //     // Trigger the hint activation event
    //     // OnHintEvent?.Invoke(this);
    //     // OnHintActivatedWithPlayer?.Invoke(this);

    //     // Debug log to simulate dialogue for now
    //     // Debug.Log($"Hint activated for object: {name}");
    // }

    private void SetHighlighted(bool highlighted)
    {
        if (highlighted)
            objectRenderer.material = highlightedMaterial;
        else
            objectRenderer.material = defaultMaterial;
    }

    public void ResetHintableObject()
    {
        SetHighlighted(true);
        activeHintableObject = true;
    }

}


// using System;
// using UnityEngine;

// public class HintableObject : MonoBehaviour
// {
//         [Header("Render")]
//         public Renderer Renderer;
//         [ColorUsage(false, true)]
//         public Color ColorOn;
//         public Color ColorOff;

//         [Header("Power")]
//         [Range(0f, 1f)]
//         public float T = 0;

//         [Range(0f, 1f)]
//         public float Decay = 0.1f; // Per second

//         public void Focus(float t)
//         {
//             T += t;
//             T = Mathf.Clamp01(T);
//         }

//         // Updates the colour every frame
//         void Update()
//         {
//             // Updates the colour
//             Color color = Color.Lerp(ColorOff, ColorOn, T);
//             Renderer.material.SetColor("_EmissionColor", color);

//             // Power decay
//             T -= Decay * Time.deltaTime;
//             T = Mathf.Clamp01(T);
//         }
// }
