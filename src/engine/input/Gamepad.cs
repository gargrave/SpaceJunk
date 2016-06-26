using UnityEngine;

namespace gkh
{
    /* the three different states of gamepad control */
    public enum GamepadState { Disabled, Generic, Xbox }

    public class Gamepad : MonoBehaviour 
    {
        #region xbox controller definitions
        /*************************************************************************
         * bindings for xbox controllers on Win, using the default MS drivers
         **************************************************************************/
        public static readonly KeyCode XboxPC_A = KeyCode.JoystickButton0;
        public static readonly KeyCode XboxPC_B = KeyCode.JoystickButton1;
        public static readonly KeyCode XboxPC_X = KeyCode.JoystickButton2;
        public static readonly KeyCode XboxPC_Y = KeyCode.JoystickButton3;


        public static readonly KeyCode XboxPC_Start = KeyCode.JoystickButton7;
        public static readonly KeyCode XboxPC_Select = KeyCode.JoystickButton6;
        public static readonly KeyCode XboxPC_BumperL = KeyCode.JoystickButton4;
        public static readonly KeyCode XboxPC_BumperR = KeyCode.JoystickButton5;


        /*************************************************************************
         * bindings for xbox controllers on Mac, using the third-party drivers
         **************************************************************************/
        public static readonly KeyCode XboxMAC_Up = KeyCode.JoystickButton5;
        public static readonly KeyCode XboxMAC_Down = KeyCode.JoystickButton6;
        public static readonly KeyCode XboxMAC_Left = KeyCode.JoystickButton7;
        public static readonly KeyCode XboxMAC_Right = KeyCode.JoystickButton8;

        public static readonly KeyCode XboxMAC_A = KeyCode.JoystickButton16;
        public static readonly KeyCode XboxMAC_B = KeyCode.JoystickButton17;
        public static readonly KeyCode XboxMAC_X = KeyCode.JoystickButton18;
        public static readonly KeyCode XboxMAC_Y = KeyCode.JoystickButton19;

        public static readonly KeyCode XboxMAC_Start = KeyCode.JoystickButton9;
        public static readonly KeyCode XboxMAC_Select = KeyCode.JoystickButton10;
        public static readonly KeyCode XboxMAC_BumperL = KeyCode.JoystickButton13;
        public static readonly KeyCode XboxMAC_BumperR = KeyCode.JoystickButton14;
        #endregion


        #region constants
        /* the prefix that will be returned with all gamepad controls */
        private const string Prefix = "joystick button ";
        /* Unity supports up to 20 buttons per gamepad */
        private const int MaxButtons = 20;
        #endregion


        public static KeyCode GetJoystickButton()
        {
            for (int i = 0; i < MaxButtons; i++)
            {
                if (Input.GetKey(Prefix + i))
                {
                    if (i == 0) return KeyCode.JoystickButton0;
                    if (i == 1) return KeyCode.JoystickButton1;
                    if (i == 2) return KeyCode.JoystickButton2;
                    if (i == 3) return KeyCode.JoystickButton3;
                    if (i == 4) return KeyCode.JoystickButton4;
                    if (i == 5) return KeyCode.JoystickButton5;
                    if (i == 6) return KeyCode.JoystickButton6;
                    if (i == 7) return KeyCode.JoystickButton7;
                    if (i == 8) return KeyCode.JoystickButton8;
                    if (i == 9) return KeyCode.JoystickButton9;
                    if (i == 10) return KeyCode.JoystickButton10;
                    if (i == 11) return KeyCode.JoystickButton11;
                    if (i == 12) return KeyCode.JoystickButton12;
                    if (i == 13) return KeyCode.JoystickButton13;
                    if (i == 14) return KeyCode.JoystickButton14;
                    if (i == 15) return KeyCode.JoystickButton15;
                    if (i == 16) return KeyCode.JoystickButton16;
                    if (i == 17) return KeyCode.JoystickButton17;
                    if (i == 18) return KeyCode.JoystickButton18;
                    if (i == 19) return KeyCode.JoystickButton19;
                }
            }
            return KeyCode.None;
        }

        public static string GetXboxString(KeyCode key)
        {
            OS os = Settings.CurrentOS;
            /* pre-map for the windows xbox controller*/
            if (os == OS.Win)
            {
                if (key == XboxPC_A) return "A";
                if (key == XboxPC_B) return "B";
                if (key == XboxPC_X) return "X";
                if (key == XboxPC_Y) return "Y";
                
                if (key == XboxPC_Start) return "Start";
                if (key == XboxPC_Select) return "Back";
                if (key == XboxPC_BumperL) return "L Bumper";
                if (key == XboxPC_BumperR) return "R Bumper";
            }
            else if (os == OS.Mac)
            {
                if (key == XboxMAC_A) return "A";
                if (key == XboxMAC_B) return "B";
                if (key == XboxMAC_X) return "X";
                if ( key == XboxMAC_Y) return "Y";
                
                if (key == XboxMAC_Start) return "Start";
                if (key == XboxMAC_Select) return "Back";
                if (key == XboxMAC_BumperL) return "L Bumper";
                if (key == XboxMAC_BumperR) return "R Bumper";
                
                if (key == XboxMAC_Up) return "D-Pad Up";
                if (key == XboxMAC_Down) return "D-Pad Down";
                if (key == XboxMAC_Left) return "D-Pad Left";
                if (key == XboxMAC_Right) return "D-Pad Right";
            }
            return "unknown";
        }
    }
}