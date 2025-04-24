using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class GameModel
    {
        public Player Player { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Level> Levels { get; private set; }

        public GameModel()
        {
            Player = new Player();
            Levels = new List<Level> { LevelCreator.CreateLevel(Player) };
            Enemies = Levels[0].GetNextStage();
        }

        public void Update()
        {
            Player.Update();
            if (Enemies == null) return;

            for (var i = 0; i < Enemies.Count; i++)
            {
                Enemies[i].Update();
                for (var j = 0; j < Player.Missiles.Count; j++)
                {
                    if (CheckCollisions(Player.Missiles[j], Enemies[i]))
                    {
                        Enemies[i].TakeDamage(Player.Missiles[j].Damage);
                        Player.Missiles.Remove(Player.Missiles[j]); 
                    }
                }

                for (var k = 0; k < Enemies[i].Missiles.Count; k++)
                {
                    if (CheckCollisions(Enemies[i].Missiles[k], Player))
                    {
                        Player.TakeDamage(Enemies[i].Missiles[k].Damage);
                        Enemies[i].Missiles.Remove(Enemies[i].Missiles[k]);
                    }
                }

                if (CheckCollisions(Enemies[i], Player))
                    Player.TakeDamage(1);

                if (Enemies[i].IsDeath()) 
                    Enemies.Remove(Enemies[i]);
            }

            if (Enemies.Count == 0)
                Enemies = Levels[0].GetNextStage();
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
