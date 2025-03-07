﻿using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIOTApp
{
    public class LightPlugin
    {
        public bool IsOn { get; set; } = false;

        [KernelFunction]
        [Description("Gets the state of the light.")]
        public string GetState() => IsOn ? "on" : "off";

        [KernelFunction]
        [Description("Changes the state of the light.'")]
        public string ChangeState(bool newState)
        {
            this.IsOn = newState;
            var state = GetState();

            // Print the state to the console
            Console.WriteLine($"[Light is now {state}]");

            return state;
        }
    }
}
