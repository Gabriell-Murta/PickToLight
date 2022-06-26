#nullable disable

using Stronzo.PickToLight.Api.Borders.Dtos;

namespace Stronzo.PickToLight.Api.Borders.Entities;

public record Product
{
    public Guid Produto_Uuid { get; init; }
    public string Nome { get; init; }
    public string Descricao { get; init; }
    public Guid Client_Uuid { get; init; }

    public ProductResponse ToProductResponse() =>
        new()
        {
            Id = Produto_Uuid,
            Name = Nome,
            Code = Descricao,
        };
}
