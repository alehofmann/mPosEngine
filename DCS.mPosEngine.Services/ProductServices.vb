Imports DCS.mPosEngine.Data.Dao
Imports DCS.mPosEngine.Services.Dto
Imports DCS.ProjectBase.Data.NHibernateSessionMgmt
Imports DCS.mPosEngine.Core.Domain.Sales
Imports DCS.mPosEngine.Core.Domain.Pagesets

Public Class ProductServices
	'Dim _posServices As PosServices

	Private Shared _log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

	Public Sub New()
		'_posServices = New PosServices
	End Sub

	Public Function GetCardProduct(ByVal userCardNumber As Long) As ProductDto
		Dim productDao As ProductDao = (New NHibernateDaoFactory).GetProductDao
		Dim product As Product
        Dim dto As ProductDto
		Dim dtoMapper As New DtoMapper

		'Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
		'	product = productDao.FindByID(1)
		'	unitOfWork.Commit()
		'End Using

        product = productDao.FindByID(1)

        dto = dtoMapper.GetProductDtoFrom(product, userCardNumber)

        Return dto
	End Function
	Public Function GetProductPages(ByVal posId As String, ByVal userCardNumber As Long) As GetProductPagesResponseDto
		Dim productPageDao As ProductPageDao = (New NHibernateDaoFactory).GetProductPageDao
		Dim retVal As GetProductPagesResponseDto = New GetProductPagesResponseDto
		Dim productPages As IList(Of ProductPage)
		Dim dtoMapper As New DtoMapper
		Dim productDto As ProductDto
		Dim pageDto As ProductPageDto

		_log.Info("Processing GetProductPages (posID=" & posId & ", userCardNumber=" & userCardNumber & ")")

		'Get Card Product****************
		_log.Debug("Getting default card product")
		retVal.CardProduct = GetCardProduct(userCardNumber)
		'********************************

		_log.Debug("Getting product pages for posId " & posId & " from DB")

		Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
			productPages = productPageDao.GetPagesForPos(posId)

		    If productPages.Count > 0 Then
			    For Each page As ProductPage In productPages
				    pageDto = New ProductPageDto
				    pageDto.PageName = page.PageName
				    pageDto.DisplayOrder = page.Id

				    For Each product As Product In page.Products
					    productDto = dtoMapper.GetProductDtoFrom(product, userCardNumber)
					    pageDto.ProductList.Add(productDto)
				    Next
				    retVal.ProductPages.Add(pageDto)
			    Next
		    Else
			    pageDto = New ProductPageDto
			    pageDto.PageName = "DEFAULT"
			    pageDto.DisplayOrder = 0
			    pageDto.ProductList = GetProducts(userCardNumber, True)
			    retVal.ProductPages.Add(pageDto)
		    End If

            unitOfWork.Commit()
		End Using

		Return retVal
	End Function

    Public Function GetProducts(ByVal userCardNumber As Integer, Optional ByVal excludeCardProduct As Boolean = False) As IList(Of ProductDto)
		Dim productDao As ProductDao = (New NHibernateDaoFactory).GetProductDao
		Dim product As Product
		Dim retVal As IList(Of ProductDto) = New List(Of ProductDto)
		Dim productDto As ProductDto
		Dim products As IList(Of Product)
		Dim dtoMapper As New DtoMapper

		_log.Info("Processing GetProducts (userCardNumber=" & userCardNumber & ")")

		_log.Debug("Retrieving products from DB")
        'Using unitOfWork As IUnitOfWork = DCS.ProjectBase.Data.NHibernateSessionMgmt.UnitOfWork.Start
        '    products = productDao.FindAll
        '    unitOfWork.Commit()
        'End Using

        products = productDao.FindAll()

        _log.Debug(products.Count & " products found. Building response DTO")
		For Each product In products
			If product.Deleted Or product.ProductData.Deleted Then Continue For

			If product.Id <> 1 Or Not excludeCardProduct Then
				productDto = dtoMapper.GetProductDtoFrom(product, userCardNumber)
				retVal.Add(productDto)
			End If
		Next

		Return retVal
	End Function
End Class