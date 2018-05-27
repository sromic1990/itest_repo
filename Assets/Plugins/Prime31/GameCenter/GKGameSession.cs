using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


#if UNITY_IOS || UNITY_TVOS

namespace Prime31
{
	public enum GKConnectionState
	{
    	NotConnected,
    	Connected
	}


	public enum GKTransportType
	{
		Unreliable,
    	Reliable
	}


	public class GKGameSession
	{
		public string identifier { get; private set; }
		public string title { get; private set; }
		public GKCloudPlayer owner { get; private set; }
		public List<GKCloudPlayer> players { get; private set; }
		public DateTime lastModifiedDate { get; private set; }
		public GKCloudPlayer lastModifiedPlayer { get; private set; }
		public int maxNumberOfConnectedPlayers { get; private set; }
		public List<GKCloudPlayer> badgedPlayers { get; private set; }


		public GKGameSession()
		{}



		#region Static bindings to native code

		[DllImport("__Internal")]
	    private static extern void _gameCenterCreateSession( string container, string title, int maxConnectedPlayers );

		// Creates a new session with the given title and maximum number of connected players (0 signfies the system limit of 16 players).
		// Results in the createSessionSucceeded/FailedEvent firing.
		public static void createSession( string container = null, string title = "", int maxConnectedPlayers = 0 )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterCreateSession( container, title, maxConnectedPlayers );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterLoadSessions( string container );

		// 
		// Results in the loadSessionSucceeded/FailedEvent firing.
		public static void loadSessions( string container = null )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadSessions( container );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterLoadSessionWithIdentifier( string identifier );

		// 
		// Results in the loadSessionWithIdentifierSucceeded/FailedEvent firing.
		public static void loadSessionWithIdentifier( string identifier )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadSessionWithIdentifier( identifier );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterRemoveSessionWithIdentifier( string identifier );

		// Removes the session with the given identifier
		// Results in the removeSessionWithIdentifierSucceeded/FailedEvent firing.
		public static void removeSessionWithIdentifier( string identifier )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterRemoveSessionWithIdentifier( identifier );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterLoadShareUrl( string identifier );

		// Results in the loadShareUrlSucceeded/FailedEvent firing.
		public static void loadShareUrl( string identifier )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadShareUrl( identifier );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterLoadData( string identifier );

		//
		// Results in the loadDataSucceeded/FailedEvent firing.
		public static void loadData( string identifier )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadData( identifier );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterSaveData( string identifer, byte[] data, int dataLength );

		// Saves the current game session data
		// Results in the loadDataSucceeded/FailedEvent firing.
		public static void saveData( string identifier, byte[] data )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterSaveData( identifier, data, data.Length );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterSetConnectionState( string identifer, int state );

		//
		// Results in the setConnectionStateSucceeded/FailedEvent firing.
		public static void setConnectionState( string identifier, GKConnectionState state )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterSetConnectionState( identifier, (int)state );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterLoadPlayersWithConnectionState( string identifer, int state );

		//
		// Results in the loadPlayersWithConnectionStateSucceeded/FailedEvent firing.
		public static void loadPlayersWithConnectionState( string identifier, GKConnectionState state )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterLoadPlayersWithConnectionState( identifier, (int)state );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterSendData( string identifer, byte[] data, int dataLength, int transportType );

		// Send data to all connected players
		// Results in the sendDataSucceeded/FailedEvent firing.
		public static void sendData( string identifier, byte[] data, GKTransportType transportType = GKTransportType.Unreliable )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
				_gameCenterSendData( identifier, data, data.Length, (int)transportType );
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterClearBadgeForPlayers( string identifer, string playerIds );

		// Clear application badge state for players for this session
		// Results in the clearBadgeForPlayersSucceeded/FailedEvent firing.
		public static void clearBadgeForPlayers( string identifier, GKCloudPlayer[] players )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
			{
				var playerIds = new string[players.Length];
				for( var i = 0; i < players.Length; i++ )
					playerIds[i] = players[i].playerId;

				_gameCenterClearBadgeForPlayers( identifier, string.Join( ",", playerIds ) );
			}
	    }


		[DllImport("__Internal")]
	    private static extern void _gameCenterSendMessage( string identifer, string playerIds );

		// Send a message to any players in the session. This uses an unreliable push mechanism. Message/data delivery
		// is not guaranteed and may take some time to arrive. Receiving players may optionally have their application badged for this session.
		// Results in the sendMessageSucceeded/FailedEvent firing.
		public static void sendMessage( string identifier, GKCloudPlayer[] players )
	    {
			if( Application.platform == RuntimePlatform.IPhonePlayer || (int)Application.platform == 31 )
			{
				var playerIds = new string[players.Length];
				for( var i = 0; i < players.Length; i++ )
					playerIds[i] = players[i].playerId;

				_gameCenterClearBadgeForPlayers( identifier, string.Join( ",", playerIds ) );
			}
	    }

		#endregion

	}
}

#endif