using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum LadderPart { complete,top,bottom};
    [SerializeField] private LadderPart part = LadderPart.complete;
    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.GetComponent<playerMovement>())
        {
            playerMovement player = col.GetComponent<playerMovement>();
            switch (part)
            {
                case LadderPart.complete:
                    player.ladder = this;

                    player.CanClimb = true;
                    break; 
                case LadderPart.top:
                    player.topLad = true;
                    break;
                case LadderPart.bottom:
                    player.bottomLad = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<playerMovement>())
        {
            playerMovement player = col.GetComponent<playerMovement>();
            switch (part)
            {
                case LadderPart.complete:
                    player.CanClimb = false;
                    break;
                case LadderPart.top:
                    player.topLad = false;
                    break;
                case LadderPart.bottom:
                    player.bottomLad = false;
                    break;
                
                default:
                    break;
            }
        }
    }
}
