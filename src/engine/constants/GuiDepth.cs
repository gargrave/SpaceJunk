namespace gkh
{
    public static class GuiDepth 
    {
        private static int counter = 0;

        // pre-defined values for GUI-depth settings
        public static readonly int AbsoluteTop = ++counter;
        public static readonly int FadeScreen = ++counter;
        public static readonly int TextMenuTop = ++counter;
        public static readonly int TextMenuBottom = ++counter;
        public static readonly int HUD = ++counter;
        public static readonly int TextDrift = ++counter;
    }
}