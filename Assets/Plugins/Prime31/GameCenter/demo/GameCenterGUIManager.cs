using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Prime31;



namespace Prime31
{
	public partial class GameCenterGUIManager : MonoBehaviourGUI
	{
#if UNITY_IOS || UNITY_TVOS
		// some useful ivars to hold information retrieved from GameCenter. These will make it easier
		// to test this code with your GameCenter enabled application because they allow the sample
		// to not hardcode any values for leaderboards and achievements.
		private List<GameCenterLeaderboard> _leaderboards;
		private List<GameCenterAchievementMetadata> _achievementMetadata;
		private List<GameCenterPlayer> _friends;

		// bools used to hide/show different buttons based on what is loaded
		private bool _hasFriends;
		private bool _hasLeaderboardData;
		private bool _hasAchievementData;


		void Start()
		{
			// use anonymous delegates for this simple example for gathering data from GameCenter. In production you would want to
			// add and remove your event listeners in OnEnable/OnDisable!
			GameCenterManager.categoriesLoadedEvent += delegate( List<GameCenterLeaderboard> leaderboards )
			{
				_leaderboards = leaderboards;
				_hasLeaderboardData = _leaderboards != null && _leaderboards.Count > 0;
			};

			GameCenterManager.achievementMetadataLoadedEvent += delegate( List<GameCenterAchievementMetadata> achievementMetadata )
			{
				_achievementMetadata = achievementMetadata;
				_hasAchievementData = _achievementMetadata != null && _achievementMetadata.Count > 0;
			};

			// after authenticating grab the players profile image
			GameCenterManager.playerAuthenticatedEvent += () =>
			{
				GameCenterBinding.loadProfilePhotoForLocalPlayer();
				loadFriends();
			};

			// always authenticate at every launch
			GameCenterBinding.authenticateLocalPlayer();
		}


		private void loadFriends()
		{
			GameCenterManager.playerDataLoadedEvent += friends =>
			{
				_friends = friends;
				_hasFriends = _friends != null && _friends.Count > 0;
			};

			Debug.Log( "player is authenticated so we are loading friends" );
			GameCenterBinding.retrieveFriends( true, true );
		}


		void OnGUI()
		{
			beginColumn();

			// toggle to show two different sets of buttons
			if( toggleButtonState( "Show Game Save Buttons" ) )
				columnOneGeneral();
			else
				columnOneGameSave();
			toggleButton( "Show Game Save Buttons", "Show General Buttons" );

			endColumn( true );



			// toggle to show two different sets of buttons
			if( toggleButtonState( "Show Achievement Buttons" ) )
				leaderboardsGUI();
			else
				achievementsGUI();
			toggleButton( "Show Achievement Buttons", "Show Leaderboard Buttons" );


			endColumn();


			if( bottomLeftButton( "Multiplayer Scene (Requires Multiplayer Plugin!)" ) )
			{
				loadLevel( "GameCenterMultiplayerTestScene" );
			}


			if( bottomRightButton( "Turn Based Multiplayer Scene (Requires TB Plugin!)" ) )
			{
				loadLevel( "GCTurnBasedTestScene" );
			}
		}


		void columnOneGeneral()
		{
			if( GUILayout.Button( "Get Player Alias" ) )
			{
				var alias = GameCenterBinding.playerAlias();
				Debug.Log( "Player alias: " + alias );
			}


			if( _hasFriends )
			{
				// see if we have any friends with a profile image on disk
				var friendWithProfileImage = _friends.Where( f => f.hasProfilePhoto ).FirstOrDefault();
				GUI.enabled = friendWithProfileImage != null;
				if( GUILayout.Button( "Show Friends Profile Image" ) )
				{
					var tex = friendWithProfileImage.profilePhoto;

					// grab our cube and display it with the texture
					var cube = GameObject.Find( "Cube" );
					cube.GetComponent<Renderer>().enabled = true;
					cube.GetComponent<Renderer>().material.mainTexture = tex;
				}
				GUI.enabled = true;
			}


			if( GUILayout.Button( "Load Received Challenges" ) )
			{
				GameCenterBinding.loadReceivedChallenges();
			}


			if( GUILayout.Button( "Show Leaderboards" ) )
			{
				GameCenterBinding.showGameCenterViewController( GameCenterViewControllerState.Leaderboards );
			}


			if( GUILayout.Button( "Show Achievements" ) )
			{
				GameCenterBinding.showGameCenterViewController( GameCenterViewControllerState.Achievements );
			}


			if( GUILayout.Button( "Show Challenges" ) )
			{
				GameCenterBinding.showGameCenterViewController( GameCenterViewControllerState.Challenges );
			}


			if( GUILayout.Button( "Generate Identity Signature" ) )
			{
				GameCenterBinding.generateIdentityVerificationSignature();
			}


			if( GUILayout.Button( "Show Custom Notification Banner" ) )
			{
				GameCenterBinding.showCustomNotificationBanner( "This is my custom banner", "It lets me stick whatever I want in a Game Center styled banner", 4f );
			}
		}


		void columnOneGameSave()
		{
			if( button( "Fetch Saved Games" ) )
			{
				GKSavedGame.fetchSavedGames();
			}


			if( button( "Save Game" ) )
			{
				var data = System.Text.Encoding.UTF8.GetBytes( "string saved data" );
				GKSavedGame.saveGame( "slot1", data );
			}


			if( button( "Delete Saved Game" ) )
			{
				GKSavedGame.deleteSavedGame( "slot1" );
			}


			if( button( "Load Saved Game Data" ) )
			{
				GKSavedGame.loadSavedGameData( "slot1" );
			}

		}


		void leaderboardsGUI()
		{
			if( GUILayout.Button( "Load Leaderboard Data" ) )
			{
				GameCenterBinding.loadLeaderboardTitles();
			}


			if( GUILayout.Button( "Show Leaderboards" ) )
			{
				GameCenterBinding.showLeaderboardWithTimeScope( GameCenterLeaderboardTimeScope.AllTime );
			}


			if( !_hasLeaderboardData )
			{
				GUILayout.Label( "Load leaderboard data to see more options" );
				return;
			}

			if( GUILayout.Button( "Post Score" ) )
			{
				var leaderboardId = _leaderboards[Random.Range( 0, _leaderboards.Count )].leaderboardId;
				Debug.Log( "about to report a random score for leaderboard: " + leaderboardId );
				GameCenterBinding.reportScore( Random.Range( 1, 99999 ), leaderboardId );
			}


			if( GUILayout.Button( "Issue Score Challenge" ) )
			{
				GameCenterBinding.issueScoreChallenge( Random.Range( 1, 9999 ), 0, _leaderboards[0].leaderboardId, null, "Beat this score!" );
			}


			if( GUILayout.Button( "Get Raw Score Data" ) )
			{
				GameCenterBinding.retrieveScores( false, GameCenterLeaderboardTimeScope.AllTime, 1, 25, _leaderboards[0].leaderboardId );
			}


			if( GUILayout.Button( "Get Scores for Me" ) )
			{
				foreach( var leaderboard in _leaderboards )
					GameCenterBinding.retrieveScoresForPlayerIds( new string[] { GameCenterBinding.playerIdentifier() }, leaderboard.leaderboardId );
			}


			if( GUILayout.Button( "Show Leaderboard " + _leaderboards[0].leaderboardId ) )
			{
				GameCenterBinding.showLeaderboardWithTimeScopeAndLeaderboard( GameCenterLeaderboardTimeScope.Week, _leaderboards[0].leaderboardId );
			}
		}


		void achievementsGUI()
		{
			if( GUILayout.Button( "Load Achievement Metadata" ) )
			{
				GameCenterBinding.retrieveAchievementMetadata();
			}


			if( GUILayout.Button( "Get Raw Achievements" ) )
			{
				GameCenterBinding.getAchievements();
			}


			if( GUILayout.Button( "Show Achievements" ) )
			{
				GameCenterBinding.showAchievements();
			}


			if( GUILayout.Button( "Reset Achievements" ) )
			{
				GameCenterBinding.resetAchievements();
			}


			if( !_hasAchievementData )
			{
				GUILayout.Label( "Load achievement metadata to see more options" );
				return;
			}


			if( GUILayout.Button( "Post Achievement" ) )
			{
				int percentComplete = (int)Random.Range( 2, 60 );
				Debug.Log( "sending percentComplete: " + percentComplete );
				GameCenterBinding.reportAchievement( _achievementMetadata[0].identifier, percentComplete );
			}


			if( GUILayout.Button( "Issue Achievement Challenge" ) )
			{
				GameCenterBinding.issueAchievementChallenge( _achievementMetadata[0].identifier, null, "I challenge you" );
			}
		}

#endif
	}

}
