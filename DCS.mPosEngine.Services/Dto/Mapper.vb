Imports DCS.mPosEngine.Core.Domain
Imports DCS.PlaycardBase.Core.CardDomain
Imports DCS.mPosEngine.Core.Domain.Sales

Namespace Dto
    Public Class Mapper
        Shared Function GetProductDto(ByVal product As Product) As ProductDto
            Dim retVal As ProductDto

            retVal = New ProductDto(product.Id, product.ProductData.Name, product.ProductData.Price, product.NeedsQuantity, product.ThumbUrl, product.ProductData.Price)

            Return retVal
        End Function

        Shared Function GetPaymodeDto(ByVal currency As PlaycardBase.Core.PosDomain.Currency) As PaymodeDto
            Dim retVal As New PaymodeDto

            retVal.Id = currency.Id
            retVal.Name = currency.Name
            retVal.Type = currency.PaymodeType

            Return retVal
        End Function

		Shared Function GetPaymodesDto(ByVal currencies As IList(Of DCS.PlaycardBase.Core.PosDomain.Currency)) As IList(Of PaymodeDto)
			Dim retVal As IList(Of PaymodeDto) = New List(Of PaymodeDto)

			For Each item As PlaycardBase.Core.PosDomain.Currency In currencies
				retVal.Add(GetPaymodeDto(item))
			Next

			Return retVal
		End Function

		Shared Function GetPosConfigDto(ByVal posConfig As IList(Of PosSetting)) As IList(Of PosConfigDto)
            Dim retVal As IList(Of PosConfigDto) = New List(Of PosConfigDto)
            Dim posConfigItem As PosConfigDto

            For Each item As PosSetting In posConfig
                posConfigItem = New PosConfigDto

                posConfigItem.Key = item.Key
                posConfigItem.Section = item.Section
                posConfigItem.Value = item.SettingValue

                retVal.Add(posConfigItem)
            Next

            Return retVal
        End Function

		Shared Function GetProductListDto(ByVal productList As IList(Of Product)) As IList(Of ProductDto)
			Dim retVal As New List(Of ProductDto)

			For Each item As Product In productList
				retVal.Add(GetProductDto(item))
			Next

			Return retVal
		End Function

		Shared Function GetCardHistoryDto(ByVal cardHistory As IList(Of CardHistoryLine)) As IList(Of CardHistoryLineDto)
            Dim lineDto As CardHistoryLineDto
			Dim retVal As IList(Of CardHistoryLineDto) = New List(Of CardHistoryLineDto)

			For Each line As CardHistoryLine In cardHistory.Where(Function(x) x IsNot Nothing)
				lineDto = New CardHistoryLineDto
				lineDto.Amount = line.amount
				lineDto.Counter = line.countername
				lineDto.Description = line.concept
				lineDto.Entity = line.opname
				lineDto.TransDate = line.transdate

				retVal.Add(lineDto)
			Next

			Return retVal
		End Function
    End Class
End Namespace