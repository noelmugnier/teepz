using MediatR;
using Teeps.Web.Api.Domain.Common;

namespace Teeps.Web.Api.Application.Mediatr;

public record BaseRequest(CurrentUser CurrentUser) : IRequest;
public record BaseRequest<T>(CurrentUser CurrentUser) : IRequest<T>;