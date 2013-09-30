#ifndef __CALLBACKS_FETCH_GAMER_UUID_H__
#define __CALLBACKS_FETCH_GAMER_UUID_H__

#include <string>

class CallbacksFetchGamerUUID
{
public:

	void OnSuccess(std::string gamerUUID);

	void OnFailure(int errorCode, std::string errorMessage);

	void OnCancel();
};

#endif