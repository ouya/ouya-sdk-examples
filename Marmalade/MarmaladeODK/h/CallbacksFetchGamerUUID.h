#ifndef __CALLBACKS_FETCH_GAMER_UUID_H__
#define __CALLBACKS_FETCH_GAMER_UUID_H__

#include "s3eEdk.h"
#include "s3eEdk_android.h"

#include <string>

class CallbacksFetchGamerUUID
{
public:

	void OnSuccess(const std::string& gamerUUID);

	void OnFailure(int errorCode, const std::string& errorMessage);

	void OnCancel();

	s3eCallback m_onSuccess;
	s3eCallback m_onFailure;
	s3eCallback m_onCancel;
};

#endif