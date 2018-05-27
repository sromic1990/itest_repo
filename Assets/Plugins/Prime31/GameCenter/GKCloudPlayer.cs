using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_IOS || UNITY_TVOS

namespace Prime31
{
	public class GKCloudPlayer
	{
		// This player's full name as displayed in the Game Center in-game UI. Use this when you need to display the
		// player's name. The display name may be very long, so be sure to use appropriate string truncation API when drawing.
		public string displayName;
		public string playerId;


		public GKCloudPlayer()
		{}
	}
}

#endif