# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution + restore
COPY *.sln ./
COPY WorkOrderDesk.Api/ WorkOrderDesk.Api/
COPY WorkOrderDesk.Application/ WorkOrderDesk.Application/
COPY WorkOrderDesk.Domain/ WorkOrderDesk.Domain/
COPY WorkOrderDesk.Infrastructure/ WorkOrderDesk.Infrastructure/

RUN dotnet restore

# Publish
RUN dotnet publish WorkOrderDesk.Api/WorkOrderDesk.Api.csproj -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /out .

# Render provides PORT env var
ENV ASPNETCORE_URLS=http://+:$PORT

ENTRYPOINT ["dotnet", "WorkOrderDesk.Api.dll"]