using Photon.Voice;
using Photon.Voice.Unity;
using UnityEngine;

public class MicInput : MonoBehaviour
{
    [SerializeField]
    string selectedMicName = null;
    Recorder recorder;
    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<Recorder>();
        SetMicName(null);
    }

    public void SetMicName(string name)
    {
        selectedMicName = name ?? "DEFAULT";
        if (name == null)
        {
            recorder.MicrophoneDevice = DeviceInfo.Default;
        }
        else
        {
            recorder.MicrophoneDevice = new DeviceInfo(name);

        }
    }
}
