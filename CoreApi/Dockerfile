# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir -p /app/Images

# Copy build artifacts to the final image
COPY publish/ .

ENTRYPOINT ["dotnet", "CoreApi.dll"]