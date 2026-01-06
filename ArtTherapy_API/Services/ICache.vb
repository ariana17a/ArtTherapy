Public Interface ICache
    Function [Get](Of T)(key As String) As T
    Sub [Set](key As String, objectData As Object, Optional cacheTimeMinutes As Integer? = Nothing)
    Function IsSet(key As String) As Boolean
    Sub Remove(key As String)
    Sub Clear()
End Interface
