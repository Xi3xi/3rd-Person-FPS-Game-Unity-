using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    private Image playerMapIcon; // Player Map
    private Image[] enemyMapIcons; // Enemy Map
    private Image[] survivorMapIcons; // Item Map

    private GameObject[] enemies; // Enemies
    private GameObject[] survivors; // Items

    private GameObject player; // Player

    // Start is called before the first frame update
    void Awake()
    {
        // Get Player
        player = GameObject.FindWithTag("Player");
        //playerMapIcon = GameObject.FindWithTag("PlayerMapIcon").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // set the player map icon position relative to the position of player on the map and direction
        //playerMapIcon.transform.position = new Vector3(player.transform.position.x + 725, player.transform.position.z + 400, 0);
        //playerMapIcon.transform.rotation = Quaternion.Euler(0, 0, -player.transform.eulerAngles.y);

    }
}
