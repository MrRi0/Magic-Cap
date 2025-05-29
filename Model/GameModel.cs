using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;


namespace GameWinForm.Model
{
    public class GameModel
    {
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public Boss Boss { get; private set; }
        public Level Level { get; private set; }
        public bool IsPause { get; set; }
        public bool IsSelectUpgrade { get; set; }
        public bool IsLevelComplete { get; private set; }
        public bool IsLose { get; private set; }
        public Upgrades[] RandomUpgrades { get; private set; }
        public Skills[] RandomSkills { get; private set; }

        public List<Missile> BulletHellStage { get; private set; }

        private Stage _stage;
        private BossStage _bossFight;
        private Timer _bossStageTimer = new Timer { Interval = 800 };

        public GameModel(int levelNumber)
        {
            Player = new Player();
            Level = LevelCreator.CreateLevel(Player, levelNumber);
            _stage = Level.GetNextStage();
            Enemies = _stage.Enemies;
            BulletHellStage = _stage.Missiles;
            _bossStageTimer.Tick += GoNextStage;
        }

        public void Update()
        {
            Player.Update();

            if (Player.IsDeath())
            { 
                IsLose = true;
                return;
            }

            if (Enemies == null) return;
            if (BulletHellStage == null) return;
            if (Boss != null)
            {
                Boss.Update();
                CheckBossCollisions(); 
            }

            for (var i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update();
                CheckPlayerMissileCollisions(Enemies[i]);
                CheckEnemyMissileCollisions(Enemies[i]);
                if (CheckCollisions(Enemies[i], Player))
                    Player.TakeDamage(Enemies[i].ContactDamage);
                if (Enemies[i].IsDeath())
                    Enemies.Remove(Enemies[i]);
            }
            CheckBulletHellCollisions();

            if (Enemies.Count == 0 && BulletHellStage.Count == 0 && _bossFight != null)
            {
                if (Boss.IsDeath())
                { 
                    IsLevelComplete = true;
                    return;
                }
                Boss.GetNextStage();
                _bossStageTimer.Start();
            }
            else if (_stage.IsEmpty())
            {
                if (Level.CurrentStageNumber == 3)
                    RandomSkills = GetRandomSkills();
                else
                    RandomUpgrades = GetRandomUpgrades();
            }
        }

        private void GoNextStage(object sender, EventArgs e)
        {
            if (_bossFight != null)
            {
                _stage = _bossFight.GetNextStage();
                Enemies = _stage.Enemies;
                BulletHellStage = _stage.Missiles;
            }
            else
            { 
                _stage = Level.GetNextStage();
                Enemies = _stage.Enemies;
                BulletHellStage = _stage.Missiles;
            }
            if (_stage.IsEmpty())
            {
                if (_bossFight != null)
                {
                    IsLevelComplete = true;
                    return;
                }
                _bossFight = Level.GetBossStage();
                Boss = _bossFight.Boss;
                Enemies = _stage.Enemies;
                BulletHellStage = _stage.Missiles;
            }
            _bossStageTimer.Stop();
        }

        public void UpgradePlayer(Upgrades upgrade)
        {
            Player.UpgradePlayer(upgrade);
            IsSelectUpgrade = false;
            GoNextStage(1, EventArgs.Empty);
        }

        public void SetPlayerSkill(Skills skill)
        {
            Player.SetSkill(skill);
            IsSelectUpgrade = false;
            GoNextStage(1, EventArgs.Empty);
        }

        private Upgrades[] GetRandomUpgrades()
        {
            IsSelectUpgrade = true;
            RandomSkills = Array.Empty<Skills>();
            var rnd = new Random();
            var posibleUpgrades = new List<Upgrades>();
            foreach (var (upgrade, count) in Player.SelectedUpgrades)
            {
                if ((upgrade == Upgrades.DashSpeed || upgrade == Upgrades.ShieldDuration || upgrade == Upgrades.SkillCoolDown)
                    && Player.CurrentSkill == Skills.NoSkill)
                    continue;

                else if (upgrade == Upgrades.DashSpeed && Player.CurrentSkill == Skills.Dash && count < 3)
                    posibleUpgrades.Add(upgrade);

                else if (upgrade == Upgrades.ShieldDuration && Player.CurrentSkill == Skills.Shield && count < 3)
                    posibleUpgrades.Add(upgrade);

                else if (count < 3 && upgrade != Upgrades.DashSpeed && upgrade != Upgrades.ShieldDuration)
                    posibleUpgrades.Add(upgrade);
            }
            var result = new Upgrades[3];
            for (int i = 0; i < 3; i++)
            {
                var randomIndex = rnd.Next(posibleUpgrades.Count);
                if (Player.SelectedUpgrades[posibleUpgrades[randomIndex]] < 3)
                    result[i] = posibleUpgrades[randomIndex];
                else
                    result[i] = Player.SelectedUpgrades.Where(x => x.Value > 0).MinBy(x => x.Value).Key;
            }
            return result;
        }

        private Skills[] GetRandomSkills()
        {
            IsSelectUpgrade = true;
            RandomUpgrades = Array.Empty<Upgrades>();
            var rnd = new Random();
            var posibleSkills = Enum.GetValues(typeof(Skills)).Cast<Skills>().ToArray();
            var result = new Skills[3];
            for (int i = 0; i < 3; i++)
            {
                var randomIndex = rnd.Next(posibleSkills.Length);
                result[i] = posibleSkills[randomIndex];
            }
            return result;
        }

        private void CheckBulletHellCollisions()
        {
            for (var i = 0; i < BulletHellStage.Count; i++)
            {
                BulletHellStage[i].Update();
                if (CheckCollisions(Player, BulletHellStage[i]))
                {
                    Player.TakeDamage(1);
                    BulletHellStage.Remove(BulletHellStage[i]);
                }
                else if (BulletHellStage[i].IsDeath())
                    BulletHellStage.Remove(BulletHellStage[i]);
            }
        }

        private void CheckEnemyMissileCollisions(Enemy enemy)
        {
            for (var k = 0; k < enemy.Missiles.Count; k++)
            {
                if (CheckCollisions(enemy.Missiles[k], Player))
                {
                    Player.TakeDamage(enemy.Missiles[k].Damage);
                    enemy.Missiles.Remove(enemy.Missiles[k]);
                }
                else if (enemy.Missiles[k].IsDeath())
                    enemy.Missiles.Remove(enemy.Missiles[k]);
            }
        }

        private void CheckPlayerMissileCollisions(Enemy enemy)
        {
            for (var j = 0; j < Player.Missiles.Count; j++)
            {
                if (CheckCollisions(Player.Missiles[j], enemy))
                {
                    enemy.TakeDamage(Player.Missiles[j].Damage);
                    Player.Missiles.Remove(Player.Missiles[j]);
                }
                else if (Player.Missiles[j].IsDeath())
                    Player.Missiles.Remove(Player.Missiles[j]);
            }
        }

        private void CheckBossCollisions()
        {
            CheckPlayerMissileCollisions(Boss);
            if (CheckCollisions(Player, Boss))
                Player.TakeDamage(Boss.ContactDamage);
        }

        private bool CheckCollisions(Entity entity1, Entity entity2)
        {
            return (Math.Max(entity1.Position.X, entity2.Position.X) 
                <= Math.Min(entity1.Position.X + entity1.Width, entity2.Position.X + entity2.Width))
                && (Math.Max(entity1.Position.Y, entity2.Position.Y) 
                <= Math.Min(entity1.Position.Y + entity1.Height, entity2.Position.Y + entity2.Height));
        }
    }
}