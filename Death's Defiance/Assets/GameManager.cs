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


    private void Awake()
    {
        current = this;

        current.onDialogueEnded += DialogueOver;
    }

    void Start()
    {
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
                    FindDialogueObjects( );

                    SetupNewCharacter( );
                }
                break;

            default:
                {
                    Debug.LogError("Scene loaded not accounted for in Game Manager.");
                }
                break;
        }
    }

    public event Action onDialogueEnded;

    public void DialogueEnded()
    {
        if( onDialogueEnded != null)
        {
            onDialogueEnded();
        } 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TriggerExposition()
    {

        m_mainMenu.SetActive(false);

        m_introduction.SetActive(true);

        m_dialogueController.StartDialogue(m_gameCharacters[0]);
    }

    public void DialogueOver()
    {
        switch (m_currentCharacterIndex)
        {
            case 0:
                {
                    SceneManager.LoadScene(1);
                }
                break;
        }
    }

    public void FindDialogueObjects( )
    {

        m_dialogueController = GameObject.Find("DialogueBox").GetComponent<DialogueController>( );

        m_characterPortrait = GameObject.Find("CharacterPortrait").GetComponent<Image>();

        m_characterName = GameObject.Find("CharacterName").GetComponent<TextMeshProUGUI>();
    }

    public void SetupNewCharacter( )
    {
        m_characterPortrait.sprite = m_gameCharacters[m_currentCharacterIndex].m_characterPortrait;

        m_characterName.text = m_gameCharacters[m_currentCharacterIndex].m_characterName;
    }

}
