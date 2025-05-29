using GameWinForm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;


namespace GameWinForm.Model
{
    public class Dasher : Enemy
    {
        public Vector2 AttackTrajectory { get; protected set; }
        protected Timer _attackTimer = new Timer { Interval = 800 };
        protected Timer _trajectoryAttackTimer = new Timer { Interval = 2000 };

        public Dasher(Player player, Vector2 position, Vector2[] trajectory, int hp) 
            : base(player, position, trajectory, hp)
        {
            AttackTrajectory = Vector2.Zero;
            _speed = 15f;
        }
        protected override void SetTimers()
        {
            _attackTimer.Tick += Attack;
            _trajectoryAttackTimer.Tick += ShowTrajectoryAttack;
            _trajectoryAttackTimer.Start();
        }

        protected void Attack(object sender, EventArgs e)
        {
            Trajectory = Array.Empty<Vector2>();
            _targetPosition = AttackTrajectory;
            AttackTrajectory = Vector2.Zero;
            _attackTimer.Stop();
        }

        protected void ShowTrajectoryAttack(object sender, EventArgs e)
        {
            AttackTrajectory = (_player.GetCenterPosition() - Position) * 1.5f + Position;
            _attackTimer.Start();
        }
    }
}
