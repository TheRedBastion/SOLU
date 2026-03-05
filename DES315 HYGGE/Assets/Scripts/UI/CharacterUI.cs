using UnityEngine;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public GameObject pressEUI;
    public GameObject dialogueUI;
    public TMP_Text dialogueText;

    private void Awake()
    {
        pressEUI.SetActive(false);
        dialogueUI.SetActive(false);
    }
}