#include "CallbacksRequestProducts.h"
#include "CallbackSingleton.h"

#include "ODK_internal.h"
#include "s3eEdk.h"
#include "s3eEdk_android.h"
#include "IwDebug.h"

#include <stdio.h>

OuyaSDK::ExtensionProduct* CallbacksRequestProducts::m_products = NULL;

void CallbacksRequestProducts::RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType)
{
	if (*savedCallback)
	{
		//IwTrace(ODK, ("Unregistering Callback"));

		s3eEdkCallbacksUnRegister(
				S3E_EXT_ODK_HASH,
				S3E_ODK_CALLBACKS_MAX,
				callbackType,
				*savedCallback,
				NULL);
		*savedCallback = NULL;
	}

	*savedCallback = callback;

	//IwTrace(ODK, ("Registering Callback"));

	s3eEdkCallbacksRegister(
			S3E_EXT_ODK_HASH,
			S3E_ODK_CALLBACKS_MAX,
			callbackType,
			*savedCallback,
			NULL,
			S3E_FALSE);
}

void CallbacksRequestProducts::RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel)
{
	RegisterCallback(onSuccess, &m_onSuccess, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_SUCCESS);
	RegisterCallback(onFailure, &m_onFailure, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_FAILURE);
	RegisterCallback(onCancel, &m_onCancel, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_CANCEL);
}

void CopyStringProduct(std::string source, char** destination)
{
	const char* str = source.c_str();
	*destination = new char[strlen(str)];
	sprintf(*destination, "%s", str);
}

void CallbacksRequestProducts::OnSuccess(const std::vector<OuyaSDK::Product>& products)
{
	//IwTrace(ODK, ("OnSuccess"));

	if (m_products)
	{
		//IwTrace(ODK, ("Cleaning old products"));
		delete [] m_products;
		m_products = NULL;
	}

	if (products.size() > 0)
	{
		//IwTrace(ODK, ("Allocating products"));
		m_products = new OuyaSDK::ExtensionProduct[products.size()];
		//IwTrace(ODK, ("Copying products"));
		for (unsigned int index = 0; index < products.size(); ++index)
		{
			OuyaSDK::Product product = products[index];
			OuyaSDK::ExtensionProduct eProduct;
			eProduct.Init();
			CopyStringProduct(product.CurrencyCode, &eProduct.CurrencyCode);
			CopyStringProduct(product.Identifier, &eProduct.Identifier);
			CopyStringProduct(product.Name, &eProduct.Name);
			eProduct.ProductVersionToBundle = product.ProductVersionToBundle;
			eProduct.LocalPrice = product.LocalPrice;
			eProduct.PriceInCents = product.PriceInCents;
			m_products[index] = eProduct;
		}
	}
	else
	{
		IwTrace(ODK, ("No products to copy"));
	}

	s3eRequestProductsSuccessEvent event;
	event.m_products = m_products;
	event.m_productsLength = products.size();

	IwTrace(ODK, ("Invoking callback"));

	m_dataRequestProductsSuccessEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_SUCCESS, &m_dataRequestProductsSuccessEvent);
}

void CallbacksRequestProducts::OnFailure(int errorCode, const std::string& errorMessage)
{
	char buffer[256];
	sprintf(buffer, "OnFailure errorCode=%d errorMessage=%s", errorCode, errorMessage.c_str());
	IwTrace(ODK, (buffer));

	s3eRequestProductsFailureEvent event;
	event.m_errorCode = errorCode;
	sprintf(event.m_errorMessage, "%s", errorMessage.c_str());

	m_dataRequestProductsFailureEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_FAILURE, &m_dataRequestProductsFailureEvent);
}

void CallbacksRequestProducts::OnCancel()
{
	IwTrace(ODK, ("OnCancel"));

	s3eRequestProductsCancelEvent event;

	m_dataRequestProductsCancelEvent = event; //don't send a temp pointer
	s3eEdkCallbacksEnqueue(S3E_EXT_ODK_HASH, S3E_ODK_CALLBACKS_REQUEST_PRODUCTS_ON_CANCEL, &m_dataRequestProductsCancelEvent);
}