﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilSpace.AddDem.ReliefProcessing
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            var demCalcForm = new PrepareDem(false);
            demCalcForm.ShowDialog();

        }
    }
}
