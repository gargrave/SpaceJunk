using UnityEngine;
using System.Collections.Generic;

namespace gkh
{
    public class InputWrapper : MonoBehaviour 
    {
        #region fields & properties
        /* whether static members have been initialized */
        public static bool Initialized { get; private set; }
        /* the current state of gamepad control */
        public static GamepadState GamepadState { get; private set; }
        /* a simple accessor for checking if gamepad is enabled */
        public static bool GamepadEnabled 
        { get { return GamepadState != GamepadState.Disabled; } }

        
        /* whether we are currently waiting to capture the next key/button press */
        public static bool IsCapturingKey { get; private set; }
        /* the last-capture key/button press */
        public static KeyCode LastCapturedKey { get; private set; }

        /* whether the keys are currently locked; we use this to prevent a key from being captured
         * instantly when the player enters the key-capture state */
        private static bool keysLocked = false;
        /* whether the keys are ready to be unlocked at the start of the next frame */
        private static bool unlockKeysAtNextFrame = false;
        /* whether the current key-capture status is ready to end; we track this flag to avoid
         * capturing multiple keys, since Unity's OnGUI method is called multipled times per frame */
        private static bool stopCapturingAtNextFrame = false;
        /* whether the player is actively using a gamepad for control; we will use this to control
         * on-screen control prompts */
        private static bool isUsingGamepad = false;
        /* whehter our axes are currently locked (applies only to single-press axis commands, like menus) */
        private static bool axisLockX = false, axisLockY = false;
        
        /* dictionaries for all mapped controls */
        private static Dictionary<Control, KeyCode> keyMap = new Dictionary<Control, KeyCode>();
        private static Dictionary<Control, KeyCode> keyMapAlt = new Dictionary<Control, KeyCode>();
        private static Dictionary<Control, KeyCode> joyMap = new Dictionary<Control, KeyCode>();
        private static Dictionary<Control, string> joyAxisMap = new Dictionary<Control, string>();
        private static Dictionary<Control, string> joyAxisMapAlt = new Dictionary<Control, string>();
        #endregion
        
        
        #region MonoBehaviour
        void Start()
        {
            if (!Initialized)
            {
                Initialized = true;
                GamepadState = Globals.InitialGamepadState;
                PremapKeyboard();
                /* this will throw errors until you set up the inputs */
                //PremapGamepad();
            }
        }

        void PremapKeyboard()
        {
            keyMap.Add(Control.MoveLeft, KeyCode.A);
            keyMap.Add(Control.MoveRight, KeyCode.D);
            keyMap.Add(Control.MoveDown, KeyCode.S);
            keyMap.Add(Control.MoveUp, KeyCode.W);
            keyMap.Add(Control.Shoot, KeyCode.Space);
            keyMapAlt.Add(Control.MoveLeft, KeyCode.LeftArrow);
            keyMapAlt.Add(Control.MoveRight, KeyCode.RightArrow);
            keyMapAlt.Add(Control.MoveDown, KeyCode.DownArrow);
            keyMapAlt.Add(Control.MoveUp, KeyCode.UpArrow);
            
            keyMap.Add(Control.Pause, KeyCode.Escape);
            //keyMapAlt.Add(Control.Pause, KeyCode.Escape);
            keyMap.Add(Control.MenuBack, KeyCode.Escape);
            //keyMapAlt.Add(Control.MenuBack, KeyCode.Escape);
            keyMap.Add(Control.MenuSelect, KeyCode.Return);
            keyMapAlt.Add(Control.MenuSelect, KeyCode.Space);
            
            if (Settings.IsDevBuild)
                keyMap.Add(Control.DevOptions, KeyCode.Tab);
        }

        void PremapGamepad()
        {
            OS os = Settings.CurrentOS;
            /* pre-map for the windows xbox controller*/
            if (os == OS.Win)
            {
                if (Settings.IsDevBuild)
                    Debug.Log ("Pre-mapping gamepad for Windows\n");

                joyMap.Add(Control.Pause, Gamepad.XboxPC_Start);
                joyMap.Add(Control.MenuBack, Gamepad.XboxPC_B);
                joyAxisMapAlt.Add(Control.MoveLeft, "XboxPcDpadX");
                joyAxisMapAlt.Add(Control.MoveRight, "XboxPcDpadX");
                joyAxisMapAlt.Add(Control.MoveUp, "XboxPcDpadY");
                joyAxisMapAlt.Add(Control.MoveDown, "XboxPcDpadY");
            }
            /* pre-map for the Mac Xbox controller */
            else if (os == OS.Mac)
            {
                if (Settings.IsDevBuild)
                    Debug.Log ("Pre-mapping gamepad for Mac\n");

                joyMap.Add(Control.MoveUp, Gamepad.XboxMAC_Up);
                joyMap.Add(Control.MoveRight, Gamepad.XboxMAC_Right);
                joyMap.Add(Control.MoveDown, Gamepad.XboxMAC_Down);
                joyMap.Add(Control.MoveLeft, Gamepad.XboxMAC_Left);

                joyMap.Add(Control.Pause, Gamepad.XboxMAC_Start);
                joyMap.Add(Control.MenuBack, Gamepad.XboxMAC_B);
            }
        }
        
        void Update()
        {
            if (stopCapturingAtNextFrame)
            {
                stopCapturingAtNextFrame = false;
                IsCapturingKey = false;
            }
            
            if (unlockKeysAtNextFrame)
            {
                unlockKeysAtNextFrame = false;
                keysLocked = false;
            }
        }
        
        void OnGUI()
        {
            if (IsCapturingKey)
            {
                /* if keys are currently locked, check if all keys have been released;
                 * if so, unlock the controls */
                if (keysLocked)
                {
                    if (!Input.anyKey) 
                        unlockKeysAtNextFrame = true;
                    return;
                }

                /* since Unity's GUI events do not capture joystick hits, we need to check for one
                 * of those first; if we do not have a joystick key press, process key events as usual */
                if (GamepadEnabled)
                {
                    KeyCode joystickKey = Gamepad.GetJoystickButton();
                    if (joystickKey != KeyCode.None)
                    {
                        LastCapturedKey = joystickKey;
                        return;
                    }
                }
                
                /* capture the next-pressed key and store its value */
                Event e = Event.current;
                if (e.type != EventType.KeyUp &&
                    e.type == EventType.KeyDown)
                {
                    if (e.keyCode != KeyCode.None)
                        LastCapturedKey = e.keyCode;
                }
            }
        }
        #endregion
        
        
        #region control string methods
        public static string GetFullControlString(Control action)
        {
            if (!keyMap.ContainsKey(action))
            {
                Debug.LogWarning("GetControlString(): action is not in keyMap: " + action);
                return string.Empty;
            }
            
            string altKeySeparator = "  |  ";
            string s = keyMap[action].ToString();
            
            /* if there is an alternate control, append that string onto the existing one */
            if (keyMapAlt.ContainsKey(action) &&
                keyMapAlt[action] != keyMap[action])
            {
                /* if the first command is un-mapped, simply make the full string this control */
                if (s == "None") s = keyMapAlt[action].ToString();
                /* otherwise, combine the two string */
                else s = string.Format("{0}{1}{2}", s, altKeySeparator, keyMapAlt[action].ToString());
            }
            
            /* if there is a joystick control, append that string onto the existing one */
            if (GamepadEnabled)
            {
                if (joyMap.ContainsKey(action))
                {
                    string joyString = string.Empty;
                    if (GamepadState == GamepadState.Xbox)
                        joyString = Gamepad.GetXboxString(joyMap[action]);
                    else joyString = joyMap[action].ToString().Replace("JoystickButton", "Gamepad ");
                    s = string.Format("{0}{1}{2}", s, altKeySeparator, joyString);
                }
            }
            
            /* add a space before the word "Arrow" as Unity mashes the words together */
            s = s.Replace("Arrow", "");
            s = s.Replace("Escape", "Esc");
            if (s == "None")
                return "???";
            return s;
        }

        public static string GetCleanControlString(Control action)
        {
            if (!keyMap.ContainsKey(action))
            {
                Debug.LogWarning("GetControlString(): action is not in keyMap: " + action);
                return string.Empty;
            }

            string s = string.Empty;
            if (!isUsingGamepad)
            {
                string altKeySeparator = " or ";
                s = keyMap[action].ToString();
                
                /* if there is an alternate control, append that string onto the existing one */
                if (keyMapAlt.ContainsKey(action) &&
                    keyMapAlt[action] != keyMap[action])
                {
                    /* if the first command is un-mapped, simply make the full string this control */
                    if (s == "None") s = keyMapAlt[action].ToString();
                    /* otherwise, combine the two string */
                    else s = string.Format("{0}{1}{2}", s, altKeySeparator, keyMapAlt[action].ToString());
                }
                /* add a space before the word "Arrow" as Unity mashes the words together */
                s = s.Replace("Arrow", "");
                s = s.Replace("Escape", "Esc");
            }
            
            /* if there is a joystick control, append that string onto the existing one */
            else
            {
                if (joyMap.ContainsKey(action))
                {
                    if (GamepadState == GamepadState.Xbox)
                    {
                        s = Gamepad.GetXboxString(joyMap[action]);
                    }
                    else s = joyMap[action].ToString().Replace("JoystickButton", "Gamepad ");
                }
            }
            return s;
        }
        #endregion
        
        
        #region control mgmt
        public static void UpdateControl(Control action, KeyCode newButton)
        {
            /* re-mapping of the ESC key is not allowed! we need at least one default key */
            if (newButton == KeyCode.Escape) return;
            
            /* check if this is a normal key, or a joystick button */
            Dictionary<Control, KeyCode> controlsToUpdate = 
                (newButton.ToString().StartsWith("Joy")) ? joyMap : keyMap;

            if (!controlsToUpdate.ContainsKey(action))
            {
                Debug.LogWarning("UpdateControl(): action is not mapped: " + action);
                return;
            }
            
            /* check if we have this key mapped to another action; if we do, 
             * store the relevent dictionary key so we can update it */
            Control previousAction = Control.Undefined;
            if (controlsToUpdate.ContainsValue(newButton))
            {
                foreach(KeyValuePair<Control, KeyCode> entry in controlsToUpdate)
                {
                    if (entry.Value == newButton)
                    {
                        previousAction = entry.Key;
                        break;
                    }
                }
            }
            
            /* if the key was found mapped to an existing action, 
             * take the other action's key before updating it */
            if (previousAction != Control.Undefined)
            {
                if (controlsToUpdate[action] != KeyCode.Escape)
                    controlsToUpdate[previousAction] = controlsToUpdate[action];
                else controlsToUpdate[previousAction] = KeyCode.None;
            }
            /* now update the requested entry with the supplied key/button */
            controlsToUpdate[action] = newButton;
        }
        #endregion
        
        
        #region control-checking methods
        public static bool Pressed(Control action)
        {
            if (IsCapturingKey) return false;
            
            /* check the standard keys */
            if (keyMap.ContainsKey(action) &&
                Input.GetKeyDown(keyMap[action]))
            {
                isUsingGamepad = false;
                return true;
            }
            
            /* check the alternate keys */
            if (keyMapAlt.ContainsKey(action) &&
                Input.GetKeyDown(keyMapAlt[action]))
            {
                isUsingGamepad = false;
                return true;
            }
            
            /* check the gamepad */
            if (GamepadEnabled)
            {
                bool found = false;
                /* check the gamepad's axes for move commands */
                if (action.ToString().StartsWith("Move"))
                    found = CheckAxis(action, true);

                if (!found &&
                    joyMap.ContainsKey(action) &&
                    Input.GetKeyDown(joyMap[action]))
                    found = true;

                if (found)
                {
                    isUsingGamepad = true;
                    return true;
                }
            }
            return false;
        }
        
        public static bool Held(Control action)
        {
            if (IsCapturingKey) return false;

            /* check the standard keys */
            if (keyMap.ContainsKey(action) &&
                Input.GetKey(keyMap[action]))
            {
                isUsingGamepad = false;
                return true;
            }
            
            /* check the alternate keys */
            if (keyMapAlt.ContainsKey(action) &&
                Input.GetKey(keyMapAlt[action]))
            {
                isUsingGamepad = false;
                return true;
            }
            
            /* check the gamepad */
            if (GamepadEnabled)
            {

                bool found = false;
                if (action.ToString().StartsWith("Move"))
                    found = CheckAxis(action, false);
                
                if (!found &&
                    joyMap.ContainsKey(action) &&
                    Input.GetKey(joyMap[action]))
                    found = true;
                
                if (found)
                {
                    isUsingGamepad = true;
                    return true;
                }
            }
            return false;
        }

        private static bool CheckAxis(Control action, bool checkForLock)
        {
            bool found = false;
            if (joyAxisMap.ContainsKey(action) || joyAxisMapAlt.ContainsKey(action))
            {
                /* store the current values of the requested axis from both the normal and "alt" axis lists */
                float axisA = (joyAxisMap.ContainsKey(action)) ? Input.GetAxis(joyAxisMap[action]) : 0f;
                float axisB = (joyAxisMapAlt.ContainsKey(action)) ? Input.GetAxis(joyAxisMapAlt[action]) : 0f;

                /*****************************************************
                 * check vertical axes
                 *****************************************************/
                if (action == Control.MoveDown || action == Control.MoveUp)
                {
                    if (action == Control.MoveDown && 
                        (Mathf.Approximately(axisA, 1f) || Mathf.Approximately(axisB, -1f)))
                    {
                        if (!checkForLock) found = true;
                        else if (checkForLock && !axisLockY) found = true;
                        axisLockY = true;
                    }
                    else if (action == Control.MoveUp && 
                             (Mathf.Approximately(axisA, -1f) || Mathf.Approximately(axisB, 1f)))
                    {
                        if (!checkForLock) found = true;
                        else if (checkForLock && !axisLockY) found = true;
                        axisLockY = true;
                    }
                    /* if none of the mapped axes are pressed, unlock the vertical axis */
                    else if (Mathf.Approximately(axisA, 0) && 
                             Mathf.Approximately(axisB, 0)) axisLockY = false;
                }

                /*****************************************************
                 * check horizontal axes
                 *****************************************************/
                else if (action == Control.MoveLeft || action == Control.MoveRight)
                {
                    if (action == Control.MoveLeft && 
                        (Mathf.Approximately(axisA, -1f) || Mathf.Approximately(axisB, -1f)))
                    {
                        if (!checkForLock) found = true;
                        else if (checkForLock && !axisLockX) found = true;
                        axisLockX = true;
                    }
                    else if (action == Control.MoveRight && 
                             (Mathf.Approximately(axisA, 1f) || Mathf.Approximately(axisB, 1f)))
                    {
                        if (!checkForLock) found = true;
                        else if (checkForLock && !axisLockX) found = true;
                        axisLockX = true;
                    }
                    /* if none of the mapped axes are pressed, unlock the horizontal axis */
                    else if (Mathf.Approximately(axisA, 0) && 
                             Mathf.Approximately(axisB, 0)) axisLockX = false;
                }
            }
            return found;
        }
        
        public static void EnableCaptureNextKey()
        {
            keysLocked = true;
            unlockKeysAtNextFrame = false;
            IsCapturingKey = true;
            stopCapturingAtNextFrame = false;
            LastCapturedKey = KeyCode.None;
        }
        
        public static void DisableCaptureNextKey()
        {
            keysLocked = false;
            stopCapturingAtNextFrame = true;
            LastCapturedKey = KeyCode.None;
        }
        #endregion


        #region gamepad mgmt
        public static void CycleGamepadState(int dir)
        {
            GamepadState next = GamepadState.Disabled;

            if (Settings.IsWinOrMac)
            {
                if (GamepadState == GamepadState.Disabled)
                    next = (dir == 1) ? GamepadState.Xbox : GamepadState.Generic;
                else if (GamepadState == GamepadState.Xbox)
                    next = (dir == 1) ? GamepadState.Generic : GamepadState.Disabled;
                else next = (dir == 1) ? GamepadState.Disabled : GamepadState.Xbox;
            }
            /* for linux, simply toggle between disabled & generic; no xbox controller support */
//            else
//            {
//                if (GamepadState == GamepadState.Disabled) next = GamepadState.Generic;
//                else if (GamepadState == GamepadState.Generic) next = GamepadState.Disabled;
//            }
            GamepadState = next;
        }

        public static string GetGamepadStateString()
        {
            if (GamepadState == GamepadState.Xbox) return "Xbox";
            if (GamepadState == GamepadState.Generic) return "Generic";
            return "Disabled";
        }
        #endregion
    }
}