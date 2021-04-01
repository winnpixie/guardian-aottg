using UnityEngine;

public class BTN_LEADERBOARD_QUIT : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject leaderboard;

    private void OnClick()
    {
        NGUITools.SetActive(mainMenu, state: true);
        NGUITools.SetActive(leaderboard, state: false);
    }
}
