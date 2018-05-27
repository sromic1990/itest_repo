using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


#if UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX

namespace Prime31
{
	public class GKInvite
	{
		public GKPlayer sender;
		public bool isHosted;
		public int playerGroup;
		public int playerAttributes;
	
	
		public GKInvite()
		{}


		public override string ToString()
		{
			 return string.Format( "<GKInvite> sender: {0}, isHosted: {1}, playerGroup: {2}, playerAttributes: {3}",
				sender, isHosted, playerGroup, playerAttributes );
		}
	
	}

}
#endif

