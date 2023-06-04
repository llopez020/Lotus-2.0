using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingState : IPlayerState
{
    public IPlayerState DoState(PlayerController player)
    {
        // state values
        player.playerstate = "Falling";
        player.setAnimState(3);

        // state responsibilities
        player.doCoyoteTime();
        player.handleInput();
        player.doAnim();
        player.death();

        // change state
        if (Input.GetButtonDown("Jump"))
        {
            player.initJump();
            return player.jumpingstate;
        }
        else if (player.velocity.y == 0f || player.isGrounded())
            return player.idlestate;
        else
            return player.fallingstate;
    }
}
