Imports System.Collections.Generic
Imports Repository_DBFirst

Public Interface IPlanuriService
    Function GetAll() As IEnumerable(Of planuri_utilizare)
    Function Insert(p As planuri_utilizare) As planuri_utilizare
    Function Delete(id As Integer) As Boolean
End Interface
