using Microsoft.AspNetCore.Mvc;

namespace Befit.Models
{
    public class ExerciseStatsViewModel
    {
        public string ExerciseName { get; set; }
        public int TimesPerformed { get; set; }
        public int TotalReps { get; set; }
        public double AverageWeight { get; set; }
        public double MaxWeight { get; set; }
    }
}