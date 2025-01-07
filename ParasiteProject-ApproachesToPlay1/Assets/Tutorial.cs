using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Tutorial : MonoBehaviour
{
    public TMP_Text TutorialText;
    public static int TutorialStage = 0;
    // Start is called before the first frame update
    public List<String> TutorialTexts = new List<string>();
    private List<HintableObject> HintableObjects = new List<HintableObject>();
    public GameObject AlienAwarenessBar;
    public Image AlienAwarenessBarImage;
    public GameObject HumanAwarenessBar;
    public Image HumanAwarenessBarImage;
    public List<GameObject> Humans = new List<GameObject>();
    public List<GameObject> Aliens = new List<GameObject>();
    // public List<VisionCone> VisionCones = new List<VisionCone>();
    private PlayerController playerController;
    void Start()
    {   TutorialStage = 0;
        TutorialText.text = "Use your mouse to look around and hold the left mouse button on purple 'Alien Relics', which will focus your vision and Hint at things in your environment.";
        
        foreach (var hintableObject in FindObjectsOfType<HintableObject>())
        {
            HintableObjects.Add(hintableObject);
            
        }

        // foreach ( var visionCone in FindObjectsOfType<VisionCone>())
        // {
        //     VisionCones.Add(visionCone);
        // }
        playerController = FindObjectOfType<PlayerController>();
    }   

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(HumanAwarenessBarImage.fillAmount);
        if(HumanAwarenessBarImage.fillAmount == 1 && TutorialStage == 1)
        {
            ContinueToNextTutorial();
        }   
    }

    public void ContinueToNextTutorial()
    {
        if(TutorialStage==4){
            Debug.Log("Tutorial Over");
            SceneManager.LoadScene("SampleScene");
        }
        Debug.Log(TutorialStage);
        TutorialStage++;
        TutorialText.text = TutorialTexts[TutorialStage];
        
        if(TutorialStage == 1)
        {
            foreach (var hintableObject in HintableObjects)
            {
                hintableObject.ResetHintableObject();
                
            }
            HumanAwarenessBar.SetActive(true);
            
        }
        if(TutorialStage == 3)
        {
            
            AlienAwarenessBar.SetActive(true);

            Aliens[0].gameObject.SetActive(true);
            // foreach (var visionCone in VisionCones)
            // {
            //     visionCone.ToggleVisibility(true);
            // }
            
        }
    }
}
