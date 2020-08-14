using UnityEngine;

public class RegionTrigger : MonoBehaviour
{
    public RCEvent playerEventEnter;
    public RCEvent titanEventEnter;
    public RCEvent playerEventExit;
    public RCEvent titanEventExit;
    public string myName;

    public void CopyTrigger(RegionTrigger copyTrigger)
    {
        playerEventEnter = copyTrigger.playerEventEnter;
        titanEventEnter = copyTrigger.titanEventEnter;
        playerEventExit = copyTrigger.playerEventExit;
        titanEventExit = copyTrigger.titanEventExit;
        myName = copyTrigger.myName;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject gameObject = other.transform.gameObject;
        if (gameObject.layer == 8)
        {
            if (playerEventEnter == null)
            {
                return;
            }
            HERO component = gameObject.GetComponent<HERO>();
            if (component != null)
            {
                string key = (string)FengGameManagerMKII.RCVariableNames["OnPlayerEnterRegion[" + myName + "]"];
                if (FengGameManagerMKII.PlayerVariables.ContainsKey(key))
                {
                    FengGameManagerMKII.PlayerVariables[key] = component.photonView.owner;
                }
                else
                {
                    FengGameManagerMKII.PlayerVariables.Add(key, component.photonView.owner);
                }
                playerEventEnter.CheckEvent();
            }
        }
        else
        {
            if (gameObject.layer != 11 || titanEventEnter == null)
            {
                return;
            }
            TITAN component2 = gameObject.transform.root.gameObject.GetComponent<TITAN>();
            if (component2 != null)
            {
                string key = (string)FengGameManagerMKII.RCVariableNames["OnTitanEnterRegion[" + myName + "]"];
                if (FengGameManagerMKII.TitanVariables.ContainsKey(key))
                {
                    FengGameManagerMKII.TitanVariables[key] = component2;
                }
                else
                {
                    FengGameManagerMKII.TitanVariables.Add(key, component2);
                }
                titanEventEnter.CheckEvent();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject gameObject = other.transform.root.gameObject;
        if (gameObject.layer == 8)
        {
            if (playerEventExit == null)
            {
                return;
            }
            HERO component = gameObject.GetComponent<HERO>();
            if (component != null)
            {
                string key = (string)FengGameManagerMKII.RCVariableNames["OnPlayerLeaveRegion[" + myName + "]"];
                if (FengGameManagerMKII.PlayerVariables.ContainsKey(key))
                {
                    FengGameManagerMKII.PlayerVariables[key] = component.photonView.owner;
                }
                else
                {
                    FengGameManagerMKII.PlayerVariables.Add(key, component.photonView.owner);
                }
            }
        }
        else
        {
            if (gameObject.layer != 11 || titanEventExit == null)
            {
                return;
            }
            TITAN component2 = gameObject.GetComponent<TITAN>();
            if (component2 != null)
            {
                string key = (string)FengGameManagerMKII.RCVariableNames["OnTitanLeaveRegion[" + myName + "]"];
                if (FengGameManagerMKII.TitanVariables.ContainsKey(key))
                {
                    FengGameManagerMKII.TitanVariables[key] = component2;
                }
                else
                {
                    FengGameManagerMKII.TitanVariables.Add(key, component2);
                }
                titanEventExit.CheckEvent();
            }
        }
    }
}
