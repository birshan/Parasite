using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static GameManager Instance { get; private set; }

    public List<GameObject> Aliens = new List<GameObject>();
    public GameObject reloadUI;
    public GameObject successUI;
    public int AliensDeactivated = 0;

    public int TimeToSurvive = 30;
    private int _timeRemaining;
    private float _clockFillAmount;
    private Color warningColor = Color.red; // Color when time is low
    private PlayerController playerController;

    void Awake(){
        if(Instance != null && Instance != this){
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        reloadUI.SetActive(false);
        successUI.SetActive(false);
        Time.timeScale = 1;
        _timeRemaining = TimeToSurvive;
        _clockFillAmount = 1f;
        // UIManager.Instance.UpdateClock(1f); // 30 seconds should be full. The 'clock' is a circular bar.
        UIManager.Instance.UpdateSecondsLeft(Mathf.CeilToInt(_timeRemaining));
        StartCoroutine(Clock());

        StartCoroutine(WaitOneSecondAndToggleHinTMode());

        
    }
    // Helper function to check if we are in the Tutorial scene
    public bool IsInTutorialScene()
    {
        return SceneManager.GetActiveScene().name == "Tutorial";
    }
    IEnumerator WaitOneSecondAndToggleHinTMode()
    {
        yield return new WaitForSeconds(1.0f);
        playerController.ToggleHintMode(true);
    }
    // Update is called once per frame
    void Update()
    {
        if(!IsInTutorialScene())
        {
            if(Aliens.Count == 0)
            {
                EnableReloadOnSuccess();
            }

            if(AliensDeactivated == Aliens.Count)
            {
                EnableReloadOnSuccess();
            }
        }
        
        
    }
    IEnumerator Clock()
    {
        _timeRemaining = TimeToSurvive;

        while(_timeRemaining>=0)
        {
            if (_timeRemaining <= 10) // Change color when 10 seconds or less
            {
                UIManager.Instance.Clock.color = warningColor;
            }
            _clockFillAmount= (float)_timeRemaining/TimeToSurvive;
            
            UIManager.Instance.UpdateClock(_clockFillAmount);
            UIManager.Instance.Clock.fillAmount = _clockFillAmount;
            UIManager.Instance.UpdateSecondsLeft(Mathf.CeilToInt(_timeRemaining));

            // Debug.Log("Time Remaining: " + _timeRemaining);
            // Debug.Log("Time To Survive: " + TimeToSurvive);
            // Debug.Log("Clock Fill Amount: " + _clockFillAmount);

            _timeRemaining--;
            yield return new WaitForSeconds(1.0f);
        }

        // EnableReloadOnGameOver();
        if(!IsInTutorialScene())
        {
            StartCoroutine(WaitOneSecondAndEnableReloadOnSuccess());
        }
        
        


    }
    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(1.0f);
    }
    public void EnableReloadOnGameOver()
    {
        // Enable the reload UI
        Time.timeScale = 0;
        reloadUI.SetActive(true);
    }

    IEnumerator WaitOneSecondAndEnableReloadOnSuccess(){
        yield return new WaitForSeconds(1.0f);
        // Enable the reload UI
        Time.timeScale = 0;
        successUI.SetActive(true);
    }
    public void EnableReloadOnSuccess()
    {

        // Enable the reload UI
        Time.timeScale = 0;
        successUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ReloadCurrentScene()
    {
        

        // Reload the scene
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // reloadUI.SetActive(false);
        // successUI.SetActive(false);
    }
}
