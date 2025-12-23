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
        public List<string> GenerateDailyPlan(double totalHours, double completedHours, DateTime examDate)
        {
            var plan = new List<string>();
            int daysLeft = GetDaysRemaining(examDate);

            if (daysLeft <= 0)
            {
                plan.Add("L'esame è oggi! Ripassa i concetti chiave e riposati.");
                return plan;
            }

            // Calcoliamo quanto manca davvero
            double remainingHours = totalHours - completedHours;

            if (remainingHours <= 0)
            {
                plan.Add("Complimenti! Hai completato lo studio previsto per questo esame. 🎉");
            }
            else
            {
                double hoursPerDay = Math.Round(remainingHours / daysLeft, 1);
                plan.Add($"Ti mancano {remainingHours} ore. Devi studiare circa {hoursPerDay} ore al giorno per i prossimi {daysLeft} giorni.");
            }

            return plan;
        }
    }
}
