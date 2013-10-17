#ifndef __CALLBACKS_FETCH_GAMER_UUID_H__
#define __CALLBACKS_FETCH_GAMER_UUID_H__

#include <string>

class CallbacksFetchGamerUUID
{
public:

	virtual void OnSuccess(const std::string& gamerUUID) = 0;

	virtual void OnFailure(int errorCode, const std::string& errorMessage) = 0;

	virtual void OnCancel() = 0;
};

#endif