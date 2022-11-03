using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
	
	public Vector3 ViewPos => Camera.main.transform.position;
	public Vector3 ViewDir => Camera.main.transform.forward;

	public UseableEntity UseEntity { get; set; }

	public readonly List<Item> Inventory = new();
	public int MaxInventorySlots = 8;
	public bool IsInventoryFull => Inventory.Count > MaxInventorySlots;

	[Header( "Camera Noise" )]
	public CinemachineVirtualCamera CinemachineVC;
	public float DefaultCameraNoiseFrequency = .3f;
	public float WalkCameraNoiseFrequency = 1.0f;
	public float RunCameraNoiseFrequency = 3.0f;
	public float SmoothNoiseFrequencySpeed = 3.0f;

	[Header( "Misc" )]
	public float DropItemDistance = 2.0f;

	StarterAssets.StarterAssetsInputs inputs;
	CinemachineBasicMultiChannelPerlin cinemachineNoise;

	void Awake()
	{
		Instance = this;

		inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
		cinemachineNoise = CinemachineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
	}

	void Update()
	{
		#region MovementShake
		//  get appropriate frequency
		float frequency = DefaultCameraNoiseFrequency;
		if ( inputs.move != Vector2.zero )
		{
			if ( inputs.sprint )
			{
				frequency = RunCameraNoiseFrequency;
			}
			else
			{
				frequency = WalkCameraNoiseFrequency;
			}
		}

		//  smoothing frequency
		cinemachineNoise.m_FrequencyGain = Mathf.Lerp( cinemachineNoise.m_FrequencyGain, frequency, Time.deltaTime * SmoothNoiseFrequencySpeed );
		#endregion
	} 

	public void OnUse( InputValue input )
	{
		if ( UseEntity == null ) return;

		UseEntity.Use( this );
	}

	public void OnDrop( InputValue input )
	{
		if ( Inventory.Count == 0 ) return;

		Inventory[0].Drop();
	}
}
