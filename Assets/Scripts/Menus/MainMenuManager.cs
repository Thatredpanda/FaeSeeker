using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [Header("Link Objects")]
    [SerializeField] private List<EntryUIAnim> elementsUI;
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private GameObject credits;

    [Header("Controls")]
    [SerializeField] private KeyCode exitCode = KeyCode.Escape;

    private bool isCredits;

    private OptionsMenu optionsMenu;

    private void Start()
    {
        optionsMenu = GetComponent<OptionsMenu>();
        ShowElements(true);
    }

    private void Update()
    {
        if (isCredits && Input.GetKey(exitCode))
        {
            EndCreditsComplete();
        }
    }

    public void PlayButton()
    {
        AudioManager.Instance.PlayAudio(AudioManager.SoundType.ButtonClick, 0, 0);
        ShowElements(false);
        fadeIn.SetActive(true);
    }

    public void OptionsButton()
    {
        AudioManager.Instance.PlayAudio(AudioManager.SoundType.ButtonClick, 0, 0);
        ShowElements(false);
        optionsMenu.OpenOptions();
    }

    public void CreditsButton()
    {
        AudioManager.Instance.PlayAudio(AudioManager.SoundType.ButtonClick, 0, 0);
        isCredits = true;
        ShowElements(false);
        credits.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ShowElements(bool boolState)
    {
        foreach (EntryUIAnim element in elementsUI)
        {
            element.OnScreen(boolState);
        }
    }

    public void EndCreditsComplete()
    {
        isCredits = false;
        ShowElements(true);
        credits.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}