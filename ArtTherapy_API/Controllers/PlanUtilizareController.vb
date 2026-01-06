Imports System.Web.Http
Imports NivelAccesDate_DBFirst
Imports Repository_DBFirst
Imports System.Diagnostics
Imports System.Linq

Namespace ArtTherapy_API.Controllers

    Public Class PlanUtilizareController
        Inherits ApiController

        Private ReadOnly _service As IPlanuriService

        ' Parameterless constructor for Web API when DI is not configured
        Public Sub New()
            ' Create cache and service and assign to field
            Dim cache = New MemoryCacheService()
            _service = New PlanuriService(cache)
        End Sub

        ' Constructor for DI
        Public Sub New(service As IPlanuriService)
            _service = service
        End Sub

        ' GET api/PlanUtilizare
        <HttpGet>
        Public Function [Get]() As IHttpActionResult
            Trace.WriteLine("GET /api/PlanUtilizare called")

            Try
                Dim planuri = _service.GetAll()
                Trace.WriteLine($"GET returned {planuri.Count()} items")
                Return Ok(planuri)

            Catch ex As Exception
                Trace.WriteLine("ERROR in GET /api/PlanUtilizare")
                Trace.WriteLine(ex.ToString())
                Return InternalServerError(ex)
            End Try
        End Function

        ' POST api/PlanUtilizare
        <HttpPost>
        Public Function Post(<FromBody> ByVal p As planuri_utilizare) As IHttpActionResult
            Trace.WriteLine("POST /api/PlanUtilizare called")

            If p Is Nothing Then
                Trace.WriteLine("POST failed: body is null")
                Return BadRequest("Body is required")
            End If

            Try
                Dim inserted = _service.Insert(p)
                Trace.WriteLine($"POST created plan with id={inserted.id}")
                Return Created(Request.RequestUri, inserted)

            Catch ex As Exception
                Trace.WriteLine("ERROR in POST /api/PlanUtilizare")
                Trace.WriteLine(ex.ToString())
                Return InternalServerError(ex)
            End Try
        End Function

        ' DELETE api/PlanUtilizare/{id}
        <HttpDelete>
        Public Function Delete(ByVal id As Integer) As IHttpActionResult
            Trace.WriteLine($"DELETE /api/PlanUtilizare/{id} called")

            Try
                Dim deleted = _service.Delete(id)

                If deleted Then
                    Trace.WriteLine($"DELETE succeeded for id={id}")
                    Return StatusCode(System.Net.HttpStatusCode.NoContent)
                Else
                    Trace.WriteLine($"DELETE failed: id={id} not found")
                    Return NotFound()
                End If

            Catch ex As Exception
                Trace.WriteLine($"ERROR in DELETE /api/PlanUtilizare/{id}")
                Trace.WriteLine(ex.ToString())
                Return InternalServerError(ex)
            End Try
        End Function

    End Class

End Namespace
