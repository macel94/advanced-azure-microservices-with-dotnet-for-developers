#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WisdomPetMedicine.RescueQuery.API/WisdomPetMedicine.RescueQuery.API.csproj", "src/WisdomPetMedicine.RescueQuery.API/"]
RUN dotnet restore "src/WisdomPetMedicine.RescueQuery.API/WisdomPetMedicine.RescueQuery.API.csproj"
COPY . .
WORKDIR "/src/src/WisdomPetMedicine.RescueQuery.API"
RUN dotnet build "WisdomPetMedicine.RescueQuery.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WisdomPetMedicine.RescueQuery.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WisdomPetMedicine.RescueQuery.API.dll"]