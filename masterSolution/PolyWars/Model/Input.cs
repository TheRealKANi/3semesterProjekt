﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PolyWars.Logic
{
    //Binds controls to user defined keys 
    class Input
    {
        private Dictionary<Key, Action> keyBindings = new Dictionary<Key, Action>();
        public Input()
        {
            
        }

        public bool initInput()
        {
            bool result = false;
            keyBindings[Key.W] = new Action(MoveUp);
            keyBindings[Key.A] = new Action(MoveLeft);
            keyBindings[Key.S] = new Action(MoveDown);
            keyBindings[Key.D] = new Action(MoveRight);
            keyBindings[Key.Up] = new Action(MoveUp);
            keyBindings[Key.Left] = new Action(MoveLeft);
            keyBindings[Key.Down] = new Action(MoveDown);
            keyBindings[Key.Right] = new Action(MoveRight);

            // TODO  Make verification logic
            if(keyBindings.Count > 0)
            {
                result = true;
            }
            return result;
        }

        private void MoveUp()
        {
            throw new NotImplementedException();
        }

        private void MoveRight()
        {
            throw new NotImplementedException();
        }

        private void MoveDown()
        {
            throw new NotImplementedException();
        }

        private void MoveLeft()
        {
            throw new NotImplementedException();
        }

        public void onKeyPressed(object sender, KeyEventArgs e)
        {
            try
            {
                keyBindings[e.Key].Invoke();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}

