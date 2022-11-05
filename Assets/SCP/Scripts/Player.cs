using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

using Cinemachine;

public class Player : MonoBehaviour
{
	public static Player Instance { get; private set; }

	public bool InInterface => InventoryUI.Instance.IsVisible;

	public bool IsGasMaskEquiped => GetEquipedItem( ItemSlotType.Helmet ) is GasMaskItem;
	public bool InGas { get; set; }

	public bool IsAlive => Health > 0;
	public int Health = 100;
	public int MaxHealth { get; private set; }
	
	public Vector3 ViewPos => Camera.main.transform.position;
	public Vector3 ViewDir => Camera.main.transform.forward;

	public UseableEntity UseEntity { get; private set; }

	readonly List<UseableEntity> UseableEntitiesPool = new();

	public Dictionary<ItemSlotType, Item> EquipedItems = new();
	public readonly Dictionary<int, Item> Inventory = new();
	public int MaxInventorySlots = 8;
	public bool IsInventoryFull => Inventory.Count > MaxInventorySlots;


	[Header( "Camera Noise" )]
	public CinemachineVirtualCamera CinemachineVC;
	public float DefaultCameraNoiseFrequency = .3f;
	public float WalkCameraNoiseFrequency = 1.0f;
	public float RunCameraNoiseFrequency = 3.0f;
	public float SmoothNoiseFrequencySpeed = 3.0f;

	float noiseFrequency = 0.0f;

	[Header( "Sounds" )]
	public FootstepPlayer WalkFoostepPlayer;
	public FootstepPlayer RunFoostepPlayer;
	public FootstepPlayer CoughPlayer;

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

	void Start()
	{
		MaxHealth = Health;
	}

	void OnWalkUpdate()
	{
		//  footsteps players
		WalkFoostepPlayer.enabled = true;
		RunFoostepPlayer.enabled = false;

		//  get appropriate frequency
		noiseFrequency = WalkCameraNoiseFrequency;
	}

	void OnRunUpdate()
	{
		//  footsteps players
		WalkFoostepPlayer.enabled = false;
		RunFoostepPlayer.enabled = true;

		//  get appropriate frequency
		noiseFrequency = RunCameraNoiseFrequency;
	}

	void OnIdleUpdate()
	{
		//  footsteps players
		WalkFoostepPlayer.enabled = false;
		RunFoostepPlayer.enabled = false;

		//  get appropriate frequency
		noiseFrequency = DefaultCameraNoiseFrequency;
	}

	void Update()
	{
		//  movement updates
		if ( inputs.move != Vector2.zero )
		{
			if ( inputs.sprint )
			{
				OnRunUpdate();
			}
			else
			{
				OnWalkUpdate();
			}
		}
		else
		{
			OnIdleUpdate();
		}

		//  in gas
		CoughPlayer.enabled = InGas;
		
		//  smoothing noise frequency
		cinemachineNoise.m_FrequencyGain = Mathf.Lerp( cinemachineNoise.m_FrequencyGain, noiseFrequency, Time.deltaTime * SmoothNoiseFrequencySpeed );
	} 

	public void TakeDamage( int damage )
	{
		if ( !IsAlive ) return;

		Health -= damage;

		if ( !IsAlive )
		{
			print( "dead" );
		}
	}

	public void AddUseable( UseableEntity ent )
	{
		if ( UseableEntitiesPool.Contains( ent ) ) return;

		if ( UseEntity == null )
		{
			UseEntity = ent;
		}
		else
		{
			UseableEntitiesPool.Add( ent );
		}
	}

	public void RemoveUseable( UseableEntity ent )
	{
		if ( UseEntity == ent )
		{
			UseEntity = null;
			
			if ( UseableEntitiesPool.Count > 0 )
			{
				UseEntity = UseableEntitiesPool[0];
				UseableEntitiesPool.RemoveAt( 0 );
			}
		}
		else 
		{
			int id = UseableEntitiesPool.IndexOf( ent );

			if ( id > -1 )
			{
				UseableEntitiesPool.Remove( ent );
			}
		}
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

		//  remove from equiped
		ItemSlotType type = item.Type;
		if ( EquipedItems.TryGetValue( type, out Item equiped ) )
		{
			if ( item == equiped )
			{
				EquipedItems[type] = null;
			}
		}

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

	public void EquipItem( Item item )
	{
		if ( item.Owner != this ) return;

		ItemSlotType type = item.Type;

		//  un-equip previous 
		UnEquipItemType( type );

		//  equip
		EquipedItems[type] = item;
		if ( item != null )
		{
			item.OnEquiped();
		}
	}

	public void UnEquipItemType( ItemSlotType type )
	{
		//  callback previous equiped item
		Item equiped = GetEquipedItem( type );
		if ( equiped != null )
		{
			equiped.OnUnEquiped();
		}

		//  remove
		EquipedItems.Remove( type );
	}

	public Item GetEquipedItem( ItemSlotType type )
	{
		if ( EquipedItems.TryGetValue( type, out Item equiped ) )
		{
			return equiped;
		}

		return null;
	}

	public void OnUse( InputValue input )
	{
		if ( !input.isPressed ) return;
		if ( InInterface ) return;
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
