public class Tornado : DisasterObject
{
    public override void DisasterLifeCycle()
    {
        StateLabel state = Manager.StateController.GetRegionalState(transform.position).StateLabel;
        if (state == StateLabel.POLLUTED)
        {
            Disasterlife -= 1;
        }
        else if (state == StateLabel.NORMAL)
        {
            Disasterlife -= 3;
        }
        else if (state == StateLabel.SAFE)
        {
            Disasterlife -= 5;
        }

        if (Disasterlife > 0)
        {
            Invoke(nameof(DisasterLifeCycle), 1f);
        } 
        else
        {
            // for the manager
        }
    }
}