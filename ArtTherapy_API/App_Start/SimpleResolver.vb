Imports System
Imports System.Collections.Generic
Imports System.Web.Http.Dependencies
Imports ArtTherapy_API.Services

' Minimal IDependencyResolver implementation (no Unity needed)
Public Class SimpleResolver
    Implements IDependencyResolver

    Private Shared ReadOnly _cacheInstance As ICache = New MemoryCacheService()

    Public Shared ReadOnly Property CacheInstance As ICache
        Get
            Return _cacheInstance
        End Get
    End Property

    Public Function GetService(serviceType As Type) As Object Implements IDependencyScope.GetService
        If serviceType Is GetType(IPlanuriService) Then
            ' DI: PlanuriService primește cache-ul singleton
            Return New PlanuriService(_cacheInstance)
        End If

        If serviceType Is GetType(ICache) Then
            Return _cacheInstance
        End If

        Return Nothing
    End Function

    Public Function GetServices(serviceType As Type) As IEnumerable(Of Object) Implements IDependencyScope.GetServices
        Return New List(Of Object)()
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        ' no-op
    End Sub

    Public Function BeginScope() As IDependencyScope Implements IDependencyResolver.BeginScope
        Return Me
    End Function
End Class
