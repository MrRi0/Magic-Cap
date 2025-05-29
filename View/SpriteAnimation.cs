using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.View
{
    public class SpriteAnimation
    {
        public int FrameWidth { get; private set; }
        public int FrameHeight { get; private set; }
        public int AnimationSpeed { get; private set; }

        private readonly Bitmap _spritesheet;
        private readonly int _totalFrames;
        private int _currentFrame;
        private readonly int _columns;
        private int _counter;
        private int _countCycledFrames;
        private bool _isCycled;

        public SpriteAnimation(Bitmap spritesheet, int frameWidth, int frameHeight,
                             int totalFrames, int columns, int speed, int countCycledFrames)
        {
            _spritesheet = spritesheet;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            _totalFrames = totalFrames;
            _columns = columns;
            AnimationSpeed = speed;
            _currentFrame = 0;
            _counter = 0;
            _isCycled = false;
            _countCycledFrames = countCycledFrames;
        }

        public SpriteAnimation(Bitmap spritesheet, int frameWidth, int frameHeight,
                             int totalFrames, int columns, int speed)
        {
            _spritesheet = spritesheet;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            _totalFrames = totalFrames;
            _columns = columns;
            AnimationSpeed = speed;
            _currentFrame = 0;
            _counter = 0;
            _isCycled = false;
            _countCycledFrames = -1;
        }

        public void Update()
        {
            _counter++;
            if (_counter < AnimationSpeed) return;

            _counter = 0;
            
            if (_currentFrame + 1 > _totalFrames - _countCycledFrames || _isCycled)
            {
                _currentFrame = (_currentFrame) % _countCycledFrames + 3;
                _isCycled = true;
            }
            else
                _currentFrame = (_currentFrame + 1) % _totalFrames;

        }

        public void Draw(Graphics g, int x, int y)
        {
            var row = _currentFrame / _columns;
            var column = _currentFrame % _columns;

            var sourceRect = new Rectangle(column * FrameWidth, row * FrameHeight,
                                         FrameWidth, FrameHeight);
            var destRect = new Rectangle(x, y, FrameWidth, FrameHeight);

            g.DrawImage(_spritesheet, destRect, sourceRect, GraphicsUnit.Pixel);
        }

        public void StartOver()
        {
            _currentFrame = 0;
            _isCycled = false;
        }
    }
}
