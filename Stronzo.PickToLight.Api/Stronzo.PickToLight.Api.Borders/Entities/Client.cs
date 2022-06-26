#nullable disable

namespace Stronzo.PickToLight.Api.Borders.Entities;

public record Client
{
    public Guid Cliente_Uuid { get; init; }
    public string Nome { get; init; }
    public string Cnpj { get; init; }
    public string Token_Cliente { get; init; }
}
