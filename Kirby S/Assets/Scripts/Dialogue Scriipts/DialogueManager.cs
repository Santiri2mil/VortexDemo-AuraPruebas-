using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject TB;
    public PlayerDetector PlayerDetector;
    private Queue<string> sentences;
    public int SenCount=0;
   
    void Start()
    {
        sentences = new Queue<string>();
        LeanTween.scale(TB, new Vector3(0, 0, 0), 0f);

    }
    void FixedUpdate()
    {

        if (PlayerDetector.pH() == true)
            LeanTween.scale(TB, new Vector3(0.8f, 0.8f, 0.8f), 0.1f);
        else
            LeanTween.scale(TB, new Vector3(0, 0, 0), 0.1f);
    }
    public void StartDialogue(Dialogues dialogues)
    {
        
        Debug.Log("Start conversation with " + dialogues.name);
        nameText.text = dialogues.name;
        sentences.Clear();

        foreach (string sentence in dialogues.sentences)
        {
            sentences.Enqueue(sentence);
        }
        SenCount = sentences.Count;
        DisplaynextSentence();
       
    }

    public void DisplaynextSentence()
    {
        if (sentences.Count==0)
        {
            EndDialgue();
            return;
        }
        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
        SenCount--;
        //Debug.Log(sentence);
    }
    void EndDialgue()
    {
        Debug.Log("End of Coversation");
        if(SenCount>sentences.Count-1)
        {
            LeanTween.scale(TB, new Vector3(0, 0, 0), 0.5f);
           
        }
            
    }
}
