using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AiControl : MonoBehaviour

{
    protected GameObject[] enemies;
    

    // Start is called before the first frame update
    void Start()
    {

    }

        


    // Update is called once per frame
    void Update()
    {
        
    }

    public void AiUpdateMethod(Vector2[] playerPositions, Vector2 playerVelocity)
    {
        
        //Find all enemies
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Pass Player Info to AI Update method
        foreach (GameObject enemy in enemies)
        {
            var AiUpdate = enemy.GetComponent<EnemyAiAbstract>();
            AiUpdate.AiUpdate(playerPositions, playerVelocity);
        }
        
    }
}
