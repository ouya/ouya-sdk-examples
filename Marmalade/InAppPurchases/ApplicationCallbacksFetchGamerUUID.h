#ifndef __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__
#define __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__

#include <string>

class ApplicationCallbacksFetchGamerUUID
{
public:

	void OnSuccess(const std::string& gamerUUID);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif