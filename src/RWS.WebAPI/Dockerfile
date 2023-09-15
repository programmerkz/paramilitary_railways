#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/RWS.WebAPI/RWS.WebAPI.csproj", "src/RWS.WebAPI/"]
COPY ["src/RWS.Domain/RWS.Domain.csproj", "src/RWS.Domain/"]
COPY ["src/RWS.Application/RWS.Application.csproj", "src/RWS.Application/"]
COPY ["src/RWS.Infrastructure.Persistence/RWS.Infrastructure.Persistence.csproj", "src/RWS.Infrastructure.Persistence/"]
RUN dotnet restore "src/RWS.WebAPI/RWS.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/RWS.WebAPI"
RUN dotnet build "RWS.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RWS.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:5000
ENTRYPOINT ["dotnet", "RWS.WebAPI.dll"]