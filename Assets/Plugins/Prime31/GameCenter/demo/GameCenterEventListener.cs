using UnityEngine;
using System.Collections;
using System.Collections.Generic;




namespace Prime31
{
	public partial class GameCenterEventListener : MonoBehaviour
	{
#if UNITY_IOS || UNITY_TVOS
		void Start()
		{
			// Listens to all the GameCenter events.  All event listeners MUST be removed before this object is disposed!
			// Player events
			GameCenterManager.loadPlayerDataFailedEvent += loadPlayerDataFailed;
			GameCenterManager.playerDataLoadedEvent += playerDataLoaded;
			GameCenterManager.playerAuthenticatedEvent += playerAuthenticated;
			GameCenterManager.playerFailedToAuthenticateEvent += playerFailedToAuthenticate;
			GameCenterManager.playerAuthenticationRequiredEvent += playerAuthenticationRequiredEvent;
			GameCenterManager.gameCenterDisabledEvent += gameCenterDisabledEvent;
			GameCenterManager.playerLoggedOutEvent += playerLoggedOut;
			GameCenterManager.profilePhotoLoadedEvent += profilePhotoLoaded;
			GameCenterManager.profilePhotoFailedEvent += profilePhotoFailed;
			GameCenterManager.generateIdentityVerificationSignatureSucceededEvent += generateIdentityVerificationSignatureSucceededEvent;
			GameCenterManager.generateIdentityVerificationSignatureFailedEvent += generateIdentityVerificationSignatureFailedEvent;

			// Leaderboards and scores
			GameCenterManager.loadCategoryTitlesFailedEvent += loadCategoryTitlesFailed;
			GameCenterManager.categoriesLoadedEvent += categoriesLoaded;
			GameCenterManager.reportScoreFailedEvent += reportScoreFailed;
			GameCenterManager.reportScoreFinishedEvent += reportScoreFinished;
			GameCenterManager.retrieveScoresFailedEvent += retrieveScoresFailed;
			GameCenterManager.scoresLoadedEvent += scoresLoaded;
			GameCenterManager.retrieveScoresForPlayerIdsFailedEvent += retrieveScoresForPlayerIdsFailed;
			GameCenterManager.scoresForPlayerIdsLoadedEvent += scoresForPlayerIdsLoaded;

			// Achievements
			GameCenterManager.reportAchievementFailedEvent += reportAchievementFailed;
			GameCenterManager.reportAchievementFinishedEvent += reportAchievementFinished;
			GameCenterManager.loadAchievementsFailedEvent += loadAchievementsFailed;
			GameCenterManager.achievementsLoadedEvent += achievementsLoaded;
			GameCenterManager.resetAchievementsFailedEvent += resetAchievementsFailed;
			GameCenterManager.resetAchievementsFinishedEvent += resetAchievementsFinished;
			GameCenterManager.retrieveAchievementMetadataFailedEvent += retrieveAchievementMetadataFailed;
			GameCenterManager.achievementMetadataLoadedEvent += achievementMetadataLoaded;

			// Challenges
			GameCenterManager.localPlayerDidReceiveChallengeEvent += localPlayerDidReceiveChallengeEvent;
			GameCenterManager.localPlayerDidSelectChallengeEvent += localPlayerDidSelectChallengeEvent;
			GameCenterManager.localPlayerDidCompleteChallengeEvent += localPlayerDidCompleteChallengeEvent;
			GameCenterManager.remotePlayerDidCompleteChallengeEvent += remotePlayerDidCompleteChallengeEvent;
			GameCenterManager.challengesLoadedEvent += challengesLoadedEvent;
			GameCenterManager.challengesFailedToLoadEvent += challengesFailedToLoadEvent;
			GameCenterManager.challengeIssuedSuccessfullyEvent += challengeIssuedSuccessfullyEvent;
			GameCenterManager.challengeNotIssuedEvent += challengeNotIssuedEvent;

			// Saved Games
			GameCenterManager.fetchSavedGamesSucceededEvent += fetchSavedGamesSucceededEvent;
			GameCenterManager.fetchSavedGamesFailedEvent += fetchSavedGamesFailedEvent;
			GameCenterManager.saveGameSucceededEvent += saveGameSucceededEvent;
			GameCenterManager.saveGameFailedEvent += saveGameFailedEvent;
			GameCenterManager.deleteSavedGameSucceededEvent += deleteSavedGameSucceededEvent;
			GameCenterManager.deleteSavedGameFailedEvent += deleteSavedGameFailedEvent;
			GameCenterManager.loadSavedGameDataSucceededEvent += loadSavedGameDataSucceededEvent;
			GameCenterManager.loadSavedGameDataFailedEvent += loadSavedGameDataFailedEvent;
		}


		void OnDisable()
		{
			// Remove all the event handlers
			// Player events
			GameCenterManager.loadPlayerDataFailedEvent -= loadPlayerDataFailed;
			GameCenterManager.playerDataLoadedEvent -= playerDataLoaded;
			GameCenterManager.playerAuthenticatedEvent -= playerAuthenticated;
			GameCenterManager.playerFailedToAuthenticateEvent -= playerFailedToAuthenticate;
			GameCenterManager.playerAuthenticationRequiredEvent -= playerAuthenticationRequiredEvent;
			GameCenterManager.gameCenterDisabledEvent -= gameCenterDisabledEvent;
			GameCenterManager.playerLoggedOutEvent -= playerLoggedOut;
			GameCenterManager.profilePhotoLoadedEvent -= profilePhotoLoaded;
			GameCenterManager.profilePhotoFailedEvent -= profilePhotoFailed;
			GameCenterManager.generateIdentityVerificationSignatureSucceededEvent -= generateIdentityVerificationSignatureSucceededEvent;
			GameCenterManager.generateIdentityVerificationSignatureFailedEvent -= generateIdentityVerificationSignatureFailedEvent;

			// Leaderboards and scores
			GameCenterManager.loadCategoryTitlesFailedEvent -= loadCategoryTitlesFailed;
			GameCenterManager.categoriesLoadedEvent -= categoriesLoaded;
			GameCenterManager.reportScoreFailedEvent -= reportScoreFailed;
			GameCenterManager.reportScoreFinishedEvent -= reportScoreFinished;
			GameCenterManager.retrieveScoresFailedEvent -= retrieveScoresFailed;
			GameCenterManager.scoresLoadedEvent -= scoresLoaded;
			GameCenterManager.retrieveScoresForPlayerIdsFailedEvent -= retrieveScoresForPlayerIdsFailed;
			GameCenterManager.scoresForPlayerIdsLoadedEvent -= scoresForPlayerIdsLoaded;

			// Achievements
			GameCenterManager.reportAchievementFailedEvent -= reportAchievementFailed;
			GameCenterManager.reportAchievementFinishedEvent -= reportAchievementFinished;
			GameCenterManager.loadAchievementsFailedEvent -= loadAchievementsFailed;
			GameCenterManager.achievementsLoadedEvent -= achievementsLoaded;
			GameCenterManager.resetAchievementsFailedEvent -= resetAchievementsFailed;
			GameCenterManager.resetAchievementsFinishedEvent -= resetAchievementsFinished;
			GameCenterManager.retrieveAchievementMetadataFailedEvent -= retrieveAchievementMetadataFailed;
			GameCenterManager.achievementMetadataLoadedEvent -= achievementMetadataLoaded;

			// Challenges
			GameCenterManager.localPlayerDidReceiveChallengeEvent -= localPlayerDidReceiveChallengeEvent;
			GameCenterManager.localPlayerDidSelectChallengeEvent -= localPlayerDidSelectChallengeEvent;
			GameCenterManager.localPlayerDidCompleteChallengeEvent -= localPlayerDidCompleteChallengeEvent;
			GameCenterManager.remotePlayerDidCompleteChallengeEvent -= remotePlayerDidCompleteChallengeEvent;
			GameCenterManager.challengesLoadedEvent -= challengesLoadedEvent;
			GameCenterManager.challengesFailedToLoadEvent -= challengesFailedToLoadEvent;
			GameCenterManager.challengeIssuedSuccessfullyEvent -= challengeIssuedSuccessfullyEvent;
			GameCenterManager.challengeNotIssuedEvent -= challengeNotIssuedEvent;

			// Saved Games
			GameCenterManager.fetchSavedGamesSucceededEvent -= fetchSavedGamesSucceededEvent;
			GameCenterManager.fetchSavedGamesFailedEvent -= fetchSavedGamesFailedEvent;
			GameCenterManager.saveGameSucceededEvent -= saveGameSucceededEvent;
			GameCenterManager.saveGameFailedEvent -= saveGameFailedEvent;
			GameCenterManager.deleteSavedGameSucceededEvent -= deleteSavedGameSucceededEvent;
			GameCenterManager.deleteSavedGameFailedEvent -= deleteSavedGameFailedEvent;
			GameCenterManager.loadSavedGameDataSucceededEvent -= loadSavedGameDataSucceededEvent;
			GameCenterManager.loadSavedGameDataFailedEvent -= loadSavedGameDataFailedEvent;
		}



		#region Player Events

		void playerAuthenticated()
		{
			Debug.Log( "playerAuthenticatedEvent" );
		}


		void playerFailedToAuthenticate( string error )
		{
			Debug.Log( "playerFailedToAuthenticateEvent: " + error );
		}


		void playerAuthenticationRequiredEvent()
		{
			Debug.Log( "playerAuthenticationRequiredEvent" );
		}
		
		
		void gameCenterDisabledEvent()
		{
			Debug.Log( "gameCenterDisabledEvent" );
		}


		void playerLoggedOut()
		{
			Debug.Log( "playerLoggedOutEvent" );
		}


		void playerDataLoaded( List<GameCenterPlayer> players )
		{
			Debug.Log( "playerDataLoadedEvent" );
			foreach( var player in players )
				Debug.Log( player );
		}


		void loadPlayerDataFailed( string error )
		{
			Debug.Log( "loadPlayerDataFailedEvent: " + error );
		}


		void profilePhotoLoaded( string path )
		{
			Debug.Log( "profilePhotoLoadedEvent: " + path );
		}


		void profilePhotoFailed( string error )
		{
			Debug.Log( "profilePhotoFailedEvent: " + error );
		}


		void generateIdentityVerificationSignatureSucceededEvent( Dictionary<string,string> dictionary )
		{
			Debug.Log( "generateIdentityVerificationSignatureSucceededEvent" );
			Prime31.Utils.logObject( dictionary );
		}


		void generateIdentityVerificationSignatureFailedEvent( string error )
		{
			Debug.Log( "generateIdentityVerificationSignatureFailedEvent" );
		}

		#endregion;



		#region Leaderboard Events

		void categoriesLoaded( List<GameCenterLeaderboard> leaderboards )
		{
			Debug.Log( "categoriesLoadedEvent" );
			Prime31.Utils.logObject( leaderboards );
		}


		void loadCategoryTitlesFailed( string error )
		{
			Debug.Log( "loadCategoryTitlesFailedEvent: " + error );
		}

		#endregion;


		#region Score Events

		void scoresLoaded( GameCenterRetrieveScoresResult retrieveScoresResult )
		{
			Debug.Log( "scoresLoadedEvent" );
			Prime31.Utils.logObject( retrieveScoresResult );
		}


		void retrieveScoresFailed( string error )
		{
			Debug.Log( "retrieveScoresFailedEvent: " + error );
		}


		void retrieveScoresForPlayerIdsFailed( string error )
		{
			Debug.Log( "retrieveScoresForPlayerIdFailedEvent: " + error );
		}


		void scoresForPlayerIdsLoaded( GameCenterRetrieveScoresResult retrieveScoresResult )
		{
			Debug.Log( "scoresForPlayerIdsLoadedEvent" );
			Prime31.Utils.logObject( retrieveScoresResult );
		}


		void reportScoreFinished( string category )
		{
			Debug.Log( "reportScoreFinishedEvent for category: " + category );
		}


		void reportScoreFailed( string error )
		{
			Debug.Log( "reportScoreFailedEvent: " + error );
		}

		#endregion;


		#region Achievement Events

		void achievementMetadataLoaded( List<GameCenterAchievementMetadata> achievementMetadata )
		{
			Debug.Log( "achievementMetadatLoadedEvent" );
			foreach( GameCenterAchievementMetadata s in achievementMetadata )
				Debug.Log( s );
		}


		void retrieveAchievementMetadataFailed( string error )
		{
			Debug.Log( "retrieveAchievementMetadataFailedEvent: " + error );
		}


		void resetAchievementsFinished()
		{
			Debug.Log( "resetAchievmenetsFinishedEvent" );
		}


		void resetAchievementsFailed( string error )
		{
			Debug.Log( "resetAchievementsFailedEvent: " + error );
		}


		void achievementsLoaded( List<GameCenterAchievement> achievements )
		{
			Debug.Log( "achievementsLoadedEvent" );
			foreach( GameCenterAchievement s in achievements )
				Debug.Log( s );
		}


		void loadAchievementsFailed( string error )
		{
			Debug.Log( "loadAchievementsFailedEvent: " + error );
		}


		void reportAchievementFinished( string identifier )
		{
			Debug.Log( "reportAchievementFinishedEvent: " + identifier );
		}


		void reportAchievementFailed( string error )
		{
			Debug.Log( "reportAchievementFailedEvent: " + error );
		}

		#endregion;


		#region Challenges

		void localPlayerDidReceiveChallengeEvent( GameCenterChallenge challenge )
		{
			Debug.Log( "localPlayerDidReceiveChallengeEvent: " + challenge );
		}


		void localPlayerDidSelectChallengeEvent( GameCenterChallenge challenge )
		{
			Debug.Log( "localPlayerDidSelectChallengeEvent : " + challenge );
		}


		void localPlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
		{
			Debug.Log( "localPlayerDidCompleteChallengeEvent : " + challenge );
		}


		void remotePlayerDidCompleteChallengeEvent( GameCenterChallenge challenge )
		{
			Debug.Log( "remotePlayerDidCompleteChallengeEvent : " + challenge );
		}


		void challengesLoadedEvent( List<GameCenterChallenge> list )
		{
			Debug.Log( "challengesLoadedEvent" );
			Prime31.Utils.logObject( list );
		}


		void challengesFailedToLoadEvent( string error )
		{
			Debug.Log( "challengesFailedToLoadEvent: " + error );
		}


		void challengeIssuedSuccessfullyEvent( List<object> playerIds )
		{
			Debug.Log( "challengeIssuedSuccessfullyEvent" );
			Prime31.Utils.logObject( playerIds );
		}


		void challengeNotIssuedEvent()
		{
			Debug.Log( "challengeNotIssuedEvent" );
		}

		#endregion


		#region Saved Games

		void fetchSavedGamesSucceededEvent( List<GKSavedGame> games )
		{
			Utils.logObject( games );
			Debug.Log( "fetchSavedGamesSucceededEvent" );
		}


		void fetchSavedGamesFailedEvent( P31Error error )
		{
			Debug.Log( "fetchSavedGamesFailedEvent: " + error );
		}


		void saveGameSucceededEvent()
		{
			Debug.Log( "saveGameSucceededEvent" );
		}


		void saveGameFailedEvent( P31Error error )
		{
			Debug.Log( "saveGameFailedEvent: " + error );
		}


		void deleteSavedGameSucceededEvent()
		{
			Debug.Log( "deleteSavedGameSucceededEvent" );
		}


		void deleteSavedGameFailedEvent( P31Error error )
		{
			Debug.Log( "deleteSavedGameFailedEvent: " + error );
		}


		void loadSavedGameDataSucceededEvent( byte[] data )
		{
			Debug.Log( "loadSavedGameDataSucceededEvent. Data length: " + data.Length );
			Debug.Log( System.Text.Encoding.UTF8.GetString( data ) );
		}


		void loadSavedGameDataFailedEvent( P31Error error )
		{
			Debug.Log( "loadSavedGameDataFailedEvent: " + error );
		}

		#endregion

#endif
	}

}
