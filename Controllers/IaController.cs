﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    class IAController
    {
        internal int attack(int atual_life)
        {
            if (atual_life == 0)
            {
                return atual_life;
            }
            return atual_life - 20;
        }
    }
}
