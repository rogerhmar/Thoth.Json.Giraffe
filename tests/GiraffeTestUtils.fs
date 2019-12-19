module GiraffeTestUtils
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.DependencyInjection
open Giraffe

let configureApp webApp (app:IApplicationBuilder) =
  app.UseGiraffe webApp

let ConfigureServices (services : IServiceCollection) =
  services.AddGiraffe() |> ignore

let startGiraffe port webApp =
  let host = 
    WebHostBuilder()
      .UseKestrel()
      .Configure(Action<IApplicationBuilder> (configureApp webApp))
      .ConfigureServices(ConfigureServices)
      .UseUrls("http://0.0.0.0:" + port.ToString() + "/")
      .Build()
  host.Start()
  host :> IDisposable

let rec startOnFreePort port webApp =
  try
    port, startGiraffe port webApp
  with
  | :? IO.IOException ->
    startOnFreePort (port+1) webApp