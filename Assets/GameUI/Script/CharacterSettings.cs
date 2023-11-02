using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSettings : MonoBehaviour
{
    public GameObject characterChangeForm;
    public GameObject otherChangeForm;

    public Button[] buttons;
    private List<bool> myChr = new List<bool>() { true, false, false, false };
    // Start is called before the first frame update
    void Start()
    {
        characterChangeForm.SetActive(true);
        otherChangeForm.SetActive(false);
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
        for (int i = 0; i < myChr.Count; i++)
        {
            buttons[i].interactable = myChr[i];
            Transform lockImageTransform = buttons[i].transform.Find("Lock");
            GameObject lockImage = lockImageTransform.gameObject;
            lockImage.SetActive(!myChr[i]);
        }
    }
}
