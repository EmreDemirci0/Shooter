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
	public Button joinWatchButton;
	public void SetInfo(Room room)
	{
		roomidText.text = room.roomID;
		int count = room.players.FindAll(p => p.isCam == false).Count;
		countText.text = count + "/" + 4;
		if (count >= 4)
		{
			joinButton.interactable = false;
		}
		joinButton.onClick.AddListener(() =>
		{
			SocketManager.Instance.socket.Emit("GoRoom", room.roomID);
		});
		joinWatchButton.onClick.AddListener(() =>
		{
			SocketManager.Instance.socket.Emit("GoWatchRoom", room.roomID);
		});
	}
}
