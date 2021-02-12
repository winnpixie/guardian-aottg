using ExitGames.Client.Photon;
using UnityEngine;

public class BTN_choose_titan : MonoBehaviour
{
    private FengGameManagerMKII fgmkii;

    private void Start()
    {
        if (!FengGameManagerMKII.Level.PlayerTitans)
        {
            base.gameObject.GetComponent<UIButton>().isEnabled = false;
        }
        else
        {
            fgmkii = GameObject.Find("MultiplayerManager").GetComponent<FengGameManagerMKII>();
        }
    }

    private void OnClick()
    {
        if (FengGameManagerMKII.Level.Mode == GameMode.PVP_AHSS)
        {
            string text = "AHSS";
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
            fgmkii.needChooseSide = false;
            if (!PhotonNetwork.isMasterClient && fgmkii.roundTime > 60f)
            {
                fgmkii.NOTSpawnPlayer(text);
                fgmkii.photonView.RPC("restartGameByClient", PhotonTargets.MasterClient);
            }
            else
            {
                fgmkii.SpawnPlayer(text, "playerRespawn2");
            }
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
            IN_GAME_MAIN_CAMERA.UsingTitan = false;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetHUDPosition();

            Hashtable hashtable = new Hashtable();
            hashtable.Add(PhotonPlayerProperty.Character, text);
            PhotonNetwork.player.SetCustomProperties(hashtable);
        }
        else
        {
            if (FengGameManagerMKII.Level.Mode == GameMode.PVP_CAPTURE)
            {
                fgmkii.checkpoint = GameObject.Find("PVPchkPtT");
            }
            string selection = GameObject.Find("PopupListCharacterTITAN").GetComponent<UIPopupList>().selection;
            NGUITools.SetActive(base.transform.parent.gameObject, state: false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[0], state: true);
            if ((!PhotonNetwork.isMasterClient && fgmkii.roundTime > 60f) || fgmkii.justSuicide)
            {
                fgmkii.justSuicide = false;
                fgmkii.NOTSpawnNonAITitan(selection);
            }
            else
            {
                fgmkii.SpawnNonAITitan2(selection);
            }
            fgmkii.needChooseSide = false;
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[1], state: false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[2], state: false);
            NGUITools.SetActive(GameObject.Find("UI_IN_GAME").GetComponent<UIReferArray>().panels[3], state: false);
            IN_GAME_MAIN_CAMERA.UsingTitan = true;
            GameObject.Find("MainCamera").GetComponent<IN_GAME_MAIN_CAMERA>().SetHUDPosition();
        }
    }
}
