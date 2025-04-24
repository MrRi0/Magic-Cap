using GameWinForm.Core;
using GameWinForm.Model;
using GameWinForm.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Controller
{
    public class MouseController
    {
        private readonly GameModel _model;
        private Point _mousePosition = new();

        private bool _isLeftButtonDown;

        public Vector2 MouseWorldPosition => ScreenToWorld(_mousePosition);

        public MouseController(GameModel model)
        {
            _model = model;
        }

        public void HandleMouseMove(MouseEventArgs e)
        {
            _mousePosition = e.Location;
        }

        public void HandleMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftButtonDown = true;
                ProcessInput();
            }
        }

        public void HandleMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isLeftButtonDown = false;
                ProcessInput();
            }
        }

        private void ProcessInput()
        {
            if(_isLeftButtonDown)
                _model.Player.Attack(MouseWorldPosition);
        }

        public Vector2 ScreenToWorld(Point screenPos)
        {
            return new Vector2(
                screenPos.X,
                screenPos.Y
            );
        }
    }
}
