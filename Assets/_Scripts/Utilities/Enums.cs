public enum GameMode
{
    SinglePlayer,
    Multiplayer
}

public enum GameStatus
{
    OutOfSession,
    InSession,
    InBetweenSession,
    IntroSession
}

public enum SinglePlayerStatus
{
    NewGame,
    ResumeGame
}

public enum QuestionType
{
    Base,
    NoBase
}

[System.Serializable]
public enum QPattern
{
    CatchMonkey,
    ConfusionTouch,
    Homophone,
    IfElse,
    ObjectInPrevious,
    PictureAnd4Answers,
    QuestionAnd4Answers,
    SlowFast,
    SmallestAndBiggest,
    SpeedTouch,
    TestYourSight,
    TextAnd4Answers,
    TouchAppearOrder3,
    TouchAppearOrder5,
    TouchFiveMen,
    TouchOneManQuickly,
    TrueFalse,
    XYZSeconds,
    //Unique Patterns
    Giraffe,
    Hifi,
    KeysLocks,
    KnockTheDoor,
    LadyHands,
    MemorizeOrder,
    MemorizeOrderHard,
    NextFiveBalloons,
    NextThreeBalloons,
    OpenCloseDoor,
    ThirdBalloon,
    TouchBallsWrongCount,
    TouchExceptYellowBall,
    Umbrellas,
    PlayButton,
    Different_Umbrellas,
    SmallestBlueBall,
    SpotMistake,
    EyeIdiot,
    BouncingMonkey,
    //Default
    Default

}

public enum Balls
{
    Ball_Blue,
    Ball_Red,
    Ball_Yellow,
    Ball_Green
}

public enum Balloons
{
    Balloon_Blue,
    Balloon_Red,
    Balloon_Yellow,
    Balloon_Green
}

public enum ResetType
{
    Full,
    Progress,
    GameFinishedOrStart,
    LevelSpecific
}

public enum ShuffleList
{
    None,
    FloatData,
    IntData
}

public enum AnswerID
{
    None,
    Correct,
    Wrong,
    //Eye
    Eye_1,
    Eye_2,
    Eye_3,
    Eye_4,
    Eye_5,
    //Men
    ConfusedMan,
    WinnerMan,
    LoserMan,
    StrongMan,
    WeakMan,
    HandyMan,
    SportsMan,
    SportsFan,
    HappyMan,
    GreedyMan,
    RichMan,
    //SantaMonkey
    Small_MonkeySanta,
    Medium_MonkeySanta,
    Big_MonkeySanta,
    //Balls
    Ball_Blue,
    Ball_Red,
    Ball_Yellow,
    Ball_Green,
    //Balloons
    Balloon_Blue,
    Balloon_Red,
    Balloon_Yellow,
    Balloon_Green,
    //MonkeyLuggage
    Fast_MonkeyLuggage,
    Medium_MonkeyLuggage,
    Slow_MonkeyLuggage,
    //Balls_SmallestAndBiggest
    Small_Ball,
    Middle_Ball,
    Big_Ball
}

public enum TypeOfMan
{
    ConfusedMan,
    WinnerMan,
    LoserMan,
    StrongMan,
    WeakMan,
    HandyMan,
    SportsMan,
    SportsFan,
    HappyMan,
    GreedyMan,
    RichMan
}

public enum OnOffButton
{
    On = 1,
    Off = 0
}

public enum ShowHideAction
{
    Hide,
    Show
}

public enum ActivateDeactivateAction
{
    Activate,
    Deactivate
}

public enum UIDeactivableButton
{
    GoBack5Questions,
    RewardAd
}

public enum AnswerResult
{
    Correct,
    Wrong
}

public enum SpriteID : int
{
    DummySprite = 0,
    AnswerBackGround,
    Babushka,
    Babushka_Bottom,
    Babushka_Top,
    Ball_Blue,
    Ball_Green,
    Ball_Yellow,
    Ball_Red,
    Balloon_Blue,
    Balloon_Green,
    Balloon_Yellow,
    Balloon_Red,
    Board,
    Cube,
    //Monkeys
    Monkey_Boxing,
    Monkey_Goggles,
    Monkey_Hifi,
    Monkey_Hint,
    Monkey_Luggage,
    Monkey_Orangutan,
    Monkey_Santa,
    Monkey_Thumbsup,
    Monkey_Whatever,
    Monkey_Relax,
    Monkey_Thinking1,
    Monkey_Thinking2,
    Monkey_Thinking3,
    Monkey_Buy,

    Door_Closed,
    Door_Opened,
    //Man
    Man_Confused,
    Man_Greedy,
    Man_Handy,
    Man_Happy,
    Man_Loser,
    Man_Rich,
    Man_SportsFan,
    Man_Sports,
    Man_Strong,
    Man_Weak,
    Man_Winner,

    Desert,
    Dessert,
    //Umbrella
    Umbrella_Drink,
    Umbrella_Folded,
    Umbrella_Open,
    Umbrella_WithMan,

    Email,
    Feet,
    Feat,
    Beer_Full,
    Beer_Empty,
    Giraffe,
    Zebra,
    Globe,
    HandShake,
    Jeans,
    Genes,
    Key,
    Lock,
    LadyHands,
    Lungs,
    Mail,
    MatchSticks,
    Meat,
    MultiFingers,
    NotSleepingCat,
    Pencil,
    PlayButton,
    PokerCards,
    Queue,
    Right,
    Wrong,
    See,
    Sea,
    Seats,
    Skull,
    Staircase,
    SweetPotato,
    Tape,
    ThreeColors,
    UsainBolt,
    Waist,
    Waste,
    Weight,
    WhiteBoard,
    Witch,
    Hen,
    Handshake,
    Elephant_Legs,
    DialPad,
    Deer,
    Cereals,
    CatOverBooks,
    Candles_On,
    Candles_Off,
    Calender,
    Background,
    Bear,
    Banana,
    Balance,
    TornTShirt,
    Triangles,
    //Monkey Poses
    Monkey_New_Front,
    //1
    Monkey_New_Tease,
    //2
    Monkey_New_Surprized,
    //3
    Monkey_New_Dream,
    //4
    Monkey_New_EarClose,
    //5
    Monkey_New_Hand,
    //6
    Monkey_New_TwoHands,
    //7
    Monkey_New_Taunt,
    //8
    Monkey_New_Yak,
    //9
    Monkey_New_HandStand,
    //10
    Monkey_New_HeadScrach,
    //11
    Monkey_New_Frustrated,
    //12
    Monkey_New_Confused,
    //13
    Monkey_New_Thinking,
    //14
    Monkey_New_Idea,
    //15
    Monkey_New_ThumbsUp,
    //16
    Monkey_New_ThumbsUpSide,
    //17
    Monkey_New_Side
    //18

}

public enum IDType
{
    AnswerID,
    SpriteID
}

public enum ConfusionTouch
{
    ColorX,
    WordX,
    WordColorX
}

public enum SpeedTouch
{
    OneX,
    ExceptX,
    AnyX,
    LeftX,
    RightX,
    AboveX,
    BelowX
}

public enum ColorEnum
{
    Red,
    Blue,
    Yellow,
    Green
}

public enum IfElseAppear
{
    First,
    Last
}

public enum ObjectsInPrevious
{
    In,
    NotIn
}

public enum OptionOrder
{
    First = 0,
    Second,
    Third,
    Fourth,
    Fifth,
    Sixth,
    Seventh,
    Eighth,
    Ninth,
    Tenth
}

public enum Achievement
{
    RookieStudent,
    TinyBrains,
    PeckyHead,
    TrickPrick,
    RiseAndShine,
    GoodGoing,
    KeepItUp,
    YouRock,
    IsThatAll,
    Struggler,
    JustGiveUp,
    JustMissed,
    Opportunist,
    ThatsTheSpirit,
    VictoryInName,
    ToughGetsGoing
}

public enum Currency
{
    Life,
    Banana
}

public enum AddDeductAction
{
    Add,
    Deduct
}

public enum MultiplayerGameplayType
{
    Person,
    AI
}

public enum MultiplayerSecondPlayerJoinStatus
{
    Joined,
    NotJoined
}
public enum MultiplayerGameType
{
    Random,
    Challenge
}
