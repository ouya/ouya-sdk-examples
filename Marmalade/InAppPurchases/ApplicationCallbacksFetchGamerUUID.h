#ifndef __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__
#define __APPLICATION_CALLBACKS_FETCH_GAMER_UUID_H__

#include <string>

class ApplicationCallbacksFetchGamerUUID
{
public:
	
	s3eCallback GetSuccessEvent();

	s3eCallback GetFailureEvent();

	s3eCallback GetCancelEvent();

	void OnSuccess(const std::string& gamerUUID);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();
};

#endif