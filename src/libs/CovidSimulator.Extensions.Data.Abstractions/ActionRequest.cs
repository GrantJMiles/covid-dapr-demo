namespace CovidSimulator.Extensions.Data; // {}

public record ActionRequest<T>(string id, DateTime CreatedAt, T data, DateTime? StartedAt = null, bool started = false, bool completed = false,  Exception? exception = null, DateTime? FinishedAt = null);