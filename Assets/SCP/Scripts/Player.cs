using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
	
	public Button UseEntity { get; set; }

	public CinemachineVirtualCamera CinemachineVC;
	public float DefaultCameraNoiseFrequency = .3f;
	public float WalkCameraNoiseFrequency = 1.0f;
	public float RunCameraNoiseFrequency = 3.0f;
	public float SmoothNoiseFrequencySpeed = 3.0f;

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

		UseEntity.Use();
	}
}
