Imports System.Web.Http
Imports NivelAccesDate_DBFirst

Namespace ArtTherapy_API.Controllers

    Public Class PlanUtilizareController
        Inherits ApiController

        ' GET api/PlanUtilizare
        <HttpGet>
        Public Function GetAll() As IHttpActionResult
            Dim acc As New PlanuriUtilizareAccessor()
            Dim planuri = acc.GetAll()
            Return Ok(planuri)
        End Function

    End Class

End Namespace
