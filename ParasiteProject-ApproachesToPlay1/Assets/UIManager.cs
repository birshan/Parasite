using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } 
    [SerializeField] private Image suspicionBar; // For Aliens
    [SerializeField] private Image awarenessBar; // For humans
    [SerializeField] private Image FocusBar; //Can only focus when full
    public Image Clock; //Clock to show time left
    [SerializeField] private Text SecondsLeft;
    // Start is called before the first frame update
    
    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateSuspicionBar(float value){
        suspicionBar.fillAmount = value;
    }

    public void UpdateAwarenessBar(float value){
        awarenessBar.fillAmount = value;
    }

    public void UpdateFocusBar(float value){
        FocusBar.fillAmount = value;
    }
    
    public void UpdateClock(float value){
        Clock.fillAmount = value;
    }
    public void UpdateSecondsLeft(int value){
        SecondsLeft.text = value.ToString();
    }
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
