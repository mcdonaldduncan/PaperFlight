using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeTags : MonoBehaviour
{
    [SerializeField] private GameObject cave;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject birds;
    [SerializeField] private GameObject lake;

    // Start is called before the first frame update
    void Start()
    {
        birds.gameObject.tag = "Birds";
        player.gameObject.tag = "Player";
        lake.gameObject.tag = "Lake";
        cave.gameObject.tag = "Cave";
        this.gameObject.SetActive(false);
    }
}
