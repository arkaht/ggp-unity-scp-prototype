using System.Collections;
using UnityEngine;

public class ActivableEntity : MonoBehaviour
{
	public virtual bool Activate( Player player, MonoBehaviour caller ) => false;
}