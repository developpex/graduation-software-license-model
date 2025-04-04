using System;
using System.Data;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace LicenseService.ServiceTests;

public class ApiServiceTestBase : IDisposable
{
    private readonly string? _databaseName;
    private readonly string _connectionString;

    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public string DatabaseName => _databaseName ?? string.Empty;

    public ApiServiceTestBase()
    {
        _webApplicationFactory = new WebApplicationFactoryServiceTests();
        _connectionString = GetConnectionString();
        _databaseName = GetDatabaseName(_connectionString);
        if (SystemTextJsonSerializerConfig.Options.PropertyNameCaseInsensitive)
        {
            SystemTextJsonSerializerConfig.Options.PropertyNameCaseInsensitive = false;
            SystemTextJsonSerializerConfig.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        StartServer();
    }

    private void StartServer()
    {
        _ = _webApplicationFactory.Server;
    }

    public HttpClient CreateClient() => _webApplicationFactory.CreateClient();

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _webApplicationFactory.Dispose();
        NpgsqlConnection.ClearAllPools();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static string GetConnectionString()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
            .AddJsonFile("LicenseService.json")
            .AddJsonFile($"LicenseService.{Environments.Development}.json")
            .AddJsonFile("LicenseService.ServiceTests.json")
            .Build();
        return configuration.GetConnectionString("LicenseService");
    }

    private static string? GetDatabaseName(string connectionString) => new NpgsqlConnectionStringBuilder(connectionString).Database;

    private string CreateConnectionString(string? databaseName) =>
        new NpgsqlConnectionStringBuilder(_connectionString)
        {
            CommandTimeout = 10000,
            Pooling = false,
            Username = "postgres",
            Password = "postgres",
            Database = databaseName
        }.ConnectionString;

    private static void ExecuteCommand(string connectionString, string query)
    {
        using var connection = new NpgsqlConnection(connectionString);
        connection.Open();
        ExecuteCommand(connection, query);
        connection.Close();
    }

    private static void ExecuteCommand(IDbConnection connection, string query)
    {
        using var dbCommand = connection.CreateCommand();
        dbCommand.CommandText = query;
        dbCommand.ExecuteNonQuery();
    }

}

internal class WebApplicationFactoryServiceTests : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(Environments.Development)
            .ConfigureAppConfiguration(configuration => configuration.AddJsonFile("LicenseService.ServiceTests.json"));
    }
}
