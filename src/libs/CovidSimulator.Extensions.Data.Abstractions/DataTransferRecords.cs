namespace CovidSimulator.Extensions.Data;//{}

public record CovidPatient(string Name, string NhsNumber, bool vaccinated);

public record GovernmentAdvice(int CurrentDaysInIsolation, int TotalNumberOfAvailableVaccinations, LockdownLevel LockdownLevel);

public enum LockdownLevel
{
    AlertLevelOne = 1,
    AlertLevelTwo,
    AlertLevelThree,
    AlertLevelFour,
    AlertLevelFive,
}