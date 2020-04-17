Imports DCS.mPosEngine.Services.Dto
Imports NUnit.Framework
Imports DCS.mPosEngine.Services

Namespace Services
    <TestFixture()> _
    Public Class TestDtoMapping

        <Test()>
        Public Sub NullCommandDto()
            Dim service As New PurchasingServices(Nothing)

            Dim command As CommitTransactionCommandDto

            Try
                service.CommitTransaction(command)
                Assert.Fail("Exception expected and never raised")
            Catch ex As NullCommandDtoException

            End Try
        End Sub

        <Test()>
        Public Sub EmptyCartCommitDto()
            Dim command As New CommitTransactionCommandDto
            Dim cart As New TransactionCartDto
            Dim service As New PurchasingServices(Nothing)

            'cart.LineItems.Add(New LineitemDto(1,1,1,1)            
            command.Cart = cart
            command.OperatorId = 1

            Try
                service.CommitTransaction(command)
                Assert.Fail("Exception expected and never raised")
            Catch ex As InvalidDtoException
                If ex.ReasonId <> 1 Then
                    Assert.Fail("Exception expected and never raised")
                End If
            End Try
        End Sub

        <Test()>
        Public Sub InvalidPaymodeId()
            Dim command As New CommitTransactionCommandDto
            Dim cart As New TransactionCartDto
            Dim service As New PurchasingServices(Nothing)

            command.Payitems.Add(New PayItemDto(55, 1, 0, "", "", "", "",0))
            cart.LineItems.Add(New LineitemDto(1, 1, 1, 1))
            command.Cart = cart
            command.OperatorId = 1

            Try
                service.CommitTransaction(command)
                Assert.Fail("Exception expected and never raised")
            Catch ex As InvalidDtoFieldException
                If ex.FieldName <> "paymodeId" Then
                    Assert.Fail("Exception expected and never raised")
                End If
            End Try


        End Sub
        Dim dtoMapper As New DtoMapper


    End Class
End Namespace
