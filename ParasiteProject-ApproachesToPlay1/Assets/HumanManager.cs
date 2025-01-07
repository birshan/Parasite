using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : MonoBehaviour
{
    
    private static List<HumanAI> awareUnarmedHumans = new List<HumanAI>();
    private static List<HumanAI> awareArmedHumans = new List<HumanAI>();
    
    public List<Weapon> Weapons = new List<Weapon>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (var weapon in FindObjectsOfType<Weapon>())
        {
            Weapons.Add(weapon);
            weapon.OnWeaponHintedEvent += AssignWeaponToHuman;
        }

        foreach (var alien in FindObjectsOfType<AlienAI>())
        {
            alien.OnKillOrderEvent += OrderArmedHumanToKillAliens;
        }
    }
    // Will only add a human once the human becomes aware. 
    public static void RegisterAwareHuman(HumanAI human)
    {
        if(!human.GetIsArmed() && !awareUnarmedHumans.Contains(human))
        {
            awareUnarmedHumans.Add(human);
        }

        // Debug.Log(awareUnarmedHumans.Count);
    }

    // Ensure each human moves one at a time to weapon by calling assign weapon through human manager. 
    public static void AssignWeaponToHuman(Transform weaponTransform)
    {
        Debug.Log("Assigning weapon to human");
        if(awareUnarmedHumans.Count > 0)
        {
            HumanAI nextHuman = awareUnarmedHumans[0];
            awareUnarmedHumans.RemoveAt(0);
            nextHuman.HandleWeaponHintEvent(weaponTransform);
            
            awareArmedHumans.Add(nextHuman);
        }
    }

    public static void OrderArmedHumanToKillAlien(Transform alienTransform)
    {
        if(awareArmedHumans.Count > 0)
        {
            HumanAI nextHuman = awareArmedHumans[0];
            awareArmedHumans.RemoveAt(0);
            if(!nextHuman.GetIsKillTargetAquired()){
                nextHuman.OrderKillEvent(alienTransform);
                Debug.Log("Human is ordered to kill alien");
            }
            
        }
    }

    public static void OrderArmedHumanToKillAliens(Transform aTransform)
    {
       ;
        if(awareArmedHumans.Count==0 || GameManager.Instance.Aliens.Count==0)
        {
            return;
        }

        //to assign each human to kill an alien - how many assignments can be made
        int assignments = Mathf.Min(awareArmedHumans.Count, GameManager.Instance.Aliens.Count);
        
        for (int i = 0; i < assignments; i++)
        {
            HumanAI nextHuman = awareArmedHumans[i];
            // awareArmedHumans.RemoveAt(0);
            if(!nextHuman.GetIsKillTargetAquired()){
                nextHuman.OrderKillEvent(GameManager.Instance.Aliens[i].transform);
            }
        }
    }
    //Issue persists because in AlienAI the event is triggered whenever we focus there, but we only want it to trigger for humans who are aware state

    // Update is called once per frame
    void Update()
    {
        
    }
}
