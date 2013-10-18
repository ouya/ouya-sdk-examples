#ifndef __CALLBACKS_FETCH_GAMER_UUID_H__
#define __CALLBACKS_FETCH_GAMER_UUID_H__

#include "s3eTypes.h"

#include <string>

typedef struct s3eFetchGamerUuidSuccessEvent
{
	char m_gamerUUID[256];
} s3eFetchGamerUuidSuccessEvent;

typedef struct s3eFetchGamerUuidFailureEvent
{
	int m_errorCode;
	char m_errorMessage[1024];
} s3eFetchGamerUuidFailureEvent;

typedef struct s3eFetchGamerUuidCancelEvent
{
} s3eFetchGamerUuidCancelEvent;

class CallbacksFetchGamerUUID
{
public:

	void RegisterCallbacks(s3eCallback onSuccess, s3eCallback onFailure, s3eCallback onCancel);

	void OnSuccess(const std::string& gamerUUID);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

private:
	//extension callbacks
	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;

	//event data sent to callbacks can't be temporary
	s3eFetchGamerUuidSuccessEvent m_dataFetchGamerUuidSuccessEvent;
	s3eFetchGamerUuidFailureEvent m_dataFetchGamerUuidFailureEvent;
	s3eFetchGamerUuidCancelEvent m_dataFetchGamerUuidCancelEvent;

	void RegisterCallback(s3eCallback callback, s3eCallback* savedCallback, int callbackType);
};

#endif