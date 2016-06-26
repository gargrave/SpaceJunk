using System.Collections.Generic;

namespace gkh
{
    public static class TutorialData
    {
        public static List<string> GetTutStrings(int index)
        {
            /********************************************************
             * tutorials for moving and catching pieces
             ********************************************************/
            if (index == 0)
            {
                var list = new List<string>();
                list.Add("Alright, listen up, greenhorn!");
                list.Add("Earth's junk is floating around all over the place,");
                list.Add("and somebody has to clean it all up.");
                list.Add("That's where you come in.");
                list.Add("");
                list.Add("Steer your ship with WASD or Arrow keys, ");
                list.Add("and collide with the junk to collect it.");
                list.Add("");
                list.Add("You can hold up to 6 at a time,");
                list.Add("but the more you carry, the slower you move.");
                list.Add("In other words: don't overdo it, hotshot!");
                return list;
            }

            /********************************************************
             * tutorials for shooting pieces
             ********************************************************/
            if (index == 1)
            {
                var list = new List<string>();
                list.Add("Great, look at you, you caught your first piece of junk!");
                list.Add("You've got employee of the month written all over you.");
                list.Add("");
                list.Add("Use your mouse to aim at the pieces in the lower left,");
                list.Add("and press left-click or spacebar to shoot it.");
                list.Add("");
                list.Add("Match 3 or more of the same type to clear them.");
                list.Add("Try to clear them all so we can send more your way.");
                list.Add("");
                list.Add("(Set up combos for bonus points and time extensions!)");
                return list;
            }

            /********************************************************
             * tutorials for clearing grids
             ********************************************************/
            if (index == 2)
            {
                var list = new List<string>();
                list.Add("Nice work newbie, you cleared your first pile!");
                list.Add("Maybe you won't be so bad after all. (I said MAYBE.)");
                list.Add("");
                list.Add("Every time you clear a pile, a bigger one will take its place,");
                list.Add("and you'll get more time to work.");
                list.Add("");
                list.Add("That's about all you need to know,");
                list.Add("so you won't hear from me again unless you ask.");
                list.Add("Good luck out there, rookie. Don't screw this up!");
                return list;
            }
            return new List<string>();
        }
    }
}