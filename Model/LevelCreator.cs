using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWinForm.Model
{
    public static class LevelCreator
    {
        private static int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private static int _windowWidth = Screen.PrimaryScreen.Bounds.Width;
        public static Level CreateLevel(Player player)
        {
            var stages = new Queue<List<Enemy>>();
            stages.Enqueue(GetEnemies(player));
            stages.Enqueue(GetEnemies(player));
            return new Level(1, stages);
        }

        private static List<Enemy> GetEnemies(Player player)
        {
            var trajectory = GetTrajectory();
            return new List<Enemy>
            {
                new Enemy(player, new Vector2(-100, _windowHeight / 2), trajectory[0], 50, 50, 10 ),
                new Enemy(player, new Vector2(_windowWidth / 2, -100), trajectory[1], 50, 50, 10 ),
                new Enemy(player, new Vector2(_windowWidth + 150, _windowHeight / 2), trajectory[2], 50, 50, 10 ),
                new Enemy(player, new Vector2(_windowWidth / 2, _windowHeight + 150), trajectory[3], 50, 50, 10 ),
            };
        }

        private static List<Vector2>[] GetTrajectory()
        {
            var result1 = new List<Vector2>
            {
                new Vector2(_windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, _windowHeight / 4),
                new Vector2(3 * _windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, 3 * _windowHeight / 4),
            };

            var result2 = new List<Vector2>
            {
                new Vector2(_windowWidth / 2, _windowHeight / 4),
                new Vector2(3 * _windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, 3 * _windowHeight / 4),
                new Vector2(_windowWidth / 4, _windowHeight / 2)
            };

            var result3 = new List<Vector2>
            {
                new Vector2(3 * _windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, 3 * _windowHeight / 4),
                new Vector2(_windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, _windowHeight / 4)
            };

            var result4 = new List<Vector2>
            {
                new Vector2(_windowWidth / 2, 3 * _windowHeight / 4),
                new Vector2(_windowWidth / 4, _windowHeight / 2),
                new Vector2(_windowWidth / 2, _windowHeight / 4),
                new Vector2(3 * _windowWidth / 4, _windowHeight / 2)
            };

            return new[] {result1, result2, result3, result4};
        }
    }
}
