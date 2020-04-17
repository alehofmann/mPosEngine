Namespace Domain.Sales.Fulfillment
    Public MustInherit Class InvalidOperationDataException
        Inherits ApplicationException


        Public Sub New(ByVal reason As String)
            MyBase.New(reason)
        End Sub

        Public Sub New()

        End Sub
    End Class
    Public Class CardNotSoldException
        Inherits InvalidOperationDataException

        Private _cardNumber As Long
        Public ReadOnly Property CardNumber() As Long
            Get
                Return _cardNumber
            End Get
            
        End Property


        Public Sub New(ByVal cardNumber As Long)
            MyBase.New("Card " & cardNumber & " is not sold. Cannot charge credits to an unsold card")

            _cardNumber = cardNumber
        End Sub

    End Class

    Public Class CardAlreadySoldException
        Inherits InvalidOperationDataException



        Public Sub New(ByVal cardNumber As Long)
            MyBase.New("Card " & cardNumber & " is already sold. Cannot sell an already sold card")
        End Sub

    End Class

    Public Class CannotAddMoreThanOneNewCardProductToLineitemException
        Inherits InvalidOperationDataException

        
        Public Sub New()
            MyBase.New("Maximum MPosOperation quantity for a new card product is 1")
        End Sub
    End Class

    Public Class NullCardAndCardNeededException
        Inherits InvalidOperationDataException
        Public Sub New()
            MyBase.New("Card needed for this type of operation")
        End Sub
    End Class
End Namespace
