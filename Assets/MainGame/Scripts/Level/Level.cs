using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : CustomBehaviour
{
    #region Collections
    public List<Interactable> InteractableList;
    #endregion

    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
    }

    public void ActivateAndInitializeInteractables()
    {
        if(InteractableList != null)
        {
            InteractableList.ForEach(x =>
            {
                x.gameObject.SetActive(true);
                x.Initialize(GameManager);
            });
        }
    }

    public void DeactivateInteractables()
    {
        if(InteractableList != null)
        {
            InteractableList.ForEach(x =>
            {
                x.gameObject.SetActive(false);
            });
        }
    }
}
