
FROM mcr.microsoft.com/dotnet/sdk:3.1

LABEL "author"="az6bcn@gmail.com"

WORKDIR /app

# Copy csproj and restore as distinct layer
COPY *.csproj /IdentityServer/

WORKDIR /IdentityServer

# Run executes the command, in a new layer and creates a new image (commit) with the result
# The result image is used for the next step
RUN ["dotnet", "restore"]

# Ports to expose in the conatiner when image built.
EXPOSE 5000
EXPOSE 5001

# Add env variables we want to use/ have available in the container
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
ENV ASPNETCORE_URLS=https://+;http://+
ENV ASPNETCORE_ENVIRONMENT=Development

# Copy everything else
COPY . ./

# Build
RUN ["dotnet", "build"]

# Main Command: cmd to execute in the container when it starts up.
ENTRYPOINT ["dotnet", "watch", "run"]

