﻿using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Utilities;
using Sourav.Utilities.Scripts;

namespace IdiotTest.Scripts.GameScripts
{
    public class GameDataManager : Singleton<GameDataManager>
    {
        #region Fields
        #region Private Fields
        [SerializeField]
        private GameData data;
        private bool hasDataChanged;
        #endregion

        #region Fields

        public GameMode CurrentGameMode
        {
            get { return data.CurrentGameMode; }
            set
            {
                data.CurrentGameMode = value;
                StateChanged();
            }
        }

        public SinglePlayerStatus SinglePlayerStatus
        {
            get { return data.SinglePlayerStatus; }
            set
            {
                data.SinglePlayerStatus = value;
                StateChanged();
            }
        }

        public int CurrentLevelProgress
        {
            get { return data.CurrentlevelProgress; }
            set
            {
                data.CurrentlevelProgress = value;
                StateChanged();
                if(data.CurrentlevelProgress < 5)
                {
                    UIManager.Instance.ChangeButtonFor_GetMoreLives(ActivateDeactivateAction.Deactivate, UIDeactivableButton.GoBack5Questions);
                }
                else
                {
                    UIManager.Instance.ChangeButtonFor_GetMoreLives(ActivateDeactivateAction.Activate, UIDeactivableButton.GoBack5Questions);
                }
            }
        }

        public int NumberOfAttemptsInThisLevel
        {
            get { return data.NumberOfAttemptsInThisLevel; }
            set
            {
                data.NumberOfAttemptsInThisLevel = value;
                StateChanged();
            }
        }

        public int RightAnswersForLevel
        {
            get { return data.RightAnswersForLevel; }
            set
            {
                data.RightAnswersForLevel = value;
                StateChanged();
            }
        }

        public int WrongAnswersForLevel
        {
            get { return data.WrongAnswersForLevel; }
            set
            {
                data.WrongAnswersForLevel = value;
                StateChanged();
            }
        }

        public int CurrentQuestion
        {
            get { return data.currentQuestion; }
            set
            {
                data.currentQuestion = value;
                StateChanged();
            }
        }

        public int CurrentTestQuestion
        {
            get { return data.currentTestQuestion; }
            set
            {
                data.currentTestQuestion = value;
                StateChanged();
            }
        }

        public int LastFallBackQuestion
        {
            get { return data.LastFallBackQuestion; }
            set
            {
                data.LastFallBackQuestion = value;
                StateChanged();
            }
        }

        public int LevelFallBackQuestion
        {
            get { return data.LevelFallBackQuestion; }
            set
            {
                data.LevelFallBackQuestion = value;
                StateChanged();
            }
        }

        public int QuestionAttempt
        {
            get { return data.QuestionAttempt; }
            set
            {
                data.QuestionAttempt = value;
                StateChanged();
            }
        }

        public int LivesEarned
        {
            get { return data.LivesEarned; }
            set
            {
                data.LivesEarned = value;
                StateChanged();
            }
        }

        public int CurrentLevel
        {
            get { return data.currentLevel; }
            set
            {
                data.currentLevel = value;
                StateChanged();
            }
        }

        public int TotalLevels
        {
            get { return data.TotalLevels; }
            //set
            //{
            //    data.TotalLevels = value;
            //    StateChanged();
            //}
        }

        public int RemainingLevels
        {
            get { return data.RemainingLevels; }
            set
            {
                data.RemainingLevels = value;
                StateChanged();
            }
        }

        public int QuestionsLeft
        {
            get { return data.QuestionsLeft; }
            set
            {
                data.QuestionsLeft = value;
                StateChanged();
            }
        }

        public int QuestionsAnsweredCorrectly
        {
            get { return data.QuestionsAnsweredCorrectly; }
            set
            {
                data.QuestionsAnsweredCorrectly = value;
                StateChanged();
            }
        }

        public int OverallAttemptsTaken
        {
            get
            {
                return data.OverallAttemptsTaken;
            }
            set
            {
                data.OverallAttemptsTaken = value;
                StateChanged();
            }
        }

        public int TotalLives
        {
            get { return data.TotalLives; }
            set
            {
                data.TotalLives = value;

                if (data.TotalLives < 0)
                {
                    data.TotalLives = 0;
                }

                AchievementManager.Instance.OnLivesChanged(data.TotalLives);
                StateChanged();
            }
        }

        public int Score
        {
            get { return data.Score; }
            set
            {
                data.Score = value;
                if (Score < 0)
                {
                    Score = 0;
                }
                StateChanged();
            }
        }

        public int TotalBananas
        {
            get { return data.TotalBananas; }
            set
            {
                data.TotalBananas = value;
                if (data.TotalBananas < 0)
                {
                    data.TotalBananas = 0;
                }

                AchievementManager.Instance.OnBananasChanged(data.TotalBananas);
                StateChanged();
            }
        }

        public bool HasNameBeenEntered
        {
            get { return data.HasNameBeenEntered; }
            set
            {
                data.HasNameBeenEntered = value;
                StateChanged();
            }
        }

        public OnOffButton MusicButton
        {
            get { return data.MusicButton; }
            set
            {
                data.MusicButton = value;
                StateChanged();
            }
        }

        public OnOffButton SFXButton
        {
            get { return data.SFXButton; }
            set
            {
                data.SFXButton = value;
                StateChanged();
            }
        }

        public OnOffButton TestButton
        {
            get { return data.TestButton; }
            set
            {
                data.TestButton = value;
                StateChanged();
            }
        }

        public BetAmount BetAmount
        {
            get { return data.BetAmount; }
            set
            {
                data.BetAmount = value;
                Debug.Log("BetAmount = "+data.BetAmount.ToString());
                StateChanged();
            }
        }

        //public string PlayerID
        //{
        //    get { return data.PlayerID; }
        //    set
        //    {
        //        PlayerID = value;

        //    }
        //}

        //ACHIEVEMENTS RELATED
        public int WinStreak_Regular
        {
            get { return data.WinStreak_Regular; }
            set
            {
                data.WinStreak_Regular = value;
                AchievementManager.Instance.OnStreakChanged_Regular(data.WinStreak_Regular + 1);
            }
        }

        public int WinStreak_Multiplayer
        {
            get { return data.WinStreak_Multiplayer; }
            set
            {
                data.WinStreak_Multiplayer = value;
                AchievementManager.Instance.OnStreakChanged_Multiplayer(data.WinStreak_Multiplayer + 1);
            }
        }

        public int LoseStreak_Multiplayer
        {
            get { return data.LoseStreak_Multiplayer; }
            set
            {
                data.LoseStreak_Multiplayer = value;
                AchievementManager.Instance.OnStreakChanged_Multiplayer(data.LoseStreak_Multiplayer + 1);
            }
        }

        public int QuestionPassed
        {
            get { return data.QuestionPassed; }
            set
            {
                data.QuestionPassed = value;
                AchievementManager.Instance.OnQuestionPassed(data.QuestionPassed + 1);
            }
        }

        //REMOVE ADS
        public bool IsRemoveAds
        {
            get { return data.IsRemoveAds; }
            set
            {
                data.IsRemoveAds = value;
                StateChanged();
            }
        }

        //UNLIMITED LIVES
        public bool IsUnlimitedLives
        {
            get { return data.IsUnlimitedLives; }
            set
            {
                data.IsUnlimitedLives = value;
                StateChanged();
            }
        }

        //REWARD RECEIVED FOR VIDEO ADS
        public bool IsRewardFromReviewReceived
        {
            get { return data.IsRewardForReviewReceived; }
            set
            {
                data.IsRewardForReviewReceived = value;
                StateChanged();
            }
        }

        #endregion
        #endregion

        #region Methods
        #region MONO Methods

        private void Awake()
        {
            if (!RetrieveData())
            {
                Debug.Log("!RetrieveData()");
                //ResetData();
                SaveData();
            }
            //PlayerID = RandomAlphaNumeric.GenerateRandomAlpha(3);
            //PlayerID += RandomAlphaNumeric.GenerateRandomNumeric(3);
            //Debug.Log("PlayerID = "+PlayerID);
        }

        private void Start()
        {
            string PlayerID = RandomAlphaNumeric.GenerateRandomAlpha(3);
            PlayerID += RandomAlphaNumeric.GenerateRandomNumeric(3);
            UIManager.Instance.SetPlayerID(PlayerID);
            StateChanged();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                if (hasDataChanged)
                {
                    SaveData();
                    hasDataChanged = false;
                }
            }
        }

        #endregion

        #region SAVE, RETRIEVE and RESET DATA

        void DataChanged()
        {
            hasDataChanged = true;
        }

        private void SaveData()
        {
            if (data == null)
            {
                Debug.Log("DATA IS NULL");
                ResetData();
            }
            string str = JsonUtility.ToJson(data);
            FileIO.WriteData(str);
        }

        private bool RetrieveData()
        {
            if (FileIO.FileExists())
            {
                Debug.Log("FILE EXISTS");
                string str = FileIO.ReadData();
                data = JsonUtility.FromJson<GameData>(str);
                return true;
            }

            return false;
        }

        private void ResetData()
        {
            data = new GameData();
        }

        private void StateChanged()
        {
            DataChanged();

            //FIXME Temporary
            SaveData();

            GameManager.Instance.GameModeChanged();
            UIManager.Instance.SingleplayerStatusChanged();
            GameManager.Instance.OverallAttemptsChanged();
            GameManager.Instance.LivesChanged();
            GameManager.Instance.BananasChanged();
            GameManager.Instance.MusicStateChanged();
            GameManager.Instance.SFXStateChanged();
            GameManager.Instance.TestStateChanged();
            GameManager.Instance.BetChanged();
            GameManager.Instance.LevelsChanged();
            GameManager.Instance.ScoreChanged();
        }

        #endregion

        #region LEVEL and GAME DATA RESET

        public void ResetData(ResetType type)
        {
            if (data == null)
                return;

            switch (type)
            {
                case ResetType.LevelSpecific:
                    data.ResetLevelSpecificData();
                    break;

                case ResetType.GameFinishedOrStart:
                    data.ResetAfterGameFinish();
                    break;

                case ResetType.Progress:
                    data.ResetProgress();
                    break;

                case ResetType.Full:
                    data.ResetFull();
                    break;

            }
        }

        #endregion

        #region Achievements Releated

        public bool AddAchievement(Achievement achievement)
        {
            if (data.Achievements.Contains(achievement))
            {
                return false;
            }
            data.Achievements.Add(achievement);
            StateChanged();
            return true;
        }
        #endregion
        #endregion
    }

    [Serializable]
    public class GameData
    {
        public GameMode CurrentGameMode;
        public SinglePlayerStatus SinglePlayerStatus;
        //LEVEL SPECIFIC------------------------>
        public int CurrentlevelProgress;
        // say 5 answered, so will look like 5/11
        public int NumberOfAttemptsInThisLevel;
        public int currentQuestion;
        public int currentTestQuestion;
        public int currentLevel;
        //LEVEL SPECIFIC------------------------>
        public int LivesEarned;
        public int RightAnswersForLevel;
        public int WrongAnswersForLevel;
        //RESET AFTER FULL GAME----------------->
        public int QuestionsLeft;
        public int QuestionsAnsweredCorrectly;
        public int OverallAttemptsTaken;
        public List<Achievement> Achievements;
        public int Score;
        public int TotalLevels;
        public int RemainingLevels;
        //RESET AFTER FULL GAME----------------->
        public int TotalLives;
        public int TotalBananas;
        public bool HasNameBeenEntered;

        public int LevelFallBackQuestion;
        public int LastFallBackQuestion;

        public int QuestionAttempt;

        public OnOffButton MusicButton;
        public OnOffButton SFXButton;
        public OnOffButton TestButton;
        public string PlayerName;

        public BetAmount BetAmount;

        //----------------ACHIEVEMENTS RELATED------------->
        public int WinStreak_Regular;
        public int WinStreak_Multiplayer;
        public int LoseStreak_Multiplayer;
        public int TotalQuestionsAnsweredCorrectly;
        public int QuestionPassed;
        //----------------ACHIEVEMENTS RELATED------------->

        public bool IsRemoveAds;
        public bool IsUnlimitedLives;
        public bool IsRewardForReviewReceived;

        public GameData()
        {
            ResetAll();
        }

        /// <summary>
        /// Resets the level specific data.
        /// </summary>
        public void ResetLevelSpecificData()
        {
            CurrentlevelProgress = 0;
            NumberOfAttemptsInThisLevel = 1;
            RightAnswersForLevel = 0;
            WrongAnswersForLevel = 0;
            QuestionAttempt = 1;
        }

        /// <summary>
        /// Resets the data after game finish or at game start for the first time.
        /// </summary>
        public void ResetAfterGameFinish()
        {
            ResetLevelSpecificData();
            //QuestionList = new List<int>();
            SetQuestions();
            currentQuestion = 0;
            currentTestQuestion = 0;
            currentLevel = 1;
            LivesEarned = 0;
            QuestionsLeft = 250;
            QuestionsAnsweredCorrectly = 0;
            OverallAttemptsTaken = 1;
            LastFallBackQuestion = 0;
            LevelFallBackQuestion = 0;
        }

        /// <summary>
        /// Resets all data except user name. This is for when player resets the game from settings.
        /// </summary>
        public void ResetProgress()
        {
            ResetAfterGameFinish();
            
            TotalLives = 10;
            TotalBananas = 10;
            BetAmount = BetAmount.Five;
            Achievements = new List<Achievement>();
            Score = 0;
            TotalLevels = 25;
            RemainingLevels = 25;
        }

        /// <summary>
        /// Complete reset.
        /// </summary>
        public void ResetFull()
        {
            ResetProgress();
            CurrentGameMode = GameMode.SinglePlayer;
            SinglePlayerStatus = SinglePlayerStatus.NewGame;
            HasNameBeenEntered = false;
            MusicButton = OnOffButton.On;
            SFXButton = OnOffButton.On;
            TestButton = OnOffButton.Off;
            PlayerName = string.Empty;

            WinStreak_Regular = 0;
            WinStreak_Multiplayer = 0;
            LoseStreak_Multiplayer = 0;
            TotalQuestionsAnsweredCorrectly = 0;
            QuestionPassed = 0;
        }

        private void ResetAll()
        {
            ResetFull();
            IsRemoveAds = false;
            IsUnlimitedLives = false;
            IsRewardForReviewReceived = false;
        }

        private void SetQuestions()
        {
            //TODO set questions in the list
        }
    }
}