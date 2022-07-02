#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated6.0 AS base
WORKDIR /home/site/wwwroot
EXPOSE 80
COPY /src/WisdomPetMedicine.Hospital.Projector/cosmosdbemulator.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/WisdomPetMedicine.Hospital.Projector/WisdomPetMedicine.Hospital.Projector.csproj", "src/WisdomPetMedicine.Hospital.Projector/"]
COPY ["src/WisdomPetMedicine.Hospital.Domain/WisdomPetMedicine.Hospital.Domain.csproj", "src/WisdomPetMedicine.Hospital.Domain/"]
COPY ["src/WisdomPetMedicine.Common/WisdomPetMedicine.Common.csproj", "src/WisdomPetMedicine.Common/"]
COPY ["src/WisdomPetMedicine.Hospital.Infrastructure/WisdomPetMedicine.Hospital.Infrastructure.csproj", "src/WisdomPetMedicine.Hospital.Infrastructure/"]
RUN dotnet restore "src/WisdomPetMedicine.Hospital.Projector/WisdomPetMedicine.Hospital.Projector.csproj"

COPY . .
WORKDIR "/src/src/WisdomPetMedicine.Hospital.Projector"
RUN dotnet build "WisdomPetMedicine.Hospital.Projector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WisdomPetMedicine.Hospital.Projector.csproj" -c Release -o /app/publish
RUN dotnet test -c Release

FROM base AS final
WORKDIR /home/site/wwwroot
COPY --from=publish /app/publish .

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
