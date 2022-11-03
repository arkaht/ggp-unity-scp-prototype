using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }
	
	public Vector3 ViewPos => Camera.main.transform.position;
	public Vector3 ViewDir => Camera.main.transform.forward;

	public UseableEntity UseEntity { get; set; }

	public readonly Dictionary<int, Item> Inventory = new();
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

	public void AddItemToInventory( Item item )
	{
		if ( Inventory.Count >= MaxInventorySlots ) return;

		//  get available position
		int id;
		for ( id = 0; id < MaxInventorySlots; id++ )
		{
			if ( !Inventory.ContainsKey( id ) )
			{
				break;
			}
		}

		//  add to inventory
		item.Owner = this;
		item.InventoryID = id;
		Inventory.Add( id, item );

		//  setup transform
		item.transform.SetParent( item.transform );
		item.gameObject.SetActive( false );

		//  update inventory ui
		if ( InventoryUI.Instance.IsVisible )
			InventoryUI.Instance.Show();
	}

	public void RemoveItemFromInventory( Item item )
	{
		if ( item.InventoryID == -1 ) return;

		//  remove from inventory
		Inventory.Remove( item.InventoryID );
		item.Owner = null;
		item.InventoryID = -1;

		//  reset transform
		transform.SetParent( null );
		item.gameObject.SetActive( true );
	}

	public void SwapItems( int first_id, int second_id )
	{
		//  wtf is this, seems like a cool way to swap
		( Inventory[second_id], Inventory[first_id] ) = ( Inventory[first_id], Inventory[second_id] );

		//  update IDs
		Inventory[first_id].InventoryID = first_id;
		Inventory[second_id].InventoryID = second_id;
	}

	public void MoveItemTo( int source_id, int target_id )
	{
		//  move item
		Inventory[target_id] = Inventory[source_id];
		Inventory.Remove( source_id );

		//  update ID
		Inventory[target_id].InventoryID = target_id;
	}

	public void OnUse( InputValue input )
	{
		if ( !input.isPressed ) return;
		if ( InventoryUI.Instance.IsVisible ) return;
		if ( UseEntity == null ) return;

		UseEntity.Use( this );
	}

	public void OnInventory( InputValue input )
	{
		if ( !input.isPressed ) return;

		//  reset look
		inputs.look = Vector2.zero;

		InventoryUI.Instance.Toggle();
	}

	void OnApplicationFocus( bool hasFocus )
	{
		SetCursorLocked( hasFocus );
	}

	public static void SetCursorLocked( bool is_locked )
	{
		Cursor.lockState = is_locked ? CursorLockMode.Locked : CursorLockMode.None;
	}
}
