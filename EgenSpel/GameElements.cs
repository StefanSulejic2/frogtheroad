using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgenSpel
{
    static class GameElements
    {
        static Texture2D menuSprite;
        static Vector2 menuPos;
        static Player player;
        static List<Enemy> enemies;
        static List<Fly> fly;
        static Texture2D flySprite;
        static PrintText printText;

        public enum State { Menu, Run, HighScore, Quit};
        public static State currentState;
        //Sida 105
    }
}
