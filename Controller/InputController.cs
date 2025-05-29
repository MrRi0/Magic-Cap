using GameWinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Controller
{
    public class InputController
    {
        private readonly GameModel _model;
        private readonly Dictionary<Keys, bool> _keyStates = new();

        public InputController(GameModel model)
        {
            _model = model;
        }

        public void HandleKeyDown(Keys key)
        {
            _keyStates[key] = true;
            if (!_model.IsPause && !_model.IsSelectUpgrade)
                ProcessInput();
        }

        public void HandleKeyUp(Keys key)
        {
            _keyStates[key] = false;
                ProcessInput();
        }

        private void ProcessInput()
        {
            var moveDirection = Vector2.Zero;
            if (_keyStates.GetValueOrDefault(Keys.A, false)) moveDirection.X -= 1;
            if (_keyStates.GetValueOrDefault(Keys.D, false)) moveDirection.X += 1;
            if (_keyStates.GetValueOrDefault(Keys.W, false)) moveDirection.Y -= 1;
            if (_keyStates.GetValueOrDefault(Keys.S, false)) moveDirection.Y += 1;
            if (_keyStates.GetValueOrDefault(Keys.Space, false) && _model.Player.CurrentSkill != Skills.NoSkill)
                _model.Player.UseSkill(moveDirection);
            if (_keyStates.GetValueOrDefault(Keys.Escape, false)) _model.IsPause = true;

            _model.Player.LastMoveDirection = moveDirection;
            if (moveDirection != Vector2.Zero && !_model.Player.IsDash)
                _model.Player.Move(moveDirection);
        }
    }
}
