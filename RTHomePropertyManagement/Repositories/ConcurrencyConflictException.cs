namespace RTHomePropertyManagement.Repositories;

// Thrown by a repository when an update loses an optimistic-concurrency
// check (the row changed between read and write). Endpoints catch this and
// translate it into a 409 rather than letting DbUpdateConcurrencyException
// bubble up as an unhandled 500.
public class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException(string message) : base(message)
    {
    }
}
