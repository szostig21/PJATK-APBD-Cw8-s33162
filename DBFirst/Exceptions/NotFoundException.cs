using Microsoft.AspNetCore.Http.HttpResults;

namespace DBFirst.Exceptions;

public class NotFoundException(string msg) : Exception(msg)
{
}