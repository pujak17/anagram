 using UnityEngine;
 using UnityEngine.UI;
 using System.Collections;


public class getInputScript : MonoBehaviour
{
     InputField txt;
     public string change;
 
     // Use this for initialization
     void Start () {
         txt = gameObject.GetComponent<InputField>(); 
     }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string getText () {
        return txt.text;
    }

}

