#ifndef __CALLBACKS_FETCH_GAMER_UUID_H__
#define __CALLBACKS_FETCH_GAMER_UUID_H__

class CallbacksFetchGamerUUID
{
public:

	void OnSuccess(const char* gamerUUID);

	void OnFailure(int errorCode, const char* errorMessage);

	void OnCancel();
};

#endif