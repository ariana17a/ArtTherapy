Imports Repository_DBFirst
Imports System.Diagnostics

Public Class PlanuriService
    Implements IPlanuriService

    Private ReadOnly _cache As ICache
    Private Const CACHE_KEY As String = "planuri_utilizare_all"

    Public Sub New(cache As ICache)
        _cache = cache
    End Sub

    Public Function GetAll() As IEnumerable(Of planuri_utilizare) Implements IPlanuriService.GetAll
        Try
            If _cache IsNot Nothing AndAlso _cache.IsSet(CACHE_KEY) Then
                Trace.WriteLine("CACHE HIT: " & CACHE_KEY)
                Dim cached = _cache.Get(Of List(Of planuri_utilizare))(CACHE_KEY)
                If cached IsNot Nothing Then
                    Return cached
                End If
            End If

            Trace.WriteLine("CACHE MISS: " & CACHE_KEY)
            Using ctx As New ArtTherapyEntities()
                ctx.Configuration.LazyLoadingEnabled = False
                ctx.Configuration.ProxyCreationEnabled = False

                Dim data = ctx.planuri_utilizare.ToList()

                If _cache IsNot Nothing Then
                    _cache.Set(CACHE_KEY, data)
                End If

                Return data
            End Using
        Catch ex As Exception
            Trace.WriteLine("Error in GetAll: " & ex.ToString())
            Throw
        End Try
    End Function

    Public Function Insert(p As planuri_utilizare) As planuri_utilizare Implements IPlanuriService.Insert
        Using ctx As New ArtTherapyEntities()
            Dim entity = New planuri_utilizare() With {
                .nume = p.nume,
                .descriere = p.descriere,
                .limita_lucrari = p.limita_lucrari,
                .emotii_extinse = p.emotii_extinse,
                .feedback_deblocabil = p.feedback_deblocabil,
                .sesiuni_terapie = p.sesiuni_terapie,
                .chat_securizat = p.chat_securizat
            }

            ctx.planuri_utilizare.Add(entity)
            ctx.SaveChanges()

            If _cache IsNot Nothing Then
                Trace.WriteLine("Invalidating cache: " & CACHE_KEY)
                _cache.Remove(CACHE_KEY)
            End If

            Return entity
        End Using
    End Function

    Public Function Delete(id As Integer) As Boolean Implements IPlanuriService.Delete
        Using ctx As New ArtTherapyEntities()
            Dim entity = ctx.planuri_utilizare.FirstOrDefault(Function(x) x.id = id)
            If entity Is Nothing Then Return False

            ctx.planuri_utilizare.Remove(entity)
            ctx.SaveChanges()

            If _cache IsNot Nothing Then
                Trace.WriteLine("Invalidating cache: " & CACHE_KEY)
                _cache.Remove(CACHE_KEY)
            End If

            Return True
        End Using
    End Function

End Class
