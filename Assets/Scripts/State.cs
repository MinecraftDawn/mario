using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State {
public interface BaseState
{
    public BaseState Update(GameObject actor);
    public BaseState FixedUpdate(GameObject actor);
}

// TODO: Maybe you will need a Movable state to prevent some redundant code.

public class OnLandState : BaseState
{
    public BaseState Update(GameObject actor)
    {
        // TODO: need to fill in
        return this;
    }

    public BaseState FixedUpdate(GameObject actor)
    {
        // TODO: need to fill in
        return this;
    }
}

public class JumpState : BaseState
{
    public BaseState Update(GameObject actor)
    {
        // TODO: need to fill in
        return this;
    }

    public BaseState FixedUpdate(GameObject actor)
    {
        // TODO: need to fill in
        return this;
    }
}

}
