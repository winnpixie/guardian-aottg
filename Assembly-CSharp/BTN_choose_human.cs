using ExitGames.Client.Photon;
using UnityEngine;

public class BTN_choose_human : MonoBehaviour
{
    private FengGameManagerMKII fgmkii;

    public void Start()
    {
        fgmkii = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
    }

    public bool AreAllPlayersDead()
    {
        foreach (PhotonPlayer photonPlayer in PhotonNetwork.playerList)
        {
            if (GExtensions.AsInt(photonPlayer.customProperties[PhotonPlayerProperty.IsTitan]) == 1)
            {
                if (!GExtensions.AsBool(photonPlayer.customProperties[PhotonPlayerProperty.Dead]))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void OnClick()
    {
        string selection = GameObject.Find("PopupListCharacterHUMAN").GetComponent<UIPopupList>().selection;
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
        fgmkii.needChooseSide = false;
        if (FengGameManagerMKII.Level.Mode == GameMode.PVP_CAPTURE)
        {
            fgmkii.checkpoint = GameObject.Find("PVPchkPtH");
        }
        if (!PhotonNetwork.isMasterClient && fgmkii.roundTime > 60f)
        {
            if (!AreAllPlayersDead())
            {
                fgmkii.NOTSpawnPlayer(selection);
            }
            else
            {
                fgmkii.NOTSpawnPlayer(selection);
                fgmkii.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
            }
        }
        else if (FengGameManagerMKII.Level.Mode == GameMode.BOSS_FIGHT_CT || FengGameManagerMKII.Level.Mode == GameMode.TROST || FengGameManagerMKII.Level.Mode == GameMode.PVP_CAPTURE)
        {
            if (AreAllPlayersDead())
            {
                fgmkii.NOTSpawnPlayer(selection);
                fgmkii.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
            }
            else
            {
                fgmkii.SpawnPlayer(selection);
            }
        }
        else
        {
            fgmkii.SpawnPlayer(selection);
        }
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
        NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
        IN_GAME_MAIN_CAMERA.UsingTitan = false;
        GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().setHUDposition();
        PhotonNetwork.player.SetCustomProperties(new Hashtable
        {
            { PhotonPlayerProperty.Character, selection }
        });
    }
}
