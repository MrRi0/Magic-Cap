using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace GameWinForm.Model
{
    public class Entity
    {
        public int HP { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }
        public Vector2 Position { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public List<Missile> Missiles { get; set; }
        public int ContactDamage { get; protected set; } = 1;

        private Timer _framesInvulnerability = new Timer { Interval = 400 };
        public bool IsInvulnerability = false;

        public Entity()
        {
            _framesInvulnerability.Tick += RemoveInvulnerability;
        }
        public void TakeDamage(int damage)
        {
            if (!IsInvulnerability)
            { 
                HP -= damage;
                IsInvulnerability = true;
                _framesInvulnerability.Start();
            }
        }

        public bool IsDeath()
        {
            return HP <= 0;
        }

        public Vector2 GetCenterPosition()
        {
            return new Vector2(Position.X + Width / 2, Position.Y + Height / 2);
        }

        private void RemoveInvulnerability(object sender, EventArgs e)
        {
            IsInvulnerability = false;
            _framesInvulnerability.Stop();
        }
    }
}
