#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OfficesAPI.Web/OfficesAPI.Web.csproj", "OfficesAPI.Web/"]
COPY ["OfficesAPI.Application.Abstraction/OfficesAPI.Application.Abstraction.csproj", "OfficesAPI.Application.Abstraction/"]
COPY ["OfficesAPI.Application/OfficesAPI.Application.csproj", "OfficesAPI.Application/"]
COPY ["OfficesAPI.Domain/OfficesAPI.Domain.csproj", "OfficesAPI.Domain/"]
COPY ["OfficesAPI.Persistence/OfficesAPI.Persistence.csproj", "OfficesAPI.Persistence/"]
COPY ["OfficesAPI.Presentation/OfficesAPI.Presentation.csproj", "OfficesAPI.Presentation/"]
RUN dotnet restore "OfficesAPI.Web/OfficesAPI.Web.csproj"
COPY . .
WORKDIR "/src/OfficesAPI.Web"
RUN dotnet build "OfficesAPI.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OfficesAPI.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OfficesAPI.Web.dll"]