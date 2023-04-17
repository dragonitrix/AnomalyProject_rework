using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public static class SecureHelper
{
    public static string Hash(string data)
    {
        byte[] textToBytes = Encoding.UTF8.GetBytes(data);
        SHA256Managed sha512 = new SHA256Managed();

        byte[] hash = sha512.ComputeHash(textToBytes);
        return GetHexFromHash(hash);
    }

    public static string HashSalt(string dataString, string saltString)
    {
        var dataBytes = Encoding.UTF8.GetBytes(dataString);
        var saltBytes = Encoding.UTF8.GetBytes(saltString);

        HashAlgorithm algorithm = new SHA256Managed();

        byte[] plainTextWithSaltBytes =
          new byte[dataBytes.Length + saltBytes.Length];

        for (int i = 0; i < dataBytes.Length; i++)
        {
            plainTextWithSaltBytes[i] = dataBytes[i];
        }
        for (int i = 0; i < saltBytes.Length; i++)
        {
            plainTextWithSaltBytes[dataBytes.Length + i] = saltBytes[i];
        }

        return GetHexFromHash(algorithm.ComputeHash(plainTextWithSaltBytes));
    }

    private static string GetHexFromHash(byte[] hash)
    {
        string hex = string.Empty;
        foreach (byte b in hash)
        {
            hex += b.ToString("x2");
        }
        return hex;
    }

    private static string GetBase64FromHash(byte[] hash)
    {
        return Convert.ToBase64String(hash);
    }

}
