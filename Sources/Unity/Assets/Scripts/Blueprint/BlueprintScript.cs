using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueprintScript : MonoBehaviour
{

    public int index;
    
    private void Start()
    {
        if (!PlayerPrefs.HasKey("weaponBlueprint"))
        {
            PlayerPrefs.SetInt("weaponBlueprint", 0);
        }

        if (!PlayerPrefs.HasKey("propulsorBluePrint"))
        {
            PlayerPrefs.SetInt("propulsorBluePrint", 0);
        }

        if (!PlayerPrefs.HasKey("engineBlueprint"))
        {
            PlayerPrefs.SetInt("engineBlueprint", 0);
        }
    }

    private void library(int index)
    {
        switch (index)
        {
            case 0:
                PlayerPrefs.SetInt("weaponBlueprint",PlayerPrefs.GetInt("weaponBlueprint") + 1);
                break;
            case 1:
                PlayerPrefs.SetInt("propulsorBluePrint",PlayerPrefs.GetInt("propulsorBluePrint") + 1); 
                break;
            case 2:
                PlayerPrefs.SetInt("engineBlueprint",PlayerPrefs.GetInt("engineBlueprint") + 1); 
                break;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 6)
        {
            library(index);
            Destroy(gameObject);
        }
    }

    // ----- Testing purpose onyl ----- //
    
    private void OnDestroy()
    {
        if (PlayerPrefs.HasKey("weaponBlueprint"))
        {
            PlayerPrefs.DeleteKey("weaponBlueprint");    
        }

        if (PlayerPrefs.HasKey("propulsorBluePrint"))
        {
            PlayerPrefs.DeleteKey("propulsorBluePrint");
        }

        if (PlayerPrefs.HasKey("engineBlueprint"))
        {
            PlayerPrefs.DeleteKey("engineBlueprint");
        }
        
    }
}
