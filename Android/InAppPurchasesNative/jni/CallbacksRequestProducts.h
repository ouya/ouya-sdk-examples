#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

class CallbacksRequestProducts
{
public:

	void OnSuccess(const char* jsonData);

	void OnFailure(int errorCode, const char* errorMessage);

	void OnCancel();
};

#endif