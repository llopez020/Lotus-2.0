using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : IPlayerState
{
    public IPlayerState DoState(PlayerController player)
    {
        // state values
        player.playerstate = "Jumping";
        player.setAnimState(2);

        // state responsibilities
        player.doCoyoteTime();
        player.handleJump();
        player.handleInput();
        player.doAnim();
        player.death();
 
        // change state
        if (player.velocity.y < 0f)
            return player.fallingstate;
        else
            return player.jumpingstate;
    }
}
