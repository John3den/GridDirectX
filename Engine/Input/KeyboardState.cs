using System.Collections.Generic;
using System.Windows.Forms;

namespace Engine
{
    public static class KeyboardState 
    {
        private static Dictionary<Keys, bool> keysPressed = new Dictionary<Keys, bool>()
        {
            [Keys.W] = false,
            [Keys.S] = false,
            [Keys.ShiftKey] = false,
            [Keys.Space] = false,
            [Keys.A] = false,
            [Keys.D] = false
        }; 

        public static void KeyUp(Keys key)
        {
            keysPressed[key] = false;
        }

        public static void KeyDown(Keys key)
        {
            keysPressed[key] = true;
        }

        public static bool IsPressed(Keys key)
        {
            return keysPressed[key];
        }
    }
}