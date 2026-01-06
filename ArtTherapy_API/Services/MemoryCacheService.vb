Imports System
Imports System.Linq
Imports System.Collections.Concurrent

Public Class MemoryCacheService
    Implements ICache

    Private Class CacheItem
        Public Property Value As Object
        Public Property Expiration As DateTime?
    End Class

    Private ReadOnly _store As ConcurrentDictionary(Of String, CacheItem)
    Private ReadOnly _defaultMinutes As Integer

    Public Sub New(Optional cacheTimeMinutes As Integer = 10)
        _defaultMinutes = cacheTimeMinutes
        _store = New ConcurrentDictionary(Of String, CacheItem)()
    End Sub

    Public Function [Get](Of T)(key As String) As T Implements ICache.Get
        Dim item As CacheItem = Nothing
        If _store.TryGetValue(key, item) Then
            If item Is Nothing Then Return Nothing
            If item.Expiration.HasValue AndAlso item.Expiration.Value < DateTime.UtcNow Then
                Dim removed As CacheItem = Nothing
                _store.TryRemove(key, removed)
                Return Nothing
            End If
            Return CType(item.Value, T)
        End If
        Return Nothing
    End Function

    Public Sub [Set](key As String, objectData As Object, Optional cacheTimeMinutes As Integer? = Nothing) Implements ICache.Set
        If objectData Is Nothing Then Return
        Dim minutes = If(cacheTimeMinutes.HasValue, cacheTimeMinutes.Value, _defaultMinutes)
        Dim exp As DateTime? = If(minutes > 0, DateTime.UtcNow.AddMinutes(minutes), CType(Nothing, DateTime?))
        Dim item = New CacheItem With {.Value = objectData, .Expiration = exp}
        _store.AddOrUpdate(key, item, Function(k, old) item)
    End Sub

    Public Function IsSet(key As String) As Boolean Implements ICache.IsSet
        Dim item As CacheItem = Nothing
        If _store.TryGetValue(key, item) Then
            If item Is Nothing Then Return False
            If item.Expiration.HasValue AndAlso item.Expiration.Value < DateTime.UtcNow Then
                Dim removed As CacheItem = Nothing
                _store.TryRemove(key, removed)
                Return False
            End If
            Return True
        End If
        Return False
    End Function

    Public Sub Remove(key As String) Implements ICache.Remove
        Dim removed As CacheItem = Nothing
        _store.TryRemove(key, removed)
    End Sub

    Public Sub Clear() Implements ICache.Clear
        _store.Clear()
    End Sub
End Class
