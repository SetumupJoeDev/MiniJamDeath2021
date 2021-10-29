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

    // Start is called before the first frame update
    void Start()
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
    }

    public void StartDialogue( CharacterContainer character)
    {
        m_dialogueQueue.Clear();

        foreach( string sentence in character.m_characterSentences)
        {
            m_dialogueQueue.Enqueue(sentence);

        }

        DisplayNextSentence();

    }

    public void DisplayNextSentence()
    {
        if(m_dialogueQueue.Count == 0)
        {
            //End dialogue here
            Debug.Log("Dialogue over!");
            return;
        }
        else
        {
            string sentence = m_dialogueQueue.Dequeue();

            StartCoroutine(TypeText(sentence));
        }
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
