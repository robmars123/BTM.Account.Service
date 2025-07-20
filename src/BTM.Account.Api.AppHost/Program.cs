using Aspire.Hosting;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var cacheName = "cache"; // Or use a different name if needed

// Fetch Redis connection string from the configuration
var redisConnectionString = builder.Configuration["RedisSettings:ConnectionString"];

var cache = builder.AddRedis(cacheName, 6379);

builder.AddProject<Projects.BTM_Account_Api>("btm-account-api");

builder.AddProject<Projects.BTM_Account_MVC_UI>("btm-account-mvc-client");
//builder.AddProject<Projects.BTM_Caching_Redis>("btm-caching-redis");

builder.Build().Run();
