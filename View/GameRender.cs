using GameWinForm.Model;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class GameRender
    {
        private readonly GameModel _model;

        private readonly Bitmap _playerSprite;
        private readonly Bitmap _missileSprite;

        private Color attackColor = Color.FromArgb(150, Color.Red);

        public GameRender(GameModel model)
        {
            _model = model;
            _playerSprite = new Bitmap(model.Player.Width, model.Player.Height);
            using (Graphics g = Graphics.FromImage(_playerSprite)) g.Clear(Color.Green);
            _missileSprite = new Bitmap(15, 15);
            using (Graphics g = Graphics.FromImage(_missileSprite)) g.Clear(Color.Black);
        }

        public void Render(Graphics graphics)
        {
            DrawPlayer(graphics);
            DrawEnemies(graphics);
            DrawBulletHellStage(graphics);
        }

        private void DrawPlayer(Graphics graphics)
        {
            if (!_model.Player.IsDeath())
                graphics.DrawImage(_playerSprite,
                    _model.Player.Position.X,
                    _model.Player.Position.Y,
                    _playerSprite.Width,
                    _playerSprite.Height);
                DrawPlayerMissile(graphics);
        }

        private void DrawPlayerMissile(Graphics graphics)
        {
            foreach (var missile in _model.Player.Missiles)
            {
                graphics.DrawImage(_missileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _missileSprite.Width,
                    _missileSprite.Height);
            }
        }

        private void DrawEnemies(Graphics graphics)
        {
            if (_model.Enemies == null) return;
            foreach (var enemy in _model.Enemies)
            {
                graphics.DrawRectangle(new Pen(Color.Red), enemy.Position.X, enemy.Position.Y, enemy.Width, enemy.Height);
                if (enemy is Shooter && ((Shooter)enemy).AttackTrajectory != Vector2.Zero)
                {
                    var shooter = (Shooter)enemy;
                    graphics.DrawLine(new Pen(attackColor),
                        shooter.GetCenterPosition().X, 
                        shooter.GetCenterPosition().Y, 
                        shooter.AttackTrajectory.X, 
                        shooter.AttackTrajectory.Y);
                }

                if (enemy is Dasher && ((Dasher)enemy).AttackTrajectory != Vector2.Zero)
                {
                    var dasher = (Dasher)enemy;
                    graphics.DrawLine(new Pen(attackColor, 50),
                        dasher.GetCenterPosition().X,
                        dasher.GetCenterPosition().Y,
                        dasher.AttackTrajectory.X,
                        dasher.AttackTrajectory.Y);
                }

                foreach (var missile in enemy.Missiles)
                    DrawEnemyMissile(missile, graphics);

            }
        }

        private void DrawEnemyMissile(Missile missile, Graphics graphics)
        {
            graphics.DrawImage(_missileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _missileSprite.Width,
                    _missileSprite.Height);
        }

        private void DrawBulletHellStage(Graphics graphics)
        {
            foreach (var missile in _model.BulletHellStage)
            {
                if (missile.AttackTrajectory != Vector2.Zero)
                    graphics.DrawLine(new Pen(attackColor),
                        missile.GetCenterPosition().X,
                        missile.GetCenterPosition().Y,
                        missile.AttackTrajectory.X,
                        missile.AttackTrajectory.Y);
                graphics.DrawImage(_missileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _missileSprite.Width,
                    _missileSprite.Height);
            }
        }
    }
}
