using CovidSimulator.Extensions.Data;

namespace CovidTestingLabApi;

public record TestCovidPatientCommand(string Name, string NhsNumber, bool vaccinated) 
: CovidPatient(Name, NhsNumber, vaccinated);