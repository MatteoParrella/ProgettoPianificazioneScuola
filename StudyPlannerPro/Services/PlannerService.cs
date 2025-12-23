using System;

namespace StudyPlannerPro.Services
{
    public class PlannerService
    {
        // Calcola i giorni rimasti tra oggi e l'esame
        public int GetDaysRemaining(DateTime examDate)
        {
            var remaining = examDate - DateTime.Now;
            return (int)Math.Max(0, Math.Ceiling(remaining.TotalDays));
        }

        // Calcola il "Peso" dello studio richiesto
        public double CalculateStudyLoad(int difficulty, int importance, int daysRemaining)
        {
            if (daysRemaining <= 0) return 100; // Troppo tardi!

            // Formula base: (Difficoltà * Importanza) / Giorni rimasti
            // Più è alto il numero, più è urgente studiare
            double baseLoad = (difficulty * importance) / (double)daysRemaining;
            return Math.Round(baseLoad, 2);
        }
    }
}
