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
        public static Level CreateLevel(Player player, int number)
        {
            foreach (var level in GetLevels(player, number))
                return level;
            return null;
        }

        private static IEnumerable<Level> GetLevels(Player player, int number)
        {
            yield return CreateLevelNum1(player);
        }

        private static List<Enemy> GetEnemiesInCircle<T>(Player player, int count, int width, int height, int hp) where T : Enemy
        {
            var centerAngle = (180 - (count - 2) * 180 / (double)count) * Math.PI / 180;
            var targetPositions = new Vector2[count];
            var startPositions = new Vector2[count];
            for (var i = 0; i < count; i++)
            {
                targetPositions[i] = new Vector2(
                        (float)((_windowWidth - width) / 2 + _windowWidth / 3 * Math.Cos(centerAngle + 2 * Math.PI * i / count + 3 * Math.PI / 2)),
                        (float)((_windowHeight - height) / 2 + _windowHeight / 3 * Math.Sin(centerAngle + 2 * Math.PI * i / count + 3 * Math.PI / 2)));
                startPositions[i] = (targetPositions[i] - new Vector2((_windowWidth - width) / 2, (_windowHeight - height) / 2)) * 2 + new Vector2((_windowWidth - width) / 2, (_windowHeight - height) / 2);
            }
            var trajectories = GetSwitchTrajectory(targetPositions);
            var enemies = new List<Enemy>();
            foreach (var position in startPositions)
            {
                Enemy enemy = (T)Activator.CreateInstance(typeof(T),
                    new object[] { player, position, trajectories.Dequeue(), width, height, hp });
                enemies.Add(enemy);
            }
            return enemies;
        }

        private static Queue<Vector2[]> GetSwitchTrajectory(Vector2[] targets)
        {
            var trajectories = new Queue<Vector2>();
            foreach (var target in targets)
                trajectories.Enqueue(target);
            var result = new Queue<Vector2[]>();

            for (int i = 0; i < trajectories.Count; i++)
            {
                result.Enqueue(trajectories.ToArray());
                var temp = trajectories.Dequeue();
                trajectories.Enqueue(temp);
            }
            return result;
        }

        private static List<Missile> GetBulletHellStage()
        {
            var result = new List<Missile>();
            for (int i = 0; i < 6; i++)
            {
                result.Add(
                    new Missile(new Vector2(_windowWidth / 7 * (i + 1), -10), 
                    new Vector2(_windowWidth / 7 * (i + 1), _windowHeight), 
                    1));
                result.Add(
                    new Missile(new Vector2(-10, _windowHeight / 7 * (i + 1)), 
                    new Vector2(_windowWidth, _windowHeight / 7 * (i + 1)), 
                    1));
            }
            return result;
        }

        private static Level CreateLevelNum1(Player player)
        {
            var stagesNum1 = new Queue<Stage>();
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Enemy(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(new List<Enemy>(), new List<Missile> { new Missile(new Vector2(-20, _windowHeight / 2), new Vector2(_windowWidth, _windowHeight / 2), 1) }));
            //stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Enemy>(player, 4, 50, 50, 10), new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(new List<Enemy> { new Shooter(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetBulletHellStage()));
            //stagesNum1.Enqueue(new Stage(new List<Enemy> { new Dasher(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Dasher>(player, 2, 50, 50, 10), new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Shooter>(player, 6, 50, 50, 5), new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Boss(player, new Vector2(_windowWidth / 4, -200), new[] { new Vector2(_windowWidth / 4, 0) }, _windowWidth / 2, 200, 15) }, new List<Missile>()));

            var bossNum1 = new Boss(player, new Vector2(_windowWidth / 4, -200), new[] { new Vector2(_windowWidth / 4, 0) }, _windowWidth / 2, 200, 15);
            var bossStages = new List<Stage>();
            bossStages.Add(new Stage(new List<Enemy>
            {
                new Enemy(player, bossNum1, bossNum1.Position + new Vector2(-25, bossNum1.Height - 25), 50, 50, 10),
                new Enemy(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width / 2 - 25, bossNum1.Height - 25), 50, 50, 10),
                new Enemy(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width - 25, bossNum1.Height - 25), 50, 50, 10)
            }, new List<Missile>()));

            var bossFightNum1 = new BossStage(
                new Boss(player, new Vector2(_windowWidth / 4, -200), new[] { new Vector2(_windowWidth / 4, 0) }, _windowWidth / 2, 200, 15),
                bossStages,
                new[] { new Vector2(0, -200), new Vector2(0, _windowHeight), new Vector2(_windowWidth / 2, _windowHeight), new Vector2(_windowWidth / 2, -200) });
            return new Level(1, stagesNum1, bossFightNum1);
        }
    }
}
