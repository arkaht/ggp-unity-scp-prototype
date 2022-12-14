using UnityEngine;
using System.Collections.Generic;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>
public class LightFlickerEffect : MonoBehaviour
{
	[Tooltip( "External light to flicker; you can leave this null if you attach script to a light" )]
	public Light Light;
	[Tooltip( "Random light intensity range" )]
	public Vector2 IntensityRange = new( 0.0f, 1.0f );
	[Tooltip( "How much to smooth out the randomness; lower values = sparks, higher = lantern" ), Range( 1, 50 )]
	public int Smoothing = 5;

	// Continuous average calculation via FIFO queue
	// Saves us iterating every time we update, we just change by the delta
	Queue<float> smoothQueue = new();
	float lastSum = 0;


	/// <summary>
	/// Reset the randomness and start again. You usually don't need to call
	/// this, deactivating/reactivating is usually fine but if you want a strict
	/// restart you can do.
	/// </summary>
	public void Reset()
	{
		smoothQueue.Clear();
		lastSum = 0;
	}

	void Start()
	{
		smoothQueue = new Queue<float>( Smoothing );

		//  external or internal light?
		if ( Light == null )
		{
			Light = GetComponent<Light>();
		}
	}

	void Update()
	{
		if ( Light == null ) return;

		//  pop off an item if too big
		while ( smoothQueue.Count >= Smoothing )
		{
			lastSum -= smoothQueue.Dequeue();
		}

		//  generate random new item, calculate new average
		float newVal = Random.Range( IntensityRange.x, IntensityRange.y );
		smoothQueue.Enqueue( newVal );
		lastSum += newVal;

		//  calculate new smoothed average
		Light.intensity = lastSum / smoothQueue.Count;
	}

}