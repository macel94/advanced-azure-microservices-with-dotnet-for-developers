#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["src/WisdomPetMedicine.Hospital.Api/WisdomPetMedicine.Hospital.Api.csproj", "src/WisdomPetMedicine.Hospital.Api/"]
COPY ["src/WisdomPetMedicine.Hospital.Domain/WisdomPetMedicine.Hospital.Domain.csproj", "src/WisdomPetMedicine.Hospital.Domain/"]
COPY ["src/WisdomPetMedicine.Common/WisdomPetMedicine.Common.csproj", "src/WisdomPetMedicine.Common/"]
COPY ["src/WisdomPetMedicine.Hospital.Infrastructure/WisdomPetMedicine.Hospital.Infrastructure.csproj", "src/WisdomPetMedicine.Hospital.Infrastructure/"]
RUN dotnet restore "src/WisdomPetMedicine.Hospital.Api/WisdomPetMedicine.Hospital.Api.csproj"

COPY . .
WORKDIR "/src/src/WisdomPetMedicine.Hospital.Api"
RUN dotnet build "WisdomPetMedicine.Hospital.Api.csproj" -c Release -o /app/build
RUN dotnet test -c Release

FROM build AS publish
RUN dotnet publish "WisdomPetMedicine.Hospital.Api.csproj" -c Release -o /app/publish

FROM base AS final
ENV ASPNETCORE_ENVIRONMENT Production

# Install cultures (same approach as Alpine SDK image)
RUN apk add bash icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS http://+:80
EXPOSE 80

# Trust cosmosdb cert
WORKDIR /src/scripts
COPY /src/scripts .
RUN sudo ./trust_cosmos_db_emulator_crt.sh

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WisdomPetMedicine.Hospital.Api.dll"]