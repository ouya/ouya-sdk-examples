#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

#include <string>

class CallbacksRequestProducts
{
public:

	void OnSuccess(const std::string& jsonData);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif