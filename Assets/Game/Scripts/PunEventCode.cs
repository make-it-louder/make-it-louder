public static class PunEventCode
{
    public static byte ModifySoundEvent = 1;
    public class ModifySoundEventDTO : IPunSerializable
    {
        public bool add;
        public int viewID;
        public ModifySoundEventDTO(bool add, int viewID)
        {
            this.add = add;
            this.viewID = viewID;
        }
        public ModifySoundEventDTO(object[] data)
        {
            PunDeserialize(data);
        }

        public object[] PunSerialize()
        {
            return new object[] { add, viewID };
        }

        public void PunDeserialize(object[] serialized)
        {
            add = (serialized[0] as bool?).Value;
            viewID = (serialized[1] as int?).Value;
        }
    }
}
