namespace QuadrantGTD.Models;

public enum Quadrant
{
    UrgentImportant = 1,      // Q1: 重要且紧急 - Do First
    NotUrgentImportant = 2,   // Q2: 重要不紧急 - Schedule
    UrgentNotImportant = 3,   // Q3: 不重要但紧急 - Delegate
    NotUrgentNotImportant = 4 // Q4: 不重要不紧急 - Eliminate
}