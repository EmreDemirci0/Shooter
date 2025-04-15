using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomController : MonoBehaviour
{
	public TextMeshProUGUI roomidText; 
	public TextMeshProUGUI countText; 
	public Button joinButton;
	public void SetInfo(Room room)
	{
		roomidText.text = room.roomID;
		countText.text = room.players.Count + "/" + 4;
		if (room.players.Count>=4)
		{
			joinButton.interactable = false;
		}
		joinButton.onClick.AddListener(()=> {
			SocketManager.Instance.socket.Emit("GoRoom",room.roomID);
		});
	}
}
