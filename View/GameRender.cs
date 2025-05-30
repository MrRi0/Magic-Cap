using GameWinForm.Core;
using GameWinForm.Model;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameWinForm.View
{
    public class GameRender
    {
        private readonly GameModel _model;

        private readonly Bitmap _playerMoveRightSpriteSheet;
        private readonly Bitmap _playerMoveLeftSpriteSheet;
        private readonly Bitmap _playerStayLeftSpriteSheet;
        private readonly Bitmap _playerStayRightSpriteSheet;
        private readonly Bitmap _shooterSpriteSheet;
        private readonly Bitmap _dasherSpriteSheet;
        private readonly Bitmap _bossSprite;

        private SpriteAnimation _currentPlayerAnimation;
        private SpriteAnimation _stayLeftAnimation;
        private SpriteAnimation _stayRightAnimation;
        private SpriteAnimation _moveRightAnimation;
        private SpriteAnimation _moveLeftAnimation;
        private SpriteAnimation _shooterAnimation;
        private SpriteAnimation _dasherAnimation;

        private readonly Bitmap _playerMissileSprite;
        private readonly Bitmap _enemyMissileSprite;
        private readonly Bitmap _heartSprite;

        private const int MoveAnimationSpeed = 6;
        private const int StayAnimationSpeed = 12;

        private Color attackColor = Color.FromArgb(200, Color.Red);

        public GameRender(GameModel model)
        { 

            _playerStayLeftSpriteSheet = GetSprite("SpriteSheetStayLeft.png");
            _playerStayRightSpriteSheet = GetSprite("SpriteSheetStayRight.png");
            _playerMoveLeftSpriteSheet = GetSprite("MoveLeftSpriteSheet.png");
            _playerMoveRightSpriteSheet = GetSprite("MoveRightSpriteSheet.png");
            _playerMissileSprite = GetSprite("playerMissle.png");
            _enemyMissileSprite = GetSprite("enemyMissile.png");
            _shooterSpriteSheet = GetSprite("shooterSheet.png");
            _dasherSpriteSheet = GetSprite("dasherSheet.png");
            _bossSprite = GetSprite("bossSprite.png");

            _model = model;

            _stayLeftAnimation = new SpriteAnimation(_playerStayLeftSpriteSheet, _model.Player.Width, _model.Player.Height, 9, 9, StayAnimationSpeed);
            _stayRightAnimation = new SpriteAnimation(_playerStayRightSpriteSheet, _model.Player.Width, _model.Player.Height, 9, 9, StayAnimationSpeed);
            _moveRightAnimation = new SpriteAnimation(_playerMoveRightSpriteSheet, _model.Player.Width, _model.Player.Height, 5, 5, MoveAnimationSpeed, 2);
            _moveLeftAnimation = new SpriteAnimation(_playerMoveLeftSpriteSheet, _model.Player.Width, _model.Player.Height, 5, 5, MoveAnimationSpeed, 2);
            _shooterAnimation = new SpriteAnimation(_shooterSpriteSheet, 75, 75, 6, 6, 8);
            _dasherAnimation = new SpriteAnimation(_dasherSpriteSheet, 75, 75, 6, 6, 8);

            _heartSprite = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(_heartSprite)) g.Clear(Color.Red);
        }

        private static Bitmap GetSprite(string fileName)
        {
            var fullPath = Path.Combine(
                Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
                "View", "Image", fileName
            );
            var result = new Bitmap(fullPath);
            result.MakeTransparent(Color.White);
            return result;
        }

        public void Render(Graphics graphics)
        {
            DrawPlayer(graphics);
            DrawEnemies(graphics);
            DrawBulletHellStage(graphics);
            DrawBoss(graphics);
        }

        private void DrawPlayer(Graphics graphics)
        {
            if (!_model.Player.IsDeath())
            {
                var player = _model.Player;
                if (player.Velocity == Vector2.Zero || player.LastMoveDirection == Vector2.Zero)
                {
                    if (player.LastDirectionView.X > 0)
                        _currentPlayerAnimation = _stayRightAnimation;
                    else
                        _currentPlayerAnimation = _stayLeftAnimation;
                    _moveRightAnimation.StartOver();
                    _moveLeftAnimation.StartOver();
                }
                else
                {
                    if (player.Velocity.X > 0)
                    {
                        _currentPlayerAnimation = _moveRightAnimation;
                        _moveLeftAnimation.StartOver();
                    }
                    if (player.Velocity.X < 0)
                    {
                        _currentPlayerAnimation = _moveLeftAnimation;
                        _moveRightAnimation.StartOver();
                    }
                }

                _currentPlayerAnimation.Update();
                _currentPlayerAnimation.Draw(graphics, (int)player.Position.X, (int)player.Position.Y);
                DrawPlayerMissile(graphics);
                DrawPlayerUI(graphics, player);
            }
        }

        private void DrawPlayerUI(Graphics graphics, Player player)
        {
            for (int i = 0; i < player.HP; i++)
                graphics.DrawImage(_heartSprite, new Point(20 + _heartSprite.Width * 3 / 2 * i, 20));

            graphics.DrawRectangle(new Pen(Color.Red), 20, 20 + _heartSprite.Height + 10, 201, 31);
            if (player.CurrentSkill != Skills.NoSkill)
                graphics.DrawLine(new Pen(Color.Purple, 30), 21, 20 + _heartSprite.Height + 10 + 15,
                    20 + player.SkillCoolDown / (player.SkillCoolDownTime / 200), 20 + _heartSprite.Height + 10 + 15);

            if (player.IsShield)
                graphics.DrawEllipse(new Pen(Color.Purple, 3),
                    player.GetCenterPosition().X - player.Height * 3 / 4,
                    player.GetCenterPosition().Y - player.Height * 3 / 4,
                    player.Height * 3 / 2,
                    player.Height * 3 / 2);
        }

        private void DrawBoss(Graphics graphics)
        {
            var boss = _model.Boss;
            if (boss != null && !_model.Boss.IsDeath())
            {
                graphics.DrawImage(_bossSprite, boss.Position.X, boss.Position.Y);
                if (boss.AttackTrajectory != Vector2.Zero)
                {
                    var normalizedTrajectory = boss.AttackTrajectory.Normalize();
                    var widthLine = normalizedTrajectory.X * boss.Height + normalizedTrajectory.Y * boss.Width;
                    graphics.DrawLine(new Pen(attackColor, widthLine),
                        boss.GetCenterPosition().X,
                        boss.GetCenterPosition().Y,
                        boss.AttackTrajectory.X,
                        boss.AttackTrajectory.Y);
                }
            }
        }

        private void DrawPlayerMissile(Graphics graphics)
        {
            foreach (var missile in _model.Player.Missiles)
            {
                graphics.DrawImage(_playerMissileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _playerMissileSprite.Width,
                    _playerMissileSprite.Height);
            }   
        }

        private void DrawEnemies(Graphics graphics)
        {
            if (_model.Enemies == null) return;
            foreach (var enemy in _model.Enemies)
            {
                if (enemy is Shooter)
                {
                    var shooter = (Shooter)enemy;
                    _shooterAnimation.Update();
                    _shooterAnimation.Draw(graphics, (int)enemy.Position.X, (int)enemy.Position.Y);
                    if (((Shooter)enemy).AttackTrajectory != Vector2.Zero)
                    {
                        graphics.DrawLine(new Pen(attackColor, 2),
                            shooter.GetCenterPosition().X,
                            shooter.GetCenterPosition().Y,
                            shooter.AttackTrajectory.X,
                            shooter.AttackTrajectory.Y);
                    }
                }

                else if (enemy is Dasher)
                {
                    var dasher = (Dasher)enemy;
                    _dasherAnimation.Update();
                    _dasherAnimation.Draw(graphics, (int)enemy.Position.X, (int)enemy.Position.Y);
                    if (((Dasher)enemy).AttackTrajectory != Vector2.Zero)
                    {
                        var normalizedTrajectory = dasher.AttackTrajectory.Normalize();
                        var widthLine = normalizedTrajectory.X * dasher.Height + normalizedTrajectory.Y * dasher.Width;
                        graphics.DrawLine(new Pen(attackColor, widthLine),
                            dasher.GetCenterPosition().X,
                            dasher.GetCenterPosition().Y,
                            dasher.AttackTrajectory.X,
                            dasher.AttackTrajectory.Y);
                    }
                }

                else
                {
                    _shooterAnimation.Update();
                    _shooterAnimation.Draw(graphics, (int)enemy.Position.X, (int)enemy.Position.Y);
                }

                    foreach (var missile in enemy.Missiles)
                        DrawEnemyMissile(missile, graphics);

            }
        }

        private void DrawEnemyMissile(Missile missile, Graphics graphics)
        {
            graphics.DrawImage(_enemyMissileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _enemyMissileSprite.Width,
                    _enemyMissileSprite.Height);
        }

        private void DrawBulletHellStage(Graphics graphics)
        {
            foreach (var missile in _model.BulletHellStage)
            {
                if (missile.AttackTrajectory != Vector2.Zero)
                    graphics.DrawLine(new Pen(attackColor, 2),
                        missile.GetCenterPosition().X,
                        missile.GetCenterPosition().Y,
                        missile.AttackTrajectory.X,
                        missile.AttackTrajectory.Y);
                graphics.DrawImage(_enemyMissileSprite,
                    missile.Position.X,
                    missile.Position.Y,
                    _enemyMissileSprite.Width,
                    _enemyMissileSprite.Height);
            }
        }
    }
}
