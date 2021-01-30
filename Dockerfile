
FROM mcr.microsoft.com/dotnet/sdk:3.1

LABEL "author"="az6bcn@gmail.com"

WORKDIR /app

# Ports to expose in the conatiner when image built. Not published)
EXPOSE 5000
EXPOSE 5001

# Add env variables we want to use/ have available in the container
ENV DOTNET_USE_POLLING_FILE_WATCHER=1
# dotnet watch run only work with localhost type urls, to get it working in the container
# change the host url to the types below with the desired port here as an ENV and in launchsettings (dev) (localhost for 0.0.0.0 i.e 0.0.0.0 is the container's ip address bcos we'll map to the project through a voulme and run the project in the container, for this reason it needs the container's ip address and not localhost host of the host.)
#https://stackoverflow.com/questions/51188774/docker-dotnet-watch-run-error-unable-to-bind-to-https-localhost5000-on-the-i
ENV ASPNETCORE_URLS=http://0.0.0.0:5001;https://0.0.0.0:5005
ENV ASPNETCORE_ENVIRONMENT=Container

# cmd to execute in the container when it starts up, WILL BE PASSED TO ENTRYPOINT.
CMD ["/bin/bash", "-c", "dotnet restore && dotnet watch run --launch-profile Container"]

