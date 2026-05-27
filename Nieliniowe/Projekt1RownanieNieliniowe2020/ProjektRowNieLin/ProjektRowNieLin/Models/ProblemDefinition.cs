using BIBLIOTEKA_NR1.NieLiniowe;
using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace ProjektRowNieLin.Models
{
    public class ProblemDefinition
    {
        public string DisplayName { get; }
        public string Description { get; }

        // Human-readable variable labels shown in the UI
        public string Variable1Caption { get; }
        public string Variable2Caption { get; }

        // Compact captions used as DataGridView row headers (e.g. "x1 =", "p1 =")
        public string Variable1RowHeader { get; }
        public string Variable2RowHeader { get; }

        // Two equations F1(X)=0 and F2(X)=0 written for display
        public string Equation1Text { get; }
        public string Equation2Text { get; }

        // Default initial guess for X[1], X[2] (X[0] is unused by the solver)
        // Length must be 3 to match the solver's 1-based indexing style.
        public double[] DefaultInitialGuess { get; }

        // Nonlinear system delegate: F(X)
        public FunWektorWektor Function { get; }

        // Draws background curves for F1(X)=0 and F2(X)=0 into chart series [0] and [1]
        public Action<Chart> DrawCurves { get; }

        public ProblemDefinition(
            string displayName,
            string description,
            string variable1Caption,
            string variable2Caption,
            string variable1RowHeader,
            string variable2RowHeader,
            string equation1Text,
            string equation2Text,
            double[] defaultInitialGuess,
            FunWektorWektor function,
            Action<Chart> drawCurves)
        {
            DisplayName = displayName;
            Description = description;
            Variable1Caption = variable1Caption;
            Variable2Caption = variable2Caption;
            Variable1RowHeader = variable1RowHeader;
            Variable2RowHeader = variable2RowHeader;
            Equation1Text = equation1Text;
            Equation2Text = equation2Text;
            DefaultInitialGuess = (double[])defaultInitialGuess.Clone();
            Function = function;
            DrawCurves = drawCurves;
        }
    }
}

