using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager current; 

    public CharacterContainer[] m_gameCharacters;

    public int m_currentCharacterIndex = 0;

    public Image m_characterPortrait;

    public TextMeshProUGUI m_characterName;

    public DialogueController m_dialogueController;

    public GameObject m_mainMenu;

    public GameObject m_introduction;

    public GameObject m_interactionButtons;

    public GameObject m_nextSoulButton;

    public GameObject m_nextEpilogueButton;

    public GameObject[] m_deathPointIcons;

    public int m_deathPoints;


    private void Awake()
    {
        current = this;

        //Subscribes methods to the events they should be triggered by
        current.onDialogueEnded += DialogueOver;

        current.onSoulSpared += SpareSoul;

        current.onNextSoulRequested += NextSoul;

        current.onSoulReaped += ReapSoul;
    }

    void Start()
    {
        //Sets this object to persist between scenes because for whatever reason this game has multiple scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnLevelWasLoaded(int level)
    {
        switch (level)
        {
            case 0:
                {

                }
                break;

            case 1:
                {
                    //Finds and assigns the dialogue objects within the scene e.g dialogue box, character avatar etc.
                    FindDialogueObjects( );

                    //Sets up the dialogue controller with the first character's attributes
                    SetupNewCharacter( );

                    //Starts the dialogue
                    m_dialogueController.StartDialogue(m_gameCharacters[m_currentCharacterIndex]);
                }
                break;

            default:
                {
                    Debug.LogError("Scene loaded not accounted for in Game Manager.");
                }
                break;
        }
    }

    #region EventActions
    public event Action onDialogueEnded;

    public event Action onSoulSpared;

    public event Action onSoulReaped;

    public event Action onNextSoulRequested;

    #endregion


    #region Event Stuff
    public void DialogueEnded()
    {
        if( onDialogueEnded != null)
        {
            onDialogueEnded();
        } 
    }

    public void SoulSpared()
    {
        if( onSoulSpared != null)
        {
            onSoulSpared();
        }
    }

    public void NextSoulRequested()
    {
        if( onNextSoulRequested != null)
        {
            onNextSoulRequested();
        }
    }

    public void SoulReaped()
    {
        if(onSoulReaped != null)
        {
            onSoulReaped();
        }
    }

    #endregion 

    public void QuitGame()
    {
        //Closes the game
        Application.Quit();
    }

    public void TriggerExposition()
    {

        //Disables the main menu interface so it cannot be seen or interacted with
        m_mainMenu.SetActive(false);

        //Activates the introduction UI to display the expositional dialogue
        m_introduction.SetActive(true);

        //Starts the expositional dialogue
        m_dialogueController.StartDialogue(m_gameCharacters[0]);

    }

    public void DialogueOver()
    {
        switch (m_currentCharacterIndex)
        {
            case 0:
                {
                    //Once the exposition is done, the game scene is loaded
                    LoadGameScene();
                }
                break;
        }
    }

    public void LoadGameScene()
    {
        //Increments the character index by one before loading the game scene in order to load up the first character
        m_currentCharacterIndex++;

        SceneManager.LoadScene(1);
    }

    public void SpareSoul()
    {
        //Disables the interaction buttons as they cannot be used at this point
        m_interactionButtons.SetActive(false);

        //Displays the current character's reaction to being spared
        m_dialogueController.DisplayDecisionReaction(m_gameCharacters[m_currentCharacterIndex].m_soulSparedResponse);

        //Sets the current character as having been spared
        m_gameCharacters[m_currentCharacterIndex].m_wasSpared = true;

        //Increments the character index so that the next character can be loaded
        m_currentCharacterIndex++;

        //Disables the next death point icon in the array to display health having been lost
        m_deathPointIcons[m_deathPoints - 1].SetActive(false);

        //Reduces the value of death points to track the player's health in code
        m_deathPoints--;

        //Activates the "Next" button so the player can progress
        m_nextSoulButton.SetActive(true);

    }

    public void ReapSoul()
    {
        //Disables the interaction buttons as they cannot be used at this point
        m_interactionButtons.SetActive(false);

        //Displays the current character's reaction to being reaped
        m_dialogueController.DisplayDecisionReaction(m_gameCharacters[m_currentCharacterIndex].m_soulReapedResponse);

        //Increments the character index so that the next character can be loaded
        m_currentCharacterIndex++;

        //Activates the "Next" button so the player can progress
        m_nextSoulButton.SetActive(true);
    }

    public void NextSoul()
    {
        //If the player has dealth with all souls or run out of health then the epilogue is loaded
        if (m_currentCharacterIndex >= m_gameCharacters.Length || m_deathPoints <= 0)
        {
            LoadEpilogue();
        }
        //Otherwise the next character is loaded
        else
        {
            //Sets up the next character in the array
            SetupNewCharacter();

            //Starts the next character's dialogue
            m_dialogueController.StartDialogue(m_gameCharacters[m_currentCharacterIndex]);

            //Disables this button so the player cannot skip through the souls
            m_nextSoulButton.SetActive(false);

            //Enables these so the player can interact with the soul
            m_interactionButtons.SetActive(true);

        }

    }

    public void LoadEpilogue()
    {
        //Sets the name box as blank instead of disabling it because why not
        m_characterName.text = "";

        //Sets the avatar as blank instead of disabling it because why not
        m_characterPortrait.sprite = null;

        //Disables the "next soul" button as it no longer functions
        m_nextSoulButton.SetActive(false);

        //Enables the "next epilogue" button so that the player can progress through the epilogue
        m_nextEpilogueButton.SetActive(true);

        //Starts the epilogue and passes in the array of characters to determine which were spared and which dialogue to enqueue
        m_dialogueController.StartEpilogue(m_gameCharacters);
    }

    public void FindDialogueObjects( )
    {
        //I'm not commenting this it's self explanatory and I'm lazy

        m_dialogueController = GameObject.Find("DialogueBox").GetComponent<DialogueController>( );

        m_characterPortrait = GameObject.Find("CharacterPortrait").GetComponent<Image>();

        m_characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();

        m_interactionButtons = GameObject.Find("InteractionButtons");

        m_nextSoulButton = GameObject.Find("NextSoul");

        m_nextSoulButton.SetActive(false);

        m_nextEpilogueButton = GameObject.Find("NextButton");

        m_nextEpilogueButton.SetActive(false);

        for(int i = 0; i < 3; i++)
        {
            m_deathPointIcons[i] = GameObject.Find("HealthIcon" + i);
        }

    }

    public void SetupNewCharacter( )
    {
        //I'm not commenting this it's self explanatory and I'm lazy

        m_characterPortrait.sprite = m_gameCharacters[m_currentCharacterIndex].m_characterPortrait;

        m_characterName.text = m_gameCharacters[m_currentCharacterIndex].m_characterName;
    }

}
