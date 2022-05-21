#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/WisdomPetMedicine.Pet.Api/WisdomPetMedicine.Pet.Api.csproj", "src/WisdomPetMedicine.Pet.Api/"]
COPY ["src/WisdomPetMedicine.Pet.Domain/WisdomPetMedicine.Pet.Domain.csproj", "src/WisdomPetMedicine.Pet.Domain/"]
COPY ["src/WisdomPetMedicine.Common/WisdomPetMedicine.Common.csproj", "src/WisdomPetMedicine.Common/"]
RUN dotnet restore "src/WisdomPetMedicine.Pet.Api/WisdomPetMedicine.Pet.Api.csproj"
COPY . .
WORKDIR "/src/src/WisdomPetMedicine.Pet.Api"
RUN dotnet build "WisdomPetMedicine.Pet.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WisdomPetMedicine.Pet.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WisdomPetMedicine.Pet.Api.dll"]