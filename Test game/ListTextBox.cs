using System;
using SadConsole;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Test_game
{
    //A scrollable window which displays messsages
    // using a FIFO queue
    public class ListTextBox : Window
    {
        private static readonly int _maxLines = 100;

        //first line added is the first line removed 
        //when nuber of lines exceeds 100
        //stores the lines held in the message log
        private readonly Queue<string> _lines;

        //the messageConsole displays the active messages
        private SadConsole.ScrollingConsole _messageConsole;

        //scrollbar for message console
        private SadConsole.Controls.ScrollBar _messageScrollBar;

        //Track the current position of the scrollbar
        private int _scrollBarCurrentPosition;

        //this acounts for the thickness of the window border
        //to prevent the scorllbar from spilling over
        private int _windowBorderThickness = 2;

        //Create a new window with a centred title
        //And can be dragged around

        public ListTextBox(int width, int height, string title) : base(width, height)
        {
            Theme.WindowTheme.FillStyle.IsVisible = true;
            Theme.WindowTheme.FillStyle.Background = Color.Gray;
            CanDrag = true;
            Title = title.Align(HorizontalAlignment.Center, Width);

            //instatiating the queue
            _lines = new Queue<string>();
            //Setting its width to fit in the window
            _messageConsole = new SadConsole.ScrollingConsole(width - _windowBorderThickness, _maxLines);
            //Setting its postion the center the console in the window
            _messageConsole.Position = new Point(1, 1);
            //the console is the length of 100 lines
            //but the viewport of the console
            //fits inside the window
            //and is moved by a scrolllbar
            _messageConsole.ViewPort = new Rectangle(0, 0, width - 1, height - _windowBorderThickness);

            //creates a scrollbar and attching it to an event handler
            //to handle the scrollbar being moved
            //then adding it to the window
            _messageScrollBar = new SadConsole.Controls.ScrollBar(SadConsole.Orientation.Vertical, height - _windowBorderThickness); // makes sure the scorllbar is contained in the window
            _messageScrollBar.Position = new Point(_messageConsole.Width + 1, _messageConsole.Position.X); // sets the postion on the right side of the message console
            _messageScrollBar.IsEnabled = false;
            _messageScrollBar.ValueChanged += MessageScrollBar_ValueChanged; // subscribing the Scroll bar value changed event to my ValueChanged handler
            Add(_messageScrollBar); // adds the scrollbar to the window
            //Allows mouse input
            UseMouse = true;

            //Adds the child console to the window
            Children.Add(_messageConsole);
        }

        

        //Custom Update method which allows for a vertical scrollbar
        //To be honest i dont understand most of the stuff here very well
        public override void Update(TimeSpan time)
        {
            base.Update(time);

            //Ensures that the scrollbar tracks the current postion of _messageConsole
            //By catching it when it moves up for the first time 
            if (_messageConsole.TimesShiftedUp != 0 | _messageConsole.Cursor.Position.Y >= _messageConsole.ViewPort.Height + _scrollBarCurrentPosition)
            {
                //enable the scrollbar once the messagelog has filled up with enough text to require scrolling
                _messageScrollBar.IsEnabled = true;

                // Make sure we've never scrolled the entire size of the buffer
                if (_scrollBarCurrentPosition < _messageConsole.Height - _messageConsole.ViewPort.Height)
                {
                    // Record how much we've scrolled to enable how far back the bar can see
                    //this is effectively a shortened if else statement
                    _scrollBarCurrentPosition += _messageConsole.TimesShiftedUp != 0 ? _messageConsole.TimesShiftedUp : 1;
                }
                // Determines the scrollbar's max vertical position
                _messageScrollBar.Maximum = _scrollBarCurrentPosition - _windowBorderThickness;

                // This will follow the cursor since we move the render area in the event.
                _messageScrollBar.Value = _scrollBarCurrentPosition;

                // Reset the shift amount.
                _messageConsole.TimesShiftedUp = 0;
            }

        }
        //add a line to the queue of messages
        public void Add(string message)
        {
            //adds the message to back of the queue
            _lines.Enqueue(message);
            //when execeeding the limit remove the message at front of queue
            //aka oldest message
            if (_lines.Count > _maxLines)
            {
                _lines.Dequeue();
            }
            //Move the cursor to the last line and print the message
            _messageConsole.Cursor.Position = new Point(1, _lines.Count);
            _messageConsole.Cursor.Print(message + "\n");

        }

        //Controls the position of the messagelog viewport
        //based on the scrollbar postion using an event handler
        //Every time the scrollbars value changes
        //update the viewport position
        private void MessageScrollBar_ValueChanged(object sender, EventArgs e)
        {
            _messageConsole.ViewPort = new Rectangle(0, _messageScrollBar.Value + _windowBorderThickness, _messageConsole.Width, _messageConsole.ViewPort.Height);
        }
    }
}
