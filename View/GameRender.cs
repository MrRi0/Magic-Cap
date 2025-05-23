﻿using GameWinForm.Core;
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

        private readonly Bitmap _playerSprite;
        private readonly Bitmap _missileSprite;
        private readonly Bitmap _heartSprite;

        private Color attackColor = Color.FromArgb(150, Color.Red);

        public GameRender(GameModel model)
        {
            //_playerSprite = GetSprite("player2.png");
            _model = model;
            _playerSprite = new Bitmap(model.Player.Width, model.Player.Height);
            using (Graphics g = Graphics.FromImage(_playerSprite)) g.Clear(Color.Green);
            _missileSprite = new Bitmap(15, 15);
            using (Graphics g = Graphics.FromImage(_missileSprite)) g.Clear(Color.Black);
            _heartSprite = new Bitmap(30, 30);
            using (Graphics g = Graphics.FromImage(_heartSprite)) g.Clear(Color.Red);
        }

        private static Bitmap GetSprite(string fileName)
        {
            var fullPath = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
                "View", "Image", fileName);
            return new Bitmap(fullPath);
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
                graphics.DrawImage(_playerSprite,
                    player.Position.X,
                    player.Position.Y,
                    _playerSprite.Width,
                    _playerSprite.Height);
                DrawPlayerMissile(graphics);

                for (int i = 0; i < player.Hp; i++)
                    graphics.DrawImage(_heartSprite, new Point(20 + _heartSprite.Width * 3 / 2 * i, 20));

                graphics.DrawRectangle(new Pen(Color.Red), 20, 20 + _heartSprite.Height + 10, 200, 31);
                graphics.DrawLine(new Pen(Color.Cyan, 30), 21, 20 + _heartSprite.Height + 10 + 15 + 1,
                    20 + player.DashCoolDown / 10, 20 + _heartSprite.Height + 10 + 15 + 1);
            }
        }

        private void DrawBoss(Graphics graphics)
        {
            var boss = _model.Boss;
            if (boss != null && !_model.Boss.IsDeath())
            {
                graphics.DrawRectangle(new Pen(Color.Red), boss.Position.X, boss.Position.Y, boss.Width, boss.Height);
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
                    var normalizedTrajectory = dasher.AttackTrajectory.Normalize();
                    var widthLine = normalizedTrajectory.X * dasher.Height + normalizedTrajectory.Y * dasher.Width;
                    graphics.DrawLine(new Pen(attackColor, widthLine),
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
