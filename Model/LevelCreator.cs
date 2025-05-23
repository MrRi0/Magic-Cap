using Accessibility;
using GameWinForm.Core;
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

        private static List<Missile> GetGridBulletHellStage(int countVerticalBullet, int countHorizontalBullet)
        {
            var result = new List<Missile>();
            for (int i = 0; i < countVerticalBullet; i++)
            {
                result.Add(
                    new Missile(new Vector2(_windowWidth / (countVerticalBullet + 1) * (i + 1), -10), 
                    new Vector2(_windowWidth / (countVerticalBullet + 1) * (i + 1), _windowHeight), 
                    1));
                if (i < countHorizontalBullet)
                {
                    result.Add(
                    new Missile(new Vector2(-10, _windowHeight / (countHorizontalBullet + 1) * (i + 1)),
                    new Vector2(_windowWidth, _windowHeight / (countHorizontalBullet + 1) * (i + 1)),
                    1));
                }
            }
            return result;
        }

        private static List<Missile> GetDiagonalGridBulletHellStage(int countTopBullet, int countDownBullet)
        {
            var result = new List<Missile>();
            var diagonalLeftTopToRightDown = new Vector2(_windowWidth, _windowHeight);
            var diagonalVectorRightTopToLeftDown = new Vector2(_windowWidth, -_windowHeight) / 2;
            var diagonalRightTopToLeftDown = new Vector2(_windowWidth, _windowHeight);
            var diagonalVectorLeftTopToRightDown = new Vector2(_windowWidth, -_windowHeight) / 2;
            for (int i = 0; i < countTopBullet; i++)
            {
                result.Add(
                    new Missile(diagonalLeftTopToRightDown.Normalize() * diagonalLeftTopToRightDown * (i + 1) / (countTopBullet + 1) + diagonalVectorRightTopToLeftDown,
                    diagonalLeftTopToRightDown.Normalize() * diagonalLeftTopToRightDown * (i + 1) / (countTopBullet + 1) - diagonalVectorRightTopToLeftDown, 1));
            }
            for (int i = 0; i < countDownBullet; i++)
            {
                result.Add(
                    new Missile(diagonalRightTopToLeftDown.Normalize() * diagonalRightTopToLeftDown * (i + 1) / (countDownBullet + 1) + diagonalVectorLeftTopToRightDown + new Vector2(_windowWidth, 0),
                    diagonalRightTopToLeftDown.Normalize() * diagonalRightTopToLeftDown * (i + 1) / (countDownBullet + 1) - diagonalVectorLeftTopToRightDown + new Vector2(_windowWidth, 0), 1));
            }
            return result;
        }

        private static Level CreateLevelNum1(Player player)
        {
            var stagesNum1 = new Queue<Stage>();
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Enemy(player, new Vector2(_windowWidth / 2, _windowHeight / 2 - 500), new Vector2[0], 1, 1000, 10), new Enemy(player, new Vector2(_windowWidth / 2 - 1000, _windowHeight / 2), new Vector2[0], 2000, 1, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Enemy(player, new Vector2(_windowWidth / 2, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), new List<Missile> { new Missile(new Vector2(-20, _windowHeight / 2), new Vector2(_windowWidth, _windowHeight / 2), 1) }));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Enemy>(player, 4, 50, 50, 10), new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Shooter(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            //stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3)));
            //stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(1, 1)));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Dasher(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 50, 50, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Dasher>(player, 2, 50, 50, 10), new List<Missile>()));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Shooter>(player, 6, 50, 50, 5), new List<Missile>()));

            var bossNum1 = new Boss(player, new Vector2(_windowWidth / 4, -200), new[] { new Vector2(_windowWidth / 4, 0) }, _windowWidth / 2, 200, 50);
            var bossStages = new List<Stage>();
            bossStages.Add(new Stage(new List<Enemy>
            {
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(-25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width / 2 - 25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width - 25, bossNum1.Height - 25), 50, 50, 10)
            }, new List<Missile>()));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(2, 2)));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(3, 3)));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(4, 4)));
            bossStages.Add(new Stage(new List<Enemy>
            {
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(-25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width / 2 - 25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width - 25, bossNum1.Height - 25), 50, 50, 10)
            }, new List<Missile>()));
            bossStages.Add(new Stage(GetEnemiesInCircle<Shooter>(player, 4, 75, 75, 5), new List<Missile>()));
            bossStages.Add(new Stage(new List<Enemy>
            {
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(-25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width / 2 - 25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width - 25, bossNum1.Height - 25), 50, 50, 10)
            }, new List<Missile>()));
            bossStages.Add(new Stage(GetEnemiesInCircle<Dasher>(player, 3, 75, 75, 5), new List<Missile>()));
            bossStages.Add(new Stage(new List<Enemy>
            {
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(-25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width / 2 - 25, bossNum1.Height - 25), 50, 50, 10),
                new Shooter(player, bossNum1, bossNum1.Position + new Vector2(bossNum1.Width - 25, bossNum1.Height - 25), 50, 50, 10)
            }, new List<Missile>()));
            var bossFightNum1 = new BossStage(
                bossNum1,
                bossStages);
            return new Level(1, stagesNum1, bossFightNum1);
        }
    }
}
