namespace gkh
{
    /* all of the types of actions that can be linked to input */
    public enum Control
    { 
        Undefined, DevOptions, Pause, MenuSelect, MenuBack,
        MoveLeft, MoveRight, MoveDown, MoveUp, Shoot
    }

    public class InputStrings
    {
        public static string Get(Control action)
        {
            if (action == Control.Pause) return "Pause/Unpause";
            if (action == Control.MenuSelect) return "Confirm";
            if (action == Control.MenuBack) return "Back/Cancel";

            if (action == Control.MoveLeft) return "Left";
            if (action == Control.MoveRight) return "Right";
            if (action == Control.MoveDown) return "Down";
            if (action == Control.MoveUp) return "Up";
            if (action == Control.Shoot) return "Shoot";

            return "Undefined";
        }
    }
}