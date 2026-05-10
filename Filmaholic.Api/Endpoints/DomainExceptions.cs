using Filmaholic.Shared.Dtos;
using Filmaholic.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Filmaholic.Api.Requests;

namespace Filmaholic.Api.Endpoints;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public sealed class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}