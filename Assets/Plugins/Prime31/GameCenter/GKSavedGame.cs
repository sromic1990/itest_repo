using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


#if UNITY_IOS || UNITY_TVOS

namespace Prime31
{
	public class GKSavedGame
	{
		public string name { get; private set; }
		public string deviceName { get; private set; }
		public double rawModificationDate { get; private set; }
		public DateTime modificationDate
		{
			get
			{
				var intermediate = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Utc );
				return intermediate.AddSeconds( rawModificationDate );
			}
		}


		[DllImport("__Internal")]
		private static extern void _gameCenterFetchSavedGames();

		// Fetches any available saved games. Results in the fetchSavedGamesSucceeded/FailedEvent firing.
		public static void fetchSavedGames()
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterFetchSavedGames();
		}


		[DllImport("__Internal")]
		private static extern void _gameCenterSaveGame( string name, byte[] data, int dataLength );

		// Saves a game in a slot with the given name. Results in the saveGameSucceeded/FailedEvent firing.
		public static void saveGame( string name, byte[] data )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterSaveGame( name, data, data.Length );
		}


		[DllImport("__Internal")]
		private static extern void _gameCenterDeleteSavedGame( string name );

		// Deletes the saved game. Results in the deleteSavedGameSucceeded/FailedEvent firing.
		public static void deleteSavedGame( string name )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterDeleteSavedGame( name );
		}


		[DllImport("__Internal")]
		private static extern void _gameCenterLoadSavedGameData( string name );

		// Loads the raw data for the saved game. Results in the loadSavedGameDataSucceeded/FailedEvent firing.
		public static void loadSavedGameData( string name )
		{
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadSavedGameData( name );
		}

	}
}

#endif