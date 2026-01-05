Imports Repository_DBFirst

Public Class PlanuriService
    Implements IPlanuriService

    Public Function GetAll() As IEnumerable(Of planuri_utilizare) Implements IPlanuriService.GetAll
        Using ctx As New ArtTherapyEntities()
            ctx.Configuration.LazyLoadingEnabled = False
            ctx.Configuration.ProxyCreationEnabled = False
            Return ctx.planuri_utilizare.ToList()
        End Using
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
            Return entity
        End Using
    End Function

    Public Function Delete(id As Integer) As Boolean Implements IPlanuriService.Delete
        Using ctx As New ArtTherapyEntities()
            Dim entity = ctx.planuri_utilizare.FirstOrDefault(Function(x) x.id = id)
            If entity Is Nothing Then Return False
            ctx.planuri_utilizare.Remove(entity)
            ctx.SaveChanges()
            Return True
        End Using
    End Function
End Class
