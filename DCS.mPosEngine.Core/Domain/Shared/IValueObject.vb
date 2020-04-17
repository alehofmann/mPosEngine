Namespace Domain.Shared
    Public Interface IValueObject(Of T)
        Function SameValueAs(ByVal candidate As T) As Boolean
    End Interface
End Namespace