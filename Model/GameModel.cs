using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public Level Level { get; private set; }

        public List<Missile> BulletHellStage { get; private set; }

        private Stage _stage;
        private BossStage _bossFight;
        private Timer _stageTimer = new Timer { Interval = 1600 };

        public GameModel()
        {
            Player = new Player();
            Level = LevelCreator.CreateLevel(Player, 1);
            _stage = Level.GetNextStage();
            Enemies = _stage.Enemies;
            BulletHellStage = _stage.Missiles;
            _stageTimer.Tick += GoNextStage;
        }

        public void Update()
        {
            Player.Update();
            if (Enemies == null) return;
            if (BulletHellStage == null) return;
            if (_bossFight != null)
                _bossFight.Update();

            for (var i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update();
                CheckPlayerMissileCollisions(i);
                CheckEnemyMissileCollisions(i);
                if (CheckCollisions(Enemies[i], Player))
                    Player.TakeDamage(Enemies[i].ContactDamage);
                if (Enemies[i].IsDeath())
                    Enemies.Remove(Enemies[i]);
            }
            CheckBulletHellCollisions();

            if (_bossFight != null && _bossFight.StageIsSwitch)
            {
                _stage = _bossFight.CurrentStage;
                return;
            }

            if (Enemies.Count == 0 && BulletHellStage.Count == 0)
                _stageTimer.Start();
        }

        private void GoNextStage(object sender, EventArgs e)
        {
            _stage = Level.GetNextStage();
            if (_stage == null)
            {
                _bossFight = Level.GetBossStage();
                _stage = _bossFight.CurrentStage;
            }
            Enemies = _stage.Enemies;
            BulletHellStage = _stage.Missiles;
            _stageTimer.Stop();
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

        private void CheckEnemyMissileCollisions(int i)
        {
            for (var k = 0; k < Enemies[i].Missiles.Count; k++)
            {
                if (CheckCollisions(Enemies[i].Missiles[k], Player))
                {
                    Player.TakeDamage(Enemies[i].Missiles[k].Damage);
                    Enemies[i].Missiles.Remove(Enemies[i].Missiles[k]);
                }
                else if (Enemies[i].Missiles[k].IsDeath())
                    Enemies[i].Missiles.Remove(Enemies[i].Missiles[k]);
            }
        }

        private void CheckPlayerMissileCollisions(int i)
        {
            for (var j = 0; j < Player.Missiles.Count; j++)
            {
                if (CheckCollisions(Player.Missiles[j], Enemies[i]))
                {
                    Enemies[i].TakeDamage(Player.Missiles[j].Damage);
                    Player.Missiles.Remove(Player.Missiles[j]);
                }
                else if (Player.Missiles[j].IsDeath())
                    Player.Missiles.Remove(Player.Missiles[j]);
            }
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