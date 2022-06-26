using Stronzo.PickToLight.Api.Borders.Shared;

namespace Stronzo.PickToLight.Api.Borders.Shared.Helpers;

public interface IUseCase<TRequest, TResponse>
{
    Task<UseCaseResponse<TResponse>> Execute(TRequest request);
}
public interface IUseCaseWithRequest<TRequest>
{
    Task Execute(TRequest request);
}
public interface IUseCaseWithResponse<TResponse>
{
    Task<UseCaseResponse<TResponse>> Execute();
}

public interface IUseCase
{
    Task Execute();
}