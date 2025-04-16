using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Newtonsoft.Json;
using System.Linq;
using Akila.FPSFramework;
using Akila.FPSFramework.Animation;

public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject camPlayer;
    public Transform spawnPos;

    public List<PlayerController> PlaConts;


    private void OnEnable()
    {
        string js = JsonUtility.ToJson(SocketManager.Instance.room);
        SocketManager.Instance.socket.Emit("CreatedRoom", js);
        SocketManager.Instance.socket.OnUnityThread("SetPlayerMove", data =>
            {
                var pla = JsonConvert.DeserializeObject<List<Player>>(data.ToString())[0];
                var den = PlaConts.FirstOrDefault(x => x.player.userID == pla.userID);
                if (den == null || den.gameObject == null)
                {
                    return; // Eğer null veya silinmişse işlemi durdur
                }
                if (den.player.userID != SocketManager.Instance.player.userID)
                {
                    den.gameObject.transform.position = pla.pos;
                }
                den.GetComponent<FirstPersonController>().SetRotaitonSoceket(pla.rotate.x, pla.rotate.y);

            });
        SocketManager.Instance.socket.OnUnityThread("SetFire", data =>
        {
            var fire = JsonConvert.DeserializeObject<List<Fire>>(data.ToString())[0];
            var pla = PlaConts.FirstOrDefault(x => x.player.userID == fire.userID);
            pla.inv.GetComponentInChildren<Firearm>().Fire(fire);
        });
        SocketManager.Instance.socket.OnUnityThread("SetAnim", data =>
        {
            var fire = JsonConvert.DeserializeObject<List<Player>>(data.ToString())[0];
            var pla = PlaConts.FirstOrDefault(x => x.player.userID == fire.userID);
            pla.inv.items[pla.inv.currentItemIndex].GetComponentInChildren<ProceduralAnimator>().Play(fire.animType);
        });
        SocketManager.Instance.socket.OnUnityThread("SetReload", data =>
        {
            var fire = JsonConvert.DeserializeObject<List<string>>(data.ToString())[0];
            var pla = PlaConts.FirstOrDefault(x => x.player.userID == fire);
            pla.inv.GetComponentInChildren<Firearm>().Reload();
        });
        SocketManager.Instance.socket.OnUnityThread("RemovePlayer", data =>
        {
            var fire = JsonConvert.DeserializeObject<List<string>>(data.ToString())[0];
            var pla = PlaConts.FirstOrDefault(x => x.player.userID == fire);
            PlaConts.Remove(pla);
            Destroy(pla.gameObject);
        });

    }
    void Start()
    {
        UIManagerNew.Instance.SetPlaInfo(SocketManager.Instance.player);

        foreach (var item in SocketManager.Instance.room.players)
        {
            AddPlayer(item);
        }

        SocketManager.Instance.socket.OnUnityThread("JoinRooms", rooms =>
        {
            var room = JsonConvert.DeserializeObject<List<Room>>(rooms.ToString())[0];
            Room oldRoom = SocketManager.Instance.room;
            SocketManager.Instance.room = room;
            Room newRoom = room;
            var joinedPlayers = newRoom.players
                .Where(np => !oldRoom.players.Any(op => op.userID == np.userID))
                .ToList();
            if (joinedPlayers.Count() > 0)
            {
                foreach (var item in joinedPlayers)
                {
                    AddPlayer(item);
                }
            }




        });

    }

    public void AddPlayer(Player item)
    {
        if (item.userID == SocketManager.Instance.player.userID && item.isCam)
        {
            GameObject cam = Instantiate(camPlayer, spawnPos.position, quaternion.identity);
            cam.GetComponent<CameraPlayer>().player = item;
            return;
        }
        if (item.isCam)
            return;
        GameObject pla = Instantiate(player, spawnPos.position, quaternion.identity);
        pla.GetComponent<PlayerController>().player = item;

        if (item.userID == SocketManager.Instance.player.userID)
        {
            pla.GetComponent<PlayerController>().canMove = true;
            pla.GetComponentInChildren<CameraManager>().mainCamera.enabled = true;
            pla.GetComponentInChildren<CameraManager>().mainCamera.GetComponent<AudioListener>().enabled = true;
            pla.GetComponentInChildren<CameraManager>().overlayCamera.enabled = true;
        }
        pla.name = item.PlaName;

        pla.GetComponent<PlayerController>().SetControl();
        PlaConts.Add(pla.GetComponent<PlayerController>());
    }
}
