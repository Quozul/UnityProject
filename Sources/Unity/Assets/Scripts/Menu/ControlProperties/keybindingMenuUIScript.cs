using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class keybindingMenuUIScript : MonoBehaviour
{
    
    [SerializeField] private GameObject accessor;
    private PlayerController _controller;

    public List<InputActionReference> keybindings = new List<InputActionReference>();
    

    private void Start()
    {
        _controller = accessor.GetComponent<KeybindManager>().AccessController();
    }

    private InputAction InitInputAction(InputActionReference inputref)
    {
        if(inputref)
        {
            return _controller.asset.FindAction(inputref.action.name); 
        }
        return null;
    }

    private int GetIndex()
    {
        return 0;
    }
    
    public void SaveBinding()
    {
        int bindingIndex = 0;
        
        String path =  $"{Application.dataPath}/{"keybind"}.txt";

        Dictionary<string, string> bindings = new Dictionary<string, string>();

        foreach (InputAction action in _controller)
        {
            if(action.bindings[GetIndex()].overridePath != null)
                bindings.Add(action.actionMap+action.name,action.bindings[GetIndex()].overridePath);
            else
                bindings.Add(action.actionMap+action.name,action.bindings[GetIndex()].path);
            
            Debug.Log(action.bindings[GetIndex()].overridePath);
        }
        
        // writing 

        using (StreamWriter sw = new StreamWriter(path))
        {
            sw.BaseStream.Seek(0, SeekOrigin.Begin);
            string json = JsonConvert.SerializeObject(bindings);
            sw.Write(json);
        }
        
    }
    
    public void ResetOriginalBinding()
    {

        InputAction action;
        int bindingIndex = 0;
            
        foreach (InputActionReference i in keybindings)
        {
            action = InitInputAction(i);
            action.Disable();
            
            if (action.bindings[GetIndex()].isComposite)
            {
                for (int n = bindingIndex; n < action.bindings.Count && action.bindings[n].isPartOfComposite; n++)
                {
                    action.RemoveBindingOverride(n);
                }
            }
            else
            {
                action.RemoveBindingOverride(bindingIndex);
            }
            
            action.Enable();
        }
    }
}