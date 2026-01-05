Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http
Imports System.Web.Http.Cors

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)
        ' Web API configuration and services

        ' Enable CORS globally (permissive for development)
        Dim cors As New EnableCorsAttribute("*", "*", "*")
        config.EnableCors(cors)

        ' Remove XML formatter and keep JSON as default
        If config.Formatters.XmlFormatter IsNot Nothing Then
            config.Formatters.Remove(config.Formatters.XmlFormatter)
        End If

        ' Configure JSON serializer to ignore reference loops
        config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore

        ' Register a simple DI resolver for IPlanuriService
        config.DependencyResolver = New SimpleResolver()

        ' Web API routes
        config.MapHttpAttributeRoutes()

        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{id}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
    End Sub
End Module
