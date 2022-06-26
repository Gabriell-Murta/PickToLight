ARG ASPNET=mcr.microsoft.com/dotnet/aspnet
ARG VERSION=6.0

FROM $ASPNET:$VERSION AS runtime
WORKDIR /app

ARG PUBLISH_DIR
COPY $PUBLISH_DIR ./

EXPOSE 80

ENTRYPOINT ["dotnet", "Stronzo.PickToLight.Api.dll"]
