using State;

namespace Monster
{

public class MagicFort : Monster
{
    protected override void InitialState() { _stateManager.Init<FortActiveState>(); }
}

}