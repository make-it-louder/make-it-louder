using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public static class PunEventCode
{
    public enum Code : byte
    {
        ModifySoundEvent = 1,
        GetSoundEventResponse = 2,
    }
    public class ModifySoundEventDTO : IPunSerializable
    {
        public enum Code
        {
            add = 0,
            remove = 1,
            addAndGet = 2,
        }
        private int _codeValue;

        public Code code
        {
            get { return (Code)_codeValue; }
            set { _codeValue = (int)value; }
        }
        public int viewID;
        public ModifySoundEventDTO(Code code, int viewID)
        {
            this.code = code;
            this.viewID = viewID;
        }
        public ModifySoundEventDTO(object[] data)
        {
            PunDeserialize(data);
        }

        public object[] PunSerialize()
        {
            Debug.Log($"Original Value: {_codeValue}, {viewID}");
            object[] result = new object[2];
            result[0] = _codeValue;
            result[1] = viewID;
            Debug.Log($"Serialized: {result}");
            Debug.Log($"{result[0]}, {result[1]}");
            return result;
        }

        public void PunDeserialize(object[] serialized)
        {
            _codeValue = (int)serialized[0];
            viewID = (int)serialized[1];
        }
    }
    public class GetSoundEventResponseDTO : IPunSerializable
    {
        public List<int> list;
        public GetSoundEventResponseDTO(List<int> list)
        {
            this.list = list;
        }
        public GetSoundEventResponseDTO(List<INormalizedSoundInput> list)
        {
            this.list = new List<int>();
            foreach (var item in list)
            {
                this.list.Add(item.gameObject.transform.parent.GetComponent<PhotonView>().ViewID);
            }
        }
        public GetSoundEventResponseDTO(object[] data)
        {
            PunDeserialize(data);
        }

        public void PunDeserialize(object[] data)
        {
            list = new List<int>();
            int count = (int)data[0];
            for (int i = 0; i < count; i++)
            {
                list.Add((int)data[i + 1]);
            }
        }

        public object[] PunSerialize()
        {
            object[] result = new object[list.Count + 1];
            result[0] = list.Count;
            for (int i = 0; i < list.Count; i++)
            {
                result[i + 1] = list[i];
            }
            return result;
        }
    }
}
