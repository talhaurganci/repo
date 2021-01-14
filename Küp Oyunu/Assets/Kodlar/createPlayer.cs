using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createPlayer : MonoBehaviour {

    public GameObject playerPrefab;

	
    public void Create() //bu kod roll kodundan cagırılıyor, obje düşün 
    {
        Invoke("CreatePrefab", 3f); //süreyi ayarlayabilirsin 

    }
    public void CreatePrefab()
    {      
      Instantiate(playerPrefab,transform.position, Quaternion.identity); // instantiate yeni obje olusturma metodu ilk prefab, 2.pozisyon, 3.durus acısı 
    }
}
