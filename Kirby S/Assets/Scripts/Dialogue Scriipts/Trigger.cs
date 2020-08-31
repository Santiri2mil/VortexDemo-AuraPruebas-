using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    public Dialogues dialogues;

    public void TriggerDialogue()
    {

        FindObjectOfType<DialogueManager>().StartDialogue(dialogues);
        
    }
}
