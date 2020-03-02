using System;
using SadConsole;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Test_game
{
    public class SavedMapsWindow : Window
    {
        private const int _maxLines = 20;
        private Queue<string> _lines;
        private ScrollingConsole SavedMapsConsole;
        private SadConsole.Controls.ScrollBar _scrollbar;
        private int _scrollbarCurrentPosition;
        private const int _windowBorderThickness = 2;

        public SavedMapsWindow(int width, int height, string title) : base(width,height)
        {
            UseMouse = true;
            IsFocused = true;
            this.DefaultBackground = Color.Navy;
            this.DefaultForeground = Color.NavajoWhite;
            this.Title = title.Align(HorizontalAlignment.Center, Width);
            _lines = new Queue<string>();

            SavedMapsConsole = new ScrollingConsole(width - _windowBorderThickness, _maxLines)
            {
                Position = new Point(1, 1),
                Parent = this,
                ViewPort = new Rectangle(0, 0, width - 1, height - _windowBorderThickness),                
            };

            _scrollbar = new SadConsole.Controls.ScrollBar(SadConsole.Orientation.Vertical, height - _windowBorderThickness)
            {
                Position = new Point(SavedMapsConsole.Width, SavedMapsConsole.Position.X),
                IsEnabled = false,
                IsVisible = true,
                Parent = this,
                UseMouse = true,
                IsFocused = true
            };

            //Event handler
            _scrollbar.ValueChanged += ScrollBarValueChanged;

           
           Add(_scrollbar);
        }

        public override void Update(TimeSpan time)
        {
            base.Update(time);

            if(SavedMapsConsole.TimesShiftedUp != 0 | SavedMapsConsole.Position.Y >= SavedMapsConsole.ViewPort.Height + _scrollbarCurrentPosition)
            {
                _scrollbar.IsEnabled = true;

                if(_scrollbarCurrentPosition < SavedMapsConsole.Height - SavedMapsConsole.Height)
                {
                    if(SavedMapsConsole.TimesShiftedUp !=0)
                    {
                        _scrollbarCurrentPosition += SavedMapsConsole.TimesShiftedUp;
                    }
                    else
                    {
                        _scrollbarCurrentPosition += 1;
                    }
                }

                _scrollbar.Maximum = _scrollbarCurrentPosition - _windowBorderThickness;
                _scrollbar.Value = _scrollbarCurrentPosition;

                //SavedMapsConsole.TimesShiftedUp = 0;
            }
        }

        public void Add(string message)
        {
            _lines.Enqueue(message);

            if(_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
            
            SavedMapsConsole.Cursor.Position = new Point(1, _lines.Count);         
            SavedMapsConsole.Cursor.Print(message + "\n");
        }


        private void ScrollBarValueChanged(object sender, EventArgs e)
        {
            SavedMapsConsole.ViewPort = new Rectangle(0, _scrollbar.Value + _windowBorderThickness, SavedMapsConsole.Width, SavedMapsConsole.ViewPort.Height);
        }
    }
}
