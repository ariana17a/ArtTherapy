Imports System
Imports System.Collections.Generic
Imports System.Web.Http.Dependencies

' Minimal IDependencyResolver implementation to avoid requiring Unity package
Public Class SimpleResolver
    Implements IDependencyResolver

    Public Function GetService(serviceType As Type) As Object Implements IDependencyScope.GetService
        If serviceType Is GetType(IPlanuriService) Then
            Return New PlanuriService()
        End If

        ' fallback: return Nothing to let Web API create controllers by default
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
