using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WalkingState : IPlayerState
{
    public IPlayerState DoState(PlayerController player)
    {
        // state values
        player.playerstate = "Walking";
        player.setAnimState(1);

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
        else if (!player.isGrounded() && player.getCoyoteTime() <= 0f)
            return player.fallingstate;
        else if (math.abs(player.velocity.x) > 0f)
            return player.walkingstate;
        else
            return player.idlestate;
    }
}
