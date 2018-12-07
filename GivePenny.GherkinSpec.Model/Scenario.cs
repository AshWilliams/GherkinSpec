﻿using System.Collections.Generic;
using System.Linq;

namespace GivePenny.GherkinSpec.Model
{
    public class Scenario
    {
        public Scenario(string title, IEnumerable<IStep> steps, int startingLineNumber)
        {
            Title = title;
            StartingLineNumber = startingLineNumber;
            Steps = steps.ToList().AsReadOnly();
        }

        public string Title { get; }
        public int StartingLineNumber { get; }
        public IReadOnlyCollection<IStep> Steps { get; }
    }
}
