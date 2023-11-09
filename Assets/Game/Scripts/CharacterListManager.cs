using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterListManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] characterList;
    [SerializeField]
    MicInputManager[] micInputs;
    public GameObject[] CharacterList
    {
        get { return characterList; }
    }
    public MicInputManager[] MicInputs
    {
        get { return micInputs; }
    }
    // Start is called before the first frame update
    void Start()
    {
        characterList = new GameObject[0];
        micInputs = new MicInputManager[0];
    }

    private void OnTransformChildrenChanged()
    {
        characterList = new GameObject[transform.childCount];
        for (int i = 0;i < transform.childCount;i++)
        {
            characterList[i] = transform.GetChild(i).gameObject;
        }
        micInputs = transform.GetComponentsInChildren<MicInputManager>();
    }
}
