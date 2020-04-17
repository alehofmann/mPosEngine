Imports DCS.ProjectBase.Data
Imports DCS.mPosEngine.Core.Domain

Namespace DataInterfaces
    Public Interface ICreditCardTransactionDao
        Inherits IGenericDao(Of CreditCardTransaction, Integer)

        Overloads Function GetById(id As Integer) As CreditCardTransaction

        Overloads Function FindAll() As IList(of CreditCardTransaction)
    End Interface
end Namespace
