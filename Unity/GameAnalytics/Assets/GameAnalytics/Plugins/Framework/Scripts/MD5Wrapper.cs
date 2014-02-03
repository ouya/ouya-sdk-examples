using UnityEngine;
using System.Collections;

[NotConverted]

[NotRenamed]

public static class MD5Wrapper
{
	#if !UNITY_WP8 && !UNITY_METRO
	[NotRenamed]

	public static string Md5Sum (string strToEncrypt)
	{

		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();

		byte[] bytes = ue.GetBytes (strToEncrypt);

     

		// encrypt bytes

		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();

		byte[] hashBytes = md5.ComputeHash (bytes);

     

		// Convert the encrypted bytes back to a string (base 16)

		string hashString = "";

     

		for (int i = 0; i < hashBytes.Length; i++) {

			hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');

		}

		return hashString.PadLeft (32, '0');
	}
	#endif
}