#ifndef __CALLBACKS_REQUEST_PRODUCTS_H__
#define __CALLBACKS_REQUEST_PRODUCTS_H__

class CallbacksRequestProducts
{
public:

	void OnSuccess();

	void OnFailure(int errorCode, char* errorMessage);

	void OnCancel();
};

#endif