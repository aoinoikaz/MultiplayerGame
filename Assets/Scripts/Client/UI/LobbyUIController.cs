using UnityEngine;
using UnityEngine.UI;

public class LobbyUIController : MonoBehaviour
{
    LobbyUIScrollController scrollController;

    int currentPanelId;
    int previousPanelId;

    Text debug { get; set; }
    
    void Start()
    {
        scrollController = FindObjectOfType<LobbyUIScrollController>();

        scrollController.Panels[0].transform.Find("LoggedInAsLabel").GetComponent<Text>().text = AuthenticationManager.Instance != null ? AuthenticationManager.Instance.AuthenticatedUsername.ToUpper() : "Disconnected".ToUpper();

        scrollController.Panels[0].transform.Find("PlayersOnline").GetComponent<Text>().text 
            = "PLAYERS: " + PhotonNetwork.countOfPlayers;

        debug = scrollController.Panels[0].transform.Find("DebugLabel").GetComponent<Text>();
    }


    void Update()
    {
        debug.text = "Width: " + Screen.width + " | Height: " + Screen.height;
    }


    void RefreshPanels(int inx)
    {
        switch(inx)
        {
            case 0:
                //scrollController.Panels[inx].transform.Find("LoggedInAsLabel");
                break;
            case 1:
                break;
        }
    }
}
