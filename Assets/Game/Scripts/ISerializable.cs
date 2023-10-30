public interface IPunSerializable
{
    object[] PunSerialize();
    void PunDeserialize(object[] data);
}
