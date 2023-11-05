using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettings : MonoBehaviour
{
    public GameObject characterChangeForm;
    public GameObject otherChangeForm;

    FirebaseManager.Profile profile;

    public Button[] buttons;
    private List<bool> avatars;
    // Start is called before the first frame update
    void Start()
    {
        characterChangeForm.SetActive(true);
        otherChangeForm.SetActive(false);
        profile = RecordManager.Instance.UserProfile;
        avatars = profile.avatars;
        SearchingMyCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleValueChange(bool value)
    {
        if (!value)
        {
            characterChangeForm.SetActive(false);
            otherChangeForm.SetActive(true);
        } else
        {
            characterChangeForm.SetActive(true);
            otherChangeForm.SetActive(false);
        }
    }

    public void OnClickChangeCharacter ()
    {
    }
    public void SearchingMyCharacter()
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            buttons[i].interactable = avatars[i];
            Transform lockImageTransform = buttons[i].transform.Find("Lock");
            GameObject lockImage = lockImageTransform.gameObject;
            lockImage.SetActive(!avatars[i]);
        }
    }
}
