using CovidSimulator.Extensions.Data;

namespace CovidTestingLabApi;

public record TestCovidPatientCommand(string Name, string NhsNumber, int numberOfVaccinations) 
: CovidPatient(Name, NhsNumber, numberOfVaccinations);