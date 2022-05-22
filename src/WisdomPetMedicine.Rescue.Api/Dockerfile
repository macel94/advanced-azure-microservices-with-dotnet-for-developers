#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/WisdomPetMedicine.Rescue.Api/WisdomPetMedicine.Rescue.Api.csproj", "src/WisdomPetMedicine.Rescue.Api/"]
COPY ["src/WisdomPetMedicine.Rescue.Domain/WisdomPetMedicine.Rescue.Domain.csproj", "src/WisdomPetMedicine.Rescue.Domain/"]
COPY ["src/WisdomPetMedicine.Common/WisdomPetMedicine.Common.csproj", "src/WisdomPetMedicine.Common/"]
RUN dotnet restore "src/WisdomPetMedicine.Rescue.Api/WisdomPetMedicine.Rescue.Api.csproj"

COPY . .
WORKDIR "/src/src/WisdomPetMedicine.Rescue.Api"
RUN dotnet build "WisdomPetMedicine.Rescue.Api.csproj" -c Release -o /app/build
RUN dotnet test -c Release

FROM build AS publish
RUN dotnet publish "WisdomPetMedicine.Rescue.Api.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WisdomPetMedicine.Rescue.Api.dll"]