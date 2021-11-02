using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{

    private Queue<string> m_dialogueQueue;

    public TextMeshProUGUI m_dialogueText;

    public AudioSource m_textTypeSound;

    public float m_baseTypeSpeed = 0.10f;

    public float m_quickTypeSpeed = 0.05f;

    private float m_typeSpeed;

    private string m_currentSentence;

    public IEnumerator typeText;

    // Start is called before the first frame update
    void Awake()
    {
        m_dialogueQueue = new Queue<string>();

        //Sets the typing speed as the base typing speed
        m_typeSpeed = m_baseTypeSpeed;

    }

    // Update is called once per frame
    void Update()
    {

        //If the user is holding down the Z key and the type speed is the base type speed, then it is increased to the quick typing speed
        if (Input.GetKey(KeyCode.Z) && m_typeSpeed == m_baseTypeSpeed )
        {
            m_typeSpeed = m_quickTypeSpeed;
        }

        //Reverts the typing speed to the base speed when Z is released
        if (Input.GetKeyUp(KeyCode.Z))
        {
            m_typeSpeed = m_baseTypeSpeed;
        }

        //Stops the typing coroutine and simply displays the current sentence on-screen
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(typeText != null)
            {
                StopCoroutine(typeText);

                m_dialogueText.text = m_currentSentence;
            }
        }
    }

    public void StartDialogue( CharacterContainer character)
    {
        //Clears the dialogue queue of all old strings
        m_dialogueQueue.Clear();

        //Loops through the array of sentences in the character passed in and adds them to the queue
        foreach( string sentence in character.m_characterSentences)
        {
            m_dialogueQueue.Enqueue(sentence);

        }

        //Displays the first sentence in the queue
        DisplayNextSentence();

    }

    public void StartEpilogue( CharacterContainer[] characters)
    {
        //Clears all old dialogue from the queue
        m_dialogueQueue.Clear();

        //Loops through each character to check if they were spared
        foreach(CharacterContainer character in characters)
        {
            //If they were spared, then their epilogue/life events are added to the dialogue queue
            if (character.m_wasSpared)
            {
                foreach (string sentence in character.m_lifeEvents)
                {
                    m_dialogueQueue.Enqueue(sentence);
                }
            }
        }

        //Displays the first in the list of epilogues
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //If there are no more strings in the queue then dialogue has ended
        if(m_dialogueQueue.Count == 0)
        {
            DialogueEnded();
            return;
        }
        else
        {
            //If the coroutine has been assigned to, then it is stopped to avoid mangling the strings together
            if (typeText != null)
            {
                StopCoroutine(typeText);
            }

            //Dequeues the next string in the dialogue queue and sets it as the currentSentence to display
            m_currentSentence = m_dialogueQueue.Dequeue();

            //Assigns the coroutine with the new sentence to display
            typeText = TypeText( m_currentSentence );

            //Starts the coroutine
            StartCoroutine(typeText);
        }
    }

    //I made the game really weirdly so I needed to add an events system because I am dumb, might fix it later
    #region EventTriggers
    public void SpareSoul()
    {
        GameManager.current.SoulSpared();
    }

    public void ReapSoul()
    {
        GameManager.current.SoulReaped();
    }

    public void NextSoul()
    {
        GameManager.current.NextSoulRequested();
    }

    public void DialogueEnded()
    {
        GameManager.current.DialogueEnded();
    }

    #endregion

    public void DisplayDecisionReaction( string reactionText )
    {
        //If the coroutine is assigned, it is stopped
        if(typeText != null)
        {
            StopCoroutine(typeText);
        }

        //Sets the current sentence to be the string passed in via args
        m_currentSentence = reactionText;

        //Assigns the coroutine using the new current sentence
        typeText = TypeText(m_currentSentence);

        //Starts the coroutine
        StartCoroutine(typeText);
    }

    public IEnumerator TypeText( string textToType )
    {

        //Sets the text content of the dialogue box to be empty, effectively clearing it
        m_dialogueText.text = "";

        //Loops through each character in the string passed in to the coroutine and adds them on to the text
        foreach (char character in textToType.ToCharArray())
        {

            //Adds the current character in the loop to the UI text
            m_dialogueText.text += character;

            //If the character isn't a space, then the "typing" sound is played. This is to make the sound sync with the visuals better
            if (character != ' ')
            {
                m_textTypeSound.Play();
            }

            //Waits for the duration of typeSpeed before adding another letter to the text
            yield return new WaitForSeconds(m_typeSpeed);
        }

    }

}
