using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterContainer", menuName = "ScriptableObjects/Characters", order = 1)]

public class CharacterContainer : ScriptableObject
{

    [Tooltip("The character avatar sprite that will represent this character.")]
    public Sprite m_characterPortrait;

    [Tooltip("The lines of dialogue that will be spoken to the player by this NPC.")]
    [TextArea]
    public string[] m_characterSentences;

    [Tooltip("The name of this character.")]
    public string m_characterName;

    [Tooltip("The character's response to having their soul reaped.")]
    public string m_soulReapedResponse;

    [Tooltip("The character's response to having their soul spared.")]
    public string m_soulSparedResponse;

    public bool m_wasSpared;

    public string[] m_lifeEvents;

}
