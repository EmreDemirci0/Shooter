using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;
using System.Linq;
using Akila.FPSFramework;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public Transform spawnPos;
    void Start()
    {
        UIManagerNew.Instance.SetPlaInfo(SocketManager.Instance.player);

        foreach (var item in SocketManager.Instance.room.players)
        {
            GameObject pla = Instantiate(player, spawnPos.position, quaternion.identity);
            pla.GetComponent<PlayerController>().player = item;
            if(item.userID==SocketManager.Instance.player.userID)
            {
                 pla.GetComponent<PlayerController>().canMove = true;
            }
             pla.GetComponent<PlayerController>().SetControl();
            
        }

        SocketManager.Instance.socket.OnUnityThread("JoinRooms", rooms =>
        {
            var room = JsonConvert.DeserializeObject<List<Room>>(rooms.ToString())[0];
            Room oldRoom = SocketManager.Instance.room;
            SocketManager.Instance.room = room;
            Room newRoom = room;
            var leftPlayers = oldRoom.players
    .Where(op => !newRoom.players.Any(np => np.userID == op.userID))
    .ToList();

            var joinedPlayers = newRoom.players
                .Where(np => !oldRoom.players.Any(op => op.userID == np.userID))
                .ToList();
            if (joinedPlayers.Count() > 0)
            {
                foreach (var item in joinedPlayers)
                {
                    GameObject pla = Instantiate(player, spawnPos.position, quaternion.identity);
                }
            }

            if(leftPlayers.Count() > 0)
            {

            }


        });

    }
}
