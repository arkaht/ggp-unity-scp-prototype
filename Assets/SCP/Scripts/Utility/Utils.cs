using System.Collections;
using UnityEngine;

public static class Utils
{
	public static T GetRandomElement<T>( T[] array )
	{
		return array[Random.Range( 0, array.Length )];
	}
}