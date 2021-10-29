using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public CharacterContainer[] m_gameCharacters;

    public int m_currentCharacterIndex = 0;

    public Image m_characterPortrait;

    public TextMeshProUGUI m_characterName;

    public DialogueController m_dialogueController;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        m_dialogueController.StartDialogue(m_gameCharacters[0]);

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
