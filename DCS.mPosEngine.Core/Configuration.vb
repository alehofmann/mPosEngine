Public Class Configuration
    Private Shared ReadOnly _items As IDictionary(Of String, String) = New Dictionary(Of String, String)

    Shared Sub SetItem(ByVal key As String, ByVal value As String)
        AddOrModify(key, value)
    End Sub

    Shared Sub SetItem(ByVal key As String, ByVal value As Integer)
        AddOrModify(key, CStr(value))
    End Sub

    Private Shared Sub AddOrModify(ByVal key As String, ByVal value As String)
        If _items.ContainsKey(key) Then
            _items(key) = value
        Else
            _items.Add(key, value)
        End If
    End Sub

    Shared Function GetString(ByVal key As String, Optional ByVal defValue As String = "") As String
        If key = "" Then
            Throw New ArgumentNullException("key", "key must not be an empty string")
        End If
        If _items.ContainsKey(key) Then
            Return _items(key)
        Else
            Return defValue
        End If
    End Function

    Shared Function GetInteger(ByVal key As String, Optional ByVal defValue As Integer = 0) As Integer
        Dim stringValue As String
        Dim retVal As Integer

        stringValue = GetString(key, CStr(defValue))

        If Integer.TryParse(stringValue, retVal) Then
            Return retVal
        Else
            Throw New InvalidOperationException("Config item with key [" & key & "] is [" & stringValue & ", which cannot be converted to an integer")
        End If
    End Function
End Class
