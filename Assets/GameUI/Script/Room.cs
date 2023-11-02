using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using Photon.Pun;

public class Room : MonoBehaviour
{
    public static readonly SHA256 sha = SHA256.Create();
    
    public byte[] hashedPw;
    public string RoomName;

    public static byte[] Encrypt(string password)
    {
        return sha.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
    public void setPassword(string password)
    {
        hashedPw = Encrypt(password);
    }
    public bool Check(string check)
    {
        return IsEqual(Encrypt(check), hashedPw);
    }
    private bool IsEqual(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        for (int i = 0; i < a.Length; i++)
        {
            if(a[i] != b[i]) return false;
        }
        return true;
    }
    public void Join(string check=null)
    {
        bool allowed = true;
        if (hashedPw != null)
        {
            if (check == null)
            {
                allowed = false;
            }
            else
            {
                allowed = Check(check);
            }
        }
        if (allowed)
        {
            PhotonNetwork.JoinRoom(RoomName);
        }
    }
}
