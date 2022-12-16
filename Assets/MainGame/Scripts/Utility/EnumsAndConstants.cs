using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumsAndConstants
{

}

public class ConstantDatas
{
    public const string PLAYER_DATA = "PlayerData";

    public const int TOTAL_PLAYER_COUNT = 20;
    public const int TOTAL_LEVEL_COUNT = 2;

    public const string SOUND_STATE = "SoundState";
    public const string VIBRATION_STATE = "VibrationState";

    public const int LEVEL_COMPLETE_REWARD = 100;
    public const int LEVEL_FAIL_REWARD = 50;

    public const string SWIPE_TO_PLAY = "SWIPE TO PLAY";
    public const string TAP_TO_PLAY = "TAP TO PLAY";
}
public class TAGS
{
    public const string Player = "Player";
    public const string AI = "AI";
    public const string Enemy = "Enemy";
    public const string Interactable = "Interactable";
    public const string Collectable = "Collectable";
    public const string Obstacle = "Obstacle";
    public const string Environment = "Environment";
    public const string Skill = "Skill";
}

public class AnimationTriggers
{
    public const string IDLE = "Idle";
    public const string RUNWITHSOURCE = "RunWithSource";
    public const string IDLEWITHSOURCE = "IdleWithSource";
    public const string PUTSOURCE = "PutSource";
    public const string RUN = "Run";
    public const string JUMP = "Jump";
    public const string DIE = "Die";
    public const string HIT = "Hit";
    public const string GETHIT = "GetHit";
    public const string CHEER = "Cheer";
}
public enum GameMechanics
{
    Swipe = 0,
    Tap = 1,
    TapOnTime = 2,
    Other = 3
}

public enum StartTextTypes
{
    TapToPlay = 0,
    SwipeToPlay = 1
}

public enum ConstantMoveAxis
{
    X = 0,
    Y = 1,
    Z = 2
}

public enum ControlType
{
    Swipe = 0,
    Joystick = 1,
    TapToMove = 2
}

public enum GuideTypes
{
    Swipe = 0,
    Infinity = 1,
    None = 2
}

public enum Panels
{
    MainMenu = 0,
    Hud = 1,
    Settings = 2,
    Finish = 3,
    Initial = 4,
    Pause = 5,
    SelectSkill = 6,

    MAX = 7
}

public enum PlayerStates
{
    Selected = 1,
    Open = 2,
    Purchase = 3,

    MAX = 4
}

public enum ClickSounds
{
    Click = 0,
    Purhace = 1,
    Upgrade = 2,

    MAX = 3
}

public enum GameStateSounds
{
    Countdown = 0,
    ReadyGo = 1,
    LevelSuccess = 2,
    LevelFinish = 3,

    MAX = 4
}

public enum PlayerInteractionSounds
{
    Fail = 0,
    OnFire = 1,
    LevelWin = 2,

    MAX = 3
}

public enum ConfettiTypes
{
    Directional = 0,
    Explosion = 1,
    Fountain = 2
}

public enum ConfettiColors
{
    Green = 0,
    Halloween = 1,
    Magic = 2,
    Purple = 3,
    Rainbow = 4,
    Romantic = 5
}

public enum SkillNature
{
    Damage = 0,
    Utility = 1
}

public enum DamagePattern
{
    Yoyo = 1,
    Projectile = 2,
    Bouncing = 3,
    Area = 4,
    Shotgun = 5,
    Whip = 6,
    Bomb = 7,
    Bounce = 8,
    SkunkGas = 9,
    BananaGuardian = 10
}

public enum PoolerType
{
    ChestnutPooler = 0,
    CloverPooler = 1,
    PoisonDartPooler = 2,
    SlingPooler = 3,
    WhipPooler = 4,
    SpiderWebPooler = 5,
    SpiderPoisonPooler = 6,
    BeeShotPooler = 7,
    BirdBomb = 8,
    SkunGasPooler = 9,
    BananaPooler = 10
}

public enum BossAttackType
{
    ChargeAttackWithInd = 0,
    TripleCharge = 1,
    JumpLandingWithInd = 2,
    Shotgun = 3,
    RapidFire = 4,
    CuverShot = 5,
    Dot = 6
}

public enum BossMoveType
{
    Stationary = 0,
    Chase = 1,
}

public enum CollectablePoolerType
{
    BarrelPooler = 0,
    MagnetPooler = 1,
    BombPooler = 2
}

public enum EnemyPoolerType
{
    BasicEnemyPooler = 0,
    BasicRangedPooler = 1,
}

public enum EnemyBulletPoolerType
{
    BasicBullet = 0
}

public enum CoinType
{
    Small = 0,
    Medium = 1,
    Large = 2
}
public enum ExpPoolerType
{
    SmallExperience = 0
}

public enum UpgradeLevel
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2,
    Level4 = 3,
    Level5 = 4
}

public enum UpgradeType
{
    PropertyChange = 0,
    Evolve = 1
}

public enum PropertyChangeType
{
    Multiplication = 0,
    Addition = 1
}

public enum PropertyToChange
{
    SizeX = 0,
    Size = 1,
    Damage = 2,
    Speed = 3,
    Count = 4,
    Cooldown = 5,
    Range = 6,
    Evolve = 7,
    Guardian = 8
}

public enum UtilityPattern
{
    MovementSpeed = 0,
    Health = 1,
    DamageReduction = 2,
    RecharcableShield = 3,
    HealthRegen = 4,
    WeaponCooldownReduction = 5
}

public enum UtilityTypeToChange
{
    Value = 0,
    Duration = 1
}
