#ifndef __APPLICATTON_CALLBACKS_REQUEST_PRODUCTS_H__
#define __APPLICATTON_CALLBACKS_REQUEST_PRODUCTS_H__

#include "ApplicationProduct.h"

#include <string>
#include <vector>

class ApplicationCallbacksRequestProducts
{
public:

	s3eCallback GetSuccessEvent();

	s3eCallback GetFailureEvent();

	s3eCallback GetCancelEvent();

	void OnSuccess(const std::vector<ApplicationProduct>& products);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif