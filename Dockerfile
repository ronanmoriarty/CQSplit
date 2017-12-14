FROM microsoft/dotnet-framework:4.7
WORKDIR /app
COPY . .
CMD dir
