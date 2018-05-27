using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;


#if UNITY_IOS || UNITY_TVOS

namespace Prime31
{
	public class GameSessionManager : AbstractManager
	{
		public static event Action<GKCloudPlayer> didAddPlayerEvent;

		public static event Action<GKCloudPlayer> didRemovePlayerEvent;



		// Fired when 
		public static event Action<GKGameSession> createSessionSucceededEvent;

		// Fired when 
		public static event Action<P31Error> createSessionFailedEvent;

		public static event Action<List<GKGameSession>> loadSessionsSucceededEvent;

		public static event Action<P31Error> loadSessionsFailedEvent;

		public static event Action<GKGameSession> loadSessionWidthIdentifierSucceededEvent;

		public static event Action<P31Error> loadSessionWidthIdentifierFailedEvent;

		public static event Action removeSessionWithIdentifierSucceededEvent;

		public static event Action<P31Error> removeSessionWithIdentifierFailedEvent;

		public static event Action<string> loadShareUrlSucceededEvent;

		public static event Action<P31Error> loadShareUrlFailedEvent;

		// Fired when
		public static event Action<byte[]> loadDataSucceededEvent;

		public static event Action<P31Error> loadDataFailedEvent;

		public static event Action saveDataSucceededEvent;

		public static event Action<P31Error> saveDataFailedEvent;

		public static event Action setConnectionStateSucceededEvent;

		public static event Action<P31Error> setConnectionStateFailedEvent;

		public static event Action<List<GKCloudPlayer>> loadPlayersWithConnectionStateSucceededEvent;

		public static event Action<P31Error> loadPlayersWithConnectionStateFailedEvent;

		public static event Action sendDataSucceededEvent;

		public static event Action<P31Error> sendDataFailedEvent;

		public static event Action sendMessageSucceededEvent;

		public static event Action<P31Error> sendMessageFailedEvent;

		public static event Action clearBadgeForPlayersSucceededEvent;

		public static event Action<P31Error> clearBadgeForPlayersFailedEvent;

		public static event Action<string,byte[]> playerSaveDataEvent;

		public static event Action<string,byte[]> receivedDataEvent;

		public static event Action<string,string,byte[]> receivedMessageEvent;


		enum GKGenericMessageType
		{
			DidSaveData,
			DidReceiveData,
			DidReceiveMessage
		}


		delegate void GameSessionReceiveDataCallback( IntPtr dataBuf, int length );
		delegate void GameSessionGenericDataCallback( int messageType, IntPtr dataBuf, int length, string message, string playerId );
	
		[DllImport("__Internal")]
		static extern void _gameCenterSetGameSessionReceivedDataCallback( GameSessionReceiveDataCallback callback, GameSessionGenericDataCallback callback2 );


	    static GameSessionManager()
	    {
			AbstractManager.initialize( typeof( GameSessionManager ) );

			if( Application.platform == RuntimePlatform.IPhonePlayer )
				_gameCenterSetGameSessionReceivedDataCallback( didReceiveData, didReceiveGenericData );
	    }


		[AOT.MonoPInvokeCallback( typeof( GameSessionReceiveDataCallback ) )]
		private static void didReceiveData( IntPtr dataBuf, int length )
		{
			var data = new byte[length];
			Marshal.Copy( dataBuf, data, 0, length );
	
			if( loadDataSucceededEvent != null )
				loadDataSucceededEvent( data );
		}


		[AOT.MonoPInvokeCallback( typeof( GameSessionGenericDataCallback ) )]
		private static void didReceiveGenericData( int messageType, IntPtr dataBuf, int length, string message, string playerId )
		{
			byte[] data = null;
			if( length > 0 )
			{
				data = new byte[length];
				Marshal.Copy( dataBuf, data, 0, length );
			}

			switch( (GKGenericMessageType)messageType )
			{
				case GKGenericMessageType.DidSaveData:
					playerSaveDataEvent.fire( playerId, data );
					break;
				case GKGenericMessageType.DidReceiveData:
					receivedDataEvent.fire( playerId, data );
					break;
				case GKGenericMessageType.DidReceiveMessage:
					receivedMessageEvent.fire( playerId, message, data );
					break;
			}
		}


		void didAddPlayer( string json )
		{
			if( didAddPlayerEvent != null )
				didAddPlayerEvent( Json.decode<GKCloudPlayer>( json ) );
		}


		void didRemovePlayer( string json )
		{
			if( didRemovePlayerEvent != null )
				didRemovePlayerEvent( Json.decode<GKCloudPlayer>( json ) );
		}


		void createSessionSucceeded( string json )
		{
			if( createSessionSucceededEvent != null )
				createSessionSucceededEvent( Json.decode<GKGameSession>( json ) );
		}


		void createSessionFailed( string json )
		{
			if( createSessionFailedEvent != null )
				createSessionFailedEvent( P31Error.errorFromJson( json ) );
		}


		void loadSessionsSucceeded( string json )
		{
			if( loadSessionsSucceededEvent != null )
				loadSessionsSucceededEvent( Json.decode<List<GKGameSession>>( json ) );
		}


		void loadSessionsFailed( string json )
		{
			if( loadSessionsFailedEvent != null )
				loadSessionsFailedEvent( P31Error.errorFromJson( json ) );
		}


		void loadSessionWidthIdentifierSucceeded( string json )
		{
			if( loadSessionWidthIdentifierSucceededEvent != null )
				loadSessionWidthIdentifierSucceededEvent( Json.decode<GKGameSession>( json ) );
		}


		void loadSessionWidthIdentifierFailed( string json )
		{
			if( loadSessionWidthIdentifierFailedEvent != null )
				loadSessionWidthIdentifierFailedEvent( P31Error.errorFromJson( json ) );
		}


		void removeSessionWithIdentifierSucceeded( string jsonOrEmptyString )
		{
			if( string.IsNullOrEmpty( jsonOrEmptyString ) )
				removeSessionWithIdentifierSucceededEvent.fire();
			else
				removeSessionWithIdentifierFailedEvent.fire( P31Error.errorFromJson( jsonOrEmptyString ) );
		}


		void loadShareUrlSucceeded( string url )
		{
			if( loadShareUrlSucceededEvent != null )
				loadShareUrlSucceededEvent( url );
		}


		void loadShareUrlFailed( string json )
		{
			if( loadShareUrlFailedEvent != null )
				loadShareUrlFailedEvent( P31Error.errorFromJson( json ) );
		}


		void loadDataFailed( string json )
		{
			if( loadDataFailedEvent != null )
				loadDataFailedEvent( P31Error.errorFromJson( json ) );
		}


		void saveDataFailed( string json )
		{
			if( saveDataFailedEvent != null )
				saveDataFailedEvent( P31Error.errorFromJson( json ) );
		}


		void saveDataSucceeded( string empty )
		{
			if( saveDataSucceededEvent != null )
				saveDataSucceededEvent();
		}		


		void setConnectionStateSucceeded( string empty )
		{
			if( setConnectionStateSucceededEvent != null )
				setConnectionStateSucceededEvent();
		}


		void setConnectionStateFailed( string json )
		{
			if( setConnectionStateFailedEvent != null )
				setConnectionStateFailedEvent( P31Error.errorFromJson( json ) );
		}


		void loadPlayersWithConnectionStateSucceeded( string json )
		{
			if( loadPlayersWithConnectionStateSucceededEvent != null )
				loadPlayersWithConnectionStateSucceededEvent( Json.decode<List<GKCloudPlayer>>( json ) );
		}


		void loadPlayersWithConnectionStateFailed( string json )
		{
			if( loadPlayersWithConnectionStateFailedEvent != null )
				loadPlayersWithConnectionStateFailedEvent( P31Error.errorFromJson( json ) );
		}


		void sendDataSucceeded( string empty )
		{
			sendDataSucceededEvent.fire();
		}


		void sendDataFailed( string json )
		{
			sendDataFailedEvent.fire( P31Error.errorFromJson( json ) );
		}


		void sendMessageSucceeded( string empty )
		{
			sendMessageSucceededEvent.fire();
		}


		void sendMessageFailed( string json )
		{
			sendMessageFailedEvent.fire( P31Error.errorFromJson( json ) );
		}


		void clearBadgeForPlayersSucceeded( string empty )
		{
			clearBadgeForPlayersSucceededEvent.fire();
		}


		void clearBadgeForPlayersFailed( string json )
		{
			clearBadgeForPlayersFailedEvent.fire( P31Error.errorFromJson( json ) );
		}
		
	}
}

#endif