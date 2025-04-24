using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class GameRender
    {
        private readonly GameModel _model;

        private readonly Bitmap _playerSprite;
        private readonly Bitmap _missileSprite;

        public GameRender(GameModel model)
        {
            _model = model;
            _playerSprite = new Bitmap(@"C:\Users\www\Documents\Вузик\GameWinForm\View\player2.png");
            _missileSprite = new Bitmap(@"C:\Users\www\Documents\Вузик\GameWinForm\View\missile.png");
        }

        public void Render(Graphics graphics)
        {
            DrawPlayer(graphics);
            DrawEnemies(graphics);
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
                if (enemy.AttackTrajectory != Vector2.Zero)
                    graphics.DrawLine(new Pen(Color.Red), 
                        enemy.GetCenterPosition().X, enemy.GetCenterPosition().Y, enemy.AttackTrajectory.X, enemy.AttackTrajectory.Y);
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
    }
}
