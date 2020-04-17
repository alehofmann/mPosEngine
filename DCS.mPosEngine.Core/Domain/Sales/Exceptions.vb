Namespace Domain.Sales
    Public Class ProductNotReturnableException
        Inherits ApplicationException

        Private _productName As String
        Public ReadOnly Property ProductName() As String
            Get
                Return _productName
            End Get            
        End Property


        Public Sub New(ByVal productName As String)
            MyBase.New("Product [" & productName & "] is Not returnable")

            _productName = productName
        End Sub
    End Class
End Namespace
