using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public bool activeWeapon = false; // start as false, and then make all weapons active once atleast one human is AWARE
    private MeshRenderer meshRenderer; //Disable on start, and activate weapons when atleast one human is awar 

    public static bool areWeaponsActivated = false;// Use to make sure the OnHumanAware events methods which are subscribed to it run only once.
    public static event Action OnHumansAware;//All instances of the weapons will subscribe to this method so that they activate themselves.
    public event Action<Transform> OnWeaponHintedEvent;// When the player uses the vision cone to hint at the weapon. - Humans will subscribe to this event, once they are AWARE.
    private bool weaponHinted = false;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;//Disable at satrt

        OnHumansAware += ActivateWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Focus(float deltaTime)
    {
        if(!weaponHinted)
        {
            weaponHinted = true;
            OnWeaponHintedEvent?.Invoke(transform);
        }
        
    }

    // Call this method when the object is being focused on

    private void ActivateWeapon()
    {
        activeWeapon = true;
        meshRenderer.enabled = true;
    }

    void OnDestroy()
    {
        OnHumansAware -= ActivateWeapon;
    }

    //This method will be called when a human enters the aware state. whchich will trigger the OnHumansAware event.
    public static void NotifyHumansAware()
    {
        if(!areWeaponsActivated)
        {
            areWeaponsActivated = true;
            OnHumansAware?.Invoke(); //ActivateWeapon is subscribed to this event, all weapons will activate.
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Human"))
        {
            //Parent the weapon to the human
            transform.parent = other.transform;
            //disable mesh renderer
            meshRenderer.enabled = false;
            //Disable trigger collider
            GetComponent<Collider>().enabled = false;
        }
    }

    
}
