using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : CustomBehaviour
{
    public bool UseConfetti;
    public ConfettiTypes ConfettiType = ConfettiTypes.Directional;
    public ConfettiColors ConfettiColor = ConfettiColors.Green;
    public List<GameObject> SpawnPointList;

    [Header("Confettie Prefabs")]
    public List<ParticleSystem> Directional;
    public List<ParticleSystem> Explosion;
    public List<ParticleSystem> Fountain;

    public Dictionary<string, List<ParticleSystem>> ConfettiDict = new Dictionary<string, List<ParticleSystem>>();

    private ParticleSystem mSelectedConfetti;
    private List<GameObject> mSpawnedConfettiList = new List<GameObject>();

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);

        ConfettiDict["Directional"] = Directional;
        ConfettiDict["Explosion"] = Explosion;
        ConfettiDict["Fountain"] = Fountain;

        GameManager.OnDistributionStart += OnDistrubitionStart;
        //GameManager.OnTutorialCompleted += OnTutorialComplete;
        GameManager.OnReturnToMainMenu += OnResetToMainMenu;
    }

    private void ActivateConfetties()
    {
        if (UseConfetti)
        {
            mSelectedConfetti = GetSelectedConfetti();
            mSpawnedConfettiList = new List<GameObject>();

            foreach (var spawnPoint in SpawnPointList)
            {
                var tmpConfetti = Instantiate(mSelectedConfetti, spawnPoint.transform);
                mSpawnedConfettiList.Add(tmpConfetti.gameObject);
            }
        }
    }

    private void DestroyConfetties()
    {
        mSpawnedConfettiList.ForEach(confetti => Destroy(confetti.gameObject));
        mSpawnedConfettiList.Clear();
    }

    private ParticleSystem GetSelectedConfetti()
    {
        return ConfettiDict[ConfettiType.ToString()][ConfettiColor.GetHashCode()];
    }

    #region Events

    private void OnDistrubitionStart()
    {
        ActivateConfetties();
    }

    private void OnTutorialComplete()
    {
        ActivateConfetties();
    }

    private void OnResetToMainMenu()
    {
        DestroyConfetties();
    }

    private void OnDestroy()
    {
        if (GameManager != null)
        {
            //GameManager.OnLevelCompleted -= OnDistrubitionStart;
            GameManager.OnDistributionStart -= OnDistrubitionStart;
            //GameManager.OnTutorialCompleted -= OnTutorialComplete;
            GameManager.OnReturnToMainMenu -= OnResetToMainMenu;
        }
    }

    #endregion
}
