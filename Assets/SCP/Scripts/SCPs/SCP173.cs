using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SCP173 : MonoBehaviour
{
	public static SCP173 Instance { get; private set; }

	public Door ActiveDoor { get; set; }

	public Vector3[] DetectionPositions;

	public float ActiveDistance = 20.0f;
	public bool UseRaycast = true;
	public float KillDistance = 1.0f;
	public float SeeDistance = 10.0f;
	public float WaitingDoorTime = 2.0f;

	[TextArea( 3, 10 )]
	public string DeathMessage;

	[Header( "Sounds" )]
	public AudioClip OpenDoorSound;
	public AudioClip[] KillSounds;

	bool canMove = false;
	
	float waitingDoorTime = 0.0f;

	float activeDistanceSqr = 1.0f;

	new AudioSource audio;
	NavMeshAgent agent;


	void Awake()
	{
		Instance = this;

		audio = GetComponent<AudioSource>();
		agent = GetComponent<NavMeshAgent>();
	}

	void Start()
	{
		activeDistanceSqr = ActiveDistance * ActiveDistance;
	}

	void FixedUpdate()
	{
		Player player = Player.Instance;
		if ( player == null ) return;

		//  disable if dead
		if ( !player.IsAlive ) 
		{
			StopMovement();
			return;
		}

		//  active if in range
		float player_distance_sqr = ( player.transform.position - transform.position ).sqrMagnitude;
		if ( player_distance_sqr > activeDistanceSqr )
		{
			StopMovement();
			return;
		}

		//  detect visibility
		bool is_seen = false;
		foreach ( Vector3 pos in DetectionPositions )
		{
			if ( player.IsPosVisible( transform.position + pos, SeeDistance, UseRaycast ) )
			{
				is_seen = true;
				break;
			}
		}

		//  freeze
		if ( is_seen )
		{
			StopMovement();
			return;
		}
		else if ( !canMove )
		{
			canMove = true;
			return;
		}

		if ( ActiveDoor != null && !ActiveDoor.IsOpen )
		{
			if ( ( waitingDoorTime -= Time.deltaTime ) <= 0.0f )
			{
				ActiveDoor.SetToggle( true );
				ActiveDoor.Audio.PlayOneShot( OpenDoorSound );
				ActiveDoor = null;
			}

			StopMovement();
		} 
		else
		{
			waitingDoorTime = WaitingDoorTime;

			//  move towards player
			agent.SetDestination( player.transform.position );
			agent.isStopped = false;

			if ( agent.remainingDistance > agent.stoppingDistance )
			{
				//  play move sound
				if ( !audio.isPlaying )
				{
					audio.Play();
				}
			}
			else
			{
				if ( player_distance_sqr <= KillDistance * KillDistance )
				{
					//  kill player
					player.TakeDamage( player.Health, DeathMessage );

					//  play kill sound
					AudioNotification.PlayAudioAt( Vector3.zero, KillSounds, 0.75f );
				}
			}
		}
	}

	void OnDrawGizmos()
	{
		Gizmos.color = new( 1.0f, 0.0f, 0.0f, 0.5f );

		//  draw detection spots
		Vector3 size = new( 0.1f, 0.1f, 0.1f );
		foreach ( Vector3 pos in DetectionPositions )
		{
			Gizmos.DrawWireCube( transform.position + pos, size );
			Gizmos.DrawCube( transform.position + pos, size );
		}

		//  draw see range
		Gizmos.DrawWireSphere( transform.position, SeeDistance );

		//  draw active range
		Gizmos.color = new( 1.0f, 0.5f, 0.0f, 0.5f );
		Gizmos.DrawWireSphere( transform.position, ActiveDistance );
	}

	private void StopMovement()
	{
		//  stop movement
		agent.isStopped = true;
		canMove = false;

		//  stop sound
		audio.Stop();
	}
}
