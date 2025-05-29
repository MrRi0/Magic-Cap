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

        private const int EnemySize = 75;
        public static Level CreateLevel(Player player, int number)
        {
            switch (number)
            {
                case 1:
                    return CreateLevelNum1(player);
            }
            return null;
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
                    new object[] { player, position, trajectories.Dequeue(), hp });
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

        private static List<Missile> GetGridBulletHellStage(int countVerticalBullet, int countHorizontalBullet, 
            int downHorizontalIndent = 0, int topHorizontalIndent = 0, int downVerticalIndent = 0, int topVerticalIndent = 0)
        {
            var result = new List<Missile>();
            for (int i = 0; i < countVerticalBullet; i++)
            {
                result.Add(
                    new Missile(new Vector2(_windowWidth / (countVerticalBullet + 1) * (i + 1) + topHorizontalIndent, -10), 
                    new Vector2(_windowWidth / (countVerticalBullet + 1) * (i + 1) + downHorizontalIndent, _windowHeight), 
                    1));
                if (i < countHorizontalBullet)
                {
                    result.Add(
                    new Missile(new Vector2(-10, _windowHeight / (countHorizontalBullet + 1) * (i + 1) + topVerticalIndent),
                    new Vector2(_windowWidth, _windowHeight / (countHorizontalBullet + 1) * (i + 1) + downVerticalIndent), 1));
                }
            }
            return result;
        }

        private static Stage GetBossWithGunsStage(Player player, Boss boss, int enemyWidth)
        {
            return new Stage(new List<Enemy>
            {
                new Shooter(player, boss, boss.Position + new Vector2(-enemyWidth / 2, boss.Height / 2 + enemyWidth / 2), 10),
                new Shooter(player, boss, boss.Position + new Vector2(boss.Width / 2 - enemyWidth / 2, boss.Height + enemyWidth / 2), 10),
                new Shooter(player, boss, boss.Position + new Vector2(boss.Width - enemyWidth / 2, boss.Height / 2 + enemyWidth / 2), 10)
            }, new List<Missile>());
        }

        private static Level CreateLevelNum1(Player player)
        {
            var stagesNum1 = new Queue<Stage>();
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Enemy(player, new Vector2(_windowWidth / 2, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), new List<Missile> { new Missile(new Vector2(-20, _windowHeight / 2), new Vector2(_windowWidth, _windowHeight / 2), 1) }));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Enemy>(player, 4, EnemySize, EnemySize, 5), new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Shooter(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy> { new Dasher(player, new Vector2(-100, _windowHeight / 2), new[] { new Vector2(_windowWidth / 4, _windowHeight / 2) }, 10) }, new List<Missile>()));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3)));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3, _windowWidth / 10, _windowWidth / 10)));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3, _windowWidth / 5)));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3, 0, 0, _windowHeight / 6)));
            stagesNum1.Enqueue(new Stage(new List<Enemy>(), GetGridBulletHellStage(5, 3, 0, 0, _windowHeight / 6, _windowHeight / 6)));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Dasher>(player, 2, EnemySize, EnemySize, 14), new List<Missile>()));
            stagesNum1.Enqueue(new Stage(GetEnemiesInCircle<Shooter>(player, 6, EnemySize, EnemySize, 12), new List<Missile>()));

            var bossNum1 = new Boss(player, new Vector2(_windowWidth / 4, -200), new[] { new Vector2(_windowWidth / 4, 0) }, 75);
            var bossStages = new List<Stage>();
            bossStages.Add(GetBossWithGunsStage(player, bossNum1, EnemySize));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(2, 2)));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(3, 3)));
            bossStages.Add(new Stage(new List<Enemy>(), GetGridBulletHellStage(4, 4)));
            bossStages.Add(GetBossWithGunsStage(player, bossNum1, EnemySize));
            bossStages.Add(new Stage(GetEnemiesInCircle<Shooter>(player, 4, EnemySize, EnemySize, 10), new List<Missile>()));
            bossStages.Add(GetBossWithGunsStage(player, bossNum1, EnemySize));
            bossStages.Add(new Stage(GetEnemiesInCircle<Dasher>(player, 3, EnemySize, EnemySize, 10), new List<Missile>()));
            bossStages.Add(GetBossWithGunsStage(player, bossNum1, EnemySize));
            var bossFightNum1 = new BossStage(
                bossNum1,
                bossStages);
            return new Level(1, stagesNum1, bossFightNum1);
        }
    }
}
