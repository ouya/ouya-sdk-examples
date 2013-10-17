#ifndef __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__
#define __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__

#include "CallbacksFetchGamerUUID.h"

class ApplicationCallbacksFetchGamerUUID : public CallbacksFetchGamerUUID
{
public:

	virtual void OnSuccess(const std::string& gamerUUID);

	virtual void OnFailure(int errorCode, const std::string& errorMessage);

	virtual void OnCancel();
};

#endif