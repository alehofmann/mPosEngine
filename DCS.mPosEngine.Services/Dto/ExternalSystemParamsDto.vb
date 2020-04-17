Imports System.Runtime.Serialization

Namespace Dto

    <CollectionDataContract(Name:="params", _
                        KeyName:="name", _
                        ValueName:="value")> _
    Public Class ExternalSystemParamsDto
        Inherits Dictionary(Of String, String)
    End Class

End Namespace