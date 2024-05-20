﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace caro1
{
    public class Player
    {
        private string name;

        public string Name { get => name; set => name = value; }
        public Image Mark { get => mark; set => mark = value; }

        private Image mark;
        public Player(string name, Image mark) 
        {
            this.Name = name;
            this.Mark = mark;
        }
    }
    
}
